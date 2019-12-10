using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Achievement;

public class AchievementManager : MonoBehaviour
{
	public event EventHandler<Achievement> AchievementCompleted;

	public static AchievementManager Instance { get; private set; }

	public List<Achievement> Achievements { get; private set; }

	void Awake()
	{
		if (Instance != null)
			Destroy(this);
		else
		{
			Instance = this;

			Achievements = new List<Achievement>();
			GetComponent<StringManager>().Loaded += (s, e) => LoadAchievements();

			Globals.ScoreUpdated += (s, score) => CalculateForScore();
		}
	}

	private void LoadAchievements()
	{
		foreach (AchievementID id in Enum.GetValues(typeof(AchievementID)))
		{
			var (Name, Detail) = GetNameDetail(id);

			Achievements.Add(new Achievement()
			{
				ID = id,
				Name = Name,
				Detail = Detail
			});
		}
	}

	private (string Name, string Detail) GetNameDetail(AchievementID id)
	{
		string name;
		string detail;

		if (id >= SCORE_START && id < BONUS_START)
		{
			string val;

			if (id == AchievementID.Score1K)
				val = "1K";
			else if (id == AchievementID.Score10K)
				val = "10K";
			else if (id == AchievementID.Score100K)
				val = "100K";
			else if (id == AchievementID.Score1M)
				val = "1M";
			else if (id == AchievementID.Score10M)
				val = "10M";
			else if (id == AchievementID.Score100M)
				val = "100M";
			else if (id == AchievementID.ScoreN1K)
				val = "-1K";
			else if (id == AchievementID.ScoreN10K)
				val = "-10K";
			else if (id == AchievementID.ScoreN100K)
				val = "-100K";
			else if (id == AchievementID.ScoreN1M)
				val = "-1M";
			else
				throw new ArgumentException($"Invalid Score Achievement ID {id}");

			name = String.Format(StringManager.Instance.Strings["Score"], val);
			detail = String.Format(StringManager.Instance.Strings["GetScoreOfSingleGame"], val);
		}
		else if (id >= BONUS_START)
		{
			name = "";
			detail = "";
		}
		else
			throw new ArgumentException($"Invalid Achievement ID {id}");

		return (name, detail);
	}
	
	private void CalculateForScore()
	{
		// Every Achievement Not In CurrentSaveData
		foreach (var achievement in Achievements.Where(a => !SaveManager.CurrentSaveData.Achievements.Any(i => i == a.ID)))
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
		foreach (var achievement in Achievements.Where(a => !SaveManager.CurrentSaveData.Achievements.Any(i => i == a.ID)))
		{
			// TODO All Achievements for Bonus
		}
	}

	private void ShowAchievementComplete(AchievementID id)
	{
		SaveManager.CurrentSaveData.SetAchievement(id);

		AchievementCompleted?.Invoke(this, Achievements.First(a => a.ID == id));

		SaveManager.Save();
	}
}
