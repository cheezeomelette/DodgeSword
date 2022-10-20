using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] float speed;
    [SerializeField] SkillUI skillUI;
    SpriteRenderer sprite;
	Animator anim;

    public bool invincibility;

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
        if (!GameManager.Instance.isDead)
        {
            Movement(moveX);
            UseSkill();
            if (!canUseSkill)
			{
                currentSkillCool += Time.deltaTime;
                Debug.Log(canUseSkill);
                if (currentSkillCool > skillCooltime)
				{
                    currentSkillCool = skillCooltime;
                    canUseSkill = true;
				}
                skillUI.SkiilAmount(currentSkillCool, skillCooltime);
            }
        }
    }

    void Movement(float x)
	{
        
        if(x < 0)
		{
            sprite.flipX = true;
            anim.SetBool("isMove", true);
		}
        else if(x > 0)
		{
            sprite.flipX = false;
            anim.SetBool("isMove", true);
        }
        else
		{
            rigid.velocity = new Vector2(rigid.velocity.x * 0.98f, 0);
            anim.SetBool("isMove", false);
		}
        if (rigid.velocity.x < -15f && x < 0)
            return;
        else if (rigid.velocity.x > 15f && x > 0)
            return;
		//rigid.velocity = new Vector2(x * speed, rigid.velocity.y);

		rigid.AddForce(new Vector2(x * speed, 0));
	}

	public void IsDead()
	{
        anim.SetTrigger("onDead");
        Time.timeScale = 0.5f;
        Invoke(nameof(StopTimeScale), 1.5f * Time.timeScale);
	}

    private void StopTimeScale()
    {
        Time.timeScale = 0f;
        GameManager.Instance.isGameOver = true;
        GameManager.Instance.GameOverPanel.SetActive(true);
    }

    void UseSkill()
	{
		if (Input.GetKeyDown(KeyCode.Z) && canUseSkill)
		{
            anim.SetTrigger("onSpin");
            invincibility = true;
            canUseSkill = false;
            currentSkillCool = 0;
        }
    }

    private void ExitSkill()
	{
        invincibility = false;
	}
}
