using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Linq;

namespace EpicPrefsTools
{
	[Serializable]
	public class EpicPrefsContainer
	{
		public Dictionary<string, int> IntegerDictionary = new Dictionary<string, int> ();
		public Dictionary<string, float> FloatDictionary = new Dictionary<string, float> ();
		public Dictionary<string, bool> BooleanDictionary = new Dictionary<string, bool> ();
		public Dictionary<string, string> StringDictionary = new Dictionary<string, string> ();
		public Dictionary<string, double> DoubleDictionary = new Dictionary<string, double> ();
		public Dictionary<string, long> LongDictionary = new Dictionary<string, long> ();
		public Dictionary<string, int[]> IntegerArrayDictionary = new Dictionary<string, int[]> ();
		public Dictionary<string, string[]> StringArrayDictionary = new Dictionary<string, string[]> ();
		public Dictionary<string, float[]> FloatArrayDictionary = new Dictionary<string, float[]> ();
		public Dictionary<string, double[]> DoubleArrayDictionary = new Dictionary<string, double[]> ();
		public Dictionary<string, List<string>> StringListDictionary = new Dictionary<string, List<string>> ();
		public Dictionary<string, List<int>> IntegerListDictionary = new Dictionary<string, List<int>> ();
		public Dictionary<string, List<bool>> BooleanListDictionary = new Dictionary<string, List<bool>> ();
		public Dictionary<string, List<float>> FloatListDictionary = new Dictionary<string, List<float>> ();
		public Dictionary<string, List<double>> DoubleListDictionary = new Dictionary<string, List<double>> ();
		public Dictionary<string, List<long>> LongListDictionary = new Dictionary<string, List<long>> ();
		public Dictionary<string, Dictionary<string,string>> StringDictionaryDictionary = new Dictionary<string, Dictionary<string, string>> ();
		public Dictionary<string, Dictionary<string,int>> IntegerDictionaryDictionary = new Dictionary<string, Dictionary<string, int>> ();
		public Dictionary<string, Dictionary<string,bool>> BooleanDictionaryDictionary = new Dictionary<string, Dictionary<string, bool>> ();
		public Dictionary<string, Dictionary<string,float>> FloatDictionaryDictionary = new Dictionary<string, Dictionary<string, float>> ();
		public Dictionary<string, Dictionary<string,double>> DoubleDictionaryDictionary = new Dictionary<string, Dictionary<string, double>> ();
		public Dictionary<string, Dictionary<string,long>> LongDictionaryDictionary = new Dictionary<string, Dictionary<string, long>> ();
		public Dictionary<string, Quaternion> QuaternionDictionary = new Dictionary<string, Quaternion> ();
		public Dictionary<string, Vector2> Vector2Dictionary = new Dictionary<string, Vector2> ();
		public Dictionary<string, Vector3> Vector3Dictionary = new Dictionary<string, Vector3> ();
		public Dictionary<string, Vector4> Vector4Dictionary = new Dictionary<string, Vector4> ();
		public Dictionary<string, Color> ColorDictionary = new Dictionary<string, Color> ();
	}

	public static class EpicPrefsContainerHandler
	{
		//Save&Load Vector2Dictionary
		public static EpicPrefsContainer LoadEpicPrefsContainer (bool encrypted)
		{
			EpicPrefsContainer loadedContainer = new EpicPrefsContainer ();
			var filePath = "";
			#if UNITY_EDITOR
			filePath = Application.dataPath;
			filePath = filePath.Substring(0,filePath.IndexOf("/Assets"));
			if (!Directory.Exists(filePath + "/HotTotem"))
			{
				Directory.CreateDirectory(filePath + "/HotTotem/");
				Directory.CreateDirectory(filePath + "/HotTotem/EpicPrefsData");
				Directory.CreateDirectory(filePath + "/HotTotem/EpicPrefsData/Encrypted");
				Directory.CreateDirectory(filePath + "/HotTotem/EpicPrefsData/NotEncrypted");
			}
			#endif
			if (encrypted) {
				#if UNITY_EDITOR
				filePath = filePath + "/HotTotem/EpicPrefsData/Encrypted/";
				#else
				filePath = Application.persistentDataPath + "/HotTotem/EpicPrefsData/Encrypted/";
				#endif
			}
			else {
				#if UNITY_EDITOR
				filePath = filePath + "/HotTotem/EpicPrefsData/NotEncrypted/";
				#else
				filePath = Application.persistentDataPath + "/HotTotem/EpicPrefsData/NotEncrypted/";
				#endif
			}
			var bf = new BinaryFormatter ();
			var ss = new SurrogateSelector ();
			ss.AddSurrogate (typeof(Dictionary<string,Vector2>),
				new StreamingContext (StreamingContextStates.All),
				new Vector2DictSerializationSurrogate ());
			ss.AddSurrogate (typeof(Dictionary<string,Vector3>),
				new StreamingContext (StreamingContextStates.All),
				new Vector3DictSerializationSurrogate ());
			ss.AddSurrogate (typeof(Dictionary<string,Vector4>),
				new StreamingContext (StreamingContextStates.All),
				new Vector4DictSerializationSurrogate ());
			ss.AddSurrogate (typeof(Dictionary<string,Color>),
				new StreamingContext (StreamingContextStates.All),
				new ColorDictSerializationSurrogate ());
			ss.AddSurrogate (typeof(Dictionary<string,Quaternion>),
				new StreamingContext (StreamingContextStates.All),
				new QuaternionDictSerializationSurrogate ());
			// Associate the SurrogateSelector with the BinaryFormatter.
			bf.SurrogateSelector = ss;
			if (File.Exists (filePath + "EpicPrefsContainer.epic")) {
				if (encrypted) {
					try{
						var memStream = new MemoryStream (Cryptor.Decrypt(File.ReadAllText (filePath + "EpicPrefsContainer.epic")));
						var obj = bf.Deserialize (memStream);
						loadedContainer = (EpicPrefsContainer)obj;
					} 
					catch{
						File.Delete (filePath + "EpicPrefsContainer.epic");
					}
				} else {
					FileStream fs;
					fs = new FileStream (filePath + "EpicPrefsContainer.epic", FileMode.Open);
					if (fs.Length == 0) {
						fs.Close ();
						return loadedContainer;
					}
					try {
						var obj = bf.Deserialize (fs);
						loadedContainer = (EpicPrefsContainer)obj;
					} catch (Exception e) {
						Debug.LogError ("EpicPrefs Error : " + e);
					}
					fs.Close ();
				}
			} 
			return loadedContainer;
		}
		public static EpicPrefsContainer ImportEpicPrefsContainer (EpicPrefsContainer Container,bool encrypted)
		{
			EpicPrefsContainer loadedContainer = new EpicPrefsContainer ();
			EpicPrefsContainer defaultContainer = new EpicPrefsContainer ();
			var bf = new BinaryFormatter ();
			var ss = new SurrogateSelector ();
			ss.AddSurrogate (typeof(Dictionary<string,Vector2>),
				new StreamingContext (StreamingContextStates.All),
				new Vector2DictSerializationSurrogate ());
			ss.AddSurrogate (typeof(Dictionary<string,Vector3>),
				new StreamingContext (StreamingContextStates.All),
				new Vector3DictSerializationSurrogate ());
			ss.AddSurrogate (typeof(Dictionary<string,Vector4>),
				new StreamingContext (StreamingContextStates.All),
				new Vector4DictSerializationSurrogate ());
			ss.AddSurrogate (typeof(Dictionary<string,Color>),
				new StreamingContext (StreamingContextStates.All),
				new ColorDictSerializationSurrogate ());
			ss.AddSurrogate (typeof(Dictionary<string,Quaternion>),
				new StreamingContext (StreamingContextStates.All),
				new QuaternionDictSerializationSurrogate ());
			// Associate the SurrogateSelector with the BinaryFormatter.
			bf.SurrogateSelector = ss;
			if (encrypted) {
				var encryptedPrefs = Resources.Load ("HotTotemAssets/EpicPrefs/Encrypted/EpicPrefsContainer") as TextAsset;
				if (encryptedPrefs != null) {
					try{
						var memStream = new MemoryStream (Cryptor.Decrypt (encryptedPrefs.text));
						var obj = bf.Deserialize (memStream);
						loadedContainer = (EpicPrefsContainer)obj;
					} 
					catch{
						Resources.UnloadAsset (encryptedPrefs);
						loadedContainer = new EpicPrefsContainer ();
					}
				}
				encryptedPrefs = Resources.Load ("HotTotemAssets/EpicPrefs/Encrypted/Default/EpicPrefsContainer") as TextAsset;
				if (encryptedPrefs != null) {
					try{
						var memStream = new MemoryStream (Cryptor.Decrypt (encryptedPrefs.text));
						var obj = bf.Deserialize (memStream);
						defaultContainer = (EpicPrefsContainer)obj;
					} 
					catch{
						Resources.UnloadAsset (encryptedPrefs);
						defaultContainer = new EpicPrefsContainer ();
					}
				}
			} else {
				var notEncryptedPrefs = Resources.Load ("HotTotemAssets/EpicPrefs/NotEncrypted/EpicPrefsContainer") as TextAsset;
				if (notEncryptedPrefs != null) {
					try{
						var memStream = new MemoryStream (notEncryptedPrefs.bytes);
						var obj = bf.Deserialize (memStream);
						loadedContainer = (EpicPrefsContainer)obj;
					} 
					catch{
						Resources.UnloadAsset (notEncryptedPrefs);
						loadedContainer = new EpicPrefsContainer ();
					}
				}
				notEncryptedPrefs = Resources.Load ("HotTotemAssets/EpicPrefs/NotEncrypted/Default/EpicPrefsContainer") as TextAsset;
				if (notEncryptedPrefs != null) {
					try{
						var memStream = new MemoryStream (notEncryptedPrefs.bytes);
						var obj = bf.Deserialize (memStream);
						defaultContainer = (EpicPrefsContainer)obj;
					} 
					catch{
						Resources.UnloadAsset (notEncryptedPrefs);
						defaultContainer = new EpicPrefsContainer ();
					}
				}
			}

			defaultContainer.BooleanDictionary.ToList ().ForEach (x => {
				if(!Container.BooleanDictionary.ContainsKey(x.Key)){
					Container.BooleanDictionary [x.Key] = x.Value;
				}
			});
			loadedContainer.BooleanDictionary.ToList ().ForEach (x => Container.BooleanDictionary [x.Key] = x.Value);

			defaultContainer.BooleanDictionaryDictionary.ToList ().ForEach (x => {
				if(!Container.BooleanDictionaryDictionary.ContainsKey(x.Key)){
					Container.BooleanDictionaryDictionary [x.Key] = x.Value;
				}
			});
			loadedContainer.BooleanDictionaryDictionary.ToList ().ForEach (x => Container.BooleanDictionaryDictionary [x.Key] = x.Value);

			defaultContainer.BooleanListDictionary.ToList ().ForEach (x => {
				if(!Container.BooleanListDictionary.ContainsKey(x.Key)){
					Container.BooleanListDictionary [x.Key] = x.Value;
				}
			});
			loadedContainer.BooleanListDictionary.ToList ().ForEach (x => Container.BooleanListDictionary [x.Key] = x.Value);

			defaultContainer.ColorDictionary.ToList ().ForEach (x => {
				if(!Container.ColorDictionary.ContainsKey(x.Key)){
					Container.ColorDictionary [x.Key] = x.Value;
				}
			});
			loadedContainer.ColorDictionary.ToList ().ForEach (x => Container.ColorDictionary [x.Key] = x.Value);

			defaultContainer.DoubleArrayDictionary.ToList ().ForEach (x => {
				if(!Container.DoubleArrayDictionary.ContainsKey(x.Key)){
					Container.DoubleArrayDictionary [x.Key] = x.Value;
				}
			});
			loadedContainer.DoubleArrayDictionary.ToList ().ForEach (x => Container.DoubleArrayDictionary [x.Key] = x.Value);

			defaultContainer.DoubleDictionary.ToList ().ForEach (x => {
				if(!Container.DoubleDictionary.ContainsKey(x.Key)){
					Container.DoubleDictionary [x.Key] = x.Value;
				}
			});
			loadedContainer.DoubleDictionary.ToList ().ForEach (x => Container.DoubleDictionary [x.Key] = x.Value);

			defaultContainer.DoubleDictionaryDictionary.ToList ().ForEach (x => {
				if(!Container.DoubleDictionaryDictionary.ContainsKey(x.Key)){
					Container.DoubleDictionaryDictionary [x.Key] = x.Value;
				}
			});
			loadedContainer.DoubleDictionaryDictionary.ToList ().ForEach (x => Container.DoubleDictionaryDictionary [x.Key] = x.Value);

			defaultContainer.DoubleListDictionary.ToList ().ForEach (x => {
				if(!Container.DoubleListDictionary.ContainsKey(x.Key)){
					Container.DoubleListDictionary [x.Key] = x.Value;
				}
			});
			loadedContainer.DoubleListDictionary.ToList ().ForEach (x => Container.DoubleListDictionary [x.Key] = x.Value);

			defaultContainer.FloatArrayDictionary.ToList ().ForEach (x => {
				if(!Container.FloatArrayDictionary.ContainsKey(x.Key)){
					Container.FloatArrayDictionary [x.Key] = x.Value;
				}
			});
			loadedContainer.FloatArrayDictionary.ToList ().ForEach (x => Container.FloatArrayDictionary [x.Key] = x.Value);

			defaultContainer.FloatDictionary.ToList ().ForEach (x => {
				if(!Container.FloatDictionary.ContainsKey(x.Key)){
					Container.FloatDictionary [x.Key] = x.Value;
				}
			});
			loadedContainer.FloatDictionary.ToList ().ForEach (x => Container.FloatDictionary [x.Key] = x.Value);

			defaultContainer.FloatDictionary.ToList ().ForEach (x => {
				if(!Container.FloatDictionary.ContainsKey(x.Key)){
					Container.FloatDictionary [x.Key] = x.Value;
				}
			});
			loadedContainer.FloatDictionary.ToList ().ForEach (x => Container.FloatDictionary [x.Key] = x.Value);

			defaultContainer.FloatDictionaryDictionary.ToList ().ForEach (x => {
				if(!Container.FloatDictionaryDictionary.ContainsKey(x.Key)){
					Container.FloatDictionaryDictionary [x.Key] = x.Value;
				}
			});
			loadedContainer.FloatDictionaryDictionary.ToList ().ForEach (x => Container.FloatDictionaryDictionary [x.Key] = x.Value);

			defaultContainer.FloatListDictionary.ToList ().ForEach (x => {
				if(!Container.FloatListDictionary.ContainsKey(x.Key)){
					Container.FloatListDictionary [x.Key] = x.Value;
				}
			});
			loadedContainer.FloatListDictionary.ToList ().ForEach (x => Container.FloatListDictionary [x.Key] = x.Value);

			defaultContainer.IntegerArrayDictionary.ToList ().ForEach (x => {
				if(!Container.IntegerArrayDictionary.ContainsKey(x.Key)){
					Container.IntegerArrayDictionary [x.Key] = x.Value;
				}
			});
			loadedContainer.IntegerArrayDictionary.ToList ().ForEach (x => Container.IntegerArrayDictionary [x.Key] = x.Value);

			defaultContainer.IntegerDictionary.ToList ().ForEach (x => {
				if(!Container.IntegerDictionary.ContainsKey(x.Key)){
					Container.IntegerDictionary [x.Key] = x.Value;
				}
			});
			loadedContainer.IntegerDictionary.ToList ().ForEach (x => Container.IntegerDictionary [x.Key] = x.Value);

			defaultContainer.IntegerDictionaryDictionary.ToList ().ForEach (x => {
				if(!Container.IntegerDictionaryDictionary.ContainsKey(x.Key)){
					Container.IntegerDictionaryDictionary [x.Key] = x.Value;
				}
			});
			loadedContainer.IntegerDictionaryDictionary.ToList ().ForEach (x => Container.IntegerDictionaryDictionary [x.Key] = x.Value);

			defaultContainer.IntegerListDictionary.ToList ().ForEach (x => {
				if(!Container.IntegerListDictionary.ContainsKey(x.Key)){
					Container.IntegerListDictionary [x.Key] = x.Value;
				}
			});
			loadedContainer.IntegerListDictionary.ToList ().ForEach (x => Container.IntegerListDictionary [x.Key] = x.Value);

			defaultContainer.IntegerListDictionary.ToList ().ForEach (x => {
				if(!Container.IntegerListDictionary.ContainsKey(x.Key)){
					Container.IntegerListDictionary [x.Key] = x.Value;
				}
			});
			loadedContainer.IntegerListDictionary.ToList ().ForEach (x => Container.IntegerListDictionary [x.Key] = x.Value);

			defaultContainer.LongDictionary.ToList ().ForEach (x => {
				if(!Container.LongDictionary.ContainsKey(x.Key)){
					Container.LongDictionary [x.Key] = x.Value;
				}
			});
			loadedContainer.LongDictionary.ToList ().ForEach (x => Container.LongDictionary [x.Key] = x.Value);

			defaultContainer.LongDictionaryDictionary.ToList ().ForEach (x => {
				if(!Container.LongDictionaryDictionary.ContainsKey(x.Key)){
					Container.LongDictionaryDictionary [x.Key] = x.Value;
				}
			});
			loadedContainer.LongDictionaryDictionary.ToList ().ForEach (x => Container.LongDictionaryDictionary [x.Key] = x.Value);

			defaultContainer.LongListDictionary.ToList ().ForEach (x => {
				if(!Container.LongListDictionary.ContainsKey(x.Key)){
					Container.LongListDictionary [x.Key] = x.Value;
				}
			});
			loadedContainer.LongListDictionary.ToList ().ForEach (x => Container.LongListDictionary [x.Key] = x.Value);

			defaultContainer.QuaternionDictionary.ToList ().ForEach (x => {
				if(!Container.QuaternionDictionary.ContainsKey(x.Key)){
					Container.QuaternionDictionary [x.Key] = x.Value;
				}
			});
			loadedContainer.QuaternionDictionary.ToList ().ForEach (x => Container.QuaternionDictionary [x.Key] = x.Value);

			defaultContainer.StringArrayDictionary.ToList ().ForEach (x => {
				if(!Container.StringArrayDictionary.ContainsKey(x.Key)){
					Container.StringArrayDictionary [x.Key] = x.Value;
				}
			});
			loadedContainer.StringArrayDictionary.ToList ().ForEach (x => Container.StringArrayDictionary [x.Key] = x.Value);

			defaultContainer.StringDictionary.ToList ().ForEach (x => {
				if(!Container.StringDictionary.ContainsKey(x.Key)){
					Container.StringDictionary [x.Key] = x.Value;
				}
			});
			loadedContainer.StringDictionary.ToList ().ForEach (x => Container.StringDictionary [x.Key] = x.Value);

			defaultContainer.StringDictionaryDictionary.ToList ().ForEach (x => {
				if(!Container.StringDictionaryDictionary.ContainsKey(x.Key)){
					Container.StringDictionaryDictionary [x.Key] = x.Value;
				}
			});
			loadedContainer.StringDictionaryDictionary.ToList ().ForEach (x => Container.StringDictionaryDictionary [x.Key] = x.Value);

			defaultContainer.StringListDictionary.ToList ().ForEach (x => {
				if(!Container.StringListDictionary.ContainsKey(x.Key)){
					Container.StringListDictionary [x.Key] = x.Value;
				}
			});
			loadedContainer.StringListDictionary.ToList ().ForEach (x => Container.StringListDictionary [x.Key] = x.Value);

			defaultContainer.Vector2Dictionary.ToList ().ForEach (x => {
				if(!Container.Vector2Dictionary.ContainsKey(x.Key)){
					Container.Vector2Dictionary [x.Key] = x.Value;
				}
			});
			loadedContainer.Vector2Dictionary.ToList ().ForEach (x => Container.Vector2Dictionary [x.Key] = x.Value);

			defaultContainer.Vector3Dictionary.ToList ().ForEach (x => {
				if(!Container.Vector3Dictionary.ContainsKey(x.Key)){
					Container.Vector3Dictionary [x.Key] = x.Value;
				}
			});
			loadedContainer.Vector3Dictionary.ToList ().ForEach (x => Container.Vector3Dictionary [x.Key] = x.Value);

			defaultContainer.Vector4Dictionary.ToList ().ForEach (x => {
				if(!Container.Vector4Dictionary.ContainsKey(x.Key)){
					Container.Vector4Dictionary [x.Key] = x.Value;
				}
			});
			loadedContainer.Vector4Dictionary.ToList ().ForEach (x => Container.Vector4Dictionary [x.Key] = x.Value);

			return Container;
		}

		public static void SaveEpicPrefsContainer (EpicPrefsContainer epicPrefsContainer, bool encrypted)
		{
			var filePath = "";
			#if UNITY_EDITOR
			filePath = Application.dataPath;
			filePath = filePath.Substring(0,filePath.IndexOf("/Assets"));
			if (!Directory.Exists(filePath + "/HotTotem"))
			{
				Directory.CreateDirectory(filePath + "/HotTotem/");
				Directory.CreateDirectory(filePath + "/HotTotem/EpicPrefsData");
				Directory.CreateDirectory(filePath + "/HotTotem/EpicPrefsData/Encrypted");
				Directory.CreateDirectory(filePath + "/HotTotem/EpicPrefsData/NotEncrypted");
			}
			#endif
			if (encrypted) {
				#if UNITY_EDITOR
				filePath = filePath + "/HotTotem/EpicPrefsData/Encrypted/";
				#else
				filePath = Application.persistentDataPath + "/HotTotem/EpicPrefsData/Encrypted/";
				#endif
			}
			else {
				#if UNITY_EDITOR
				filePath = filePath + "/HotTotem/EpicPrefsData/NotEncrypted/";
				#else
				filePath = Application.persistentDataPath + "/HotTotem/EpicPrefsData/NotEncrypted/";
				#endif
			}
			var bf = new BinaryFormatter ();
			var ss = new SurrogateSelector ();
			ss.AddSurrogate (typeof(Dictionary<string,Vector2>),
				new StreamingContext (StreamingContextStates.All),
				new Vector2DictSerializationSurrogate ());
			ss.AddSurrogate (typeof(Dictionary<string,Vector3>),
				new StreamingContext (StreamingContextStates.All),
				new Vector3DictSerializationSurrogate ());
			ss.AddSurrogate (typeof(Dictionary<string,Vector4>),
				new StreamingContext (StreamingContextStates.All),
				new Vector4DictSerializationSurrogate ());
			ss.AddSurrogate (typeof(Dictionary<string,Color>),
				new StreamingContext (StreamingContextStates.All),
				new ColorDictSerializationSurrogate ());
			ss.AddSurrogate (typeof(Dictionary<string,Quaternion>),
				new StreamingContext (StreamingContextStates.All),
				new QuaternionDictSerializationSurrogate ());
			// Associate the SurrogateSelector with the BinaryFormatter.
			bf.SurrogateSelector = ss;
			if (!Directory.Exists (filePath)) {
				Directory.CreateDirectory (filePath);
			}
			if (encrypted) {
				var memStream = new MemoryStream ();
				bf.Serialize (memStream, epicPrefsContainer);
				File.WriteAllText(filePath + "EpicPrefsContainer.epic", Cryptor.Encrypt(memStream.ToArray()));
			} else {
				FileStream fs;
				fs = new FileStream (filePath + "EpicPrefsContainer.epic", FileMode.Create);
				bf.Serialize (fs, epicPrefsContainer);
				fs.Close ();
			}
		}
		#if UNITY_EDITOR
		public static void ExportEpicPrefsContainer (EpicPrefsContainer epicPrefsContainer, bool encrypted)
		{
			var filePath = Application.dataPath + "/HotTotemAssets/EpicPrefs/Resources/HotTotemAssets/EpicPrefs";
			if (encrypted)
				filePath += "/Encrypted/";
			else
				filePath += "/NotEncrypted/";
			var bf = new BinaryFormatter ();
			var ss = new SurrogateSelector ();
			ss.AddSurrogate (typeof(Dictionary<string,Vector2>),
				new StreamingContext (StreamingContextStates.All),
				new Vector2DictSerializationSurrogate ());
			ss.AddSurrogate (typeof(Dictionary<string,Vector3>),
				new StreamingContext (StreamingContextStates.All),
				new Vector3DictSerializationSurrogate ());
			ss.AddSurrogate (typeof(Dictionary<string,Vector4>),
				new StreamingContext (StreamingContextStates.All),
				new Vector4DictSerializationSurrogate ());
			ss.AddSurrogate (typeof(Dictionary<string,Color>),
				new StreamingContext (StreamingContextStates.All),
				new ColorDictSerializationSurrogate ());
			ss.AddSurrogate (typeof(Dictionary<string,Quaternion>),
				new StreamingContext (StreamingContextStates.All),
				new QuaternionDictSerializationSurrogate ());
			// Associate the SurrogateSelector with the BinaryFormatter.
			bf.SurrogateSelector = ss;
			if (!Directory.Exists (filePath)) {
				Directory.CreateDirectory (filePath);
			}
			if (encrypted) {
				var memStream = new MemoryStream ();
				bf.Serialize (memStream, epicPrefsContainer);
				File.WriteAllText(filePath + "EpicPrefsContainer.bytes", Cryptor.Encrypt(memStream.ToArray()));
			} else {
				FileStream fs;
				fs = new FileStream (filePath + "EpicPrefsContainer.bytes", FileMode.Create);
				bf.Serialize (fs, epicPrefsContainer);
				fs.Close ();
			}
		}
		public static void ExportDefaultEpicPrefsContainer (EpicPrefsContainer epicPrefsContainer, bool encrypted)
		{
			var filePath = Application.dataPath + "/HotTotemAssets/EpicPrefs/Resources/HotTotemAssets/EpicPrefs";
			if (encrypted)
				filePath += "/Encrypted/Default/";
			else
				filePath += "/NotEncrypted/Default/";
			var bf = new BinaryFormatter ();
			var ss = new SurrogateSelector ();
			ss.AddSurrogate (typeof(Dictionary<string,Vector2>),
				new StreamingContext (StreamingContextStates.All),
				new Vector2DictSerializationSurrogate ());
			ss.AddSurrogate (typeof(Dictionary<string,Vector3>),
				new StreamingContext (StreamingContextStates.All),
				new Vector3DictSerializationSurrogate ());
			ss.AddSurrogate (typeof(Dictionary<string,Vector4>),
				new StreamingContext (StreamingContextStates.All),
				new Vector4DictSerializationSurrogate ());
			ss.AddSurrogate (typeof(Dictionary<string,Color>),
				new StreamingContext (StreamingContextStates.All),
				new ColorDictSerializationSurrogate ());
			ss.AddSurrogate (typeof(Dictionary<string,Quaternion>),
				new StreamingContext (StreamingContextStates.All),
				new QuaternionDictSerializationSurrogate ());
			// Associate the SurrogateSelector with the BinaryFormatter.
			bf.SurrogateSelector = ss;
			if (!Directory.Exists (filePath)) {
				Directory.CreateDirectory (filePath);
			}
			if (encrypted) {
				var memStream = new MemoryStream ();
				bf.Serialize (memStream, epicPrefsContainer);
				File.WriteAllText(filePath + "EpicPrefsContainer.bytes", Cryptor.Encrypt(memStream.ToArray()));
			} else {
				FileStream fs;
				fs = new FileStream (filePath + "EpicPrefsContainer.bytes", FileMode.Create);
				bf.Serialize (fs, epicPrefsContainer);
				fs.Close ();
			}
		}
		#endif
		public static EpicPrefsContainer Copy(EpicPrefsContainer epicPrefsContainer){
			MemoryStream ms = new MemoryStream();
			BinaryFormatter bf = new BinaryFormatter();
			var ss = new SurrogateSelector ();
			ss.AddSurrogate (typeof(Dictionary<string,Vector2>),
				new StreamingContext (StreamingContextStates.All),
				new Vector2DictSerializationSurrogate ());
			ss.AddSurrogate (typeof(Dictionary<string,Vector3>),
				new StreamingContext (StreamingContextStates.All),
				new Vector3DictSerializationSurrogate ());
			ss.AddSurrogate (typeof(Dictionary<string,Vector4>),
				new StreamingContext (StreamingContextStates.All),
				new Vector4DictSerializationSurrogate ());
			ss.AddSurrogate (typeof(Dictionary<string,Color>),
				new StreamingContext (StreamingContextStates.All),
				new ColorDictSerializationSurrogate ());
			ss.AddSurrogate (typeof(Dictionary<string,Quaternion>),
				new StreamingContext (StreamingContextStates.All),
				new QuaternionDictSerializationSurrogate ());
			// Associate the SurrogateSelector with the BinaryFormatter.
			bf.SurrogateSelector = ss;
			bf.Serialize(ms, epicPrefsContainer);
			ms.Position = 0;
			object obj = bf.Deserialize(ms);
			ms.Close();
			return obj as EpicPrefsContainer;
		}
	}
}

