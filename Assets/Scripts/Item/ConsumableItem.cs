using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConsumableItem : Item
{
	private bool isConsumed;

	protected override void RaiseItemTrigger(Vector2 position)
	{		
		if (!isConsumed)
		{
			isConsumed = true;
			base.RaiseItemTrigger(position);
		}
	}
}
