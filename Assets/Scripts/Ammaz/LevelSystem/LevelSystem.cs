using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSystem : MonoBehaviour
{
    #region Variables

    public GameObject MissionHud;
    public GameObject LevelMenu;

    public Button[] LevelBtn;

    public static LevelSystem instance;

    #endregion
    
    #region Singleton



    #endregion

    #region Unity Methods
    
    void Start()
    {
        LevelSystem.instance = this;
    }

    
    void Update()
    {
        
    }

    #endregion

    #region Custom Methods

    public void GenerateLevel(int levelNumber)
    {
        LevelMenu.SetActive(false);
        MissionHud.SetActive(true);
        SceneManager.instance.StartGame();
        MissionManager.instance.OnGameStart();

        switch (levelNumber)
        {
            case 1:
                MissionManager.instance.MissionName = "WHITE BELT MISSIONS";
                break;
            case 2:
                MissionManager.instance.MissionName = "Yellow BELT MISSIONS";
                break;
            case 3:
                MissionManager.instance.MissionName = "Yellow Black BELT MISSIONS";
                break;
            case 4:
                MissionManager.instance.MissionName = "Green BELT MISSIONS";
                break;
            case 5:
                MissionManager.instance.MissionName = "Green Black BELT MISSIONS";
                break;
            case 6:
                MissionManager.instance.MissionName = "Purple BELT MISSIONS";
                break;
            case 7:
                MissionManager.instance.MissionName = "Purple Black BELT MISSIONS";
                break;
            case 8:
                MissionManager.instance.MissionName = "Orange BELT MISSIONS";
                break;
            case 9:
                MissionManager.instance.MissionName = "Orange Black BELT MISSIONS";
                break;
            case 10:
                MissionManager.instance.MissionName = "Blue BELT MISSIONS";
                break;
            case 11:
                MissionManager.instance.MissionName = "Blue Black BELT MISSIONS";
                break;
            case 12:
                MissionManager.instance.MissionName = "Brown BELT MISSIONS";
                break;
            case 13:
                MissionManager.instance.MissionName = "Brow Black BELT MISSIONS";
                break;
            case 14:
                MissionManager.instance.MissionName = "Red BELT MISSIONS";
                break;
            case 15:
                MissionManager.instance.MissionName = "Red Black BELT MISSIONS";
                break;
            case 16:
                MissionManager.instance.MissionName = "Black BELT MISSIONS";
                break;
        }
    }

    public void CompletedorLockedLevel()
    {
        switch (MissionManager.instance.CurrentLevelProgress)
        {
            case "WHITE BELT MISSIONS":
                levelUnlockandLock(0);
                break;
            case "YELLOW BELT MISSIONS":
                levelUnlockandLock(1);
                break;
            case "YELLOW BLACK BELT MISSIONS":
                levelUnlockandLock(2);
                break;
            case "GREEN BELT MISSIONS":
                levelUnlockandLock(3);
                break;
            case "GREEN BLACK BELT MISSIONS":
                levelUnlockandLock(4);
                break;
            case "PURPLE BELT MISSIONS":
                levelUnlockandLock(5);
                break;
            case "PURPLE BLACK BELT MISSIONS":
                levelUnlockandLock(6);
                break;
            case "ORANGE BELT MISSIONS":
                levelUnlockandLock(7);
                break;
            case "ORANGE BLACK BELT MISSIONS":
                levelUnlockandLock(8);
                break;
            case "BLUE BELT MISSIONS":
                levelUnlockandLock(9);
                break;
            case "BLUE BLACK BELT MISSIONS":
                levelUnlockandLock(10);
                break;
            case "BROWN BELT MISSIONS":
                levelUnlockandLock(11);
                break;
            case "BROWN BLACK BELT MISSIONS":
                levelUnlockandLock(12);
                break;
            case "RED BELT MISSIONS":
                levelUnlockandLock(13);
                break;
            case "RED BLACK BELT MISSIONS":
                levelUnlockandLock(14);
                break;
            case "BLACK BELT MISSIONS":
                levelUnlockandLock(15);
                break;
        }
    }

    private void levelUnlockandLock(int levelNumber)
    {
        //Unlocking Level
        LevelBtn[levelNumber].interactable = true;
        LevelBtn[levelNumber].GetComponentInChildren<Text>().text = "";
        //Mission Completed
        for (int a = 0; a < levelNumber; a++)
        {
            LevelBtn[a].interactable = false;
            LevelBtn[a].GetComponentInChildren<Text>().text = "Completed";
        }
        //Mission Locked
        for (int a = levelNumber+1; a < LevelBtn.Length; a++)
        {
            LevelBtn[a].interactable = false;
            LevelBtn[a].GetComponentInChildren<Text>().text = "Locked";
        }
    }

    #endregion
}
