using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
	[SerializeField] private TMP_Text scoreText;
	[SerializeField] private Fillbar healthBar;

	public void SetHealth(float percent)
	{
		healthBar.SetPercentage(percent);
	}

	public void SetScore(int score)
	{
		scoreText.text = $"Kills: {score}";
	}
}
