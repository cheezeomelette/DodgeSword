using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SkillUI : MonoBehaviour
{
	[SerializeField] Image coolTime;

	public void UpdateUI(float current, float max)
	{
		// image type을 filled모드로 해서 비율을 조절한다.
		coolTime.fillAmount = current / max;
	}
}
