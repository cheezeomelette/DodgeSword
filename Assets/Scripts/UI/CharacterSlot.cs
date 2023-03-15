using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSlot : MonoBehaviour
{
    [SerializeField] CharacterInfo info;

	public System.Action<CharacterInfo> SelectCallback;
	
	public void OnClickSlot()
	{
		SelectCallback(info);
	}
}
