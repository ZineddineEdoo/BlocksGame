using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SceneAnimationEventsManager : AnimationEventsManager
{
#if UNITY_EDITOR
	void Awake()
	{
		if (FindObjectsOfType<SceneAnimationEventsManager>().Where(m => m.gameObject.scene == gameObject.scene).Count() > 1)
			Debug.LogError("Only One Scene Animation Events Manager Is Allowed Per Scene");
	}
#endif
}
