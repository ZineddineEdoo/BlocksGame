using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class OverlayManager : MonoBehaviour
{
	public event EventHandler<(string Message, Action<Result> ResultCallback)> OverlayRequested;

	public static OverlayManager Instance { get; private set; }

	public enum Result
	{
		OK,
		Cancel
	}

	void Awake()
	{
		if (Instance != null)
			Destroy(this);
		else
			Instance = this;
	}

	public void ShowOverlay(string message, Action<Result> resultCallback)
	{
		OverlayRequested?.Invoke(this, (message, resultCallback));
	}
}
