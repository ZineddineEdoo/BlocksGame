using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GameObjectExtension
{
	public static bool CompareTagRecursively(this GameObject gameObject, string tag) =>
		gameObject.GetComponentsInParent<Transform>().FirstOrDefault(g => g.CompareTag(tag));
}
