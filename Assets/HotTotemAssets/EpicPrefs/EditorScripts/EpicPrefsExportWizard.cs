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

namespace EpicPrefsEditorTools
{
	#if UNITY_EDITOR
	public class EpicPrefsExportWizard : ScriptableWizard
	{
		EpicPrefsContainer encryptedContainer,notEncryptedContainer;
		Vector2 scrollPos = new Vector2(0,0);
		Dictionary<string,bool> exportPref;
		Dictionary<string,bool> foldouts;
		static bool initialized = false;
		public EpicPrefsExportWizard ()
		{
			
		}
		~EpicPrefsExportWizard(){
			initialized = false;
		}
		void Init(){
			if (!initialized) {
				initialized = true;
				exportPref = new Dictionary<string, bool> ();
				foldouts = new Dictionary<string, bool> ();
				foldouts ["NEstring"] = false;
				foldouts ["Estring"] = false;
				foldouts ["NEint"] = false;
				foldouts ["Eint"] = false;
				foldouts ["NEfloat"] = false;
				foldouts ["Efloat"] = false;
				foldouts ["NEdouble"] = false;
				foldouts ["Edouble"] = false;
				foldouts ["NElong"] = false;
				foldouts ["Elong"] = false;
				foldouts ["NEbool"] = false;
				foldouts ["Ebool"] = false;
				foldouts ["NEstringdict"] = false;
				foldouts ["Estringdict"] = false;
				foldouts ["NEintdict"] = false;
				foldouts ["Eintdict"] = false;
				foldouts ["NEfloatdict"] = false;
				foldouts ["Efloatdict"] = false;
				foldouts ["NEdoubledict"] = false;
				foldouts ["Edoubledict"] = false;
				foldouts ["NElongdict"] = false;
				foldouts ["Elongdict"] = false;
				foldouts ["NEbooldict"] = false;
				foldouts ["Ebooldict"] = false;
				foldouts ["NEstringlist"] = false;
				foldouts ["Estringlist"] = false;
				foldouts ["NEintlist"] = false;
				foldouts ["Eintlist"] = false;
				foldouts ["NEfloatlist"] = false;
				foldouts ["Efloatlist"] = false;
				foldouts ["NEdoublelist"] = false;
				foldouts ["Edoublelist"] = false;
				foldouts ["NElonglist"] = false;
				foldouts ["Elonglist"] = false;
				foldouts ["NEboollist"] = false;
				foldouts ["Eboollist"] = false;
				foldouts ["NEstringarray"] = false;
				foldouts ["Estringarray"] = false;
				foldouts ["NEintarray"] = false;
				foldouts ["Eintarray"] = false;
				foldouts ["NEfloatarray"] = false;
				foldouts ["Efloatarray"] = false;
				foldouts ["NEdoublearray"] = false;
				foldouts ["Edoublearray"] = false;
				foldouts ["NEcolor"] = false;
				foldouts ["Ecolor"] = false;
				foldouts ["NEquaternion"] = false;
				foldouts ["Equaternion"] = false;
				foldouts ["NEvector2"] = false;
				foldouts ["Evector2"] = false;
				foldouts ["NEvector3"] = false;
				foldouts ["Evector3"] = false;
				foldouts ["NEvector4"] = false;
				foldouts ["Evector4"] = false;
				encryptedContainer = Serializer.GetContainer (true);
				notEncryptedContainer = Serializer.GetContainer (false);
				Operators.CopySettings ();
				Operators.CopyPrefs (this, encryptedContainer, true, true);
				Operators.CopyPrefs (this, notEncryptedContainer, false, true);
			}
		}
		public static void CreateWizard () 
		{
			ScriptableWizard.DisplayWizard<EpicPrefsExportWizard>("EpicPrefs Export Wizard");
		}
		void OnGUI()
		{
			Init ();
			scrollPos = GUILayout.BeginScrollView(scrollPos, false, false, GUIStyle.none, GUI.skin.verticalScrollbar);
			GUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Select All", GUILayout.ExpandWidth (true))) {
				var newExport = new Dictionary<string,bool> ();
				foreach (var key in exportPref.Keys) {
					newExport [key] = true;
				}
				exportPref = newExport;
				Repaint ();
			}
			if (GUILayout.Button ("Deselect All", GUILayout.ExpandWidth (true))) {
				var newExport = new Dictionary<string,bool> ();
				foreach (var key in exportPref.Keys) {
					newExport [key] = false;
				}
				exportPref = newExport;
				Repaint ();
			}
			if (GUILayout.Button ("Collapse All", GUILayout.ExpandWidth (true))) {
				var newFoldout = new Dictionary<string,bool> ();
				foreach (var key in foldouts.Keys) {
					newFoldout [key] = false;
				}
				foldouts = newFoldout;
				Repaint ();
			}
			if (GUILayout.Button ("Unfold All", GUILayout.ExpandWidth (true))) {
				var newFoldout = new Dictionary<string,bool> ();
				foreach (var key in foldouts.Keys) {
					newFoldout [key] = true;
				}
				foldouts = newFoldout;
				Repaint ();
			}
			GUILayout.EndHorizontal ();
			if(notEncryptedContainer.StringDictionary.Count>0)
				foldouts["NEstring"] = EditorGUILayout.Foldout(foldouts["NEstring"],"Strings - Not Encrypted");
			if (foldouts ["NEstring"]) {
				foreach (var key in notEncryptedContainer.StringDictionary.Keys) {
					if (exportPref.ContainsKey ("NEstring_" + key)) {
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Space (20);
						exportPref ["NEstring_" + key] = GUILayout.Toggle (exportPref ["NEstring_" + key], key);
						GUILayout.EndHorizontal ();
					}
					else
					{
						exportPref["NEstring_" + key] = false;
					}

				}
			}
			if(encryptedContainer.StringDictionary.Count>0)
				foldouts["Estring"] = EditorGUILayout.Foldout(foldouts["Estring"],"Strings - Encrypted");
			if (foldouts ["Estring"]) {
				foreach (var key in encryptedContainer.StringDictionary.Keys) {
					if (exportPref.ContainsKey ("Estring_" + key)) {
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Space (20);
						exportPref ["Estring_" + key] = GUILayout.Toggle (exportPref ["Estring_" + key], key);
						GUILayout.EndHorizontal ();
					}
					else
					{
						exportPref["Estring_" + key] = false;
					}

				}
			}
			if(notEncryptedContainer.IntegerDictionary.Count>0)
				foldouts["NEint"] = EditorGUILayout.Foldout(foldouts["NEint"],"Integer - Not Encrypted");
			if (foldouts ["NEint"]) {
				foreach (var key in notEncryptedContainer.IntegerDictionary.Keys) {
					if (exportPref.ContainsKey ("NEint_" + key)) {
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Space (20);
						exportPref ["NEint_" + key] = GUILayout.Toggle (exportPref ["NEint_" + key], key);
						GUILayout.EndHorizontal ();
					}
					else
					{
						exportPref["NEint_" + key] = false;
					}

				}
			}
			if(encryptedContainer.IntegerDictionary.Count>0)
				foldouts["Eint"] = EditorGUILayout.Foldout(foldouts["Eint"],"Integer - Encrypted");
			if (foldouts ["Eint"]) {
				foreach (var key in encryptedContainer.IntegerDictionary.Keys) {
					if (exportPref.ContainsKey ("Eint_" + key)) {
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Space (20);
						exportPref ["Eint_" + key] = GUILayout.Toggle (exportPref ["Eint_" + key], key);
						GUILayout.EndHorizontal ();
					}
					else
					{
						exportPref["Eint_" + key] = false;
					}

				}
			}
			if(notEncryptedContainer.FloatDictionary.Count>0)
				foldouts["NEfloat"] = EditorGUILayout.Foldout(foldouts["NEfloat"],"Float - Not Encrypted");
			if (foldouts ["NEfloat"]) {
				foreach (var key in notEncryptedContainer.FloatDictionary.Keys) {
					if (exportPref.ContainsKey ("NEfloat_" + key)) {
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Space (20);
						exportPref ["NEfloat_" + key] = GUILayout.Toggle (exportPref ["NEfloat_" + key], key);
						GUILayout.EndHorizontal ();
					}
					else
					{
						exportPref["NEfloat_" + key] = false;
					}

				}
			}
			if(encryptedContainer.FloatDictionary.Count>0)
				foldouts["Efloat"] = EditorGUILayout.Foldout(foldouts["Efloat"],"Float - Encrypted");
			if (foldouts ["Efloat"]) {
				foreach (var key in encryptedContainer.FloatDictionary.Keys) {
					if (exportPref.ContainsKey ("Efloat_" + key)) {
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Space (20);
						exportPref ["Efloat_" + key] = GUILayout.Toggle (exportPref ["Efloat_" + key], key);
						GUILayout.EndHorizontal ();
					}
					else
					{
						exportPref["Efloat_" + key] = false;
					}

				}
			}
			if(notEncryptedContainer.LongDictionary.Count>0)
				foldouts["NElong"] = EditorGUILayout.Foldout(foldouts["NElong"],"Long - Not Encrypted");
			if (foldouts ["NElong"]) {
				foreach (var key in notEncryptedContainer.LongDictionary.Keys) {
					if (exportPref.ContainsKey ("NElong_" + key)) {
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Space (20);
						exportPref ["NElong_" + key] = GUILayout.Toggle (exportPref ["NElong_" + key], key);
						GUILayout.EndHorizontal ();
					}
					else
					{
						exportPref["NElong_" + key] = false;
					}

				}
			}
			if(encryptedContainer.LongDictionary.Count>0)
				foldouts["Elong"] = EditorGUILayout.Foldout(foldouts["Elong"],"Long - Encrypted");
			if (foldouts ["Elong"]) {
				foreach (var key in encryptedContainer.LongDictionary.Keys) {
					if (exportPref.ContainsKey ("Elong_" + key)) {
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Space (20);
						exportPref ["Elong_" + key] = GUILayout.Toggle (exportPref ["Elong_" + key], key);
						GUILayout.EndHorizontal ();
					}
					else
					{
						exportPref["Elong_" + key] = false;
					}

				}
			}
			if(notEncryptedContainer.DoubleDictionary.Count>0)
				foldouts["NEdouble"] = EditorGUILayout.Foldout(foldouts["NEdouble"],"Double - Not Encrypted");
			if (foldouts ["NEdouble"]) {
				foreach (var key in notEncryptedContainer.DoubleDictionary.Keys) {
					if (exportPref.ContainsKey ("NEdouble_" + key)) {
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Space (20);
						exportPref ["NEdouble_" + key] = GUILayout.Toggle (exportPref ["NEdouble_" + key], key);
						GUILayout.EndHorizontal ();
					}
					else
					{
						exportPref["NEdouble_" + key] = false;
					}

				}
			}
			if(encryptedContainer.DoubleDictionary.Count>0)
				foldouts["Edouble"] = EditorGUILayout.Foldout(foldouts["Edouble"],"Double - Encrypted");
			if (foldouts ["Edouble"]) {
				foreach (var key in encryptedContainer.DoubleDictionary.Keys) {
					if (exportPref.ContainsKey ("Edouble_" + key)) {
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Space (20);
						exportPref ["Edouble_" + key] = GUILayout.Toggle (exportPref ["Edouble_" + key], key);
						GUILayout.EndHorizontal ();
					}
					else
					{
						exportPref["Edouble_" + key] = false;
					}

				}
			}
			if(notEncryptedContainer.BooleanDictionary.Count>0)
				foldouts["NEbool"] = EditorGUILayout.Foldout(foldouts["NEbool"],"Boolean - Not Encrypted");
			if (foldouts ["NEbool"]) {
				foreach (var key in notEncryptedContainer.BooleanDictionary.Keys) {
					if (exportPref.ContainsKey ("NEbool_" + key)) {
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Space (20);
						exportPref ["NEbool_" + key] = GUILayout.Toggle (exportPref ["NEbool_" + key], key);
						GUILayout.EndHorizontal ();
					}
					else
					{
						exportPref["NEbool_" + key] = false;
					}

				}
			}
			if(encryptedContainer.BooleanDictionary.Count>0)
				foldouts["Ebool"] = EditorGUILayout.Foldout(foldouts["Ebool"],"Boolean - Encrypted");
			if (foldouts ["Ebool"]) {
				foreach (var key in encryptedContainer.BooleanDictionary.Keys) {
					if (exportPref.ContainsKey ("Ebool_" + key)) {
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Space (20);
						exportPref ["Ebool_" + key] = GUILayout.Toggle (exportPref ["Ebool_" + key], key);
						GUILayout.EndHorizontal ();
					}
					else
					{
						exportPref["Ebool_" + key] = false;
					}

				}
			}
			if(notEncryptedContainer.StringDictionaryDictionary.Count>0)
				foldouts["NEstringdict"] = EditorGUILayout.Foldout(foldouts["NEstringdict"],"String Dictionaries - Not Encrypted");
			if (foldouts ["NEstringdict"]) {
				foreach (var key in notEncryptedContainer.StringDictionaryDictionary.Keys) {
					if (exportPref.ContainsKey ("NEstringdict_" + key)) {
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Space (20);
						exportPref ["NEstringdict_" + key] = GUILayout.Toggle (exportPref ["NEstringdict_" + key], key);
						GUILayout.EndHorizontal ();
					}
					else
					{
						exportPref["NEstringdict_" + key] = false;
					}

				}
			}
			if(encryptedContainer.StringDictionaryDictionary.Count>0)
				foldouts["Estringdict"] = EditorGUILayout.Foldout(foldouts["Estringdict"],"Strings Dictionaries - Encrypted");
			if (foldouts ["Estringdict"]) {
				foreach (var key in encryptedContainer.StringDictionaryDictionary.Keys) {
					if (exportPref.ContainsKey ("Estringdict_" + key)) {
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Space (20);
						exportPref ["Estringdict_" + key] = GUILayout.Toggle (exportPref ["Estringdict_" + key], key);
						GUILayout.EndHorizontal ();
					}
					else
					{
						exportPref["Estringdict_" + key] = false;
					}

				}
			}
			if(notEncryptedContainer.IntegerDictionaryDictionary.Count>0)
				foldouts["NEintdict"] = EditorGUILayout.Foldout(foldouts["NEintdict"],"Integer Dictionaries - Not Encrypted");
			if (foldouts ["NEintdict"]) {
				foreach (var key in notEncryptedContainer.IntegerDictionaryDictionary.Keys) {
					if (exportPref.ContainsKey ("NEintdict_" + key)) {
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Space (20);
						exportPref ["NEintdict_" + key] = GUILayout.Toggle (exportPref ["NEintdict_" + key], key);
						GUILayout.EndHorizontal ();
					}
					else
					{
						exportPref["NEintdict_" + key] = false;
					}

				}
			}
			if(encryptedContainer.IntegerDictionaryDictionary.Count>0)
				foldouts["Eintdict"] = EditorGUILayout.Foldout(foldouts["Eintdict"],"Integer Dictionaries - Encrypted");
			if (foldouts ["Eintdict"]) {
				foreach (var key in encryptedContainer.IntegerDictionaryDictionary.Keys) {
					if (exportPref.ContainsKey ("Eintdict_" + key)) {
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Space (20);
						exportPref ["Eintdict_" + key] = GUILayout.Toggle (exportPref ["Eintdict_" + key], key);
						GUILayout.EndHorizontal ();
					}
					else
					{
						exportPref["Eintdict_" + key] = false;
					}

				}
			}
			if(notEncryptedContainer.FloatDictionaryDictionary.Count>0)
				foldouts["NEfloatdict"] = EditorGUILayout.Foldout(foldouts["NEfloatdict"],"Float Dictionaries - Not Encrypted");
			if (foldouts ["NEfloatdict"]) {
				foreach (var key in notEncryptedContainer.FloatDictionaryDictionary.Keys) {
					if (exportPref.ContainsKey ("NEfloatdict_" + key)) {
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Space (20);
						exportPref ["NEfloatdict_" + key] = GUILayout.Toggle (exportPref ["NEfloatdict_" + key], key);
						GUILayout.EndHorizontal ();
					}
					else
					{
						exportPref["NEfloatdict_" + key] = false;
					}

				}
			}
			if(encryptedContainer.FloatDictionaryDictionary.Count>0)
				foldouts["Efloatdict"] = EditorGUILayout.Foldout(foldouts["Efloatdict"],"Float Dictionaries - Encrypted");
			if (foldouts ["Efloatdict"]) {
				foreach (var key in encryptedContainer.FloatDictionaryDictionary.Keys) {
					if (exportPref.ContainsKey ("Efloatdict_" + key)) {
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Space (20);
						exportPref ["Efloatdict_" + key] = GUILayout.Toggle (exportPref ["Efloatdict_" + key], key);
						GUILayout.EndHorizontal ();
					}
					else
					{
						exportPref["Efloatdict_" + key] = false;
					}

				}
			}
			if(notEncryptedContainer.LongDictionaryDictionary.Count>0)
				foldouts["NElongdict"] = EditorGUILayout.Foldout(foldouts["NElongdict"],"Long Dictionaries - Not Encrypted");
			if (foldouts ["NElongdict"]) {
				foreach (var key in notEncryptedContainer.LongDictionaryDictionary.Keys) {
					if (exportPref.ContainsKey ("NElongdict_" + key)) {
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Space (20);
						exportPref ["NElongdict_" + key] = GUILayout.Toggle (exportPref ["NElongdict_" + key], key);
						GUILayout.EndHorizontal ();
					}
					else
					{
						exportPref["NElongdict_" + key] = false;
					}

				}
			}
			if(encryptedContainer.LongDictionaryDictionary.Count>0)
				foldouts["Elongdict"] = EditorGUILayout.Foldout(foldouts["Elongdict"],"Long Dictionaries - Encrypted");
			if (foldouts ["Elongdict"]) {
				foreach (var key in encryptedContainer.LongDictionaryDictionary.Keys) {
					if (exportPref.ContainsKey ("Elongdict_" + key)) {
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Space (20);
						exportPref ["Elongdict_" + key] = GUILayout.Toggle (exportPref ["Elongdict_" + key], key);
						GUILayout.EndHorizontal ();
					}
					else
					{
						exportPref["Elongdict_" + key] = false;
					}

				}
			}
			if(notEncryptedContainer.DoubleDictionaryDictionary.Count>0)
				foldouts["NEdoubledict"] = EditorGUILayout.Foldout(foldouts["NEdoubledict"],"Double Dictionaries - Not Encrypted");
			if (foldouts ["NEdoubledict"]) {
				foreach (var key in notEncryptedContainer.DoubleDictionaryDictionary.Keys) {
					if (exportPref.ContainsKey ("NEdoubledict_" + key)) {
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Space (20);
						exportPref ["NEdoubledict_" + key] = GUILayout.Toggle (exportPref ["NEdoubledict_" + key], key);
						GUILayout.EndHorizontal ();
					}
					else
					{
						exportPref["NEdoubledict_" + key] = false;
					}

				}
			}
			if(encryptedContainer.DoubleDictionaryDictionary.Count>0)
				foldouts["Edoubledict"] = EditorGUILayout.Foldout(foldouts["Edoubledict"],"Double Dictionaries - Encrypted");
			if (foldouts ["Edoubledict"]) {
				foreach (var key in encryptedContainer.DoubleDictionaryDictionary.Keys) {
					if (exportPref.ContainsKey ("Edoubledict_" + key)) {
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Space (20);
						exportPref ["Edoubledict_" + key] = GUILayout.Toggle (exportPref ["Edoubledict_" + key], key);
						GUILayout.EndHorizontal ();
					}
					else
					{
						exportPref["Edoubledict_" + key] = false;
					}

				}
			}
			if(notEncryptedContainer.BooleanDictionaryDictionary.Count>0)
				foldouts["NEbooldict"] = EditorGUILayout.Foldout(foldouts["NEbooldict"],"Boolean Dictionaries - Not Encrypted");
			if (foldouts ["NEbooldict"]) {
				foreach (var key in notEncryptedContainer.BooleanDictionaryDictionary.Keys) {
					if (exportPref.ContainsKey ("NEbooldict_" + key)) {
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Space (20);
						exportPref ["NEbooldict_" + key] = GUILayout.Toggle (exportPref ["NEbooldict_" + key], key);
						GUILayout.EndHorizontal ();
					}
					else
					{
						exportPref["NEbooldict_" + key] = false;
					}

				}
			}
			if(encryptedContainer.BooleanDictionaryDictionary.Count>0)
				foldouts["Ebooldict"] = EditorGUILayout.Foldout(foldouts["Ebooldict"],"Boolean Dictionaries - Encrypted");
			if (foldouts ["Ebooldict"]) {
				foreach (var key in encryptedContainer.BooleanDictionaryDictionary.Keys) {
					if (exportPref.ContainsKey ("Ebooldict_" + key)) {
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Space (20);
						exportPref ["Ebooldict_" + key] = GUILayout.Toggle (exportPref ["Ebooldict_" + key], key);
						GUILayout.EndHorizontal ();
					}
					else
					{
						exportPref["Ebooldict_" + key] = false;
					}

				}
			}
			if(notEncryptedContainer.StringListDictionary.Count>0)
				foldouts["NEstringlist"] = EditorGUILayout.Foldout(foldouts["NEstringlist"],"String Lists - Not Encrypted");
			if (foldouts ["NEstringlist"]) {
				foreach (var key in notEncryptedContainer.StringListDictionary.Keys) {
					if (exportPref.ContainsKey ("NEstringlist_" + key)) {
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Space (20);
						exportPref ["NEstringlist_" + key] = GUILayout.Toggle (exportPref ["NEstringlist_" + key], key);
						GUILayout.EndHorizontal ();
					}
					else
					{
						exportPref["NEstringlist_" + key] = false;
					}

				}
			}
			if(encryptedContainer.StringListDictionary.Count>0)
				foldouts["Estringlist"] = EditorGUILayout.Foldout(foldouts["Estringlist"],"Strings Lists - Encrypted");
			if (foldouts ["Estringlist"]) {
				foreach (var key in encryptedContainer.StringListDictionary.Keys) {
					if (exportPref.ContainsKey ("Estringlist_" + key)) {
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Space (20);
						exportPref ["Estringlist_" + key] = GUILayout.Toggle (exportPref ["Estringlist_" + key], key);
						GUILayout.EndHorizontal ();
					}
					else
					{
						exportPref["Estringlist_" + key] = false;
					}

				}
			}
			if(notEncryptedContainer.IntegerListDictionary.Count>0)
				foldouts["NEintlist"] = EditorGUILayout.Foldout(foldouts["NEintlist"],"Integer Lists - Not Encrypted");
			if (foldouts ["NEintlist"]) {
				foreach (var key in notEncryptedContainer.IntegerListDictionary.Keys) {
					if (exportPref.ContainsKey ("NEintlist_" + key)) {
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Space (20);
						exportPref ["NEintlist_" + key] = GUILayout.Toggle (exportPref ["NEintlist_" + key], key);
						GUILayout.EndHorizontal ();
					}
					else
					{
						exportPref["NEintlist_" + key] = false;
					}

				}
			}
			if(notEncryptedContainer.IntegerListDictionary.Count>0)
				foldouts["Eintlist"] = EditorGUILayout.Foldout(foldouts["Eintlist"],"Integer Lists - Encrypted");
			if (foldouts ["Eintlist"]) {
				foreach (var key in encryptedContainer.IntegerListDictionary.Keys) {
					if (exportPref.ContainsKey ("Eintlist_" + key)) {
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Space (20);
						exportPref ["Eintlist_" + key] = GUILayout.Toggle (exportPref ["Eintlist_" + key], key);
						GUILayout.EndHorizontal ();
					}
					else
					{
						exportPref["Eintlist_" + key] = false;
					}

				}
			}
			if(notEncryptedContainer.FloatListDictionary.Count>0)
				foldouts["NEfloatlist"] = EditorGUILayout.Foldout(foldouts["NEfloatlist"],"Float Lists - Not Encrypted");
			if (foldouts ["NEfloatlist"]) {
				foreach (var key in notEncryptedContainer.FloatListDictionary.Keys) {
					if (exportPref.ContainsKey ("NEfloatlist_" + key)) {
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Space (20);
						exportPref ["NEfloatlist_" + key] = GUILayout.Toggle (exportPref ["NEfloatlist_" + key], key);
						GUILayout.EndHorizontal ();
					}
					else
					{
						exportPref["NEfloatlist_" + key] = false;
					}

				}
			}
			if(encryptedContainer.FloatListDictionary.Count>0)
				foldouts["Efloatlist"] = EditorGUILayout.Foldout(foldouts["Efloatlist"],"Float Lists - Encrypted");
			if (foldouts ["Efloatlist"]) {
				foreach (var key in encryptedContainer.FloatListDictionary.Keys) {
					if (exportPref.ContainsKey ("Efloatlist_" + key)) {
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Space (20);
						exportPref ["Efloatlist_" + key] = GUILayout.Toggle (exportPref ["Efloatlist_" + key], key);
						GUILayout.EndHorizontal ();
					}
					else
					{
						exportPref["Efloatlist_" + key] = false;
					}

				}
			}
			if(notEncryptedContainer.LongListDictionary.Count>0)
				foldouts["NElonglist"] = EditorGUILayout.Foldout(foldouts["NElonglist"],"Long Lists - Not Encrypted");
			if (foldouts ["NElonglist"]) {
				foreach (var key in notEncryptedContainer.LongListDictionary.Keys) {
					if (exportPref.ContainsKey ("NElonglist_" + key)) {
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Space (20);
						exportPref ["NElonglist_" + key] = GUILayout.Toggle (exportPref ["NElonglist_" + key], key);
						GUILayout.EndHorizontal ();
					}
					else
					{
						exportPref["NElonglist_" + key] = false;
					}

				}
			}
			if(encryptedContainer.LongListDictionary.Count>0)
				foldouts["Elonglist"] = EditorGUILayout.Foldout(foldouts["Elonglist"],"Long Lists - Encrypted");
			if (foldouts ["Elonglist"]) {
				foreach (var key in encryptedContainer.LongListDictionary.Keys) {
					if (exportPref.ContainsKey ("Elonglist_" + key)) {
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Space (20);
						exportPref ["Elonglist_" + key] = GUILayout.Toggle (exportPref ["Elonglist_" + key], key);
						GUILayout.EndHorizontal ();
					}
					else
					{
						exportPref["Elonglist_" + key] = false;
					}

				}
			}
			if(notEncryptedContainer.DoubleListDictionary.Count>0)
				foldouts["NEdoublelist"] = EditorGUILayout.Foldout(foldouts["NEdoublelist"],"Double Lists - Not Encrypted");
			if (foldouts ["NEdoublelist"]) {
				foreach (var key in notEncryptedContainer.DoubleListDictionary.Keys) {
					if (exportPref.ContainsKey ("NEdoublelist_" + key)) {
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Space (20);
						exportPref ["NEdoublelist_" + key] = GUILayout.Toggle (exportPref ["NEdoublelist_" + key], key);
						GUILayout.EndHorizontal ();
					}
					else
					{
						exportPref["NEdoublelist_" + key] = false;
					}

				}
			}
			if(encryptedContainer.DoubleListDictionary.Count>0)
				foldouts["Edoublelist"] = EditorGUILayout.Foldout(foldouts["Edoublelist"],"Double Lists - Encrypted");
			if (foldouts ["Edoublelist"]) {
				foreach (var key in encryptedContainer.DoubleListDictionary.Keys) {
					if (exportPref.ContainsKey ("Edoublelist_" + key)) {
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Space (20);
						exportPref ["Edoublelist_" + key] = GUILayout.Toggle (exportPref ["Edoublelist_" + key], key);
						GUILayout.EndHorizontal ();
					}
					else
					{
						exportPref["Edoublelist_" + key] = false;
					}

				}
			}
			if(notEncryptedContainer.BooleanListDictionary.Count>0)
				foldouts["NEboollist"] = EditorGUILayout.Foldout(foldouts["NEboollist"],"Boolean Lists - Not Encrypted");
			if (foldouts ["NEboollist"]) {
				foreach (var key in notEncryptedContainer.BooleanListDictionary.Keys) {
					if (exportPref.ContainsKey ("NEboollist_" + key)) {
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Space (20);
						exportPref ["NEboollist_" + key] = GUILayout.Toggle (exportPref ["NEboollist_" + key], key);
						GUILayout.EndHorizontal ();
					}
					else
					{
						exportPref["NEboollist_" + key] = false;
					}

				}
			}
			if(encryptedContainer.BooleanListDictionary.Count>0)
				foldouts["Eboollist"] = EditorGUILayout.Foldout(foldouts["Eboollist"],"Boolean Lists - Encrypted");
			if (foldouts ["Eboollist"]) {
				foreach (var key in encryptedContainer.BooleanListDictionary.Keys) {
					if (exportPref.ContainsKey ("Eboollist_" + key)) {
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Space (20);
						exportPref ["Eboollist_" + key] = GUILayout.Toggle (exportPref ["Eboollist_" + key], key);
						GUILayout.EndHorizontal ();
					}
					else
					{
						exportPref["Eboollist_" + key] = false;
					}

				}
			}
			if(notEncryptedContainer.StringArrayDictionary.Count>0)
				foldouts["NEstringarray"] = EditorGUILayout.Foldout(foldouts["NEstringarray"],"String[] - Not Encrypted");
			if (foldouts ["NEstringarray"]) {
				foreach (var key in notEncryptedContainer.StringArrayDictionary.Keys) {
					if (exportPref.ContainsKey ("NEstringarray_" + key)) {
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Space (20);
						exportPref ["NEstringarray_" + key] = GUILayout.Toggle (exportPref ["NEstringarray_" + key], key);
						GUILayout.EndHorizontal ();
					}
					else
					{
						exportPref["NEstringarray_" + key] = false;
					}

				}
			}
			if(encryptedContainer.StringArrayDictionary.Count>0)
				foldouts["Estringarray"] = EditorGUILayout.Foldout(foldouts["Estringarray"],"Strings[] - Encrypted");
			if (foldouts ["Estringarray"]) {
				foreach (var key in encryptedContainer.StringArrayDictionary.Keys) {
					if (exportPref.ContainsKey ("Estringarray_" + key)) {
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Space (20);
						exportPref ["Estringarray_" + key] = GUILayout.Toggle (exportPref ["Estringarray_" + key], key);
						GUILayout.EndHorizontal ();
					}
					else
					{
						exportPref["Estringarray_" + key] = false;
					}

				}
			}
			if(notEncryptedContainer.IntegerArrayDictionary.Count>0)
				foldouts["NEintarray"] = EditorGUILayout.Foldout(foldouts["NEintarray"],"Integer[] - Not Encrypted");
			if (foldouts ["NEintarray"]) {
				foreach (var key in notEncryptedContainer.IntegerArrayDictionary.Keys) {
					if (exportPref.ContainsKey ("NEintarray__" + key)) {
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Space (20);
						exportPref ["NEintarray__" + key] = GUILayout.Toggle (exportPref ["NEintarray__" + key], key);
						GUILayout.EndHorizontal ();
					}
					else
					{
						exportPref["NEintarray__" + key] = false;
					}

				}
			}
			if(encryptedContainer.IntegerArrayDictionary.Count>0)
				foldouts["Eintarray"] = EditorGUILayout.Foldout(foldouts["Eintarray"],"Integer[] - Encrypted");
			if (foldouts ["Eintarray"]) {
				foreach (var key in encryptedContainer.IntegerArrayDictionary.Keys) {
					if (exportPref.ContainsKey ("Eintarray__" + key)) {
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Space (20);
						exportPref ["Eintarray__" + key] = GUILayout.Toggle (exportPref ["Eintarray__" + key], key);
						GUILayout.EndHorizontal ();
					}
					else
					{
						exportPref["Eintarray__" + key] = false;
					}

				}
			}
			if(notEncryptedContainer.FloatArrayDictionary.Count>0)
				foldouts["NEfloatarray"] = EditorGUILayout.Foldout(foldouts["NEfloatarray"],"Float[] - Not Encrypted");
			if (foldouts ["NEfloatarray"]) {
				foreach (var key in notEncryptedContainer.FloatArrayDictionary.Keys) {
					if (exportPref.ContainsKey ("NEfloatarray_" + key)) {
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Space (20);
						exportPref ["NEfloatarray_" + key] = GUILayout.Toggle (exportPref ["NEfloatarray_" + key], key);
						GUILayout.EndHorizontal ();
					}
					else
					{
						exportPref["NEfloatarray_" + key] = false;
					}

				}
			}
			if(encryptedContainer.FloatArrayDictionary.Count>0)
				foldouts["Efloatarray"] = EditorGUILayout.Foldout(foldouts["Efloatarray"],"Float[] - Encrypted");
			if (foldouts ["Efloatarray"]) {
				foreach (var key in encryptedContainer.FloatArrayDictionary.Keys) {
					if (exportPref.ContainsKey ("Efloatarray_" + key)) {
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Space (20);
						exportPref ["Efloatarray_" + key] = GUILayout.Toggle (exportPref ["Efloatarray_" + key], key);
						GUILayout.EndHorizontal ();
					}
					else
					{
						exportPref["Efloatarray_" + key] = false;
					}

				}
			}
			if(notEncryptedContainer.DoubleArrayDictionary.Count>0)
				foldouts["NEdoublearray"] = EditorGUILayout.Foldout(foldouts["NEdoublearray"],"Double[] - Not Encrypted");
			if (foldouts ["NEdoublearray"]) {
				foreach (var key in notEncryptedContainer.DoubleArrayDictionary.Keys) {
					if (exportPref.ContainsKey ("NEdoublearray_" + key)) {
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Space (20);
						exportPref ["NEdoublearray_" + key] = GUILayout.Toggle (exportPref ["NEdoublearray_" + key], key);
						GUILayout.EndHorizontal ();
					}
					else
					{
						exportPref["NEdoublearray_" + key] = false;
					}

				}
			}
			if(encryptedContainer.DoubleArrayDictionary.Count>0)
				foldouts["Edoublearray"] = EditorGUILayout.Foldout(foldouts["Edoublearray"],"Double[] - Encrypted");
			if (foldouts ["Edoublearray"]) {
				foreach (var key in encryptedContainer.DoubleArrayDictionary.Keys) {
					if (exportPref.ContainsKey ("Edoublearray_" + key)) {
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Space (20);
						exportPref ["Edoublearray_" + key] = GUILayout.Toggle (exportPref ["Edoublearray_" + key], key);
						GUILayout.EndHorizontal ();
					}
					else
					{
						exportPref["Edoublearray_" + key] = false;
					}

				}
			}
			if(notEncryptedContainer.ColorDictionary.Count>0)
				foldouts["NEcolor"] = EditorGUILayout.Foldout(foldouts["NEcolor"],"Color - Not Encrypted");
			if (foldouts ["NEcolor"]) {
				foreach (var key in notEncryptedContainer.ColorDictionary.Keys) {
					if (exportPref.ContainsKey ("NEcolor_" + key)) {
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Space (20);
						exportPref ["NEcolor_" + key] = GUILayout.Toggle (exportPref ["NEcolor_" + key], key);
						GUILayout.EndHorizontal ();
					}
					else
					{
						exportPref["NEcolor_" + key] = false;
					}

				}
			}
			if(encryptedContainer.ColorDictionary.Count>0)
				foldouts["Ecolor"] = EditorGUILayout.Foldout(foldouts["Ecolor"],"Color - Encrypted");
			if (foldouts ["Ecolor"]) {
				foreach (var key in encryptedContainer.ColorDictionary.Keys) {
					if (exportPref.ContainsKey ("Ecolor_" + key)) {
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Space (20);
						exportPref ["Ecolor_" + key] = GUILayout.Toggle (exportPref ["Ecolor_" + key], key);
						GUILayout.EndHorizontal ();
					}
					else
					{
						exportPref["Ecolor_" + key] = false;
					}

				}
			}
			if(notEncryptedContainer.QuaternionDictionary.Count>0)
				foldouts["NEquaternion"] = EditorGUILayout.Foldout(foldouts["NEquaternion"],"Quaternion - Not Encrypted");
			if (foldouts ["NEquaternion"]) {
				foreach (var key in notEncryptedContainer.QuaternionDictionary.Keys) {
					if (exportPref.ContainsKey ("NEquaternion_" + key)) {
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Space (20);
						exportPref ["NEquaternion_" + key] = GUILayout.Toggle (exportPref ["NEquaternion_" + key], key);
						GUILayout.EndHorizontal ();
					}
					else
					{
						exportPref["NEquaternion_" + key] = false;
					}

				}
			}
			if(encryptedContainer.QuaternionDictionary.Count>0)
				foldouts["Equaternion"] = EditorGUILayout.Foldout(foldouts["Equaternion"],"Quaternion - Encrypted");
			if (foldouts ["Equaternion"]) {
				foreach (var key in encryptedContainer.QuaternionDictionary.Keys) {
					if (exportPref.ContainsKey ("Equaternion_" + key)) {
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Space (20);
						exportPref ["Equaternion_" + key] = GUILayout.Toggle (exportPref ["Equaternion_" + key], key);
						GUILayout.EndHorizontal ();
					}
					else
					{
						exportPref["Equaternion_" + key] = false;
					}

				}
			}
			if(notEncryptedContainer.Vector2Dictionary.Count>0)
				foldouts["NEvector2"] = EditorGUILayout.Foldout(foldouts["NEvector2"],"Vector2 - Not Encrypted");
			if (foldouts ["NEvector2"]) {
				foreach (var key in notEncryptedContainer.Vector2Dictionary.Keys) {
					if (exportPref.ContainsKey ("NEvector2_" + key)) {
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Space (20);
						exportPref ["NEvector2_" + key] = GUILayout.Toggle (exportPref ["NEvector2_" + key], key);
						GUILayout.EndHorizontal ();
					}
					else
					{
						exportPref["NEvector2_" + key] = false;
					}

				}
			}
			if(encryptedContainer.Vector2Dictionary.Count>0)
				foldouts["Evector2"] = EditorGUILayout.Foldout(foldouts["Evector2"],"Vector2 - Encrypted");
			if (foldouts ["Evector2"]) {
				foreach (var key in encryptedContainer.Vector2Dictionary.Keys) {
					if (exportPref.ContainsKey ("Evector2_" + key)) {
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Space (20);
						exportPref ["Evector2_" + key] = GUILayout.Toggle (exportPref ["Evector2_" + key], key);
						GUILayout.EndHorizontal ();
					}
					else
					{
						exportPref["Evector2_" + key] = false;
					}

				}
			}
			if(notEncryptedContainer.Vector3Dictionary.Count>0)
				foldouts["NEvector3"] = EditorGUILayout.Foldout(foldouts["NEvector3"],"Vector3 - Not Encrypted");
			if (foldouts ["NEvector3"]) {
				foreach (var key in notEncryptedContainer.Vector3Dictionary.Keys) {
					if (exportPref.ContainsKey ("NEvector3_" + key)) {
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Space (30);
						exportPref ["NEvector3_" + key] = GUILayout.Toggle (exportPref ["NEvector3_" + key], key);
						GUILayout.EndHorizontal ();
					}
					else
					{
						exportPref["NEvector3_" + key] = false;
					}

				}
			}
			if(encryptedContainer.Vector3Dictionary.Count>0)
				foldouts["Evector3"] = EditorGUILayout.Foldout(foldouts["Evector3"],"Vector3 - Encrypted");
			if (foldouts ["Evector3"]) {
				foreach (var key in encryptedContainer.Vector3Dictionary.Keys) {
					if (exportPref.ContainsKey ("Evector3_" + key)) {
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Space (30);
						exportPref ["Evector3_" + key] = GUILayout.Toggle (exportPref ["Evector3_" + key], key);
						GUILayout.EndHorizontal ();
					}
					else
					{
						exportPref["Evector3_" + key] = false;
					}

				}
			}
			if(notEncryptedContainer.Vector4Dictionary.Count>0)
				foldouts["NEvector4"] = EditorGUILayout.Foldout(foldouts["NEvector4"],"Vector4 - Not Encrypted");
			if (foldouts ["NEvector4"]) {
				foreach (var key in notEncryptedContainer.Vector4Dictionary.Keys) {
					if (exportPref.ContainsKey ("NEvector4_" + key)) {
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Space (40);
						exportPref ["NEvector4_" + key] = GUILayout.Toggle (exportPref ["NEvector4_" + key], key);
						GUILayout.EndHorizontal ();
					}
					else
					{
						exportPref["NEvector4_" + key] = false;
					}

				}
			}
			if(encryptedContainer.Vector4Dictionary.Count>0)
				foldouts["Evector4"] = EditorGUILayout.Foldout(foldouts["Evector4"],"Vector4 - Encrypted");
			if (foldouts ["Evector4"]) {
				foreach (var key in encryptedContainer.Vector4Dictionary.Keys) {
					if (exportPref.ContainsKey ("Evector4_" + key)) {
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Space (40);
						exportPref ["Evector4_" + key] = GUILayout.Toggle (exportPref ["Evector4_" + key], key);
						GUILayout.EndHorizontal ();
					}
					else
					{
						exportPref["Evector4_" + key] = false;
					}

				}
			}
			GUILayout.EndScrollView ();
			if (GUILayout.Button ("Overwrite selected",GUILayout.ExpandWidth (true))) {
				if (EditorUtility.DisplayDialog ("Export for overwrite",
					   "Are you sure you want to export the selected EpicPrefs to your build ? Upon first run they will overwrite existing EpicPrefs in case of collisions.", "Yes", "No")) {
					foreach (var _export in exportPref) {
						if (_export.Value == false) {
							if (_export.Key.Substring(0,2) == "NE") {
								//Not encrypted values to remove
								int indexOfDelimiter = _export.Key.IndexOf("_");
								var type = _export.Key.Substring (2, indexOfDelimiter - 2);
								var key = _export.Key.Substring(indexOfDelimiter + 1);
								switch (type) {
								case "string":
									notEncryptedContainer.StringDictionary.Remove (key);
									break;
								case "int":
									notEncryptedContainer.IntegerDictionary.Remove (key);
									break;
								case "float":
									notEncryptedContainer.FloatDictionary.Remove (key);
									break;
								case "double":
									notEncryptedContainer.DoubleDictionary.Remove (key);
									break;
								case "long":
									notEncryptedContainer.LongDictionary.Remove (key);
									break;
								case "bool":
									notEncryptedContainer.BooleanDictionary.Remove (key);
									break;
								case "stringdict":
									notEncryptedContainer.StringDictionaryDictionary.Remove (key);
									break;
								case "intdict":
									notEncryptedContainer.IntegerDictionaryDictionary.Remove (key);
									break;
								case "floatdict":
									notEncryptedContainer.FloatDictionaryDictionary.Remove (key);
									break;
								case "doubledict":
									notEncryptedContainer.DoubleDictionaryDictionary.Remove (key);
									break;
								case "longdict":
									notEncryptedContainer.LongDictionaryDictionary.Remove (key);
									break;
								case "stringlist":
									notEncryptedContainer.StringListDictionary.Remove (key);
									break;
								case "intlist":
									notEncryptedContainer.IntegerListDictionary.Remove (key);
									break;
								case "floatlist":
									notEncryptedContainer.FloatListDictionary.Remove (key);
									break;
								case "doublelist":
									notEncryptedContainer.DoubleListDictionary.Remove (key);
									break;
								case "longlist":
									notEncryptedContainer.LongListDictionary.Remove (key);
									break;
								case "boollist":
									notEncryptedContainer.BooleanListDictionary.Remove (key);
									break;
								case "stringarray":
									notEncryptedContainer.StringArrayDictionary.Remove (key);
									break;
								case "intarray":
									notEncryptedContainer.IntegerArrayDictionary.Remove (key);
									break;
								case "floatarray":
									notEncryptedContainer.FloatArrayDictionary.Remove (key);
									break;
								case "doublearray":
									notEncryptedContainer.DoubleArrayDictionary.Remove (key);
									break;
								case "color":
									notEncryptedContainer.ColorDictionary.Remove (key);
									break;
								case "quaternion":
									notEncryptedContainer.QuaternionDictionary.Remove (key);
									break;
								case "vector2":
									notEncryptedContainer.Vector2Dictionary.Remove (key);
									break;
								case "vector3":
									notEncryptedContainer.Vector3Dictionary.Remove (key);
									break;
								case "vector4":
									notEncryptedContainer.Vector4Dictionary.Remove (key);
									break;
								default:
									Debug.LogError ("Type not implemented : " + type);
									break;
								}
							} else {
								//Encrypted values to remove
								int indexOfDelimiter = _export.Key.IndexOf("_");
								var type = _export.Key.Substring (1, indexOfDelimiter - 1);
								var key = _export.Key.Substring(indexOfDelimiter + 1);
								switch (type) {
								case "string":
									encryptedContainer.StringDictionary.Remove (key);
									break;
								case "int":
									encryptedContainer.IntegerDictionary.Remove (key);
									break;
								case "float":
									encryptedContainer.FloatDictionary.Remove (key);
									break;
								case "double":
									encryptedContainer.DoubleDictionary.Remove (key);
									break;
								case "long":
									encryptedContainer.LongDictionary.Remove (key);
									break;
								case "bool":
									encryptedContainer.BooleanDictionary.Remove (key);
									break;
								case "stringdict":
									encryptedContainer.StringDictionaryDictionary.Remove (key);
									break;
								case "intdict":
									encryptedContainer.IntegerDictionaryDictionary.Remove (key);
									break;
								case "floatdict":
									encryptedContainer.FloatDictionaryDictionary.Remove (key);
									break;
								case "doubledict":
									encryptedContainer.DoubleDictionaryDictionary.Remove (key);
									break;
								case "longdict":
									encryptedContainer.LongDictionaryDictionary.Remove (key);
									break;
								case "stringlist":
									encryptedContainer.StringListDictionary.Remove (key);
									break;
								case "intlist":
									encryptedContainer.IntegerListDictionary.Remove (key);
									break;
								case "floatlist":
									encryptedContainer.FloatListDictionary.Remove (key);
									break;
								case "doublelist":
									encryptedContainer.DoubleListDictionary.Remove (key);
									break;
								case "longlist":
									encryptedContainer.LongListDictionary.Remove (key);
									break;
								case "boollist":
									encryptedContainer.BooleanListDictionary.Remove (key);
									break;
								case "stringarray":
									encryptedContainer.StringArrayDictionary.Remove (key);
									break;
								case "intarray":
									encryptedContainer.IntegerArrayDictionary.Remove (key);
									break;
								case "floatarray":
									encryptedContainer.FloatArrayDictionary.Remove (key);
									break;
								case "doublearray":
									encryptedContainer.DoubleArrayDictionary.Remove (key);
									break;
								case "color":
									encryptedContainer.ColorDictionary.Remove (key);
									break;
								case "quaternion":
									encryptedContainer.QuaternionDictionary.Remove (key);
									break;
								case "vector2":
									encryptedContainer.Vector2Dictionary.Remove (key);
									break;
								case "vector3":
									encryptedContainer.Vector3Dictionary.Remove (key);
									break;
								case "vector4":
									encryptedContainer.Vector4Dictionary.Remove (key);
									break;
								default:
									Debug.LogError ("Type not implemented : " + type);
									break;
								}
							}
						}
					}
					Operators.CopyPrefs (this, notEncryptedContainer, false,false);
					Operators.CopyPrefs (this, encryptedContainer, true,false);
					AssetDatabase.Refresh();  
					this.Close ();
				}
			}
		}
	}

	#endif
}

