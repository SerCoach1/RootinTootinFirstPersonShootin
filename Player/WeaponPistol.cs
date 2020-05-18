using Godot;
using System;

public class WeaponPistol : Weapon
{
	private PackedScene bullet_scene;
	public override void _Ready()
	{
		DAMAGE = 15.0f;
		IDLE_ANIM_NAME = "Pistol_idle";
		FIRE_ANIM_NAME = "Pistol_fire";
		IDLE_UNARMED_ANIM = "Idle_unarmed";
		WPN_UNEQUIP_ANIM = "Pistol_unequip";
		WPN_EQUIP_ANIM = "Pistol_equip";
		bullet_scene = (PackedScene)ResourceLoader.Load("Bullet_Scene.tscn");
		animManager = GetNode<Player_Animation_Manager>("../../Model/Animation_Player");
	}

	public override void fireWeapon()
	{
		// Spawn bullet
		var clone = bullet_scene.Instance();
		GetTree().Root.AddChild(clone);

		// Find created bullet 
		var bullet = GetNode<Bullet_Scene>("Bullet_Scene");
		// Set bullet parameters
		bullet.GlobalTransform = this.GlobalTransform;
		bullet.Scale = new Vector3(4, 4, 4);
		bullet.BULLET_DAMAGE = DAMAGE;
	}
}
