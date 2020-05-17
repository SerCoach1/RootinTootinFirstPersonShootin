using Godot;
using System;

public class WeaponKnife : Spatial
{
	private const float DAMAGE = 40.0f;
	private const string IDLE_ANIM_NAME = "Knife_idle";
	private const string FIRE_ANIM_NAME = "Knife_fire";
	private bool isWeaponEnabled = false;

	private Player_Animation_Manager animManager = null;
	public Player playerNode = null;
	public override void _Ready()
	{
		animManager = GetNode<Player_Animation_Manager>("Animation_Player");
	}

	private void fireWeapon()
	{
		var area = GetNode<Area>("Area");
		var bodies = area.GetOverlappingBodies();

		foreach (Godot.Object body in bodies)
		{
            // if(body != playerNode && body.HasMethod("bullet_hit"))
            // {
            //     body.bullet_hit(DAMAGE, area.GlobalTransform);
            // }
		}
	}

	private bool equipWeapon()
	{
		if (animManager.currentState == IDLE_ANIM_NAME)
		{
			isWeaponEnabled = true;
			return true;
		}

		if (animManager.currentState == "Idle_unarmed")
		{
			animManager.setAnimation("Knife_equip");
		}

		return false;
	}

	private bool unequipWeapon()
	{
		// TODO: combine 2 if statements into one?
		if (animManager.currentState == IDLE_ANIM_NAME)
		{
			if (animManager.currentState != "Knife_unequip")
			{
				animManager.setAnimation("Knife_unequip");
			}
		}

		if (animManager.currentState == "Idle_unarmed")
		{
			isWeaponEnabled = false;
			return true;
		}

		return false;
	}
}
