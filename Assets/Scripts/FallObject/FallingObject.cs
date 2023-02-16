using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class FallingObject : MonoBehaviour
{
	[SerializeField] float addAcc = 5f;

	float velocity;
	float acc = -9.8f;
	float randAcc;

	protected void AccelThisObject()
	{
		velocity += ObjectTime.deltaTime * (acc + randAcc);
		transform.position += new Vector3(0, ObjectTime.deltaTime * velocity, 0);
	}

	// Į �ʱ� ����
	public virtual void Setup()
	{
		// �����ӵ��� �����ϰ� ������ �´� Į�� ��������Ʈ�� �����Ѵ�.
		velocity = 0;
		randAcc = Random.Range(0, -addAcc);
	}
}
