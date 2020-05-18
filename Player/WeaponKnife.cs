using Godot;
using System;

public class WeaponKnife : Weapon
{
	public override void _Ready()
	{
		DAMAGE = 40.0f;
		IDLE_ANIM_NAME = "Knife_idle";
		FIRE_ANIM_NAME = "Knife_fire";
		IDLE_UNARMED_ANIM = "Idle_unarmed";
		WPN_UNEQUIP_ANIM = "Knife_unequip";
		WPN_EQUIP_ANIM = "Knife_equip";
		animManager = GetNode<Player_Animation_Manager>("../../Model/Animation_Player");
	}

	public override void fireWeapon()
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
}
