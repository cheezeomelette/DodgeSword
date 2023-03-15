using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSelectUI : MonoBehaviour
{
	[SerializeField] CharacterManager characterManager;
	[SerializeField] CharacterSlot[] characterSlots;
	[SerializeField] Image characterImage;
	[SerializeField] TMP_Text nameText;
    [SerializeField] StatGage speedGage;
    [SerializeField] StatGage brakeGage;
	[SerializeField] HpUI hpUI;
	[SerializeField] SkillDescriptionUI descriptionUI;

	CharacterInfo currentCharacterInfo;

	private void Start()
	{
		foreach(CharacterSlot slot in characterSlots)
		{
			slot.SelectCallback += ClickCharacter;
		}
		characterSlots[0].OnClickSlot();
		ClickCharacter();
	}

    private void SetGage(float currentSpeed, float maxSpeed, float currentBrake, float maxBrake)
	{
		speedGage.SetGage(currentSpeed, maxSpeed);
		brakeGage.SetGage(currentBrake, maxBrake);
	}

	private void ClickCharacter(CharacterInfo info)
	{
		currentCharacterInfo = info;
		//	Ŭ������ �� ĳ���� ���� ������Ʈ
	}

	public void ClickCharacter()
	{
		nameText.text = currentCharacterInfo.characterName;
		characterImage.sprite = currentCharacterInfo.idleSprite;
		SetGage(currentCharacterInfo.speed, 200, currentCharacterInfo.brake, 200);
		hpUI.InitHp(currentCharacterInfo.hp, currentCharacterInfo.maxHp);
		descriptionUI.SetSkill(currentCharacterInfo.skillSprite, currentCharacterInfo.skillName, currentCharacterInfo.skillCoolTime, currentCharacterInfo.skillDescription);
		//	Ŭ������ �� ĳ���� ���� ������Ʈ
	}

	public void SelectCharacter()
	{
		characterManager.SetPlayerCharacter(currentCharacterInfo.characterName);
	}
}
