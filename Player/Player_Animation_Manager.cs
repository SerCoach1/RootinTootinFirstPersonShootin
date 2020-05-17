using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class Player_Animation_Manager : AnimationPlayer
{
	private Dictionary<string, string[]> states = new Dictionary<string, string[]>()
	{
	{"Idle_unarmed", new string[] {"Knife_equip", "Pistol_equip", "Rifle_equip", "Idle_unarmed"}},

	{"Pistol_equip",new string[]{ "Pistol_idle"}},
	{"Pistol_fire",new string[]{ "Pistol_idle"}},
	{"Pistol_idle",new string[]{ "Pistol_fire", "Pistol_reload", "Pistol_unequip", "Pistol_idle"}},
	{"Pistol_reload",new string[]{ "Pistol_idle"}},
	{"Pistol_unequip",new string[]{ "Idle_unarmed"}},

	{"Rifle_equip",new string[]{ "Rifle_idle"}},
	{"Rifle_fire",new string[]{ "Rifle_idle"}},
	{"Rifle_idle",new string[]{ "Rifle_fire", "Rifle_reload", "Rifle_unequip", "Rifle_idle"}},
	{"Rifle_reload",new string[]{ "Rifle_idle"}},
	{"Rifle_unequip",new string[]{ "Idle_unarmed"}},

	{"Knife_equip",new string[]{ "Knife_idle"}},
	{"Knife_fire",new string[]{ "Knife_idle"}},
	{"Knife_idle",new string[]{ "Knife_fire", "Knife_unequip", "Knife_idle"}},
	{"Knife_unequip",new string[]{ "Idle_unarmed"}},
	};

	private Dictionary<string, float> animationSpeeds = new Dictionary<string, float>()
	{
		{"Idle_unarmed",1.0f},

		{"Pistol_equip",1.4f},
		{"Pistol_fire",1.8f},
		{"Pistol_idle",1.0f},
		{"Pistol_reload",1.0f},
		{"Pistol_unequip",1.4f},

		{"Rifle_equip",2.0f},
		{"Rifle_fire",6.0f},
		{"Rifle_idle",1.0f},
		{"Rifle_reload",1.45f},
		{"Rifle_unequip",2.0f},

		{"Knife_equip",1.0f},
		{"Knife_fire",1.35f},
		{"Knife_idle",1.0f},
		{"Knife_unequip",1.0f}
	};

	string currentState = null;

	public delegate void FiringFunction();
	public FiringFunction callbackFunction = null;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		setAnimation("Idle_unarmed");

		// Call animationEnded method when we get animation_ended signal
		Connect("animation_finished", this, "animationEnded");
	}

	private bool setAnimation(string animationName)
	{
		if (animationName == currentState)
		{
			GD.Print("Player_Animation_Manager.cs -- WARNING: animation is already " + animationName);
			return true;
		}

		if (HasAnimation(animationName))
		{
			if (currentState != null)
			{
				var possibleAnimations = states[currentState];
				if (possibleAnimations.Contains(animationName))
				{
					currentState = animationName;
					Play(animationName, -1, animationSpeeds[animationName]);
					return true;
				}
				else
				{
					GD.Print("Player_Animation_Manager.cs -- WARNING: Cannot change animation to ", animationName, " from ", currentState);
					return false;
				}
			}
			else
			{
				currentState = animationName;
				Play(animationName, -1, animationSpeeds[animationName]);
				return true;
			}
		}
		return false;
	}

	// TODO: Implement proper state machine
	private void animationEnded(string animationName)
	{
		switch (currentState)
		{
			// UNARMED transitions
			case "Idle_unarmed":
				break;

			// KNIFE transitions	
			case "Knife_equip":
				setAnimation("Knife_idle");
				break;
			case "Knife_idle":
				break;
			case "Knife_fire":
				setAnimation("Knife_idle");
				break;
			case "Knife_unequip":
				setAnimation("Idle_unarmed");
				break;

			// PISTOL transitions
			case "Pistol_equip":
				setAnimation("Pistol_idle");
				break;
			case "Pistol_idle":
				break;
			case "Pistol_fire":
				setAnimation("Pistol_idle");
				break;
			case "Pistol_unequip":
				setAnimation("Idle_unarmed");
				break;
			case "Pistol_reload":
				setAnimation("Pistol_idle");
				break;

			// RIFLE transitions
			case "Rifle_equip":
				setAnimation("Rifle_idle");
				break;
			case "Rifle_idle":
				break;
			case "Rifle_fire":
				setAnimation("Rifle_idle");
				break;
			case "Rifle_unequip":
				setAnimation("Idle_unarmed");
				break;
			case "Rifle_reload":
				setAnimation("Rifle_idle");
				break;
		}
	}

	public void animationCallback()
	{
		if(callbackFunction == null)
		{
			GD.Print("Player_Animation_Manager.cs -- WARNING: No callback function for the animation to call!");
		}
		else
		{
			callbackFunction();
		}
	}
}