using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField]
	private GameManager gameManager = default;

	private Vector3 startPos;

	public bool CanMove { get; set; }

	public GameManager GameManager => gameManager;

	void Awake()
	{
		startPos = transform.position;

		gameManager.GameStarting += (s, e) => Initialise();
		gameManager.GameStarted += (s, e) => CanMove = true;
		gameManager.GameEnding += (s, e) => Hide();

		GetComponentInChildren<Animator>().SetFloat("Speed", 2f);
	}

	public void Initialise()
	{
		GetComponentInChildren<Animator>().SetBool("FadeIn", true);
	}

	public void Hide()
	{
		CanMove = false;

		GetComponentInChildren<Animator>().SetBool("FadeIn", false);

		this.RestartCoroutine(ResetPositionDelayed(), nameof(ResetPositionDelayed));
	}

	private IEnumerator ResetPositionDelayed()
	{
		var animEventsManager = GetComponentInChildren<AnimationEventsManager>();

		yield return new WaitUntil(() => animEventsManager.FadedOut);

		transform.position = startPos;

		this.RemoveCoroutine(nameof(ResetPositionDelayed));
	}
}
