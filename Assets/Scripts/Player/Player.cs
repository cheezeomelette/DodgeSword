using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Rigidbody2D))]
public class Player : MonoBehaviour
{
	[SerializeField] SkillUI skillUI;
	[SerializeField] HpUI hpUI;
	SpriteRenderer spriteRenderer;
	CharacterInfo characterInfo;
	TouchManager mTouchManager;
	Rigidbody2D rigid;
	Animator anim;

	// 스킬 사용시 무적
	bool invincibility;
	public bool isInvincibility => invincibility;

	float currentSkillCool;
	float skillCooltime;
	bool canUseSkill;
	bool isDead;
	public bool IsDead => isDead;
	
	float speed;
	float brake;
	int maxHp;
	int startHp;
	int hp;


	private void Start()
	{
		mTouchManager = new TouchManager(Camera.main.transform);
		spriteRenderer = GetComponent<SpriteRenderer>();
		rigid = GetComponent<Rigidbody2D>();
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
		{
			SlowDown();
			return;
		}

		Movement();
		UpdateSkillUI();
	}

	public void SetCharacterInfo(CharacterInfo info)
	{
		skillCooltime = info.skillCoolTime;
		speed = info.speed;
		brake = info.brake;
		maxHp = info.maxHp;
		startHp = info.hp;
		hp = startHp;
		hpUI.InitHp(startHp, maxHp);
	}

	public void GetDamaged()
	{
		hp -= 1;
		UpdateHpUI(hp, maxHp);
		StartCoroutine(DamageBlinking());
		if (hp <= 0)
		{
			Dead();
		}
	}

	public void GetLife()
	{
		hp += 1;
		hp = Mathf.Min(hp, maxHp);
		UpdateHpUI(hp, maxHp);
	}

	public void GetCoin()
	{
		GameManager.Instance.GetCoin();
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
		ObjectTime.timeScale = 0.5f;
		// 1.5초 후 함수 발생
		Invoke(nameof(StopAnim), 1.5f * ObjectTime.timeScale);
	}

	// 리스폰
	public void Respawn(Vector2 position)
	{
		isDead = false;
		hp = startHp;
		anim.speed = 1f;
		transform.position = position;
		currentSkillCool = 0f;
		gameObject.SetActive(true);
		UpdateHpUI(hp, maxHp);
	}
	
	public void UpdateHpUI(int hp, int maxHp)
	{
		hpUI.UpdateHp(hp, maxHp);
	}

	// 시간을 멈추고 시작 메뉴를 켜줌
	public void StopAnim()
	{
		anim.speed = 0f;
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
		rigid.AddForce(new Vector2(moveX * speed, 0) * 15 *ObjectTime.deltaTime);
	}

	private void SlowDown()
	{
		if (rigid.velocity.x > 5)
			rigid.velocity = rigid.velocity - new Vector2(brake * ObjectTime.deltaTime, 0);
		else if(rigid.velocity.x > 0)
			rigid.velocity = rigid.velocity - new Vector2(brake / 2f * ObjectTime.deltaTime, 0);
		else if (rigid.velocity.x < -5)
			rigid.velocity = rigid.velocity + new Vector2(brake * ObjectTime.deltaTime, 0);
		else if(rigid.velocity.x < 0)
			rigid.velocity = rigid.velocity + new Vector2(brake / 2f * ObjectTime.deltaTime, 0);
	}

	private void UpdateSkillUI()
	{
		// 스킬 쿨타임이 돌고있다면
		if (!canUseSkill)
		{
			// 쿨타임을 줄여준다
			currentSkillCool -= ObjectTime.deltaTime;
			// 쿨타임을 다 돌면 사용가능 상태로 만든다
			if (currentSkillCool < 0)
			{
				canUseSkill = true;
			}
			// ui를 계속 업데이트 한다.
			skillUI.UpdateUI(currentSkillCool, skillCooltime);
		}
	}

	// 스킬 종료 시 무적 해제
	private void ExitSkill()
	{
		invincibility = false;
	}

	IEnumerator DamageBlinking()
	{
		invincibility = true;
		yield return null;
		float alpha = 1;
		Color currentColor = spriteRenderer.color;
		for(int i = 0; i < 3; i++)
		{
			while(alpha > 0)
			{
				currentColor.a = alpha;
				spriteRenderer.color = currentColor;
				alpha -= Time.deltaTime * 6;
				yield return null;
			}
			while(alpha < 1)
			{
				currentColor.a = alpha;
				spriteRenderer.color = currentColor;
				alpha += Time.deltaTime * 6;
				yield return null;
			}
		}
		invincibility = false;
	}
}
