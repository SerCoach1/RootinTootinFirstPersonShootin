using Godot;
using System;

public class WeaponRifle : Weapon
{
	public override void _Ready()
	{
		DAMAGE = 4.0f;
		MAG_SIZE = 30;
		ammoInWeapon = MAG_SIZE;
		spareAmmo = 270;
		RELOADING_ANIM_NAME = "Rifle_reload";
		IDLE_ANIM_NAME = "Rifle_idle";
		FIRE_ANIM_NAME = "Rifle_fire";
		IDLE_UNARMED_ANIM = "Idle_unarmed";
		WPN_UNEQUIP_ANIM = "Rifle_unequip";
		WPN_EQUIP_ANIM = "Rifle_equip";
		animManager = (Player_Animation_Manager)GetNode<AnimationPlayer>("../../Model/Animation_Player");
	}

	public override void fireWeapon()
	{
		// Spawn bullet
		var ray = GetNode<RayCast>("Ray_Cast");
		ray.ForceRaycastUpdate();

		if (ray.IsColliding())
		{
			var body = ray.GetCollider();

			if (body != playerNode && body.HasMethod("bullet_hit"))
			{
				body.Call("bullet_hit", DAMAGE, ray.GlobalTransform);
			}
		}
		ammoInWeapon--;
	}
}
