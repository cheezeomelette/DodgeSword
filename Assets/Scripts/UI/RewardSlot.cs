using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardSlot : MonoBehaviour
{
	[SerializeField] Image rewardImage;
	[SerializeField] Image frameImage;
	[SerializeField] Image frameBackImage;
	[SerializeField] TMP_Text countText;

	const string rewardFormat = "Reward/{0}";
	const string frameFormat = "Reward/Frame{0}";
	const string frameBackFormat = "Reward/FrameBack{0}";
    public void SetRewardSlot(Reward reward)
	{
		rewardImage.sprite = Resources.Load<Sprite>(string.Format(rewardFormat, reward.name));
		frameImage.sprite = Resources.Load<Sprite>(string.Format(frameFormat, reward.type.ToString()));
		frameBackImage.sprite = Resources.Load<Sprite>(string.Format(frameBackFormat, reward.type.ToString()));
		countText.text = reward.count.ToString();
	}
}
