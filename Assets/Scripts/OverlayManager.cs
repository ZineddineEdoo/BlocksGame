using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class OverlayManager : MonoBehaviour
{
	public enum Result
	{
		OK,
		Cancel
	}

	[SerializeField]
	private TextMeshProUGUI messageText = default;

	private Result? result;

	// Need to prevent multiple calls
	public IEnumerator ShowOverlay(string message, Action<Result> resultCallback)
	{
		result = null;
		messageText.SetText(message);

		GetComponentInChildren<Animator>().SetBool("FadeIn", true);

		yield return new WaitUntil(() => result != null);

		resultCallback?.Invoke((Result) result);
	}

	private void HideOverlay()
	{
		GetComponentInChildren<Animator>().SetBool("FadeIn", false);

		//messageText.SetText("");
	}

	public void OnOkSelected()
	{
		result = Result.OK;
		HideOverlay();
	}

	public void OnCancelSelected()
	{
		result = Result.Cancel;
		HideOverlay();
	}
}
