using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class MonoBehaviourExtension
{
	private static Dictionary<Type, Coroutine> coroutines;

	public static Coroutine RestartCoroutine(this MonoBehaviour monoBehaviour, IEnumerator routine)
	{
		var routineType = routine.GetType();

		if (coroutines == null)
			coroutines = new Dictionary<Type, Coroutine>();
		
		if (coroutines.ContainsKey(routineType))
		{
			if (coroutines[routineType] != null)
				monoBehaviour.StopCoroutine(coroutines[routineType]);

			coroutines.Remove(routineType);
		}

		var coroutine = monoBehaviour.StartCoroutine(routine);
		coroutines.Add(routineType, coroutine);

		return coroutine;
	}

	public static void RemoveCoroutine(this MonoBehaviour monoBehaviour, IEnumerator routine)
	{
		if (coroutines != null)
			coroutines.Remove(routine.GetType());
	}
}
