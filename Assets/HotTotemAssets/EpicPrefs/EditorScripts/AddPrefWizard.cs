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
	public class AddPrefWizard : ScriptableWizard {
		private Serializer.SerializationTypes type;
		private string _name = "";
		private string specType = "";
		private List<bool> encrypted;
		private List<string> names, values;
		private Vector2 scrollPos = new Vector2(0,0);
		public static void CreateWizard () {
			ScriptableWizard.DisplayWizard<AddPrefWizard>("Add EpicPref");

		}
		void OnEnable()
		{
			var complicatedType = EditorPrefs.GetString ("EpicPrefsAddType", "String");
			var splitted = complicatedType.Split ('.');
			specType = splitted [0];
			type = (Serializer.SerializationTypes)Enum.Parse (typeof(Serializer.SerializationTypes), splitted[1]);
			names = new List<string> ();
			names.Add ("");
			values = new List<string> ();
			switch (type) {
			case Serializer.SerializationTypes.Vector2:
				values.Add (Operators.Vector2String(new Vector2 (0, 0)));
				break;
			case Serializer.SerializationTypes.Vector3:
				values.Add (Operators.Vector2String(new Vector3 (0, 0, 0)));
				break;
			case Serializer.SerializationTypes.Vector4:
				values.Add (Operators.Vector2String(new Vector4 (0, 0, 0, 0)));
				break;
			case Serializer.SerializationTypes.Quaternion:
				values.Add (Operators.Quaternion2String(new Quaternion (0, 0 ,0 ,0)));
				break;
			default:
				values.Add ("");
				break;
			}
			encrypted = new List<bool> ();
			encrypted.Add (false);
			minSize = new Vector2 (350,250);
		}
		void OnGUI()
		{
			scrollPos = GUILayout.BeginScrollView(scrollPos, false, false, GUIStyle.none, GUI.skin.verticalScrollbar);
			if(specType == "simple" && type != Serializer.SerializationTypes.Color && type != Serializer.SerializationTypes.Quaternion&& type != Serializer.SerializationTypes.Bool){
				RenderSimpleType();
			}
			if (type == Serializer.SerializationTypes.Bool) {
				RenderBool ();
			}
			if (specType == "dict" && type != Serializer.SerializationTypes.DictB) {
				RenderDict ();
			}
			if (type == Serializer.SerializationTypes.DictB) {
				RenderBoolDict ();
			}
			if ((specType == "array" || specType == "list") && type != Serializer.SerializationTypes.ListB) {
				RenderArrayAndList ();
			}
			if (type == Serializer.SerializationTypes.Vector2 || type == Serializer.SerializationTypes.Vector3 || type == Serializer.SerializationTypes.Vector4 || type == Serializer.SerializationTypes.Quaternion) {
				RenderVector (type);
			}
			if (type == Serializer.SerializationTypes.ListB) {
				RenderBoolList ();
			}
			if (type == Serializer.SerializationTypes.Color) {
				RenderColor ();
			}
			GUILayout.EndScrollView ();
			EditorGUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Save", GUILayout.ExpandWidth (false))) {
				SavePrefs ();
			}
			if (GUILayout.Button ("Cancel", GUILayout.ExpandWidth (false))) {
				Close ();
			}
			EditorGUILayout.EndHorizontal ();
		}
		void RenderVector(Serializer.SerializationTypes type){
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.LabelField ("Name", "Value");
			EditorGUILayout.EndHorizontal ();
			for (int i = 0; i < names.Count; i++) {
				EditorGUILayout.BeginHorizontal ();
				GUILayout.Space (10);
				names [i] = EditorGUILayout.TextField (names [i]);
				var components = new List<string> ();
				switch (type) {
				case Serializer.SerializationTypes.Vector2:
					components = (values[i]).Substring (1, (values[i]).Length - 2).Split (';').ToList();
					values [i] = Operators.Vector2String((Operators.ToVector2 (EditorGUILayout.TextField (components [0]), 
						EditorGUILayout.TextField (components [1]))));
					break;
				case Serializer.SerializationTypes.Vector3:
					components = (values[i]).Substring (1, (values[i]).Length - 2).Split (';').ToList();
					values [i] = Operators.Vector2String((Operators.ToVector3 (EditorGUILayout.TextField (components [0]), 
						EditorGUILayout.TextField (components [1]),
						EditorGUILayout.TextField (components [2]))));
					break;
				case Serializer.SerializationTypes.Vector4:
					components = (values[i]).Substring (1, (values[i]).Length - 2).Split (';').ToList();
					values [i] = Operators.Vector2String((Operators.ToVector4 (EditorGUILayout.TextField (components [0]), 
						EditorGUILayout.TextField (components [1]),
						EditorGUILayout.TextField (components [2]),
						EditorGUILayout.TextField (components [3]))));
					break;
				case Serializer.SerializationTypes.Quaternion:
					components = (values[i]).Substring (1, (values[i]).Length - 2).Split (';').ToList();
					values [i] = Operators.Quaternion2String((Operators.ToQuaternion (EditorGUILayout.TextField (components [0]), 
						EditorGUILayout.TextField (components [1]),
						EditorGUILayout.TextField (components [2]),
						EditorGUILayout.TextField (components [3]))));
					break;
				default:
					break;
				}
				EditorGUILayout.EndHorizontal ();
			}
			if (names.Last () != "" && values.Last () != "") {
				names.Add ("");
				switch (type) {
				case Serializer.SerializationTypes.Vector2:
					values.Add (Operators.Vector2String(new Vector2 (0, 0)));
					break;
				case Serializer.SerializationTypes.Vector3:
					values.Add (Operators.Vector2String(new Vector3 (0, 0, 0)));
					break;
				case Serializer.SerializationTypes.Vector4:
					values.Add (Operators.Vector2String(new Vector4 (0, 0, 0, 0)));
					break;
				case Serializer.SerializationTypes.Quaternion:
					values.Add (Operators.Quaternion2String(new Quaternion (0, 0 ,0 ,0)));
					break;
				default:
					break;
				}
			}
		}
		void RenderSimpleType(){
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.LabelField ("Name", "Value");
			EditorGUILayout.EndHorizontal ();
			for (int i = 0; i < names.Count; i++) {
				EditorGUILayout.BeginHorizontal ();
				GUILayout.Space (10);
				names [i] = EditorGUILayout.TextField (names [i]);
				values [i] = EditorGUILayout.TextField (values [i]);
				encrypted [i] = EditorGUILayout.Toggle ("Encrypted",encrypted [i]);
				EditorGUILayout.EndHorizontal ();
			}
			if (names.Last () != "" && values.Last () != "") {
				names.Add ("");
				values.Add ("");
				encrypted.Add (false);
			}
		}
		void RenderDict(){
			EditorGUILayout.BeginHorizontal ();
			_name = EditorGUILayout.TextField ("Name : " , _name, GUILayout.ExpandWidth (false));
			encrypted [0] = EditorGUILayout.Toggle ("Encrypted",encrypted [0], GUILayout.ExpandWidth (false));
			EditorGUILayout.EndHorizontal ();
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.LabelField ("Key", "Value");
			EditorGUILayout.EndHorizontal ();
			for (int i = 0; i < names.Count; i++) {
				EditorGUILayout.BeginHorizontal ();
				GUILayout.Space (10);
				names [i] = EditorGUILayout.TextField (names [i]);
				values [i] = EditorGUILayout.TextField (values [i]);
				EditorGUILayout.EndHorizontal ();
			}
			if (names.Last () != "" && values.Last () != "") {
				names.Add ("");
				values.Add ("");
			}
		}
		void RenderArrayAndList(){
			EditorGUILayout.BeginHorizontal ();
			_name = EditorGUILayout.TextField ("Name : " , _name, GUILayout.ExpandWidth (false));
			encrypted [0] = EditorGUILayout.Toggle ("Encrypted",encrypted [0], GUILayout.ExpandWidth (false));
			EditorGUILayout.EndHorizontal ();
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.LabelField ("Value");
			EditorGUILayout.EndHorizontal ();
			for (int i = 0; i < values.Count; i++) {
				EditorGUILayout.BeginHorizontal ();
				GUILayout.Space (10);
				values [i] = EditorGUILayout.TextField (values [i]);
				EditorGUILayout.EndHorizontal ();
			}
			if (values.Last() != "") {
				values.Add ("");
			}
		}
		void RenderBoolList(){
			EditorGUILayout.BeginHorizontal ();
			_name = EditorGUILayout.TextField ("Name : " , _name, GUILayout.ExpandWidth (false));
			encrypted [0] = EditorGUILayout.Toggle ("Encrypted",encrypted [0], GUILayout.ExpandWidth (false));
			EditorGUILayout.EndHorizontal ();
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.LabelField ("Value");
			EditorGUILayout.EndHorizontal ();
			for (int i = 0; i < values.Count; i++) {
				EditorGUILayout.BeginHorizontal ();
				GUILayout.Space (10);
				values [i] = EditorGUILayout.Toggle (Operators.ToBool(values [i])).ToString();
				EditorGUILayout.EndHorizontal ();
			}
			if (GUILayout.Button("Add",GUILayout.ExpandWidth(false))) {
				values.Add ("");
			}
		}
		void RenderBool(){
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.LabelField ("Name", "Value");
			EditorGUILayout.EndHorizontal ();
			for (int i = 0; i < names.Count; i++) {
				EditorGUILayout.BeginHorizontal ();
				GUILayout.Space (10);
				names [i] = EditorGUILayout.TextField (names [i]);
				values [i] = EditorGUILayout.Toggle (Operators.ToBool(values [i])).ToString();
				encrypted [i] = EditorGUILayout.Toggle ("Encrypted",encrypted [i]);
				EditorGUILayout.EndHorizontal ();
			}
			if (names.Last () != "" && values.Last () != "") {
				names.Add ("");
				values.Add ("");
				encrypted.Add (false);
			}
		}
		void RenderColor(){
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.LabelField ("Name", "Value");
			EditorGUILayout.EndHorizontal ();
			for (int i = 0; i < names.Count; i++) {
				EditorGUILayout.BeginHorizontal ();
				GUILayout.Space (10);
				names [i] = EditorGUILayout.TextField (names [i]);
				values [i] = Operators.ColorToString(EditorGUILayout.ColorField (Operators.StringToColor(values [i])));
				EditorGUILayout.EndHorizontal ();
			}
			if (names.Last () != "") {
				names.Add ("");
				values.Add ("");
			}
		}
		void RenderBoolDict(){
			EditorGUILayout.BeginHorizontal ();
			_name = EditorGUILayout.TextField ("Name : " , _name);
			encrypted [0] = EditorGUILayout.Toggle ("Encrypted",encrypted [0], GUILayout.ExpandWidth (false));
			EditorGUILayout.EndHorizontal ();
			EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.LabelField ("Key", "Value");
			EditorGUILayout.EndHorizontal ();
			for (int i = 0; i < names.Count; i++) {
				EditorGUILayout.BeginHorizontal ();
				GUILayout.Space (10);
				names [i] = EditorGUILayout.TextField (names [i]);
				values [i] = EditorGUILayout.Toggle (Operators.ToBool(values [i])).ToString();
				EditorGUILayout.EndHorizontal ();
			}
			if (names.Last () != "" && values.Last () != "") {
				names.Add ("");
				values.Add ("");
			}
		}
		void SavePrefs(){
			switch (type) {
			case Serializer.SerializationTypes.Integer:
				for (int i = 0; i < names.Count; i++) {
					if(names[i] != "")
						EpicPrefs.SetInt (names [i], Operators.ToInt(values [i]), encrypted [i]);
				}
				break;
			case Serializer.SerializationTypes.String:
				for (int i = 0; i < names.Count; i++) {
					if(names[i] != "")
						EpicPrefs.SetString (names [i], values [i], encrypted [i]);
				}
				break;
			case Serializer.SerializationTypes.Float:
				for (int i = 0; i < names.Count; i++) {
					if(names[i] != "")
						EpicPrefs.SetFloat (names [i], Operators.ToFloat(values [i]), encrypted [i]);
				}
				break;
			case Serializer.SerializationTypes.Long:
				for (int i = 0; i < names.Count; i++) {
					if(names[i] != "")
						EpicPrefs.SetLong (names [i], Operators.ToLong(values [i]), encrypted [i]);
				}
				break;
			case Serializer.SerializationTypes.Double:
				for (int i = 0; i < names.Count; i++) {
					if(names[i] != "")
						EpicPrefs.SetDouble (names [i], Operators.ToDouble(values [i]), encrypted [i]);
				}
				break;
			case Serializer.SerializationTypes.Bool:
				for (int i = 0; i < names.Count; i++) {
					if(names[i] != "")
						EpicPrefs.SetBool (names [i], Operators.ToBool(values [i]), encrypted [i]);
				}
				break;
			case Serializer.SerializationTypes.Vector2:
				for (int i = 0; i < names.Count; i++) {
					if(names[i] != "")
						EpicPrefs.SetVector2 (names [i], Operators.ToVector2(values [i]),false);
				}
				break;
			case Serializer.SerializationTypes.Vector3:
				for (int i = 0; i < names.Count; i++) {
					if(names[i] != "")
						EpicPrefs.SetVector3 (names [i], Operators.ToVector3(values [i]),false);
				}
				break;
			case Serializer.SerializationTypes.Vector4:
				for (int i = 0; i < names.Count; i++) {
					if(names[i] != "")
						EpicPrefs.SetVector4 (names [i], Operators.ToVector4(values [i]),false);
				}
				break;
			case Serializer.SerializationTypes.Quaternion:
				for (int i = 0; i < names.Count; i++) {
					if(names[i] != "")
						EpicPrefs.SetQuaternion (names [i], Operators.ToQuaternion(values [i]),false);
				}
				break;
			case Serializer.SerializationTypes.Color:
				for (int i = 0; i < names.Count; i++) {
					if(names[i] != "")
						EpicPrefs.SetColor (names [i], Operators.StringToColor(values [i]),false);
				}
				break;
			case Serializer.SerializationTypes.ArrayString:
				var sA = new List<string>();
				for (int i = 0; i < values.Count; i++) {
					if (values [i] != "")
						sA.Add (values [i]);
				}
				EpicPrefs.SetArray (_name,sA.ToArray(),encrypted [0]);
				break;
			case Serializer.SerializationTypes.ArrayInt:
				var iA = new List<int>();
				for (int i = 0; i < values.Count; i++) {
					if (values [i] != "")
						iA.Add (Operators.ToInt(values [i]));
				}
				EpicPrefs.SetArray (_name,iA.ToArray(),encrypted [0]);
				break;
			case Serializer.SerializationTypes.ArrayFloat:
				var fA = new List<float>();
				for (int i = 0; i < values.Count; i++) {
					if (values [i] != "")
						fA.Add (Operators.ToFloat(values [i]));
				}
				EpicPrefs.SetArray (_name,fA.ToArray(),encrypted [0]);
				break;
			case Serializer.SerializationTypes.ArrayDouble:
				var dA = new List<double>();
				for (int i = 0; i < values.Count; i++) {
					if (values [i] != "")
						dA.Add (Operators.ToDouble(values [i]));
				}
				EpicPrefs.SetArray (_name,dA.ToArray(),encrypted [0]);
				break;
			case Serializer.SerializationTypes.DictS:
				var stringDict = new Dictionary<string,string> ();
				for (int i = 0; i < names.Count; i++) {
					if (names [i] != "")
						stringDict [names [i]] = values [i];
				}
				EpicPrefs.SetDict (_name, stringDict, encrypted [0]);
				break;
			case Serializer.SerializationTypes.DictI:
				var intDict = new Dictionary<string,int> ();
				for (int i = 0; i < names.Count; i++) {
					if (names [i] != "")
						intDict [names [i]] = Operators.ToInt(values [i]);
				}
				EpicPrefs.SetDict (_name, intDict, encrypted [0]);
				break;
			case Serializer.SerializationTypes.DictL:
				var longDict = new Dictionary<string,long> ();
				for (int i = 0; i < names.Count; i++) {
					if (names [i] != "")
						longDict [names [i]] = Operators.ToLong(values [i]);
				}
				EpicPrefs.SetDict (_name, longDict, encrypted [0]);
				break;
			case Serializer.SerializationTypes.DictD:
				var doubleDict = new Dictionary<string,double> ();
				for (int i = 0; i < names.Count; i++) {
					if (names [i] != "")
						doubleDict [names [i]] = Operators.ToDouble(values [i]);
				}
				EpicPrefs.SetDict (_name, doubleDict, encrypted [0]);
				break;
			case Serializer.SerializationTypes.DictF:
				var floatDict = new Dictionary<string,float> ();
				for (int i = 0; i < names.Count; i++) {
					if (names [i] != "")
						floatDict [names [i]] = Operators.ToFloat(values [i]);
				}
				EpicPrefs.SetDict (_name, floatDict, encrypted [0]);
				break;
			case Serializer.SerializationTypes.DictB:
				var boolDict = new Dictionary<string,bool> ();
				for (int i = 0; i < names.Count; i++) {
					if (names [i] != "")
						boolDict [names [i]] = Operators.ToBool(values [i]);
				}
				EpicPrefs.SetDict (_name, boolDict, encrypted [0]);
				break;
			case Serializer.SerializationTypes.ListS:
				sA = new List<string> ();
				for (int i = 0; i < values.Count; i++) {
					if (values [i] != "")
						sA.Add (values [i]);
				}
				EpicPrefs.SetList (_name, sA, encrypted [0]);
				break;
			case Serializer.SerializationTypes.ListI:
				iA = new List<int> ();
				for (int i = 0; i < values.Count; i++) {
					if (values [i] != "")
						iA.Add (Operators.ToInt(values [i]));
				}
				EpicPrefs.SetList (_name, iA, encrypted [0]);
				break;
			case Serializer.SerializationTypes.ListB:
				var bA = new List<bool> ();
				for (int i = 0; i < values.Count; i++) {
					if (values [i] != "")
						bA.Add (Operators.ToBool(values [i]));
				}
				EpicPrefs.SetList (_name, bA, encrypted [0]);
				break;
			case Serializer.SerializationTypes.ListL:
				var lA = new List<long> ();
				for (int i = 0; i < values.Count; i++) {
					if (values [i] != "")
						lA.Add (Operators.ToLong(values [i]));
				}
				EpicPrefs.SetList (_name, lA, encrypted [0]);
				break;
			case Serializer.SerializationTypes.ListD:
				dA = new List<double> ();
				for (int i = 0; i < values.Count; i++) {
					if (values [i] != "")
						dA.Add (Operators.ToDouble(values [i]));
				}
				EpicPrefs.SetList (_name, dA, encrypted [0]);
				break;
			case Serializer.SerializationTypes.ListF:
				fA = new List<float> ();
				for (int i = 0; i < values.Count; i++) {
					if (values [i] != "")
						fA.Add (Operators.ToFloat(values [i]));
				}
				EpicPrefs.SetList (_name, fA, encrypted [0]);
				break;
			default:
				break;
			}
			Close ();
		}
	}
	#endif
}