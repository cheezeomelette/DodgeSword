using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
	[SerializeField] LayerMask Ground;
	[SerializeField] LayerMask Player;
	[SerializeField] float avgSpeed = 0f;
	[SerializeField] SpriteRenderer spriteRenderer;
	// 레벨에 따라 변화할 스프라이트 모음
	[SerializeField] Sprite[] sprites;
	Rigidbody2D rigid;

	// 떨어지는 랜덤속도
	float randSpeed;

	private void Start()
	{
		rigid = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		// 랜덤 속도로 아래로 떨어집니다.
		rigid.AddForce(new Vector2(0, randSpeed), ForceMode2D.Force);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		// 칼이 땅에 닿았다면
		if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
		{
			// 점수를 더하고 칼 재생성 시간을 줄인다.
			GameManager.Instance.GetScore();
			GameManager.Instance.UpdateUI();
			GameManager.Instance.LevelDesign();
			// 오브젝트 풀에 반환한다.
			SwordManager.Instance.ReturnPool(this);
		}
		// 플레이어가 맞았다면
		else if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			// 스킬울 사용해서 무적 상태라면
			if (GameManager.Instance.Getinvincibility())
			{
				// 점수를 더하고 칼 재생성 시간을 줄인다.
				GameManager.Instance.GetScore();
				GameManager.Instance.UpdateUI();
				GameManager.Instance.LevelDesign();
				// 오브젝트 풀에 반환한다.
				SwordManager.Instance.ReturnPool(this);
			}
			// 무적상태가 아니라면 
			else
			{
				// 오브젝트 풀에 반환하고 사망처리한다.
				SwordManager.Instance.ReturnPool(this);
				collision.GetComponent<Player>().Dead();
			}
		}
	}

	// 칼 초기 세팅
	public void Setup()
	{
		// 랜덤속도를 적용하고 레벨에 맞는 칼로 스프라이트를 변경한다.
		randSpeed = Random.Range(-avgSpeed, -(avgSpeed + 2));
		spriteRenderer.sprite = sprites[GameManager.Instance.level];
	}
}
