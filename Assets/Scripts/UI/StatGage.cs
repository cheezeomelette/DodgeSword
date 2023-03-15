using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatGage : MonoBehaviour
{
    [SerializeField] Image filledImage;
	[SerializeField] TMP_Text gageRatioText;
    public void SetGage(float current, float max)
	{
		filledImage.fillAmount = current / max;
		gageRatioText.text = string.Format("{0:0.#} / {1:0.#}", current, max);
	}
}
