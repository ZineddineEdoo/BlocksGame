using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static OverlayManager;

public class DisplayOverlay : MonoBehaviour
{
	[SerializeField]
	private OverlayManager overlayManager = default;

	[SerializeField]
	private TextMeshProUGUI messageText = default;
	
	private Result? result;
	private Coroutine overlayCoroutine;

	void Awake()
	{
		overlayManager.OverlayRequested += OverlayManager_OverlayRequested;
	}

	private void OverlayManager_OverlayRequested(object sender, (string Message, Action<Result> ResultCallback) e)
	{
		if (overlayCoroutine != null)
			StopCoroutine(overlayCoroutine);

		overlayCoroutine = StartCoroutine(ShowOverlay(e.Message, e.ResultCallback));
	}

	// Need to prevent multiple calls
	public IEnumerator ShowOverlay(string message, Action<Result> resultCallback)
	{
		result = null;
		messageText.SetText(message);

		GetComponentInChildren<Animator>().SetBool("FadeIn", true);

		yield return new WaitUntil(() => result != null);

		resultCallback?.Invoke((Result)result);
		overlayCoroutine = null;
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
