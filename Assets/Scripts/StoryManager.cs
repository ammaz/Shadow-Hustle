// Decompile from assembly: Assembly-CSharp.dll
using CodeStage.AntiCheat.ObscuredTypes;
using System;
using System.Collections.Generic;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
	public static StoryManager instance;

	public ObscuredInt level;

	public Queue<LevelStep> levelSteps;

	public LevelStep currentStep;

	private int stepCount;

	public bool introCompleted;

	public bool levelCompleted;

	public Npc miniboss;

	public int hitsTaken;

	protected void Awake()
	{
		StoryManager.instance = this;
		//EpicPrefs.Initialize();
	}

	protected void Start()
	{

        this.level = PlayerPrefs.GetInt("storyLevel", 0);

        if (this.level == 0)
		{
			Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, -10f);
		}
	}

	protected void Update()
	{
		this.UpdateCurrentStep();
		this.UpdateSpawns();
	}

	public void StartLevel()
	{
		this.hitsTaken = 0;
		if (SceneManager.instance.isBeltExamReady)
		{
			SceneManager.instance.isBeltExamReady = false;
            PlayerPrefsX.SetBool("BeltExam", false);
		}
		SceneManager.instance.inputStarted = false;
		this.introCompleted = false;
		this.levelCompleted = false;
		this.InstantiateSteps();
		this.StartNextStep();
		//AnalyticsManager.instance.PlayedBeltExam();
	}

	public void StartNextStep()
	{
		if (this.levelSteps.Count == 0)
		{
			if (!this.introCompleted)
			{
				this.IntroCompleted();
			}
		}
		else
		{
			this.stepCount++;
			this.currentStep = this.levelSteps.Dequeue();
			this.currentStep.Start();
		}
	}

	private void IntroCompleted()
	{
		SceneManager.instance.targetCameraZ = -7f;
		this.introCompleted = true;
		NpcManager.instance.killCount = 0;
		NpcManager.instance.lieutenantKillCount = 0;
		SceneManager.instance.inputStarted = true;
	}

	public void LevelCompleted()
	{
		this.levelCompleted = true;
		this.introCompleted = false;
		this.currentStep = null;
		this.level = ++this.level;
		if (this.level > 15)
		{
			this.level = this.hitsTaken;
		}
		StatManager.instance.stats[1].UpdateStat(this.level);
		PlayerPrefs.SetInt("storyLevel", this.level);
		ItemManager.instance.UpdateBelt();
		//if (this.level > 15)
		//{
		//	LeaderboardsManager.instance.PostShadowData();
		//}
		ScreenshotManager.instance.TakeScreenShot(true);

		SceneManager.instance.GameOver(true);
	}

	private void UpdateCurrentStep()
	{
		if (this.introCompleted || this.currentStep == null)
		{
			return;
		}
		if (this.currentStep.Update())
		{
			this.StartNextStep();
		}
	}

	public void StartFatality()
	{
		AudioManager.instance.StartFatalitySound();
		CharacterManager.instance.runTargetPos = this.miniboss.transform.position - new Vector3((float)this.miniboss.side, 0f, 0f);
		if (CharacterManager.instance.transform.position.x * (float)this.miniboss.side > CharacterManager.instance.runTargetPos.x * (float)this.miniboss.side)
		{
			CharacterManager.instance.transform.position = new Vector3(CharacterManager.instance.runTargetPos.x, CharacterManager.instance.runTargetPos.y, CharacterManager.instance.transform.position.z);
		}
		this.miniboss.weapon1Sprite.sprite = null;
		this.miniboss.weapon2Sprite.sprite = null;
		this.miniboss.weapon2hSprite.sprite = null;
		SceneManager.instance.gameStarted = false;
		SceneManager.instance.chaserObject.transform.localPosition = new Vector3((float)CharacterManager.instance.side, -1.5f, 0f);
		SceneManager.instance.targetCameraZ = -7.5f;
		CharacterManager.instance.spine.state.SetAnimation(0, ItemManager.instance.currentWeapon.category.id + "Fatality", false);
		this.miniboss.spine.state.SetAnimation(0,"FatalityGotHit", false);
	}

	public void EndFatality()
	{
		AudioManager.instance.EndFatalitySound();
		ItemManager.instance.ToggleTrails(false);
		this.miniboss.StartJustBlood();
	}

	public void AfterFatality()
	{
		SceneManager.instance.targetCameraZ = -9f;
		SceneManager.instance.chaserObject.transform.localPosition = Vector3.zero;
		this.miniboss.ChangeState(Npc.State.Dead);
		ItemManager.instance.GotItem(0, string.Empty, true, false);
	}

	private void UpdateSpawns()
	{
		if (!this.introCompleted || this.levelCompleted)
		{
			return;
		}
	}

	public void GameOver()
	{
		if (this.level > 15)
		{
			//if (this.level == int.Parse(LeaderboardsManager.instance.shadowWarriorData[4]))
			//{
			//	this.level = --this.level;
			//}
			if (this.hitsTaken > this.level)
			{
				this.level = this.hitsTaken;
				StatManager.instance.stats[1].UpdateStat(this.level);
				PlayerPrefs.SetInt("storyLevel", this.level);
				//LeaderboardsManager.instance.PostShadowData();
			}
		}
		//AnalyticsManager.instance.KilledByExaminer();
		this.introCompleted = false;
		this.currentStep = null;
	}

	private void InstantiateSteps()
	{
		this.stepCount = 0;
		this.levelSteps = new Queue<LevelStep>();
		this.levelSteps.Enqueue(new LevelStep_Wait(1f));
		this.levelSteps.Enqueue(new LevelStep_Animation(CharacterManager.instance.spine, string.Empty, true, 0f, false));
		this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, null, Vector3.zero, Vector3.zero));
		switch (this.level + 1)
		{
		case 1:
			this.levelSteps.Enqueue(new LevelStep_Tip("#Banzan", true, 0f, false));
			this.levelSteps.Enqueue(new LevelStep_SummonNpc(Color.black, -1, 0f, 1f, true, "fist2", "fullmetaljacket"));
			this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, 0f));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "FistRun", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_ObjectMovement(this.miniboss.gameObject, 0.5f, false));
			this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, this.miniboss.gameObject, Vector3.zero, Vector3.zero));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "FistIdle", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_Speech("Let's start the game", this.miniboss.head, 2.5f, false));
            this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "FistRun", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, -1f));
			this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, null, Vector3.zero, new Vector3(0f, -1.5f, 0f)));
			break;
		case 2:
			this.levelSteps.Enqueue(new LevelStep_Tip("#Joe Carman", true, 0f, false));
			this.levelSteps.Enqueue(new LevelStep_SummonNpc(Color.black, -1, 0f, 1f, true, "dagger4", "pennywise"));
			this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, 0f));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "DaggerRun", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_ObjectMovement(this.miniboss.gameObject, 0.5f, false));
			this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, this.miniboss.gameObject, Vector3.zero, Vector3.zero));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "DaggerIdle", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_Speech("Do you want to know why I use a Dagger?", this.miniboss.head, 0.5f, false));
//            this.levelSteps.Enqueue(new LevelStep_Speech("The gun is too fast.", this.miniboss.head, 0.5f, false));
//            this.levelSteps.Enqueue(new LevelStep_Speech("You can’t taste all the small emotions.", this.miniboss.head, 0.5f, false));
            this.levelSteps.Enqueue(new LevelStep_Speech(" In... You see, in the last moments of their lives, people showed you their true side.", this.miniboss.head, 1f, false));
            this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "DaggerRun", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, -1f));
			this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, null, Vector3.zero, new Vector3(0f, -1.5f, 0f)));
			break;
		case 3:
			this.levelSteps.Enqueue(new LevelStep_Tip("#Beer Guy", true, 0f, false));
			this.levelSteps.Enqueue(new LevelStep_SummonNpc(Color.black, -1, 0f, 1f, true, "dagger2", "beertime"));
			this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, 0f));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "DaggerRun", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_ObjectMovement(this.miniboss.gameObject, 0.5f, false));
			this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, this.miniboss.gameObject, Vector3.zero, Vector3.zero));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "DaggerIdle", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_Speech("Want... Hic... some beer? ...Hic ...", this.miniboss.head, 2.5f, false));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "DaggerRun", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, -1f));
			this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, null, Vector3.zero, new Vector3(0f, -1.5f, 0f)));
			break;
		case 4:
			this.levelSteps.Enqueue(new LevelStep_Tip("#Butcher", true, 0f, false));
			this.levelSteps.Enqueue(new LevelStep_SummonNpc(Color.black, -1, 0f, 1f, true, "axe5", "spikey"));
			this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, 0f));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "AxeRun", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_ObjectMovement(this.miniboss.gameObject, 0.5f, false));
			this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, this.miniboss.gameObject, Vector3.zero, Vector3.zero));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "AxeIdle", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_Speech("Come closer fresh meat!", this.miniboss.head, 2.5f, false));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "AxeRun", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, -1f));
			this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, null, Vector3.zero, new Vector3(0f, -1.5f, 0f)));
			break;
		case 5:
			this.levelSteps.Enqueue(new LevelStep_Tip("#Jako Tonokome", true, 0f, false));
			this.levelSteps.Enqueue(new LevelStep_SummonNpc(Color.black, -1, 0f, 1f, true, "sword1h2", "maskofthedead"));
			this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, 0f));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "Sword1hRun", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_ObjectMovement(this.miniboss.gameObject, 0.5f, false));
			this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, this.miniboss.gameObject, Vector3.zero, Vector3.zero));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "Sword1hIdle", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_Speech("Life could be wonderful if people would leave you alone.", this.miniboss.head, 1f, false));
//            this.levelSteps.Enqueue(new LevelStep_Speech("I won't hurt you.", this.miniboss.head, 0.5f, false));
//            this.levelSteps.Enqueue(new LevelStep_Speech("I just want to smash your head!", this.miniboss.head, 1f, false));
            this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "Sword1hRun", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, -1f));
			this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, null, Vector3.zero, new Vector3(0f, -1.5f, 0f)));
			break;
		case 6:
			this.levelSteps.Enqueue(new LevelStep_Tip("#Heisenberg", true, 0f, false));
			this.levelSteps.Enqueue(new LevelStep_SummonNpc(Color.black, -1, 0f, 1f, true, "fist4", "troll"));
			this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, 0f));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "FistRun", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_ObjectMovement(this.miniboss.gameObject, 0.5f, false));
			this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, this.miniboss.gameObject, Vector3.zero, Vector3.zero));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "FistIdle", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_Speech("I am the one who knocks.", this.miniboss.head, 2.5f, false));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "FistRun", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, -1f));
			this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, null, Vector3.zero, new Vector3(0f, -1.5f, 0f)));
                break;
		case 7:
			this.levelSteps.Enqueue(new LevelStep_Tip("#Benreto", true, 0f, false));
			this.levelSteps.Enqueue(new LevelStep_SummonNpc(Color.black, -1, 0f, 1f, true, "spear5", "vietnamese"));
			this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, 0f));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "SpearRun", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_ObjectMovement(this.miniboss.gameObject, 0.5f, false));
			this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, this.miniboss.gameObject, Vector3.zero, Vector3.zero));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "SpearIdle", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_Speech("Taste the power of the spear.", this.miniboss.head, 1f, false));
//            this.levelSteps.Enqueue(new LevelStep_Speech("Just don't cry.", this.miniboss.head, 1.5f, false));
            this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "SpearRun", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, -1f));
			this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, null, Vector3.zero, new Vector3(0f, -1.5f, 0f)));
			break;
		case 8:
			this.levelSteps.Enqueue(new LevelStep_Tip("#Eto", true, 0f, false));
			this.levelSteps.Enqueue(new LevelStep_SummonNpc(Color.black, -1, 0f, 1f, true, "sword2h2", "viking"));
			this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, 0f));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "Sword2hRun", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_ObjectMovement(this.miniboss.gameObject, 0.5f, false));
			this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, this.miniboss.gameObject, Vector3.zero, Vector3.zero));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "Sword2hIdle", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_Speech("Just don't cry.", this.miniboss.head, 1.5f, false));
//                this.levelSteps.Enqueue(new LevelStep_Speech("Take the bridal lane. This is Kalksa.", this.miniboss.head, 1f, false));
                this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "Sword2hRun", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, -1f));
			this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, null, Vector3.zero, new Vector3(0f, -1.5f, 0f)));
			break;
		case 9:
			this.levelSteps.Enqueue(new LevelStep_Tip("#Kubadou ", true, 0f, false));
			this.levelSteps.Enqueue(new LevelStep_SummonNpc(Color.black, -1, 0f, 1f, true, "sword1h6", "spanish"));
			this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, 0f));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "Sword1hRun", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_ObjectMovement(this.miniboss.gameObject, 0.5f, false));
			this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, this.miniboss.gameObject, Vector3.zero, Vector3.zero));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "Sword1hIdle", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_Speech("The gods once again favored the damn bastard!", this.miniboss.head, 2.5f, false));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "Sword1hRun", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, -1f));
			this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, null, Vector3.zero, new Vector3(0f, -1.5f, 0f)));
			break;
		case 10:
			this.levelSteps.Enqueue(new LevelStep_Tip("#King", true, 0f, false));
			this.levelSteps.Enqueue(new LevelStep_SummonNpc(Color.black, -1, 0f, 1f, true, "spear3", "theking"));
			this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, 0f));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "SpearRun", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_ObjectMovement(this.miniboss.gameObject, 0.5f, false));
			this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, this.miniboss.gameObject, Vector3.zero, Vector3.zero));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "SpearIdle", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_Speech("Wise man say only fools rush in.", this.miniboss.head, 2.5f, false));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "SpearRun", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, -1f));
			this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, null, Vector3.zero, new Vector3(0f, -1.5f, 0f)));
			break;
		case 11:
			this.levelSteps.Enqueue(new LevelStep_Tip("#Mei Ge", true, 0f, false));
			this.levelSteps.Enqueue(new LevelStep_SummonNpc(Color.black, -1, 0f, 1f, true, "axe7", "ottoman"));
			this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, 0f));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "AxeRun", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_ObjectMovement(this.miniboss.gameObject, 0.5f, false));
			this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, this.miniboss.gameObject, Vector3.zero, Vector3.zero));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "AxeIdle", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_Speech("Killing a woman is not the same as killing a man.", this.miniboss.head, 1.5f, false));
            this.levelSteps.Enqueue(new LevelStep_Speech("You must pull the trigger in another way.", this.miniboss.head, 1f, false));
                this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "AxeRun", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, -1f));
			this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, null, Vector3.zero, new Vector3(0f, -1.5f, 0f)));
			break;
		case 12:
			this.levelSteps.Enqueue(new LevelStep_Tip("#Jona Lind", true, 0f, false));
			this.levelSteps.Enqueue(new LevelStep_SummonNpc(Color.black, -1, 0f, 1f, true, "axe8", "dumbo"));
			this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, 0f));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "AxeRun", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_ObjectMovement(this.miniboss.gameObject, 0.5f, false));
			this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, this.miniboss.gameObject, Vector3.zero, Vector3.zero));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "AxeIdle", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_Speech("Do you want to know what happens when the eyeball is pierced?", this.miniboss.head, 1f, false));
            this.levelSteps.Enqueue(new LevelStep_Speech("Do you know how much blood will bleed from your neck after your throat is cut?", this.miniboss.head, 1.5f, false));
            this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "AxeRun", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, -1f));
			this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, null, Vector3.zero, new Vector3(0f, -1.5f, 0f)));
			break;
		case 13:
			this.levelSteps.Enqueue(new LevelStep_Tip("#Caesar", true, 0f, false));
			this.levelSteps.Enqueue(new LevelStep_SummonNpc(Color.black, -1, 0f, 1f, true, "dagger6", "caesar"));
			this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, 0f));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "DaggerRun", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_ObjectMovement(this.miniboss.gameObject, 0.5f, false));
			this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, this.miniboss.gameObject, Vector3.zero, Vector3.zero));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "DaggerIdle", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_Speech("I come, I see, I conquer", this.miniboss.head, 2.5f, false));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "DaggerRun", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, -1f));
			this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, null, Vector3.zero, new Vector3(0f, -1.5f, 0f)));
			break;
		case 14:
			this.levelSteps.Enqueue(new LevelStep_Tip("#Ragnar", true, 0f, false));
			this.levelSteps.Enqueue(new LevelStep_SummonNpc(Color.black, -1, 0f, 1f, true, "axe1", "ragnar"));
			this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, 0f));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "AxeRun", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_ObjectMovement(this.miniboss.gameObject, 0.5f, false));
			this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, this.miniboss.gameObject, Vector3.zero, Vector3.zero));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "AxeIdle", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_Speech("I don't stop because I'm tired, I only stop when I'm done.", this.miniboss.head, 2.5f, false));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "AxeRun", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, -1f));
			this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, null, Vector3.zero, new Vector3(0f, -1.5f, 0f)));
			break;
		case 15:
			this.levelSteps.Enqueue(new LevelStep_Tip("#The Emperor", true, 0f, false));
			this.levelSteps.Enqueue(new LevelStep_SummonNpc(Color.black, -1, 0f, 1f, true, "sword2h6", "emperor"));
			this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, 0f));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "Sword2hRun", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_ObjectMovement(this.miniboss.gameObject, 0.5f, false));
			this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, this.miniboss.gameObject, Vector3.zero, Vector3.zero));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "Sword2hIdle", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_Speech("You are very close to the end Warrior.", this.miniboss.head, 1f, false));
            this.levelSteps.Enqueue(new LevelStep_Speech("But I will enjoy killing you!", this.miniboss.head, 1.5f, false));
            this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "Sword2hRun", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, -1f));
			this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, null, Vector3.zero, new Vector3(0f, -1.5f, 0f)));
			break;
		case 16:
			this.levelSteps.Enqueue(new LevelStep_Tip("#Master Pai Mei", true, 0f, false));
			this.levelSteps.Enqueue(new LevelStep_SummonNpc(Color.black, -1, 0f, 1f, true, "sword2h5", "paimei"));
			this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, 0f));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "Sword2hRun", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_ObjectMovement(this.miniboss.gameObject, 0.5f, false));
			this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, this.miniboss.gameObject, Vector3.zero, Vector3.zero));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "Sword2hIdle", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_Speech("Well done Warrior.\n You surpassed all of my students.", this.miniboss.head, 2.5f, false));
			this.levelSteps.Enqueue(new LevelStep_Speech("Shadow Master?", CharacterManager.instance.head, 1f, false));
			this.levelSteps.Enqueue(new LevelStep_Speech("Yes. I am Pai Mei, Master of the Shadows.\n You beat me and the title is yours!", this.miniboss.head, 2.5f, false));
			this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "Sword2hRun", true, 0f, true));
			this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, -1f));
			this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, null, Vector3.zero, new Vector3(0f, -1.5f, 0f)));
			break;
            case 17:
                this.levelSteps.Enqueue(new LevelStep_Tip("#Shadown Warrior", true, 0f, false));
                this.levelSteps.Enqueue(new LevelStep_SummonNpc(Color.black, -1, 0f, 1f, true, "sword2h2", "thor"));
                this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, 0f));
                this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "Sword2hRun", true, 0f, true));
                this.levelSteps.Enqueue(new LevelStep_ObjectMovement(this.miniboss.gameObject, 0.5f, false));
                this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, this.miniboss.gameObject, Vector3.zero, Vector3.zero));
                this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "Sword2hIdle", true, 0f, true));
                this.levelSteps.Enqueue(new LevelStep_Speech("So you think you can get my title? HAH!", this.miniboss.head, 2.5f, false));
                this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, "Sword2hRun", true, 0f, true));
                this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, -1f));
                this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, null, Vector3.zero, new Vector3(0f, -1.5f, 0f)));
                break;
            default:
				this.levelSteps.Enqueue(new LevelStep_Tip("#Self", true, 0f, false));
				this.levelSteps.Enqueue(new LevelStep_SummonNpc(Color.black, -1, 0f, 1f, true, ItemManager.instance.currentWeapon.id, ItemManager.instance.currentHat.id));
				this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, 0f));
				this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, ItemManager.instance.currentWeapon.category.id + "Run", true, 0f, true));
				this.levelSteps.Enqueue(new LevelStep_ObjectMovement(this.miniboss.gameObject, 0f, false));
				this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, this.miniboss.gameObject, Vector3.zero, Vector3.zero));
				this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, ItemManager.instance.currentWeapon.category.id + "Idle", true, 0f, true));
				this.levelSteps.Enqueue(new LevelStep_Speech("When only you are left, you need to face the most powerful enemy.", this.miniboss.head, 2.5f, false));
				this.levelSteps.Enqueue(new LevelStep_Animation(this.miniboss.spine, ItemManager.instance.currentWeapon.category.id + "Run", true, 0f, true));
				this.levelSteps.Enqueue(new LevelStep_NpcEdit(this.miniboss, this.level, -1f));
				this.levelSteps.Enqueue(new LevelStep_CameraFocus(CharacterManager.instance.gameObject, null, Vector3.zero, new Vector3(0f, -1.5f, 0f)));
			
			break;
		}
		this.levelSteps.Enqueue(new LevelStep_Tip(string.Empty, false, 0f, false));
	}

	public int GetCoinStageCount(int l)
	{
		int num = 3;
		num += this.level / 150 * 7;
		if (num > 10)
		{
			num = 10;
		}
		return num;
	}

	public Vector3 GetWorldPositionOnPlane(Vector3 screenPosition)
	{
		Ray ray = Camera.main.ScreenPointToRay(screenPosition);
		Plane plane = new Plane(Vector3.forward, new Vector3(0f, 0f, CharacterManager.instance.transform.position.z));
		float distance;
		plane.Raycast(ray, out distance);
		return ray.GetPoint(distance);
	}
}
