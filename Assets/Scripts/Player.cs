using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] SkillUI skillUI;
    [SerializeField] float speed;
    SpriteRenderer sprite;
	Animator anim;

    // 스킬 사용시 무적
    public bool invincibility;
    bool isDead;
    const float skillCooltime = 3f;
    float currentSkillCool;
    bool canUseSkill;

    private void Start()
	{
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        invincibility = false;
        canUseSkill = true;
        currentSkillCool = 0f;
    }

	void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        // 플레이어가 죽어있다면 실행하지 않는다
        if (isDead)
            return;

        Movement(moveX);
        UseSkill();
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

    // 움직임
    void Movement(float x)
	{
        // 왼쪽으로 입력하면    
        if(x < 0)
		{
            // 스프라이트 반전
            sprite.flipX = true;
            anim.SetBool("isMove", true);
		}
        // 오른쪽으로 입력하면
        else if(x > 0)
		{
            // 스프라이트 원상복귀
            sprite.flipX = false;
            anim.SetBool("isMove", true);
        }
        // 입력이 없으면
        else
		{
            // 속도를 서서히 늦춘다.
            rigid.velocity = new Vector2(rigid.velocity.x * 0.98f, 0);
            anim.SetBool("isMove", false);
		}
        // 최고속도, 최저속도 조절
        if (rigid.velocity.x < -15f && x < 0)
            return;
        else if (rigid.velocity.x > 15f && x > 0)
            return;

        // 입력이 있을 때 힘을 가해서 약간 미끄러지도록 속도를 조절한다
		rigid.AddForce(new Vector2(x * speed, 0));
	}

    // 리스폰
    public void Respawn(Vector2 position)
	{
        isDead = false;
        // 포지션 재배치
        transform.position = position;
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

    // 스킬사용 함수
    void UseSkill()
	{
        // 스킬 쿨타임이 아닐 때 z키를 누르면 스킬 사용
		if (Input.GetKeyDown(KeyCode.Z) && canUseSkill)
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

    // 스킬 종료 시 무적 해제
    private void ExitSkill()
	{
        invincibility = false;
	}
}
