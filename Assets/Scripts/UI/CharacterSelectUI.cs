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
		//	클릭했을 때 캐릭터 정보 업데이트
	}

	public void ClickCharacter()
	{
		nameText.text = currentCharacterInfo.characterName;
		characterImage.sprite = currentCharacterInfo.idleSprite;
		SetGage(currentCharacterInfo.speed, 200, currentCharacterInfo.brake, 200);
		hpUI.InitHp(currentCharacterInfo.hp, currentCharacterInfo.maxHp);
		descriptionUI.SetSkill(currentCharacterInfo.skillSprite, currentCharacterInfo.skillName, currentCharacterInfo.skillCoolTime, currentCharacterInfo.skillDescription);
		//	클릭했을 때 캐릭터 정보 업데이트
	}

	public void SelectCharacter()
	{
		characterManager.SetPlayerCharacter(currentCharacterInfo.characterName);
	}
}
