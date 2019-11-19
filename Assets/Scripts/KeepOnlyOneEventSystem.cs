using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class KeepOnlyOneEventSystem : MonoBehaviour
{
	void Awake()
	{
		var eventSystems = FindObjectsOfType<EventSystem>();

		if (eventSystems.FirstOrDefault(e => e.gameObject != gameObject))
			Destroy(gameObject);
	}
}
