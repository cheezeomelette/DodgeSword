using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeItem : FallingItem
{
	protected override void OnCollisionEnter2D(Collision2D collision)
	{
		base.OnCollisionEnter2D(collision);
		if (collision.gameObject.CompareTag("Player"))
		{
			collision.gameObject.GetComponent<Player>().GetLife();
			SoundManager.Instance.Play("GetLife");
			Destroy(gameObject);
		}
	}
}
