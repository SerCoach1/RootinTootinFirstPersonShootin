using Godot;
using System;

public class WeaponPistol : Weapon
{
	private PackedScene bullet_scene;
	public override void _Ready()
	{
		DAMAGE = 15.0f;
		MAG_SIZE = 10;
		ammoInWeapon = MAG_SIZE;
		spareAmmo = 90;
		RELOADING_ANIM_NAME = "Pistol_reload";
		IDLE_ANIM_NAME = "Pistol_idle";
		FIRE_ANIM_NAME = "Pistol_fire";
		IDLE_UNARMED_ANIM = "Idle_unarmed";
		WPN_UNEQUIP_ANIM = "Pistol_unequip";
		WPN_EQUIP_ANIM = "Pistol_equip";
		bullet_scene = (PackedScene)ResourceLoader.Load("assets/Bullet_Scene.tscn");
		animManager = GetNode<Player_Animation_Manager>("../../Model/Animation_Player");
	}

	public override void fireWeapon()
	{
		// Spawn bullet
		var clone = (Spatial)bullet_scene.Instance();
		var scene_root = (Node)GetTree().Root.GetChildren()[0];
		scene_root.AddChild(clone);

		// Set bullet parameters
		clone.GlobalTransform = this.GlobalTransform;
		clone.Scale = new Vector3(4, 4, 4);
		//clone.BULLET_DAMAGE = DAMAGE; bullet broken atm
		ammoInWeapon --;
	}
}
