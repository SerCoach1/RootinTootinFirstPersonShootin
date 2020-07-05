#include "bullet.hpp"

using namespace godot;

void Bullet::_register_methods()
{
	register_method("_PhysicsProcess", &Bullet::_PhysicsProcess);
	register_method("collided", &Bullet::collided); // NEED THIS
}

void Bullet::_init()
{
	auto area = get_node("Area");
	// area->connect("body_entered", this, "collided");

	BULLET_SPEED = 200.0f;
	BULLET_DAMAGE = 15.0f;
	KILL_TIMER = 2.0f;
	timer = 0.0f;
	hitSomething = false;
}

void Bullet::_PhysicsProcess(float delta)
{
	auto forward_dir = get_global_transform().basis.z.normalized();
	global_translate(forward_dir * BULLET_SPEED * delta);

	timer += delta;
	if (timer >= KILL_TIMER)
	{
		queue_free();
	}
}

Bullet::Bullet() {}

Bullet::~Bullet() {}

void Bullet::collided(PhysicsBody body)
{
	if (!hitSomething)
	{
		if (body.has_method("bullet_hit"))
		{
			body.call("bullet_hit", BULLET_DAMAGE, get_global_transform());
		}

		hitSomething = true;
		queue_free();
	}
}