#ifndef BULLET_HPP
#define BULLET_HPP

#include <Godot.hpp>
#include <Spatial.hpp>
#include <PhysicsBody.hpp>

namespace godot
{
	class Bullet : public Spatial
	{
		GODOT_CLASS(Bullet, Spatial)
	private:
		float BULLET_SPEED;
		float BULLET_DAMAGE;
		float KILL_TIMER;
		float timer;
		bool hitSomething;

	public:
		static void _register_methods();
		Bullet();
		~Bullet(); //DO NOT DECLARE IF YOU'RE NOT GOING TO IMPLEMENT
		void _init();
		void _physics_process(float delta);
		void _ready();
		void collided(PhysicsBody *body);
	};
} // namespace godot

#endif