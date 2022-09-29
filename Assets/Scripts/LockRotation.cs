// Decompile from assembly: Assembly-CSharp.dll
using System;
using UnityEngine;

[ExecuteInEditMode]
public class LockRotation : MonoBehaviour
{
	private void Update()
	{
		base.transform.rotation = Camera.main.transform.rotation;
	}
}
