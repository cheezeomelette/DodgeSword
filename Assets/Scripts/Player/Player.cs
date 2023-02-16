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

	// ��ų ���� ����
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
		// �÷��̾ �׾��ִٸ� �������� �ʴ´�
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
		ObjectTime.timeScale = 0.5f;
		// 1.5�� �� �Լ� �߻�
		Invoke(nameof(StopAnim), 1.5f * ObjectTime.timeScale);
	}

	// ������
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

	// �ð��� ���߰� ���� �޴��� ����
	public void StopAnim()
	{
		anim.speed = 0f;
	}

	// ������
	private void Movement()
	{
		float moveX = 0;
		if (Input.touchCount == 1 && mTouchManager.touch2BoardPosition.y > -3f && mTouchManager.touch2BoardPosition.y < 3f)
		{
			moveX = mTouchManager.touch2BoardPosition.x > 0 ? 1 : -1;
		}

		// �ְ��ӵ�, �����ӵ� ����
		if (rigid.velocity.x < -15f || rigid.velocity.x > 15f)
			return;

		// �������� �Է��ϸ�    
		if (moveX < 0)
		{
			// ��������Ʈ ����
			spriteRenderer.flipX = true;
			anim.SetBool("isMove", true);
		}
		// ���������� �Է��ϸ�
		else if (moveX > 0)
		{
			// ��������Ʈ ���󺹱�
			spriteRenderer.flipX = false;
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
		// ��ų ��Ÿ���� �����ִٸ�
		if (!canUseSkill)
		{
			// ��Ÿ���� �ٿ��ش�
			currentSkillCool -= ObjectTime.deltaTime;
			// ��Ÿ���� �� ���� ��밡�� ���·� �����
			if (currentSkillCool < 0)
			{
				canUseSkill = true;
			}
			// ui�� ��� ������Ʈ �Ѵ�.
			skillUI.UpdateUI(currentSkillCool, skillCooltime);
		}
	}

	// ��ų ���� �� ���� ����
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