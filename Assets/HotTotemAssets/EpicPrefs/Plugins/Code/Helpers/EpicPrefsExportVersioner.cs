using System;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace EpicPrefsTools
{
	public static class EpicPrefsExportVersioner
	{
		public static string prefix = "#EpicPrefsInitializer";
		public static string guid = "7fc91008-be77-449d-81cb-f59e9313d0c8";
		#if UNITY_EDITOR
		public static void TimeStamp(ScriptableObject _obj){
			MonoScript ms = MonoScript.FromScriptableObject (_obj);
			var m_ScriptFilePath = AssetDatabase.GetAssetPath( ms );
			var m_ScriptFolder = "";
			FileInfo fi = new FileInfo( m_ScriptFilePath);
			m_ScriptFolder = fi.Directory.ToString();
			var currentFolder = m_ScriptFolder.Replace( '\\', '/');
			var parentDirectory = Directory.GetParent (currentFolder);
			var currentFile = Path.Combine (parentDirectory.ToString (), "Plugins/Code/Helpers/") + "EpicPrefsExportVersioner.cs";
			string[] arrLine = File.ReadAllLines(currentFile);
			arrLine [12] = "\t\tpublic static string guid = \"" + Guid.NewGuid() + "\";";
			File.WriteAllLines (currentFile, arrLine);
		}
		#endif
	};
}

