using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillDescriptionUI : MonoBehaviour
{
    [SerializeField] Image skillImage;
    [SerializeField] TMP_Text skillDescription;

    const string descriptionFormat = "{0}\nCoolTime : {1:0.#}\nDescription : {2}";
    public void SetSkill(Sprite skillSprite, string skillName, float coolTime, string description)
	{
        skillImage.sprite = skillSprite;
        skillDescription.text = string.Format(descriptionFormat, skillName, coolTime, description);
	}
}
