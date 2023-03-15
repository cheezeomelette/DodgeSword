using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : FallingObject
{
	[SerializeField] LayerMask Ground;
	[SerializeField] LayerMask Player;
	[SerializeField] SpriteRenderer spriteRenderer;
	// ������ ���� ��ȭ�� ��������Ʈ ����
	[SerializeField] Sprite[] sprites;

	private void Update()
	{
		AccelThisObject();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		// Į�� ���� ��Ҵٸ�
		if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
		{
			// Į ����� �ð��� ���δ�.
			GameManager.Instance.GenerateSwordFaster();
			// ������Ʈ Ǯ�� ��ȯ�Ѵ�.
			SwordPool.Instance.ReturnPool(this);
		}
		// �÷��̾ �¾Ҵٸ�
		else if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			// ��ų�� ����ؼ� ���� ���¶��
			if (collision.GetComponent<Character>().isInvincibility)
			{
				// Į ����� �ð��� ���δ�.
				GameManager.Instance.GenerateSwordFaster();
				// ������Ʈ Ǯ�� ��ȯ�Ѵ�.
				SwordPool.Instance.ReturnPool(this);
			}
			// �������°� �ƴ϶�� 
			else
			{
				// ������Ʈ Ǯ�� ��ȯ�ϰ� ������ó���Ѵ�.
				SwordPool.Instance.ReturnPool(this);
				collision.GetComponent<Character>().GetDamaged();
				SoundManager.Instance.Play("GetDamaged");
			}
		}
		GameManager.Instance.GetScore();
		GameManager.Instance.UpdateScoreUI();
	}

	// Į �ʱ� ����
	public override void Setup()
	{
		base.Setup();
		spriteRenderer.sprite = sprites[GameManager.Instance.Level];
	}
}
