using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] Character[] Characters;
	[SerializeField] Transform LobbyCharacterTransform;
	[SerializeField] LobbyCharacter prefab;
	[SerializeField] SkillUI skillUI;
	[SerializeField] HpUI hpUI;

	Character currentPlayerCharacter;
	Dictionary<string, Character> characterDictionary;

	private void Start()
	{
		characterDictionary = new Dictionary<string, Character>();

		foreach(Character player in Characters)
		{
			characterDictionary.Add(player.characterName, player);
		}
		currentPlayerCharacter = Characters[0];
	}

	public void SetPlayerCharacter(string characterName)
	{
		currentPlayerCharacter = characterDictionary[characterName];
	}

	public Character CreatePlayerCharacter()
	{
		Character player = Instantiate(currentPlayerCharacter);

		player.SetPlayerUI(skillUI, hpUI);
		player.Init();

		return player;
	}
	public void SetLobbyCharacter()
	{
		for(int i = 0; i < Characters.Length; i++)
		{
			LobbyCharacter newCharacter = Instantiate(prefab, new Vector3(15, 3, 0), Quaternion.identity, LobbyCharacterTransform);
			newCharacter.Init();
			newCharacter.SetInfo(Characters[i].characterInfo, i);
		}
	}
	public void ClearLobbyCharacter()
	{
		foreach (Transform child in LobbyCharacterTransform)
		{
			Destroy(child.gameObject);
		}
	}

}
