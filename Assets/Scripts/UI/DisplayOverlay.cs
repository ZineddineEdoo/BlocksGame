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

	[SerializeField]
	private TextMeshProUGUI okButtonText = default;

	[SerializeField]
	private TextMeshProUGUI cancelButtonText = default;

	private Result? result;

	void Awake()
	{
		overlayManager.OverlayRequested += OverlayManager_OverlayRequested;
	}

	private void OverlayManager_OverlayRequested(object sender, (string Message, ActionOptions ActionOptions, Action<Result> ResultCallback) e)
	{
		messageText.SetText(e.Message);

		if (e.ActionOptions == ActionOptions.YesNo)
		{
			okButtonText.SetText("Yes");
			cancelButtonText.SetText("No");
		}
		else if (e.ActionOptions == ActionOptions.OkCancel)
		{
			okButtonText.SetText("OK");
			cancelButtonText.SetText("Cancel");
		}

		this.RestartCoroutine(ShowOverlay(e.ResultCallback), nameof(ShowOverlay));
	}

	private IEnumerator ShowOverlay(Action<Result> resultCallback)
	{
		result = null;

		GetComponentInChildren<Animator>().SetBool("FadeIn", true);

		yield return new WaitUntil(() => result != null);

		resultCallback?.Invoke((Result)result);
		this.RemoveCoroutine(nameof(ShowOverlay));
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
