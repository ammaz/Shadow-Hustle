using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.Runtime.Serialization;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace EpicPrefsEditorTools
{
	#if UNITY_EDITOR
	[Serializable]
	public class EpicPrefsEditorContainer
	{
		public Dictionary<string, int> IntegerDictionary;
		public Dictionary<string, float> FloatDictionary;
		public Dictionary<string, bool> BooleanDictionary;
		public Dictionary<string, string> StringDictionary;
		public Dictionary<string, double> DoubleDictionary;
		public Dictionary<string, long> LongDictionary;
		public Dictionary<string, int[]> IntegerArrayDictionary;
		public Dictionary<string, string[]> StringArrayDictionary;
		public Dictionary<string, float[]> FloatArrayDictionary;
		public Dictionary<string, double[]> DoubleArrayDictionary;
		public Dictionary<string, List<string>> StringListDictionary;
		public Dictionary<string, List<int>> IntegerListDictionary;
		public Dictionary<string, List<bool>> BooleanListDictionary;
		public Dictionary<string, List<float>> FloatListDictionary;
		public Dictionary<string, List<double>> DoubleListDictionary;
		public Dictionary<string, List<long>> LongListDictionary;
		public Dictionary<string, Dictionary<string,string>> StringDictionaryDictionary;
		public Dictionary<string, Dictionary<string,int>> IntegerDictionaryDictionary;
		public Dictionary<string, Dictionary<string,bool>> BooleanDictionaryDictionary;
		public Dictionary<string, Dictionary<string,float>> FloatDictionaryDictionary;
		public Dictionary<string, Dictionary<string,double>> DoubleDictionaryDictionary;
		public Dictionary<string, Dictionary<string,long>> LongDictionaryDictionary;
		public Dictionary<string, Quaternion> QuaternionDictionary;
		public Dictionary<string, Vector2> Vector2Dictionary;
		public Dictionary<string, Vector3> Vector3Dictionary;
		public Dictionary<string, Vector4> Vector4Dictionary;
		public Dictionary<string, Color> ColorDictionary;
		[NonSerialized]public bool edited = false;
		public EpicPrefsEditorContainer ()
		{
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
			LoadBooleanListDictionary ();
			LoadFloatListDictionary ();
			LoadDoubleListDictionary ();
			LoadLongListDictionary ();
			LoadStringDictionaryDictionary ();
			LoadIntegerDictionaryDictionary ();
			LoadFloatDictionaryDictionary ();
			LoadDoubleDictionaryDictionary ();
			LoadLongDictionaryDictionary ();
			LoadBooleanDictionaryDictionary ();
			LoadQuaternionDictionary ();
			LoadVector2Dictionary ();
			LoadVector3Dictionary ();
			LoadVector4Dictionary ();
			LoadColorDictionary ();

		}
		private void LoadIntegerDictionary ()
		{
			IntegerDictionary = new Dictionary<string, int> ();
		}
		//Save&Load FloatDictionary
		private void LoadFloatDictionary ()
		{
			FloatDictionary = new Dictionary<string, float> ();
		}

		//Save&Load BooleanDictionary
		private void LoadBooleanDictionary ()
		{
			BooleanDictionary = new Dictionary<string, bool> ();
		}

		//Save&Load StringDictionary
		private void LoadStringDictionary ()
		{
			StringDictionary = new Dictionary<string, string> ();
		}

		//Save&Load DoubleDictionary
		private void LoadDoubleDictionary ()
		{
			DoubleDictionary = new Dictionary<string, double> ();
		}

		//Save&Load LongDictionary
		private void LoadLongDictionary ()
		{
			LongDictionary = new Dictionary<string, long> ();
		}
		//Save&Load IntegerArrayDictionary
		private void LoadIntegerArrayDictionary ()
		{
			IntegerArrayDictionary = new Dictionary<string, int[]> ();
		}

		//Save&Load StringArrayDictionary
		private void LoadStringArrayDictionary ()
		{
			StringArrayDictionary = new Dictionary<string, string[]> ();
		}


		//Save&Load FloatArrayDictionary
		private void LoadFloatArrayDictionary ()
		{
			FloatArrayDictionary = new Dictionary<string, float[]> ();
		}


		//Save&Load DoubleArrayDictionary
		private void LoadDoubleArrayDictionary ()
		{
			DoubleArrayDictionary = new Dictionary<string, double[]> ();
		}

		//Save&Load StringListDictionary
		private void LoadStringListDictionary ()
		{
			StringListDictionary = new Dictionary<string, List<string>> ();
		}
		private void LoadIntegerListDictionary ()
		{
			IntegerListDictionary = new Dictionary<string, List<int>> ();
		}
		//Save&Load BooleanListDictionary
		private void LoadBooleanListDictionary ()
		{
			BooleanListDictionary = new Dictionary<string, List<bool>> ();
		}


		//Save&Load FloatListDictionary
		private void LoadFloatListDictionary ()
		{
			FloatListDictionary = new Dictionary<string, List<float>> ();
		}

		//Save&Load DoubleListDictionary
		private void LoadDoubleListDictionary ()
		{
			DoubleListDictionary = new Dictionary<string, List<double>> ();
		}

		//Save&Load LongListDictionary
		private void LoadLongListDictionary ()
		{
			LongListDictionary = new Dictionary<string, List<long>> ();
		}
		//Save&Load StringDictionaryDictionary
		private void LoadStringDictionaryDictionary ()
		{
			StringDictionaryDictionary = new Dictionary<string, Dictionary<string,string>> ();
		}

		//Save&Load IntegerDictionaryDictionary
		private void LoadIntegerDictionaryDictionary ()
		{
			IntegerDictionaryDictionary = new Dictionary<string, Dictionary<string,int>> ();
		}
		//Save&Load BooleanDictionaryDictionary
		private void LoadBooleanDictionaryDictionary ()
		{
			BooleanDictionaryDictionary = new Dictionary<string, Dictionary<string,bool>> ();
		}

		//Save&Load FloatDictionaryDictionary
		private void LoadFloatDictionaryDictionary ()
		{
			FloatDictionaryDictionary = new Dictionary<string, Dictionary<string,float>> ();
		}
		//Save&Load DoubleDictionaryDictionary
		private void LoadDoubleDictionaryDictionary ()
		{
			DoubleDictionaryDictionary = new Dictionary<string, Dictionary<string,double>> ();
		}

		//Save&Load LongDictionaryDictionary
		private void LoadLongDictionaryDictionary ()
		{
			LongDictionaryDictionary = new Dictionary<string, Dictionary<string,long>> ();
		}

		//Save&Load QuaternionDictionary
		private void LoadQuaternionDictionary ()
		{
			QuaternionDictionary = new Dictionary<string, Quaternion> ();
		}

		//Save&Load Vector2Dictionary
		private void LoadVector2Dictionary ()
		{
			Vector2Dictionary = new Dictionary<string, Vector2> ();
		}

		//Save&Load Vector3Dictionary
		private void LoadVector3Dictionary ()
		{
			Vector3Dictionary = new Dictionary<string, Vector3> ();
		}


		//Save&Load Vector4Dictionary
		private void LoadVector4Dictionary ()
		{
			Vector4Dictionary = new Dictionary<string, Vector4> ();
		}



		//Save&Load ColorDictionary
		private void LoadColorDictionary ()
		{
			ColorDictionary = new Dictionary<string, Color> ();
		}
	}


	public static class EpicPrefsEditorContainerHandler
	{
		//Save&Load Vector2Dictionary
		public static EpicPrefsEditorContainer LoadEpicPrefsEditorContainer (bool encrypted)
		{
			FileStream fs;
			EpicPrefsEditorContainer loadedContainer = new EpicPrefsEditorContainer ();
			var filePath = "";
			if(encrypted)
				filePath = Application.dataPath + "/HotTotemAssets/EpicPrefs/EpicEditorPrefs/Encrypted/";
			else	
				filePath = Application.dataPath + "/HotTotemAssets/EpicPrefs/EpicEditorPrefs/NotEncrypted/";
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
			if (File.Exists (filePath + "EpicEditorPrefsContainer.epic")) {
				fs = new FileStream (filePath + "EpicEditorPrefsContainer.epic", FileMode.Open);
				if (fs.Length == 0) {
					fs.Close ();
					return loadedContainer;
				}
				try {
					var obj = bf.Deserialize (fs);
					loadedContainer = (EpicPrefsEditorContainer)obj;
				} catch (Exception e) {
					Debug.LogError ("EpicPrefs Error : " + e);
				}
				fs.Close ();
			} 
			return loadedContainer;
		}

		public static void SaveEpicPrefsEditorContainer (EpicPrefsEditorContainer epicPrefsEditorContainer,bool encrypted)
		{
			var filePath = "";
			if (epicPrefsEditorContainer.edited) {
				if (encrypted)
					filePath = Application.dataPath + "/HotTotemAssets/EpicPrefs/EpicEditorPrefs/Encrypted/";
				else
					filePath = Application.dataPath + "/HotTotemAssets/EpicPrefs/EpicEditorPrefs/NotEncrypted/";
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
				FileStream fs;
				fs = new FileStream (filePath + "EpicEditorPrefsContainer.epic", FileMode.Create);
				bf.Serialize (fs, epicPrefsEditorContainer);
				fs.Close ();
			}
		}
	}
	#endif
}

