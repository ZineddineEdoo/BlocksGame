using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class MonoBehaviourExtension
{
	private static Dictionary<string, Coroutine> coroutines;

	/// <summary>
	/// Finds if a Coroutine associated with "name" is Stored. <br/>
	/// If found, the Coroutine is Stopped.<br/><br/>
	/// A Coroutine for "routine" is Started and Stored with "name" as Key.
	/// </summary>
	/// <param name="monoBehaviour"></param>
	/// <param name="routine">The Routine to Start</param>
	/// <param name="name">Must be Unique for this MonoBehavior</param>
	/// <returns></returns>
	public static Coroutine RestartCoroutine(this MonoBehaviour monoBehaviour, IEnumerator routine, string name)
	{		
		if (coroutines == null)
			coroutines = new Dictionary<string, Coroutine>();
		
		if (coroutines.ContainsKey(name))
		{
			if (coroutines[name] != null)
				monoBehaviour.StopCoroutine(coroutines[name]);

			coroutines.Remove(name);
		}

		var coroutine = monoBehaviour.StartCoroutine(routine);
		coroutines.Add(name, coroutine);		

		return coroutine;
	}

	/// <summary>
	/// Removes a Coroutine Reference Without Stopping it
	/// </summary>
	/// <param name="monoBehaviour"></param>
	/// <param name="routineName">Typically, nameof(Method) or nameof(Method) + "2"</param>
	public static void RemoveCoroutine(this MonoBehaviour monoBehaviour, string routineName)
	{
		if (coroutines != null)
			coroutines.Remove(routineName);
	}
}
