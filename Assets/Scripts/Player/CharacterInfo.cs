using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "characterInfo", menuName = "Data/CharacterInfo")]

public class CharacterInfo : ScriptableObject
{
    public string characterName;
    public float skillCoolTime;
    public float speed;
    public float brake;
    public int maxHp;
    public int hp;
}
