using UnityEngine;
using System.Collections;

namespace EpicPrefsTools
{
	public class EpicSaver : MonoBehaviour {
		void OnAwake(){
			DontDestroyOnLoad (this.gameObject);
		}
		public void DestroySaver(){
			Destroy (this.gameObject);
		}
		// Use this for initialization
		void OnDestroy () {
			Serializer.BatchWrite ();
			#if UNITY_EDITOR
			EpicPrefsEditorTools.EpicPrefsEditor.Exit();
			#endif
		}
	}
}