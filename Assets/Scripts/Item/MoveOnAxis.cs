using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveOnAxis : MonoBehaviour
{
	enum Axis
	{
		Horizontal,
		Vertical,
		LeftDiagonal,
		RightDiagonal
	}

	[SerializeField]
	private Axis axis = default;

	[SerializeField]
	private bool loop = default;

	[SerializeField]
	[Tooltip("Only Used When Loop is On")]
	private bool respectItemBounds = default;

	[SerializeField]
	private float speed = default;

	[SerializeField]
	private bool automatic = default;

	private Item item;
	private Vector2 screenBounds;
	private Vector2 gameBounds;

	void OnValidate()
	{
		if (speed < 0f)
			speed = 0f;

		if (!loop)
			respectItemBounds = false;
	}

	void Awake()
	{
		item = GetComponent<Item>();
		screenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

		gameBounds = screenBounds - item.GetRendererExtentBounds();
	}

	void Update()
	{
		if (Camera.main.WorldToViewportPoint(transform.position).y <= 0f)
			Destroy(gameObject);
		else if (automatic && !item.IsDestroyed)
			Move();
	}

	public void Move()
	{
		Vector2 bounds;

		if (respectItemBounds)
			bounds = gameBounds;
		else
			bounds = screenBounds;

		if (axis == Axis.Horizontal)
		{
			var currentT = Mathf.InverseLerp(-bounds.x, bounds.x, transform.position.x);
			var newT = currentT + (speed * Time.deltaTime);

			var newX = Mathf.Lerp(-bounds.x, bounds.x, loop ? Mathf.PingPong(Time.time * speed, 1f) : newT);
			transform.position = new Vector2(newX, transform.position.y);
		}
		else if (axis == Axis.Vertical)
		{
			var currentT = Mathf.InverseLerp(bounds.y, -bounds.y, transform.position.y);
			var newT = currentT + (speed * Time.deltaTime);

			var newY = Mathf.Lerp(bounds.y, -bounds.y, loop ? Mathf.PingPong(Time.time * speed, 1f) : newT);
			transform.position = new Vector2(transform.position.x, newY);
		}
		else if (axis == Axis.LeftDiagonal)
		{
			// TODO
			throw new NotImplementedException();
		}
		else if (axis == Axis.RightDiagonal)
		{
			// TODO
			throw new NotImplementedException();
		}
	}
}
