using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
