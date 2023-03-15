using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "characterInfo", menuName = "Data/CharacterInfo")]

public class CharacterInfo : ScriptableObject
{
    public RuntimeAnimatorController animController;
    public Sprite idleSprite;
    public Sprite skillSprite;
    public string characterName;
    public string skillName;
    public string skillDescription;
    public float skillCoolTime;
    public float speed;
    public float brake;
    public int maxHp;
    public int hp;
}
