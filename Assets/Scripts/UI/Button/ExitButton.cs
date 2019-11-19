using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour
{
	void Awake()
	{
		GetComponent<Button>().onClick.AddListener(() => SceneManager.Exit());
	}
}
