// Decompile from assembly: Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionManager : MonoBehaviour
{
    public static MissionManager instance;

    public Mission[] missions;

    public Text[] missionTexts;

	public string MissionName;
	public string CurrentLevelProgress;

    public Text[] missionColliders;

    public Image[] missionLines;

    public CanvasGroup[] missionCanvases;

    //public CanvasGroup skipCanvas;

    //public Text skipText;

    private int missionCount = 3;

    private List<int> oldMissionTypes = new List<int>();

    private int[] missionTypes;

    private int[] missionValues;

    private float missionAlpha;

    public double missionFadeTime;

    private void Awake()
    {
        MissionManager.instance = this;
    }

    private void Start()
    {
        this.missions = null;
        this.missionTypes = PlayerPrefsX.GetIntArray("missionTypes");
        this.missionValues = PlayerPrefsX.GetIntArray("missionValues");
        if (this.missionTypes != null && this.missionTypes.Length>0)
		{
			this.missions = new Mission[this.missionCount];
			for (int i = 0; i < this.missionCount; i++)
			{
				this.missions[i] = this.CreateMission(this.missionTypes[i]);
				this.missions[i].currentValue = this.missionValues[i];
				this.missions[i].tempValue = this.missionValues[i];

            }
		}
	}

	private void Update()
	{
		if (!SceneManager.instance.gameStarted || !SceneManager.instance.isEndless || ((double)Time.time > this.missionFadeTime && !MenuManager.instance.isPaused))
		{
			this.missionAlpha = 0f;
		}
		else
		{
			this.missionAlpha = 1f;
		}
		for (int i = 0; i < this.missionCount; i++)
		{
			if (this.missions != null)
			{
				this.missionTexts[i].gameObject.SetActive(true);
				string text = this.missions[i].text;
				if (!this.missions[i].IsCompleted())
				{
					string text2 = text;
					text = string.Concat(new object[]
					{
						text2,
						" (",
						this.missions[i].tempValue,
						")"
					});
				}
				if (this.missionTexts[i].text != text)
				{
					this.missionTexts[i].text = text;
				}
				this.missionLines[i].gameObject.SetActive(this.missions[i].IsCompleted());
				this.missionColliders[i].gameObject.SetActive(!this.missions[i].IsCompleted() && MenuManager.instance.isPaused && SceneManager.instance.isEndless);
				float num = Mathf.Max(this.missionAlpha, this.missions[i].GetAlpha());
				if (!SceneManager.instance.gameStarted || TutorialManager.instance.tutorialIndex < 4)
				{
					num = 0f;
				}
				if (this.missionCanvases[i].alpha != num)
				{
					this.missionCanvases[i].alpha = Mathf.MoveTowards(this.missionCanvases[i].alpha, num, Time.unscaledDeltaTime * 2f);
				}
			}
			else
			{
				this.missionColliders[i].gameObject.SetActive(false);
				this.missionTexts[i].gameObject.SetActive(false);
			}
		}
		float target = this.missionAlpha;
		if (!SceneManager.instance.gameStarted || TutorialManager.instance.tutorialIndex < 4)
		{
			target = 0f;
		}
		this.missionCanvases[this.missionCount].alpha = Mathf.MoveTowards(this.missionCanvases[this.missionCount].alpha, target, Time.unscaledDeltaTime * 2f);
		string text3 = ItemManager.instance.LevelToBeltString(StoryManager.instance.level + 1).ToUpper() + " BELT MISSIONS";

		//Giving info about player's current progress
		CurrentLevelProgress = text3;

		//Setting Mission Progress (Subject to change)
		//MissionName = text3;
		text3 = MissionName;

		if (this.missionTexts[this.missionCount].text != text3)
		{
			this.missionTexts[this.missionCount].text = text3;
		}
		float num2 = 0f;
		if (MenuManager.instance.isPaused && SceneManager.instance.isEndless)
		{
			num2 = 1f;
		}
		//if (this.skipCanvas.alpha != num2)
		//{
		//	this.skipCanvas.alpha = Mathf.MoveTowards(this.skipCanvas.alpha, num2, Time.unscaledDeltaTime * 2f);
		//}
	}

	public void OnGameStart()
	{
		base.Invoke("SetMissionFadeTime", 1.5f);
		MissionManager.instance.SetColors(Color.white, -1);
		if (this.missions == null)
		{
			this.SetNewMissions();
		}
		else
		{
			for (int i = 0; i < this.missionCount; i++)
			{
				if (!this.missions[i].IsCompleted() && this.missions[i].resetAtGameOver)
				{
					this.missions[i].tempValue = this.missions[i].currentValue;

                }
			}
		}
	}

	private void SetMissionFadeTime()
	{
		this.missionFadeTime = (double)(Time.time + 3.5f);
	}

	public void OnGameOver()
	{
		this.missionFadeTime = (double)Time.time;
		if (this.missions == null)
		{
			return;
		}
		bool flag = false;
		for (int i = 0; i < this.missionCount; i++)
		{
			if (!this.missions[i].IsCompleted())
			{
				this.missions[i].OnGameOver();
			}
			if (!this.missions[i].IsCompleted())
			{
				if (this.missions[i].resetAtGameOver)
				{
					this.missions[i].Reset();
				}
				flag = true;
			}
		}
        // set complete mission
        //flag = false;
		this.SaveMissions();
		if (!flag)
		{
			this.AllMissionsCompleted();
		}
		if (this.missions == null)
		{
			this.SetNewMissions();
		}
	}

	public void OnBossKilled()
	{
		if (this.missions != null)
		{
			for (int i = 0; i < this.missionCount; i++)
			{
				if (!this.missions[i].IsCompleted())
				{
					this.missions[i].OnBossKilled();
				}
			}
		}
	}

	public void OnEnemyKilled(int type)
	{
		if (this.missions != null)
		{
			for (int i = 0; i < this.missionCount; i++)
			{
				if (!this.missions[i].IsCompleted())
				{
					this.missions[i].OnEnemyKilled(type);
				}
			}
		}
	}

	public void OnCoinCollected(int amount)
	{
		if (this.missions != null)
		{
			for (int i = 0; i < this.missionCount; i++)
			{
				if (!this.missions[i].IsCompleted())
				{
					this.missions[i].OnCoinCollected(amount);
				}
			}
		}
	}

	public void SetColors(Color color, int index = -1)
	{
		if (this.missions == null)
		{
			return;
		}
		if (index == -1)
		{
			for (int i = 0; i < this.missionCount + 1; i++)
			{
				this.SetColors(color, i);
			}
		}
		else
		{
			this.missionTexts[index].color = color;
			this.missionLines[index].color = color;
		}
	}

	private void SetNewMissions()
	{
		this.missions = new Mission[this.missionCount];
		List<int> list = new List<int>();
		list.Add(0);
		list.Add(1);
		list.Add(4);
		list.Add(5);
		list.Add(11);
		list.Add(12);
		if (StoryManager.instance.level >= 1)
		{
			list.Add(9);
			list.Add(10);
			if (StoryManager.instance.level >= 2)
			{
				list.Add(2);
			}
		}
		for (int i = 0; i < this.oldMissionTypes.Count; i++)
		{
			if (list.Count <= 3)
			{
				break;
			}
			if (list.Contains(this.RealTypeToMissionType(this.oldMissionTypes[i])))
			{
				list.Remove(this.RealTypeToMissionType(this.oldMissionTypes[i]));
			}
		}
		for (int j = 0; j < this.missionCount; j++)
		{
			int index = UnityEngine.Random.Range(0, list.Count);
			int type = list[index];
			list.RemoveAt(index);
			int type2 = this.MissionTypeToRealType(type);
			this.missions[j] = this.CreateMission(type2);
		}
		this.SaveMissions();
	}

	private void SaveMissions()
	{
		if (this.missions == null)
		{
			PlayerPrefs.DeleteKey("missionTypes");
            PlayerPrefs.DeleteKey("missionValues");
			return;
		}
		this.missionTypes = new int[this.missionCount];
		this.missionValues = new int[this.missionCount];
		for (int i = 0; i < this.missionCount; i++)
		{
			this.missionTypes[i] = this.missions[i].type;
			this.missionValues[i] = this.missions[i].currentValue;
		}
		PlayerPrefsX.SetIntArray("missionTypes", this.missionTypes);
        PlayerPrefsX.SetIntArray("missionValues", this.missionValues);
	}

	private int MissionTypeToRealType(int type)
	{
		if (type == 2)
		{
			List<int> list = new List<int>();
			list.Add(3);
			if (StoryManager.instance.level >= 4)
			{
				list.Add(2);
			}
			return list[UnityEngine.Random.Range(0, list.Count)];
		}
		if (type == 9)
		{
			List<int> list2 = new List<int>();
			if (StoryManager.instance.level >= 1)
			{
				list2.Add(10);
				if (StoryManager.instance.level >= 2)
				{
					list2.Add(11);
					if (StoryManager.instance.level >= 3)
					{
						list2.Add(12);
						if (StoryManager.instance.level >= 4)
						{
							list2.Add(13);
							if (StoryManager.instance.level >= 5)
							{
								list2.Add(14);
								if (StoryManager.instance.level >= 6)
								{
									list2.Add(9);
								}
							}
						}
					}
				}
			}
			return list2[UnityEngine.Random.Range(0, list2.Count)];
		}
		if (type == 10)
		{
			return UnityEngine.Random.Range(15, 17);
		}
		if (type == 11)
		{
			return UnityEngine.Random.Range(17, 20);
		}
		if (type == 12)
		{
			return 20;
		}
		return type;
	}

	private int RealTypeToMissionType(int realType)
	{
		switch (realType)
		{
		case 9:
		case 10:
		case 11:
		case 12:
		case 13:
		case 14:
			return 9;
		case 15:
		case 16:
			return 10;
		case 17:
		case 18:
		case 19:
			return 11;
		case 20:
			return 12;
		default:
			if (realType != 2 && realType != 3)
			{
				return realType;
			}
			return 2;
		}
	}


	private Mission CreateMission(int type)
	{
		bool reset = false;
		float num = 0f;
		int num2 = StoryManager.instance.level;
		if (num2 > 15)
		{
			num2 = 15;
		}
		string text = string.Empty;
		switch (type)
		{
		case 0:
			num = (float)(20 * num2 + 15);
			text = "Collect " + (int)num + " souls";
			break;
		case 1:
			num = 50f * (float)num2 + 50f;
			text = "Kill " + (int)num + " enemies";
			break;
		case 2:
			num = (float)(3 * num2 + 5);
			text = "Reach " + (int)num + " souls combo";
			reset = true;
			break;
		case 3:
			num = (float)(3 * num2 + 10);
			text = "Reach " + (int)num + " souls";
			reset = true;
			break;
		case 4:
			num = (float)(1800 * num2 + 3000);
			text = "Reach " + (int)num + " points";
			reset = true;
			break;
		case 5:
			num = (float)(1200 * num2 + 2000);
			text = "Reach " + (int)num + " score without collecting any souls";
			reset = true;
			break;
		case 9:
		case 10:
		case 11:
		case 12:
		case 13:
		case 14:
			num = 36.67f * (float)num2 + 50f;
			if (type == 9)
			{
				text = " with a axe";
			}
			else if (type == 10)
			{
				text = " with a one-handed sword";
			}
			else if (type == 11)
			{
				text = " with a dagger";
			}
			else if (type == 12)
			{
				text = "with a two-handed sword";
			}
			else if (type == 13)
			{
				text = "with a gloves";
			}
			else if (type == 14)
			{
				text = "with a spear";
			}
            text += "Kill " + (int)num + " enemies";
            break;
		case 15:
		case 16:
			num = 5.714f * (float)num2 + 14.286f;
			text = "Kill " + (int)num + " enemies during rain or snow";
			break;
		case 17:
		case 18:
			num = 3f * (float)num2 + 3f;
			text = "Kill " + (int)num;
			if (type == 17)
			{
				text += " Red enemies in one game";
			}
			else if (type == 18)
			{
				text += " Blue enemies in one game";
			}
			reset = true;
			break;
		case 19:
			num = (float)(20 * num2 + 25);
			text = "Kill " + (int)num + " enemies in one game";
			reset = true;
			break;
		case 20:
			num = 1.8f * (float)num2 + 3f;
			text = "Die " + (int)num + " times";
			break;
		}
		return new Mission(type, (int)num, reset, text);
	}

	public void CheckAllMissionsCompletion()
	{
		for (int i = 0; i < this.missionCount; i++)
		{
			if (!this.missions[i].IsCompleted())
			{
				return;
			}
		}
		this.missionFadeTime = (double)(Time.time + 4f);

		//Starting boss fight when player completes all missions
		//SceneManager.instance.Reset();
		LevelSystem.instance.GenerateLevel(0);
	}

	private void AllMissionsCompleted()
	{
		if (this.missions != null)
		{
			this.oldMissionTypes.Clear();
			for (int i = 0; i < this.missions.Length; i++)
			{
				this.oldMissionTypes.Add(this.missions[i].type);
			}
		}
		this.missions = null;
		SceneManager.instance.isBeltExamReady = true;
        PlayerPrefsX.SetBool("BeltExam", true);
		this.SaveMissions();
	}

	public void BuyMission(int missionNo)
	{
		if (this.missions[missionNo].type >= 9 && this.missions[missionNo].type <= 14)
		{
			return;
		}
		if (MonetizationManager.instance.coins >= 200)
		{
			MonetizationManager expr_47 = MonetizationManager.instance;
			expr_47.coins -= 200;
			EndlessManager.instance.coinAmount -= 200;
			MonetizationManager.instance.SaveCoins();
			this.missions[missionNo].Complete();
		}
	}
}
