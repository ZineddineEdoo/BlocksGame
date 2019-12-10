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
			Globals.BonusUpdated += (s, bonus) => CalculateForBonus();
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
		(string Name, string Detail) nameDetail;

		if (id >= SCORE_START && id < BONUS_START)
			nameDetail = GetScoreNameDetail(id);
		else if (id >= BONUS_START)
			nameDetail = GetBonusNameDetail(id);
		else
			throw new ArgumentException($"Invalid Achievement ID {id}");

		return nameDetail;
	}

	private (string Name, string Detail) GetScoreNameDetail(AchievementID id)
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

		var name = string.Format(StringManager.Instance.Strings["Score"], val);
		var detail = string.Format(StringManager.Instance.Strings["GetScoreOfSingleGame"], val);

		return (name, detail);
	}

	private (string Name, string Detail) GetBonusNameDetail(AchievementID id)
	{
		string val;

		if (id == AchievementID.Bonus1K)
			val = "1K";
		else if (id == AchievementID.Bonus10K)
			val = "10K";
		else if (id == AchievementID.Bonus100K)
			val = "100K";
		else if (id == AchievementID.Bonus1M)
			val = "1M";
		else if (id == AchievementID.Bonus10M)
			val = "10M";
		else if (id == AchievementID.Bonus100M)
			val = "100M";
		else if (id == AchievementID.BonusN1K)
			val = "-1K";
		else if (id == AchievementID.BonusN10K)
			val = "-10K";
		else if (id == AchievementID.BonusN100K)
			val = "-100K";
		else if (id == AchievementID.BonusN1M)
			val = "-1M";
		else
			throw new ArgumentException($"Invalid Score Achievement ID {id}");

		var name = string.Format(StringManager.Instance.Strings["ScoreBonus"], val);
		var detail = string.Format(StringManager.Instance.Strings["GetBonusOfSingleGame"], val);

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
			if (achievement.ID == AchievementID.Bonus1K && Globals.Bonus >= Globals.THOUSAND)
				ShowAchievementComplete(AchievementID.Bonus1K);
			else if (achievement.ID == AchievementID.Bonus10K && Globals.Bonus >= 10 * Globals.THOUSAND)
				ShowAchievementComplete(AchievementID.Bonus10K);
			else if (achievement.ID == AchievementID.Bonus100K && Globals.Bonus >= 100 * Globals.THOUSAND)
				ShowAchievementComplete(AchievementID.Bonus100K);
			else if (achievement.ID == AchievementID.Bonus1M && Globals.Bonus >= 1 * Globals.MILLION)
				ShowAchievementComplete(AchievementID.Bonus1M);
			else if (achievement.ID == AchievementID.Bonus10M && Globals.Bonus >= 10 * Globals.MILLION)
				ShowAchievementComplete(AchievementID.Bonus10M);
			else if (achievement.ID == AchievementID.Bonus100M && Globals.Bonus >= 100 * Globals.MILLION)
				ShowAchievementComplete(AchievementID.Bonus100M);
			else if (achievement.ID == AchievementID.BonusN1K && Globals.Bonus <= -1 * Globals.THOUSAND)
				ShowAchievementComplete(AchievementID.BonusN1K);
			else if (achievement.ID == AchievementID.BonusN10K && Globals.Bonus <= -10 * Globals.THOUSAND)
				ShowAchievementComplete(AchievementID.BonusN10K);
			else if (achievement.ID == AchievementID.BonusN100K && Globals.Bonus <= -100 * Globals.THOUSAND)
				ShowAchievementComplete(AchievementID.BonusN100K);
			else if (achievement.ID == AchievementID.BonusN1M && Globals.Bonus <= -1 * Globals.MILLION)
				ShowAchievementComplete(AchievementID.BonusN1M);
		}
	}

	private void ShowAchievementComplete(AchievementID id)
	{
		SaveManager.CurrentSaveData.SetAchievement(id);

		AchievementCompleted?.Invoke(this, Achievements.First(a => a.ID == id));

		SaveManager.Save();
	}
}
