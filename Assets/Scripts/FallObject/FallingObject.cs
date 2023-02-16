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

	// 칼 초기 세팅
	public virtual void Setup()
	{
		// 랜덤속도를 적용하고 레벨에 맞는 칼로 스프라이트를 변경한다.
		velocity = 0;
		randAcc = Random.Range(0, -addAcc);
	}
}
