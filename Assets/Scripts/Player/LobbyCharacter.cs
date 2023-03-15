using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class LobbyCharacter : MonoBehaviour
{
	SpriteRenderer spriteRenderer;
	Rigidbody2D rigid;
	Animator anim;

	float speed;
	float brake;
	State state = State.Idle;

	enum State
	{
		Idle,
		Move,
		Skill,
		Count
	}

	public void Init()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		rigid = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		StartCoroutine(StateMachine());
	}

	public void SetInfo(CharacterInfo info, int orderLayer)
	{
		anim.runtimeAnimatorController = info.animController;
		spriteRenderer.sortingOrder = 10 + orderLayer;
		speed = info.speed;
		brake = info.brake;
	}

	public void UseSkill()
	{
		// 스킬 애니메이션 실행
		anim.SetTrigger("onSpin");
	}

	// 움직임
	private void Movement(int inputX)
	{
		float moveX = inputX;

		// 최고속도, 최저속도 조절
		if (rigid.velocity.x < -15f || rigid.velocity.x > 15f)
			return;

		// 왼쪽으로 입력하면    
		if (moveX < 0)
		{
			// 스프라이트 반전
			spriteRenderer.flipX = true;
			anim.SetBool("isMove", true);
		}
		// 오른쪽으로 입력하면
		else if (moveX > 0)
		{
			// 스프라이트 원상복귀
			spriteRenderer.flipX = false;
			anim.SetBool("isMove", true);
		}
		// 입력이 없으면
		else
		{
			// 서서히 멈춘다.
			SlowDown();
			anim.SetBool("isMove", false);
		}
		// 입력이 있을 때 힘을 가해서 약간 미끄러지도록 속도를 조절한다
		rigid.AddForce(new Vector2(moveX * speed, 0) * 15 * ObjectTime.deltaTime);
		anim.SetFloat("speed", rigid.velocity.x);
	}

	private void SlowDown()
	{
		if (rigid.velocity.x > 5)
			rigid.velocity = rigid.velocity - new Vector2(brake * ObjectTime.deltaTime, 0);
		else if (rigid.velocity.x > 0)
			rigid.velocity = rigid.velocity - new Vector2(brake / 2f * ObjectTime.deltaTime, 0);
		else if (rigid.velocity.x < -5)
			rigid.velocity = rigid.velocity + new Vector2(brake * ObjectTime.deltaTime, 0);
		else if (rigid.velocity.x < 0)
			rigid.velocity = rigid.velocity + new Vector2(brake / 2f * ObjectTime.deltaTime, 0);
	}
	private void GetRandomInput()
	{
		state = (State)Random.Range(1, 3);
	}

	// 스킬 종료 시 무적 해제
	private void ExitSkill()
	{
	}

	IEnumerator Move()
	{
		int x;
		if (transform.position.x > 25)
			x = -1;
		else if (transform.position.x < 5)
			x = 1;
		else
			x = Random.Range(-100f, 100f) > 0 ? 1 : -1;
		float time = Random.Range(0.5f, 0.8f);
		float currentTime = 0;
		while (currentTime < time)
		{
			Movement(x);
			currentTime += Time.deltaTime;
			yield return null;
		}
		time = 0.5f;
		currentTime = 0;
		while (currentTime < time)
		{
			Movement(0);
			currentTime += Time.deltaTime;
			yield return null;
		}
	}
	IEnumerator StateMachine()
	{
		while (true)
		{
			switch (state)
			{
				case State.Idle:
					anim.SetBool("isMove", false);
					yield return new WaitForSeconds(1f);
					GetRandomInput();
					break;
				case State.Move:
					yield return Move();
					state = State.Idle;
					break;
				case State.Skill:
					UseSkill();
					yield return new WaitForSeconds(1f); 
					state = State.Idle;
					break;
			}
			yield return null;
		}
	}
}
