using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if !INSTANT
#pragma warning disable 0414
#endif

public class TrialDisplay : MonoBehaviour
{
	[SerializeField]
	private RectTransform trialPanel = default;

#if INSTANT
	void Awake()
	{
		trialPanel.gameObject.SetActive(true);
	}
#endif
}

#if !INSTANT
#pragma warning restore 0414
#endif