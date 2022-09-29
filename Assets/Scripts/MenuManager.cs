// Decompile from assembly: Assembly-CSharp.dll
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;
using System.Collections;
using Com.LuisPedroFonseca.ProCamera2D;

public class MenuManager : MonoBehaviour
{
	public static MenuManager instance;

	public bool isOpen;

	public bool isPaused;

	public bool isCounting;

	public CanvasGroup menuCanvas;

	public BlurOptimized blur;

	public CanvasGroup settingsCanvas;

	public CanvasGroup statsCanvas;

	//public CanvasGroup creditsCanvas;

	private int currentPanelIndex;

	public MenuItemPanel[] panels;


	public Text pauseCountdownText;

	public GameObject continueButton;

	public CanvasGroup pauseMenuCanvasGroup;

	public CanvasGroup pauseButtonCanvasGroup;

	private float pauseEndTime;

	[Header("Main Menu Buttons")]
	public GameObject[] MenuButtons;

	[Header("Revive Panel")]
	public GameObject revivePanel;

	protected void Awake()
	{
		this.isCounting = false;
		MenuManager.instance = this;
	}

	private void Start()
	{
		this.currentPanelIndex = 0;
		this.panels[0].isOpen = true;
	}

	private void Update()
	{
		Helpers.UpdateCanvasVisibilty(this.menuCanvas, 2f, this.isOpen, true);
		Helpers.UpdateCanvasVisibilty(this.pauseButtonCanvasGroup, 2f, SceneManager.instance.gameStarted && (SceneManager.instance.isEndless || StoryManager.instance.introCompleted), false);
		Helpers.UpdateCanvasVisibilty(this.pauseMenuCanvasGroup, 2f, this.isPaused, true);
		if (this.isOpen)
		{
			MenuItemPanel[] array = this.panels;
			for (int i = 0; i < array.Length; i++)
			{
				MenuItemPanel menuItemPanel = array[i];
				Helpers.UpdateCanvasVisibilty(menuItemPanel.canvasGroup, 2f, menuItemPanel.isOpen, true);
				menuItemPanel.SetColor();
			}
		}
		if (this.isOpen)
		{
			if (!this.blur.enabled)
			{
				this.blur.blurSize = 0f;
				this.blur.enabled = true;
			}
			else if (this.blur.blurSize < 10f)
			{
				this.blur.blurSize += Time.unscaledDeltaTime * 20f;
			}
			else if (this.blur.blurSize != 10f)
			{
				this.blur.blurSize = 10f;
			}
		}
        else if ( this.blur.enabled)
        {
            if (this.blur.blurSize > 0f)
            {
                this.blur.blurSize -= Time.unscaledDeltaTime * 20f;
            }
            else
            {
                this.blur.blurSize = 0f;
                this.blur.enabled = false;
            }
        }
        if (!this.isPaused && this.isCounting)
		{
			int num = 3 - (int)(Time.unscaledTime - this.pauseEndTime);
			if (num <= 0)
			{
				this.isCounting = false;
				Time.timeScale = 1f;
				this.pauseCountdownText.gameObject.SetActive(false);
			}
			else
			{
				this.pauseCountdownText.text = num.ToString();
			}
		}
	}

	public void Pause()
	{
		this.isPaused = true;
		this.isCounting = false;
		this.continueButton.SetActive(true);
		this.pauseCountdownText.gameObject.SetActive(false);
		MissionManager.instance.missionFadeTime = 0.0;
		MissionManager.instance.SetColors(Color.white, -1);
		Time.timeScale = 0f;
	}

	public void Unpause()
	{
		this.isPaused = false;
		this.isCounting = true;
		MissionManager.instance.missionFadeTime = (double)(Time.time + 3f);
		MissionManager.instance.SetColors(Color.black, -1);
		this.pauseEndTime = Time.unscaledTime;
		this.continueButton.SetActive(false);
		this.pauseCountdownText.gameObject.SetActive(true);
	}

	public IEnumerator ReviveOpen()
    {
		//Enabling Revive Panel
		revivePanel.SetActive(true);

		yield return new WaitForSeconds(2f);

		//For Timer
		this.isCounting = true;
		this.pauseEndTime = Time.unscaledTime;
		this.pauseCountdownText.gameObject.SetActive(true);
	}

	public void ReviveClose()
    {
		//Disabling Revive Panel
		revivePanel.SetActive(false);
		this.pauseCountdownText.gameObject.SetActive(false);
	}

	public void GotoHome()
    {
		//MenuUIController.instance.OpenMenu();
		SceneManager.instance.ChangeState(SceneManager.State.trainArrived);
		//this.menu.Play("menuOpen");
		MenuUIController.instance.OpenMenu();
		MenuUIController.instance.ShowLogo();

		//ItemManager.instance.CloseItemMenu();

		ItemManager.instance.CheckIfCurrentItemsOwned();
		ItemManager.instance.EquipCurrentWeapon(true);
		ItemManager.instance.EquipCurrentHat(true);
		ItemManager.instance.itemMenuOpen = false;
		SceneManager.instance.chaserObject.transform.localPosition = new Vector3(0f, -0.8f, 0f);
		NpcManager.instance.ContinueAll();
		SceneManager.instance.targetCameraZ = -9f;
		Camera.main.GetComponent<ProCamera2D>().OverallOffset.x = 0f;
		Camera.main.GetComponent<ProCamera2D>().OverallOffset.y = 0f;

		ItemManager.instance.SurvivalBtn.SetActive(true);
		ItemManager.instance.CampaignBtn.SetActive(true);

		if (!SceneManager.instance.gettingItem)
		{
			if (SceneManager.instance.currentState == SceneManager.State.gameOver)
			{
				//SceneManager.instance.menu.Play("menuGameOver");
				MenuUIController.instance.OpenGameOver();
			}
			else
			{
				//SceneManager.instance.headingAnim.Play("headingStart");
				MenuUIController.instance.ShowLogo();
				MenuUIController.instance.OpenMenu();
				//SceneManager.instance.menu.Play("menuOpen");
			}
		}

		foreach(GameObject btn in MenuButtons)
        {
			btn.SetActive(true);
        }

	}

	public void Open()
	{
		if (this.currentPanelIndex == 2)
		{
			AnalyticsManager.instance.ShopPressed();
		}
		this.isOpen = true;
        //AdsController.instance.HideBanner();
	}

	public void Close()
	{
		this.isOpen = false;
        //AdsController.instance.VisibleBanner();
    }

	public void ChangePanel(int panelId)
	{
		this.panels[this.currentPanelIndex].isOpen = false;
		this.currentPanelIndex = panelId;
		this.panels[this.currentPanelIndex].isOpen = true;
	}

	protected void OnApplicationFocus(bool focus)
	{
		//if (this.isPaused)
		//{
		//	return;
		//}
		//if (!focus && SceneManager.instance.gameStarted && (SceneManager.instance.isEndless || StoryManager.instance.introCompleted))
		//{
		//	this.Pause();
		//}
	}

	protected void OnApplicationPause(bool pause)
	{
		//if (this.isPaused)
		//{
		//	return;
		//}
		//if (pause && SceneManager.instance.gameStarted && (SceneManager.instance.isEndless || StoryManager.instance.introCompleted))
		//{
		//	this.Pause();
		//}
	}
}
