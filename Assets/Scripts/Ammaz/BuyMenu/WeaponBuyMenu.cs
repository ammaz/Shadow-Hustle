using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AirFishLab.ScrollingList;

public class WeaponBuyMenu : MonoBehaviour
{
    #region Variables

    public SpriteRenderer[] PlayerDecoy1;
    public SpriteRenderer[] PlayerDecoy2;
    public SpriteRenderer[] PlayerDecoy3;

    //For Player
    //It will make Player unvisible in weapon buy menu
    public MeshRenderer Player;
    public SpriteRenderer PlayerSprite;
    public SpriteRenderer[] PlayerWeaponSprites;

    //Player Weapon Holder Objects
    public GameObject[] PlayerWpnHolderObjects;

    //Hat Count
    public Text itemCount;

    //asd
    //public ListPositionCtrl listPositionC;
    [SerializeField]
    private CircularScrollingList _list;
    [SerializeField]
    private Text _centeredContentText;

    //Item Manager Reference
    public ItemManager IM;

    #endregion

    #region Singleton
    #endregion

    #region Unity Methods

    void Start()
    {
        
    }

    
    void Update()
    {
        for (int a = 0; a < PlayerDecoy1.Length; a++)
        {
            if (PlayerWpnHolderObjects[a].activeSelf)
            {
                PlayerDecoy1[a].enabled = true;
                PlayerDecoy1[a].sprite = PlayerWeaponSprites[a].sprite;
            }
            else
            {
                PlayerDecoy1[a].enabled = false;
            }
            
        }
        for (int a = 0; a < PlayerDecoy2.Length; a++)
        {
            if (PlayerWpnHolderObjects[a].activeSelf)
            {
                PlayerDecoy2[a].enabled = true;
                PlayerDecoy2[a].sprite = PlayerWeaponSprites[a].sprite;
            }
            else
            {
                PlayerDecoy2[a].enabled = false;
            }
        }
        for (int a = 0; a < PlayerDecoy3.Length; a++)
        {
            if (PlayerWpnHolderObjects[a].activeSelf)
            {
                PlayerDecoy3[a].enabled = true;
                PlayerDecoy3[a].sprite = PlayerWeaponSprites[a].sprite;
            }
            else
            {
                PlayerDecoy3[a].enabled = false;
            }
        }
    }

    #endregion

    #region Custom Methods

    public void OnListCenteredContentChanged(int centeredContentID)
    {
        int content = (int)_list.listBank.GetListContent(centeredContentID);

        //Changing player hats
        if (int.Parse(_centeredContentText.text) < content)
        {
            //IM.verticalScrollSnapHat.NextScreen();
            IM.verticalScrollSnapEquipment.NextScreen();
        }
        else if (int.Parse(_centeredContentText.text) > content)
        {
            //IM.verticalScrollSnapHat.PreviousScreen();
            IM.verticalScrollSnapEquipment.PreviousScreen();
        }

        _centeredContentText.text = content + "";
    }

    public void OnWeaponButtonPressed(bool Check)
    {
        Player.enabled = Check;
        PlayerSprite.enabled = Check;

        //Disabling Player Weapon Sprite
        foreach (SpriteRenderer clone in PlayerWeaponSprites)
        {
            clone.enabled = Check;
        }
    }

    #endregion
}
