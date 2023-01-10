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

    // ��ų ���� ����
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
        // �÷��̾ �׾��ִٸ� �������� �ʴ´�
        if (isDead)
            return;

        Movement(moveX);
        UseSkill();
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
    void Movement(float x)
	{
        // �������� �Է��ϸ�    
        if(x < 0)
		{
            // ��������Ʈ ����
            sprite.flipX = true;
            anim.SetBool("isMove", true);
		}
        // ���������� �Է��ϸ�
        else if(x > 0)
		{
            // ��������Ʈ ���󺹱�
            sprite.flipX = false;
            anim.SetBool("isMove", true);
        }
        // �Է��� ������
        else
		{
            // �ӵ��� ������ �����.
            rigid.velocity = new Vector2(rigid.velocity.x * 0.98f, 0);
            anim.SetBool("isMove", false);
		}
        // �ְ�ӵ�, �����ӵ� ����
        if (rigid.velocity.x < -15f && x < 0)
            return;
        else if (rigid.velocity.x > 15f && x > 0)
            return;

        // �Է��� ���� �� ���� ���ؼ� �ణ �̲��������� �ӵ��� �����Ѵ�
		rigid.AddForce(new Vector2(x * speed, 0));
	}

    // ������
    public void Respawn(Vector2 position)
	{
        isDead = false;
        // ������ ���ġ
        transform.position = position;
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

    // ��ų��� �Լ�
    void UseSkill()
	{
        // ��ų ��Ÿ���� �ƴ� �� zŰ�� ������ ��ų ���
		if (Input.GetKeyDown(KeyCode.Z) && canUseSkill)
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

    // ��ų ���� �� ���� ����
    private void ExitSkill()
	{
        invincibility = false;
	}
}
