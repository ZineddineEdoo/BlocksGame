using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Item : MonoBehaviour
{
	public event EventHandler<Collision2D> ItemCollided;
	public event EventHandler<Vector2> ItemTriggering;
	
	[SerializeField]
	private float maxBonus = default;

	private Collider2D bonusCollider;

	public bool IsDestroyed { get; private set; }

	void Awake()
	{
		bonusCollider = GetComponentsInChildren<Collider2D>().FirstOrDefault(c => c.isTrigger);
	}

	public Vector2 GetRendererExtentBounds()
	{
		return GetComponentInChildren<Renderer>().bounds.extents;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		ItemCollided?.Invoke(this, collision);
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		// TODO Maybe remove check here
		if (collision.gameObject.CompareTagRecursively("Player"))
			ItemTriggering?.Invoke(this, collision.transform.position);		
	}

	public IEnumerator DestroyGameObject()
	{
		IsDestroyed = true;

		var animator = GetComponentInChildren<Animator>();
		if (animator != null)
		{
			animator.SetBool("FadeOut", true);

			yield return new WaitUntil(() => GetComponentInChildren<AnimationEventsManager>().FadedOut);
		}

		if (gameObject != null)
			Destroy(gameObject);
	}

	public float CalcBonus(Vector2 position)
	{
		float bonus;

		var xRelative = transform.InverseTransformPoint(position).x;
		var extents = bonusCollider.bounds.extents;
		var t = Mathf.InverseLerp(-extents.x, extents.x, xRelative);

		var amount = Mathf.Clamp(t > 0.5f ? 1 - t : t, 0.1f, 0.5f);
		bonus = (amount / 0.5f) * maxBonus;

		return bonus;
	}
}
