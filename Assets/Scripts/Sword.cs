using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
	[SerializeField] LayerMask Ground;
	[SerializeField] LayerMask Player;
	[SerializeField] float avgSpeed = 0f;
	[SerializeField] SpriteRenderer spriteRenderer;
	// ������ ���� ��ȭ�� ��������Ʈ ����
	[SerializeField] Sprite[] sprites;
	Rigidbody2D rigid;

	// �������� �����ӵ�
	float randSpeed;

	private void Start()
	{
		rigid = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		// ���� �ӵ��� �Ʒ��� �������ϴ�.
		rigid.AddForce(new Vector2(0, randSpeed), ForceMode2D.Force);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		// Į�� ���� ��Ҵٸ�
		if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
		{
			// ������ ���ϰ� Į ����� �ð��� ���δ�.
			GameManager.Instance.GetScore();
			GameManager.Instance.UpdateUI();
			GameManager.Instance.LevelDesign();
			// ������Ʈ Ǯ�� ��ȯ�Ѵ�.
			SwordManager.Instance.ReturnPool(this);
		}
		// �÷��̾ �¾Ҵٸ�
		else if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			// ��ų�� ����ؼ� ���� ���¶��
			if (GameManager.Instance.Getinvincibility())
			{
				// ������ ���ϰ� Į ����� �ð��� ���δ�.
				GameManager.Instance.GetScore();
				GameManager.Instance.UpdateUI();
				GameManager.Instance.LevelDesign();
				// ������Ʈ Ǯ�� ��ȯ�Ѵ�.
				SwordManager.Instance.ReturnPool(this);
			}
			// �������°� �ƴ϶�� 
			else
			{
				// ������Ʈ Ǯ�� ��ȯ�ϰ� ���ó���Ѵ�.
				SwordManager.Instance.ReturnPool(this);
				collision.GetComponent<Player>().Dead();
			}
		}
	}

	// Į �ʱ� ����
	public void Setup()
	{
		// �����ӵ��� �����ϰ� ������ �´� Į�� ��������Ʈ�� �����Ѵ�.
		randSpeed = Random.Range(-avgSpeed, -(avgSpeed + 2));
		spriteRenderer.sprite = sprites[GameManager.Instance.level];
	}
}
