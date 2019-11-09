using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Generator : MonoBehaviour
{
	[Serializable]
	public class ItemPrefab
	{
		public Item item;
		[Range(0, 100)]
		public int weight;
	}	

	[Serializable]
	public class Difficulty
	{
		public List<int> indexWeights;
	}

	public event EventHandler<Item> ItemGenerated;

	private const int MAX_TRIES = 10;

	#region Inspector Variables
	[SerializeField]
	[Tooltip("X Axis OR Y Axis Must be 0")]
	private bool generateOutside = default;

	[SerializeField]
	[Range(0, 1)]
	private float xAxis = default;

	[SerializeField]
	[Range(0, 1)]
	private float yAxis = default;

	[SerializeField]
	[Tooltip("If True, X Axis is not used")]
	private bool randomX = default;

	[SerializeField]
	[Tooltip("If True, Y Axis is not used")]
	private bool randomY = default;

	[SerializeField]
	[Tooltip("Only used for Random X")]
	[Range(0.1f, 1)]
	private float xLimit = default;

	[SerializeField]
	[Tooltip("Only used for Random Y")]
	[Range(0.1f, 1)]
	private float yLimit = default;

	[SerializeField]
	private List<ItemPrefab> itemPrefabs = default;

	[SerializeField]
	private List<Difficulty> difficulties = default;
	#endregion

	private Vector2 screenBounds;
	private List<Item> generatedItems;

	public IReadOnlyList<Item> GeneratedItems => generatedItems;
	public void RemoveNullGeneratedItems() => generatedItems.RemoveAll(c => c == null);

	void OnValidate()
	{
		if (xAxis > 0f && yAxis > 0f)
			generateOutside = false;

		if (xLimit < 0.1f)
			xLimit = 0.1f;

		if (yLimit < 0.1f)
			yLimit = 0.1f;

		if (itemPrefabs != default && difficulties.Count > 0)
		{
			for (int i = 0; i < difficulties.Count; i++)
			{
				if (difficulties[i].indexWeights.Count != itemPrefabs.Count)
				{
					difficulties[i].indexWeights.Clear();
					difficulties[i].indexWeights.AddRange(itemPrefabs.Select(ip => ip.weight));
				}
			}
		}
	}

	void Awake()
	{
		generatedItems = new List<Item>();
		screenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
	}

	public void DeleteItems()
	{
		foreach (var item in generatedItems.Where(i => i != null))
		{
			StartCoroutine(item.DestroyGameObject());
		}

		generatedItems.Clear();
	}

	public void SetDifficulty(int level)
	{		
		level = Mathf.Clamp(level, 0, difficulties.Count - 1);

		for (int i = 0; i < itemPrefabs.Count; i++)
		{
			itemPrefabs[i].weight = difficulties[level].indexWeights[i];
		}
	}

	public Item GenerateNext()
	{
		Item item = null;
		var part = 1f / itemPrefabs.Sum(i => i.weight);
		var totalLeft = 1f;
		var randomVal = UnityEngine.Random.value;

		foreach (var itemPrefab in itemPrefabs.OrderByDescending(i => i.weight))
		{
			totalLeft -= part * itemPrefab.weight;

			if (randomVal >= totalLeft)
			{
				var position = GetEmptyPosition(itemPrefab.item);
				if (position != null)
				{
					item = Instantiate(itemPrefab.item, (Vector2)position, Quaternion.identity, transform);

					generatedItems.Add(item);
				}
				break;
			}
		}

		RemoveNullGeneratedItems();
		
		if (item != null)
			ItemGenerated?.Invoke(this, item);

		return item;
	}

	private Vector2? GetEmptyPosition(Item item)
	{
		Vector2? position = null;

		// To prevent Instantiating outside Screen Bounds
		var gameBounds = screenBounds - item.GetRendererExtentBounds();

		if (generateOutside)
		{
			if (xAxis == 0f || xAxis == 1f)
				gameBounds.x -= item.GetRendererExtentBounds().x;
			else if (yAxis == 0f || yAxis == 1f)
				gameBounds.y += item.GetRendererExtentBounds().y;
		}

		var testPos = new Vector2();
		int tries = 0;
		do
		{
			if (randomX)
				xAxis = UnityEngine.Random.value % xLimit;
			if (randomY)
				yAxis = UnityEngine.Random.value % yLimit;

			float xPos = Mathf.Lerp(-gameBounds.x, gameBounds.x, xAxis);
			float yPos = Mathf.Lerp(gameBounds.y, -gameBounds.y, yAxis);

			testPos.Set(xPos, yPos);
			
			tries++;
		//} while (!IsPositionClear(position, item.GetComponentInChildren<Renderer>().bounds.size));
		} while (tries < MAX_TRIES && Physics2D.OverlapBox(testPos, item.GetComponentInChildren<Renderer>().bounds.size, 0) != null);
		
		if (tries < MAX_TRIES)
			position = testPos;

		return position;
	}
	
	// TODO-Delete
	private bool IsPositionClear(Vector2 position, Vector2 size)
	{
		bool isClear = true;

		if (Physics2D.OverlapBox(position, size, 0) != null)
			isClear = false;

		//var colliders = Physics2D.OverlapAreaAll(new Vector2(-screenBounds.x, screenBounds.y), new Vector2(screenBounds.x, -screenBounds.y));

		//foreach (var collider in colliders)
		//{
		//	var bounds = new Bounds(position, size);

		//	if (collider.bounds.Intersects(bounds))
		//	{
		//		isClear = false;
		//		break;
		//	}
		//}

		return isClear;
	}
}
