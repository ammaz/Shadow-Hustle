// Decompile from assembly: Assembly-CSharp.dll
using CodeStage.AntiCheat.ObscuredTypes;
using System;
using UnityEngine;

public class EndlessManager : MonoBehaviour
{
	public static EndlessManager instance;

	public ObscuredInt point;

	public ObscuredInt pointWithoutCoins;

	public GameObject trees;

	public int coinsWon;

	public int coinAmount;

	private Vector3 missionChaserOriginalPosition;

	private void Awake()
	{
		EndlessManager.instance = this;
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public int CoinsWon()
	{
		if (this.coinAmount == 0)
		{
			return this.coinsWon;
		}
		return MonetizationManager.instance.coins - this.coinAmount;
	}

	public void OnStarted()
	{
		this.coinAmount = MonetizationManager.instance.coins;
		MissionManager.instance.OnGameStart();
		this.point = 0;
		this.pointWithoutCoins = 0;
	}

	public void OnGameOver()
	{
		this.coinsWon = MonetizationManager.instance.coins - this.coinAmount;
		this.coinAmount = 0;
		if (this.point > StatManager.instance.stats[0].value)
		{
			StatManager.instance.stats[0].UpdateStat(this.point);
		}
		MissionManager.instance.OnGameOver();
	}

	public void OnNormalEnemyKilled()
	{
		if (!SceneManager.instance.gameStarted)
		{
			return;
		}
		this.point += 100;
		this.pointWithoutCoins += 100;
	}

	public void OnSpecialEnemyKilled()
	{
		if (!SceneManager.instance.gameStarted)
		{
			return;
		}
		this.point += 150;
		this.pointWithoutCoins += 150;
	}
}
