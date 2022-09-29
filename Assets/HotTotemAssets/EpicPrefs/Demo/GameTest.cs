using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class GameTest : MonoBehaviour {
	string Name,value,getName,getValue;
    Dictionary<string,string> getDict;
	bool encryption;
	void Awake(){
		Name = "";
		value = "";
		getName = "";
		getValue = "";
		encryption = false; 
		EpicPrefs.setBatchMode (true);
		EpicPrefs.HasKey ("PrefName", false, EpicPrefs.Types.String);
		EpicPrefs.Save ();
    }
	void OnGUI(){
		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Key : ");
		Name = GUILayout.TextField (Name,GUILayout.Width(250));
		GUILayout.Label ("Value : ");
		value = GUILayout.TextField (value,GUILayout.Width(250));
		encryption = GUILayout.Toggle (encryption,"AES Encryption : ");
        if (GUILayout.Button ("Add"))
			EpicPrefs.SetString (Name, value,encryption);
		GUILayout.EndHorizontal ();
		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Key : ");
		getName = GUILayout.TextField (getName,GUILayout.Width(250));
		GUILayout.Label ("Value : ");
		GUILayout.Label (getValue);
		encryption = GUILayout.Toggle (encryption,"AES Encryption : ");
		if (GUILayout.Button ("Get"))
			getValue = EpicPrefs.GetString (getName, encryption);
		GUILayout.EndHorizontal ();
        
        GUILayout.BeginHorizontal ();
		GUILayout.Label ("DictionaryName : ");
		getName = GUILayout.TextField (getName,GUILayout.Width(250));
		encryption = GUILayout.Toggle (encryption,"AES Encryption : ");
		if (GUILayout.Button ("Get")){
            getDict = EpicPrefs.GetDictStringString(getName,encryption);  
        }
		GUILayout.EndHorizontal ();
        if(getDict != null){
                GUILayout.BeginVertical();
                foreach(KeyValuePair<string,string> pair in getDict)
                     GUILayout.Label ("Key : " + pair.Key + "   Value : " + pair.Value);
                GUILayout.EndVertical();
        }
		if (GUILayout.Button ("BatchTest")){
			for (int i = 0; i < 50; i++)
				EpicPrefs.SetString (i.ToString () + "Pref", i.ToString ());
		}     
	}
}
