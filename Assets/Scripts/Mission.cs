// Decompile from assembly: Assembly-CSharp.dll
using System;
using UnityEngine;

public class Mission
{
	public int type;

	public int targetValue;

	public int currentValue;

	public int tempValue;

	public bool resetAtGameOver = true;

	public string text;

	public double completeTime;

	public Mission(int type, int targetValue, bool reset, string str)
	{
		this.type = type;
		this.targetValue = targetValue;
		this.resetAtGameOver = reset;
		this.text = str;
	}

	public bool IsCompleted()
	{
		bool flag;
		if (this.type == 8)
		{
			flag = (this.currentValue <= this.targetValue);
		}
		else
		{
			flag = (this.currentValue >= this.targetValue);
		}
		if (flag)
		{
			this.tempValue = this.currentValue;
		}
		return flag;
	}

	public void OnGameOver()
	{
		if (this.currentValue < 0)
		{
			this.currentValue = 0;
		}
		else if (this.type == 20)
		{
			this.currentValue++;
		}
		this.tempValue = this.currentValue;
	}

	public void OnBossKilled()
	{
		this.tempValue = this.currentValue;
	}

	public float GetAlpha()
	{
		float result;
		if (this.completeTime == 0.0 || (double)Time.time > this.completeTime + 4.0)
		{
			result = 0f;
		}
		else
		{
			result = 1f;
		}
		return result;
	}

	public void OnEnemyKilled(int enemyType)
	{
		bool flag = false;
		if (this.type == 1 || this.type == 19)
		{
			this.currentValue++;
			flag = true;
		}
		else if (this.type == 9 && ItemManager.instance.currentWeapon.category.id == "Axe")
		{
			this.currentValue++;
			flag = true;
		}
		else if (this.type == 10 && ItemManager.instance.currentWeapon.category.id == "Sword1h")
		{
			this.currentValue++;
			flag = true;
		}
		else if (this.type == 11 && ItemManager.instance.currentWeapon.category.id == "Dagger")
		{
			this.currentValue++;
			flag = true;
		}
		else if (this.type == 12 && ItemManager.instance.currentWeapon.category.id == "Sword2h")
		{
			this.currentValue++;
			flag = true;
		}
		else if (this.type == 13 && ItemManager.instance.currentWeapon.category.id == "Fist")
		{
			this.currentValue++;
			flag = true;
		}
		else if (this.type == 14 && ItemManager.instance.currentWeapon.category.id == "Spear")
		{
			this.currentValue++;
			flag = true;
		}
		else if ((this.type == 15 || this.type == 16) && (WeatherManager.instance.activeWeather == 2 || WeatherManager.instance.activeWeather == 3))
		{
			this.currentValue++;
			flag = true;
		}
		else if (this.type == 17 && enemyType == 2)
		{
			this.currentValue++;
			flag = true;
		}
		else if (this.type == 18 && enemyType == 3)
		{
			this.currentValue++;
			flag = true;
		}
		else if (this.type == 5 && this.currentValue >= 0)
		{
			this.currentValue = EndlessManager.instance.pointWithoutCoins;
			flag = true;
		}
		else if (this.type == 4)
		{
			this.currentValue = EndlessManager.instance.point;
			flag = true;
		}
		if (flag)
		{
			this.tempValue = this.currentValue;
			if (this.IsCompleted())
			{
				AudioManager.instance.MissionCompletedSound();
				MissionManager.instance.SetColors(Color.black, -1);
				this.completeTime = (double)Time.time;
				MissionManager.instance.CheckAllMissionsCompletion();
			}
		}
	}

	public void Complete()
	{
		this.currentValue = this.targetValue;
		this.tempValue = this.currentValue;
		AudioManager.instance.MissionCompletedSound();
		this.completeTime = (double)Time.time;
		MissionManager.instance.CheckAllMissionsCompletion();
	}

	public void OnCoinCollected(int amount)
	{
		bool flag = false;
		if (this.type == 0 || this.type == 3)
		{
			this.currentValue += amount;
			flag = true;
		}
		else if (this.type == 2 && MonetizationManager.instance.comboCount > this.currentValue)
		{
			this.currentValue = MonetizationManager.instance.comboCount;
			flag = true;
		}
		else if (this.type == 5)
		{
			EndlessManager.instance.pointWithoutCoins = 0;
			this.currentValue = 0;
			flag = true;
		}
		if (flag)
		{
			this.tempValue = this.currentValue;
			if (this.IsCompleted())
			{
				AudioManager.instance.MissionCompletedSound();
				this.completeTime = (double)Time.time;
				MissionManager.instance.CheckAllMissionsCompletion();
			}
		}
	}

	public void Reset()
	{
		if (!this.IsCompleted())
		{
			this.currentValue = 0;
		}
	}
}
