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

	private Action<Result> resultCallback;

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

		resultCallback = e.ResultCallback;

		GetComponent<AnimationEventsManager>().FadeIn();
	}

	private IEnumerator HideOverlay(Result result)
	{
		GetComponent<AnimationEventsManager>().FadeOut();
		
		yield return new WaitUntil(() => GetComponent<AnimationEventsManager>().FadedOut);

		resultCallback?.Invoke(result);
		//messageText.SetText("");
	}

	public void OnOkSelected() =>
		this.RestartCoroutine(HideOverlay(Result.OK), nameof(HideOverlay));

	public void OnCancelSelected() =>
		this.RestartCoroutine(HideOverlay(Result.Cancel), nameof(HideOverlay));
}
