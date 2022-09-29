using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Text;
using System;

#if UNITY_EDITOR
using EpicPrefsEditorTools;
#endif
namespace EpicPrefsTools
{
	public static class Serializer
	{
		private static EpicPrefsContainer epicPrefsContainerNotEncrypted, epicPrefsContainerEncrypted;
		private static bool batchWrite = false;
		private static bool edited = false;
		private static EpicSaver saverObject;
		#region Editor Only

		#if UNITY_EDITOR
		public class PrefEventArgs
		{
			public PrefEventArgs (string s, SerializationTypes t, bool e)
			{
				Name = s;
				Type = t;
				Encrpyted = e;
				Deleted = false;
			}

			public PrefEventArgs (string s, SerializationTypes t, bool e, bool d)
			{
				Name = s;
				Type = t;
				Encrpyted = e;
				Deleted = d;
			}

			public String Name { get; private set; }

			public SerializationTypes Type { get; private set; }

			public bool Encrpyted { get; private set; }

			public bool Deleted { get; private set; }
		}
		#endif
		#endregion

		static Serializer ()
		{
			if (epicPrefsContainerEncrypted == null) {
				epicPrefsContainerEncrypted = EpicPrefsContainerHandler.LoadEpicPrefsContainer (true);
			}
			if (epicPrefsContainerNotEncrypted == null) {
				epicPrefsContainerNotEncrypted = EpicPrefsContainerHandler.LoadEpicPrefsContainer (false);
			}

		}
		public static void SetBatchWrite(bool batching){
			batchWrite = batching;
			if (batching) {
				if (saverObject == null) {
					var _saver = new GameObject ();
					_saver.name = "EpicSaver";
					_saver.hideFlags = HideFlags.HideInHierarchy;
					saverObject = _saver.AddComponent<EpicSaver> ();
				}
			} else {
				if (saverObject != null) {
					saverObject.DestroySaver ();	
				}
			}
		}
		public static void ImportPrefs(){
			epicPrefsContainerEncrypted = EpicPrefsContainerHandler.ImportEpicPrefsContainer (epicPrefsContainerEncrypted,true);
			epicPrefsContainerNotEncrypted = EpicPrefsContainerHandler.ImportEpicPrefsContainer (epicPrefsContainerNotEncrypted,false);
			EpicPrefsContainerHandler.SaveEpicPrefsContainer (epicPrefsContainerEncrypted, true);
			EpicPrefsContainerHandler.SaveEpicPrefsContainer (epicPrefsContainerNotEncrypted, false);
		}
		public static void BatchWrite(){
			if (edited && batchWrite) {
				if (epicPrefsContainerEncrypted != null) {
					EpicPrefsContainerHandler.SaveEpicPrefsContainer (epicPrefsContainerEncrypted, true);
				}
				if (epicPrefsContainerNotEncrypted != null) {
					EpicPrefsContainerHandler.SaveEpicPrefsContainer (epicPrefsContainerNotEncrypted, false);
				}
				edited = false;
			}
		}
		public static bool Serialize (string name, object value, SerializationTypes type, bool encrypted)
		{
			if (encrypted && epicPrefsContainerEncrypted == null) {
				epicPrefsContainerEncrypted = EpicPrefsContainerHandler.LoadEpicPrefsContainer (true);
			}
			if (!encrypted && epicPrefsContainerNotEncrypted == null) {
				epicPrefsContainerNotEncrypted = EpicPrefsContainerHandler.LoadEpicPrefsContainer (false);
			}
			if (encrypted)
				Save (name, value, type, encrypted, ref epicPrefsContainerEncrypted);
			else
				Save (name, value, type, encrypted, ref epicPrefsContainerNotEncrypted);
			#if UNITY_EDITOR
			EpicPrefsEditor.SetEditorPref (type, name, value, encrypted);
			#endif
			return true;
		}

		public static object Deserialize (string name, SerializationTypes type, bool encrypted)
		{
			if (encrypted && epicPrefsContainerEncrypted == null) {
				epicPrefsContainerEncrypted = EpicPrefsContainerHandler.LoadEpicPrefsContainer (true);
			}
			if (!encrypted && epicPrefsContainerNotEncrypted == null) {
				epicPrefsContainerNotEncrypted = EpicPrefsContainerHandler.LoadEpicPrefsContainer (false);
			}
			if (encrypted)
				return Load (name, type, ref epicPrefsContainerEncrypted);
			else
				return Load (name, type, ref epicPrefsContainerNotEncrypted);
		}

		public static EpicPrefsContainer GetContainer (bool encrypted)
		{
			if (encrypted && epicPrefsContainerEncrypted == null) {
				epicPrefsContainerEncrypted = EpicPrefsContainerHandler.LoadEpicPrefsContainer (true);
			}
			if (!encrypted && epicPrefsContainerNotEncrypted == null) {
				epicPrefsContainerNotEncrypted = EpicPrefsContainerHandler.LoadEpicPrefsContainer (false);
			}
			if (encrypted)
				return EpicPrefsContainerHandler.Copy (epicPrefsContainerEncrypted);
			else
				return EpicPrefsContainerHandler.Copy (epicPrefsContainerNotEncrypted);
		}

		private static bool Save (string name, object value, SerializationTypes type, bool encrypted, ref EpicPrefsContainer container)
		{
			switch (type) {
			case SerializationTypes.ArrayDouble:
				container.DoubleArrayDictionary [name] = (double[])value;
				break;
			case SerializationTypes.ArrayFloat:
				container.FloatArrayDictionary [name] = (float[])value;
				break;
			case SerializationTypes.ArrayInt:
				container.IntegerArrayDictionary [name] = (int[])value;
				break;
			case SerializationTypes.ArrayString:
				container.StringArrayDictionary [name] = (string[])value;
				break;
			case SerializationTypes.Bool:
				container.BooleanDictionary [name] = (bool)value;
				break;
			case SerializationTypes.Color:
				container.ColorDictionary [name] = (Color)value;
				break;
			case SerializationTypes.DictB:
				container.BooleanDictionaryDictionary [name] = (Dictionary<string,bool>)value;
				break;
			case SerializationTypes.DictD:
				container.DoubleDictionaryDictionary [name] = (Dictionary<string,double>)value;
				break;
			case SerializationTypes.DictF:
				container.FloatDictionaryDictionary [name] = (Dictionary<string,float>)value;
				break;
			case SerializationTypes.DictI:
				container.IntegerDictionaryDictionary [name] = (Dictionary<string,int>)value;
				break;
			case SerializationTypes.DictL:
				container.LongDictionaryDictionary [name] = (Dictionary<string,long>)value;
				break;
			case SerializationTypes.DictS:
				container.StringDictionaryDictionary [name] = (Dictionary<string,string>)value;
				break;
			case SerializationTypes.Double:
				container.DoubleDictionary [name] = (double)value;
				break;
			case SerializationTypes.Float:
				container.FloatDictionary [name] = (float)value;
				break;
			case SerializationTypes.Integer:
				container.IntegerDictionary [name] = (int)value;
				break;
			case SerializationTypes.ListB:
				container.BooleanListDictionary [name] = (List<bool>)value;
				break;
			case SerializationTypes.ListD:
				container.DoubleListDictionary [name] = (List<double>)value;
				break;
			case SerializationTypes.ListF:
				container.FloatListDictionary [name] = (List<float>)value;
				break;
			case SerializationTypes.ListI:
				container.IntegerListDictionary [name] = (List<int>)value;
				break;
			case SerializationTypes.ListL:
				container.LongListDictionary [name] = (List<long>)value;
				break;
			case SerializationTypes.ListS:
				container.StringListDictionary [name] = (List<string>)value;
				break;
			case SerializationTypes.Long:
				container.LongDictionary [name] = (long)value;
				break;
			case SerializationTypes.Quaternion:
				container.QuaternionDictionary [name] = (Quaternion)value;
				break;
			case SerializationTypes.String:
				container.StringDictionary [name] = (string)value;
				break;
			case SerializationTypes.Vector2:
				container.Vector2Dictionary [name] = (Vector2)value;
				break;
			case SerializationTypes.Vector3:
				container.Vector3Dictionary [name] = (Vector3)value;
				break;
			case SerializationTypes.Vector4:
				container.Vector4Dictionary [name] = (Vector4)value;
				break;
			default:
				Debug.LogError ("YOu missed me " + type.ToString ());
				return false;
			}
			if (!batchWrite)
				EpicPrefsContainerHandler.SaveEpicPrefsContainer (container, encrypted);
			else
				edited = true;
			return true;
		}

		private static object Load (string name, SerializationTypes type, ref EpicPrefsContainer container)
		{
			switch (type) {
			case SerializationTypes.ArrayDouble:
				if (container.DoubleArrayDictionary.ContainsKey (name))
					return container.DoubleArrayDictionary [name];
				else
					return null;
			case SerializationTypes.ArrayFloat:
				if (container.FloatArrayDictionary.ContainsKey (name))
					return container.FloatArrayDictionary [name];
				else
					return null;
			case SerializationTypes.ArrayInt:
				if (container.IntegerArrayDictionary.ContainsKey (name))
					return container.IntegerArrayDictionary [name];
				else
					return null;
			case SerializationTypes.ArrayString:
				if (container.StringArrayDictionary.ContainsKey (name))
					return container.StringArrayDictionary [name];
				else
					return null;
			case SerializationTypes.Bool:
				if (container.BooleanDictionary.ContainsKey (name))
					return container.BooleanDictionary [name];
				else
					return null;
			case SerializationTypes.Color:
				if (container.ColorDictionary.ContainsKey (name))
					return container.ColorDictionary [name];
				else
					return null;
			case SerializationTypes.DictB:
				if (container.BooleanDictionaryDictionary.ContainsKey (name))
					return container.BooleanDictionaryDictionary [name];
				else
					return null;
			case SerializationTypes.DictD:
				if (container.DoubleDictionaryDictionary.ContainsKey (name))
					return container.DoubleDictionaryDictionary [name];
				else
					return null;
			case SerializationTypes.DictF:
				if (container.FloatDictionaryDictionary.ContainsKey (name))
					return container.FloatDictionaryDictionary [name];
				else
					return null;
			case SerializationTypes.DictI:
				if (container.IntegerDictionaryDictionary.ContainsKey (name))
					return container.IntegerDictionaryDictionary [name];
				else
					return null;
			case SerializationTypes.DictL:
				if (container.LongDictionaryDictionary.ContainsKey (name))
					return container.LongDictionaryDictionary [name];
				else
					return null;
			case SerializationTypes.DictS:
				if (container.StringDictionaryDictionary.ContainsKey (name))
					return container.StringDictionaryDictionary [name];
				else
					return null;
			case SerializationTypes.Double:
				if (container.DoubleDictionary.ContainsKey (name))
					return container.DoubleDictionary [name];
				else
					return null;
			case SerializationTypes.Float:
				if (container.FloatDictionary.ContainsKey (name))
					return container.FloatDictionary [name];
				else
					return null;
			case SerializationTypes.Integer:
				if (container.IntegerDictionary.ContainsKey (name))
					return container.IntegerDictionary [name];
				else
					return null;
			case SerializationTypes.ListB:
				if (container.BooleanListDictionary.ContainsKey (name))
					return container.BooleanListDictionary [name];
				else
					return null;
			case SerializationTypes.ListD:
				if (container.DoubleListDictionary.ContainsKey (name))
					return container.DoubleListDictionary [name];
				else
					return null;
			case SerializationTypes.ListF:
				if (container.FloatListDictionary.ContainsKey (name))
					return container.FloatListDictionary [name];
				else
					return null;
			case SerializationTypes.ListI:
				if (container.IntegerListDictionary.ContainsKey (name))
					return container.IntegerListDictionary [name];
				else
					return null;
			case SerializationTypes.ListL:
				if (container.LongListDictionary.ContainsKey (name))
					return container.LongListDictionary [name];
				else
					return null;
			case SerializationTypes.ListS:
				if (container.StringListDictionary.ContainsKey (name))
					return container.StringListDictionary [name];
				else
					return null;
			case SerializationTypes.Long:
				if (container.LongDictionary.ContainsKey (name))
					return container.LongDictionary [name];
				else
					return null;
			case SerializationTypes.Quaternion:
				if (container.QuaternionDictionary.ContainsKey (name))
					return container.QuaternionDictionary [name];
				else
					return null;
			case SerializationTypes.String:
				if (container.StringDictionary.ContainsKey (name))
					return container.StringDictionary [name];
				else
					return null;
			case SerializationTypes.Vector2:
				if (container.Vector2Dictionary.ContainsKey (name))
					return container.Vector2Dictionary [name];
				else
					return null;
			case SerializationTypes.Vector3:
				if (container.Vector3Dictionary.ContainsKey (name))
					return container.Vector3Dictionary [name];
				else
					return null;
			case SerializationTypes.Vector4:
				if (container.Vector4Dictionary.ContainsKey (name))
					return container.Vector4Dictionary [name];
				else
					return null;
			default:
				Debug.LogError ("YOu missed me " + type.ToString ());
				return null;
			}
		}

		public static bool Delete (string name, SerializationTypes type, bool encrypted)
		{
			if (encrypted && epicPrefsContainerEncrypted == null) {
				epicPrefsContainerEncrypted = EpicPrefsContainerHandler.LoadEpicPrefsContainer (true);
			}
			if (!encrypted && epicPrefsContainerNotEncrypted == null) {
				epicPrefsContainerNotEncrypted = EpicPrefsContainerHandler.LoadEpicPrefsContainer (false);
			}
			var success = false;
			if (encrypted) {
				success = Remove (name, type, ref epicPrefsContainerEncrypted);
				if(!batchWrite)
					EpicPrefsContainerHandler.SaveEpicPrefsContainer (epicPrefsContainerEncrypted, encrypted);
				else
					edited = true;
			} else {
				success = Remove (name, type, ref epicPrefsContainerNotEncrypted);
				if(!batchWrite)
					EpicPrefsContainerHandler.SaveEpicPrefsContainer (epicPrefsContainerNotEncrypted, encrypted);
				else
					edited = true;
			}
			#if UNITY_EDITOR
			EpicPrefsEditor.DeleteEditorPref (name, type, encrypted, true);
			#endif
			return success;
		}

		private static bool Remove (string name, SerializationTypes type, ref EpicPrefsContainer container)
		{
			switch (type) {
			case SerializationTypes.ArrayDouble:
				if (container.DoubleArrayDictionary.ContainsKey (name))
					container.DoubleArrayDictionary.Remove (name);
				else
					return false;
				break;
			case SerializationTypes.ArrayFloat:
				if (container.FloatArrayDictionary.ContainsKey (name))
					container.FloatArrayDictionary.Remove (name);
				else
					return false;
				break;
			case SerializationTypes.ArrayInt:
				if (container.IntegerArrayDictionary.ContainsKey (name))
					container.IntegerArrayDictionary.Remove (name);
				else
					return false;
				break;
			case SerializationTypes.ArrayString:
				if (container.StringArrayDictionary.ContainsKey (name))
					container.StringArrayDictionary.Remove (name);
				else
					return false;
				break;
			case SerializationTypes.Bool:
				if (container.BooleanDictionary.ContainsKey (name))
					container.BooleanDictionary.Remove (name);
				else
					return false;
				break;
			case SerializationTypes.Color:
				if (container.ColorDictionary.ContainsKey (name))
					container.ColorDictionary.Remove (name);
				else
					return false;
				break;
			case SerializationTypes.DictB:
				if (container.BooleanDictionaryDictionary.ContainsKey (name))
					container.BooleanDictionaryDictionary.Remove (name);
				else
					return false;
				break;
			case SerializationTypes.DictD:
				if (container.DoubleDictionaryDictionary.ContainsKey (name))
					container.DoubleDictionaryDictionary.Remove (name);
				else
					return false;
				break;
			case SerializationTypes.DictF:
				if (container.FloatDictionaryDictionary.ContainsKey (name))
					container.FloatDictionaryDictionary.Remove (name);
				else
					return false;
				break;
			case SerializationTypes.DictI:
				if (container.IntegerDictionaryDictionary.ContainsKey (name))
					container.IntegerDictionaryDictionary.Remove (name);
				else
					return false;
				break;
			case SerializationTypes.DictL:
				if (container.LongDictionaryDictionary.ContainsKey (name))
					container.LongDictionaryDictionary.Remove (name);
				else
					return false;
				break;
			case SerializationTypes.DictS:
				if (container.StringDictionaryDictionary.ContainsKey (name))
					container.StringDictionaryDictionary.Remove (name);
				else
					return false;
				break;
			case SerializationTypes.Double:
				if (container.DoubleDictionary.ContainsKey (name))
					container.DoubleDictionary.Remove (name);
				else
					return false;
				break;
			case SerializationTypes.Float:
				if (container.FloatDictionary.ContainsKey (name))
					container.FloatDictionary.Remove (name);
				else
					return false;
				break;
			case SerializationTypes.Integer:
				if (container.IntegerDictionary.ContainsKey (name))
					container.IntegerDictionary.Remove (name);
				else
					return false;
				break;
			case SerializationTypes.ListB:
				if (container.BooleanListDictionary.ContainsKey (name))
					container.BooleanListDictionary.Remove (name);
				else
					return false;
				break;
			case SerializationTypes.ListD:
				if (container.DoubleListDictionary.ContainsKey (name))
					container.DoubleListDictionary.Remove (name);
				else
					return false;
				break;
			case SerializationTypes.ListF:
				if (container.FloatListDictionary.ContainsKey (name))
					container.FloatListDictionary.Remove (name);
				else
					return false;
				break;
			case SerializationTypes.ListI:
				if (container.IntegerListDictionary.ContainsKey (name))
					container.IntegerListDictionary.Remove (name);
				else
					return false;
				break;
			case SerializationTypes.ListL:
				if (container.LongListDictionary.ContainsKey (name))
					container.LongListDictionary.Remove (name);
				else
					return false;
				break;
			case SerializationTypes.ListS:
				if (container.StringListDictionary.ContainsKey (name))
					container.StringListDictionary.Remove (name);
				else
					return false;
				break;
			case SerializationTypes.Long:
				if (container.LongDictionary.ContainsKey (name))
					container.LongDictionary.Remove (name);
				else
					return false;
				break;
			case SerializationTypes.Quaternion:
				if (container.QuaternionDictionary.ContainsKey (name))
					container.QuaternionDictionary.Remove (name);
				else
					return false;
				break;
			case SerializationTypes.String:
				if (container.StringDictionary.ContainsKey (name))
					container.StringDictionary.Remove (name);
				else
					return false;
				break;
			case SerializationTypes.Vector2:
				if (container.Vector2Dictionary.ContainsKey (name))
					container.Vector2Dictionary.Remove (name);
				else
					return false;
				break;
			case SerializationTypes.Vector3:
				if (container.Vector3Dictionary.ContainsKey (name))
					container.Vector3Dictionary.Remove (name);
				else
					return false;
				break;
			case SerializationTypes.Vector4:
				if (container.Vector4Dictionary.ContainsKey (name))
					container.Vector4Dictionary.Remove (name);
				else
					return false;
				break;
			default:
				Debug.LogError ("You missed me " + type.ToString ());
				return false;
			}
			return true;
		}

		public static void DeleteAll (bool encrypted)
		{
			if (encrypted && epicPrefsContainerEncrypted == null) {
				epicPrefsContainerEncrypted = EpicPrefsContainerHandler.LoadEpicPrefsContainer (true);
			}
			if (!encrypted && epicPrefsContainerNotEncrypted == null) {
				epicPrefsContainerNotEncrypted = EpicPrefsContainerHandler.LoadEpicPrefsContainer (false);
			}
			if (encrypted) {
				epicPrefsContainerEncrypted = new EpicPrefsContainer ();
				if(!batchWrite)
					EpicPrefsContainerHandler.SaveEpicPrefsContainer (epicPrefsContainerEncrypted, encrypted);
				else
					edited = true;
			} else {
				epicPrefsContainerNotEncrypted = new EpicPrefsContainer ();
				if(!batchWrite)
					EpicPrefsContainerHandler.SaveEpicPrefsContainer (epicPrefsContainerNotEncrypted, encrypted);
				else
					edited = true;
			}
			#if UNITY_EDITOR
			EpicPrefsEditor.DeleteAllEditorPrefs (encrypted);
			#endif
		}
		public static bool HasKey(string name, bool encrypted, SerializationTypes type){
			if (encrypted) {
				if (Load (name, type, ref epicPrefsContainerEncrypted) == null) {
					return false;
				} else {
					return true;
				}
			} else {
				if (Load (name, type, ref epicPrefsContainerNotEncrypted) == null) {
					return false;
				} else {
					return true;
				}
			}
		}
		public enum SerializationTypes
		{
			Integer,
			String,
			Float,
			Long,
			Double,
			Bool,
			Vector2,
			Vector3,
			Vector4,
			List,
			Dict,
			Transform,
			Quaternion,
			Color,
			ArrayString,
			ArrayInt,
			ArrayFloat,
			ArrayDouble,
			Editor,
			DictS,
			DictI,
			DictB,
			DictL,
			DictD,
			DictF,
			ListS,
			ListI,
			ListB,
			ListL,
			ListD,
			ListF
		}
	}
}