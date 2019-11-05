using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using static AchievementLoader;
using static AchievementLoader.Achievement;

public class AchievementManager : MonoBehaviour
{
	public event EventHandler<Achievement> AchievementCompleted;

	public AchievementLoader Loader { get; private set; }

	void Awake()
	{
		Loader = new AchievementLoader();

		SceneManager.LoadSceneAsync(SceneChanger.MAIN_MENU_SCENE);
	}

	void Update()
	{
		Calculate();
	}

	private void Calculate()
	{
		// Every Achievement Not In CurrentSaveData
		foreach (var achievement in Loader.Achievements.Where(a => !SaveManager.CurrentSaveData.Achievements.Any(i => i == a.ID)))
		{
			if (achievement.ID == AchievementID.Score1K && Globals.Score >= Globals.THOUSAND)
				ShowAchievementComplete(AchievementID.Score1K);
			else if (achievement.ID == AchievementID.Score10K && Globals.Score >= 10 * Globals.THOUSAND)
				ShowAchievementComplete(AchievementID.Score10K);
			// TODO All the Other Achievements
		}
	}

	private void ShowAchievementComplete(AchievementID id)
	{
		SaveManager.CurrentSaveData.SetAchievement(id);

		AchievementCompleted?.Invoke(this, Loader.Achievements.First(a => a.ID == id));

		SaveManager.Save();
	}
}
