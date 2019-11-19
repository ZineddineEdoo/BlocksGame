using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static OverlayManager;

/// <summary>
/// Requires DisplayOverlay Component in Parent Hierarchy
/// </summary>
public class OverlayButton : MonoBehaviour
{
	[SerializeField]
	private Result buttonType = default;

	void Awake()
	{
		GetComponent<Button>().onClick.AddListener(() =>
		{
			if (buttonType == Result.OK)
				GetComponentInParent<DisplayOverlay>().OnOkSelected();
			else
				GetComponentInParent<DisplayOverlay>().OnCancelSelected();
		});
	}
}
