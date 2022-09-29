// Decompile from assembly: Assembly-CSharp.dll
using System;
using UnityEngine;

public class GameOverTipManager : MonoBehaviour
{
	public static GameOverTipManager instance;

	private string[] tips;

	private int lastTipIndex;

	private void Awake()
	{
		GameOverTipManager.instance = this;
	}

	private void Start()
	{
		this.lastTipIndex = PlayerPrefs.GetInt("lastTipIndex", 0);
		this.tips = new string[9];
		this.tips[0] = "Complete all missions to unlock next belt exam";
		this.tips[1] = "You can use souls to re-enter a failed belt exam";
		this.tips[2] = "If you move without hitting an enemy or a souls, you get stunned briefly";
		this.tips[3] = "Collecting souls stops movement only if you wont hit an enemy otherwise";
		this.tips[4] = "Collecting souls before they disappear builds up your soul combo";
		this.tips[5] = "Getting souls or soul combos doesn't effect your score";
		this.tips[6] = "Every time you pass a belt exam, you unlock the examiners hat and weapon";
		this.tips[7] = "Every time you pass a belt exam, you unlock the examiners hat and weapon";
		this.tips[8] = "Collecting souls stops movement only if you wont hit an enemy otherwise";
	}

	public string GetNextTip()
	{
		this.lastTipIndex++;
		if (this.lastTipIndex > this.tips.Length)
		{
			this.lastTipIndex = 1;
		}
		return this.tips[this.lastTipIndex - 1];
	}

	public void Save()
	{
		PlayerPrefs.SetInt("lastTipIndex", this.lastTipIndex);
	}
}
