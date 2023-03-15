using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : FallingObject
{
	[SerializeField] LayerMask Ground;
	[SerializeField] LayerMask Player;
	[SerializeField] SpriteRenderer spriteRenderer;
	// 레벨에 따라 변화할 스프라이트 모음
	[SerializeField] Sprite[] sprites;

	private void Update()
	{
		AccelThisObject();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		// 칼이 땅에 닿았다면
		if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
		{
			// 칼 재생성 시간을 줄인다.
			GameManager.Instance.GenerateSwordFaster();
			// 오브젝트 풀에 반환한다.
			SwordPool.Instance.ReturnPool(this);
		}
		// 플레이어가 맞았다면
		else if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			// 스킬울 사용해서 무적 상태라면
			if (collision.GetComponent<Character>().isInvincibility)
			{
				// 칼 재생성 시간을 줄인다.
				GameManager.Instance.GenerateSwordFaster();
				// 오브젝트 풀에 반환한다.
				SwordPool.Instance.ReturnPool(this);
			}
			// 무적상태가 아니라면 
			else
			{
				// 오브젝트 풀에 반환하고 데미지처리한다.
				SwordPool.Instance.ReturnPool(this);
				collision.GetComponent<Character>().GetDamaged();
				SoundManager.Instance.Play("GetDamaged");
			}
		}
		GameManager.Instance.GetScore();
		GameManager.Instance.UpdateScoreUI();
	}

	// 칼 초기 세팅
	public override void Setup()
	{
		base.Setup();
		spriteRenderer.sprite = sprites[GameManager.Instance.Level];
	}
}
