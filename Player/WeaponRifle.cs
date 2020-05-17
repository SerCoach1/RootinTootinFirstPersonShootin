using Godot;
using System;

public class WeaponRifle : Spatial
{
    private const float DAMAGE = 4.0f;
    private const string IDLE_ANIM_NAME = "Rifle_idle";
    private const string FIRE_ANIM_NAME = "Rifle_fire";
    private bool isWeaponEnabled = false;

    private Player_Animation_Manager animManager = null;
    public Player playerNode = null;
    public override void _Ready()
    {
        animManager = GetNode<Player_Animation_Manager>("Animation_Player");
    }

    private void fireWeapon()
    {
        // Spawn bullet
        var ray = GetNode<RayCast>("Ray_Cast");
        ray.ForceRaycastUpdate();
        
        if(ray.IsColliding())
        {
            var body = ray.GetCollider();

            // if(body != playerNode && body.HasMethod("bullet_hit"))
            // {
            //     body.bullet_hit(DAMAGE, ray.GlobalTransform());
            // }
        }
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
            animManager.setAnimation("Rifle_equip");
        }

        return false;
    }

    private bool unequipWeapon()
    {
        // TODO: combine 2 if statements into one?
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
