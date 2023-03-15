using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Penguin : Character
{
	protected override void Update()
	{
		base.Update();

		if (Mathf.Abs(rigid.velocity.x) > 8)
			UseSkill();
		else if (Mathf.Abs(rigid.velocity.x) < 5)
			ExitSkill();
	}

	protected override void Movement()
	{
		base.Movement();

		anim.SetFloat("speed", rigid.velocity.x);
	}
}
