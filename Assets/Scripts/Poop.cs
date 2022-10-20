using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poop : MonoBehaviour
{
	[SerializeField] LayerMask Ground;
	[SerializeField] LayerMask Player;
	[SerializeField] float avgSpeed = 0f;
	[SerializeField] SpriteRenderer spriteRenderer;
	[SerializeField] Sprite[] sprites;
	Rigidbody2D rigid;

	float randSpeed;

	private void Start()
	{
		rigid = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		rigid.AddForce(new Vector2(0, randSpeed), ForceMode2D.Force);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
		{
			if (!GameManager.Instance.isDead)
			{
				GameManager.Instance.GetScore();
				GameManager.Instance.UpdateUI();
				GameManager.Instance.LevelDesign();
			}
			PoopManager.Instance.ReturnPool(this);
		}
		else if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			if (GameManager.Instance.Getinvincibility())
			{
				GameManager.Instance.GetScore();
				GameManager.Instance.UpdateUI();
				GameManager.Instance.LevelDesign();
				PoopManager.Instance.ReturnPool(this);
			}
			else
			{
				PoopManager.Instance.ReturnPool(this);
				GameManager.Instance.IsDead();
			}
		}
	}

	public void Setup()
	{
		randSpeed = Random.Range(-avgSpeed, -(avgSpeed + 2));
		spriteRenderer.sprite = sprites[GameManager.Instance.level];
	}
}
