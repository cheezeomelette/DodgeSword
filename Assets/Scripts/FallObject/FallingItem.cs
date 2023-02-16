using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
public class FallingItem : MonoBehaviour
{
	[SerializeField] GameObject particle;
	Animator anim;

	private void Start()
	{
		anim = GetComponent<Animator>();
	}

	protected virtual void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Ground")
		{
			DestroyItem();
		}
	}
	protected void DestroyItem()
	{
		anim.SetTrigger("onDestroy");
		Destroy(gameObject, 1f);
	}
	private void OnDestroy()
	{
		Instantiate(particle,transform.position,Quaternion.Euler(-40,0,0), null);
	}
}
