using UnityEngine;
using System.Collections;
using System;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#if UNITY_5_3_OR_NEWER
using UnityEditor.SceneManagement;
#endif
#endif

namespace EpicPrefsEditorTools {

	#if UNITY_EDITOR
	[InitializeOnLoad]
	[ExecuteInEditMode]
	#endif
	public class EpicPrefsWatchDog : MonoBehaviour {
		#if UNITY_EDITOR
		private static EpicPrefsWatchDog watchDog = null;
		// Use this for initialization
		static EpicPrefsWatchDog () {
			EditorApplication.update += updated;
			EditorApplication.playmodeStateChanged += playPressed;
		}
		private static void hierachyChanged()
		{
			Debug.Log ("Called");
		}
		private static void updated()
		{
			if (watchDog == null)
			{
				var _activeDogs = FindObjectsOfType<EpicPrefsWatchDog> ();
				if (_activeDogs.Length == 0) {
					GameObject createdGO = new GameObject ();
					createdGO.hideFlags = HideFlags.HideInHierarchy;
					watchDog = createdGO.AddComponent<EpicPrefsWatchDog> ();
				} else {
					watchDog = _activeDogs [0];
					for (int i = 1; i < _activeDogs.Length; i++) {
						DestroyImmediate (_activeDogs [i]);
					}
				}
			}
		}
		static void playPressed()
		{
			// This method is run whenever the playmode state is changed.
			if (EditorApplication.isPlayingOrWillChangePlaymode)
			{
				EpicPrefsEditor.Exit ();
			}
		}

		void OnDestroy()
		{
			EpicPrefsEditor.Exit ();
		}
		#endif
	}
}