using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
	[SerializeField] GameObject lobbyUI;
	[SerializeField] GameObject inGameUI;
	[SerializeField] GameObject stopUI;
	[SerializeField] GameObject optionUI;
	[SerializeField] TMP_Text timeText;
	[SerializeField] ResultUI resultUI;

	const string TIME_FORMAT = "{0}:{1}";
    public void SetLobby()
	{
		lobbyUI.SetActive(true);
		resultUI.gameObject.SetActive(false);
		inGameUI.SetActive(false);
		optionUI.SetActive(false);
	}

	public void SetIngame()
	{
		inGameUI.SetActive(true);
		lobbyUI.SetActive(false);
		stopUI.SetActive(false);
		resultUI.gameObject.SetActive(false);
	}

	public void PauseGame()
	{
		stopUI.SetActive(true);
	}

	public void ContinueGame()
	{
		stopUI.SetActive(false);
	}


	public void SetResult(int score, int best, Reward[] rewards)
	{
		resultUI.gameObject.SetActive(true);
		StartCoroutine(resultUI.ShowResult(score, best, rewards));
	}

	public void SetOption()
	{
		optionUI.SetActive(true);
	}

	public void UpdateTime(float elapsed)
	{
		int minute = (int)elapsed / 60;
		int second = (int)elapsed % 60;
		timeText.text = string.Format(TIME_FORMAT, minute.ToString("00"), second.ToString("00"));
	}
}
