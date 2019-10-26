using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StopOnFall : MonoBehaviour
{
	private GameManager gameManager;

	void Awake()
	{
		gameManager = GetComponent<Player>().GameManager;
	}

	void Update()
	{
		if (Camera.main.WorldToViewportPoint(transform.position).y <= 0f)
		{
			gameManager.StopGame();
		}
	}
}
