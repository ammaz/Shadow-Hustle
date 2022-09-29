using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using System;
using System.IO;
#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
#endif
namespace EpicPrefsTools{
	public static class Operators {
	    public static bool IsInteger(string sValue)
	    {
	        if (sValue == "-" || sValue == "" || sValue == " ")
	            return false;
	        else
	            return true;
	    }
	    public static bool IsFloat(string sValue)
	    {
	        if (sValue == "-" || sValue == "" || sValue == " " || sValue == "." || sValue == "-.")
	            return false;
	        else
	            return true;
	    }
	    public static bool ToBool(string value)
	    {
	        if (value.ToLower().Trim() == "true" || value == "1")
	            return true;
	        else
	            return false;
	    }
	    public static float ToFloat(string value)
	    {
	        float newFloat = 0;
	        string minus = "-";
	        bool negative = false;
	        if (value.Length > 0)
	            if (value[0] == minus[0])
	                negative = true;
	        value = Regex.Replace(value, "[^0-9.]", "");
	        if (value.Contains("."))
	        {
	            string preDot = "";
	            string afterDot = "";
	            preDot = value.Substring(0, 1 + value.IndexOf("."));
	            afterDot = value.Substring(value.IndexOf(".") + 1);
	            afterDot = Regex.Replace(afterDot, "[^0-9]", "");
	            value = preDot + afterDot;
	        }
	        if (negative)
	            value = "-" + value;
	        if (value != "" && value != "-")
	            newFloat = Convert.ToSingle(value);
	        return newFloat;
	    }
	    public static double ToDouble(string value)
	    {
	        double newDouble = 0;
	        string minus = "-";
	        bool negative = false;
	        if (value.Length > 0)
	            if (value[0] == minus[0])
	                negative = true;
	        value = Regex.Replace(value, "[^0-9.]", "");
	        if (value.Contains("."))
	        {
	            string preDot = "";
	            string afterDot = "";
	            preDot = value.Substring(0, 1 + value.IndexOf("."));
	            afterDot = value.Substring(value.IndexOf(".") + 1);
	            afterDot = Regex.Replace(afterDot, "[^0-9]", "");
	            value = preDot + afterDot;
	        }
	        if (negative)
	            value = "-" + value;
	        if (value != "" && value != "-")
	            newDouble = Convert.ToDouble(value);
	        return newDouble;
	    }
	    public static int ToInt(string value)
	    {
	        int newInt = 0;
	        string minus = "-";
	        bool negative = false;
	        if (value.Length > 0)
	            if (value[0] == minus[0])
	                negative = true;
	        value = Regex.Replace(value, "[^0-9]", "");
	        if (negative)
	            value = "-" + value;
	        if (value != "" && value != "-")
	            newInt = Convert.ToInt32(value);
	        return newInt;
	    }
	    public static long ToLong(string value)
	    {
	        long newLong = 0;
	        string minus = "-";
	        bool negative = false;
	        if (value.Length > 0)
	            if (value[0] == minus[0])
	                negative = true;
	        value = Regex.Replace(value, "[^0-9]", "");
	        if (negative)
	            value = "-" + value;
	        if (value != "" && value != "-")
	            newLong = Convert.ToInt64(value);
	        return newLong;
		}
		public static Vector2 ToVector2(string a,string b){
			return new Vector2 (ToFloat (a), ToFloat (b));
		}
		public static Vector2 ToVector2(string vec){
			var components = vec.Substring (1, vec.Length - 2).Split (';');
			return new Vector2 (ToFloat (components[0]), ToFloat (components[1]));
		}
		public static Vector3 ToVector3(string a,string b, string c){
			return new Vector3 (ToFloat (a), ToFloat (b), ToFloat(c));
		}
		public static Vector3 ToVector3(string vec){
			var components = vec.Substring (1, vec.Length - 2).Split (';');
			return new Vector3 (ToFloat (components[0]), ToFloat (components[1]), ToFloat (components[2]));
		}
		public static Vector4 ToVector4(string a,string b, string c, string d){
			return new Vector4 (ToFloat (a), ToFloat (b), ToFloat(c), ToFloat(d));
		}
		public static Vector4 ToVector4(string vec){
			var components = vec.Substring (1, vec.Length - 2).Split (';');
			return new Vector4 (ToFloat (components[0]), ToFloat (components[1]), ToFloat (components[2]), ToFloat (components[3]));
		}
		public static string Vector2String(Vector2 vec){
			return "(" + vec.x + ";" + vec.y + ")";
		}
		public static string Vector2String(Vector3 vec){
			return "(" + vec.x + ";" + vec.y + ";" + vec.z + ")";
		}
		public static string Vector2String(Vector4 vec){
			return "(" + vec.x + ";" + vec.y + ";" + vec.z + ";" + vec.w + ")";
		}
		public static Quaternion ToQuaternion(string a,string b, string c, string d){
			return new Quaternion (ToFloat (a), ToFloat (b), ToFloat(c), ToFloat(d));
		}
		public static Quaternion ToQuaternion(string quat){
			var components = quat.Substring (1, quat.Length - 2).Split (';');
			return new Quaternion (ToFloat (components[0]), ToFloat (components[1]), ToFloat (components[2]), ToFloat (components[3]));
		}
		public static string Quaternion2String(Quaternion quat){
			return "(" + quat.x + ";" + quat.y + ";" + quat.z + ";" + quat.w + ")";
		}
		public static Color StringToColor(string value) {
			float r = 0;
			float g = 0;
			float b = 0;
			float a = 0;
	        if(value.Length != 0){
	            r = ToFloat(value.Substring (3, value.IndexOf ("[g]") - 3)); // [r]0.2[g]0.8[b]0.9
	            g = ToFloat(value.Substring (value.IndexOf ("[g]") + 3, value.IndexOf ("[b]") - (value.IndexOf ("[g]") + 3))); // [r]0.2[g]0.8[b]0.9
	            b = ToFloat (value.Substring (value.IndexOf ("[b]") + 3, value.IndexOf ("[a]") - (value.IndexOf ("[b]") + 3)));
	            a = ToFloat (value.Substring (value.IndexOf ("[a]") + 3));
	            if(r<0f)
	                r = 0f;
	            if(g<0f)
	                g = 0f;
	            if(b<0f)
	                b = 0f;
	            if(a<0f)
	                a = 0f;
	        }
			Color newColor = new Color (r,g,b,a);
			return newColor;
		}
		public static string ColorToString(Color value){
			float r = value.r;
			float g = value.g;
			float b = value.b;
			float a = value.a;
			string strCol = "[r]" + r.ToString () + "[g]" + g.ToString () + "[b]" + b.ToString () + "[a]" + a.ToString ();
	        return strCol;
		}

	    public static void DeleteDirectory(string path,bool recursively){
	        if(Directory.Exists(path))
	            Directory.Delete(path,recursively);  
	    }
	    public static void setupPrefs(){
			if(PlayerPrefs.GetString(EpicPrefsExportVersioner.prefix+EpicPrefsExportVersioner.guid,"false") == "false"){
				if(Directory.Exists(Application.persistentDataPath + "/HotTotem/EpicPrefsData/Settings")){
					Directory.Delete (Application.persistentDataPath + "/HotTotem/EpicPrefsData/Settings", true);
				}
				TextAsset sourceTexts = Resources.Load("HotTotemAssets/EpicPrefs/Settings/SettingsFiles") as TextAsset;
	            string[] source = sourceTexts.text.Split("\n"[0]);
				for (int i = 0; i < source.Length; i++) {
					var fromPath = source [i];
					TextAsset file = Resources.Load (fromPath) as TextAsset;
					if (file != null) {
						var directory = Directory.GetParent (Application.persistentDataPath + "/" + fromPath.Replace ("HotTotemAssets/EpicPrefs/", "HotTotem/EpicPrefsData/"));
						directory.Create ();
						File.WriteAllBytes (Application.persistentDataPath + "/" + fromPath.Replace ("HotTotemAssets/EpicPrefs/", "HotTotem/EpicPrefsData/"), file.bytes);   
					}
				}

				Serializer.ImportPrefs ();
				PlayerPrefs.SetString(EpicPrefsExportVersioner.prefix+EpicPrefsExportVersioner.guid,"true");
			}
	    }
		#if UNITY_EDITOR
		public static void CopyPrefs(ScriptableObject _obj,EpicPrefsContainer _container,bool _encrypted,bool _default)
		{
			if (_default) {
				EpicPrefsContainerHandler.ExportDefaultEpicPrefsContainer (_container, _encrypted);
			} else {
				EpicPrefsExportVersioner.TimeStamp (_obj);
				EpicPrefsContainerHandler.ExportEpicPrefsContainer (_container, _encrypted);
			}
		}
		public static void MigratePrefsToNewKey(){
			EditorUtility.DisplayProgressBar ("Migrating...", "Re-crypting your EpicPrefs", 0f);
			var filePath = Application.dataPath;
			DeleteDirectory (filePath + "/HotTotemAssets/EpicPrefs/Resources",true);
			filePath = filePath.Substring(0,filePath.IndexOf("/Assets"));
			filePath = filePath + "/HotTotem/EpicPrefsData/Encrypted/";
			if (Directory.Exists (filePath)) {
				Directory.Delete (filePath,true);
			}
			Operators.CopySettings();
			AssetDatabase.Refresh ();
			var epicPrefsContainerEncrypted = EpicPrefsEditorTools.EpicPrefsEditorContainerHandler.LoadEpicPrefsEditorContainer (true);
			epicPrefsContainerEncrypted.BooleanDictionary.ToList ().ForEach (x => {
				EpicPrefs.SetBool(x.Key,x.Value,true);
			});
			EditorUtility.DisplayProgressBar ("Migrating...", "Re-crypting your EpicPrefs", 0.2f);

			epicPrefsContainerEncrypted.BooleanDictionaryDictionary.ToList ().ForEach (x => {
				EpicPrefs.SetDict(x.Key,x.Value,true);
			});

			epicPrefsContainerEncrypted.BooleanListDictionary.ToList ().ForEach (x => {
				EpicPrefs.SetList(x.Key,x.Value,true);
			});

			epicPrefsContainerEncrypted.ColorDictionary.ToList ().ForEach (x => {
				EpicPrefs.SetColor(x.Key,x.Value,true);
			});

			epicPrefsContainerEncrypted.DoubleArrayDictionary.ToList ().ForEach (x => {
				EpicPrefs.SetArray(x.Key,x.Value,true);
			});

			epicPrefsContainerEncrypted.DoubleDictionary.ToList ().ForEach (x => {
				EpicPrefs.SetDouble(x.Key,x.Value,true);
			});

			epicPrefsContainerEncrypted.DoubleDictionaryDictionary.ToList ().ForEach (x => {
				EpicPrefs.SetDict(x.Key,x.Value,true);
			});

			epicPrefsContainerEncrypted.DoubleListDictionary.ToList ().ForEach (x => {
				EpicPrefs.SetList(x.Key,x.Value,true);
			});

			epicPrefsContainerEncrypted.FloatArrayDictionary.ToList ().ForEach (x => {
				EpicPrefs.SetArray(x.Key,x.Value,true);
			});

			epicPrefsContainerEncrypted.FloatDictionary.ToList ().ForEach (x => {
				EpicPrefs.SetFloat(x.Key,x.Value,true);
			});
			EditorUtility.DisplayProgressBar ("Migrating...", "Re-crypting your EpicPrefs", 0.4f);

			epicPrefsContainerEncrypted.FloatDictionaryDictionary.ToList ().ForEach (x => {
				EpicPrefs.SetDict(x.Key,x.Value,true);
			});

			epicPrefsContainerEncrypted.FloatListDictionary.ToList ().ForEach (x => {
				EpicPrefs.SetList(x.Key,x.Value,true);
			});

			epicPrefsContainerEncrypted.IntegerArrayDictionary.ToList ().ForEach (x => {
				EpicPrefs.SetArray(x.Key,x.Value,true);
			});

			epicPrefsContainerEncrypted.IntegerDictionary.ToList ().ForEach (x => {
				EpicPrefs.SetInt(x.Key,x.Value,true);
			});

			epicPrefsContainerEncrypted.IntegerDictionaryDictionary.ToList ().ForEach (x => {
				EpicPrefs.SetDict(x.Key,x.Value,true);
			});

			epicPrefsContainerEncrypted.IntegerListDictionary.ToList ().ForEach (x => {
				EpicPrefs.SetList(x.Key,x.Value,true);
			});

			epicPrefsContainerEncrypted.LongDictionary.ToList ().ForEach (x => {
				EpicPrefs.SetLong(x.Key,x.Value,true);
			});

			epicPrefsContainerEncrypted.LongDictionaryDictionary.ToList ().ForEach (x => {
				EpicPrefs.SetDict(x.Key,x.Value,true);
			});

			epicPrefsContainerEncrypted.LongListDictionary.ToList ().ForEach (x => {
				EpicPrefs.SetList(x.Key,x.Value,true);
			});

			epicPrefsContainerEncrypted.QuaternionDictionary.ToList ().ForEach (x => {
				EpicPrefs.SetQuaternion(x.Key,x.Value,true);
			});
			EditorUtility.DisplayProgressBar ("Migrating...", "Re-crypting your EpicPrefs", 0.6f);

			epicPrefsContainerEncrypted.StringArrayDictionary.ToList ().ForEach (x => {
				EpicPrefs.SetArray(x.Key,x.Value,true);
			});

			epicPrefsContainerEncrypted.StringDictionary.ToList ().ForEach (x => {
				EpicPrefs.SetString(x.Key,x.Value,true);
			});

			epicPrefsContainerEncrypted.StringDictionaryDictionary.ToList ().ForEach (x => {
				EpicPrefs.SetDict(x.Key,x.Value,true);
			});

			epicPrefsContainerEncrypted.StringListDictionary.ToList ().ForEach (x => {
				EpicPrefs.SetList(x.Key,x.Value,true);
			});
			EditorUtility.DisplayProgressBar ("Migrating...", "Re-crypting your EpicPrefs", 0.8f);

			epicPrefsContainerEncrypted.Vector2Dictionary.ToList ().ForEach (x => {
				EpicPrefs.SetVector2(x.Key,x.Value,true);
			});

			epicPrefsContainerEncrypted.Vector3Dictionary.ToList ().ForEach (x => {
				EpicPrefs.SetVector3(x.Key,x.Value,true);
			});

			epicPrefsContainerEncrypted.Vector4Dictionary.ToList ().ForEach (x => {
				EpicPrefs.SetVector4(x.Key,x.Value,true);
			});
			EditorUtility.DisplayProgressBar ("Migrating...", "Re-crypting your EpicPrefs", 1f);
			EditorUtility.ClearProgressBar();
			AssetDatabase.Refresh ();
		}
		#endif
		public static void CopySettings()
		{
			var sourceDirName = "HotTotem/EpicPrefsData/Settings/DataA/";
			var filePath = Application.dataPath;
			filePath = filePath.Substring(0,filePath.IndexOf("/Assets"));
			var SourcePath = filePath + "/" + sourceDirName;
			var DestinationPath = Application.dataPath +"/HotTotemAssets/EpicPrefs/Resources/HotTotemAssets/EpicPrefs/Settings/DataA/";
			if(Directory.Exists(filePath +"/"+ sourceDirName)){
				foreach (string dirPath in Directory.GetDirectories(filePath +"/"+ sourceDirName, "*",SearchOption.AllDirectories))
					Directory.CreateDirectory(dirPath.Replace(SourcePath, DestinationPath));
				foreach (string newPath in Directory.GetFiles(SourcePath, "*.*", SearchOption.AllDirectories)) {
					File.Copy (newPath, newPath.Replace (SourcePath, DestinationPath) + ".bytes", true);
					var tmpPath = newPath.Replace (SourcePath, DestinationPath);
					var fileName = tmpPath.Replace (Application.dataPath + "/HotTotemAssets/EpicPrefs/Resources/", "");
					File.AppendAllText (Application.dataPath + "/HotTotemAssets/EpicPrefs/Resources/HotTotemAssets/EpicPrefs/Settings/SettingsFiles.bytes", fileName + Environment.NewLine);
				}
			}
			sourceDirName = "HotTotem/EpicPrefsData/Settings/DataB/";
			filePath = Application.dataPath;
			filePath = filePath.Substring(0,filePath.IndexOf("/Assets"));
			SourcePath = filePath + "/" + sourceDirName;
			DestinationPath = Application.dataPath +"/HotTotemAssets/EpicPrefs/Resources/HotTotemAssets/EpicPrefs/Settings/DataB/";
			if(Directory.Exists(filePath +"/"+ sourceDirName)){
				foreach (string dirPath in Directory.GetDirectories(filePath +"/"+ sourceDirName, "*",SearchOption.AllDirectories))
					Directory.CreateDirectory(dirPath.Replace(SourcePath, DestinationPath));
				foreach (string newPath in Directory.GetFiles(SourcePath, "*.*", SearchOption.AllDirectories)) {
					File.Copy (newPath, newPath.Replace (SourcePath, DestinationPath) + ".bytes", true);
					var tmpPath = newPath.Replace (SourcePath, DestinationPath);
					var fileName = tmpPath.Replace (Application.dataPath + "/HotTotemAssets/EpicPrefs/Resources/", "");
					File.AppendAllText (Application.dataPath + "/HotTotemAssets/EpicPrefs/Resources/HotTotemAssets/EpicPrefs/Settings/SettingsFiles.bytes", fileName + Environment.NewLine);
				}
			}
		}
	}
}