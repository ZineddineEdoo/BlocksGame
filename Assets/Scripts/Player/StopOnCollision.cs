using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StopOnCollision : MonoBehaviour
{
	private GameManager gameManager;

	private void Awake()
	{
		gameManager = GetComponent<Player>().GameManager;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Enemy"))
		{
			gameManager.StopGame();
		}
	}
}
