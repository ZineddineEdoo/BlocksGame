using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Conditional : MonoBehaviour
{
	[SerializeField]
	private GameObject androidGameObject = default;

	[SerializeField]
	private GameObject uwpGameObject = default;

	void OnValidate()
	{
#if UNITY_ANDROID
		ActivateAndroid();
#elif UNITY_WSA
		ActivateUWP();
#endif
	}

	private void ActivateAndroid()
	{
		androidGameObject.SetActive(true);
		uwpGameObject.SetActive(false);
	}

	private void ActivateUWP()
	{
		androidGameObject.SetActive(false);
		uwpGameObject.SetActive(true);
	}
}
