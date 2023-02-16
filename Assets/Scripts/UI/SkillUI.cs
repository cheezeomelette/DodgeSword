using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SkillUI : MonoBehaviour
{
	[SerializeField] Image coolTime;

	public void UpdateUI(float current, float max)
	{
		// image type�� filled���� �ؼ� ������ �����Ѵ�.
		coolTime.fillAmount = current / max;
	}
}
