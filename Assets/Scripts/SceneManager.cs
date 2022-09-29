// Decompile from assembly: Assembly-CSharp.dll
using Com.LuisPedroFonseca.ProCamera2D;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour
{
	public enum State
	{
		waiting,
		justStarted,
		trainArrived,
		gameStarted,
		gameOver,
        gotItem,
        none
	}

	public static SceneManager instance;



	public GameObject playerObject;

	public GameObject chaserObject;

	public TrainManager trainManager;

	public bool gameStarted;

	public bool inputStarted;

	public bool bossFight;


	//public BoxCollider groundCollider;

	//public GameObject ground;

	public bool isEndless;

	public bool gettingItem;

	private float lastHitRange;

	private int lastNpcSpawnSide = 1;

	private Npc lastRight;

	private Npc lastLeft;

	public bool firstStart = true;

	public Text coinsText;

	public Text pointsText;

	public float targetCameraZ;

	public TestMovement trainMover;

	public GameObject mover;

	public bool isBeltExamReady;

	public Image beltImage;

	public Image beltAltImage;

	public GameObject beltRadial;

	public GameObject ui1;

	public GameObject ui2;

	private ProCamera2D cam;

	public ProCamera2DShake shaker;

	public ShakePreset shakePreset;

	public SceneManager.State currentState;

    public SceneManager.State previousState=State.none ;

	//Main Menu GameObjects
	public GameObject[] MainMenuObjects;

    private void Awake()
	{
        Application.runInBackground = true;
		this.targetCameraZ = -9f;
		SceneManager.instance = this;
	}

	private void Start()
	{
        this.firstStart = PlayerPrefsX.GetBool("FirstStart", true);  
		this.cam = Camera.main.GetComponent<ProCamera2D>();
		this.inputStarted = false;
		this.lastHitRange = UnityEngine.Random.Range(2f, 3f);
		this.lastNpcSpawnSide = UnityEngine.Random.Range(0, 2) * 2 - 1;
		this.gameStarted = false;
		this.isBeltExamReady = PlayerPrefsX.GetBool("BeltExam", false);
		if (this.firstStart)
		{
            MenuUIController.instance.CloseMenuFirstStart();
            PlayerPrefsX.SetBool("FirstStart", false);
			base.Invoke("JustStart", 0.5f);
			AnalyticsManager.instance.FirstStart();
		}
		else
		{
			//LeaderboardsManager.instance.GetShadowWarriorData();
			base.Invoke("JustArrive", 0.5f);
		}
		this.SetStartBeltState();
        //AdsController.instance.VisibleBanner();
        //TNCPManager.Start();
    }

	public void Shake()
	{
		this.shaker.Shake();
	}
    public void ShakePreset()
    {
        this.shaker.Shake(this.shakePreset);
    }
    public void GameOver(bool instant = false)
	{
		AudioManager.instance.OnGameOver();

		if (TutorialManager.instance.tutorialIndex == 4)
		{
			TutorialManager.instance.tutorialIndex = 5;
			PlayerPrefs.SetInt("tutorialIndex", 5);
		}
		this.inputStarted = false;
		this.ChangeState(SceneManager.State.gameOver);
		Time.timeScale = 1f;
		if (instant)
		{
			this.Reset();
		}
		else
		{
			base.Invoke("Reset", 2f);
		}

		//Opening Revive panel (Subject to change)
		StartCoroutine(MenuManager.instance.ReviveOpen());
	}

	public void Reset()
	{
        Debug.Log("scenme manager reset");
		//this.ui2.SetActive(true);
		this.targetCameraZ = -9f;
		this.cam.OverallOffset.y = 0f;
		Time.timeScale = 1f;
		this.chaserObject.transform.SetParent(this.playerObject.transform);
		this.chaserObject.transform.localPosition = Vector3.zero;
		NpcManager.instance.OnGameOver();
		if (this.isEndless)
		{
			EndlessManager.instance.OnGameOver();
		}
		//I have disabled ad on gameover(Subject to change)
		//MonetizationManager.instance.OnGameOver();
		StatManager.instance.UpdateStats();
		this.gameStarted = false;
        MenuUIController.instance.OpenGameOver();
		this.SetStartBeltState();
        NpcManager.instance.ContinueAll();
        NpcManager.instance.KillAll(new Vector2(-1f, 10f), new Vector2(2f, 10f), null);
        CharacterManager.instance.Reset();

        //TNCPManager.Show();
        //if (this.isBeltExamReady && StoryManager.instance.level >= 15)
        //{
        //LeaderboardsManager.instance.GetShadowWarriorData();
        //}
    }

	public void Replay()
    {
		MonetizationManager.instance.OnGameOver();
		MenuManager.instance.ReviveClose();
		//this.StartGame();

		//Debug.Log("startgame " + SceneManager.instance.currentState + " "+ this.gettingItem);
		if (SceneManager.instance.currentState != SceneManager.State.gotItem)
		{
			if (this.gettingItem)
			{
				return;
			}
			if (ItemManager.instance.itemMenuOpen || ItemManager.instance.gotItemOpen)
			{
				return;
			}
			if (!this.gameStarted)
			{

				//this.ground.transform.position = new Vector3(CharacterManager.instance.transform.position.x, this.ground.transform.position.y, this.ground.transform.position.z);

				this.ui2.SetActive(false);
				this.chaserObject.transform.SetParent(this.playerObject.transform);
				this.chaserObject.transform.localPosition = Vector3.zero;
				this.isEndless = !this.isBeltExamReady;
				// chinh mode danh
				//this.isEndless = false;
				this.ChangeState(SceneManager.State.gameStarted);
				MonetizationManager.instance.OnGameStart();
				AudioManager.instance.OnGameStart();
				NpcManager.instance.OnGameStart();

				//MainMenu Disable
				foreach (GameObject enemy in MainMenuObjects)
				{
					enemy.SetActive(false);
				}


				if (!this.isEndless)
				{
					StoryManager.instance.StartLevel();
				}
				else
				{
					//EndlessManager.instance.OnStarted();

					EndlessManager.instance.coinAmount = MonetizationManager.instance.coins;
					MissionManager.instance.OnGameStart();

					base.Invoke("StartInput", 0.5f);
				}
				this.gameStarted = true;
				if (MonetizationManager.instance.gameOverCount == 1)
				{
					AnalyticsManager.instance.PlayedFirstGame();
				}
			}
		}
	}

	private void SetStartBeltState()
	{
		if (this.isBeltExamReady)
		{
			this.beltImage.gameObject.SetActive(true);
			this.beltRadial.SetActive(true);
			int num = StoryManager.instance.level;
			if (num > 15)
			{
				num = 15;
			}
			this.beltImage.color = ItemManager.instance.beltColors[num];
			if (num != 0 && num % 2 == 0)
			{
				this.beltAltImage.gameObject.SetActive(true);
			}
			else
			{
				this.beltAltImage.gameObject.SetActive(false);
			}
		}
		else
		{
			this.beltImage.gameObject.SetActive(false);
			this.beltRadial.SetActive(false);
		}
	}

	private void Update()
	{
		
		string text;
		string text2;
		if (this.isEndless)
		{
			text = EndlessManager.instance.point.ToString() + " ";
			if (this.currentState == SceneManager.State.gameOver)
			{
				text += "points";
			}
			else
			{
				text += "p";
			}
			text2 = string.Concat(new object[]
			{
				MonetizationManager.instance.coins.ToString(),
				" (",
				EndlessManager.instance.CoinsWon(),
				") "
			});
		}
		else
		{
			if (this.currentState == SceneManager.State.gameOver)
			{
				if (StoryManager.instance.levelCompleted)
				{
					text = ItemManager.instance.LevelToBeltString(StoryManager.instance.level) + " Belt Exam Passed";
				}
				else
				{
					text = ItemManager.instance.LevelToBeltString(StoryManager.instance.level + 1) + " Belt Exam Failed";
				}
			}
			else
			{
				text = ItemManager.instance.LevelToBeltString(StoryManager.instance.level + 1) + " Belt Exam";
			}
			text2 = MonetizationManager.instance.coins.ToString() + " ";
		}
		if (this.currentState == SceneManager.State.gameOver)
		{
			text2 += "souls";
		}
		else
		{
			text2 += "s";
		}
		if (this.pointsText.text != text)
		{
			this.pointsText.text = text;
		}
		if (this.coinsText.text != text2)
		{
			this.coinsText.text = text2;
		}
		
		switch (this.currentState)
		{
		case SceneManager.State.justStarted:
			if (this.playerObject.transform.position.x > this.chaserObject.transform.position.x)
			{
				this.ChangeState(SceneManager.State.trainArrived);
				this.StartGame();
			}
			break;
		case SceneManager.State.trainArrived:
			this.AutoNpc();
			break;
		case SceneManager.State.gameOver:
			this.AutoNpc();
			break;
		}
		if (this.gameStarted)
		{
			if (CharacterManager.instance.isZooming)
			{
				Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, this.targetCameraZ), Time.unscaledDeltaTime);
			}
			else
			{
				Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, this.targetCameraZ), Time.unscaledDeltaTime);
			}
		}
		else
		{
			Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, this.targetCameraZ), Time.unscaledDeltaTime);
		}
	}

	public void AutoNpc()
	{
		if (!NpcManager.instance.paused)
		{
			Npc npc = null;
			Npc npc2 = null;
			if (NpcManager.instance.leftNpcs.Count > 0)
			{
				npc = NpcManager.instance.leftNpcs.First.Value;
			}
			if (NpcManager.instance.rightNpcs.Count > 0)
			{
				npc2 = NpcManager.instance.rightNpcs.First.Value;
			}
			if (npc2 == null && npc == null)
			{
				this.lastNpcSpawnSide = -(int)Mathf.Sign(CharacterManager.instance.transform.position.x - Camera.main.transform.position.x);
				NpcManager.instance.SpawnEnemyStarting(this.lastNpcSpawnSide);
			}
			if (npc != null && npc.CanBeHit())
			{
				float num = Mathf.Abs(npc.transform.position.x - CharacterManager.instance.transform.position.x);
				if (num < 8.5f && this.lastLeft != npc)
				{
					this.lastLeft = npc;
					this.lastNpcSpawnSide = -(int)Mathf.Sign(CharacterManager.instance.transform.position.x + (float)this.lastNpcSpawnSide * this.lastHitRange - Camera.main.transform.position.x);
					NpcManager.instance.SpawnEnemyStarting(this.lastNpcSpawnSide);
				}
				if (num <= this.lastHitRange)
				{
					CharacterManager.instance.inputQueue.Enqueue(-1);
					this.lastHitRange = UnityEngine.Random.Range(2f, 3f);
				}
			}
			if (npc2 != null && npc2.CanBeHit())
			{
				float num2 = Mathf.Abs(npc2.transform.position.x - CharacterManager.instance.transform.position.x);
				if (num2 < 8.5f && this.lastRight != npc2)
				{
					this.lastRight = npc2;
					this.lastNpcSpawnSide = -(int)Mathf.Sign(CharacterManager.instance.transform.position.x + (float)this.lastNpcSpawnSide * this.lastHitRange - Camera.main.transform.position.x);
					NpcManager.instance.SpawnEnemyStarting(this.lastNpcSpawnSide);
				}
				if (num2 <= this.lastHitRange)
				{
					CharacterManager.instance.inputQueue.Enqueue(1);
					this.lastHitRange = UnityEngine.Random.Range(2f, 3f);
				}
			}
		}
	}
	
	public void ChangeState(SceneManager.State newState = SceneManager.State.justStarted)
	{
		if (newState != SceneManager.State.justStarted)
		{
			if (newState != SceneManager.State.trainArrived)
			{
				if (newState == SceneManager.State.gameStarted)
				{
					//this.groundCollider.enabled = false;
					this.chaserObject.transform.SetParent(this.playerObject.transform);
					this.chaserObject.transform.localPosition = Vector3.zero;
					NpcManager.instance.KillAll(new Vector2(-15f, 15f), new Vector2(-5f, 10f), null);
					if (this.currentState == SceneManager.State.gameOver)
					{
                        MenuUIController.instance.RestartMenu();
                        //this.menu.Play("menuRestart");
                    }
					else
					{
                        MenuUIController.instance.HideLogo();
                        //this.headingAnim.Play("headingClose");
                        if (!this.firstStart)
						{
                            MenuUIController.instance.CloseMenu();
                            //this.menu.Play("menuClose");
						}
					}
				}
			}
			else
			{
				this.trainMover.enabled = false;
				this.trainManager.enabled = true;
				this.chaserObject.transform.SetParent(this.playerObject.transform);
				this.chaserObject.transform.localPosition = new Vector3(0f, -1f, 0f);
			}
		}
        this.previousState = this.currentState;
		this.currentState = newState;
	}

	private void JustStart()
	{

        //this.headingAnim.Play("headingStart");
        MenuUIController.instance.ShowLogo();
		base.Invoke("JustStart2", 3.5f);
	}

	private void JustStart2()
	{
		this.StartGame();
	}

	private void JustArrive()
	{
        this.ChangeState(SceneManager.State.trainArrived);
		//this.menu.Play("menuOpen");
        MenuUIController.instance.OpenMenu();
        MenuUIController.instance.ShowLogo();
        //this.headingAnim.Play("headingStart");
    }

	public void StartGame()
	{
        //Debug.Log("startgame " + SceneManager.instance.currentState + " "+ this.gettingItem);
        if (SceneManager.instance.currentState != SceneManager.State.gotItem) {
            if (this.gettingItem)
            {
                return;
            }
            if (ItemManager.instance.itemMenuOpen || ItemManager.instance.gotItemOpen)
            {
                return;
            }
            if (!this.gameStarted)
            {
                
                //this.ground.transform.position = new Vector3(CharacterManager.instance.transform.position.x, this.ground.transform.position.y, this.ground.transform.position.z);

                this.ui2.SetActive(false);
                this.chaserObject.transform.SetParent(this.playerObject.transform);
                this.chaserObject.transform.localPosition = Vector3.zero;
                this.isEndless = !this.isBeltExamReady;
                // chinh mode danh
                //this.isEndless = false;
                this.ChangeState(SceneManager.State.gameStarted);
                MonetizationManager.instance.OnGameStart();
                AudioManager.instance.OnGameStart();
                NpcManager.instance.OnGameStart();

				//MainMenu Disable
				foreach(GameObject enemy in MainMenuObjects)
                {
					enemy.SetActive(false);
				}


				if (!this.isEndless)
                {
                    StoryManager.instance.StartLevel();
                }
                else
                {
                    EndlessManager.instance.OnStarted();
                    base.Invoke("StartInput", 0.5f);
                }
                this.gameStarted = true;
                if (MonetizationManager.instance.gameOverCount == 1)
                {
                    AnalyticsManager.instance.PlayedFirstGame();
                }
            }
        }
	}

	public void StartGameSurvival()
	{
		/*//Debug.Log("startgame " + SceneManager.instance.currentState + " "+ this.gettingItem);
		if (SceneManager.instance.currentState != SceneManager.State.gotItem)
		{
			if (this.gettingItem)
			{
				return;
			}
			if (ItemManager.instance.itemMenuOpen || ItemManager.instance.gotItemOpen)
			{
				return;
			}
			if (!this.gameStarted)
			{

				//this.ground.transform.position = new Vector3(CharacterManager.instance.transform.position.x, this.ground.transform.position.y, this.ground.transform.position.z);

				this.ui2.SetActive(false);
				this.chaserObject.transform.SetParent(this.playerObject.transform);
				this.chaserObject.transform.localPosition = Vector3.zero;
				this.isEndless = !this.isBeltExamReady;
				// chinh mode danh
				//this.isEndless = false;
				this.ChangeState(SceneManager.State.gameStarted);
				MonetizationManager.instance.OnGameStart();
				AudioManager.instance.OnGameStart();
				NpcManager.instance.OnGameStart();

				//MainMenu Disable
				foreach (GameObject enemy in MainMenuObjects)
				{
					enemy.SetActive(false);
				}


				if (!this.isEndless)
				{
					StoryManager.instance.StartLevel();
				}
				else
				{
					EndlessManager.instance.OnStarted();
					base.Invoke("StartInput", 0.5f);
				}
				this.gameStarted = true;
				if (MonetizationManager.instance.gameOverCount == 1)
				{
					AnalyticsManager.instance.PlayedFirstGame();
				}
			}
		}*/
	}

	private void StartInput()
	{
		this.inputStarted = true;
	}

	
}
