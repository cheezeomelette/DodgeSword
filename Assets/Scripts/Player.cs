using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField] Rigidbody2D rigid;
	[SerializeField] SkillUI skillUI;
	[SerializeField] float speed;
	TouchManager mTouchManager;
	SpriteRenderer sprite;
	Animator anim;

	// 스킬 사용시 무적
	public bool invincibility;
	const float skillCooltime = 3f;
	float currentSkillCool;
	bool canUseSkill;
	bool isDead;

	private void Start()
	{
		mTouchManager = new TouchManager(Camera.main.transform);
		sprite = GetComponent<SpriteRenderer>();
		anim = GetComponent<Animator>();

		invincibility = false;
		canUseSkill = true;
		isDead = true;

		currentSkillCool = 0f;
	}

	private void Update()
	{
		// 플레이어가 죽어있다면 실행하지 않는다
		if (isDead)
			return;

		Movement();
		UpdateSkillUI();

	}

	// 움직임
	private void Movement()
	{
		float moveX = 0;
		if (Input.touchCount == 1 && mTouchManager.touch2BoardPosition.y > -3f && mTouchManager.touch2BoardPosition.y < 3f)
		{
			moveX = mTouchManager.touch2BoardPosition.x > 0 ? 1 : -1;
		}

		// 최고속도, 최저속도 조절
		if (rigid.velocity.x < -15f || rigid.velocity.x > 15f)
			return;

		// 왼쪽으로 입력하면    
		if (moveX < 0)
		{
			// 스프라이트 반전
			sprite.flipX = true;
			anim.SetBool("isMove", true);
		}
		// 오른쪽으로 입력하면
		else if (moveX > 0)
		{
			// 스프라이트 원상복귀
			sprite.flipX = false;
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
		rigid.AddForce(new Vector2(moveX * speed, 0) * 15 *Time.deltaTime);
	}

	// 스킬사용 함수
	public void UseSkill()
	{
		// 스킬 쿨타임이 아닐 때 z키를 누르면 스킬 사용
		if (canUseSkill && !isDead)
		{
			// 스킬 애니메이션 실행
			anim.SetTrigger("onSpin");
			// 무적상태
			invincibility = true;
			// 스킬사용 불가
			canUseSkill = false;
			// 현재 쿨타임 갱신
			currentSkillCool = skillCooltime;
		}
	}

	private void SlowDown()
	{
		if (rigid.velocity.x > 5)
			rigid.velocity = rigid.velocity - new Vector2(20f * Time.deltaTime, 0);
		else if(rigid.velocity.x > 0)
			rigid.velocity = rigid.velocity - new Vector2(10f * Time.deltaTime, 0);
		else if (rigid.velocity.x < -5)
			rigid.velocity = rigid.velocity + new Vector2(20f * Time.deltaTime, 0);
		else if(rigid.velocity.x < 0)
			rigid.velocity = rigid.velocity + new Vector2(10f * Time.deltaTime, 0);
	}

	private void UpdateSkillUI()
	{
		// 스킬 쿨타임이 돌고있다면
		if (!canUseSkill)
		{
			// 쿨타임을 줄여준다
			currentSkillCool -= Time.deltaTime;
			// 쿨타임을 다 돌면 사용가능 상태로 만든다
			if (currentSkillCool < 0)
			{
				canUseSkill = true;
			}
			// ui를 계속 업데이트 한다.
			skillUI.UpdateUI(currentSkillCool, skillCooltime);
		}
	}
	// 리스폰
	public void Respawn(Vector2 position)
	{
		gameObject.SetActive(false);
		isDead = false;
		transform.position = position;
		currentSkillCool = 0f;
		gameObject.SetActive(true);
	}

	// 죽었을 때
	public void Dead()
	{
		// 이미 죽어있다면 실행하지 않는다.
		if (isDead)
			return;

		isDead = true;
		GameManager.Instance.Dead();
		// 사망 애니메이션 실행
		anim.SetTrigger("onDead");
		// 속도 늦추는 연출
		Time.timeScale = 0.5f;
		// 1.5초 후 함수 발생
		Invoke(nameof(StopTimeScale), 1.5f * Time.timeScale);
	}

	// 시간을 멈추고 시작 메뉴를 켜줌
	private void StopTimeScale()
	{
		// 플레이어의 속도를 0으로 만든다.
		rigid.velocity = Vector2.zero;
		Time.timeScale = 0f;
		GameManager.Instance.isGameOver = true;
		GameManager.Instance.SetMenu();
	}


	// 스킬 종료 시 무적 해제
	private void ExitSkill()
	{
		invincibility = false;
	}

	public void Test()
	{
		Debug.Log("Doing");
	}
}
