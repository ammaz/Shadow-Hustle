// Decompile from assembly: Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class StatManager : MonoBehaviour
{
	public static StatManager instance;

	public List<Stat> stats;

	public StatContainer statContainerPrefab;

	protected void Awake()
	{
		StatManager.instance = this;
	}

	private void Start()
	{
		this.Initialize();
	}

	private void Initialize()
	{
		this.stats = new List<Stat>();
		this.stats.Add(new Stat(0, "Highest score", "p"));
		this.stats.Add(new Stat(1, "Belt", string.Empty));
		this.stats.Add(new Stat(2, "Enemies killed", string.Empty));
		this.stats.Add(new Stat(3, "Boss killed", string.Empty));
		this.stats.Add(new Stat(4, "Shadow killed", string.Empty));
		this.stats.Add(new Stat(5, "Fasted shadow kill", "s"));
		this.stats.Add(new Stat(6, "Shadow dodge", string.Empty));
		this.stats.Add(new Stat(7, "Deaths", string.Empty));
		this.stats.Add(new Stat(8, "Souls collected", "s"));
		this.stats.Add(new Stat(9, "Highest soul combo", string.Empty));
		this.stats.Add(new Stat(10, "Souls spent", string.Empty));
		this.stats.Add(new Stat(11, "Gifts opened", string.Empty));
		this.stats.Add(new Stat(12, "Duplicate items won", string.Empty));
		bool flag = true;
		int[] defaultValue = new int[this.stats.Count];
        int[] arrayInt;
        if (PlayerPrefs.HasKey("statValues")) {

            arrayInt = PlayerPrefsX.GetIntArray("statValues");
        } else
        {
            arrayInt = defaultValue;
        }
		foreach (Stat current in this.stats)
		{
			if (current.id < 3 || current.id > 6)
			{
				StatContainer statContainer = UnityEngine.Object.Instantiate<StatContainer>(this.statContainerPrefab, this.statContainerPrefab.transform.parent);
				if (!flag)
				{
					statContainer.bg.color = new Color(1f, 1f, 1f, 0.05882353f);
				}
				statContainer.stat = current;
				current.statContainer = statContainer;
				current.UpdateStat(arrayInt[current.id]);
				statContainer.descriptionText.text = current.description;
				statContainer.stat.UpdateStat(statContainer.stat.value);
				flag = !flag;
				statContainer.gameObject.SetActive(true);
			}
		}
	}

	public void UpdateStats()
	{
		int[] array = new int[this.stats.Count];
		for (int i = 0; i < this.stats.Count; i++)
		{
			array[i] = this.stats[i].value;
		}
		PlayerPrefsX.SetIntArray("statValues", array);
	}
}
