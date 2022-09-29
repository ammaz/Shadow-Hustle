#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using EpicPrefsTools;


namespace EpicPrefsEditorTools {
	#if UNITY_EDITOR
	public class EpicPrefsSetupWizard : ScriptableWizard {
		public static string Version = "2.1";
		private bool editPassKeys = false;
		private string passPhrase,initVector;
		private bool watchDogRunning = false;

		[MenuItem ("Tools/HotTotemAssets/EpicPrefs/Setup Wizard")]
		static void CreateWizard () {
			ScriptableWizard.DisplayWizard<EpicPrefsSetupWizard>("EpicPrefs Setup Wizard");
		}
		[MenuItem("Tools/HotTotemAssets/EpicPrefs/Show documentation...")]
		private static void showDocu()
		{
			#if UNITY_EDITOR_OSX
			System.Diagnostics.Process.Start(Application.dataPath + "/HotTotemAssets/EpicPrefs/Documentation/EpicPrefs Documentation.pdf");
			#else
			Application.OpenURL(Application.dataPath + "/HotTotemAssets/EpicPrefs/Documentation/EpicPrefs Documentation.pdf");
			#endif
		}
		[MenuItem("Tools/HotTotemAssets/EpicPrefs/Help...")]
		private static void help()
		{
			#if UNITY_EDITOR_OSX
			System.Diagnostics.Process.Start("mailto:support@hot-totem.com?subject=EpicPrefs Help");
			#else
			Application.OpenURL("mailto:support@hot-totem.com?subject=EpicPrefs Help");
			#endif
		}

		void OnEnable()
		{
			passPhrase = EpicPrefs.getPassPhrase();
			initVector = EpicPrefs.getInitVector();
			if ((GameObject.FindObjectOfType<EpicPrefsWatchDog> ()) != null) {
				watchDogRunning = true;
			}
		}
		void OnGUI()
		{
			if (!editPassKeys) {
				EditorGUILayout.HelpBox ("The encryption keys below are used for encrypting your prefs, so make sure these are unique.", MessageType.Info);
			} else {
				EditorGUILayout.HelpBox ("The initialization vector MUST be 16 bytes long.", MessageType.Warning);
			}
			EditorGUILayout.BeginHorizontal ();
			GUILayout.Label ("Encryption Passphrase :",GUILayout.ExpandWidth (false));
			if (!editPassKeys) {
				GUILayout.Box (passPhrase, GUILayout.ExpandWidth (true));
			} else {
				passPhrase = GUILayout.TextField (passPhrase);
			}
			EditorGUILayout.EndHorizontal ();
			EditorGUILayout.BeginHorizontal ();
			GUILayout.Label ("Initialization Vector :",GUILayout.ExpandWidth (false));
			if (!editPassKeys) {
				GUILayout.Box(initVector,GUILayout.ExpandWidth (true));
			} else {
				initVector = GUILayout.TextField (initVector);
			}
			EditorGUILayout.EndHorizontal ();
			if (editPassKeys) {
				if(GUILayout.Button("Apply",GUILayout.ExpandWidth (false))){
					if (System.Text.ASCIIEncoding.ASCII.GetByteCount(initVector) != 16)
					{
						if (EditorUtility.DisplayDialog("Error", "The initialization vector MUST be 16 bytes long.", "Ok, got it!"))
						{
							return;
						}
					}
					else {
						if (passPhrase != EpicPrefs.getPassPhrase () || initVector != EpicPrefs.getInitVector ()) {
							EpicPrefs.setPassPhrase(passPhrase);
							EpicPrefs.setInitVector(initVector);
							Operators.MigratePrefsToNewKey ();
							EpicPrefsEditor.passChanged = true;
						}
						editPassKeys = false;
					}
				}
			} else {
				if(GUILayout.Button("Edit keys",GUILayout.ExpandWidth (false))){
					if (EditorUtility.DisplayDialog ("Encryption change", "Are you sure you want to edit the AES encryption settings ? Doing " +
					    "so will migrate all your encrypted EpicPrefs, which can take some time depending on the amount of Prefs. You will need to re-export your EpicPrefs.", "Yes", "No")) {
						editPassKeys = true;
					}
				}
			}
			EditorGUILayout.HelpBox ("You can export EpicPrefs to release builds. This means every EpicPref you set at Editor time will also exist in your release build. To do so simply click the export button, once you added all your EpicPrefs.", MessageType.Info);
			if(GUILayout.Button("Export EpicPrefs to build.")){
				if (EditorUtility.DisplayDialog ("Export", "Every EpicPref in your Editor will be added to the game. In case of existing Prefs in the game with the same names, only those selected in the upcoming dialog will overwrite existing ones.", "Ok")) {
					EpicPrefsExportWizard.CreateWizard (); 
				}
			}
			if (watchDogRunning) {
				EditorGUI.LabelField (new Rect (10, 220, 220, 30), "Watchdog Status : Running");
			} else {
				EditorGUI.LabelField (new Rect (10, 220, 220, 30), "Watchdog Status : Offline");
			}
			EditorGUI.LabelField(new Rect(250,220,80,30),"Version " + Version);
		}
	}
	#endif
}