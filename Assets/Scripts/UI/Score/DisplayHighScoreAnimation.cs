using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Requires Animator Component in Parent Hierarchy
/// </summary>
public class DisplayHighScoreAnimation : MonoBehaviour
{
	void Awake()
	{
		if (Globals.Score > Globals.PreviousHighScore)
			GetComponentInParent<Animator>().SetBool("IsNewHighScore", true);
	}
}
