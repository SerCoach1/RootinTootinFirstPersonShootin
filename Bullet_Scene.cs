using Godot;
using System;

public class Bullet_Scene : Spatial
{

	public float BULLET_SPEED = 70.0f;
	public float BULLET_DAMAGE = 15.0f;

	public const float KILL_TIMER = 4.0f;
	float timer = 0.0f;

	bool hitSomething = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var area = GetNode<Area>("Area");
		area.Connect("body_entered", this, "collided");
	}

	public override void _PhysicsProcess(float delta)
	{
		var forward_dir = GlobalTransform.basis.z.Normalized();
		GlobalTranslate(forward_dir * BULLET_SPEED * delta);

		timer += delta;
		if (timer >= KILL_TIMER)
		{
			QueueFree();
		}
	}

	public void collided(PhysicsBody body)
	{
		if (hitSomething == false)
		{
			if (body.HasMethod("bullet_hit"))
			{
				body.Call("bullet_hit", BULLET_DAMAGE, GlobalTransform);
			}

			hitSomething = true;
			QueueFree();
		}
	}
}
