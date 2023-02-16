using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] CharacterInfo[] characterInfos;
	[SerializeField] Transform LobbyCharacterTransform;
	[SerializeField] LobbyCharacter prefab;
    [SerializeField] Player player;

    Dictionary<string, CharacterInfo> characterDictionary;

	private void Start()
	{
		characterDictionary = new Dictionary<string, CharacterInfo>();

		foreach(CharacterInfo info in characterInfos)
		{
			characterDictionary.Add(info.characterName, info);
			player.SetCharacterInfo(characterDictionary[info.characterName]);
		}
	}
	public void SetCharacter(string characterName)
	{
        player.SetCharacterInfo(characterDictionary[characterName]);
	}
	public void SetLobbyCharacter()
	{
		foreach(CharacterInfo info in characterInfos)
		{
			LobbyCharacter newCharacter = Instantiate(prefab, new Vector3(15, 3, 0), Quaternion.identity, LobbyCharacterTransform);
			newCharacter.SetInfo(info);
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
