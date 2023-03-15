using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinItem : FallingItem
{
	protected override void OnCollisionEnter2D(Collision2D collision)
	{
		base.OnCollisionEnter2D(collision);
		if (collision.gameObject.CompareTag("Player"))
		{
			collision.gameObject.GetComponent<Character>().GetCoin();
			SoundManager.Instance.Play("GetCoin");
			CreateParticle();
			Destroy(gameObject);
		}
	}
}
