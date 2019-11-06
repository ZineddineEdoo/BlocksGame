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

		Globals.ScoreUpdated += (s, score) => CalculateForScore();
	}

	private void CalculateForScore()
	{
		// Every Achievement Not In CurrentSaveData
		foreach (var achievement in Loader.Achievements.Where(a => !SaveManager.CurrentSaveData.Achievements.Any(i => i == a.ID)))
		{
			if (achievement.ID == AchievementID.Score1K && Globals.Score >= Globals.THOUSAND)
				ShowAchievementComplete(AchievementID.Score1K);
			else if (achievement.ID == AchievementID.Score10K && Globals.Score >= 10 * Globals.THOUSAND)
				ShowAchievementComplete(AchievementID.Score10K);
			else if (achievement.ID == AchievementID.Score100K && Globals.Score >= 100 * Globals.THOUSAND)
				ShowAchievementComplete(AchievementID.Score100K);
			else if (achievement.ID == AchievementID.Score1M && Globals.Score >= 1 * Globals.MILLION)
				ShowAchievementComplete(AchievementID.Score1M);
			else if (achievement.ID == AchievementID.Score10M && Globals.Score >= 10 * Globals.MILLION)
				ShowAchievementComplete(AchievementID.Score10M);
			else if (achievement.ID == AchievementID.Score100M && Globals.Score >= 100 * Globals.MILLION)
				ShowAchievementComplete(AchievementID.Score100M);
			else if (achievement.ID == AchievementID.ScoreN1K && Globals.Score <= -1 * Globals.THOUSAND)
				ShowAchievementComplete(AchievementID.ScoreN1K);
			else if (achievement.ID == AchievementID.ScoreN10K && Globals.Score <= -10 * Globals.THOUSAND)
				ShowAchievementComplete(AchievementID.ScoreN10K);
			else if (achievement.ID == AchievementID.ScoreN100K && Globals.Score <= -100 * Globals.THOUSAND)
				ShowAchievementComplete(AchievementID.ScoreN100K);
			else if (achievement.ID == AchievementID.ScoreN1M && Globals.Score <= -1 * Globals.MILLION)
				ShowAchievementComplete(AchievementID.ScoreN1M);
		}
	}

	private void CalculateForBonus()
	{
		foreach (var achievement in Loader.Achievements.Where(a => !SaveManager.CurrentSaveData.Achievements.Any(i => i == a.ID)))
		{
			// TODO All Achievements for Bonus
		}
	}

	private void ShowAchievementComplete(AchievementID id)
	{
		SaveManager.CurrentSaveData.SetAchievement(id);

		AchievementCompleted?.Invoke(this, Loader.Achievements.First(a => a.ID == id));

		SaveManager.Save();
	}
}
