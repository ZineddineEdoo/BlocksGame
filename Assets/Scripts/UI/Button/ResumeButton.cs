using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ResumeButton : MonoBehaviour
{
	void Awake()
	{
		GetComponent<Button>().onClick.AddListener(() => 
		{
			FindObjectOfType<SceneController>().FadeOutScene(GetComponentInParent<Animator>());
		});
	}
}
