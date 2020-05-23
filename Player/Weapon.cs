using Godot;
using System;

/* BASE CLASS FOR WEAPONS
    All Weapon Scenes should inhering from this class*/
public abstract class Weapon : Spatial
{
	// TODO: make virtual read-only property?
	protected float DAMAGE = 4.0f;
	public string IDLE_ANIM_NAME;
	public string FIRE_ANIM_NAME;
	protected string IDLE_UNARMED_ANIM;
	protected string WPN_UNEQUIP_ANIM;
	protected string WPN_EQUIP_ANIM;
	public bool isWeaponEnabled = false;
	protected Player_Animation_Manager animManager = null;
	public Player playerNode = null;

	public int ammoInWeapon; // amount in weapon currently
	public int spareAmmo; // ammo in reserve
	public int MAG_SIZE; // magazine size

	public bool CAN_RELOAD = true;
	public bool CAN_REFILL = true;

	public string RELOADING_ANIM_NAME;
	public override void _Ready()
	{
		animManager = (Player_Animation_Manager)GetNode<AnimationPlayer>("../../Model/Animation_Player");
	}

	public abstract void fireWeapon();

	public bool equipWeapon()
	{
		if (animManager.currentState == IDLE_ANIM_NAME)
		{
			isWeaponEnabled = true;
			return true;
		}

		if (animManager.currentState == IDLE_UNARMED_ANIM)
		{
			animManager.setAnimation(WPN_EQUIP_ANIM);
		}

		return false;
	}

	public bool unequipWeapon()
	{
		// TODO: combine 2 if statements into one?
		if (animManager.currentState == IDLE_ANIM_NAME)
		{
			if (animManager.currentState != WPN_UNEQUIP_ANIM)
			{
				animManager.setAnimation(WPN_UNEQUIP_ANIM);
			}
		}

		if (animManager.currentState == IDLE_UNARMED_ANIM)
		{
			isWeaponEnabled = false;
			return true;
		}

		return false;
	}

	public virtual bool reloadWeapon()
	{
		var canReload = false;

		if (playerNode.animManager.currentState == IDLE_ANIM_NAME)
		{
			canReload = true;
		}

		if (spareAmmo <= 0 || ammoInWeapon == MAG_SIZE)
		{
			canReload = false;
		}

		if (canReload)
		{
			var ammoNeeded = MAG_SIZE - ammoInWeapon;

			if (spareAmmo >= ammoNeeded)
			{
				spareAmmo -= ammoNeeded;
				ammoInWeapon = MAG_SIZE;
			}
			else
			{
				ammoInWeapon += spareAmmo;
				spareAmmo = 0;
			}

			playerNode.animManager.setAnimation(RELOADING_ANIM_NAME);

			return true;
		}
		return false;
	}
}
