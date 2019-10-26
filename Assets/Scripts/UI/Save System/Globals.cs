using System;
using System.Collections.Generic;
using System.Linq;

public static class Globals
{
	public static float Score { get; set; }

	// Will Load SaveFile
	public static float PreviousHighScore { get; set; } = SaveManager.CurrentSaveData.HighScore;
}
