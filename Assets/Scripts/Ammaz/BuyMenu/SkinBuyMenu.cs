using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AirFishLab.ScrollingList;

public class SkinBuyMenu : MonoBehaviour
{
    #region Variables

    //Player Skin
    public Material PlayerSkinColor;

    //Skin Colors
    private Color[] Colors;
    //Color Index
    public int colorIndex;

    //Circullar Scrolling List
    [SerializeField]
    private CircularScrollingList _list;
    [SerializeField]
    private Text _centeredContentText;

    //Text Alpha
    public Text skinCountText;

    #endregion

    #region Singleton
    #endregion

    #region Unity Methods

    void Start()
    {
        //Declaring Colors
        Colors = new Color[10]; 
        Colors[0] = Color.gray;
        //White
        Colors[1] = new Color(255,255,255, 1f);
        Colors[2] = Color.red;
        Colors[3] = Color.green;
        Colors[4] = Color.blue;
        //Black
        Colors[5] = Color.black;
        //Maroon
        Colors[6] = new Color(128, 0, 0, 1f);
        //Teal
        Colors[7] = new Color(0, 128, 128, 1f);
        //Magenta
        Colors[8] = new Color(255, 0, 255, 1f);
        //Yellow
        Colors[9] = Color.yellow;

        colorIndex = 0;

        //PlayerSkinColor.SetColor("_Color", Color.blue);
    }

    
    void Update()
    {
        
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
            if (colorIndex != 9)
            {
                colorIndex++;
            }

            SetSkinColor(Colors[colorIndex]);
        }
        else if (int.Parse(_centeredContentText.text) > content)
        {
            //IM.verticalScrollSnapHat.PreviousScreen();
            if (colorIndex != 0)
            {
                colorIndex--;
            }
            
            SetSkinColor(Colors[colorIndex]);
        }

        _centeredContentText.text = content + "";
    }

    public void SetSkinColor(Color colour)
    {
        PlayerSkinColor.SetColor("_Color", colour);
    }

    public void OnSkinMenuOpen()
    {
        skinCountText.enabled = false;
    }

    public void OnSkinMenuClose()
    {
        skinCountText.enabled = true;
    }

    #endregion
}
