using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ResultUI : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;
    [SerializeField] Transform rewardTransform;
    [SerializeField] RewardSlot prefab;

    const string scoreFormat = "Score : {0}\nBest : {1}";
	public IEnumerator ShowResult(int score, int best, Reward[] rewards)
    {
        foreach (Transform child in rewardTransform)
        {
            Destroy(child.gameObject) ;
        }
        yield return SetScore(score, best);
		yield return new WaitForSeconds(0.5f);
        yield return SetReward(rewards);
	}

    IEnumerator SetScore(int score, int best)
	{
        int currentScore = 0;
        while(currentScore < score)
		{
            currentScore += (int)(score * Time.deltaTime * 2f);
            scoreText.text = string.Format(scoreFormat, currentScore.ToString(), best.ToString());
            yield return null;
		}
        scoreText.text = string.Format(scoreFormat, score.ToString(), best.ToString());
    }
    IEnumerator SetReward(Reward[] rewards)
	{
        foreach(Reward reward in rewards)
		{
            RewardSlot slot = Instantiate(prefab, rewardTransform);
            slot.SetRewardSlot(reward);
            yield return new WaitForSeconds(0.5f);
		}
	}
}
