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

	// ��ų ���� ����
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
		// �÷��̾ �׾��ִٸ� �������� �ʴ´�
		if (isDead)
			return;

		Movement();
		UpdateSkillUI();

	}

	// ������
	private void Movement()
	{
		float moveX = 0;
		if (Input.touchCount == 1 && mTouchManager.touch2BoardPosition.y > -3f && mTouchManager.touch2BoardPosition.y < 3f)
		{
			moveX = mTouchManager.touch2BoardPosition.x > 0 ? 1 : -1;
		}

		// �ְ�ӵ�, �����ӵ� ����
		if (rigid.velocity.x < -15f || rigid.velocity.x > 15f)
			return;

		// �������� �Է��ϸ�    
		if (moveX < 0)
		{
			// ��������Ʈ ����
			sprite.flipX = true;
			anim.SetBool("isMove", true);
		}
		// ���������� �Է��ϸ�
		else if (moveX > 0)
		{
			// ��������Ʈ ���󺹱�
			sprite.flipX = false;
			anim.SetBool("isMove", true);
		}
		// �Է��� ������
		else
		{
			// ������ �����.
			SlowDown();
			anim.SetBool("isMove", false);
		}

		// �Է��� ���� �� ���� ���ؼ� �ణ �̲��������� �ӵ��� �����Ѵ�
		rigid.AddForce(new Vector2(moveX * speed, 0) * 15 *Time.deltaTime);
	}

	// ��ų��� �Լ�
	public void UseSkill()
	{
		// ��ų ��Ÿ���� �ƴ� �� zŰ�� ������ ��ų ���
		if (canUseSkill && !isDead)
		{
			// ��ų �ִϸ��̼� ����
			anim.SetTrigger("onSpin");
			// ��������
			invincibility = true;
			// ��ų��� �Ұ�
			canUseSkill = false;
			// ���� ��Ÿ�� ����
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
		// ��ų ��Ÿ���� �����ִٸ�
		if (!canUseSkill)
		{
			// ��Ÿ���� �ٿ��ش�
			currentSkillCool -= Time.deltaTime;
			// ��Ÿ���� �� ���� ��밡�� ���·� �����
			if (currentSkillCool < 0)
			{
				canUseSkill = true;
			}
			// ui�� ��� ������Ʈ �Ѵ�.
			skillUI.UpdateUI(currentSkillCool, skillCooltime);
		}
	}
	// ������
	public void Respawn(Vector2 position)
	{
		gameObject.SetActive(false);
		isDead = false;
		transform.position = position;
		currentSkillCool = 0f;
		gameObject.SetActive(true);
	}

	// �׾��� ��
	public void Dead()
	{
		// �̹� �׾��ִٸ� �������� �ʴ´�.
		if (isDead)
			return;

		isDead = true;
		GameManager.Instance.Dead();
		// ��� �ִϸ��̼� ����
		anim.SetTrigger("onDead");
		// �ӵ� ���ߴ� ����
		Time.timeScale = 0.5f;
		// 1.5�� �� �Լ� �߻�
		Invoke(nameof(StopTimeScale), 1.5f * Time.timeScale);
	}

	// �ð��� ���߰� ���� �޴��� ����
	private void StopTimeScale()
	{
		// �÷��̾��� �ӵ��� 0���� �����.
		rigid.velocity = Vector2.zero;
		Time.timeScale = 0f;
		GameManager.Instance.isGameOver = true;
		GameManager.Instance.SetMenu();
	}


	// ��ų ���� �� ���� ����
	private void ExitSkill()
	{
		invincibility = false;
	}

	public void Test()
	{
		Debug.Log("Doing");
	}
}
