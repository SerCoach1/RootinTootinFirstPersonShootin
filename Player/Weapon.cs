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
}
