using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SkillUI : MonoBehaviour
{
	[SerializeField] Image skillImage;
	[SerializeField] Image coolTime;

	System.Action OnSkill;

	public void SetSkill(System.Action onSkill, Sprite skillSprite)
	{
		OnSkill = onSkill;
		skillImage.sprite = skillSprite;
	}
	public void UpdateUI(float current, float max)
	{
		// image type을 filled모드로 해서 비율을 조절한다.
		coolTime.fillAmount = current / max;
	}
	public void OnPointerClick()
	{
		OnSkill?.Invoke();
	}
}
