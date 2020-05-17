using Godot;
using System;

public class WeaponPistol : Spatial
{
    private const float DAMAGE = 15.0f;
    private const string IDLE_ANIM_NAME = "Pistol_idle";
    private const string FIRE_ANIM_NAME = "Pistol_fire";
    private bool isWeaponEnabled = false;
    private PackedScene bullet_scene;
    private Player_Animation_Manager animManager = null;
    private Player playerNode = null;
    public override void _Ready()
    {
        bullet_scene = (PackedScene)ResourceLoader.Load("Bullet_Scene.tscn");
        animManager = GetNode<Player_Animation_Manager>("Animation_Player");
    }

    private void fireWeapon()
    {
        // Spawn bullet
        var clone = bullet_scene.Instance();
        GetTree().Root.AddChild(clone);
        
        // Find created bullet 
        var bullet = GetNode<Bullet_Scene>("Bullet_Scene");
        // Set bullet parameters
        bullet.GlobalTransform = this.GlobalTransform;
        bullet.Scale = new Vector3(4,4,4);
        bullet.BULLET_DAMAGE = DAMAGE;
    }

    private bool equipWeapon()
    {
        if(animManager.currentState == IDLE_ANIM_NAME)
        {
            isWeaponEnabled = true;
            return true;
        }

        if(animManager.currentState == "Idle_unarmed")
        {
            animManager.setAnimation("Pistol_equip");
        }

        return false;
    }

    private bool unequipWeapon()
    {
        if(animManager.currentState == IDLE_ANIM_NAME)
        {
            if(animManager.currentState != "Rifle_unequip")
            {
                animManager.setAnimation("Rifle_unequip");
            }
        }

        if(animManager.currentState == "Idle_unarmed")
        {
            isWeaponEnabled = false;
            return true;
        }

        return false;
    }
}
