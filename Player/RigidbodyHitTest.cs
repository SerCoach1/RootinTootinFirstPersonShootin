using Godot;
using System;

public class RigidbodyHitTest : RigidBody
{
    public const int BASE_BULLET_BOOST = 9;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public void bullet_hit(float damage, Transform bulletGlobalTrans)
    {
        var directionVect = bulletGlobalTrans.basis.z.Normalized() * BASE_BULLET_BOOST;

        ApplyImpulse((bulletGlobalTrans.origin - GlobalTransform.origin).Normalized(), directionVect * damage);
    }
}
