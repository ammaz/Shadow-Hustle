using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AirFishLab.ScrollingList;
using System;
using System.Text.RegularExpressions;

public class HatBuyMenu : MonoBehaviour
{
    #region Variables
    public GameObject[] HatPlayerDecoys;

    //For Player
    //It will make Player unvisible in hat buy menu
    public MeshRenderer Player;
    public GameObject PlayerHat;

    //Hat Count
    public Text itemCount;

    //Circullar Scrolling List
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
        for (int a = 0; a < HatPlayerDecoys.Length; a++)
        {
            HatPlayerDecoys[a].GetComponent<SpriteRenderer>().sprite = PlayerHat.GetComponent<SpriteRenderer>().sprite;
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
            //IM.verticalScrollSnapHat.PreviousScreen();
            IM.verticalScrollSnapHat.NextScreen();
        }
        else if (int.Parse(_centeredContentText.text) > content)
        {
            //IM.verticalScrollSnapHat.NextScreen();
            IM.verticalScrollSnapHat.PreviousScreen();
        }

        _centeredContentText.text = content + "";
    }

    public void OnHatButtonPressed(bool Check)
    {
        Player.enabled = Check;
        PlayerHat.GetComponent<SpriteRenderer>().enabled = Check;

        //Disabling Player Weapon Sprite
        foreach (SpriteRenderer clone in IM.weaponSpriteRenderers)
        {
            clone.enabled = Check;
        }
    }

    #endregion
}
