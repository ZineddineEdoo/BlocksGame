using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
	void Awake()
	{
		GetComponent<Button>().onClick.AddListener(() => SceneManager.Instance.PauseGame());
	}
}
