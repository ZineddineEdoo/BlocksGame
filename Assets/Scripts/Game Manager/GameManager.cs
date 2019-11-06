using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	// GameStarting -> GameStarted -> GameEnding
	public event EventHandler GameStarting;
	public event EventHandler GameStarted;
	public event EventHandler GameEnding;
	
	// True: GameStarted -> GameEnding	
	public bool IsGameStarted { get; private set; }
	public bool IsUILoaded { get; set; }
	// Not In Use
	//public float GameTime { get; private set; }

	void Awake()
	{
		StartGame();
	}

	public void StartGame()
	{
		if (!IsGameStarted)
		{
			StartCoroutine(StartGameDelayed());
		}
	}

	private IEnumerator StartGameDelayed()
	{
		GameStarting?.Invoke(this, null);

		yield return new WaitUntil(() => IsUILoaded);

		IsGameStarted = true;
		//GameTime = Time.time;
		Globals.CurrentStartTime = Time.time;

		GameStarted?.Invoke(this, null);
	}

	public void StopGame()
	{
		if (IsGameStarted)
		{
			IsGameStarted = false;
			//GameTime = 0f;
			Globals.CurrentStartTime = 0f;

			GameEnding?.Invoke(this, null);
		}
	}
}
