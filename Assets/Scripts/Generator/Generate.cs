using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Generate : MonoBehaviour
{
	[SerializeField]
	protected GameManager gameManager = default;

	protected Generator generator;

	void Awake()
	{
		OnAwake();		
	}

	protected virtual void OnAwake()
	{
		generator = GetComponent<Generator>();
		if (generator == null)
			Debug.LogError($"{nameof(Generator)} Component must be on this GameObject");

		gameManager.GameStarting += (s, e) => Initialise();
		gameManager.GameEnding += (s, e) => generator.DeleteItems();
	}

	protected abstract void Initialise();
}
