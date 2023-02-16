using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpUI : MonoBehaviour
{
	[SerializeField] Transform lifeTransform;
	[SerializeField] GameObject fullHp;
	[SerializeField] GameObject emptyHp;

	GameObject[] fullArr;
	GameObject[] emptyArr;

	public void InitHp(int hp, int maxHp)
	{
		foreach (Transform transform in lifeTransform)
		{
			Destroy(transform.gameObject);
		}
		fullArr = new GameObject[maxHp];
		emptyArr = new GameObject[maxHp];
		for (int i = 0; i < maxHp; i++)
		{
			fullArr[i] = Instantiate(fullHp, lifeTransform);
			if (i < hp)
				fullArr[i].gameObject.SetActive(true);
			else
				fullArr[i].gameObject.SetActive(false);
		}
		for (int i = 0; i < maxHp; i++)
		{
			emptyArr[i] = Instantiate(emptyHp, lifeTransform);
			if (i >= hp)
				emptyArr[i].gameObject.SetActive(true);
			else
				emptyArr[i].gameObject.SetActive(false);
		}
	}

	public void UpdateHp(int hp, int maxHp)
	{
		for (int i = 0; i < maxHp; i++)
		{
			if (i < hp)
				fullArr[i].gameObject.SetActive(true);
			else
				fullArr[i].gameObject.SetActive(false);
		}
		for (int i = 0; i < maxHp; i++)
		{
			if (i >= hp)
				emptyArr[i].gameObject.SetActive(true);
			else
				emptyArr[i].gameObject.SetActive(false);
		}
	}
}
