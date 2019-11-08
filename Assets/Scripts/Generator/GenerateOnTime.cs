using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GenerateOnTime : Generate
{
	[SerializeField]
	private float minTime = default;

	[SerializeField]
	private float maxTime = default;

	private float lastGenerationTime;
	private float randomDelay;
	private ScoreManager scoreManager;

	void OnValidate()
	{
		if (minTime < 0f)
			minTime = 0f;

		if (maxTime < minTime)
			maxTime = minTime + 1f;
	}

	protected override void OnAwake()
	{
		base.OnAwake();

		scoreManager = gameManager.GetComponent<ScoreManager>();
	}

	protected override void Initialise()
	{
		lastGenerationTime = 0f;
		randomDelay = UnityEngine.Random.Range(minTime, maxTime);
	}

	void Update()
	{
		if (gameManager.IsGameStarted && Time.time >= lastGenerationTime + randomDelay)
		{
			var item = generator.GenerateNext();
			if (item != null)
			{
				item.ItemTriggering += Item_ItemTriggering;

				lastGenerationTime = Time.time;
			}

			randomDelay = UnityEngine.Random.Range(minTime, maxTime);
		}
	}

	private void Item_ItemTriggering(object sender, Vector2 position)
	{
		if (sender is ConsumableItem consumableItem)
		{
			var bonus = consumableItem.CalcBonus(position);

			scoreManager.AddBonus(position, bonus, true);
			StartCoroutine(consumableItem.DestroyGameObject());
		}
		else if (sender is Item item)
		{
			var bonus = item.CalcBonus(position);

			scoreManager.AddBonus(position, bonus * Time.deltaTime);
		}
	}
}
