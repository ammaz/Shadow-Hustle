// Decompile from assembly: Assembly-CSharp.dll
using CodeStage.AntiCheat.ObscuredTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;
using Spine.Unity;
using GoogleMobileAds.Api;

public class MonetizationManager : MonoBehaviour
{
	public static MonetizationManager instance;

	private const int factor = 2;

	private ObscuredInt freeGiftCount;

	private ObscuredInt gotItemCount;

	private DateTime lastGiftTime;

	public GameObject tipObject;

	public Text tipText;

	public Text giftNotReady;

	public Text giftClaim;

	public Text watchToWin;

	public Text coinNotReady;

	public Text coinClaim;

	public Text beltExam;

	public GameObject giftObject;

	public GameObject itemObject;

	public GameObject topObject;

	public ObscuredInt coins;

	public GameObject coinObject;

    public GameObject coinEffect;

    public SkeletonAnimation soulAnim;

	public Text comboText;

	//private bool willShowAd;

	private bool willShowItem;

	private bool willShowGift;

	public double coinSpawnTime;

	public bool coinSpawned;

	public int coinSide;

	private int targetMinutes;

	private int lastCoinSide;

	public ObscuredInt comboCount;

	public ObscuredInt interstitialShowCount;

	public int gameOverCount;

	public int watchToWinAmount;

	public ObscuredInt highestCoinCombo;

	public Image beltImage;

	public Image beltAltImage;

	public GameObject loadingObject;

    public GameObject gameOverMenu;

	public MenuToggle notifToggle;

	private BannerView bannerView;
	private InterstitialAd interstitial;

	//public bool justShowAdBefore = false;
	private void Awake()
	{
		MonetizationManager.instance = this;
	}

	private void Start()
	{
		MobileAds.Initialize(initStatus => { });
		Debug.Log("InitAds");
		this.RequestInterstitial();
		this.RequestBanner();

		this.gameOverCount = PlayerPrefs.GetInt("gameOverCount", 0);
		this.coins = PlayerPrefs.GetInt("coins", 0);
        this.freeGiftCount = PlayerPrefs.GetInt("freeGiftCount", 0);
		this.gotItemCount = PlayerPrefs.GetInt("gotItemCount", 0);
		this.lastGiftTime = DateTime.Parse(PlayerPrefs.GetString("lastGiftTime", DateTime.Now.ToString()));
		this.interstitialShowCount = PlayerPrefs.GetInt("interstitialShowCount", 0);
		if (this.interstitialShowCount == 0)
		{
			this.interstitialShowCount = this.gameOverCount + 1;
            PlayerPrefs.SetInt("interstitialShowCount", this.interstitialShowCount);
		}
	}

	private void RequestBanner()
	{
            string adUnitBanner = "ca-app-pub-3940256099942544/6300978111";
		this.bannerView = new BannerView(adUnitBanner, AdSize.Banner, AdPosition.Top);
		this.bannerView.OnAdFailedToLoad += this.HandleOnAdFailedToLoadBanner;
		this.bannerView.OnAdOpening += this.HandleOnAdOpenedBanner;
		AdRequest request = new AdRequest.Builder().Build();
		this.bannerView.LoadAd(request);
	}

	public void HandleOnAdFailedToLoadBanner(object sender, AdFailedToLoadEventArgs args)
	{
		this.RequestBanner();
	}

	public void HandleOnAdOpenedBanner(object sender, EventArgs args)
	{
		this.RequestBanner();
	}

	private void RequestInterstitial()
	{
        string adUnitInterstitial = "ca-app-pub-3940256099942544/1033173712";
		this.interstitial = new InterstitialAd(adUnitInterstitial);
		this.interstitial.OnAdOpening += HandleOnAdOpened;
		this.interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
		AdRequest request = new AdRequest.Builder().Build();
		this.interstitial.LoadAd(request);
	}

	public void HandleOnAdOpened(object sender, EventArgs args)
	{
		this.RequestInterstitial();
	}

	public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
	{
		this.RequestInterstitial();
	}



	public void OnGameStart()
	{
		this.coinSpawnTime = (double)(Time.time - 10f);
		this.highestCoinCombo = 0;
		
	}

    public void OnGameOver()
    {
		if (this.interstitial.IsLoaded())
		{
			this.interstitial.Show();
		}
		
		this.SaveCoins();
        this.coinSpawned = false;
        //this.willShowAd = false;
        this.willShowGift = false;
        this.willShowItem = false;
        //this.justShowAdBefore = false;


        //if (this.gameOverCount > 5 && PlayerPrefsX.GetBool("removeads", false) == false)
        //{
        //    //AdsController.instance.ShowInterstitial();
        //    justShowAdBefore = true;
        //}

        // sua sau

        //Debug.Log("reward load " + AdsController.instance.rewardBasedVideo.IsLoaded() + " unity reward " + Advertisement.IsReady());

        //if (AdsController.instance.rewardBasedVideo.IsLoaded() || Advertisement.IsReady())
        //{
        //    int num = this.gameOverCount % 3;
        //    if (num == 0)
        //    {
        //        this.willShowAd = false;
        //    }
        //    else if (num == 1)
        //    {
        //        this.willShowAd = (UnityEngine.Random.Range(0, 2) == 0);
        //    }
        //    else
        //    {
        //        this.willShowAd = true;
        //    }
        //    if (this.willShowAd)
        //    {
        //        this.watchToWinAmount = 50;
        //        if (SceneManager.instance.isEndless)
        //        {
        //            if (EndlessManager.instance.coinsWon * 2 > this.watchToWinAmount)
        //            {
        //                this.watchToWinAmount = EndlessManager.instance.CoinsWon() * 2;
        //                this.watchToWin.text = "x3 灵魂 (+" + this.watchToWinAmount + ")";
        //            }
        //            else
        //            {
        //                this.watchToWin.text = "免费 +" + this.watchToWinAmount + " 灵魂";
        //            }
        //        }
        //        else
        //        {
        //            this.watchToWin.text = "免费 +" + this.watchToWinAmount + " 灵魂";
        //        }
        //    }
        //}
  //      if (!this.willShowAd)
		//{
		//	this.NextTip();
		//}
  //      if (this.gameOverCount > 10 && this.gameOverCount % 5 == 0)
  //      {
  //          this.willShowAd = true;
  //      } else
  //      {
  //          this.willShowAd = false;
  //      }
  //      if (this.gameOverCount > 10 && this.gameOverCount % 7==0 && StatManager.instance.stats[0].value> PlayerPrefs.GetInt("leaderboardsBestScore", 0))
		//{
		//	this.willShowAd = false;
		//}

  //      if (justShowAdBefore && PlayerPrefsX.GetBool("removeads", false) == true)
  //      {
  //          this.willShowAd = false;
  //      }

		if (this.CheckForFreeGift())
		{
			this.willShowGift = true;
		}
		else
		{
			TimeSpan timeSpan = DateTime.Now - this.lastGiftTime;
			double num2 = (double)this.targetMinutes - timeSpan.TotalMinutes;
			if (num2 < 10.0)
			{
				if (this.gameOverCount % 3 == 0)
				{
					this.willShowGift = true;
				}
			}
			else if (this.gameOverCount % 6 == 0)
			{
				this.willShowGift = true;
			}
		}
		this.beltExam.gameObject.SetActive(false);
		if (!SceneManager.instance.isEndless && !StoryManager.instance.levelCompleted)
		{
			this.beltExam.gameObject.SetActive(true);
			this.coinClaim.gameObject.SetActive(false);
			this.coinNotReady.gameObject.SetActive(false);
			int num3 = StoryManager.instance.level;
			if (num3 > 15)
			{
				num3 = 15;
			}
			this.beltImage.color = ItemManager.instance.beltColors[num3];
			if (num3 != 0 && num3 % 2 == 0)
			{
				this.beltAltImage.gameObject.SetActive(true);
			}
			else
			{
				this.beltAltImage.gameObject.SetActive(false);
			}
			int num4 = StoryManager.instance.level * 50;
			if (StoryManager.instance.level == 0)
			{
				num4 = 0;
			}
			else if (num4 < 200)
			{
				num4 = 200;
			}
			else if (num4 > 600)
			{
				num4 = 600;
			}
			this.beltExam.text = "TRY AGAIN FOR " + num4 + " SOULS";
			this.willShowItem = true;
		}
		else if (this.CheckForCoinGift())
		{
			this.willShowItem = true;
		}
		else if (this.coins >= 180)
		{
			if (this.gameOverCount % 2 == 0)
			{
				this.willShowItem = true;
			}
		}
		else if (this.gameOverCount % 4 == 0)
		{
			this.willShowItem = true;
		}
		this.gameOverCount++;
        PlayerPrefs.SetInt("gameOverCount", this.gameOverCount);
		GameOverTipManager.instance.Save();
	}

	public void SpawnCoin(int side = 0)
	{
		this.coinSpawnTime = (double)Time.time;
		if (side == 0)
		{
			this.coinSide = -CharacterManager.instance.side;
		}
		else
		{
			this.coinSide = side;
		}
		if (UnityEngine.Random.Range(0, 2) == 0)
		{
			this.coinSide = UnityEngine.Random.Range(0, 2) * 2 - 1;
		}
		float num = UnityEngine.Random.Range(5f, 7f);
		this.lastCoinSide = this.coinSide;
		this.coinObject.transform.localScale = Vector3.zero;
		this.coinObject.transform.position = new Vector3(CharacterManager.instance.transform.position.x + (float)this.coinSide * num, 1, this.coinObject.transform.position.z);
		this.coinSpawned = true;
	}

	public void HideCoin()
	{
		this.coinSpawned = false;
		this.comboCount = 0;
	}

	public void NextTip()
	{
		this.tipText.text = GameOverTipManager.instance.GetNextTip();
	}

	private void Update()
	{
		if (this.comboCount > 1)
		{
			if (!this.comboText.gameObject.activeInHierarchy)
			{
				this.comboText.gameObject.SetActive(true);
			}
			string text = "Soul combo x " + this.comboCount;
			if (this.comboText.text != text)
			{
				this.comboText.text = text;
			}
		}
		else if (this.comboText.gameObject.activeInHierarchy)
		{
			this.comboText.gameObject.SetActive(false);
		}
		if (this.coinSpawned)
		{
			if (!this.coinObject.activeInHierarchy)
			{
				this.coinObject.SetActive(true);
                soulAnim.AnimationState.SetAnimation(0, "start", false);
                soulAnim.AnimationState.AddAnimation(0, "idle", true,0);
            }
			if (this.coinObject.transform.localScale.x < 0.07f)
			{
				this.coinObject.transform.localScale += new Vector3(Time.deltaTime, Time.deltaTime, Time.deltaTime) / 3f;
			}
			else
			{
				this.coinObject.transform.localScale = new Vector3(0.07f, 0.07f, 0.07f);
			}
			float num;
			if (this.coinSide == 1)
			{
				num = CharacterManager.instance.transform.position.x - this.coinObject.transform.position.x;
			}
			else
			{
				num = this.coinObject.transform.position.x - CharacterManager.instance.transform.position.x;
			}
			if (num > -0.3f)
			{
				this.CoinCollected();
			}
		}
		else if (this.coinObject.transform.localScale.x > 0f)
		{
			this.coinObject.transform.localScale -= new Vector3(Time.deltaTime, Time.deltaTime, Time.deltaTime) / 3f;
		}
		else
		{
			this.coinObject.transform.localScale = new Vector3(0f, 0f, 0f);
			this.coinObject.SetActive(false);
		}
		if (SceneManager.instance.gameStarted && TutorialManager.instance.tutorialIndex >= 4 && SceneManager.instance.isEndless && TutorialManager.instance.tutorialIndex > 4)
		{
			if (this.coinSpawnTime + 10.0 < (double)Time.time && NpcManager.instance.killCount > 0)
			{
				this.SpawnCoin(0);
			}
			if (this.coinObject.activeInHierarchy && this.coinSpawnTime + 4.0 < (double)Time.time)
			{
				if (!SceneManager.instance.isEndless && !StoryManager.instance.introCompleted)
				{
					return;
				}
				this.HideCoin();
			}
		}
		else
		{
			this.HideCoin();
		}
		if (SceneManager.instance.currentState == SceneManager.State.gameOver)
		{
			this.CheckForFreeGift();
			this.CheckForCoinGift();
            if (!this.topObject.activeInHierarchy)
            {
                this.topObject.SetActive(true);
            }
            if (!this.tipObject.activeInHierarchy)
            {
                this.NextTip();
                this.tipObject.SetActive(true);
            }

            //if (this.willShowAd)
            //{
            //    if (this.tipObject.activeInHierarchy)
            //    {
            //        AnalyticsManager.instance.RewardedVideoAvailable();
            //        this.tipObject.SetActive(false);
            //    }

            //    if (this.removeAdObject.activeInHierarchy)
            //    {
            //        this.removeAdObject.SetActive(false);
            //    }

            //}      

            //if (this.justShowAdBefore)
            //{
            //    if (this.tipObject.activeInHierarchy)
            //    {
            //        this.tipObject.SetActive(false);
            //    }
            //    if (!this.removeAdObject.activeInHierarchy)
            //    {
            //        this.removeAdObject.SetActive(true);
            //    }
            //}

            if (this.willShowItem)
			{
				if (!this.itemObject.activeInHierarchy)
				{
					this.itemObject.SetActive(true);
				}
			}
			else if (this.itemObject.activeInHierarchy)
			{
				this.itemObject.SetActive(false);
			}
			if (this.willShowGift)
			{
				if (!this.giftObject.activeInHierarchy)
				{
					this.giftObject.SetActive(true);
				}
			}
			else if (this.giftObject.activeInHierarchy)
			{
				this.giftObject.SetActive(false);
			}
		}
	}

	public void CoinCollected()
	{
        AudioManager.instance.CoinCollectSound();
        this.coinEffect.SetActive(false);
        this.coinEffect.transform.SetParent(this.coinObject.transform);
        this.coinEffect.transform.localPosition = Vector3.zero;
        this.coinEffect.SetActive(true);
        this.coinEffect.transform.SetParent(null);
        soulAnim.AnimationState.SetAnimation(0, "end", false);
		int num = 1; //自定义
		this.comboCount = ++this.comboCount;
		if (this.comboCount > this.highestCoinCombo)
		{
			this.highestCoinCombo = this.comboCount;
		}
		if (this.comboCount > StatManager.instance.stats[9].value)
		{
			StatManager.instance.stats[9].UpdateStat(this.comboCount);
		}
		this.SpawnCoin(-this.lastCoinSide);
		
		this.coins += num;
		if (SceneManager.instance.isEndless)
		{
			MissionManager.instance.OnCoinCollected(num);
		}
		StatManager.instance.stats[8].UpdateStat(StatManager.instance.stats[8].value + num);
	}

	public int GetFreeGift()
	{
		int num;
		if (this.freeGiftCount == 0 || this.freeGiftCount == 1)
		{
			num = 100;
		}
		else if (this.freeGiftCount == 2)
		{
			num = 120;
		}
		else
		{
			int num2 = UnityEngine.Random.Range(0, 101);
			if (num2 == 100)
			{
				num = UnityEngine.Random.Range(4, 101) * 10;
			}
			else if (num2 > 90)
			{
				num = UnityEngine.Random.Range(4, 51) * 10;
			}
			else if (num2 > 80)
			{
				num = UnityEngine.Random.Range(4, 21) * 10;
			}
			else if (num2 > 50)
			{
				num = UnityEngine.Random.Range(4, 13) * 10;
			}
			else
			{
				num = UnityEngine.Random.Range(4, 8) * 10;
			}
		}
		num *= 2;
		this.freeGiftCount = ++this.freeGiftCount;
        PlayerPrefs.SetInt("freeGiftCount", this.freeGiftCount);
		this.CheckForFreeGift();
		this.ResetFreeGiftTime();
		return num;
	}

	public bool CheckForFreeGift()
	{
		TimeSpan t = DateTime.Now - this.lastGiftTime;
		string text = string.Empty;
		bool flag = false;
		if (t.Days > 0)
		{
			flag = true;
		}
		else
		{
			double totalMinutes = t.TotalMinutes;
			switch (this.freeGiftCount)
			{
			case 0:
				this.targetMinutes = 0;
				break;
			case 1:
				this.targetMinutes = 3;
				break;
			case 2:
				this.targetMinutes = 6;
				break;
			case 3:
				this.targetMinutes = 30;
				break;
			case 4:
				this.targetMinutes = 60;
				break;
			case 5:
				this.targetMinutes = 180;
				break;
			default:
				this.targetMinutes = 360;
				break;
			}
			if (totalMinutes > (double)this.targetMinutes)
			{
				flag = true;
			}
		}
        // checkgift ok
        //flag = true;
        if (!flag)
		{
			TimeSpan timeSpan = TimeSpan.FromMinutes((double)this.targetMinutes) - t;
			text = string.Concat(new object[]
			{
                "FREE GIFT IN  ",
				timeSpan.Hours,
                "h ",
				timeSpan.Minutes + 1,
                "m"
            });
			if (this.giftNotReady.text != text)
			{
				this.giftNotReady.text = text;
			}
		}
		if (this.giftClaim.gameObject.activeInHierarchy == !flag)
		{
			this.giftClaim.gameObject.SetActive(flag);
		}
		if (this.giftNotReady.gameObject.activeInHierarchy == flag)
		{
			this.giftNotReady.gameObject.SetActive(!flag);
		}
       
		return flag;
	}

	public bool CheckForCoinGift()
	{
		bool flag = false;
		if (!SceneManager.instance.isEndless && !StoryManager.instance.levelCompleted)
		{
			return false;
		}
		if (this.coins >= 200)
		{
			flag = true;
		}
		if (!flag)
		{
			string text = ""+ (200 - this.coins) + " SOULS TO GO";
			if (this.coinNotReady.text != text)
			{
				this.coinNotReady.text = text;
			}
		}
		if (this.coinClaim.gameObject.activeInHierarchy == !flag)
		{
			this.coinClaim.gameObject.SetActive(flag);
		}
		if (this.coinNotReady.gameObject.activeInHierarchy == flag)
		{
			this.coinNotReady.gameObject.SetActive(!flag);
		}
		return flag;
	}

	public void ResetFreeGiftTime()
	{
#if UNITY_ANDROID
        //this.ClearAndScheduleNotifications();
#endif
        this.lastGiftTime = DateTime.Now;
        PlayerPrefs.SetString("lastGiftTime", DateTime.Now.ToString());
	}

	public void ClearAndScheduleNotifications()
	{
        //NPBinding.NotificationService.CancelAllLocalNotification();
        //NPBinding.NotificationService.ClearNotifications();
        //if (this.notifToggle.isOn)
        //{
        //    this.CreateNotification(DateTime.Now.AddMinutes((double)this.targetMinutes), "你的免费礼物在这里！", eNotificationRepeatInterval.NONE);
        //    this.CreateNotification(DateTime.Now.AddMinutes((double)this.targetMinutes).AddDays(1.0), "别忘了领取免费礼物。", eNotificationRepeatInterval.NONE);
        //    this.CreateNotification(DateTime.Now.AddMinutes((double)this.targetMinutes).AddDays(2.0), "暗影武士，我们给你准备了礼物！", eNotificationRepeatInterval.NONE);
        //    this.CreateNotification(DateTime.Now.AddMinutes((double)this.targetMinutes).AddDays(3.0), "你不想要这份礼物吗？ :(", eNotificationRepeatInterval.NONE);
        //}
    }

    public void OnBeltExamAgainPressed()
	{
		int num = StoryManager.instance.level * 50;
		if (StoryManager.instance.level == 0)
		{
			num = 0;
		}
		else if (num < 200)
		{
			num = 200;
		}
		else if (num > 600)
		{
			num = 600;
		}
		if (this.coins >= num)
		{
			this.coins -= num;
			this.SaveCoins();
			SceneManager.instance.isBeltExamReady = true;
			SceneManager.instance.StartGame();
		}
		else
		{
			MenuManager.instance.Open();
			MenuManager.instance.ChangePanel(2);
		}
	}

	public void OnClaimPressed()
	{
		if (SceneManager.instance.gettingItem)
		{
			return;
		}
		if (this.CheckForFreeGift())
		{
			if (SceneManager.instance.currentState == SceneManager.State.gameOver)
			{
				SceneManager.instance.previousState = SceneManager.instance.currentState;
				SceneManager.instance.currentState = SceneManager.State.gotItem;
				MenuUIController.instance.CloseGameOver();
				//SceneManager.instance.menu.Play("closeGameOver");
				int freeGift = this.GetFreeGift();
				StatManager.instance.stats[11].UpdateStat(StatManager.instance.stats[11].value + 1);
				StatManager.instance.UpdateStats();
				ItemManager.instance.GotCoin(freeGift);
				this.GetCoins(freeGift, true);
				if (this.coins >= 200)
				{
					this.willShowItem = true;
					if (!this.itemObject.activeInHierarchy)
					{
						this.itemObject.SetActive(true);
					}
				}
			}
		}
	}


	public void OnClaimWithCoinPressed()
	{
		if (SceneManager.instance.gettingItem)
		{
			return;
		}
		if (this.CheckForCoinGift())
		{
            if (SceneManager.instance.currentState == SceneManager.State.gameOver)
            {
                SceneManager.instance.previousState = SceneManager.instance.currentState;
                SceneManager.instance.currentState = SceneManager.State.gotItem;
                MenuUIController.instance.CloseGameOver();
                //SceneManager.instance.menu.Play("closeGameOver");
                StatManager.instance.stats[10].UpdateStat(StatManager.instance.stats[10].value + 200);
                StatManager.instance.UpdateStats();
                this.coins -= 200;
                this.SaveCoins();
                if (this.gotItemCount == 0)
                {
                    this.gotItemCount = ++this.gotItemCount;
                    PlayerPrefs.SetInt("gotItemCount", this.gotItemCount);
                    ItemManager.instance.GotItem(2, "viking", false, false);
                }
                else if (this.gotItemCount == 1)
                {
                    this.gotItemCount = ++this.gotItemCount;
                    PlayerPrefs.SetInt("gotItemCount", this.gotItemCount);
                    ItemManager.instance.GotItem(1, "sword1h0", false, true);
                }
                else if (this.gotItemCount == 2)
                {
                    this.gotItemCount = ++this.gotItemCount;
                    PlayerPrefs.SetInt("gotItemCount", this.gotItemCount);
                    ItemManager.instance.GotItem(1, "dagger2", false, false);
                }
                else
                {
                    ItemManager.instance.GotItem();
                }
            }
		}
	}

	public void OnWatchPressed()
	{
		if (SceneManager.instance.gettingItem)
		{
			return;
		}
		//if (AdsController.instance.rewardBasedVideo.IsLoaded())
		//{

  //          AdsController.instance.ShowRewardVideo();
  //          AnalyticsManager.instance.RewardedVideoStarted();
  //      } else if (Advertisement.IsReady())
  //      {
  //          this.ShowAd();
  //      }
	}

	public void GetCoins(int amt, bool save = false)
	{
		this.coins += amt;
		if (save)
		{
			this.SaveCoins();
		}
	}

	public void SaveCoins()
	{
        PlayerPrefs.SetInt("coins", this.coins);
	}

	public void ShowAd()
	{
		ItemManager.instance.loadingObject.SetActive(true);
        ShowOptions showOptions = new ShowOptions
        {
            resultCallback = new Action<ShowResult>(this.HandleAdResults)
        };
        Advertisement.Show(showOptions);
    }

	public void ShowInterstitial()
	{
		
		if (this.gameOverCount - this.interstitialShowCount <= 0)
		{
			return;
		}
		//Advertisement.Show("video");
	}

    private void HandleAdResults(ShowResult result)
    {
        ItemManager.instance.loadingObject.SetActive(false);
        if (result != ShowResult.Finished)
        {
            if (result != ShowResult.Skipped)
            {
                if (result != ShowResult.Failed)
                {
                }
            }
            else
            {
                AnalyticsManager.instance.RewardedVideoSkipped();
            }
        }
        else
        {
            MenuUIController.instance.CloseGameOver();
            //SceneManager.instance.menu.Play("closeGameOver");
            int num = this.watchToWinAmount;
            ItemManager.instance.GotCoin(num);
            this.GetCoins(num, true);
            AnalyticsManager.instance.RewardedVideoWatched();
        }
    }



	public void OnSharePressed()
	{
		//this.loadingObject.SetActive(true);
		//ShareSheet shareSheet = new ShareSheet();
		//shareSheet.Text = "火柴人战争：功夫战斗 Z";
		//shareSheet.AttachImage(ScreenshotManager.instance.screenshotTexture);
		//NPBinding.UI.SetPopoverPointAtLastTouchPosition();
  //      NPBinding.Sharing.ShowView(shareSheet, FinishedSharing);
    }

	//private void FinishedSharing(eShareResult _result)
	//{
	//	this.loadingObject.SetActive(false);
	//}

	//private void CreateNotification(DateTime fireTime, string notifText, eNotificationRepeatInterval _repeatInterval)
	//{
 //       IDictionary userInfo = new Dictionary<string, string>();
 //       CrossPlatformNotification.iOSSpecificProperties iOSSpecificProperties = new CrossPlatformNotification.iOSSpecificProperties();
 //       iOSSpecificProperties.BadgeCount = 1;
 //       CrossPlatformNotification.AndroidSpecificProperties androidSpecificProperties = new CrossPlatformNotification.AndroidSpecificProperties();
 //       androidSpecificProperties.ContentTitle = "SF:SW";
 //       androidSpecificProperties.TickerText = notifText;
 //       androidSpecificProperties.LargeIcon = "NativePlugins.png";
 //       CrossPlatformNotification crossPlatformNotification = new CrossPlatformNotification();
 //       crossPlatformNotification.AlertBody = notifText;
 //       crossPlatformNotification.FireDate = fireTime;
 //       crossPlatformNotification.RepeatInterval = _repeatInterval;
 //       crossPlatformNotification.UserInfo = userInfo;
 //       crossPlatformNotification.SoundName = "Notification.mp3";
 //       crossPlatformNotification.iOSProperties = iOSSpecificProperties;
 //       crossPlatformNotification.AndroidProperties = androidSpecificProperties;
 //       NPBinding.NotificationService.ScheduleLocalNotification(crossPlatformNotification);
 //   }

    //public void BuyRemoveAds()
    //{
    //    StoreManager.instance.BuyProductID("removeads");
    //}
}
