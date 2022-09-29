using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using EpicPrefsTools;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace EpicPrefsEditorTools {
	#if UNITY_EDITOR
	[InitializeOnLoad]
	public class EpicPrefsEditor : EditorWindow {
		#region Declarations
		public static EpicPrefsEditor instance;
		private static List<string> editKeysNE,editKeysE;
		private static List<object> editValuesNE,editValuesE;
		private static List<Serializer.SerializationTypes> editTypesNE;
		private static List<Serializer.SerializationTypes> editTypesE;
		private static bool batchWrite = false;
		private Vector2 scrollPos = new Vector2 (0, 0);
		private GenericMenu addMenu;
		private string editText = "Edit";
		private bool edit = false;
		public static bool passChanged = false;
		private Dictionary<string,bool> foldouts;
		public static EpicPrefsEditorContainer epicPrefsContainerNotEncrypted,epicPrefsContainerEncrypted;
		#endregion

		#region Initializiation
		[MenuItem("Tools/HotTotemAssets/EpicPrefs/EpicPrefsEditor %#e")]
		static void Init()
		{
			StartUp ();
			if (instance == null) {
				instance = (EpicPrefsEditor)EditorWindow.GetWindow (typeof(EpicPrefsEditor));
				instance.foldouts = new Dictionary<string,bool> ();
				instance.scrollPos = new Vector2 (0, 0);
				#if UNITY_5
					#if UNITY_5_0
						instance.title = "EpicPrefsEditor";
					#else
						instance.titleContent = new GUIContent("EpicPrefsEditor");
					#endif
				#else
					instance.title = "EpicPrefsEditor";
				#endif
				instance.InitMenu ();
			}
		}
		public static void StartUp(){
			if (passChanged) {
				epicPrefsContainerEncrypted = null;
				epicPrefsContainerNotEncrypted = null;
			}
			if (epicPrefsContainerEncrypted == null || epicPrefsContainerNotEncrypted == null) {
				epicPrefsContainerEncrypted = EpicPrefsEditorContainerHandler.LoadEpicPrefsEditorContainer (true);
				epicPrefsContainerNotEncrypted = EpicPrefsEditorContainerHandler.LoadEpicPrefsEditorContainer (false);
				passChanged = false;
			}
		}
		public static void Exit(){
			if (epicPrefsContainerEncrypted != null && epicPrefsContainerNotEncrypted != null) {
				EpicPrefsEditorContainerHandler.SaveEpicPrefsEditorContainer (epicPrefsContainerEncrypted,true);
				EpicPrefsEditorContainerHandler.SaveEpicPrefsEditorContainer (epicPrefsContainerNotEncrypted,false);
			}
			if (!System.IO.Directory.Exists (Application.dataPath + "/HotTotemAssets/EpicPrefs/Resources/HotTotemAssets/EpicPrefs/Settings")) {
				Operators.CopySettings ();
				AssetDatabase.Refresh ();	
			}
		}
		public static void SetBatchWrite(bool value){
			batchWrite = value;
		}
		#endregion
		void OnGUI () {
			Init ();
			EditorGUILayout.BeginHorizontal ();
			GUILayout.Label ("EpicPrefsEditor", EditorStyles.boldLabel);
			if (GUILayout.Button ("Add",GUILayout.ExpandWidth (false))) {
				addMenu.ShowAsContext ();
			}
			if (GUILayout.Button (editText,GUILayout.ExpandWidth (false))) {
				if (!edit) {
					editText = "Save";
					StartEdit ();
				} else {
					editText = "Edit";
					EndEdit ();
				}
				edit = !edit;
			}
			EditorGUILayout.EndHorizontal ();
			scrollPos = EditorGUILayout.BeginScrollView (scrollPos, false, false);
			foreach (Serializer.SerializationTypes type in Enum.GetValues(typeof(Serializer.SerializationTypes))) {
				DisplaySimpleDict (type, edit);
				DisplayComplexDict (type, edit);
				DisplayListsAndArrays (type, edit);
				DisplayVectors (type, edit);
				DisplayColors (type, edit);
			}
			EditorGUILayout.EndScrollView ();
		}
		#region Edit
		private static void StartEdit(){
			editKeysNE = new List<string> ();
			editValuesNE = new List<object> ();
			editTypesNE = new List<Serializer.SerializationTypes> ();
			editKeysE = new List<string> ();
			editValuesE = new List<object> ();

			editTypesE = new List<Serializer.SerializationTypes> ();
			#region IntegerDictionary
			editKeysNE.AddRange (epicPrefsContainerNotEncrypted.IntegerDictionary.Keys.ToList ());
			for (int i = 0; i < epicPrefsContainerNotEncrypted.IntegerDictionary.Keys.Count; i++) {
				editValuesNE.Add((object)((epicPrefsContainerNotEncrypted.IntegerDictionary.Values.ToArray())[i]));
				editTypesNE.Add (Serializer.SerializationTypes.Integer);
			}
			editKeysE.AddRange (epicPrefsContainerEncrypted.IntegerDictionary.Keys.ToList ());
			for (int i = 0; i < epicPrefsContainerEncrypted.IntegerDictionary.Keys.Count; i++) {
				editValuesE.Add((object)((epicPrefsContainerEncrypted.IntegerDictionary.Values.ToArray())[i]));
				editTypesE.Add (Serializer.SerializationTypes.Integer);
			}
			#endregion
			#region FloatDictionary
			editKeysNE.AddRange (epicPrefsContainerNotEncrypted.FloatDictionary.Keys.ToList ());
			for (int i = 0; i < epicPrefsContainerNotEncrypted.FloatDictionary.Keys.Count; i++) {
				editValuesNE.Add((object)((epicPrefsContainerNotEncrypted.FloatDictionary.Values.ToArray())[i]));
				editTypesNE.Add (Serializer.SerializationTypes.Float);
			}
			editKeysE.AddRange (epicPrefsContainerEncrypted.FloatDictionary.Keys.ToList ());
			for (int i = 0; i < epicPrefsContainerEncrypted.FloatDictionary.Keys.Count; i++) {
				editValuesE.Add((object)((epicPrefsContainerEncrypted.FloatDictionary.Values.ToArray())[i]));
				editTypesE.Add (Serializer.SerializationTypes.Float);
			}
			#endregion
			#region BooleanDictionary
			editKeysNE.AddRange (epicPrefsContainerNotEncrypted.BooleanDictionary.Keys.ToList ());
			for (int i = 0; i < epicPrefsContainerNotEncrypted.BooleanDictionary.Keys.Count; i++) {
				editValuesNE.Add((object)((epicPrefsContainerNotEncrypted.BooleanDictionary.Values.ToArray())[i]));
				editTypesNE.Add (Serializer.SerializationTypes.Bool);
			}
			editKeysE.AddRange (epicPrefsContainerEncrypted.BooleanDictionary.Keys.ToList ());
			for (int i = 0; i < epicPrefsContainerEncrypted.BooleanDictionary.Keys.Count; i++) {
				editValuesE.Add((object)((epicPrefsContainerEncrypted.BooleanDictionary.Values.ToArray())[i]));
				editTypesE.Add (Serializer.SerializationTypes.Bool);
			}
			#endregion
			#region StringDictionary
			editKeysNE.AddRange (epicPrefsContainerNotEncrypted.StringDictionary.Keys.ToList ());
			for (int i = 0; i < epicPrefsContainerNotEncrypted.StringDictionary.Keys.Count; i++) {
				editValuesNE.Add((object)((epicPrefsContainerNotEncrypted.StringDictionary.Values.ToArray())[i]));
				editTypesNE.Add (Serializer.SerializationTypes.String);
			}
			editKeysE.AddRange (epicPrefsContainerEncrypted.StringDictionary.Keys.ToList ());
			for (int i = 0; i < epicPrefsContainerEncrypted.StringDictionary.Keys.Count; i++) {
				editValuesE.Add((object)((epicPrefsContainerEncrypted.StringDictionary.Values.ToArray())[i]));
				editTypesE.Add (Serializer.SerializationTypes.String);
			}
			#endregion
			#region DoubleDictionary
			editKeysNE.AddRange (epicPrefsContainerNotEncrypted.DoubleDictionary.Keys.ToList ());
			for (int i = 0; i < epicPrefsContainerNotEncrypted.DoubleDictionary.Keys.Count; i++) {
				editValuesNE.Add((object)((epicPrefsContainerNotEncrypted.DoubleDictionary.Values.ToArray())[i]));
				editTypesNE.Add (Serializer.SerializationTypes.Double);
			}
			editKeysE.AddRange (epicPrefsContainerEncrypted.DoubleDictionary.Keys.ToList ());
			for (int i = 0; i < epicPrefsContainerEncrypted.DoubleDictionary.Keys.Count; i++) {
				editValuesE.Add((object)((epicPrefsContainerEncrypted.DoubleDictionary.Values.ToArray())[i]));
				editTypesE.Add (Serializer.SerializationTypes.Double);
			}
			#endregion
			#region LongDictionary
			editKeysNE.AddRange (epicPrefsContainerNotEncrypted.LongDictionary.Keys.ToList ());
			for (int i = 0; i < epicPrefsContainerNotEncrypted.LongDictionary.Keys.Count; i++) {
				editValuesNE.Add((object)((epicPrefsContainerNotEncrypted.LongDictionary.Values.ToArray())[i]));
				editTypesNE.Add (Serializer.SerializationTypes.Long);
			}
			editKeysE.AddRange (epicPrefsContainerEncrypted.LongDictionary.Keys.ToList ());
			for (int i = 0; i < epicPrefsContainerEncrypted.LongDictionary.Keys.Count; i++) {
				editValuesE.Add((object)((epicPrefsContainerEncrypted.LongDictionary.Values.ToArray())[i]));
				editTypesE.Add (Serializer.SerializationTypes.Long);
			}
			#endregion
			#region IntegerArrayDictionary
			editKeysNE.AddRange (epicPrefsContainerNotEncrypted.IntegerArrayDictionary.Keys.ToList ());
			for (int i = 0; i < epicPrefsContainerNotEncrypted.IntegerArrayDictionary.Keys.Count; i++) {
				editValuesNE.Add((object)((epicPrefsContainerNotEncrypted.IntegerArrayDictionary.Values.ToArray())[i]));
				editTypesNE.Add (Serializer.SerializationTypes.ArrayInt);
			}
			editKeysE.AddRange (epicPrefsContainerEncrypted.IntegerArrayDictionary.Keys.ToList ());
			for (int i = 0; i < epicPrefsContainerEncrypted.IntegerArrayDictionary.Keys.Count; i++) {
				editValuesE.Add((object)((epicPrefsContainerEncrypted.IntegerArrayDictionary.Values.ToArray())[i]));
				editTypesE.Add (Serializer.SerializationTypes.ArrayInt);
			}
			#endregion
			#region StringArrayDictionary
			editKeysNE.AddRange (epicPrefsContainerNotEncrypted.StringArrayDictionary.Keys.ToList ());
			for (int i = 0; i < epicPrefsContainerNotEncrypted.StringArrayDictionary.Keys.Count; i++) {
				editValuesNE.Add((object)((epicPrefsContainerNotEncrypted.StringArrayDictionary.Values.ToArray())[i]));
				editTypesNE.Add (Serializer.SerializationTypes.ArrayString);
			}
			editKeysE.AddRange (epicPrefsContainerEncrypted.StringArrayDictionary.Keys.ToList ());
			for (int i = 0; i < epicPrefsContainerEncrypted.StringArrayDictionary.Keys.Count; i++) {
				editValuesE.Add((object)((epicPrefsContainerEncrypted.StringArrayDictionary.Values.ToArray())[i]));
				editTypesE.Add (Serializer.SerializationTypes.ArrayString);
			}
			#endregion
			#region FloatArrayDictionary
			editKeysNE.AddRange (epicPrefsContainerNotEncrypted.FloatArrayDictionary.Keys.ToList ());
			for (int i = 0; i < epicPrefsContainerNotEncrypted.FloatArrayDictionary.Keys.Count; i++) {
				editValuesNE.Add((object)((epicPrefsContainerNotEncrypted.FloatArrayDictionary.Values.ToArray())[i]));
				editTypesNE.Add (Serializer.SerializationTypes.ArrayFloat);
			}
			editKeysE.AddRange (epicPrefsContainerEncrypted.FloatArrayDictionary.Keys.ToList ());
			for (int i = 0; i < epicPrefsContainerEncrypted.FloatArrayDictionary.Keys.Count; i++) {
				editValuesE.Add((object)((epicPrefsContainerEncrypted.FloatArrayDictionary.Values.ToArray())[i]));
				editTypesE.Add (Serializer.SerializationTypes.ArrayFloat);
			}
			#endregion
			#region DoubleArrayDictionary
			editKeysNE.AddRange (epicPrefsContainerNotEncrypted.DoubleArrayDictionary.Keys.ToList ());
			for (int i = 0; i < epicPrefsContainerNotEncrypted.DoubleArrayDictionary.Keys.Count; i++) {
				editValuesNE.Add((object)((epicPrefsContainerNotEncrypted.DoubleArrayDictionary.Values.ToArray())[i]));
				editTypesNE.Add (Serializer.SerializationTypes.ArrayDouble);
			}
			editKeysE.AddRange (epicPrefsContainerEncrypted.DoubleArrayDictionary.Keys.ToList ());
			for (int i = 0; i < epicPrefsContainerEncrypted.DoubleArrayDictionary.Keys.Count; i++) {
				editValuesE.Add((object)((epicPrefsContainerEncrypted.DoubleArrayDictionary.Values.ToArray())[i]));
				editTypesE.Add (Serializer.SerializationTypes.ArrayDouble);
			}
			#endregion
			#region StringListDictionary
			editKeysNE.AddRange (epicPrefsContainerNotEncrypted.StringListDictionary.Keys.ToList ());
			for (int i = 0; i < epicPrefsContainerNotEncrypted.StringListDictionary.Keys.Count; i++) {
				editValuesNE.Add((object)((epicPrefsContainerNotEncrypted.StringListDictionary.Values.ToArray())[i]));
				editTypesNE.Add (Serializer.SerializationTypes.ListS);
			}
			editKeysE.AddRange (epicPrefsContainerEncrypted.StringListDictionary.Keys.ToList ());
			for (int i = 0; i < epicPrefsContainerEncrypted.StringListDictionary.Keys.Count; i++) {
				editValuesE.Add((object)((epicPrefsContainerEncrypted.StringListDictionary.Values.ToArray())[i]));
				editTypesE.Add (Serializer.SerializationTypes.ListS);
			}
			#endregion
			#region IntegerListDictionary
			editKeysNE.AddRange (epicPrefsContainerNotEncrypted.IntegerListDictionary.Keys.ToList ());
			for (int i = 0; i < epicPrefsContainerNotEncrypted.IntegerListDictionary.Keys.Count; i++) {
				editValuesNE.Add((object)((epicPrefsContainerNotEncrypted.IntegerListDictionary.Values.ToArray())[i]));
				editTypesNE.Add (Serializer.SerializationTypes.ListI);
			}
			editKeysE.AddRange (epicPrefsContainerEncrypted.IntegerListDictionary.Keys.ToList ());
			for (int i = 0; i < epicPrefsContainerEncrypted.IntegerListDictionary.Keys.Count; i++) {
				editValuesE.Add((object)((epicPrefsContainerEncrypted.IntegerListDictionary.Values.ToArray())[i]));
				editTypesE.Add (Serializer.SerializationTypes.ListI);
			}
			#endregion
			#region BooleanListDictionary
			editKeysNE.AddRange (epicPrefsContainerNotEncrypted.BooleanListDictionary.Keys.ToList ());
			for (int i = 0; i < epicPrefsContainerNotEncrypted.BooleanListDictionary.Keys.Count; i++) {
				editValuesNE.Add((object)((epicPrefsContainerNotEncrypted.BooleanListDictionary.Values.ToArray())[i]));
				editTypesNE.Add (Serializer.SerializationTypes.ListB);
			}
			editKeysE.AddRange (epicPrefsContainerEncrypted.BooleanListDictionary.Keys.ToList ());
			for (int i = 0; i < epicPrefsContainerEncrypted.BooleanListDictionary.Keys.Count; i++) {
				editValuesE.Add((object)((epicPrefsContainerEncrypted.BooleanListDictionary.Values.ToArray())[i]));
				editTypesE.Add (Serializer.SerializationTypes.ListB);
			}
			#endregion
			#region FloatListDictionary
			editKeysNE.AddRange (epicPrefsContainerNotEncrypted.FloatListDictionary.Keys.ToList ());
			for (int i = 0; i < epicPrefsContainerNotEncrypted.FloatListDictionary.Keys.Count; i++) {
				editValuesNE.Add((object)((epicPrefsContainerNotEncrypted.FloatListDictionary.Values.ToArray())[i]));
				editTypesNE.Add (Serializer.SerializationTypes.ListF);
			}
			editKeysE.AddRange (epicPrefsContainerEncrypted.FloatListDictionary.Keys.ToList ());
			for (int i = 0; i < epicPrefsContainerEncrypted.FloatListDictionary.Keys.Count; i++) {
				editValuesE.Add((object)((epicPrefsContainerEncrypted.FloatListDictionary.Values.ToArray())[i]));
				editTypesE.Add (Serializer.SerializationTypes.ListF);
			}
			#endregion
			#region DoubleListDictionary
			editKeysNE.AddRange (epicPrefsContainerNotEncrypted.DoubleListDictionary.Keys.ToList ());
			for (int i = 0; i < epicPrefsContainerNotEncrypted.DoubleListDictionary.Keys.Count; i++) {
				editValuesNE.Add((object)((epicPrefsContainerNotEncrypted.DoubleListDictionary.Values.ToArray())[i]));
				editTypesNE.Add (Serializer.SerializationTypes.ListD);
			}
			editKeysE.AddRange (epicPrefsContainerEncrypted.DoubleListDictionary.Keys.ToList ());
			for (int i = 0; i < epicPrefsContainerEncrypted.DoubleListDictionary.Keys.Count; i++) {
				editValuesE.Add((object)((epicPrefsContainerEncrypted.DoubleListDictionary.Values.ToArray())[i]));
				editTypesE.Add (Serializer.SerializationTypes.ListD);
			}
			#endregion
			#region LongListDictionary
			editKeysNE.AddRange (epicPrefsContainerNotEncrypted.LongListDictionary.Keys.ToList ());
			for (int i = 0; i < epicPrefsContainerNotEncrypted.LongListDictionary.Keys.Count; i++) {
				editValuesNE.Add((object)((epicPrefsContainerNotEncrypted.LongListDictionary.Values.ToArray())[i]));
				editTypesNE.Add (Serializer.SerializationTypes.ListL);
			}
			editKeysE.AddRange (epicPrefsContainerEncrypted.LongListDictionary.Keys.ToList ());
			for (int i = 0; i < epicPrefsContainerEncrypted.LongListDictionary.Keys.Count; i++) {
				editValuesE.Add((object)((epicPrefsContainerEncrypted.LongListDictionary.Values.ToArray())[i]));
				editTypesE.Add (Serializer.SerializationTypes.ListL);
			}
			#endregion
			#region StringDictionaryDictionary
			editKeysNE.AddRange (epicPrefsContainerNotEncrypted.StringDictionaryDictionary.Keys.ToList ());
			for (int i = 0; i < epicPrefsContainerNotEncrypted.StringDictionaryDictionary.Keys.Count; i++) {
				editValuesNE.Add((object)((epicPrefsContainerNotEncrypted.StringDictionaryDictionary.Values.ToArray())[i]));
				editTypesNE.Add (Serializer.SerializationTypes.DictS);
			}
			editKeysE.AddRange (epicPrefsContainerEncrypted.StringDictionaryDictionary.Keys.ToList ());
			for (int i = 0; i < epicPrefsContainerEncrypted.StringDictionaryDictionary.Keys.Count; i++) {
				editValuesE.Add((object)((epicPrefsContainerEncrypted.StringDictionaryDictionary.Values.ToArray())[i]));
				editTypesE.Add (Serializer.SerializationTypes.DictS);
			}
			#endregion
			#region IntegerDictionaryDictionary
			editKeysNE.AddRange (epicPrefsContainerNotEncrypted.IntegerDictionaryDictionary.Keys.ToList ());
			for (int i = 0; i < epicPrefsContainerNotEncrypted.IntegerDictionaryDictionary.Keys.Count; i++) {
				editValuesNE.Add((object)((epicPrefsContainerNotEncrypted.IntegerDictionaryDictionary.Values.ToArray())[i]));
				editTypesNE.Add (Serializer.SerializationTypes.DictI);
			}
			editKeysE.AddRange (epicPrefsContainerEncrypted.IntegerDictionaryDictionary.Keys.ToList ());
			for (int i = 0; i < epicPrefsContainerEncrypted.IntegerDictionaryDictionary.Keys.Count; i++) {
				editValuesE.Add((object)((epicPrefsContainerEncrypted.IntegerDictionaryDictionary.Values.ToArray())[i]));
				editTypesE.Add (Serializer.SerializationTypes.DictI);
			}
			#endregion
			#region BooleanDictionaryDictionary
			editKeysNE.AddRange (epicPrefsContainerNotEncrypted.BooleanDictionaryDictionary.Keys.ToList ());
			for (int i = 0; i < epicPrefsContainerNotEncrypted.BooleanDictionaryDictionary.Keys.Count; i++) {
				editValuesNE.Add((object)((epicPrefsContainerNotEncrypted.BooleanDictionaryDictionary.Values.ToArray())[i]));
				editTypesNE.Add (Serializer.SerializationTypes.DictB);
			}
			editKeysE.AddRange (epicPrefsContainerEncrypted.BooleanDictionaryDictionary.Keys.ToList ());
			for (int i = 0; i < epicPrefsContainerEncrypted.BooleanDictionaryDictionary.Keys.Count; i++) {
				editValuesE.Add((object)((epicPrefsContainerEncrypted.BooleanDictionaryDictionary.Values.ToArray())[i]));
				editTypesE.Add (Serializer.SerializationTypes.DictB);
			}
			#endregion
			#region FloatDictionaryDictionary
			editKeysNE.AddRange (epicPrefsContainerNotEncrypted.FloatDictionaryDictionary.Keys.ToList ());
			for (int i = 0; i < epicPrefsContainerNotEncrypted.FloatDictionaryDictionary.Keys.Count; i++) {
				editValuesNE.Add((object)((epicPrefsContainerNotEncrypted.FloatDictionaryDictionary.Values.ToArray())[i]));
				editTypesNE.Add (Serializer.SerializationTypes.DictF);
			}
			editKeysE.AddRange (epicPrefsContainerEncrypted.FloatDictionaryDictionary.Keys.ToList ());
			for (int i = 0; i < epicPrefsContainerEncrypted.FloatDictionaryDictionary.Keys.Count; i++) {
				editValuesE.Add((object)((epicPrefsContainerEncrypted.FloatDictionaryDictionary.Values.ToArray())[i]));
				editTypesE.Add (Serializer.SerializationTypes.DictF);
			}
			#endregion
			#region DoubleDictionaryDictionary
			editKeysNE.AddRange (epicPrefsContainerNotEncrypted.DoubleDictionaryDictionary.Keys.ToList ());
			for (int i = 0; i < epicPrefsContainerNotEncrypted.DoubleDictionaryDictionary.Keys.Count; i++) {
				editValuesNE.Add((object)((epicPrefsContainerNotEncrypted.DoubleDictionaryDictionary.Values.ToArray())[i]));
				editTypesNE.Add (Serializer.SerializationTypes.DictD);
			}
			editKeysE.AddRange (epicPrefsContainerEncrypted.DoubleDictionaryDictionary.Keys.ToList ());
			for (int i = 0; i < epicPrefsContainerNotEncrypted.DoubleDictionaryDictionary.Keys.Count; i++) {
				editValuesE.Add((object)((epicPrefsContainerNotEncrypted.DoubleDictionaryDictionary.Values.ToArray())[i]));
				editTypesE.Add (Serializer.SerializationTypes.DictD);
			}
			#endregion
			#region LongDictionaryDictionary
			editKeysNE.AddRange (epicPrefsContainerNotEncrypted.LongDictionaryDictionary.Keys.ToList ());
			for (int i = 0; i < epicPrefsContainerNotEncrypted.LongDictionaryDictionary.Keys.Count; i++) {
				editValuesNE.Add((object)((epicPrefsContainerNotEncrypted.LongDictionaryDictionary.Values.ToArray())[i]));
				editTypesNE.Add (Serializer.SerializationTypes.DictL);
			}
			editKeysE.AddRange (epicPrefsContainerEncrypted.LongDictionaryDictionary.Keys.ToList ());
			for (int i = 0; i < epicPrefsContainerNotEncrypted.LongDictionaryDictionary.Keys.Count; i++) {
				editValuesE.Add((object)((epicPrefsContainerNotEncrypted.LongDictionaryDictionary.Values.ToArray())[i]));
				editTypesE.Add (Serializer.SerializationTypes.DictL);
			}
			#endregion
			#region QuaternionDictionary
			editKeysNE.AddRange (epicPrefsContainerNotEncrypted.QuaternionDictionary.Keys.ToList ());
			for (int i = 0; i < epicPrefsContainerNotEncrypted.QuaternionDictionary.Keys.Count; i++) {
				editValuesNE.Add((object)((epicPrefsContainerNotEncrypted.QuaternionDictionary.Values.ToArray())[i]));
				editTypesNE.Add (Serializer.SerializationTypes.Quaternion);
			}
			editKeysE.AddRange (epicPrefsContainerEncrypted.QuaternionDictionary.Keys.ToList ());
			for (int i = 0; i < epicPrefsContainerEncrypted.QuaternionDictionary.Keys.Count; i++) {
				editValuesE.Add((object)((epicPrefsContainerEncrypted.QuaternionDictionary.Values.ToArray())[i]));
				editTypesE.Add (Serializer.SerializationTypes.Quaternion);
			}
			#endregion
			#region Vector2Dictionary
			editKeysNE.AddRange (epicPrefsContainerNotEncrypted.Vector2Dictionary.Keys.ToList ());
			for (int i = 0; i < epicPrefsContainerNotEncrypted.Vector2Dictionary.Keys.Count; i++) {
				editValuesNE.Add((object)((epicPrefsContainerNotEncrypted.Vector2Dictionary.Values.ToArray())[i]));
				editTypesNE.Add (Serializer.SerializationTypes.Vector2);
			}
			editKeysE.AddRange (epicPrefsContainerEncrypted.Vector2Dictionary.Keys.ToList ());
			for (int i = 0; i < epicPrefsContainerEncrypted.Vector2Dictionary.Keys.Count; i++) {
				editValuesE.Add((object)((epicPrefsContainerEncrypted.Vector2Dictionary.Values.ToArray())[i]));
				editTypesE.Add (Serializer.SerializationTypes.Vector2);
			}
			#endregion
			#region Vector3Dictionary
			editKeysNE.AddRange (epicPrefsContainerNotEncrypted.Vector3Dictionary.Keys.ToList ());
			for (int i = 0; i < epicPrefsContainerNotEncrypted.Vector3Dictionary.Keys.Count; i++) {
				editValuesNE.Add((object)((epicPrefsContainerNotEncrypted.Vector3Dictionary.Values.ToArray())[i]));
				editTypesNE.Add (Serializer.SerializationTypes.Vector3);
			}
			editKeysE.AddRange (epicPrefsContainerEncrypted.Vector3Dictionary.Keys.ToList ());
			for (int i = 0; i < epicPrefsContainerEncrypted.Vector3Dictionary.Keys.Count; i++) {
				editValuesE.Add((object)((epicPrefsContainerEncrypted.Vector3Dictionary.Values.ToArray())[i]));
				editTypesE.Add (Serializer.SerializationTypes.Vector3);
			}
			#endregion
			#region Vector4Dictionary
			editKeysNE.AddRange (epicPrefsContainerNotEncrypted.Vector4Dictionary.Keys.ToList ());
			for (int i = 0; i < epicPrefsContainerNotEncrypted.Vector4Dictionary.Keys.Count; i++) {
				editValuesNE.Add((object)((epicPrefsContainerNotEncrypted.Vector4Dictionary.Values.ToArray())[i]));
				editTypesNE.Add (Serializer.SerializationTypes.Vector4);
			}
			editKeysE.AddRange (epicPrefsContainerEncrypted.Vector4Dictionary.Keys.ToList ());
			for (int i = 0; i < epicPrefsContainerEncrypted.Vector4Dictionary.Keys.Count; i++) {
				editValuesE.Add((object)((epicPrefsContainerEncrypted.Vector4Dictionary.Values.ToArray())[i]));
				editTypesE.Add (Serializer.SerializationTypes.Vector4
				);
			}
			#endregion
			#region ColorDictionary
			editKeysNE.AddRange (epicPrefsContainerNotEncrypted.ColorDictionary.Keys.ToList ());
			for (int i = 0; i < epicPrefsContainerNotEncrypted.ColorDictionary.Keys.Count; i++) {
				editValuesNE.Add((object)((epicPrefsContainerNotEncrypted.ColorDictionary.Values.ToArray())[i]));
				editTypesNE.Add (Serializer.SerializationTypes.Color);
			}
			editKeysE.AddRange (epicPrefsContainerEncrypted.ColorDictionary.Keys.ToList ());
			for (int i = 0; i < epicPrefsContainerEncrypted.ColorDictionary.Keys.Count; i++) {
				editTypesE.Add (Serializer.SerializationTypes.Color);
			}
			#endregion
		}
		private static void EndEdit(){
			EditorUtility.DisplayProgressBar("Saving", "Please wait while changes are being saved", 1 / (editKeysNE.Count+editKeysE.Count));
			EpicPrefs.DeleteAll (false);
			EpicPrefs.DeleteAll (true);
			AssetDatabase.Refresh ();
			EditorUtility.DisplayProgressBar("Saving", "Please wait while changes are being saved", 2 / (editKeysNE.Count+editKeysE.Count));
			epicPrefsContainerEncrypted = EpicPrefsEditorContainerHandler.LoadEpicPrefsEditorContainer (true);
			epicPrefsContainerNotEncrypted = EpicPrefsEditorContainerHandler.LoadEpicPrefsEditorContainer (false);
			for(int i = 0;i<editKeysNE.Count;i++){
				EditorUtility.DisplayProgressBar("Saving", "Please wait while changes are being saved", i / (editKeysNE.Count+editKeysE.Count));
				EpicPrefs.SetEditorPrefs (editKeysNE [i], editValuesNE [i], editTypesNE [i], false);
			}
			for(int i = 0;i<editKeysE.Count;i++){
				EditorUtility.DisplayProgressBar("Saving", "Please wait while changes are being saved", (i+editKeysNE.Count) / (editKeysNE.Count+editKeysE.Count));
				EpicPrefs.SetEditorPrefs (editKeysE [i], editValuesE [i], editTypesE [i], true);
			}
			Exit ();
			EditorUtility.ClearProgressBar ();
		}
		#endregion
		#region DisplayMethdods
		void DisplayBoolDict(Dictionary<string,bool> encDict, Dictionary<string,bool> nonEncDict,bool editable){
			var type = Serializer.SerializationTypes.Bool;
			var intermediateDictEnc = new Dictionary<string,bool> (encDict);
			var intermediateDictNotEnc = new Dictionary<string,bool> (nonEncDict);
			var encCount = intermediateDictEnc.Count;
			var notEncCount = intermediateDictNotEnc.Count;
			if (encCount + notEncCount > 0) {
				if (!foldouts.ContainsKey (type.ToString ())) {
					foldouts [type.ToString ()] = false;
				}
				foldouts[type.ToString()] = EditorGUILayout.Foldout(foldouts[type.ToString()], SerializationTypeToString(type));
				if (foldouts [type.ToString ()]) {
					if (!foldouts.ContainsKey (type.ToString () + "NotEncrypted")) {
						foldouts [type.ToString ()+ "NotEncrypted"] = false;
					}
					if (!foldouts.ContainsKey (type.ToString () + "Encrypted")) {
						foldouts [type.ToString ()+ "Encrypted"] = false;
					}
					if (notEncCount > 0) {
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Space (20);
						foldouts [type.ToString () + "NotEncrypted"] = EditorGUILayout.Foldout (foldouts [type.ToString () + "NotEncrypted"], "Not Encrypted");
						EditorGUILayout.EndHorizontal ();
						if (foldouts [type.ToString () + "NotEncrypted"]) {
							if (!editable) {
								foreach (var pair in intermediateDictNotEnc) {
									EditorGUILayout.BeginHorizontal ();
									GUILayout.Space (40);
									EditorGUILayout.Toggle (pair.Key, pair.Value);
									if (GUILayout.Button ("Encrypt", GUILayout.ExpandWidth (false))) {
										EpicPrefs.ToggleEncryptionEditorPrefs (pair.Key, pair.Value, type, false);
									}
									if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
										EpicPrefs.DeleteEditorPrefs (pair.Key, type, false);
									}
									EditorGUILayout.EndHorizontal ();
								}
							} else {
								for (int i = 0; i < editKeysNE.Count; i++) {
									if (editTypesNE [i] == type) {
										EditorGUILayout.BeginHorizontal ();
										GUILayout.Space (40);
										editKeysNE [i] = EditorGUILayout.TextField (editKeysNE [i]);
										editValuesNE [i] = (object)EditorGUILayout.Toggle ((bool)editValuesNE [i]);
										EditorGUILayout.EndHorizontal ();
									}
								}
							}
						}
					}
					if (encCount > 0) {
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Space (20);
						foldouts [type.ToString () + "Encrypted"] = EditorGUILayout.Foldout (foldouts [type.ToString () + "Encrypted"], "Encrypted");
						EditorGUILayout.EndHorizontal ();
						if (foldouts [type.ToString () + "Encrypted"]) {
							if (!editable) {
								foreach (var pair in intermediateDictEnc) {
									EditorGUILayout.BeginHorizontal ();
									GUILayout.Space (40);
									EditorGUILayout.Toggle (pair.Key, pair.Value);
									if (GUILayout.Button ("Decrypt", GUILayout.ExpandWidth (false))) {
										EpicPrefs.ToggleEncryptionEditorPrefs (pair.Key, pair.Value, type, true);
									}
									if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
										EpicPrefs.DeleteEditorPrefs (pair.Key, type, true);
									}
									EditorGUILayout.EndHorizontal ();
								}
							} else {
								for (int i = 0; i < editKeysE.Count; i++) {
									if (editTypesE [i] == type) {
										EditorGUILayout.BeginHorizontal ();
										GUILayout.Space (40);
										editKeysE [i] = EditorGUILayout.TextField (editKeysE [i]);
										editValuesE [i] = (object)EditorGUILayout.Toggle ((bool)editValuesE [i]);
										EditorGUILayout.EndHorizontal ();
									}
								}
							}
						}
					}
				}
			}
		}
		void DisplaySimpleDict(Serializer.SerializationTypes type,bool editable){
			var encCount = 0;
			var notEncCount = 0;
			var intermediateDictEnc = new Dictionary<string,string> ();
			var intermediateDictNotEnc = new Dictionary<string,string> ();
			switch (type) {
			case Serializer.SerializationTypes.Integer:
				foreach (var entry in epicPrefsContainerEncrypted.IntegerDictionary) {
					intermediateDictEnc [entry.Key] = entry.Value.ToString ();
				}
				foreach (var entry in epicPrefsContainerNotEncrypted.IntegerDictionary) {
					intermediateDictNotEnc [entry.Key] = entry.Value.ToString ();
				}
				break;
			case Serializer.SerializationTypes.String:
				foreach (var entry in epicPrefsContainerEncrypted.StringDictionary) {
					intermediateDictEnc [entry.Key] = entry.Value.ToString ();
				}
				foreach (var entry in epicPrefsContainerNotEncrypted.StringDictionary) {
					intermediateDictNotEnc [entry.Key] = entry.Value.ToString ();
				}
				break;
			case Serializer.SerializationTypes.Float:
				foreach (var entry in epicPrefsContainerEncrypted.FloatDictionary) {
					intermediateDictEnc [entry.Key] = entry.Value.ToString ();
				}
				foreach (var entry in epicPrefsContainerNotEncrypted.FloatDictionary) {
					intermediateDictNotEnc [entry.Key] = entry.Value.ToString ();
				}
				break;
			case Serializer.SerializationTypes.Long:
				foreach (var entry in epicPrefsContainerEncrypted.LongDictionary) {
					intermediateDictEnc [entry.Key] = entry.Value.ToString ();
				}
				foreach (var entry in epicPrefsContainerNotEncrypted.LongDictionary) {
					intermediateDictNotEnc [entry.Key] = entry.Value.ToString ();
				}
				break;
			case Serializer.SerializationTypes.Double:
				foreach (var entry in epicPrefsContainerEncrypted.DoubleDictionary) {
					intermediateDictEnc [entry.Key] = entry.Value.ToString ();
				}
				foreach (var entry in epicPrefsContainerNotEncrypted.DoubleDictionary) {
					intermediateDictNotEnc [entry.Key] = entry.Value.ToString ();
				}
				break;
			case Serializer.SerializationTypes.Bool:
				DisplayBoolDict (epicPrefsContainerEncrypted.BooleanDictionary,epicPrefsContainerNotEncrypted.BooleanDictionary,editable);
				break;
			default:
				break;
			}
			encCount = intermediateDictEnc.Count;
			notEncCount = intermediateDictNotEnc.Count;
			if (encCount + notEncCount > 0) {
				if (!foldouts.ContainsKey (type.ToString ())) {
					foldouts [type.ToString ()] = false;
				}
				foldouts[type.ToString()] = EditorGUILayout.Foldout(foldouts[type.ToString()], SerializationTypeToString(type));
				if (foldouts [type.ToString ()]) {
					if (!foldouts.ContainsKey (type.ToString () + "NotEncrypted")) {
						foldouts [type.ToString ()+ "NotEncrypted"] = false;
					}
					if (!foldouts.ContainsKey (type.ToString () + "Encrypted")) {
						foldouts [type.ToString ()+ "Encrypted"] = false;
					}
					if (notEncCount > 0) {
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Space (20);
						foldouts [type.ToString () + "NotEncrypted"] = EditorGUILayout.Foldout (foldouts [type.ToString () + "NotEncrypted"], "Not Encrypted");
						EditorGUILayout.EndHorizontal ();
						if (foldouts [type.ToString () + "NotEncrypted"]) {
							if (!editable) {
								foreach (var pair in intermediateDictNotEnc) {
									EditorGUILayout.BeginHorizontal ();
									GUILayout.Space (40);
									EditorGUILayout.LabelField (pair.Key, pair.Value);
									if (GUILayout.Button ("Encrypt", GUILayout.ExpandWidth (false))) {
										EpicPrefs.ToggleEncryptionEditorPrefs (pair.Key, pair.Value, type, false);
									}
									if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
										EpicPrefs.DeleteEditorPrefs (pair.Key, type, false);
									}
									EditorGUILayout.EndHorizontal ();
								}
							} else {
								for (int i = 0; i < editKeysNE.Count; i++) {
									if (editTypesNE [i] == type) {
										EditorGUILayout.BeginHorizontal ();
										GUILayout.Space (40);
										editKeysNE [i] = EditorGUILayout.TextField (editKeysNE [i]);
										editValuesNE [i] = (object)EditorGUILayout.TextField (editValuesNE [i].ToString());
										EditorGUILayout.EndHorizontal ();
									}
								}
							}
						}
					}
					if (encCount > 0) {
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Space (20);
						foldouts [type.ToString () + "Encrypted"] = EditorGUILayout.Foldout (foldouts [type.ToString () + "Encrypted"], "Encrypted");
						EditorGUILayout.EndHorizontal ();
						if (foldouts [type.ToString () + "Encrypted"]) {
							if (!editable) {
								foreach (var pair in intermediateDictEnc) {
									EditorGUILayout.BeginHorizontal ();
									GUILayout.Space (40);
									EditorGUILayout.LabelField (pair.Key, pair.Value);
									if (GUILayout.Button ("Decrypt", GUILayout.ExpandWidth (false))) {
										EpicPrefs.ToggleEncryptionEditorPrefs (pair.Key, pair.Value, type, true);
									}
									if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
										EpicPrefs.DeleteEditorPrefs (pair.Key, type, true);
									}
									EditorGUILayout.EndHorizontal ();
								}
							} else {
								for (int i = 0; i < editKeysE.Count; i++) {
									if (editTypesE [i] == type) {
										EditorGUILayout.BeginHorizontal ();
										GUILayout.Space (40);
										editKeysE [i] = EditorGUILayout.TextField (editKeysE [i]);
										editValuesE [i] = (object)EditorGUILayout.TextField (editValuesE [i].ToString());
										EditorGUILayout.EndHorizontal ();
									}
								}
							}
						}
					}
				}
			}
		}
		void DisplayComplexDict(Serializer.SerializationTypes type, bool editable){
			var encCount = 0;
			var notEncCount = 0;
			switch (type) {
			case Serializer.SerializationTypes.DictB:
				encCount = epicPrefsContainerEncrypted.BooleanDictionaryDictionary.Count;
				notEncCount = epicPrefsContainerNotEncrypted.BooleanDictionaryDictionary.Count;
				break;
			case Serializer.SerializationTypes.DictD:
				encCount = epicPrefsContainerEncrypted.DoubleDictionaryDictionary.Count;
				notEncCount = epicPrefsContainerNotEncrypted.DoubleDictionaryDictionary.Count;
				break;
			case Serializer.SerializationTypes.DictF:
				encCount = epicPrefsContainerEncrypted.FloatDictionaryDictionary.Count;
				notEncCount = epicPrefsContainerNotEncrypted.FloatDictionaryDictionary.Count;
				break;
			case Serializer.SerializationTypes.DictI:
				encCount = epicPrefsContainerEncrypted.IntegerDictionaryDictionary.Count;
				notEncCount = epicPrefsContainerNotEncrypted.IntegerDictionaryDictionary.Count;
				break;
			case Serializer.SerializationTypes.DictL:
				encCount = epicPrefsContainerEncrypted.LongDictionaryDictionary.Count;
				notEncCount = epicPrefsContainerNotEncrypted.LongDictionaryDictionary.Count;
				break;
			case Serializer.SerializationTypes.DictS:
				encCount = epicPrefsContainerEncrypted.StringDictionaryDictionary.Count;
				notEncCount = epicPrefsContainerNotEncrypted.StringDictionaryDictionary.Count;
				break;
			default:
				break;
			}
			if (encCount + notEncCount > 0) {
				if (!foldouts.ContainsKey (type.ToString ())) {
					foldouts [type.ToString ()] = false;
				}
				foldouts [type.ToString ()] = EditorGUILayout.Foldout (foldouts [type.ToString ()], SerializationTypeToString (type));
				if (foldouts [type.ToString ()]) {
					if (!foldouts.ContainsKey (type.ToString () + "NotEncrypted")) {
						foldouts [type.ToString () + "NotEncrypted"] = false;
					}
					if (!foldouts.ContainsKey (type.ToString () + "Encrypted")) {
						foldouts [type.ToString () + "Encrypted"] = false;
					}
					if (notEncCount > 0) {
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Space (20);
						foldouts [type.ToString () + "NotEncrypted"] = EditorGUILayout.Foldout (foldouts [type.ToString () + "NotEncrypted"], "Not Encrypted");
						EditorGUILayout.EndHorizontal ();
						if (foldouts [type.ToString () + "NotEncrypted"]) {
							switch (type) {
							case Serializer.SerializationTypes.DictB:
								foreach (var entry in epicPrefsContainerNotEncrypted.BooleanDictionaryDictionary) {
									if (!foldouts.ContainsKey (type.ToString () + "NotEncrypted" + entry.Key)) {
										foldouts [type.ToString () + "NotEncrypted" + entry.Key] = false;
									}
									EditorGUILayout.BeginHorizontal ();
									GUILayout.Space (40);
									foldouts [type.ToString () + "NotEncrypted" + entry.Key] = EditorGUILayout.Foldout (foldouts [type.ToString () + "NotEncrypted" + entry.Key], entry.Key);
									if (GUILayout.Button ("Encrypt", GUILayout.ExpandWidth (false))) {
										EpicPrefs.ToggleEncryptionEditorPrefs (entry.Key, entry.Value, type, false);
										return;
									}
									if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
										EpicPrefsEditor.DeleteEditorPref (entry.Key, type, false, true);
										return;
									}
									EditorGUILayout.EndHorizontal ();
									if (foldouts [type.ToString () + "NotEncrypted" + entry.Key]) {
										var deleteList = new List<int> ();
										var Count = (entry.Value).Count;
										if (Count > 0) {
											if (!editable) {
												foreach (var pair in entry.Value) {
													EditorGUILayout.BeginHorizontal ();
													GUILayout.Space (60);
													EditorGUILayout.Toggle (pair.Key, pair.Value);
													EditorGUILayout.EndHorizontal ();
												}
											} else {
												var ind = editKeysNE.IndexOf (entry.Key);
												var _keyList = ((Dictionary<string,bool>)editValuesNE [ind]).Keys.ToList ();
												var _valueList = ((Dictionary<string,bool>)editValuesNE [ind]).Values.ToList ();
												for (int m = 0; m < _keyList.Count; m++) {
													EditorGUILayout.BeginHorizontal ();
													GUILayout.Space (40);
													_keyList [m] = EditorGUILayout.TextField (_keyList [m]);
													_valueList [m] = (bool)EditorGUILayout.Toggle ((bool)_valueList [m]);
													if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
														deleteList.Add (m);
													}
													EditorGUILayout.EndHorizontal ();
												}
												EditorGUILayout.BeginHorizontal ();
												GUILayout.Space (40);
												if (GUILayout.Button ("Add", GUILayout.ExpandWidth (false))) {
													_keyList.Add ("");
													_valueList.Add (false);
												}
												EditorGUILayout.EndHorizontal ();
												foreach (int m in deleteList) {
													_keyList.RemoveAt (m);
													_valueList.RemoveAt (m);
												}
												var _replacementDict = new Dictionary<string,bool> ();
												for (int m = 0; m < _keyList.Count; m++) {
													_replacementDict [_keyList [m]] = _valueList [m];
												}
												editValuesNE [ind] = (object)_replacementDict;
											}
										}
									}
								}
								break;
							case Serializer.SerializationTypes.DictD:
								foreach (var entry in epicPrefsContainerNotEncrypted.DoubleDictionaryDictionary) {
									if (!foldouts.ContainsKey (type.ToString () + "NotEncrypted" + entry.Key)) {
										foldouts [type.ToString () + "NotEncrypted" + entry.Key] = false;
									}
									EditorGUILayout.BeginHorizontal ();
									GUILayout.Space (40);
									foldouts [type.ToString () + "NotEncrypted" + entry.Key] = EditorGUILayout.Foldout (foldouts [type.ToString () + "NotEncrypted" + entry.Key], entry.Key);
									if (GUILayout.Button ("Encrypt", GUILayout.ExpandWidth (false))) {
										EpicPrefs.ToggleEncryptionEditorPrefs (entry.Key, entry.Value, type, false);
										return;
									}
									if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
										EpicPrefsEditor.DeleteEditorPref (entry.Key, type, false, true);
										return;
									}
									EditorGUILayout.EndHorizontal ();
									if (foldouts [type.ToString () + "NotEncrypted" + entry.Key]) {
										var deleteList = new List<int> ();
										var Count = (entry.Value).Count;
										if (Count > 0) {
											if (!editable) {
												foreach (var pair in entry.Value) {
													EditorGUILayout.BeginHorizontal ();
													GUILayout.Space (60);
													EditorGUILayout.LabelField (pair.Key, pair.Value.ToString ());
													EditorGUILayout.EndHorizontal ();
												}
											} else {
												var ind = editKeysNE.IndexOf (entry.Key);
												var _keyList = ((Dictionary<string,double>)editValuesNE [ind]).Keys.ToList ();
												var _valueList = ((Dictionary<string,double>)editValuesNE [ind]).Values.ToList ();
												for (int m = 0; m < _keyList.Count; m++) {
													EditorGUILayout.BeginHorizontal ();
													GUILayout.Space (40);
													_keyList [m] = EditorGUILayout.TextField (_keyList [m]);
													_valueList [m] = Operators.ToDouble (EditorGUILayout.TextField (_valueList [m].ToString()));
													if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
														deleteList.Add (m);
													}
													EditorGUILayout.EndHorizontal ();
												}
												EditorGUILayout.BeginHorizontal ();
												GUILayout.Space (40);
												if (GUILayout.Button ("Add", GUILayout.ExpandWidth (false))) {
													_keyList.Add ("");
													_valueList.Add (0);
												}
												EditorGUILayout.EndHorizontal ();
												foreach (int m in deleteList) {
													_keyList.RemoveAt (m);
													_valueList.RemoveAt (m);
												}
												var _replacementDict = new Dictionary<string,double> ();
												for (int m = 0; m < _keyList.Count; m++) {
													_replacementDict [_keyList [m]] = _valueList [m];
												}
												editValuesNE [ind] = (object)_replacementDict;
											}
										}
									}
								}
								break;
							case Serializer.SerializationTypes.DictF:
								foreach (var entry in epicPrefsContainerNotEncrypted.FloatDictionaryDictionary) {
									if (!foldouts.ContainsKey (type.ToString () + "NotEncrypted" + entry.Key)) {
										foldouts [type.ToString () + "NotEncrypted" + entry.Key] = false;
									}
									EditorGUILayout.BeginHorizontal ();
									GUILayout.Space (40);
									foldouts [type.ToString () + "NotEncrypted" + entry.Key] = EditorGUILayout.Foldout (foldouts [type.ToString () + "NotEncrypted" + entry.Key], entry.Key);
									if (GUILayout.Button ("Encrypt", GUILayout.ExpandWidth (false))) {
										EpicPrefs.ToggleEncryptionEditorPrefs (entry.Key, entry.Value, type, false);
										return;
									}
									if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
										EpicPrefsEditor.DeleteEditorPref (entry.Key, type, false, true);
										return;
									}
									EditorGUILayout.EndHorizontal ();
									if (foldouts [type.ToString () + "NotEncrypted" + entry.Key]) {
										var deleteList = new List<int> ();
										var Count = (entry.Value).Count;
										if (Count > 0) {
											if (!editable) {
												foreach (var pair in entry.Value) {
													EditorGUILayout.BeginHorizontal ();
													GUILayout.Space (60);
													EditorGUILayout.LabelField (pair.Key, pair.Value.ToString ());
													EditorGUILayout.EndHorizontal ();
												}
											} else {
												var ind = editKeysNE.IndexOf (entry.Key);
												var _keyList = ((Dictionary<string,float>)editValuesNE [ind]).Keys.ToList ();
												var _valueList = ((Dictionary<string,float>)editValuesNE [ind]).Values.ToList ();
												for (int m = 0; m < _keyList.Count; m++) {
													EditorGUILayout.BeginHorizontal ();
													GUILayout.Space (40);
													_keyList [m] = EditorGUILayout.TextField (_keyList [m]);
													_valueList [m] = Operators.ToFloat (EditorGUILayout.TextField (_valueList [m].ToString()));
													if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
														deleteList.Add (m);
													}
													EditorGUILayout.EndHorizontal ();
												}
												EditorGUILayout.BeginHorizontal ();
												GUILayout.Space (40);
												if (GUILayout.Button ("Add", GUILayout.ExpandWidth (false))) {
													_keyList.Add ("");
													_valueList.Add (0);
												}
												EditorGUILayout.EndHorizontal ();
												foreach (int m in deleteList) {
													_keyList.RemoveAt (m);
													_valueList.RemoveAt (m);
												}
												var _replacementDict = new Dictionary<string,float> ();
												for (int m = 0; m < _keyList.Count; m++) {
													_replacementDict [_keyList [m]] = _valueList [m];
												}
												editValuesNE [ind] = (object)_replacementDict;
											}
										}
									}
								}
								break;
							case Serializer.SerializationTypes.DictI:
								foreach (var entry in epicPrefsContainerNotEncrypted.IntegerDictionaryDictionary) {
									if (!foldouts.ContainsKey (type.ToString () + "NotEncrypted" + entry.Key)) {
										foldouts [type.ToString () + "NotEncrypted" + entry.Key] = false;
									}
									EditorGUILayout.BeginHorizontal ();
									GUILayout.Space (40);
									foldouts [type.ToString () + "NotEncrypted" + entry.Key] = EditorGUILayout.Foldout (foldouts [type.ToString () + "NotEncrypted" + entry.Key], entry.Key);
									if (GUILayout.Button ("Encrypt", GUILayout.ExpandWidth (false))) {
										EpicPrefs.ToggleEncryptionEditorPrefs (entry.Key, entry.Value, type, false);
										return;
									}
									if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
										EpicPrefsEditor.DeleteEditorPref (entry.Key, type, false, true);
										return;
									}
									EditorGUILayout.EndHorizontal ();
									if (foldouts [type.ToString () + "NotEncrypted" + entry.Key]) {
										var deleteList = new List<int> ();
										var Count = (entry.Value).Count;
										if (Count > 0) {
											if (!editable) {
												foreach (var pair in entry.Value) {
													EditorGUILayout.BeginHorizontal ();
													GUILayout.Space (60);
													EditorGUILayout.LabelField (pair.Key, pair.Value.ToString ());
													EditorGUILayout.EndHorizontal ();
												}
											} else {
												var ind = editKeysNE.IndexOf (entry.Key);
												var _keyList = ((Dictionary<string,int>)editValuesNE [ind]).Keys.ToList ();
												var _valueList = ((Dictionary<string,int>)editValuesNE [ind]).Values.ToList ();
												for (int m = 0; m < _keyList.Count; m++) {
													EditorGUILayout.BeginHorizontal ();
													GUILayout.Space (40);
													_keyList [m] = EditorGUILayout.TextField (_keyList [m]);
													_valueList [m] = Operators.ToInt (EditorGUILayout.TextField (_valueList [m].ToString()));
													if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
														deleteList.Add (m);
													}
													EditorGUILayout.EndHorizontal ();
												}
												EditorGUILayout.BeginHorizontal ();
												GUILayout.Space (40);
												if (GUILayout.Button ("Add", GUILayout.ExpandWidth (false))) {
													_keyList.Add ("");
													_valueList.Add (0);
												}
												EditorGUILayout.EndHorizontal ();
												foreach (int m in deleteList) {
													_keyList.RemoveAt (m);
													_valueList.RemoveAt (m);
												}
												var _replacementDict = new Dictionary<string,int> ();
												for (int m = 0; m < _keyList.Count; m++) {
													_replacementDict [_keyList [m]] = _valueList [m];
												}
												editValuesNE [ind] = (object)_replacementDict;
											}
										}
									}
								}
								break;
							case Serializer.SerializationTypes.DictL:
								foreach (var entry in epicPrefsContainerNotEncrypted.LongDictionaryDictionary) {
									if (!foldouts.ContainsKey (type.ToString () + "NotEncrypted" + entry.Key)) {
										foldouts [type.ToString () + "NotEncrypted" + entry.Key] = false;
									}
									EditorGUILayout.BeginHorizontal ();
									GUILayout.Space (40);
									foldouts [type.ToString () + "NotEncrypted" + entry.Key] = EditorGUILayout.Foldout (foldouts [type.ToString () + "NotEncrypted" + entry.Key], entry.Key);
									if (GUILayout.Button ("Encrypt", GUILayout.ExpandWidth (false))) {
										EpicPrefs.ToggleEncryptionEditorPrefs (entry.Key, entry.Value, type, false);
										return;
									}
									if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
										EpicPrefsEditor.DeleteEditorPref (entry.Key, type, false, true);
										return;
									}
									EditorGUILayout.EndHorizontal ();
									if (foldouts [type.ToString () + "NotEncrypted" + entry.Key]) {
										var deleteList = new List<int> ();
										var Count = (entry.Value).Count;
										if (Count > 0) {
											if (!editable) {
												foreach (var pair in entry.Value) {
													EditorGUILayout.BeginHorizontal ();
													GUILayout.Space (60);
													EditorGUILayout.LabelField (pair.Key, pair.Value.ToString ());
													EditorGUILayout.EndHorizontal ();
												}
											} else {
												var ind = editKeysNE.IndexOf (entry.Key);
												var _keyList = ((Dictionary<string,long>)editValuesNE [ind]).Keys.ToList ();
												var _valueList = ((Dictionary<string,long>)editValuesNE [ind]).Values.ToList ();
												for (int m = 0; m < _keyList.Count; m++) {
													EditorGUILayout.BeginHorizontal ();
													GUILayout.Space (40);
													_keyList [m] = EditorGUILayout.TextField (_keyList [m]);
													_valueList [m] = Operators.ToLong (EditorGUILayout.TextField (_valueList [m].ToString()));
													if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
														deleteList.Add (m);
													}
													EditorGUILayout.EndHorizontal ();
												}
												EditorGUILayout.BeginHorizontal ();
												GUILayout.Space (40);
												if (GUILayout.Button ("Add", GUILayout.ExpandWidth (false))) {
													_keyList.Add ("");
													_valueList.Add (0);
												}
												EditorGUILayout.EndHorizontal ();
												foreach (int m in deleteList) {
													_keyList.RemoveAt (m);
													_valueList.RemoveAt (m);
												}
												var _replacementDict = new Dictionary<string,long> ();
												for (int m = 0; m < _keyList.Count; m++) {
													_replacementDict [_keyList [m]] = _valueList [m];
												}
												editValuesNE [ind] = (object)_replacementDict;
											}
										}
									}
								}
								break;
							case Serializer.SerializationTypes.DictS:
								foreach (var entry in epicPrefsContainerNotEncrypted.StringDictionaryDictionary) {
									if (!foldouts.ContainsKey (type.ToString () + "NotEncrypted" + entry.Key)) {
										foldouts [type.ToString () + "NotEncrypted" + entry.Key] = false;
									}
									EditorGUILayout.BeginHorizontal ();
									GUILayout.Space (40);
									foldouts [type.ToString () + "NotEncrypted" + entry.Key] = EditorGUILayout.Foldout (foldouts [type.ToString () + "NotEncrypted" + entry.Key], entry.Key);
									if (GUILayout.Button ("Encrypt", GUILayout.ExpandWidth (false))) {
										EpicPrefs.ToggleEncryptionEditorPrefs (entry.Key, entry.Value, type, false);
										return;
									}
									if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
										EpicPrefsEditor.DeleteEditorPref (entry.Key, type, false, true);
										return;
									}
									EditorGUILayout.EndHorizontal ();
									if (foldouts [type.ToString () + "NotEncrypted" + entry.Key]) {
										var deleteList = new List<int> ();
										var Count = (entry.Value).Count;
										if (Count > 0) {
											if (!editable) {
												foreach (var pair in entry.Value) {
													EditorGUILayout.BeginHorizontal ();
													GUILayout.Space (60);
													EditorGUILayout.LabelField (pair.Key, pair.Value.ToString ());
													EditorGUILayout.EndHorizontal ();
												}
											} else {
												var ind = editKeysNE.IndexOf (entry.Key);
												var _keyList = ((Dictionary<string,string>)editValuesNE [ind]).Keys.ToList ();
												var _valueList = ((Dictionary<string,string>)editValuesNE [ind]).Values.ToList ();
												for (int m = 0; m < _keyList.Count; m++) {
													EditorGUILayout.BeginHorizontal ();
													GUILayout.Space (40);
													_keyList [m] = EditorGUILayout.TextField (_keyList [m]);
													_valueList [m] = EditorGUILayout.TextField (_valueList [m]);
													if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
														deleteList.Add (m);
													}
													EditorGUILayout.EndHorizontal ();
												}
												EditorGUILayout.BeginHorizontal ();
												GUILayout.Space (40);
												if (GUILayout.Button ("Add", GUILayout.ExpandWidth (false))) {
													_keyList.Add ("");
													_valueList.Add ("");
												}
												EditorGUILayout.EndHorizontal ();
												foreach (int m in deleteList) {
													_keyList.RemoveAt (m);
													_valueList.RemoveAt (m);
												}
												var _replacementDict = new Dictionary<string,string> ();
												for (int m = 0; m < _keyList.Count; m++) {
													_replacementDict [_keyList [m]] = _valueList [m];
												}
												editValuesNE [ind] = (object)_replacementDict;
											}
										}
									}
								}
								break;
							default:
								break;
							}

						}
					}
					if (encCount > 0) {
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Space (20);
						foldouts [type.ToString () + "Encrypted"] = EditorGUILayout.Foldout (foldouts [type.ToString () + "Encrypted"], "Encrypted");
						EditorGUILayout.EndHorizontal ();
						if (foldouts [type.ToString () + "Encrypted"]) {
							switch (type) {
							case Serializer.SerializationTypes.DictB:
								foreach (var entry in epicPrefsContainerEncrypted.BooleanDictionaryDictionary) {
									if (!foldouts.ContainsKey (type.ToString () + "Encrypted" + entry.Key)) {
										foldouts [type.ToString () + "Encrypted" + entry.Key] = false;
									}
									EditorGUILayout.BeginHorizontal ();
									GUILayout.Space (40);
									foldouts [type.ToString () + "Encrypted" + entry.Key] = EditorGUILayout.Foldout (foldouts [type.ToString () + "Encrypted" + entry.Key], entry.Key);
									if (GUILayout.Button ("Decrypt", GUILayout.ExpandWidth (false))) {
										EpicPrefs.ToggleEncryptionEditorPrefs (entry.Key, entry.Value, type, true);
										return;
									}
									if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
										EpicPrefsEditor.DeleteEditorPref (entry.Key, type, true, true);
										return;
									}
									EditorGUILayout.EndHorizontal ();
									if (foldouts [type.ToString () + "Encrypted" + entry.Key]) {
										var deleteList = new List<int> ();
										var Count = (entry.Value).Count;
										if (Count > 0) {
											if (!editable) {
												foreach (var pair in entry.Value) {
													EditorGUILayout.BeginHorizontal ();
													GUILayout.Space (60);
													EditorGUILayout.Toggle (pair.Key, pair.Value);
													EditorGUILayout.EndHorizontal ();
												}
											} else {
												var ind = editKeysE.IndexOf (entry.Key);
												var _keyList = ((Dictionary<string,bool>)editValuesE [ind]).Keys.ToList ();
												var _valueList = ((Dictionary<string,bool>)editValuesE [ind]).Values.ToList ();
												for (int m = 0; m < _keyList.Count; m++) {
													EditorGUILayout.BeginHorizontal ();
													GUILayout.Space (40);
													_keyList [m] = EditorGUILayout.TextField (_keyList [m]);
													_valueList [m] = (bool)EditorGUILayout.Toggle ((bool)_valueList [m]);
													if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
														deleteList.Add (m);
													}
													EditorGUILayout.EndHorizontal ();
												}
												EditorGUILayout.BeginHorizontal ();
												GUILayout.Space (40);
												if (GUILayout.Button ("Add", GUILayout.ExpandWidth (false))) {
													_keyList.Add ("");
													_valueList.Add (false);
												}
												EditorGUILayout.EndHorizontal ();
												foreach (int m in deleteList) {
													_keyList.RemoveAt (m);
													_valueList.RemoveAt (m);
												}
												var _replacementDict = new Dictionary<string,bool> ();
												for (int m = 0; m < _keyList.Count; m++) {
													_replacementDict [_keyList [m]] = _valueList [m];
												}
												editValuesE [ind] = (object)_replacementDict;
											}
										}
									}
								}
								break;
							case Serializer.SerializationTypes.DictD:
								foreach (var entry in epicPrefsContainerEncrypted.DoubleDictionaryDictionary) {
									if (!foldouts.ContainsKey (type.ToString () + "Encrypted" + entry.Key)) {
										foldouts [type.ToString () + "Encrypted" + entry.Key] = false;
									}
									EditorGUILayout.BeginHorizontal ();
									GUILayout.Space (40);
									foldouts [type.ToString () + "Encrypted" + entry.Key] = EditorGUILayout.Foldout (foldouts [type.ToString () + "Encrypted" + entry.Key], entry.Key);
									if (GUILayout.Button ("Decrypt", GUILayout.ExpandWidth (false))) {
										EpicPrefs.ToggleEncryptionEditorPrefs (entry.Key, entry.Value, type, true);
										return;
									}
									if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
										EpicPrefsEditor.DeleteEditorPref (entry.Key, type, true, true);
										return;
									}
									EditorGUILayout.EndHorizontal ();
									if (foldouts [type.ToString () + "Encrypted" + entry.Key]) {
										var deleteList = new List<int> ();
										var Count = (entry.Value).Count;
										if (Count > 0) {
											if (!editable) {
												foreach (var pair in entry.Value) {
													EditorGUILayout.BeginHorizontal ();
													GUILayout.Space (60);
													EditorGUILayout.LabelField (pair.Key, pair.Value.ToString ());
													EditorGUILayout.EndHorizontal ();
												}
											} else {
												var ind = editKeysE.IndexOf (entry.Key);
												var _keyList = ((Dictionary<string,double>)editValuesE [ind]).Keys.ToList ();
												var _valueList = ((Dictionary<string,double>)editValuesE [ind]).Values.ToList ();
												for (int m = 0; m < _keyList.Count; m++) {
													EditorGUILayout.BeginHorizontal ();
													GUILayout.Space (40);
													_keyList [m] = EditorGUILayout.TextField (_keyList [m]);
													_valueList [m] = Operators.ToDouble (EditorGUILayout.TextField (_valueList [m].ToString()));
													if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
														deleteList.Add (m);
													}
													EditorGUILayout.EndHorizontal ();
												}
												EditorGUILayout.BeginHorizontal ();
												GUILayout.Space (40);
												if (GUILayout.Button ("Add", GUILayout.ExpandWidth (false))) {
													_keyList.Add ("");
													_valueList.Add (0);
												}
												EditorGUILayout.EndHorizontal ();
												foreach (int m in deleteList) {
													_keyList.RemoveAt (m);
													_valueList.RemoveAt (m);
												}
												var _replacementDict = new Dictionary<string,double> ();
												for (int m = 0; m < _keyList.Count; m++) {
													_replacementDict [_keyList [m]] = _valueList [m];
												}
												editValuesE [ind] = (object)_replacementDict;
											}
										}
									}
								}
								break;
							case Serializer.SerializationTypes.DictF:
								foreach (var entry in epicPrefsContainerEncrypted.FloatDictionaryDictionary) {
									if (!foldouts.ContainsKey (type.ToString () + "Encrypted" + entry.Key)) {
										foldouts [type.ToString () + "Encrypted" + entry.Key] = false;
									}
									EditorGUILayout.BeginHorizontal ();
									GUILayout.Space (40);
									foldouts [type.ToString () + "Encrypted" + entry.Key] = EditorGUILayout.Foldout (foldouts [type.ToString () + "Encrypted" + entry.Key], entry.Key);
									if (GUILayout.Button ("Decrypt", GUILayout.ExpandWidth (false))) {
										EpicPrefs.ToggleEncryptionEditorPrefs (entry.Key, entry.Value, type, true);
										return;
									}
									if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
										EpicPrefsEditor.DeleteEditorPref (entry.Key, type, true, true);
										return;
									}
									EditorGUILayout.EndHorizontal ();
									if (foldouts [type.ToString () + "Encrypted" + entry.Key]) {
										var deleteList = new List<int> ();
										var Count = (entry.Value).Count;
										if (Count > 0) {
											if (!editable) {
												foreach (var pair in entry.Value) {
													EditorGUILayout.BeginHorizontal ();
													GUILayout.Space (60);
													EditorGUILayout.LabelField (pair.Key, pair.Value.ToString ());
													EditorGUILayout.EndHorizontal ();
												}
											} else {
												var ind = editKeysE.IndexOf (entry.Key);
												var _keyList = ((Dictionary<string,float>)editValuesE [ind]).Keys.ToList ();
												var _valueList = ((Dictionary<string,float>)editValuesE [ind]).Values.ToList ();
												for (int m = 0; m < _keyList.Count; m++) {
													EditorGUILayout.BeginHorizontal ();
													GUILayout.Space (40);
													_keyList [m] = EditorGUILayout.TextField (_keyList [m]);
													_valueList [m] = Operators.ToFloat (EditorGUILayout.TextField (_valueList [m].ToString()));
													if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
														deleteList.Add (m);
													}
													EditorGUILayout.EndHorizontal ();
												}
												EditorGUILayout.BeginHorizontal ();
												GUILayout.Space (40);
												if (GUILayout.Button ("Add", GUILayout.ExpandWidth (false))) {
													_keyList.Add ("");
													_valueList.Add (0);
												}
												EditorGUILayout.EndHorizontal ();
												foreach (int m in deleteList) {
													_keyList.RemoveAt (m);
													_valueList.RemoveAt (m);
												}
												var _replacementDict = new Dictionary<string,float> ();
												for (int m = 0; m < _keyList.Count; m++) {
													_replacementDict [_keyList [m]] = _valueList [m];
												}
												editValuesE [ind] = (object)_replacementDict;
											}
										}
									}
								}
								break;
							case Serializer.SerializationTypes.DictI:
								foreach (var entry in epicPrefsContainerEncrypted.IntegerDictionaryDictionary) {
									if (!foldouts.ContainsKey (type.ToString () + "Encrypted" + entry.Key)) {
										foldouts [type.ToString () + "Encrypted" + entry.Key] = false;
									}
									EditorGUILayout.BeginHorizontal ();
									GUILayout.Space (40);
									foldouts [type.ToString () + "Encrypted" + entry.Key] = EditorGUILayout.Foldout (foldouts [type.ToString () + "Encrypted" + entry.Key], entry.Key);
									if (GUILayout.Button ("Decrypt", GUILayout.ExpandWidth (false))) {
										EpicPrefs.ToggleEncryptionEditorPrefs (entry.Key, entry.Value, type, true);
										return;
									}
									if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
										EpicPrefsEditor.DeleteEditorPref (entry.Key, type, true, true);
										return;
									}
									EditorGUILayout.EndHorizontal ();
									if (foldouts [type.ToString () + "Encrypted" + entry.Key]) {
										var deleteList = new List<int> ();
										var Count = (entry.Value).Count;
										if (Count > 0) {
											if (!editable) {
												foreach (var pair in entry.Value) {
													EditorGUILayout.BeginHorizontal ();
													GUILayout.Space (60);
													EditorGUILayout.LabelField (pair.Key, pair.Value.ToString ());
													EditorGUILayout.EndHorizontal ();
												}
											} else {
												var ind = editKeysE.IndexOf (entry.Key);
												var _keyList = ((Dictionary<string,int>)editValuesE [ind]).Keys.ToList ();
												var _valueList = ((Dictionary<string,int>)editValuesE [ind]).Values.ToList ();
												for (int m = 0; m < _keyList.Count; m++) {
													EditorGUILayout.BeginHorizontal ();
													GUILayout.Space (40);
													_keyList [m] = EditorGUILayout.TextField (_keyList [m]);
													_valueList [m] = Operators.ToInt (EditorGUILayout.TextField (_valueList [m].ToString()));
													if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
														deleteList.Add (m);
													}
													EditorGUILayout.EndHorizontal ();
												}
												EditorGUILayout.BeginHorizontal ();
												GUILayout.Space (40);
												if (GUILayout.Button ("Add", GUILayout.ExpandWidth (false))) {
													_keyList.Add ("");
													_valueList.Add (0);
												}
												EditorGUILayout.EndHorizontal ();
												foreach (int m in deleteList) {
													_keyList.RemoveAt (m);
													_valueList.RemoveAt (m);
												}
												var _replacementDict = new Dictionary<string,int> ();
												for (int m = 0; m < _keyList.Count; m++) {
													_replacementDict [_keyList [m]] = _valueList [m];
												}
												editValuesE [ind] = (object)_replacementDict;
											}
										}
									}
								}
								break;
							case Serializer.SerializationTypes.DictL:
								foreach (var entry in epicPrefsContainerEncrypted.LongDictionaryDictionary) {
									if (!foldouts.ContainsKey (type.ToString () + "Encrypted" + entry.Key)) {
										foldouts [type.ToString () + "Encrypted" + entry.Key] = false;
									}
									EditorGUILayout.BeginHorizontal ();
									GUILayout.Space (40);
									foldouts [type.ToString () + "Encrypted" + entry.Key] = EditorGUILayout.Foldout (foldouts [type.ToString () + "Encrypted" + entry.Key], entry.Key);
									if (GUILayout.Button ("Decrypt", GUILayout.ExpandWidth (false))) {
										EpicPrefs.ToggleEncryptionEditorPrefs (entry.Key, entry.Value, type, true);
										return;
									}
									if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
										EpicPrefsEditor.DeleteEditorPref (entry.Key, type, true, true);
										return;
									}
									EditorGUILayout.EndHorizontal ();
									if (foldouts [type.ToString () + "Encrypted" + entry.Key]) {
										var deleteList = new List<int> ();
										var Count = (entry.Value).Count;
										if (Count > 0) {
											if (!editable) {
												foreach (var pair in entry.Value) {
													EditorGUILayout.BeginHorizontal ();
													GUILayout.Space (60);
													EditorGUILayout.LabelField (pair.Key, pair.Value.ToString ());
													EditorGUILayout.EndHorizontal ();
												}
											} else {
												var ind = editKeysE.IndexOf (entry.Key);
												var _keyList = ((Dictionary<string,long>)editValuesE [ind]).Keys.ToList ();
												var _valueList = ((Dictionary<string,long>)editValuesE [ind]).Values.ToList ();
												for (int m = 0; m < _keyList.Count; m++) {
													EditorGUILayout.BeginHorizontal ();
													GUILayout.Space (40);
													_keyList [m] = EditorGUILayout.TextField (_keyList [m]);
													_valueList [m] = Operators.ToLong (EditorGUILayout.TextField (_valueList [m].ToString()));
													if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
														deleteList.Add (m);
													}
													EditorGUILayout.EndHorizontal ();
												}
												EditorGUILayout.BeginHorizontal ();
												GUILayout.Space (40);
												if (GUILayout.Button ("Add", GUILayout.ExpandWidth (false))) {
													_keyList.Add ("");
													_valueList.Add (0);
												}
												EditorGUILayout.EndHorizontal ();
												foreach (int m in deleteList) {
													_keyList.RemoveAt (m);
													_valueList.RemoveAt (m);
												}
												var _replacementDict = new Dictionary<string,long> ();
												for (int m = 0; m < _keyList.Count; m++) {
													_replacementDict [_keyList [m]] = _valueList [m];
												}
												editValuesE [ind] = (object)_replacementDict;
											}
										}
									}
								}
								break;
							case Serializer.SerializationTypes.DictS:
								foreach (var entry in epicPrefsContainerEncrypted.StringDictionaryDictionary) {
									if (!foldouts.ContainsKey (type.ToString () + "Encrypted" + entry.Key)) {
										foldouts [type.ToString () + "Encrypted" + entry.Key] = false;
									}
									EditorGUILayout.BeginHorizontal ();
									GUILayout.Space (40);
									foldouts [type.ToString () + "Encrypted" + entry.Key] = EditorGUILayout.Foldout (foldouts [type.ToString () + "Encrypted" + entry.Key], entry.Key);
									if (GUILayout.Button ("Decrypt", GUILayout.ExpandWidth (false))) {
										EpicPrefs.ToggleEncryptionEditorPrefs (entry.Key, entry.Value, type, true);
										return;
									}
									if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
										EpicPrefsEditor.DeleteEditorPref (entry.Key, type, true, true);
										return;
									}
									EditorGUILayout.EndHorizontal ();
									if (foldouts [type.ToString () + "Encrypted" + entry.Key]) {
										var deleteList = new List<int> ();
										var Count = (entry.Value).Count;
										if (Count > 0) {
											if (!editable) {
												foreach (var pair in entry.Value) {
													EditorGUILayout.BeginHorizontal ();
													GUILayout.Space (60);
													EditorGUILayout.LabelField (pair.Key, pair.Value.ToString ());
													EditorGUILayout.EndHorizontal ();
												}
											} else {
												var ind = editKeysE.IndexOf (entry.Key);
												var _keyList = ((Dictionary<string,string>)editValuesE [ind]).Keys.ToList ();
												var _valueList = ((Dictionary<string,string>)editValuesE [ind]).Values.ToList ();
												for (int m = 0; m < _keyList.Count; m++) {
													EditorGUILayout.BeginHorizontal ();
													GUILayout.Space (40);
													_keyList [m] = EditorGUILayout.TextField (_keyList [m]);
													_valueList [m] = EditorGUILayout.TextField (_valueList [m]);
													if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
														deleteList.Add (m);
													}
													EditorGUILayout.EndHorizontal ();
												}
												EditorGUILayout.BeginHorizontal ();
												GUILayout.Space (40);
												if (GUILayout.Button ("Add", GUILayout.ExpandWidth (false))) {
													_keyList.Add ("");
													_valueList.Add ("");
												}
												EditorGUILayout.EndHorizontal ();
												foreach (int m in deleteList) {
													_keyList.RemoveAt (m);
													_valueList.RemoveAt (m);
												}
												var _replacementDict = new Dictionary<string,string> ();
												for (int m = 0; m < _keyList.Count; m++) {
													_replacementDict [_keyList [m]] = _valueList [m];
												}
												editValuesE [ind] = (object)_replacementDict;
											}
										}
									}
								}
								break;
							default:
								break;
							}

						}
					}
				}
			}
		}
		void DisplayListsAndArrays(Serializer.SerializationTypes type, bool editable){
			var encCount = 0;
			var notEncCount = 0;
			switch (type) {
			case Serializer.SerializationTypes.ArrayInt:
				encCount = epicPrefsContainerEncrypted.IntegerArrayDictionary.Count;
				notEncCount = epicPrefsContainerNotEncrypted.IntegerArrayDictionary.Count;
				break;
			case Serializer.SerializationTypes.ArrayString:
				encCount = epicPrefsContainerEncrypted.StringArrayDictionary.Count;
				notEncCount = epicPrefsContainerNotEncrypted.StringArrayDictionary.Count;
				break;
			case Serializer.SerializationTypes.ArrayFloat:
				encCount = epicPrefsContainerEncrypted.FloatArrayDictionary.Count;
				notEncCount = epicPrefsContainerNotEncrypted.FloatArrayDictionary.Count;
				break;
			case Serializer.SerializationTypes.ArrayDouble:
				encCount = epicPrefsContainerEncrypted.DoubleArrayDictionary.Count;
				notEncCount = epicPrefsContainerNotEncrypted.DoubleArrayDictionary.Count;
				break;
			case Serializer.SerializationTypes.ListB:
				encCount = epicPrefsContainerEncrypted.BooleanListDictionary.Count;
				notEncCount = epicPrefsContainerNotEncrypted.BooleanListDictionary.Count;
				break;
			case Serializer.SerializationTypes.ListD:
				encCount = epicPrefsContainerEncrypted.DoubleListDictionary.Count;
				notEncCount = epicPrefsContainerNotEncrypted.DoubleListDictionary.Count;
				break;
			case Serializer.SerializationTypes.ListF:
				encCount = epicPrefsContainerEncrypted.FloatDictionary.Count;
				notEncCount = epicPrefsContainerNotEncrypted.FloatDictionary.Count;
				break;
			case Serializer.SerializationTypes.ListI:
				encCount = epicPrefsContainerEncrypted.IntegerListDictionary.Count;
				notEncCount = epicPrefsContainerNotEncrypted.IntegerListDictionary.Count;
				break;
			case Serializer.SerializationTypes.ListL:
				encCount = epicPrefsContainerEncrypted.LongListDictionary.Count;
				notEncCount = epicPrefsContainerNotEncrypted.LongListDictionary.Count;
				break;
			case Serializer.SerializationTypes.ListS:
				encCount = epicPrefsContainerEncrypted.StringListDictionary.Count;
				notEncCount = epicPrefsContainerNotEncrypted.StringListDictionary.Count;
				break;
			default:
				break;
			}
			if (encCount + notEncCount > 0) {
				if (!foldouts.ContainsKey (type.ToString ())) {
					foldouts [type.ToString ()] = false;
				}
				foldouts [type.ToString ()] = EditorGUILayout.Foldout (foldouts [type.ToString ()], SerializationTypeToString (type));
				if (foldouts [type.ToString ()]) {
					if (!foldouts.ContainsKey (type.ToString () + "NotEncrypted")) {
						foldouts [type.ToString () + "NotEncrypted"] = false;
					}
					if (!foldouts.ContainsKey (type.ToString () + "Encrypted")) {
						foldouts [type.ToString () + "Encrypted"] = false;
					}
					if (notEncCount > 0) {
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Space (20);
						foldouts [type.ToString () + "NotEncrypted"] = EditorGUILayout.Foldout (foldouts [type.ToString () + "NotEncrypted"], "Not Encrypted");
						EditorGUILayout.EndHorizontal ();
						if (foldouts [type.ToString () + "NotEncrypted"]) {
							switch (type) {
							case Serializer.SerializationTypes.ArrayDouble:
								foreach (var entry in epicPrefsContainerNotEncrypted.DoubleArrayDictionary) {
									if (!foldouts.ContainsKey (type.ToString () + "NotEncrypted" + entry.Key)) {
										foldouts [type.ToString () + "NotEncrypted" + entry.Key] = false;
									}
									EditorGUILayout.BeginHorizontal ();
									GUILayout.Space (40);
									foldouts [type.ToString () + "NotEncrypted" + entry.Key] = EditorGUILayout.Foldout (foldouts [type.ToString () + "NotEncrypted" + entry.Key], entry.Key);
									if (GUILayout.Button ("Encrypt", GUILayout.ExpandWidth (false))) {
										EpicPrefs.ToggleEncryptionEditorPrefs (entry.Key, entry.Value, type, false);
										return;
									}
									if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
										EpicPrefsEditor.DeleteEditorPref (entry.Key, type, false, true);
										return;
									}
									EditorGUILayout.EndHorizontal ();
									if (foldouts [type.ToString () + "NotEncrypted" + entry.Key]) {
										var deleteList = new List<int> ();
										var Count = (entry.Value).Length;
										if (Count > 0) {
											if (!editable) {
												foreach (var val in entry.Value) {
													EditorGUILayout.BeginHorizontal ();
													GUILayout.Space (60);
													EditorGUILayout.LabelField (val.ToString());
													EditorGUILayout.EndHorizontal ();
												}
											} else {
												var ind = editKeysNE.IndexOf (entry.Key);
												var _valueList = ((double[])editValuesNE [ind]).ToList ();
												for (int m = 0; m < _valueList.Count; m++) {
													EditorGUILayout.BeginHorizontal ();
													GUILayout.Space (40);
													_valueList [m] = Operators.ToDouble(EditorGUILayout.TextField (_valueList [m].ToString()));
													if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
														deleteList.Add (m);
													}
													EditorGUILayout.EndHorizontal ();
												}
												EditorGUILayout.BeginHorizontal ();
												GUILayout.Space (40);
												if (GUILayout.Button ("Add", GUILayout.ExpandWidth (false))) {
													_valueList.Add (0);
												}
												EditorGUILayout.EndHorizontal ();
												foreach (int m in deleteList) {
													_valueList.RemoveAt (m);
												}
												var _replacementArray = new double[_valueList.Count];
												for (int m = 0; m < _valueList.Count; m++) {
													_replacementArray [m] = _valueList [m];
												}
												editValuesNE [ind] = (object)_replacementArray;
											}
										}
									}
								}
								break;
							case Serializer.SerializationTypes.ArrayInt:
								foreach (var entry in epicPrefsContainerNotEncrypted.IntegerArrayDictionary) {
									if (!foldouts.ContainsKey (type.ToString () + "NotEncrypted" + entry.Key)) {
										foldouts [type.ToString () + "NotEncrypted" + entry.Key] = false;
									}
									EditorGUILayout.BeginHorizontal ();
									GUILayout.Space (40);
									foldouts [type.ToString () + "NotEncrypted" + entry.Key] = EditorGUILayout.Foldout (foldouts [type.ToString () + "NotEncrypted" + entry.Key], entry.Key);
									if (GUILayout.Button ("Encrypt", GUILayout.ExpandWidth (false))) {
										EpicPrefs.ToggleEncryptionEditorPrefs (entry.Key, entry.Value, type, false);
										return;
									}
									if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
										EpicPrefsEditor.DeleteEditorPref (entry.Key, type, false, true);
										return;
									}
									EditorGUILayout.EndHorizontal ();
									if (foldouts [type.ToString () + "NotEncrypted" + entry.Key]) {
										var deleteList = new List<int> ();
										var Count = (entry.Value).Length;
										if (Count > 0) {
											if (!editable) {
												foreach (var val in entry.Value) {
													EditorGUILayout.BeginHorizontal ();
													GUILayout.Space (60);
													EditorGUILayout.LabelField (val.ToString());
													EditorGUILayout.EndHorizontal ();
												}
											} else {
												var ind = editKeysNE.IndexOf (entry.Key);
												var _valueList = ((int[])editValuesNE [ind]).ToList ();
												for (int m = 0; m < _valueList.Count; m++) {
													EditorGUILayout.BeginHorizontal ();
													GUILayout.Space (40);
													_valueList [m] = Operators.ToInt(EditorGUILayout.TextField (_valueList [m].ToString()));
													if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
														deleteList.Add (m);
													}
													EditorGUILayout.EndHorizontal ();
												}
												EditorGUILayout.BeginHorizontal ();
												GUILayout.Space (40);
												if (GUILayout.Button ("Add", GUILayout.ExpandWidth (false))) {
													_valueList.Add (0);
												}
												EditorGUILayout.EndHorizontal ();
												foreach (int m in deleteList) {
													_valueList.RemoveAt (m);
												}
												var _replacementArray = new int[_valueList.Count];
												for (int m = 0; m < _valueList.Count; m++) {
													_replacementArray [m] = _valueList [m];
												}
												editValuesNE [ind] = (object)_replacementArray;
											}
										}
									}
								}
								break;
							case Serializer.SerializationTypes.ArrayFloat:
								foreach (var entry in epicPrefsContainerNotEncrypted.FloatArrayDictionary) {
									if (!foldouts.ContainsKey (type.ToString () + "NotEncrypted" + entry.Key)) {
										foldouts [type.ToString () + "NotEncrypted" + entry.Key] = false;
									}
									EditorGUILayout.BeginHorizontal ();
									GUILayout.Space (40);
									foldouts [type.ToString () + "NotEncrypted" + entry.Key] = EditorGUILayout.Foldout (foldouts [type.ToString () + "NotEncrypted" + entry.Key], entry.Key);
									if (GUILayout.Button ("Encrypt", GUILayout.ExpandWidth (false))) {
										EpicPrefs.ToggleEncryptionEditorPrefs (entry.Key, entry.Value, type, false);
										return;
									}
									if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
										EpicPrefsEditor.DeleteEditorPref (entry.Key, type, false, true);
										return;
									}
									EditorGUILayout.EndHorizontal ();
									if (foldouts [type.ToString () + "NotEncrypted" + entry.Key]) {
										var deleteList = new List<int> ();
										var Count = (entry.Value).Length;
										if (Count > 0) {
											if (!editable) {
												foreach (var val in entry.Value) {
													EditorGUILayout.BeginHorizontal ();
													GUILayout.Space (60);
													EditorGUILayout.LabelField (val.ToString());
													EditorGUILayout.EndHorizontal ();
												}
											} else {
												var ind = editKeysNE.IndexOf (entry.Key);
												var _valueList = ((float[])editValuesNE [ind]).ToList ();
												for (int m = 0; m < _valueList.Count; m++) {
													EditorGUILayout.BeginHorizontal ();
													GUILayout.Space (40);
													_valueList [m] = Operators.ToFloat(EditorGUILayout.TextField (_valueList [m].ToString()));
													if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
														deleteList.Add (m);
													}
													EditorGUILayout.EndHorizontal ();
												}
												EditorGUILayout.BeginHorizontal ();
												GUILayout.Space (40);
												if (GUILayout.Button ("Add", GUILayout.ExpandWidth (false))) {
													_valueList.Add (0);
												}
												EditorGUILayout.EndHorizontal ();
												foreach (int m in deleteList) {
													_valueList.RemoveAt (m);
												}
												var _replacementArray = new float[_valueList.Count];
												for (int m = 0; m < _valueList.Count; m++) {
													_replacementArray [m] = _valueList [m];
												}
												editValuesNE [ind] = (object)_replacementArray;
											}
										}
									}
								}
								break;
							case Serializer.SerializationTypes.ArrayString:
								foreach (var entry in epicPrefsContainerNotEncrypted.StringArrayDictionary) {
									if (!foldouts.ContainsKey (type.ToString () + "NotEncrypted" + entry.Key)) {
										foldouts [type.ToString () + "NotEncrypted" + entry.Key] = false;
									}
									EditorGUILayout.BeginHorizontal ();
									GUILayout.Space (40);
									foldouts [type.ToString () + "NotEncrypted" + entry.Key] = EditorGUILayout.Foldout (foldouts [type.ToString () + "NotEncrypted" + entry.Key], entry.Key);
									if (GUILayout.Button ("Encrypt", GUILayout.ExpandWidth (false))) {
										EpicPrefs.ToggleEncryptionEditorPrefs (entry.Key, entry.Value, type, false);
										return;
									}
									if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
										EpicPrefsEditor.DeleteEditorPref (entry.Key, type, false, true);
										return;
									}
									EditorGUILayout.EndHorizontal ();
									if (foldouts [type.ToString () + "NotEncrypted" + entry.Key]) {
										var deleteList = new List<int> ();
										var Count = (entry.Value).Length;
										if (Count > 0) {
											if (!editable) {
												foreach (var val in entry.Value) {
													EditorGUILayout.BeginHorizontal ();
													GUILayout.Space (60);
													EditorGUILayout.LabelField (val.ToString());
													EditorGUILayout.EndHorizontal ();
												}
											} else {
												var ind = editKeysNE.IndexOf (entry.Key);
												var _valueList = ((string[])editValuesNE [ind]).ToList ();
												for (int m = 0; m < _valueList.Count; m++) {
													EditorGUILayout.BeginHorizontal ();
													GUILayout.Space (40);
													_valueList [m] = EditorGUILayout.TextField (_valueList [m]);
													if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
														deleteList.Add (m);
													}
													EditorGUILayout.EndHorizontal ();
												}
												EditorGUILayout.BeginHorizontal ();
												GUILayout.Space (40);
												if (GUILayout.Button ("Add", GUILayout.ExpandWidth (false))) {
													_valueList.Add ("");
												}
												EditorGUILayout.EndHorizontal ();
												foreach (int m in deleteList) {
													_valueList.RemoveAt (m);
												}
												var _replacementArray = new string[_valueList.Count];
												for (int m = 0; m < _valueList.Count; m++) {
													_replacementArray [m] = _valueList [m];
												}
												editValuesNE [ind] = (object)_replacementArray;
											}
										}
									}
								}
								break;
							case Serializer.SerializationTypes.ListD:
								foreach (var entry in epicPrefsContainerNotEncrypted.DoubleListDictionary) {
									if (!foldouts.ContainsKey (type.ToString () + "NotEncrypted" + entry.Key)) {
										foldouts [type.ToString () + "NotEncrypted" + entry.Key] = false;
									}
									EditorGUILayout.BeginHorizontal ();
									GUILayout.Space (40);
									foldouts [type.ToString () + "NotEncrypted" + entry.Key] = EditorGUILayout.Foldout (foldouts [type.ToString () + "NotEncrypted" + entry.Key], entry.Key);
									if (GUILayout.Button ("Encrypt", GUILayout.ExpandWidth (false))) {
										EpicPrefs.ToggleEncryptionEditorPrefs (entry.Key, entry.Value, type, false);
										return;
									}
									if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
										EpicPrefsEditor.DeleteEditorPref (entry.Key, type, false, true);
										return;
									}
									EditorGUILayout.EndHorizontal ();
									if (foldouts [type.ToString () + "NotEncrypted" + entry.Key]) {
										var deleteList = new List<int> ();
										var Count = (entry.Value).Count;
										if (Count > 0) {
											if (!editable) {
												foreach (var val in entry.Value) {
													EditorGUILayout.BeginHorizontal ();
													GUILayout.Space (60);
													EditorGUILayout.LabelField (val.ToString());
													EditorGUILayout.EndHorizontal ();
												}
											} else {
												var ind = editKeysNE.IndexOf (entry.Key);
												var _valueList = ((List<double>)editValuesNE [ind]);
												for (int m = 0; m < _valueList.Count; m++) {
													EditorGUILayout.BeginHorizontal ();
													GUILayout.Space (40);
													_valueList [m] = Operators.ToDouble(EditorGUILayout.TextField (_valueList [m].ToString()));
													if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
														deleteList.Add (m);
													}
													EditorGUILayout.EndHorizontal ();
												}
												EditorGUILayout.BeginHorizontal ();
												GUILayout.Space (40);
												if (GUILayout.Button ("Add", GUILayout.ExpandWidth (false))) {
													_valueList.Add (0);
												}
												EditorGUILayout.EndHorizontal ();
												foreach (int m in deleteList) {
													_valueList.RemoveAt (m);
												}
												var _replacementArray = new List<double> ();
												for (int m = 0; m < _valueList.Count; m++) {
													_replacementArray.Add (_valueList [m]);
												}
												editValuesNE [ind] = (object)_replacementArray;
											}
										}
									}
								}
								break;
							case Serializer.SerializationTypes.ListF:
								foreach (var entry in epicPrefsContainerNotEncrypted.FloatListDictionary) {
									if (!foldouts.ContainsKey (type.ToString () + "NotEncrypted" + entry.Key)) {
										foldouts [type.ToString () + "NotEncrypted" + entry.Key] = false;
									}
									EditorGUILayout.BeginHorizontal ();
									GUILayout.Space (40);
									foldouts [type.ToString () + "NotEncrypted" + entry.Key] = EditorGUILayout.Foldout (foldouts [type.ToString () + "NotEncrypted" + entry.Key], entry.Key);
									if (GUILayout.Button ("Encrypt", GUILayout.ExpandWidth (false))) {
										EpicPrefs.ToggleEncryptionEditorPrefs (entry.Key, entry.Value, type, false);
										return;
									}
									if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
										EpicPrefsEditor.DeleteEditorPref (entry.Key, type, false, true);
										return;
									}
									EditorGUILayout.EndHorizontal ();
									if (foldouts [type.ToString () + "NotEncrypted" + entry.Key]) {
										var deleteList = new List<int> ();
										var Count = (entry.Value).Count;
										if (Count > 0) {
											if (!editable) {
												foreach (var val in entry.Value) {
													EditorGUILayout.BeginHorizontal ();
													GUILayout.Space (60);
													EditorGUILayout.LabelField (val.ToString());
													EditorGUILayout.EndHorizontal ();
												}
											} else {
												var ind = editKeysNE.IndexOf (entry.Key);
												var _valueList = ((List<float>)editValuesNE [ind]);
												for (int m = 0; m < _valueList.Count; m++) {
													EditorGUILayout.BeginHorizontal ();
													GUILayout.Space (40);
													_valueList [m] = Operators.ToFloat(EditorGUILayout.TextField (_valueList [m].ToString()));
													if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
														deleteList.Add (m);
													}
													EditorGUILayout.EndHorizontal ();
												}
												EditorGUILayout.BeginHorizontal ();
												GUILayout.Space (40);
												if (GUILayout.Button ("Add", GUILayout.ExpandWidth (false))) {
													_valueList.Add (0);
												}
												EditorGUILayout.EndHorizontal ();
												foreach (int m in deleteList) {
													_valueList.RemoveAt (m);
												}
												var _replacementArray = new List<float> ();
												for (int m = 0; m < _valueList.Count; m++) {
													_replacementArray.Add (_valueList [m]);
												}
												editValuesNE [ind] = (object)_replacementArray;
											}
										}
									}
								}
								break;
							case Serializer.SerializationTypes.ListI:
								foreach (var entry in epicPrefsContainerNotEncrypted.IntegerListDictionary) {
									if (!foldouts.ContainsKey (type.ToString () + "NotEncrypted" + entry.Key)) {
										foldouts [type.ToString () + "NotEncrypted" + entry.Key] = false;
									}
									EditorGUILayout.BeginHorizontal ();
									GUILayout.Space (40);
									foldouts [type.ToString () + "NotEncrypted" + entry.Key] = EditorGUILayout.Foldout (foldouts [type.ToString () + "NotEncrypted" + entry.Key], entry.Key);
									if (GUILayout.Button ("Encrypt", GUILayout.ExpandWidth (false))) {
										EpicPrefs.ToggleEncryptionEditorPrefs (entry.Key, entry.Value, type, false);
										return;
									}
									if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
										EpicPrefsEditor.DeleteEditorPref (entry.Key, type, false, true);
										return;
									}
									EditorGUILayout.EndHorizontal ();
									if (foldouts [type.ToString () + "NotEncrypted" + entry.Key]) {
										var deleteList = new List<int> ();
										var Count = (entry.Value).Count;
										if (Count > 0) {
											if (!editable) {
												foreach (var val in entry.Value) {
													EditorGUILayout.BeginHorizontal ();
													GUILayout.Space (60);
													EditorGUILayout.LabelField (val.ToString());
													EditorGUILayout.EndHorizontal ();
												}
											} else {
												var ind = editKeysNE.IndexOf (entry.Key);
												var _valueList = ((List<int>)editValuesNE [ind]);
												for (int m = 0; m < _valueList.Count; m++) {
													EditorGUILayout.BeginHorizontal ();
													GUILayout.Space (40);
													_valueList [m] = Operators.ToInt(EditorGUILayout.TextField (_valueList [m].ToString()));
													if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
														deleteList.Add (m);
													}
													EditorGUILayout.EndHorizontal ();
												}
												EditorGUILayout.BeginHorizontal ();
												GUILayout.Space (40);
												if (GUILayout.Button ("Add", GUILayout.ExpandWidth (false))) {
													_valueList.Add (0);
												}
												EditorGUILayout.EndHorizontal ();
												foreach (int m in deleteList) {
													_valueList.RemoveAt (m);
												}
												var _replacementArray = new List<int> ();
												for (int m = 0; m < _valueList.Count; m++) {
													_replacementArray.Add (_valueList [m]);
												}
												editValuesNE [ind] = (object)_replacementArray;
											}
										}
									}
								}
								break;
							case Serializer.SerializationTypes.ListL:
								foreach (var entry in epicPrefsContainerNotEncrypted.LongListDictionary) {
									if (!foldouts.ContainsKey (type.ToString () + "NotEncrypted" + entry.Key)) {
										foldouts [type.ToString () + "NotEncrypted" + entry.Key] = false;
									}
									EditorGUILayout.BeginHorizontal ();
									GUILayout.Space (40);
									foldouts [type.ToString () + "NotEncrypted" + entry.Key] = EditorGUILayout.Foldout (foldouts [type.ToString () + "NotEncrypted" + entry.Key], entry.Key);
									if (GUILayout.Button ("Encrypt", GUILayout.ExpandWidth (false))) {
										EpicPrefs.ToggleEncryptionEditorPrefs (entry.Key, entry.Value, type, false);
										return;
									}
									if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
										EpicPrefsEditor.DeleteEditorPref (entry.Key, type, false, true);
										return;
									}
									EditorGUILayout.EndHorizontal ();
									if (foldouts [type.ToString () + "NotEncrypted" + entry.Key]) {
										var deleteList = new List<int> ();
										var Count = (entry.Value).Count;
										if (Count > 0) {
											if (!editable) {
												foreach (var val in entry.Value) {
													EditorGUILayout.BeginHorizontal ();
													GUILayout.Space (60);
													EditorGUILayout.LabelField (val.ToString());
													EditorGUILayout.EndHorizontal ();
												}
											} else {
												var ind = editKeysNE.IndexOf (entry.Key);
												var _valueList = ((List<long>)editValuesNE [ind]);
												for (int m = 0; m < _valueList.Count; m++) {
													EditorGUILayout.BeginHorizontal ();
													GUILayout.Space (40);
													_valueList [m] = Operators.ToLong(EditorGUILayout.TextField (_valueList [m].ToString()));
													if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
														deleteList.Add (m);
													}
													EditorGUILayout.EndHorizontal ();
												}
												EditorGUILayout.BeginHorizontal ();
												GUILayout.Space (40);
												if (GUILayout.Button ("Add", GUILayout.ExpandWidth (false))) {
													_valueList.Add (0);
												}
												EditorGUILayout.EndHorizontal ();
												foreach (int m in deleteList) {
													_valueList.RemoveAt (m);
												}
												var _replacementArray = new List<long> ();
												for (int m = 0; m < _valueList.Count; m++) {
													_replacementArray.Add (_valueList [m]);
												}
												editValuesNE [ind] = (object)_replacementArray;
											}
										}
									}
								}
								break;
							case Serializer.SerializationTypes.ListS:
								foreach (var entry in epicPrefsContainerNotEncrypted.StringListDictionary) {
									if (!foldouts.ContainsKey (type.ToString () + "NotEncrypted" + entry.Key)) {
										foldouts [type.ToString () + "NotEncrypted" + entry.Key] = false;
									}
									EditorGUILayout.BeginHorizontal ();
									GUILayout.Space (40);
									foldouts [type.ToString () + "NotEncrypted" + entry.Key] = EditorGUILayout.Foldout (foldouts [type.ToString () + "NotEncrypted" + entry.Key], entry.Key);
									if (GUILayout.Button ("Encrypt", GUILayout.ExpandWidth (false))) {
										EpicPrefs.ToggleEncryptionEditorPrefs (entry.Key, entry.Value, type, false);
										return;
									}
									if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
										EpicPrefsEditor.DeleteEditorPref (entry.Key, type, false, true);
										return;
									}
									EditorGUILayout.EndHorizontal ();
									if (foldouts [type.ToString () + "NotEncrypted" + entry.Key]) {
										var deleteList = new List<int> ();
										var Count = (entry.Value).Count;
										if (Count > 0) {
											if (!editable) {
												foreach (var val in entry.Value) {
													EditorGUILayout.BeginHorizontal ();
													GUILayout.Space (60);
													EditorGUILayout.LabelField (val);
													EditorGUILayout.EndHorizontal ();
												}
											} else {
												var ind = editKeysNE.IndexOf (entry.Key);
												var _valueList = ((List<string>)editValuesNE [ind]);
												for (int m = 0; m < _valueList.Count; m++) {
													EditorGUILayout.BeginHorizontal ();
													GUILayout.Space (40);
													_valueList [m] = EditorGUILayout.TextField (_valueList [m]);
													if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
														deleteList.Add (m);
													}
													EditorGUILayout.EndHorizontal ();
												}
												EditorGUILayout.BeginHorizontal ();
												GUILayout.Space (40);
												if (GUILayout.Button ("Add", GUILayout.ExpandWidth (false))) {
													_valueList.Add ("");
												}
												EditorGUILayout.EndHorizontal ();
												foreach (int m in deleteList) {
													_valueList.RemoveAt (m);
												}
												var _replacementArray = new List<string> ();
												for (int m = 0; m < _valueList.Count; m++) {
													_replacementArray.Add (_valueList [m]);
												}
												editValuesNE [ind] = (object)_replacementArray;
											}
										}
									}
								}
								break;
							case Serializer.SerializationTypes.ListB:
								foreach (var entry in epicPrefsContainerNotEncrypted.BooleanListDictionary) {
									if (!foldouts.ContainsKey (type.ToString () + "NotEncrypted" + entry.Key)) {
										foldouts [type.ToString () + "NotEncrypted" + entry.Key] = false;
									}
									EditorGUILayout.BeginHorizontal ();
									GUILayout.Space (40);
									foldouts [type.ToString () + "NotEncrypted" + entry.Key] = EditorGUILayout.Foldout (foldouts [type.ToString () + "NotEncrypted" + entry.Key], entry.Key);
									if (GUILayout.Button ("Encrypt", GUILayout.ExpandWidth (false))) {
										EpicPrefs.ToggleEncryptionEditorPrefs (entry.Key, entry.Value, type, false);
										return;
									}
									if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
										EpicPrefsEditor.DeleteEditorPref (entry.Key, type, false, true);
										return;
									}
									EditorGUILayout.EndHorizontal ();
									if (foldouts [type.ToString () + "NotEncrypted" + entry.Key]) {
										var deleteList = new List<int> ();
										var Count = (entry.Value).Count;
										if (Count > 0) {
											if (!editable) {
												foreach (var val in entry.Value) {
													EditorGUILayout.BeginHorizontal ();
													GUILayout.Space (60);
													EditorGUILayout.Toggle (val);
													EditorGUILayout.EndHorizontal ();
												}
											} else {
												var ind = editKeysNE.IndexOf (entry.Key);
												var _valueList = ((List<bool>)editValuesNE [ind]);
												for (int m = 0; m < _valueList.Count; m++) {
													EditorGUILayout.BeginHorizontal ();
													GUILayout.Space (40);
													_valueList [m] = EditorGUILayout.Toggle (_valueList [m]);
													if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
														deleteList.Add (m);
													}
													EditorGUILayout.EndHorizontal ();
												}
												EditorGUILayout.BeginHorizontal ();
												GUILayout.Space (40);
												if (GUILayout.Button ("Add", GUILayout.ExpandWidth (false))) {
													_valueList.Add (false);
												}
												EditorGUILayout.EndHorizontal ();
												foreach (int m in deleteList) {
													_valueList.RemoveAt (m);
												}
												var _replacementArray = new List<bool> ();
												for (int m = 0; m < _valueList.Count; m++) {
													_replacementArray.Add (_valueList [m]);
												}
												editValuesNE [ind] = (object)_replacementArray;
											}
										}
									}
								}
								break;
							default:
								break;
							}

						}
					}
					if (encCount > 0) {
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Space (20);
						foldouts [type.ToString () + "Encrypted"] = EditorGUILayout.Foldout (foldouts [type.ToString () + "Encrypted"], "Encrypted");
						EditorGUILayout.EndHorizontal ();
						if (foldouts [type.ToString () + "Encrypted"]){
							switch (type) {
							case Serializer.SerializationTypes.ArrayDouble:
								foreach (var entry in epicPrefsContainerEncrypted.DoubleArrayDictionary) {
									if (!foldouts.ContainsKey (type.ToString () + "Encrypted" + entry.Key)) {
										foldouts [type.ToString () + "Encrypted" + entry.Key] = false;
									}
									EditorGUILayout.BeginHorizontal ();
									GUILayout.Space (40);
									foldouts [type.ToString () + "Encrypted" + entry.Key] = EditorGUILayout.Foldout (foldouts [type.ToString () + "Encrypted" + entry.Key], entry.Key);
									if (GUILayout.Button ("Decrypt", GUILayout.ExpandWidth (false))) {
										EpicPrefs.ToggleEncryptionEditorPrefs (entry.Key, entry.Value, type, true);
										return;
									}
									if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
										EpicPrefsEditor.DeleteEditorPref (entry.Key, type, true, true);
										return;
									}
									EditorGUILayout.EndHorizontal ();
									if (foldouts [type.ToString () + "Encrypted" + entry.Key]) {
										var deleteList = new List<int> ();
										var Count = (entry.Value).Length;
										if (Count > 0) {
											if (!editable) {
												foreach (var val in entry.Value) {
													EditorGUILayout.BeginHorizontal ();
													GUILayout.Space (60);
													EditorGUILayout.LabelField (val.ToString());
													EditorGUILayout.EndHorizontal ();
												}
											} else {
												var ind = editKeysE.IndexOf (entry.Key);
												var _valueList = ((double[])editValuesE [ind]).ToList ();
												for (int m = 0; m < _valueList.Count; m++) {
													EditorGUILayout.BeginHorizontal ();
													GUILayout.Space (40);
													_valueList [m] = Operators.ToDouble(EditorGUILayout.TextField (_valueList [m].ToString()));
													if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
														deleteList.Add (m);
													}
													EditorGUILayout.EndHorizontal ();
												}
												EditorGUILayout.BeginHorizontal ();
												GUILayout.Space (40);
												if (GUILayout.Button ("Add", GUILayout.ExpandWidth (false))) {
													_valueList.Add (0);
												}
												EditorGUILayout.EndHorizontal ();
												foreach (int m in deleteList) {
													_valueList.RemoveAt (m);
												}
												var _replacementArray = new double[_valueList.Count];
												for (int m = 0; m < _valueList.Count; m++) {
													_replacementArray [m] = _valueList [m];
												}
												editValuesE [ind] = (object)_replacementArray;
											}
										}
									}
								}
								break;
							case Serializer.SerializationTypes.ArrayInt:
								foreach (var entry in epicPrefsContainerEncrypted.IntegerArrayDictionary) {
									if (!foldouts.ContainsKey (type.ToString () + "Encrypted" + entry.Key)) {
										foldouts [type.ToString () + "Encrypted" + entry.Key] = false;
									}
									EditorGUILayout.BeginHorizontal ();
									GUILayout.Space (40);
									foldouts [type.ToString () + "Encrypted" + entry.Key] = EditorGUILayout.Foldout (foldouts [type.ToString () + "Encrypted" + entry.Key], entry.Key);
									if (GUILayout.Button ("Decrypt", GUILayout.ExpandWidth (false))) {
										EpicPrefs.ToggleEncryptionEditorPrefs (entry.Key, entry.Value, type, true);
										return;
									}
									if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
										EpicPrefsEditor.DeleteEditorPref (entry.Key, type, true, true);
										return;
									}
									EditorGUILayout.EndHorizontal ();
									if (foldouts [type.ToString () + "Encrypted" + entry.Key]) {
										var deleteList = new List<int> ();
										var Count = (entry.Value).Length;
										if (Count > 0) {
											if (!editable) {
												foreach (var val in entry.Value) {
													EditorGUILayout.BeginHorizontal ();
													GUILayout.Space (60);
													EditorGUILayout.LabelField (val.ToString());
													EditorGUILayout.EndHorizontal ();
												}
											} else {
												var ind = editKeysE.IndexOf (entry.Key);
												var _valueList = ((int[])editValuesE [ind]).ToList ();
												for (int m = 0; m < _valueList.Count; m++) {
													EditorGUILayout.BeginHorizontal ();
													GUILayout.Space (40);
													_valueList [m] = Operators.ToInt(EditorGUILayout.TextField (_valueList [m].ToString()));
													if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
														deleteList.Add (m);
													}
													EditorGUILayout.EndHorizontal ();
												}
												EditorGUILayout.BeginHorizontal ();
												GUILayout.Space (40);
												if (GUILayout.Button ("Add", GUILayout.ExpandWidth (false))) {
													_valueList.Add (0);
												}
												EditorGUILayout.EndHorizontal ();
												foreach (int m in deleteList) {
													_valueList.RemoveAt (m);
												}
												var _replacementArray = new int[_valueList.Count];
												for (int m = 0; m < _valueList.Count; m++) {
													_replacementArray [m] = _valueList [m];
												}
												editValuesE [ind] = (object)_replacementArray;
											}
										}
									}
								}
								break;
							case Serializer.SerializationTypes.ArrayFloat:
								foreach (var entry in epicPrefsContainerEncrypted.FloatArrayDictionary) {
									if (!foldouts.ContainsKey (type.ToString () + "Encrypted" + entry.Key)) {
										foldouts [type.ToString () + "Encrypted" + entry.Key] = false;
									}
									EditorGUILayout.BeginHorizontal ();
									GUILayout.Space (40);
									foldouts [type.ToString () + "Encrypted" + entry.Key] = EditorGUILayout.Foldout (foldouts [type.ToString () + "Encrypted" + entry.Key], entry.Key);
									if (GUILayout.Button ("Decrypt", GUILayout.ExpandWidth (false))) {
										EpicPrefs.ToggleEncryptionEditorPrefs (entry.Key, entry.Value, type, true);
										return;
									}
									if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
										EpicPrefsEditor.DeleteEditorPref (entry.Key, type, true, true);
										return;
									}
									EditorGUILayout.EndHorizontal ();
									if (foldouts [type.ToString () + "Encrypted" + entry.Key]) {
										var deleteList = new List<int> ();
										var Count = (entry.Value).Length;
										if (Count > 0) {
											if (!editable) {
												foreach (var val in entry.Value) {
													EditorGUILayout.BeginHorizontal ();
													GUILayout.Space (60);
													EditorGUILayout.LabelField (val.ToString());
													EditorGUILayout.EndHorizontal ();
												}
											} else {
												var ind = editKeysE.IndexOf (entry.Key);
												var _valueList = ((float[])editValuesE [ind]).ToList ();
												for (int m = 0; m < _valueList.Count; m++) {
													EditorGUILayout.BeginHorizontal ();
													GUILayout.Space (40);
													_valueList [m] = Operators.ToFloat(EditorGUILayout.TextField (_valueList [m].ToString()));
													if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
														deleteList.Add (m);
													}
													EditorGUILayout.EndHorizontal ();
												}
												EditorGUILayout.BeginHorizontal ();
												GUILayout.Space (40);
												if (GUILayout.Button ("Add", GUILayout.ExpandWidth (false))) {
													_valueList.Add (0);
												}
												EditorGUILayout.EndHorizontal ();
												foreach (int m in deleteList) {
													_valueList.RemoveAt (m);
												}
												var _replacementArray = new float[_valueList.Count];
												for (int m = 0; m < _valueList.Count; m++) {
													_replacementArray [m] = _valueList [m];
												}
												editValuesE [ind] = (object)_replacementArray;
											}
										}
									}
								}
								break;
							case Serializer.SerializationTypes.ArrayString:
								foreach (var entry in epicPrefsContainerEncrypted.StringArrayDictionary) {
									if (!foldouts.ContainsKey (type.ToString () + "Encrypted" + entry.Key)) {
										foldouts [type.ToString () + "Encrypted" + entry.Key] = false;
									}
									EditorGUILayout.BeginHorizontal ();
									GUILayout.Space (40);
									foldouts [type.ToString () + "Encrypted" + entry.Key] = EditorGUILayout.Foldout (foldouts [type.ToString () + "Encrypted" + entry.Key], entry.Key);
									if (GUILayout.Button ("Decrypt", GUILayout.ExpandWidth (false))) {
										EpicPrefs.ToggleEncryptionEditorPrefs (entry.Key, entry.Value, type, true);
										return;
									}
									if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
										EpicPrefsEditor.DeleteEditorPref (entry.Key, type, true, true);
										return;
									}
									EditorGUILayout.EndHorizontal ();
									if (foldouts [type.ToString () + "Encrypted" + entry.Key]) {
										var deleteList = new List<int> ();
										var Count = (entry.Value).Length;
										if (Count > 0) {
											if (!editable) {
												foreach (var val in entry.Value) {
													EditorGUILayout.BeginHorizontal ();
													GUILayout.Space (60);
													EditorGUILayout.LabelField (val.ToString());
													EditorGUILayout.EndHorizontal ();
												}
											} else {
												var ind = editKeysE.IndexOf (entry.Key);
												var _valueList = ((string[])editValuesE [ind]).ToList ();
												for (int m = 0; m < _valueList.Count; m++) {
													EditorGUILayout.BeginHorizontal ();
													GUILayout.Space (40);
													_valueList [m] = EditorGUILayout.TextField (_valueList [m]);
													if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
														deleteList.Add (m);
													}
													EditorGUILayout.EndHorizontal ();
												}
												EditorGUILayout.BeginHorizontal ();
												GUILayout.Space (40);
												if (GUILayout.Button ("Add", GUILayout.ExpandWidth (false))) {
													_valueList.Add ("");
												}
												EditorGUILayout.EndHorizontal ();
												foreach (int m in deleteList) {
													_valueList.RemoveAt (m);
												}
												var _replacementArray = new string[_valueList.Count];
												for (int m = 0; m < _valueList.Count; m++) {
													_replacementArray [m] = _valueList [m];
												}
												editValuesE [ind] = (object)_replacementArray;
											}
										}
									}
								}
								break;
							case Serializer.SerializationTypes.ListD:
								foreach (var entry in epicPrefsContainerEncrypted.DoubleListDictionary) {
									if (!foldouts.ContainsKey (type.ToString () + "Encrypted" + entry.Key)) {
										foldouts [type.ToString () + "Encrypted" + entry.Key] = false;
									}
									EditorGUILayout.BeginHorizontal ();
									GUILayout.Space (40);
									foldouts [type.ToString () + "Encrypted" + entry.Key] = EditorGUILayout.Foldout (foldouts [type.ToString () + "Encrypted" + entry.Key], entry.Key);
									if (GUILayout.Button ("Decrypt", GUILayout.ExpandWidth (false))) {
										EpicPrefs.ToggleEncryptionEditorPrefs (entry.Key, entry.Value, type, true);
										return;
									}
									if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
										EpicPrefsEditor.DeleteEditorPref (entry.Key, type, true, true);
										return;
									}
									EditorGUILayout.EndHorizontal ();
									if (foldouts [type.ToString () + "Encrypted" + entry.Key]) {
										var deleteList = new List<int> ();
										var Count = (entry.Value).Count;
										if (Count > 0) {
											if (!editable) {
												foreach (var val in entry.Value) {
													EditorGUILayout.BeginHorizontal ();
													GUILayout.Space (60);
													EditorGUILayout.LabelField (val.ToString());
													EditorGUILayout.EndHorizontal ();
												}
											} else {
												var ind = editKeysE.IndexOf (entry.Key);
												var _valueList = ((List<double>)editValuesE [ind]);
												for (int m = 0; m < _valueList.Count; m++) {
													EditorGUILayout.BeginHorizontal ();
													GUILayout.Space (40);
													_valueList [m] = Operators.ToDouble(EditorGUILayout.TextField (_valueList [m].ToString()));
													if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
														deleteList.Add (m);
													}
													EditorGUILayout.EndHorizontal ();
												}
												EditorGUILayout.BeginHorizontal ();
												GUILayout.Space (40);
												if (GUILayout.Button ("Add", GUILayout.ExpandWidth (false))) {
													_valueList.Add (0);
												}
												EditorGUILayout.EndHorizontal ();
												foreach (int m in deleteList) {
													_valueList.RemoveAt (m);
												}
												var _replacementArray = new List<double> ();
												for (int m = 0; m < _valueList.Count; m++) {
													_replacementArray.Add (_valueList [m]);
												}
												editValuesE [ind] = (object)_replacementArray;
											}
										}
									}
								}
								break;
							case Serializer.SerializationTypes.ListF:
								foreach (var entry in epicPrefsContainerEncrypted.FloatListDictionary) {
									if (!foldouts.ContainsKey (type.ToString () + "Encrypted" + entry.Key)) {
										foldouts [type.ToString () + "Encrypted" + entry.Key] = false;
									}
									EditorGUILayout.BeginHorizontal ();
									GUILayout.Space (40);
									foldouts [type.ToString () + "Encrypted" + entry.Key] = EditorGUILayout.Foldout (foldouts [type.ToString () + "Encrypted" + entry.Key], entry.Key);
									if (GUILayout.Button ("Decrypt", GUILayout.ExpandWidth (false))) {
										EpicPrefs.ToggleEncryptionEditorPrefs (entry.Key, entry.Value, type, true);
										return;
									}
									if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
										EpicPrefsEditor.DeleteEditorPref (entry.Key, type, true, true);
										return;
									}
									EditorGUILayout.EndHorizontal ();
									if (foldouts [type.ToString () + "Encrypted" + entry.Key]) {
										var deleteList = new List<int> ();
										var Count = (entry.Value).Count;
										if (Count > 0) {
											if (!editable) {
												foreach (var val in entry.Value) {
													EditorGUILayout.BeginHorizontal ();
													GUILayout.Space (60);
													EditorGUILayout.LabelField (val.ToString());
													EditorGUILayout.EndHorizontal ();
												}
											} else {
												var ind = editKeysE.IndexOf (entry.Key);
												var _valueList = ((List<float>)editValuesE [ind]);
												for (int m = 0; m < _valueList.Count; m++) {
													EditorGUILayout.BeginHorizontal ();
													GUILayout.Space (40);
													_valueList [m] = Operators.ToFloat(EditorGUILayout.TextField (_valueList [m].ToString()));
													if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
														deleteList.Add (m);
													}
													EditorGUILayout.EndHorizontal ();
												}
												EditorGUILayout.BeginHorizontal ();
												GUILayout.Space (40);
												if (GUILayout.Button ("Add", GUILayout.ExpandWidth (false))) {
													_valueList.Add (0);
												}
												EditorGUILayout.EndHorizontal ();
												foreach (int m in deleteList) {
													_valueList.RemoveAt (m);
												}
												var _replacementArray = new List<float> ();
												for (int m = 0; m < _valueList.Count; m++) {
													_replacementArray.Add (_valueList [m]);
												}
												editValuesE [ind] = (object)_replacementArray;
											}
										}
									}
								}
								break;
							case Serializer.SerializationTypes.ListI:
								foreach (var entry in epicPrefsContainerEncrypted.IntegerListDictionary) {
									if (!foldouts.ContainsKey (type.ToString () + "Encrypted" + entry.Key)) {
										foldouts [type.ToString () + "Encrypted" + entry.Key] = false;
									}
									EditorGUILayout.BeginHorizontal ();
									GUILayout.Space (40);
									foldouts [type.ToString () + "Encrypted" + entry.Key] = EditorGUILayout.Foldout (foldouts [type.ToString () + "Encrypted" + entry.Key], entry.Key);
									if (GUILayout.Button ("Decrypt", GUILayout.ExpandWidth (false))) {
										EpicPrefs.ToggleEncryptionEditorPrefs (entry.Key, entry.Value, type, true);
										return;
									}
									if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
										EpicPrefsEditor.DeleteEditorPref (entry.Key, type, true, true);
										return;
									}
									EditorGUILayout.EndHorizontal ();
									if (foldouts [type.ToString () + "Encrypted" + entry.Key]) {
										var deleteList = new List<int> ();
										var Count = (entry.Value).Count;
										if (Count > 0) {
											if (!editable) {
												foreach (var val in entry.Value) {
													EditorGUILayout.BeginHorizontal ();
													GUILayout.Space (60);
													EditorGUILayout.LabelField (val.ToString());
													EditorGUILayout.EndHorizontal ();
												}
											} else {
												var ind = editKeysE.IndexOf (entry.Key);
												var _valueList = ((List<int>)editValuesE [ind]);
												for (int m = 0; m < _valueList.Count; m++) {
													EditorGUILayout.BeginHorizontal ();
													GUILayout.Space (40);
													_valueList [m] = Operators.ToInt(EditorGUILayout.TextField (_valueList [m].ToString()));
													if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
														deleteList.Add (m);
													}
													EditorGUILayout.EndHorizontal ();
												}
												EditorGUILayout.BeginHorizontal ();
												GUILayout.Space (40);
												if (GUILayout.Button ("Add", GUILayout.ExpandWidth (false))) {
													_valueList.Add (0);
												}
												EditorGUILayout.EndHorizontal ();
												foreach (int m in deleteList) {
													_valueList.RemoveAt (m);
												}
												var _replacementArray = new List<int> ();
												for (int m = 0; m < _valueList.Count; m++) {
													_replacementArray.Add (_valueList [m]);
												}
												editValuesE [ind] = (object)_replacementArray;
											}
										}
									}
								}
								break;
							case Serializer.SerializationTypes.ListL:
								foreach (var entry in epicPrefsContainerEncrypted.LongListDictionary) {
									if (!foldouts.ContainsKey (type.ToString () + "Encrypted" + entry.Key)) {
										foldouts [type.ToString () + "Encrypted" + entry.Key] = false;
									}
									EditorGUILayout.BeginHorizontal ();
									GUILayout.Space (40);
									foldouts [type.ToString () + "Encrypted" + entry.Key] = EditorGUILayout.Foldout (foldouts [type.ToString () + "Encrypted" + entry.Key], entry.Key);
									if (GUILayout.Button ("Decrypt", GUILayout.ExpandWidth (false))) {
										EpicPrefs.ToggleEncryptionEditorPrefs (entry.Key, entry.Value, type, true);
										return;
									}
									if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
										EpicPrefsEditor.DeleteEditorPref (entry.Key, type, true, true);
										return;
									}
									EditorGUILayout.EndHorizontal ();
									if (foldouts [type.ToString () + "Encrypted" + entry.Key]) {
										var deleteList = new List<int> ();
										var Count = (entry.Value).Count;
										if (Count > 0) {
											if (!editable) {
												foreach (var val in entry.Value) {
													EditorGUILayout.BeginHorizontal ();
													GUILayout.Space (60);
													EditorGUILayout.LabelField (val.ToString());
													EditorGUILayout.EndHorizontal ();
												}
											} else {
												var ind = editKeysE.IndexOf (entry.Key);
												var _valueList = ((List<long>)editValuesE [ind]);
												for (int m = 0; m < _valueList.Count; m++) {
													EditorGUILayout.BeginHorizontal ();
													GUILayout.Space (40);
													_valueList [m] = Operators.ToLong(EditorGUILayout.TextField (_valueList [m].ToString()));
													if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
														deleteList.Add (m);
													}
													EditorGUILayout.EndHorizontal ();
												}
												EditorGUILayout.BeginHorizontal ();
												GUILayout.Space (40);
												if (GUILayout.Button ("Add", GUILayout.ExpandWidth (false))) {
													_valueList.Add (0);
												}
												EditorGUILayout.EndHorizontal ();
												foreach (int m in deleteList) {
													_valueList.RemoveAt (m);
												}
												var _replacementArray = new List<long> ();
												for (int m = 0; m < _valueList.Count; m++) {
													_replacementArray.Add (_valueList [m]);
												}
												editValuesE [ind] = (object)_replacementArray;
											}
										}
									}
								}
								break;
							case Serializer.SerializationTypes.ListS:
								foreach (var entry in epicPrefsContainerEncrypted.StringListDictionary) {
									if (!foldouts.ContainsKey (type.ToString () + "Encrypted" + entry.Key)) {
										foldouts [type.ToString () + "Encrypted" + entry.Key] = false;
									}
									EditorGUILayout.BeginHorizontal ();
									GUILayout.Space (40);
									foldouts [type.ToString () + "Encrypted" + entry.Key] = EditorGUILayout.Foldout (foldouts [type.ToString () + "Encrypted" + entry.Key], entry.Key);
									if (GUILayout.Button ("Decrypt", GUILayout.ExpandWidth (false))) {
										EpicPrefs.ToggleEncryptionEditorPrefs (entry.Key, entry.Value, type, true);
										return;
									}
									if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
										EpicPrefsEditor.DeleteEditorPref (entry.Key, type, true, true);
										return;
									}
									EditorGUILayout.EndHorizontal ();
									if (foldouts [type.ToString () + "Encrypted" + entry.Key]) {
										var deleteList = new List<int> ();
										var Count = (entry.Value).Count;
										if (Count > 0) {
											if (!editable) {
												foreach (var val in entry.Value) {
													EditorGUILayout.BeginHorizontal ();
													GUILayout.Space (60);
													EditorGUILayout.LabelField (val);
													EditorGUILayout.EndHorizontal ();
												}
											} else {
												var ind = editKeysE.IndexOf (entry.Key);
												var _valueList = ((List<string>)editValuesE [ind]);
												for (int m = 0; m < _valueList.Count; m++) {
													EditorGUILayout.BeginHorizontal ();
													GUILayout.Space (40);
													_valueList [m] = EditorGUILayout.TextField (_valueList [m]);
													if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
														deleteList.Add (m);
													}
													EditorGUILayout.EndHorizontal ();
												}
												EditorGUILayout.BeginHorizontal ();
												GUILayout.Space (40);
												if (GUILayout.Button ("Add", GUILayout.ExpandWidth (false))) {
													_valueList.Add ("");
												}
												EditorGUILayout.EndHorizontal ();
												foreach (int m in deleteList) {
													_valueList.RemoveAt (m);
												}
												var _replacementArray = new List<string> ();
												for (int m = 0; m < _valueList.Count; m++) {
													_replacementArray.Add (_valueList [m]);
												}
												editValuesE [ind] = (object)_replacementArray;
											}
										}
									}
								}
								break;
							case Serializer.SerializationTypes.ListB:
								foreach (var entry in epicPrefsContainerEncrypted.BooleanListDictionary) {
									if (!foldouts.ContainsKey (type.ToString () + "Encrypted" + entry.Key)) {
										foldouts [type.ToString () + "Encrypted" + entry.Key] = false;
									}
									EditorGUILayout.BeginHorizontal ();
									GUILayout.Space (40);
									foldouts [type.ToString () + "Encrypted" + entry.Key] = EditorGUILayout.Foldout (foldouts [type.ToString () + "Encrypted" + entry.Key], entry.Key);
									if (GUILayout.Button ("Decrypt", GUILayout.ExpandWidth (false))) {
										EpicPrefs.ToggleEncryptionEditorPrefs (entry.Key, entry.Value, type, true);
										return;
									}
									if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
										EpicPrefsEditor.DeleteEditorPref (entry.Key, type, true, true);
										return;
									}
									EditorGUILayout.EndHorizontal ();
									if (foldouts [type.ToString () + "Encrypted" + entry.Key]) {
										var deleteList = new List<int> ();
										var Count = (entry.Value).Count;
										if (Count > 0) {
											if (!editable) {
												foreach (var val in entry.Value) {
													EditorGUILayout.BeginHorizontal ();
													GUILayout.Space (60);
													EditorGUILayout.Toggle (val);
													EditorGUILayout.EndHorizontal ();
												}
											} else {
												var ind = editKeysE.IndexOf (entry.Key);
												var _valueList = ((List<bool>)editValuesE [ind]);
												for (int m = 0; m < _valueList.Count; m++) {
													EditorGUILayout.BeginHorizontal ();
													GUILayout.Space (40);
													_valueList [m] = EditorGUILayout.Toggle (_valueList [m]);
													if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
														deleteList.Add (m);
													}
													EditorGUILayout.EndHorizontal ();
												}
												EditorGUILayout.BeginHorizontal ();
												GUILayout.Space (40);
												if (GUILayout.Button ("Add", GUILayout.ExpandWidth (false))) {
													_valueList.Add (false);
												}
												EditorGUILayout.EndHorizontal ();
												foreach (int m in deleteList) {
													_valueList.RemoveAt (m);
												}
												var _replacementArray = new List<bool> ();
												for (int m = 0; m < _valueList.Count; m++) {
													_replacementArray.Add (_valueList [m]);
												}
												editValuesE [ind] = (object)_replacementArray;
											}
										}
									}
								}
								break;
							default:
								break;
							}

						}
					}
				}
			}
		}
		void DisplayVectors(Serializer.SerializationTypes type, bool editable){
			var encCount = 0;
			var notEncCount = 0;
			var intermediateDictEnc = new Dictionary<string,string> ();
			var intermediateDictNotEnc = new Dictionary<string,string> ();
			switch (type) {
			case Serializer.SerializationTypes.Vector2:
				foreach (var entry in epicPrefsContainerEncrypted.Vector2Dictionary) {
					intermediateDictEnc [entry.Key] = Operators.Vector2String (entry.Value);
				}
				foreach (var entry in epicPrefsContainerNotEncrypted.Vector2Dictionary) {
					intermediateDictNotEnc [entry.Key] = Operators.Vector2String (entry.Value);
				}
				break;
			case Serializer.SerializationTypes.Vector3:
				foreach (var entry in epicPrefsContainerEncrypted.Vector3Dictionary) {
					intermediateDictEnc [entry.Key] = Operators.Vector2String (entry.Value);
				}
				foreach (var entry in epicPrefsContainerNotEncrypted.Vector3Dictionary) {
					intermediateDictNotEnc [entry.Key] = Operators.Vector2String (entry.Value);
				}
				break;
			case Serializer.SerializationTypes.Vector4:
				foreach (var entry in epicPrefsContainerEncrypted.Vector4Dictionary) {
					intermediateDictEnc [entry.Key] = Operators.Vector2String (entry.Value);
				}
				foreach (var entry in epicPrefsContainerNotEncrypted.Vector4Dictionary) {
					intermediateDictNotEnc [entry.Key] = Operators.Vector2String (entry.Value);
				}
				break;
			case Serializer.SerializationTypes.Quaternion:
				foreach (var entry in epicPrefsContainerEncrypted.QuaternionDictionary) {
					intermediateDictEnc [entry.Key] = Operators.Quaternion2String (entry.Value);
				}
				foreach (var entry in epicPrefsContainerNotEncrypted.QuaternionDictionary) {
					intermediateDictNotEnc [entry.Key] = Operators.Quaternion2String (entry.Value);
				}
				break;
			default:
				break;
			}
			encCount = intermediateDictEnc.Count;
			notEncCount = intermediateDictNotEnc.Count;
			if (encCount + notEncCount > 0) {
				if (!foldouts.ContainsKey (type.ToString ())) {
					foldouts [type.ToString ()] = false;
				}
				foldouts[type.ToString()] = EditorGUILayout.Foldout(foldouts[type.ToString()], SerializationTypeToString(type));
				if (foldouts [type.ToString ()]) {
					if (!foldouts.ContainsKey (type.ToString () + "NotEncrypted")) {
						foldouts [type.ToString ()+ "NotEncrypted"] = false;
					}
					if (!foldouts.ContainsKey (type.ToString () + "Encrypted")) {
						foldouts [type.ToString ()+ "Encrypted"] = false;
					}
					if (notEncCount > 0) {
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Space (20);
						foldouts [type.ToString () + "NotEncrypted"] = EditorGUILayout.Foldout (foldouts [type.ToString () + "NotEncrypted"], "Not Encrypted");
						EditorGUILayout.EndHorizontal ();
						if (foldouts [type.ToString () + "NotEncrypted"]) {
							if (!editable) {
								foreach (var pair in intermediateDictNotEnc) {
									EditorGUILayout.BeginHorizontal ();
									GUILayout.Space (40);
									EditorGUILayout.LabelField (pair.Key);
									GUILayout.Space (20);
									var components = pair.Value.Substring (1, pair.Value.Length - 2).Split (';');
									foreach (var comp in components) {
										GUILayout.Label (comp);
									}
									if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
										EpicPrefs.DeleteEditorPrefs (pair.Key, type, false);
										return;
									}
									EditorGUILayout.EndHorizontal ();
								}
							} else {
								for (int i = 0; i < editKeysNE.Count; i++) {
									if (editTypesNE [i] == type) {
										EditorGUILayout.BeginHorizontal ();
										GUILayout.Space (40);
										editKeysNE [i] = EditorGUILayout.TextField (editKeysNE [i]);
										GUILayout.Space (20);
										var components = new List<string> (); 
										switch (type) {
										case Serializer.SerializationTypes.Vector2:
											components = Operators.Vector2String((Vector2)editValuesNE[i]).Substring (1, Operators.Vector2String((Vector2)editValuesNE[i]).Length - 2).Split (';').ToList();
											editValuesNE [i] = (object)(Operators.ToVector2 (EditorGUILayout.TextField (components [0]), 
												EditorGUILayout.TextField (components [1])));
											break;
										case Serializer.SerializationTypes.Vector3:
											components = Operators.Vector2String((Vector3)editValuesNE[i]).Substring (1, Operators.Vector2String((Vector3)editValuesNE[i]).Length - 2).Split (';').ToList();
											editValuesNE [i] = (object)(Operators.ToVector3 (EditorGUILayout.TextField (components [0]), 
												EditorGUILayout.TextField (components [1]), 
												EditorGUILayout.TextField (components [2])));
											break;
										case Serializer.SerializationTypes.Vector4:
											components = Operators.Vector2String((Vector4)editValuesNE[i]).Substring (1, Operators.Vector2String((Vector4)editValuesNE[i]).Length - 2).Split (';').ToList();
											editValuesNE [i] = (object)(Operators.ToVector4 (EditorGUILayout.TextField (components [0]), 
												EditorGUILayout.TextField (components [1]), 
												EditorGUILayout.TextField (components [2]), 
												EditorGUILayout.TextField (components [3])));
											break;
										case Serializer.SerializationTypes.Quaternion:
											components = Operators.Quaternion2String((Quaternion)editValuesNE[i]).Substring (1, Operators.Quaternion2String((Quaternion)editValuesNE[i]).Length - 2).Split (';').ToList();
											editValuesNE [i] = (object)(Operators.ToQuaternion (EditorGUILayout.TextField (components [0]), 
												EditorGUILayout.TextField (components [1]), 
												EditorGUILayout.TextField (components [2]), 
												EditorGUILayout.TextField (components [3])));
											break;
										default:
											break;
										}
										EditorGUILayout.EndHorizontal ();
									}
								}
							}
						}
					}
				}
			}
		}
		void DisplayColors(Serializer.SerializationTypes type, bool editable){
			if (type == Serializer.SerializationTypes.Color) {
				var encCount = epicPrefsContainerEncrypted.ColorDictionary.Count;
				var notEncCount = epicPrefsContainerNotEncrypted.ColorDictionary.Count;
				if (encCount + notEncCount > 0) {
					if (!foldouts.ContainsKey (type.ToString ())) {
						foldouts [type.ToString ()] = false;
					}
					foldouts [type.ToString ()] = EditorGUILayout.Foldout (foldouts [type.ToString ()], SerializationTypeToString (type));
					if (foldouts [type.ToString ()]) {
						if (!foldouts.ContainsKey (type.ToString () + "NotEncrypted")) {
							foldouts [type.ToString () + "NotEncrypted"] = false;
						}
						if (!foldouts.ContainsKey (type.ToString () + "Encrypted")) {
							foldouts [type.ToString () + "Encrypted"] = false;
						}
						if (notEncCount > 0) {
							EditorGUILayout.BeginHorizontal ();
							GUILayout.Space (20);
							foldouts [type.ToString () + "NotEncrypted"] = EditorGUILayout.Foldout (foldouts [type.ToString () + "NotEncrypted"], "Not Encrypted");
							EditorGUILayout.EndHorizontal ();
							if (foldouts [type.ToString () + "NotEncrypted"]) {
								if (!editable) {
									foreach (var pair in epicPrefsContainerNotEncrypted.ColorDictionary) {
										EditorGUILayout.BeginHorizontal ();
										GUILayout.Space (40);
										EditorGUILayout.LabelField (pair.Key);
										GUILayout.Space (20);
										EditorGUILayout.ColorField (pair.Value);
										if (GUILayout.Button ("Remove", GUILayout.ExpandWidth (false))) {
											EpicPrefs.DeleteEditorPrefs (pair.Key, type, false);
											return;
										}
										EditorGUILayout.EndHorizontal ();
									}
								} else {
									for (int i = 0; i < editKeysNE.Count; i++) {
										if (editTypesNE [i] == type) {
											EditorGUILayout.BeginHorizontal ();
											GUILayout.Space (40);
											editKeysNE [i] = EditorGUILayout.TextField (editKeysNE [i]);
											GUILayout.Space (20);
											editValuesNE [i] = EditorGUILayout.ColorField ((Color)editValuesNE [i]);
											EditorGUILayout.EndHorizontal ();
										}
									}
								}
							}
						}
					}
				}
			}
		}
		#endregion
		void OnDestroy(){
			Exit ();
		}
		#region EditorPrefs
		public static void SetEditorPref(Serializer.SerializationTypes type,string name, object value, bool encrypted){
			StartUp ();
			if (encrypted) {
				epicPrefsContainerEncrypted.edited = true;
			} else {
				epicPrefsContainerNotEncrypted.edited = true;				
			}
			switch (type) {
			case Serializer.SerializationTypes.Integer:
				if (encrypted) {
					epicPrefsContainerEncrypted.IntegerDictionary [name] = (int)value;	
				} else {
					epicPrefsContainerNotEncrypted.IntegerDictionary [name] = (int)value;					
				}
				break;
			case Serializer.SerializationTypes.String:
				if (encrypted) {
					epicPrefsContainerEncrypted.StringDictionary [name] = value.ToString();	
				} else {
					epicPrefsContainerNotEncrypted.StringDictionary [name] = value.ToString();					
				}
				break;
			case Serializer.SerializationTypes.Float:
				if (encrypted) {
					epicPrefsContainerEncrypted.FloatDictionary [name] = (float)value;	
				} else {
					epicPrefsContainerNotEncrypted.FloatDictionary [name] = (float)value;					
				}
				break;
			case Serializer.SerializationTypes.Long:
				if (encrypted) {
					epicPrefsContainerEncrypted.LongDictionary [name] = (long)value;	
				} else {
					epicPrefsContainerNotEncrypted.LongDictionary [name] = (long)value;					
				}
				break;
			case Serializer.SerializationTypes.Double:
				if (encrypted) {
					epicPrefsContainerEncrypted.DoubleDictionary [name] = (double)value;	
				} else {
					epicPrefsContainerNotEncrypted.DoubleDictionary [name] = (double)value;					
				}
				break;
			case Serializer.SerializationTypes.Bool:
				if (encrypted) {
					epicPrefsContainerEncrypted.BooleanDictionary [name] = (bool)value;	
				} else {
					epicPrefsContainerNotEncrypted.BooleanDictionary [name] = (bool)value;					
				}
				break;
			case Serializer.SerializationTypes.Vector2:
				if (encrypted) {
					epicPrefsContainerEncrypted.Vector2Dictionary [name] = (Vector2)value;	
				} else {
					epicPrefsContainerNotEncrypted.Vector2Dictionary [name] = (Vector2)value;					
				}
				break;
			case Serializer.SerializationTypes.Vector3:
				if (encrypted) {
					epicPrefsContainerEncrypted.Vector3Dictionary [name] = (Vector3)value;	
				} else {
					epicPrefsContainerNotEncrypted.Vector3Dictionary [name] = (Vector3)value;					
				}
				break;
			case Serializer.SerializationTypes.Vector4:
				if (encrypted) {
					epicPrefsContainerEncrypted.Vector4Dictionary [name] = (Vector4)value;	
				} else {
					epicPrefsContainerNotEncrypted.Vector4Dictionary [name] = (Vector4)value;					
				}
				break;
			case Serializer.SerializationTypes.Quaternion:
				if (encrypted) {
					epicPrefsContainerEncrypted.QuaternionDictionary [name] = (Quaternion)value;	
				} else {
					epicPrefsContainerNotEncrypted.QuaternionDictionary [name] = (Quaternion)value;					
				}
				break;
			case Serializer.SerializationTypes.Color:
				if (encrypted) {
					epicPrefsContainerEncrypted.ColorDictionary [name] = (Color)value;	
				} else {
					epicPrefsContainerNotEncrypted.ColorDictionary [name] = (Color)value;					
				}
				break;
			case Serializer.SerializationTypes.ArrayString:
				if (encrypted) {
					epicPrefsContainerEncrypted.StringArrayDictionary [name] = (string[])value;	
				} else {
					epicPrefsContainerNotEncrypted.StringArrayDictionary [name] = (string[])value;					
				}
				break;
			case Serializer.SerializationTypes.ArrayInt:
				if (encrypted) {
					epicPrefsContainerEncrypted.IntegerArrayDictionary [name] = (int[])value;	
				} else {
					epicPrefsContainerNotEncrypted.IntegerArrayDictionary [name] = (int[])value;					
				}
				break;
			case Serializer.SerializationTypes.ArrayFloat:
				if (encrypted) {
					epicPrefsContainerEncrypted.FloatArrayDictionary [name] = (float[])value;	
				} else {
					epicPrefsContainerNotEncrypted.FloatArrayDictionary [name] = (float[])value;					
				}
				break;
			case Serializer.SerializationTypes.ArrayDouble:
				if (encrypted) {
					epicPrefsContainerEncrypted.DoubleArrayDictionary [name] = (double[])value;	
				} else {
					epicPrefsContainerNotEncrypted.DoubleArrayDictionary [name] = (double[])value;					
				}
				break;
			case Serializer.SerializationTypes.DictS:
				if (encrypted) {
					epicPrefsContainerEncrypted.StringDictionaryDictionary [name] = (Dictionary<string,string>)value;	
				} else {
					epicPrefsContainerNotEncrypted.StringDictionaryDictionary [name] = (Dictionary<string,string>)value;					
				}
				break;
			case Serializer.SerializationTypes.DictI:
				if (encrypted) {
					epicPrefsContainerEncrypted.IntegerDictionaryDictionary [name] = (Dictionary<string,int>)value;	
				} else {
					epicPrefsContainerNotEncrypted.IntegerDictionaryDictionary [name] = (Dictionary<string,int>)value;					
				}
				break;
			case Serializer.SerializationTypes.DictL:
				if (encrypted) {
					epicPrefsContainerEncrypted.LongDictionaryDictionary [name] = (Dictionary<string,long>)value;	
				} else {
					epicPrefsContainerNotEncrypted.LongDictionaryDictionary [name] = (Dictionary<string,long>)value;					
				}
				break;
			case Serializer.SerializationTypes.DictD:
				if (encrypted) {
					epicPrefsContainerEncrypted.DoubleDictionaryDictionary [name] = (Dictionary<string,double>)value;	
				} else {
					epicPrefsContainerNotEncrypted.DoubleDictionaryDictionary [name] = (Dictionary<string,double>)value;					
				}
				break;
			case Serializer.SerializationTypes.DictF:
				if (encrypted) {
					epicPrefsContainerEncrypted.FloatDictionaryDictionary [name] = (Dictionary<string,float>)value;	
				} else {
					epicPrefsContainerNotEncrypted.FloatDictionaryDictionary [name] = (Dictionary<string,float>)value;					
				}
				break;
			case Serializer.SerializationTypes.DictB:
				if (encrypted) {
					epicPrefsContainerEncrypted.BooleanDictionaryDictionary [name] = (Dictionary<string,bool>)value;	
				} else {
					epicPrefsContainerNotEncrypted.BooleanDictionaryDictionary [name] = (Dictionary<string,bool>)value;					
				}
				break;
			case Serializer.SerializationTypes.ListS:
				if (encrypted) {
					epicPrefsContainerEncrypted.StringListDictionary [name] = (List<string>)value;	
				} else {
					epicPrefsContainerNotEncrypted.StringListDictionary [name] = (List<string>)value;					
				}
				break;
			case Serializer.SerializationTypes.ListI:
				if (encrypted) {
					epicPrefsContainerEncrypted.IntegerListDictionary [name] = (List<int>)value;	
				} else {
					epicPrefsContainerNotEncrypted.IntegerListDictionary [name] = (List<int>)value;					
				}
				break;
			case Serializer.SerializationTypes.ListB:
				if (encrypted) {
					epicPrefsContainerEncrypted.BooleanListDictionary [name] = (List<bool>)value;	
				} else {
					epicPrefsContainerNotEncrypted.BooleanListDictionary [name] = (List<bool>)value;					
				}
				break;
			case Serializer.SerializationTypes.ListL:
				if (encrypted) {
					epicPrefsContainerEncrypted.LongListDictionary [name] = (List<long>)value;	
				} else {
					epicPrefsContainerNotEncrypted.LongListDictionary [name] = (List<long>)value;					
				}
				break;
			case Serializer.SerializationTypes.ListD:
				if (encrypted) {
					epicPrefsContainerEncrypted.DoubleListDictionary [name] = (List<double>)value;	
				} else {
					epicPrefsContainerNotEncrypted.DoubleListDictionary [name] = (List<double>)value;					
				}
				break;
			case Serializer.SerializationTypes.ListF:
				if (encrypted) {
					epicPrefsContainerEncrypted.FloatListDictionary [name] = (List<float>)value;	
				} else {
					epicPrefsContainerNotEncrypted.FloatListDictionary [name] = (List<float>)value;					
				}
				break;
			default:
				break;
			}
			if (encrypted) {
				if(!batchWrite)
					EpicPrefsEditorContainerHandler.SaveEpicPrefsEditorContainer (epicPrefsContainerEncrypted,true);
			} else {
				if(!batchWrite)
					EpicPrefsEditorContainerHandler.SaveEpicPrefsEditorContainer (epicPrefsContainerNotEncrypted,false);			
			}
			if(instance != null)
				instance.Repaint ();
		}

		public static void DeleteEditorPref(string name,Serializer.SerializationTypes type, bool encrypted,bool delete){
			StartUp ();
			switch (type) {
			case Serializer.SerializationTypes.Integer:
				if (encrypted) {
					epicPrefsContainerEncrypted.IntegerDictionary.Remove (name);	
				} else {
					epicPrefsContainerNotEncrypted.IntegerDictionary.Remove (name);					
				}
				break;
			case Serializer.SerializationTypes.String:
				if (encrypted) {
					epicPrefsContainerEncrypted.StringDictionary.Remove (name);		
				} else {
					epicPrefsContainerNotEncrypted.StringDictionary.Remove (name);						
				}
				break;
			case Serializer.SerializationTypes.Float:
				if (encrypted) {
					epicPrefsContainerEncrypted.FloatDictionary.Remove (name);	
				} else {
					epicPrefsContainerNotEncrypted.FloatDictionary.Remove (name);					
				}
				break;
			case Serializer.SerializationTypes.Long:
				if (encrypted) {
					epicPrefsContainerEncrypted.LongDictionary.Remove (name);	
				} else {
					epicPrefsContainerNotEncrypted.LongDictionary.Remove (name);					
				}
				break;
			case Serializer.SerializationTypes.Double:
				if (encrypted) {
					epicPrefsContainerEncrypted.DoubleDictionary.Remove (name);	
				} else {
					epicPrefsContainerNotEncrypted.DoubleDictionary.Remove (name);					
				}
				break;
			case Serializer.SerializationTypes.Bool:
				if (encrypted) {
					epicPrefsContainerEncrypted.BooleanDictionary.Remove (name);	
				} else {
					epicPrefsContainerNotEncrypted.BooleanDictionary.Remove (name);					
				}
				break;
			case Serializer.SerializationTypes.Vector2:
				if (encrypted) {
					epicPrefsContainerEncrypted.Vector2Dictionary.Remove (name);	
				} else {
					epicPrefsContainerNotEncrypted.Vector2Dictionary.Remove (name);					
				}
				break;
			case Serializer.SerializationTypes.Vector3:
				if (encrypted) {
					epicPrefsContainerEncrypted.Vector3Dictionary.Remove (name);	
				} else {
					epicPrefsContainerNotEncrypted.Vector3Dictionary.Remove (name);					
				}
				break;
			case Serializer.SerializationTypes.Vector4:
				if (encrypted) {
					epicPrefsContainerEncrypted.Vector4Dictionary.Remove (name);	
				} else {
					epicPrefsContainerNotEncrypted.Vector4Dictionary.Remove (name);					
				}
				break;
			case Serializer.SerializationTypes.Quaternion:
				if (encrypted) {
					epicPrefsContainerEncrypted.QuaternionDictionary.Remove (name);	
				} else {
					epicPrefsContainerNotEncrypted.QuaternionDictionary.Remove (name);					
				}
				break;
			case Serializer.SerializationTypes.Color:
				if (encrypted) {
					epicPrefsContainerEncrypted.ColorDictionary.Remove (name);	
				} else {
					epicPrefsContainerNotEncrypted.ColorDictionary.Remove (name);					
				}
				break;
			case Serializer.SerializationTypes.ArrayString:
				if (encrypted) {
					epicPrefsContainerEncrypted.StringArrayDictionary.Remove (name);	
				} else {
					epicPrefsContainerNotEncrypted.StringArrayDictionary.Remove (name);					
				}
				break;
			case Serializer.SerializationTypes.ArrayInt:
				if (encrypted) {
					epicPrefsContainerEncrypted.IntegerArrayDictionary.Remove (name);	
				} else {
					epicPrefsContainerNotEncrypted.IntegerArrayDictionary.Remove (name);					
				}
				break;
			case Serializer.SerializationTypes.ArrayFloat:
				if (encrypted) {
					epicPrefsContainerEncrypted.FloatArrayDictionary.Remove (name);	
				} else {
					epicPrefsContainerNotEncrypted.FloatArrayDictionary.Remove (name);					
				}
				break;
			case Serializer.SerializationTypes.ArrayDouble:
				if (encrypted) {
					epicPrefsContainerEncrypted.DoubleArrayDictionary.Remove (name);	
				} else {
					epicPrefsContainerNotEncrypted.DoubleArrayDictionary.Remove (name);					
				}
				break;
			case Serializer.SerializationTypes.DictS:
				if (encrypted) {
					epicPrefsContainerEncrypted.StringDictionaryDictionary.Remove (name);	
				} else {
					epicPrefsContainerNotEncrypted.StringDictionaryDictionary.Remove (name);					
				}
				break;
			case Serializer.SerializationTypes.DictI:
				if (encrypted) {
					epicPrefsContainerEncrypted.IntegerDictionaryDictionary.Remove (name);	
				} else {
					epicPrefsContainerNotEncrypted.IntegerDictionaryDictionary.Remove (name);					
				}
				break;
			case Serializer.SerializationTypes.DictL:
				if (encrypted) {
					epicPrefsContainerEncrypted.LongDictionaryDictionary.Remove (name);	
				} else {
					epicPrefsContainerNotEncrypted.LongDictionaryDictionary.Remove (name);					
				}
				break;
			case Serializer.SerializationTypes.DictD:
				if (encrypted) {
					epicPrefsContainerEncrypted.DoubleDictionaryDictionary.Remove (name);	
				} else {
					epicPrefsContainerNotEncrypted.DoubleDictionaryDictionary.Remove (name);					
				}
				break;
			case Serializer.SerializationTypes.DictF:
				if (encrypted) {
					epicPrefsContainerEncrypted.FloatDictionaryDictionary.Remove (name);	
				} else {
					epicPrefsContainerNotEncrypted.FloatDictionaryDictionary.Remove (name);					
				}
				break;
			case Serializer.SerializationTypes.DictB:
				if (encrypted) {
					epicPrefsContainerEncrypted.BooleanDictionaryDictionary.Remove (name);	
				} else {
					epicPrefsContainerNotEncrypted.BooleanDictionaryDictionary.Remove (name);					
				}
				break;
			case Serializer.SerializationTypes.ListS:
				if (encrypted) {
					epicPrefsContainerEncrypted.StringListDictionary.Remove (name);	
				} else {
					epicPrefsContainerNotEncrypted.StringListDictionary.Remove (name);					
				}
				break;
			case Serializer.SerializationTypes.ListI:
				if (encrypted) {
					epicPrefsContainerEncrypted.IntegerListDictionary.Remove (name);	
				} else {
					epicPrefsContainerNotEncrypted.IntegerListDictionary.Remove (name);					
				}
				break;
			case Serializer.SerializationTypes.ListB:
				if (encrypted) {
					epicPrefsContainerEncrypted.BooleanListDictionary.Remove (name);	
				} else {
					epicPrefsContainerNotEncrypted.BooleanListDictionary.Remove (name);					
				}
				break;
			case Serializer.SerializationTypes.ListL:
				if (encrypted) {
					epicPrefsContainerEncrypted.LongListDictionary.Remove (name);	
				} else {
					epicPrefsContainerNotEncrypted.LongListDictionary.Remove (name);					
				}
				break;
			case Serializer.SerializationTypes.ListD:
				if (encrypted) {
					epicPrefsContainerEncrypted.DoubleListDictionary.Remove (name);	
				} else {
					epicPrefsContainerNotEncrypted.DoubleListDictionary.Remove (name);					
				}
				break;
			case Serializer.SerializationTypes.ListF:
				if (encrypted) {
					epicPrefsContainerEncrypted.FloatListDictionary.Remove (name);	
				} else {
					epicPrefsContainerNotEncrypted.FloatListDictionary.Remove (name);					
				}
				break;
			default:
				break;
			}
			if(instance != null)
				instance.Repaint ();
		}
		public static void DeleteAllEditorPrefs(bool encrypted){
			StartUp ();
			if (encrypted) {
				epicPrefsContainerEncrypted = new EpicPrefsEditorContainer ();
			} else {
				epicPrefsContainerNotEncrypted = new EpicPrefsEditorContainer ();
			}
			if (encrypted) {
				if(!batchWrite)
					EpicPrefsEditorContainerHandler.SaveEpicPrefsEditorContainer (epicPrefsContainerEncrypted,true);
			} else {
				if(!batchWrite)
					EpicPrefsEditorContainerHandler.SaveEpicPrefsEditorContainer (epicPrefsContainerNotEncrypted,false);			
			}
			if(instance != null)
				instance.Repaint ();
		}

		private static string SerializationTypeToString(Serializer.SerializationTypes type){
			var returnValue = "";
			switch (type) {
			case Serializer.SerializationTypes.Integer:
				returnValue = "Integer Prefs";
				break;
			case Serializer.SerializationTypes.String:
				returnValue = "String Prefs";
				break;
			case Serializer.SerializationTypes.Float:
				returnValue = "Float Prefs";
				break;
			case Serializer.SerializationTypes.Long:
				returnValue = "Long Prefs";
				break;
			case Serializer.SerializationTypes.Double:
				returnValue = "Double Prefs";
				break;
			case Serializer.SerializationTypes.Bool:
				returnValue = "Bool Prefs";
				break;
			case Serializer.SerializationTypes.Vector2:
				returnValue = "Vector2 Prefs";
				break;
			case Serializer.SerializationTypes.Vector3:
				returnValue = "Vector3 Prefs";
				break;
			case Serializer.SerializationTypes.Vector4:
				returnValue = "Vector4 Prefs";
				break;
			case Serializer.SerializationTypes.Quaternion:
				returnValue = "Quaternion Prefs";
				break;
			case Serializer.SerializationTypes.Color:
				returnValue = "Color Prefs";
				break;
			case Serializer.SerializationTypes.ArrayString:
				returnValue = "string[] Prefs";
				break;
			case Serializer.SerializationTypes.ArrayInt:
				returnValue = "int[] Prefs";
				break;
			case Serializer.SerializationTypes.ArrayFloat:
				returnValue = "float[] Prefs";
				break;
			case Serializer.SerializationTypes.ArrayDouble:
				returnValue = "double[] Prefs";
				break;
			case Serializer.SerializationTypes.DictS:
				returnValue = "String Dictionary Prefs";
				break;
			case Serializer.SerializationTypes.DictI:
				returnValue = "Integer Dictionary Prefs";
				break;
			case Serializer.SerializationTypes.DictL:
				returnValue = "Long Dictionary Prefs";
				break;
			case Serializer.SerializationTypes.DictD:
				returnValue = "Double Dictionary Prefs";
				break;
			case Serializer.SerializationTypes.DictF:
				returnValue = "Float Dictionary Prefs";
				break;
			case Serializer.SerializationTypes.DictB:
				returnValue = "Bool Dictionary Prefs";
				break;
			case Serializer.SerializationTypes.ListS:
				returnValue = "String List Prefs";
				break;
			case Serializer.SerializationTypes.ListI:
				returnValue = "Integer List Prefs";
				break;
			case Serializer.SerializationTypes.ListB:
				returnValue = "Bool List Prefs";
				break;
			case Serializer.SerializationTypes.ListL:
				returnValue = "Long List Prefs";
				break;
			case Serializer.SerializationTypes.ListD:
				returnValue = "Double List Prefs";
				break;
			case Serializer.SerializationTypes.ListF:
				returnValue = "Float List Prefs";
				break;
			default:
				returnValue = "";
				break;
			}
			return returnValue;
		}
		#endregion
		private void InitMenu(){
			addMenu = new GenericMenu ();
			foreach (Serializer.SerializationTypes type in Enum.GetValues(typeof(Serializer.SerializationTypes))) {
				var typeString = SerializationTypeToString (type);
				if (typeString != "") {
					typeString = typeString.Substring (0, typeString.IndexOf ("Prefs") - 1);
					if (typeString.Contains ("[]")) {
						typeString = typeString.Substring (0, typeString.IndexOf ("[]"));
						addMenu.AddItem (new GUIContent ("Arrays/" + typeString), false, AddPrefCallback, "array."+type);
					} else {
						if (typeString.Contains ("Dictionary")) {
							typeString = typeString.Substring (0, typeString.IndexOf ("Dictionary") - 1);
							addMenu.AddItem (new GUIContent ("Dictionaries/" + typeString), false, AddPrefCallback, "dict."+type);
						} else {
							if (typeString.Contains ("List")) {
								typeString = typeString.Substring (0, typeString.IndexOf ("List") - 1);
								addMenu.AddItem (new GUIContent ("Lists/" + typeString), false, AddPrefCallback, "list."+type);
							} else {
								if (typeString.Contains ("Vector")) {
									addMenu.AddItem (new GUIContent ("Vectors/" + typeString), false, AddPrefCallback, "vector."+type);
								} else {
									addMenu.AddItem (new GUIContent ("Simple Types/" + typeString), false, AddPrefCallback, "simple."+type);
								}
							}
						}
					}
				}
			}
		}
		void AddPrefCallback(object _type)
		{
			EditorPrefs.SetString("EpicPrefsAddType",_type.ToString());
			AddPrefWizard.CreateWizard ();
			Repaint ();
		}

	}
	#endif
}