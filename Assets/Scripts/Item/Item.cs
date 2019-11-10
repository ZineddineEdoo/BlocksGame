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

	[SerializeField]
	private bool isFixedBonus = default;

	private Collider2D bonusCollider;

	public bool IsDestroyed { get; private set; }
	public float MaxBonus { get => maxBonus; set => maxBonus = value; }

	void Awake() => OnAwake();

	protected virtual void OnAwake()
	{
		bonusCollider = GetComponentsInChildren<Collider2D>().FirstOrDefault(c => c.isTrigger);
	}

	public Vector2 GetRendererExtentBounds() =>	GetComponentInChildren<Renderer>().bounds.extents;

	private void OnCollisionEnter2D(Collision2D collision)
	{
		ItemCollided?.Invoke(this, collision);
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		// TODO Maybe remove check here
		if (collision.gameObject.CompareTagRecursively("Player"))
			RaiseItemTrigger(collision.transform.position);
	}

	protected virtual void RaiseItemTrigger(Vector2 position)
	{		
		ItemTriggering?.Invoke(this, position);
	}

	public IEnumerator DestroyGameObject()
	{
		IsDestroyed = true;

		var animator = GetComponentInChildren<Animator>();
		if (animator != null && animator.parameters.FirstOrDefault(p => p.name == "FadeOut") != default)
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

		if (isFixedBonus)
			bonus = maxBonus;
		else
		{
			var xRelative = transform.InverseTransformPoint(position).x;
			var extents = bonusCollider.bounds.extents;
			var t = Mathf.InverseLerp(-extents.x, extents.x, xRelative);

			var amount = Mathf.Clamp(t > 0.5f ? 1 - t : t, 0.1f, 0.5f);
			bonus = (amount / 0.5f) * maxBonus;
		}

		return bonus;
	}
}
