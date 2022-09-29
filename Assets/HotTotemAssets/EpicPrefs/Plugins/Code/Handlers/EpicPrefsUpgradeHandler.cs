using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;


#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace EpicPrefsTools
{

	#if UNITY_EDITOR
	[InitializeOnLoad]
	public static class EpicPrefsUpgradeHandler
	{
		static Dictionary<string, int> IntegerDictionary;
		static Dictionary<string, float> FloatDictionary;
		static Dictionary<string, bool> BooleanDictionary;
		static Dictionary<string, string> StringDictionary;
		static Dictionary<string, double> DoubleDictionary;
		static Dictionary<string, long> LongDictionary;
		static Dictionary<string, int[]> IntegerArrayDictionary;
		static Dictionary<string, string[]> StringArrayDictionary;
		static Dictionary<string, float[]> FloatArrayDictionary;
		static Dictionary<string, double[]> DoubleArrayDictionary;
		static Dictionary<string, List<string>> StringListDictionary;
		static Dictionary<string, List<int>> IntegerListDictionary;
		static Dictionary<string, List<bool>> BooleanListDictionary;
		static Dictionary<string, List<float>> FloatListDictionary;
		static Dictionary<string, List<double>> DoubleListDictionary;
		static Dictionary<string, List<long>> LongListDictionary;
		static Dictionary<string, Dictionary<string,string>> StringDictionaryDictionary;
		static Dictionary<string, Dictionary<string,int>> IntegerDictionaryDictionary;
		static Dictionary<string, Dictionary<string,bool>> BooleanDictionaryDictionary;
		static Dictionary<string, Dictionary<string,float>> FloatDictionaryDictionary;
		static Dictionary<string, Dictionary<string,double>> DoubleDictionaryDictionary;
		static Dictionary<string, Dictionary<string,long>> LongDictionaryDictionary;
		static Dictionary<string, Quaternion> QuaternionDictionary;
		static Dictionary<string, Vector2> Vector2Dictionary;
		static Dictionary<string, Vector3> Vector3Dictionary;
		static Dictionary<string, Vector4> Vector4Dictionary;
		static Dictionary<string, Color> ColorDictionary;
		static string filePath;
		static bool encrypted;
		static EpicPrefsUpgradeHandler ()
		{
			if (!File.Exists (Application.dataPath + "/UpdateInitializerBlockingFile.rtf")) {
				if (Directory.Exists (Application.dataPath + "/HotTotemAssets/EpicPrefs/EditorScripts/Graphics")) {
					Directory.Delete (Application.dataPath + "/HotTotemAssets/EpicPrefs/EditorScripts/Graphics", true);
					File.Delete (Application.dataPath + "/HotTotemAssets/EpicPrefs/EditorScripts/Graphics.meta");
				}
				if (File.Exists (Application.dataPath + "/HotTotemAssets/EpicPrefs/EpicEditorPrefs/NotEncrypted/IntegerDictionary.epic")) {
					if (EditorUtility.DisplayDialog ("EpicPrefs - Migrating", "The new update introduces a whole new serialization method. We will now start migrating your previous EpicPrefs", "Ok, go ahead")) {
						EditorUtility.DisplayProgressBar ("Migrating", "Letting monkeys do the work...", 0f);
						filePath = Application.dataPath + "/HotTotemAssets/EpicPrefs/EpicEditorPrefs/Encrypted/";
						Directory.CreateDirectory (Application.dataPath + "/HotTotemAssets/EpicPrefs/SafeToDelete/EpicEditorPrefs/Encrypted/");
						encrypted = true;
						LoadIntegerDictionary ();
						LoadFloatDictionary ();
						LoadBooleanDictionary ();
						LoadStringDictionary ();
						LoadDoubleDictionary ();
						LoadLongDictionary ();
						EditorUtility.DisplayProgressBar ("Migrating", "Letting monkeys do the work...", 0.2f);
						LoadIntegerArrayDictionary ();
						LoadStringArrayDictionary ();
						LoadFloatArrayDictionary ();
						LoadDoubleArrayDictionary ();
						LoadStringListDictionary ();
						LoadIntegerListDictionary ();
						LoadBooleanListDictionary ();
						LoadFloatListDictionary ();
						LoadDoubleListDictionary ();
						LoadLongListDictionary ();
						LoadStringDictionaryDictionary ();
						LoadIntegerDictionaryDictionary ();
						EditorUtility.DisplayProgressBar ("Migrating", "Letting monkeys do the work...", 0.4f);
						LoadFloatDictionaryDictionary ();
						LoadDoubleDictionaryDictionary ();
						LoadLongDictionaryDictionary ();
						LoadBooleanDictionaryDictionary ();
						LoadQuaternionDictionary ();
						LoadVector2Dictionary ();
						LoadVector3Dictionary ();
						LoadVector4Dictionary ();
						LoadColorDictionary ();
						filePath = Application.dataPath + "/HotTotemAssets/EpicPrefs/EpicEditorPrefs/NotEncrypted/";
						Directory.CreateDirectory (Application.dataPath + "/HotTotemAssets/EpicPrefs/SafeToDelete/EpicEditorPrefs/NotEncrypted/");
						EditorUtility.DisplayProgressBar ("Migrating", "Letting monkeys do the work...", 0.5f);
						encrypted = false;
						LoadIntegerDictionary ();
						LoadFloatDictionary ();
						LoadBooleanDictionary ();
						LoadStringDictionary ();
						LoadDoubleDictionary ();
						LoadLongDictionary ();
						LoadIntegerArrayDictionary ();
						LoadStringArrayDictionary ();
						LoadFloatArrayDictionary ();
						LoadDoubleArrayDictionary ();
						LoadStringListDictionary ();
						LoadIntegerListDictionary ();
						EditorUtility.DisplayProgressBar ("Migrating", "Letting monkeys do the work...", 0.6f);
						LoadBooleanListDictionary ();
						LoadFloatListDictionary ();
						LoadDoubleListDictionary ();
						LoadLongListDictionary ();
						LoadStringDictionaryDictionary ();
						LoadIntegerDictionaryDictionary ();
						LoadFloatDictionaryDictionary ();
						LoadDoubleDictionaryDictionary ();
						LoadLongDictionaryDictionary ();
						EditorUtility.DisplayProgressBar ("Migrating", "Letting monkeys do the work...", 0.8f);
						LoadBooleanDictionaryDictionary ();
						LoadQuaternionDictionary ();
						LoadVector2Dictionary ();
						LoadVector3Dictionary ();
						LoadVector4Dictionary ();
						LoadColorDictionary ();
						var thisAsset = AssetDatabase.FindAssets ("EpicPrefsUpgradeHandler", null);
						var m_ScriptFilePath = AssetDatabase.GUIDToAssetPath (thisAsset [0]);
						var m_ScriptFolder = "";
						FileInfo fi = new FileInfo (m_ScriptFilePath);
						EditorUtility.DisplayProgressBar ("Migrating", "Letting monkeys do the work...", 0.9f);
						m_ScriptFolder = fi.Directory.ToString ();
						var currentFolder = m_ScriptFolder.Replace ('\\', '/');
						var parentDirectory = Directory.GetParent (currentFolder);
						var currentFile = Path.Combine (parentDirectory.ToString (), "Handlers/") + "EpicPrefsUpgradeHandler.cs";
						string[] arrLine = File.ReadAllLines (currentFile);
						for (int i = 0; i < arrLine.Length; i++) {
							arrLine [i] = "//" + arrLine [i];
						}
						EditorUtility.DisplayProgressBar ("Migrating", "Letting monkeys do the work...", 1f);
						EditorUtility.ClearProgressBar ();
						if (EditorUtility.DisplayDialog ("EpicPrefs - Migration done!", "We finished mirgrating your EpicPrefs. Please check the EpicPrefsEditor if everything is in place! If everything looks fine, you can delete the folder HotTotemAssets/EpicPrefs/SafeToDelete ", "Alright!")) {					
							File.WriteAllLines (currentFile, arrLine);
						}
						AssetDatabase.Refresh ();
					}
				}
			}
		}
		//Save&Load IntegerDictionary
		static void LoadIntegerDictionary ()
		{
			IntegerDictionary = new Dictionary<string, int> ();
			FileStream fs;
			if (File.Exists (filePath + "IntegerDictionary.epic")) {
				fs = new FileStream (filePath + "IntegerDictionary.epic", FileMode.Open);
				if (fs.Length == 0) {
					fs.Close ();
					File.Move (fs.Name, fs.Name.Replace ("EpicEditorPrefs", "SafeToDelete/EpicEditorPrefs"));
					File.Delete (fs.Name.Replace (".epic", ".meta"));
					return;
				}
				BinaryFormatter bf = new BinaryFormatter ();
				try {
					var obj = bf.Deserialize (fs);
					IntegerDictionary = (Dictionary<string,int>)obj;
				} catch (Exception e) {
					Debug.LogError ("EpicPrefs Error : " + e);
				}
				fs.Close ();
				File.Move (fs.Name, fs.Name.Replace ("EpicEditorPrefs", "SafeToDelete/EpicEditorPrefs"));
				File.Delete (fs.Name.Replace (".epic", ".meta"));
			}
			foreach (var val in IntegerDictionary) {
				EpicPrefs.SetInt (val.Key, val.Value, encrypted);
			}
		}

		//Save&Load FloatDictionary
		static void LoadFloatDictionary ()
		{
			FloatDictionary = new Dictionary<string, float> ();
			FileStream fs;
			if (File.Exists (filePath + "FloatDictionary.epic")) {
				fs = new FileStream (filePath + "FloatDictionary.epic", FileMode.Open);
				if (fs.Length == 0) {
					fs.Close ();
					File.Move (fs.Name, fs.Name.Replace ("EpicEditorPrefs", "SafeToDelete/EpicEditorPrefs"));
					File.Delete (fs.Name.Replace (".epic", ".meta"));
					return;
				}
				BinaryFormatter bf = new BinaryFormatter ();
				try {
					var obj = bf.Deserialize (fs);
					FloatDictionary = (Dictionary<string,float>)obj;
				} catch (Exception e) {
					Debug.LogError ("EpicPrefs Error : " + e);
				}
				fs.Close ();
				File.Move (fs.Name, fs.Name.Replace ("EpicEditorPrefs", "SafeToDelete/EpicEditorPrefs"));
				File.Delete (fs.Name.Replace (".epic", ".meta"));
			}
			foreach (var val in FloatDictionary) {
				EpicPrefs.SetFloat (val.Key, val.Value, encrypted);
			}
		}

		//Save&Load BooleanDictionary
		static void LoadBooleanDictionary ()
		{
			BooleanDictionary = new Dictionary<string, bool> ();
			FileStream fs;
			if (File.Exists (filePath + "BooleanDictionary.epic")) {
				fs = new FileStream (filePath + "BooleanDictionary.epic", FileMode.Open);
				if (fs.Length == 0) {
					fs.Close ();
					File.Move (fs.Name, fs.Name.Replace ("EpicEditorPrefs", "SafeToDelete/EpicEditorPrefs"));
					File.Delete (fs.Name.Replace (".epic", ".meta"));
					return;
				}
				BinaryFormatter bf = new BinaryFormatter ();				
				try {
					var obj = bf.Deserialize (fs);
					BooleanDictionary = (Dictionary<string,bool>)obj;
				} catch (Exception e) {
					Debug.LogError ("EpicPrefs Error : " + e);
				}
				fs.Close ();
				File.Move (fs.Name, fs.Name.Replace ("EpicEditorPrefs", "SafeToDelete/EpicEditorPrefs"));
				File.Delete (fs.Name.Replace (".epic", ".meta"));
			}
			foreach (var val in BooleanDictionary) {
				EpicPrefs.SetBool (val.Key, val.Value, encrypted);
			}
		}

		//Save&Load StringDictionary
		static void LoadStringDictionary ()
		{
			StringDictionary = new Dictionary<string, string> ();
			FileStream fs;
			if (File.Exists (filePath + "StringDictionary.epic")) {
				fs = new FileStream (filePath + "StringDictionary.epic", FileMode.Open);
				if (fs.Length == 0) {
					fs.Close ();
					File.Move (fs.Name, fs.Name.Replace ("EpicEditorPrefs", "SafeToDelete/EpicEditorPrefs"));
					File.Delete (fs.Name.Replace (".epic", ".meta"));
					return;
				}
				BinaryFormatter bf = new BinaryFormatter ();				
				try {
					var obj = bf.Deserialize (fs);
					StringDictionary = (Dictionary<string,string>)obj;
				} catch (Exception e) {
					Debug.LogError ("EpicPrefs Error : " + e);
				}
				fs.Close ();
				File.Move (fs.Name, fs.Name.Replace ("EpicEditorPrefs", "SafeToDelete/EpicEditorPrefs"));
				File.Delete (fs.Name.Replace (".epic", ".meta"));
			}
			foreach (var val in StringDictionary) {
				EpicPrefs.SetString (val.Key, val.Value, encrypted);
			}
		}

		//Save&Load DoubleDictionary
		static void LoadDoubleDictionary ()
		{
			DoubleDictionary = new Dictionary<string, double> ();
			FileStream fs;
			if (File.Exists (filePath + "DoubleDictionary.epic")) {
				fs = new FileStream (filePath + "DoubleDictionary.epic", FileMode.Open);
				if (fs.Length == 0) {
					fs.Close ();
					File.Move (fs.Name, fs.Name.Replace ("EpicEditorPrefs", "SafeToDelete/EpicEditorPrefs"));
					File.Delete (fs.Name.Replace (".epic", ".meta"));
					return;
				}
				BinaryFormatter bf = new BinaryFormatter ();				
				try {
					var obj = bf.Deserialize (fs);
					DoubleDictionary = (Dictionary<string,double>)obj;
				} catch (Exception e) {
					Debug.LogError ("EpicPrefs Error : " + e);
				}
				fs.Close ();
				File.Move (fs.Name, fs.Name.Replace ("EpicEditorPrefs", "SafeToDelete/EpicEditorPrefs"));
				File.Delete (fs.Name.Replace (".epic", ".meta"));
			}
			foreach (var val in DoubleDictionary) {
				EpicPrefs.SetDouble (val.Key, val.Value, encrypted);
			}
		}

		//Save&Load LongDictionary
		static void LoadLongDictionary ()
		{
			LongDictionary = new Dictionary<string, long> ();
			FileStream fs;
			if (File.Exists (filePath + "LongDictionary.epic")) {
				fs = new FileStream (filePath + "LongDictionary.epic", FileMode.Open);
				if (fs.Length == 0) {
					fs.Close ();
					File.Move (fs.Name, fs.Name.Replace ("EpicEditorPrefs", "SafeToDelete/EpicEditorPrefs"));
					File.Delete (fs.Name.Replace (".epic", ".meta"));
					return;
				}
				BinaryFormatter bf = new BinaryFormatter ();				
				try {
					var obj = bf.Deserialize (fs);
					LongDictionary = (Dictionary<string,long>)obj;
				} catch (Exception e) {
					Debug.LogError ("EpicPrefs Error : " + e);
				}
				fs.Close ();
				File.Move (fs.Name, fs.Name.Replace ("EpicEditorPrefs", "SafeToDelete/EpicEditorPrefs"));
				File.Delete (fs.Name.Replace (".epic", ".meta"));
			}
			foreach (var val in LongDictionary) {
				EpicPrefs.SetLong (val.Key, val.Value, encrypted);
			}
		}

		//Save&Load IntegerArrayDictionary
		static void LoadIntegerArrayDictionary ()
		{
			IntegerArrayDictionary = new Dictionary<string, int[]> ();
			FileStream fs;
			if (File.Exists (filePath + "IntegerArrayDictionary.epic")) {
				fs = new FileStream (filePath + "IntegerArrayDictionary.epic", FileMode.Open);
				if (fs.Length == 0) {
					fs.Close ();
					File.Move (fs.Name, fs.Name.Replace ("EpicEditorPrefs", "SafeToDelete/EpicEditorPrefs"));
					File.Delete (fs.Name.Replace (".epic", ".meta"));
					return;
				}
				BinaryFormatter bf = new BinaryFormatter ();				
				try {
					var obj = bf.Deserialize (fs);
					IntegerArrayDictionary = (Dictionary<string,int[]>)obj;
				} catch (Exception e) {
					Debug.LogError ("EpicPrefs Error : " + e);
				}
				fs.Close ();
				File.Move (fs.Name, fs.Name.Replace ("EpicEditorPrefs", "SafeToDelete/EpicEditorPrefs"));
				File.Delete (fs.Name.Replace (".epic", ".meta"));
			}
			foreach (var val in IntegerArrayDictionary) {
				EpicPrefs.SetArray (val.Key, val.Value, encrypted);
			}
		}

		//Save&Load StringArrayDictionary
		static void LoadStringArrayDictionary ()
		{
			StringArrayDictionary = new Dictionary<string, string[]> ();
			FileStream fs;
			if (File.Exists (filePath + "StringArrayDictionary.epic")) {
				fs = new FileStream (filePath + "StringArrayDictionary.epic", FileMode.Open);
				if (fs.Length == 0) {
					fs.Close ();
					File.Move (fs.Name, fs.Name.Replace ("EpicEditorPrefs", "SafeToDelete/EpicEditorPrefs"));
					File.Delete (fs.Name.Replace (".epic", ".meta"));
					return;
				}
				BinaryFormatter bf = new BinaryFormatter ();				
				try {
					var obj = bf.Deserialize (fs);
					StringArrayDictionary = (Dictionary<string,string[]>)obj;
				} catch (Exception e) {
					Debug.LogError ("EpicPrefs Error : " + e);
				}
				fs.Close ();
				File.Move (fs.Name, fs.Name.Replace ("EpicEditorPrefs", "SafeToDelete/EpicEditorPrefs"));
				File.Delete (fs.Name.Replace (".epic", ".meta"));
			}
			foreach (var val in StringArrayDictionary) {
				EpicPrefs.SetArray (val.Key, val.Value, encrypted);
			}
		}

		//Save&Load FloatArrayDictionary
		static void LoadFloatArrayDictionary ()
		{
			FloatArrayDictionary = new Dictionary<string, float[]> ();
			FileStream fs;
			if (File.Exists (filePath + "FloatArrayDictionary.epic")) {
				fs = new FileStream (filePath + "FloatArrayDictionary.epic", FileMode.Open);
				if (fs.Length == 0) {
					fs.Close ();
					File.Move (fs.Name, fs.Name.Replace ("EpicEditorPrefs", "SafeToDelete/EpicEditorPrefs"));
					File.Delete (fs.Name.Replace (".epic", ".meta"));
					return;
				}
				BinaryFormatter bf = new BinaryFormatter ();				
				try {
					var obj = bf.Deserialize (fs);
					FloatArrayDictionary = (Dictionary<string,float[]>)obj;
				} catch (Exception e) {
					Debug.LogError ("EpicPrefs Error : " + e);
				}
				fs.Close ();
				File.Move (fs.Name, fs.Name.Replace ("EpicEditorPrefs", "SafeToDelete/EpicEditorPrefs"));
				File.Delete (fs.Name.Replace (".epic", ".meta"));
			}
			foreach (var val in FloatArrayDictionary) {
				EpicPrefs.SetArray (val.Key, val.Value, encrypted);
			}
		}

		//Save&Load DoubleArrayDictionary
		static void LoadDoubleArrayDictionary ()
		{
			DoubleArrayDictionary = new Dictionary<string, double[]> ();
			FileStream fs;
			if (File.Exists (filePath + "DoubleArrayDictionary.epic")) {
				fs = new FileStream (filePath + "DoubleArrayDictionary.epic", FileMode.Open);
				if (fs.Length == 0) {
					fs.Close ();
					File.Move (fs.Name, fs.Name.Replace ("EpicEditorPrefs", "SafeToDelete/EpicEditorPrefs"));
					File.Delete (fs.Name.Replace (".epic", ".meta"));
					return;
				}
				BinaryFormatter bf = new BinaryFormatter ();				
				try {
					var obj = bf.Deserialize (fs);
					DoubleArrayDictionary = (Dictionary<string,double[]>)obj;
				} catch (Exception e) {
					Debug.LogError ("EpicPrefs Error : " + e);
				}
				fs.Close ();
				File.Move (fs.Name, fs.Name.Replace ("EpicEditorPrefs", "SafeToDelete/EpicEditorPrefs"));
				File.Delete (fs.Name.Replace (".epic", ".meta"));
			}
			foreach (var val in DoubleArrayDictionary) {
				EpicPrefs.SetArray (val.Key, val.Value, encrypted);
			}
		}
		//Save&Load StringListDictionary
		static void LoadStringListDictionary ()
		{
			StringListDictionary = new Dictionary<string, List<string>> ();
			FileStream fs;
			if (File.Exists (filePath + "StringListDictionary.epic")) {
				fs = new FileStream (filePath + "StringListDictionary.epic", FileMode.Open);
				if (fs.Length == 0) {
					fs.Close ();
					File.Move (fs.Name, fs.Name.Replace ("EpicEditorPrefs", "SafeToDelete/EpicEditorPrefs"));
					File.Delete (fs.Name.Replace (".epic", ".meta"));
					return;
				}
				BinaryFormatter bf = new BinaryFormatter ();				
				try {
					var obj = bf.Deserialize (fs);
					StringListDictionary = (Dictionary<string,List<string>>)obj;
				} catch (Exception e) {
					Debug.LogError ("EpicPrefs Error : " + e);
				}
				fs.Close ();
				File.Move (fs.Name, fs.Name.Replace ("EpicEditorPrefs", "SafeToDelete/EpicEditorPrefs"));
				File.Delete (fs.Name.Replace (".epic", ".meta"));
			}
			foreach (var val in StringListDictionary) {
				EpicPrefs.SetList (val.Key, val.Value, encrypted);
			}
		}

		//Save&Load IntegerListDictionary
		static void LoadIntegerListDictionary ()
		{
			IntegerListDictionary = new Dictionary<string, List<int>> ();
			FileStream fs;
			if (File.Exists (filePath + "IntegerListDictionary.epic")) {
				fs = new FileStream (filePath + "IntegerListDictionary.epic", FileMode.Open);
				if (fs.Length == 0) {
					fs.Close ();
					File.Move (fs.Name, fs.Name.Replace ("EpicEditorPrefs", "SafeToDelete/EpicEditorPrefs"));
					File.Delete (fs.Name.Replace (".epic", ".meta"));
					return;
				}
				BinaryFormatter bf = new BinaryFormatter ();				
				try {
					var obj = bf.Deserialize (fs);
					IntegerListDictionary = (Dictionary<string,List<int>>)obj;
				} catch (Exception e) {
					Debug.LogError ("EpicPrefs Error : " + e);
				}
				fs.Close ();
				File.Move (fs.Name, fs.Name.Replace ("EpicEditorPrefs", "SafeToDelete/EpicEditorPrefs"));
				File.Delete (fs.Name.Replace (".epic", ".meta"));
			}
			foreach (var val in IntegerListDictionary) {
				EpicPrefs.SetList (val.Key, val.Value, encrypted);
			}
		}



		//Save&Load BooleanListDictionary
		static void LoadBooleanListDictionary ()
		{
			BooleanListDictionary = new Dictionary<string, List<bool>> ();
			FileStream fs;
			if (File.Exists (filePath + "BooleanListDictionary.epic")) {
				fs = new FileStream (filePath + "BooleanListDictionary.epic", FileMode.Open);
				if (fs.Length == 0) {
					fs.Close ();
					File.Move (fs.Name, fs.Name.Replace ("EpicEditorPrefs", "SafeToDelete/EpicEditorPrefs"));
					File.Delete (fs.Name.Replace (".epic", ".meta"));
					return;
				}
				BinaryFormatter bf = new BinaryFormatter ();				
				try {
					var obj = bf.Deserialize (fs);
					BooleanListDictionary = (Dictionary<string,List<bool>>)obj;
				} catch (Exception e) {
					Debug.LogError ("EpicPrefs Error : " + e);
				}
				fs.Close ();
				File.Move (fs.Name, fs.Name.Replace ("EpicEditorPrefs", "SafeToDelete/EpicEditorPrefs"));
				File.Delete (fs.Name.Replace (".epic", ".meta"));
			}
			foreach (var val in BooleanListDictionary) {
				EpicPrefs.SetList (val.Key, val.Value, encrypted);
			}
		}


		//Save&Load FloatListDictionary
		static void LoadFloatListDictionary ()
		{
			FloatListDictionary = new Dictionary<string, List<float>> ();
			FileStream fs;
			if (File.Exists (filePath + "FloatListDictionary.epic")) {
				fs = new FileStream (filePath + "FloatListDictionary.epic", FileMode.Open);
				if (fs.Length == 0) {
					fs.Close ();
					File.Move (fs.Name, fs.Name.Replace ("EpicEditorPrefs", "SafeToDelete/EpicEditorPrefs"));
					File.Delete (fs.Name.Replace (".epic", ".meta"));
					return;
				}
				BinaryFormatter bf = new BinaryFormatter ();				
				try {
					var obj = bf.Deserialize (fs);
					FloatListDictionary = (Dictionary<string,List<float>>)obj;
				} catch (Exception e) {
					Debug.LogError ("EpicPrefs Error : " + e);
				}
				fs.Close ();
				File.Move (fs.Name, fs.Name.Replace ("EpicEditorPrefs", "SafeToDelete/EpicEditorPrefs"));
				File.Delete (fs.Name.Replace (".epic", ".meta"));
			}
			foreach (var val in FloatListDictionary) {
				EpicPrefs.SetList (val.Key, val.Value, encrypted);
			}
		}
		//Save&Load DoubleListDictionary
		static void LoadDoubleListDictionary ()
		{
			DoubleListDictionary = new Dictionary<string, List<double>> ();
			FileStream fs;
			if (File.Exists (filePath + "DoubleListDictionary.epic")) {
				fs = new FileStream (filePath + "DoubleListDictionary.epic", FileMode.Open);
				if (fs.Length == 0) {
					fs.Close ();
					File.Move (fs.Name, fs.Name.Replace ("EpicEditorPrefs", "SafeToDelete/EpicEditorPrefs"));
					File.Delete (fs.Name.Replace (".epic", ".meta"));
					return;
				}
				BinaryFormatter bf = new BinaryFormatter ();				
				try {
					var obj = bf.Deserialize (fs);
					DoubleListDictionary = (Dictionary<string,List<double>>)obj;
				} catch (Exception e) {
					Debug.LogError ("EpicPrefs Error : " + e);
				}
				fs.Close ();
				File.Move (fs.Name, fs.Name.Replace ("EpicEditorPrefs", "SafeToDelete/EpicEditorPrefs"));
				File.Delete (fs.Name.Replace (".epic", ".meta"));
			}
			foreach (var val in DoubleListDictionary) {
				EpicPrefs.SetList (val.Key, val.Value, encrypted);
			}
		}

		//Save&Load LongListDictionary
		static void LoadLongListDictionary ()
		{
			LongListDictionary = new Dictionary<string, List<long>> ();
			FileStream fs;
			if (File.Exists (filePath + "LongListDictionary.epic")) {
				fs = new FileStream (filePath + "LongListDictionary.epic", FileMode.Open);
				if (fs.Length == 0) {
					fs.Close ();
					File.Move (fs.Name, fs.Name.Replace ("EpicEditorPrefs", "SafeToDelete/EpicEditorPrefs"));
					File.Delete (fs.Name.Replace (".epic", ".meta"));
					return;
				}
				BinaryFormatter bf = new BinaryFormatter ();				
				try {
					var obj = bf.Deserialize (fs);
					LongListDictionary = (Dictionary<string,List<long>>)obj;
				} catch (Exception e) {
					Debug.LogError ("EpicPrefs Error : " + e);
				}
				fs.Close ();
				File.Move (fs.Name, fs.Name.Replace ("EpicEditorPrefs", "SafeToDelete/EpicEditorPrefs"));
				File.Delete (fs.Name.Replace (".epic", ".meta"));
			}
			foreach (var val in LongListDictionary) {
				EpicPrefs.SetList (val.Key, val.Value, encrypted);
			}
		}

		//Save&Load StringDictionaryDictionary
		static void LoadStringDictionaryDictionary ()
		{
			StringDictionaryDictionary = new Dictionary<string, Dictionary<string,string>> ();
			FileStream fs;
			if (File.Exists (filePath + "StringDictionaryDictionary.epic")) {
				fs = new FileStream (filePath + "StringDictionaryDictionary.epic", FileMode.Open);
				if (fs.Length == 0) {
					fs.Close ();
					File.Move (fs.Name, fs.Name.Replace ("EpicEditorPrefs", "SafeToDelete/EpicEditorPrefs"));
					File.Delete (fs.Name.Replace (".epic", ".meta"));
					return;
				}
				BinaryFormatter bf = new BinaryFormatter ();				
				try {
					var obj = bf.Deserialize (fs);
					StringDictionaryDictionary = (Dictionary<string,Dictionary<string,string>>)obj;
				} catch (Exception e) {
					Debug.LogError ("EpicPrefs Error : " + e);
				}
				fs.Close ();
				File.Move (fs.Name, fs.Name.Replace ("EpicEditorPrefs", "SafeToDelete/EpicEditorPrefs"));
				File.Delete (fs.Name.Replace (".epic", ".meta"));
			}
			foreach (var val in StringDictionaryDictionary) {
				EpicPrefs.SetDict (val.Key, val.Value, encrypted);
			}
		}

		//Save&Load IntegerDictionaryDictionary
		static void LoadIntegerDictionaryDictionary ()
		{
			IntegerDictionaryDictionary = new Dictionary<string, Dictionary<string,int>> ();
			FileStream fs;
			if (File.Exists (filePath + "IntegerDictionaryDictionary.epic")) {
				fs = new FileStream (filePath + "IntegerDictionaryDictionary.epic", FileMode.Open);
				if (fs.Length == 0) {
					fs.Close ();
					File.Move (fs.Name, fs.Name.Replace ("EpicEditorPrefs", "SafeToDelete/EpicEditorPrefs"));
					File.Delete (fs.Name.Replace (".epic", ".meta"));
					return;
				}
				BinaryFormatter bf = new BinaryFormatter ();				
				try {
					var obj = bf.Deserialize (fs);
					IntegerDictionaryDictionary = (Dictionary<string,Dictionary<string,int>>)obj;
				} catch (Exception e) {
					Debug.LogError ("EpicPrefs Error : " + e);
				}
				fs.Close ();
				File.Move (fs.Name, fs.Name.Replace ("EpicEditorPrefs", "SafeToDelete/EpicEditorPrefs"));
				File.Delete (fs.Name.Replace (".epic", ".meta"));
			}
			foreach (var val in IntegerDictionaryDictionary) {
				EpicPrefs.SetDict (val.Key, val.Value, encrypted);
			}
		}

		//Save&Load BooleanDictionaryDictionary
		static void LoadBooleanDictionaryDictionary ()
		{
			BooleanDictionaryDictionary = new Dictionary<string, Dictionary<string,bool>> ();
			FileStream fs;
			if (File.Exists (filePath + "BooleanDictionaryDictionary.epic")) {
				fs = new FileStream (filePath + "BooleanDictionaryDictionary.epic", FileMode.Open);
				if (fs.Length == 0) {
					fs.Close ();
					File.Move (fs.Name, fs.Name.Replace ("EpicEditorPrefs", "SafeToDelete/EpicEditorPrefs"));
					File.Delete (fs.Name.Replace (".epic", ".meta"));
					return;
				}
				BinaryFormatter bf = new BinaryFormatter ();				
				try {
					var obj = bf.Deserialize (fs);
					BooleanDictionaryDictionary = (Dictionary<string,Dictionary<string,bool>>)obj;
				} catch (Exception e) {
					Debug.LogError ("EpicPrefs Error : " + e);
				}
				fs.Close ();
				File.Move (fs.Name, fs.Name.Replace ("EpicEditorPrefs", "SafeToDelete/EpicEditorPrefs"));
				File.Delete (fs.Name.Replace (".epic", ".meta"));
			}
			foreach (var val in BooleanDictionaryDictionary) {
				EpicPrefs.SetDict (val.Key, val.Value, encrypted);
			}
		}

		//Save&Load FloatDictionaryDictionary
		static void LoadFloatDictionaryDictionary ()
		{
			FloatDictionaryDictionary = new Dictionary<string, Dictionary<string,float>> ();
			FileStream fs;
			if (File.Exists (filePath + "FloatDictionaryDictionary.epic")) {
				fs = new FileStream (filePath + "FloatDictionaryDictionary.epic", FileMode.Open);
				if (fs.Length == 0) {
					fs.Close ();
					File.Move (fs.Name, fs.Name.Replace ("EpicEditorPrefs", "SafeToDelete/EpicEditorPrefs"));
					File.Delete (fs.Name.Replace (".epic", ".meta"));
					return;
				}
				BinaryFormatter bf = new BinaryFormatter ();				
				try {
					var obj = bf.Deserialize (fs);
					FloatDictionaryDictionary = (Dictionary<string,Dictionary<string,float>>)obj;
				} catch (Exception e) {
					Debug.LogError ("EpicPrefs Error : " + e);
				}
				fs.Close ();
				File.Move (fs.Name, fs.Name.Replace ("EpicEditorPrefs", "SafeToDelete/EpicEditorPrefs"));
				File.Delete (fs.Name.Replace (".epic", ".meta"));
			}
			foreach (var val in FloatDictionaryDictionary) {
				EpicPrefs.SetDict (val.Key, val.Value, encrypted);
			}
		}

		//Save&Load DoubleDictionaryDictionary
		static void LoadDoubleDictionaryDictionary ()
		{
			DoubleDictionaryDictionary = new Dictionary<string, Dictionary<string,double>> ();
			FileStream fs;
			if (File.Exists (filePath + "LoadDoubleDictionaryDictionary.epic")) {
				fs = new FileStream (filePath + "LoadDoubleDictionaryDictionary.epic", FileMode.Open);
				if (fs.Length == 0) {
					fs.Close ();
					File.Move (fs.Name, fs.Name.Replace ("EpicEditorPrefs", "SafeToDelete/EpicEditorPrefs"));
					File.Delete (fs.Name.Replace (".epic", ".meta"));
					return;
				}
				BinaryFormatter bf = new BinaryFormatter ();				
				try {
					var obj = bf.Deserialize (fs);
					DoubleDictionaryDictionary = (Dictionary<string,Dictionary<string,double>>)obj;
				} catch (Exception e) {
					Debug.LogError ("EpicPrefs Error : " + e);
				}
				fs.Close ();
				File.Move (fs.Name, fs.Name.Replace ("EpicEditorPrefs", "SafeToDelete/EpicEditorPrefs"));
				File.Delete (fs.Name.Replace (".epic", ".meta"));
			}
			foreach (var val in DoubleDictionaryDictionary) {
				EpicPrefs.SetDict (val.Key, val.Value, encrypted);
			}
		}

		//Save&Load LongDictionaryDictionary
		static void LoadLongDictionaryDictionary ()
		{
			LongDictionaryDictionary = new Dictionary<string, Dictionary<string,long>> ();
			FileStream fs;
			if (File.Exists (filePath + "LongDictionaryDictionary.epic")) {
				fs = new FileStream (filePath + "LongDictionaryDictionary.epic", FileMode.Open);
				if (fs.Length == 0) {
					fs.Close ();
					File.Move (fs.Name, fs.Name.Replace ("EpicEditorPrefs", "SafeToDelete/EpicEditorPrefs"));
					File.Delete (fs.Name.Replace (".epic", ".meta"));
					return;
				}
				BinaryFormatter bf = new BinaryFormatter ();				
				try {
					var obj = bf.Deserialize (fs);
					LongDictionaryDictionary = (Dictionary<string,Dictionary<string,long>>)obj;
				} catch (Exception e) {
					Debug.LogError ("EpicPrefs Error : " + e);
				}
				fs.Close ();
				File.Move (fs.Name, fs.Name.Replace ("EpicEditorPrefs", "SafeToDelete/EpicEditorPrefs"));
				File.Delete (fs.Name.Replace (".epic", ".meta"));
			}
			foreach (var val in LongDictionaryDictionary) {
				EpicPrefs.SetDict (val.Key, val.Value, encrypted);
			}
		}

		//Save&Load QuaternionDictionary
		static void LoadQuaternionDictionary ()
		{
			QuaternionDictionary = new Dictionary<string, Quaternion> ();
			FileStream fs;
			if (File.Exists (filePath + "QuaternionDictionary.epic")) {
				fs = new FileStream (filePath + "QuaternionDictionary.epic", FileMode.Open);
				if (fs.Length == 0) {
					fs.Close ();
					File.Move (fs.Name, fs.Name.Replace ("EpicEditorPrefs", "SafeToDelete/EpicEditorPrefs"));
					File.Delete (fs.Name.Replace (".epic", ".meta"));
					return;
				}
				var ss = new SurrogateSelector ();
				ss.AddSurrogate (typeof(Quaternion),
					new StreamingContext (StreamingContextStates.All),
					new QuaternionSerializationSurrogate ());
				// Associate the SurrogateSelector with the BinaryFormatter.
				BinaryFormatter bf = new BinaryFormatter ();				
				bf.SurrogateSelector = ss;
				try {
					var obj = bf.Deserialize (fs);
					QuaternionDictionary = (Dictionary<string,Quaternion>)obj;
				} catch (Exception e) {
					Debug.LogError ("EpicPrefs Error : " + e);
				}
				fs.Close ();
				File.Move (fs.Name, fs.Name.Replace ("EpicEditorPrefs", "SafeToDelete/EpicEditorPrefs"));
				File.Delete (fs.Name.Replace (".epic", ".meta"));
			}
			foreach (var val in QuaternionDictionary) {
				EpicPrefs.SetQuaternion (val.Key, val.Value, encrypted);
			}
		}

		//Save&Load Vector2Dictionary
		static void LoadVector2Dictionary ()
		{
			Vector2Dictionary = new Dictionary<string, Vector2> ();
			FileStream fs;
			if (File.Exists (filePath + "Vector2Dictionary.epic")) {
				fs = new FileStream (filePath + "Vector2Dictionary.epic", FileMode.Open);
				if (fs.Length == 0) {
					fs.Close ();
					File.Move (fs.Name, fs.Name.Replace ("EpicEditorPrefs", "SafeToDelete/EpicEditorPrefs"));
					File.Delete (fs.Name.Replace (".epic", ".meta"));
					return;
				}
				var ss = new SurrogateSelector ();
				ss.AddSurrogate (typeof(Vector2),
					new StreamingContext (StreamingContextStates.All),
					new Vector2SerializationSurrogate ());
				// Associate the SurrogateSelector with the BinaryFormatter.
				BinaryFormatter bf = new BinaryFormatter ();				
				bf.SurrogateSelector = ss;
				try {
					var obj = bf.Deserialize (fs);
					Vector2Dictionary = (Dictionary<string,Vector2>)obj;
				} catch (Exception e) {
					Debug.LogError ("EpicPrefs Error : " + e);
				}
				fs.Close ();
				File.Move (fs.Name, fs.Name.Replace ("EpicEditorPrefs", "SafeToDelete/EpicEditorPrefs"));
				File.Delete (fs.Name.Replace (".epic", ".meta"));
			}
			foreach (var val in Vector2Dictionary) {
				EpicPrefs.SetVector2 (val.Key, val.Value, encrypted);
			}
		}

		//Save&Load Vector3Dictionary
		static void LoadVector3Dictionary ()
		{
			Vector3Dictionary = new Dictionary<string, Vector3> ();
			FileStream fs;
			if (File.Exists (filePath + "Vector3Dictionary.epic")) {
				fs = new FileStream (filePath + "Vector3Dictionary.epic", FileMode.Open);
				if (fs.Length == 0) {
					fs.Close ();
					File.Move (fs.Name, fs.Name.Replace ("EpicEditorPrefs", "SafeToDelete/EpicEditorPrefs"));
					File.Delete (fs.Name.Replace (".epic", ".meta"));
					return;
				}
				var ss = new SurrogateSelector ();
				ss.AddSurrogate (typeof(Vector3),
					new StreamingContext (StreamingContextStates.All),
					new Vector3SerializationSurrogate ());
				// Associate the SurrogateSelector with the BinaryFormatter.
				BinaryFormatter bf = new BinaryFormatter ();				
				bf.SurrogateSelector = ss;
				try {
					var obj = bf.Deserialize (fs);
					Vector3Dictionary = (Dictionary<string,Vector3>)obj;
				} catch (Exception e) {
					Debug.LogError ("EpicPrefs Error : " + e);
				}
				fs.Close ();
				File.Move (fs.Name, fs.Name.Replace ("EpicEditorPrefs", "SafeToDelete/EpicEditorPrefs"));
				File.Delete (fs.Name.Replace (".epic", ".meta"));
			}
			foreach (var val in Vector3Dictionary) {
				EpicPrefs.SetVector3 (val.Key, val.Value, encrypted);
			}
		}

		//Save&Load Vector4Dictionary
		static void LoadVector4Dictionary ()
		{
			Vector4Dictionary = new Dictionary<string, Vector4> ();
			FileStream fs;
			if (File.Exists (filePath + "Vector4Dictionary.epic")) {
				fs = new FileStream (filePath + "Vector4Dictionary.epic", FileMode.Open);
				if (fs.Length == 0) {
					fs.Close ();
					File.Move (fs.Name, fs.Name.Replace ("EpicEditorPrefs", "SafeToDelete/EpicEditorPrefs"));
					File.Delete (fs.Name.Replace (".epic", ".meta"));
					return;
				}
				var ss = new SurrogateSelector ();
				ss.AddSurrogate (typeof(Vector4),
					new StreamingContext (StreamingContextStates.All),
					new Vector4SerializationSurrogate ());
				// Associate the SurrogateSelector with the BinaryFormatter.
				BinaryFormatter bf = new BinaryFormatter ();				
				bf.SurrogateSelector = ss;
				try {
					var obj = bf.Deserialize (fs);
					Vector4Dictionary = (Dictionary<string,Vector4>)obj;
				} catch (Exception e) {
					Debug.LogError ("EpicPrefs Error : " + e);
				}
				fs.Close ();
				File.Move (fs.Name, fs.Name.Replace ("EpicEditorPrefs", "SafeToDelete/EpicEditorPrefs"));
				File.Delete (fs.Name.Replace (".epic", ".meta"));
			}
			foreach (var val in Vector4Dictionary) {
				EpicPrefs.SetVector4 (val.Key, val.Value, encrypted);
			}
		}

		//Save&Load ColorDictionary
		static void LoadColorDictionary ()
		{
			ColorDictionary = new Dictionary<string, Color> ();
			FileStream fs;
			if (File.Exists (filePath + "ColorDictionary.epic")) {
				fs = new FileStream (filePath + "ColorDictionary.epic", FileMode.Open);
				if (fs.Length == 0) {
					fs.Close ();
					File.Move (fs.Name, fs.Name.Replace ("EpicEditorPrefs", "SafeToDelete/EpicEditorPrefs"));
					File.Delete (fs.Name.Replace (".epic", ".meta"));
					return;
				}
				var ss = new SurrogateSelector ();
				ss.AddSurrogate (typeof(Color),
					new StreamingContext (StreamingContextStates.All),
					new ColorSerializationSurrogate ());
				// Associate the SurrogateSelector with the BinaryFormatter.
				BinaryFormatter bf = new BinaryFormatter ();				
				bf.SurrogateSelector = ss;
				try {
					var obj = bf.Deserialize (fs);
					ColorDictionary = (Dictionary<string,Color>)obj;
				} catch (Exception e) {
					Debug.LogError ("EpicPrefs Error : " + e);
				}
				fs.Close ();
				File.Move (fs.Name, fs.Name.Replace ("EpicEditorPrefs", "SafeToDelete/EpicEditorPrefs"));
				File.Delete (fs.Name.Replace (".epic", ".meta"));
			}
			foreach (var val in ColorDictionary) {
				EpicPrefs.SetColor (val.Key, val.Value, encrypted);
			}
		}
	};
	#endif
}

