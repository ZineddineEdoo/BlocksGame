using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerSideTapMovement : MonoBehaviour
{
	[SerializeField]
	private float speed = default;

	[SerializeField]
	[Range(0f, 1f)]
	[Tooltip("Max Screen.Height For Input")]
	private float maxScreenHeight = default;

	private Player player;
	private Vector2 gameBounds;
	private float halfWidth;

	void Start()
	{
		player = GetComponent<Player>();
		gameBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
		halfWidth = GetComponentInChildren<Collider2D>().bounds.extents.x;

		gameBounds.x -= halfWidth;
	}

	void Update()
	{
		if (player.CanMove)
		{
#if UNITY_EDITOR || UNITY_STANDALONE
			GetPCInput();
#else
			GetMobileInput();
#endif
		}
	}

	private void GetMobileInput()
	{
		if (Input.touchCount > 0)
		{
			var touch = Input.GetTouch(0);

			if (touch.position.y <= Screen.height * maxScreenHeight)
			{
				if (touch.position.x >= Screen.width / 2)
					MoveRight();
				else
					MoveLeft();
			}
		}
	}

	private void GetPCInput()
	{
		if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
		{
			if (Input.mousePosition.y <= Screen.height * maxScreenHeight)
			{
				if (Input.mousePosition.x >= Screen.width / 2)
					MoveRight();
				else
					MoveLeft();
			}
		}
		else
		{
			var horizontal = Input.GetAxisRaw("Horizontal");

			if (horizontal >= 1f)
				MoveRight();
			else if (horizontal <= -1f)
				MoveLeft();
		}
	}

	private void MoveRight()
	{
		var newX = Mathf.Clamp(transform.position.x + (speed * Time.deltaTime), -gameBounds.x, gameBounds.x);
		transform.position = new Vector2(newX, transform.position.y);
	}

	private void MoveLeft()
	{
		var newX = Mathf.Clamp(transform.position.x - (speed * Time.deltaTime), -gameBounds.x, gameBounds.x);
		transform.position = new Vector2(newX, transform.position.y);
	}
}
