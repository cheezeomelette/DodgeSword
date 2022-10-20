using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
	[SerializeField] Image skillImage;

	public void SkiilAmount(float current, float max)
	{
		skillImage.fillAmount = current / max;
	}
}
