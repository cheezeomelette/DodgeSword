using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
	[SerializeField] LayerMask Ground;
	[SerializeField] LayerMask Player;
	[SerializeField] float addAcc;
	[SerializeField] SpriteRenderer spriteRenderer;
	// ������ ���� ��ȭ�� ��������Ʈ ����
	[SerializeField] Sprite[] sprites;

	float velocity;
	float acc = -9.8f;
	float randAcc;

	private void Update()
	{
		velocity += ObjectTime.deltaTime * (acc + randAcc);
		transform.position += new Vector3(0, ObjectTime.deltaTime * velocity, 0);
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
		velocity = 0;
		randAcc = Random.Range(0, -addAcc);
		spriteRenderer.sprite = sprites[GameManager.Instance.level];
	}
}
