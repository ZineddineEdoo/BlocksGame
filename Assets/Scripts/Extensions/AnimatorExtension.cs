using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class AnimatorExtension
{
	/// <summary>
	/// Checks Parameter Exists Before Setting The Value
	/// </summary>
	/// <param name="animator"></param>
	/// <param name="name"></param>
	/// <param name="value"></param>
	public static void SetBoolSafe(this Animator animator, string name, bool value)
	{
		if (animator.parameters.FirstOrDefault(p => p.name == name) != default)
			animator.SetBool(name, value);
	}
}
