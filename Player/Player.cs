using Godot;
using System;
using System.Collections.Generic;

public class Player : KinematicBody
{
	[Export]
	public float Gravity = -24.8f;
	[Export]
	public float MaxSpeed = 20.0f;
	[Export]
	public float JumpSpeed = 18.0f;
	[Export]
	public float Accel = 4.5f;
	[Export]
	public float Deaccel = 16.0f;
	[Export]
	public float MaxSlopeAngle = 40.0f;
	[Export]
	public float MouseSensitivity = 0.05f;

	private Vector3 _vel = new Vector3();
	private Vector3 _dir = new Vector3();

	private Camera _camera;
	private Spatial _rotationHelper;

	public Player_Animation_Manager animManager = null;

	private string currentWeaponName = "UNARMED";

	private bool reloadingWeapon = false;

	public Dictionary<string, Weapon> weapons = new Dictionary<string, Weapon>()
	{
		{"UNARMED",null}, {"KNIFE",null}, {"PISTOL",null}, {"RIFLE",null}
	};
	public readonly Dictionary<int, string> WEAPON_NUMBER_TO_NAME = new Dictionary<int, string>()
	{
		{0,"UNARMED"}, {1,"KNIFE"}, {2,"PISTOL"}, {3,"RIFLE"}
	};
	public readonly Dictionary<string, int> WEAPON_NAME_TO_NUMBER = new Dictionary<string, int>()
	{
		{"UNARMED",0}, {"KNIFE",1}, {"PISTOL",2}, {"RIFLE",3}
	};
	private bool changingWeapon = false;
	private string changingWeaponName = "UNARMED";

	private int health = 100;

	private Label UIStatusLabel;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_camera = GetNode<Camera>("Rotation_Helper/Camera");
		_rotationHelper = GetNode<Spatial>("Rotation_Helper");
		animManager = (Player_Animation_Manager)GetNode<AnimationPlayer>("Rotation_Helper/Model/Animation_Player");
		//TODO: hook up bullet firing method
		animManager.callbackFunction = fire_bullet;

		Input.SetMouseMode(Input.MouseMode.Captured);

		weapons["KNIFE"] = GetNode<WeaponKnife>("Rotation_Helper/Gun_Fire_Points/Knife_Point");
		weapons["PISTOL"] = GetNode<WeaponPistol>("Rotation_Helper/Gun_Fire_Points/Pistol_Point");
		weapons["RIFLE"] = GetNode<WeaponRifle>("Rotation_Helper/Gun_Fire_Points/Rifle_Point");

		var gunAimPointPos = GetNode<Spatial>("Rotation_Helper/Gun_Aim_Point").GlobalTransform.origin;

		foreach (var weapon in weapons)
		{
			var weaponNode = weapons[weapon.Key];
			if (weaponNode != null)
			{
				weaponNode.playerNode = this;
				weaponNode.LookAt(gunAimPointPos, new Vector3(0, 1, 0));
				weaponNode.RotateObjectLocal(new Vector3(0, 1, 0), Mathf.Deg2Rad(180));
			}
		}

		currentWeaponName = "UNARMED";
		changingWeaponName = "UNARMED";

		UIStatusLabel = GetNode<Label>("HUD/Panel/Gun_label");
		// flashlight = $Rotation_Helper/Flashlight
	}

	public override void _PhysicsProcess(float delta)
	{
		processInput(delta);
		processMovement(delta);
		processChangingWeapons(delta);
		processReloading(delta);
		processUI(delta);
	}

	private void processInput(float delta)
	{
		//  -------------------------------------------------------------------
		//  Walking
		_dir = new Vector3();
		Transform camXform = _camera.GlobalTransform;

		Vector2 inputMovementVector = new Vector2();

		if (Input.IsActionPressed("movement_forward"))
			inputMovementVector.y += 1;
		if (Input.IsActionPressed("movement_backward"))
			inputMovementVector.y -= 1;
		if (Input.IsActionPressed("movement_left"))
			inputMovementVector.x -= 1;
		if (Input.IsActionPressed("movement_right"))
			inputMovementVector.x += 1;

		inputMovementVector = inputMovementVector.Normalized();

		// Basis vectors are already normalized.
		_dir += -camXform.basis.z * inputMovementVector.y;
		_dir += camXform.basis.x * inputMovementVector.x;
		//  -------------------------------------------------------------------

		//  -------------------------------------------------------------------
		//  Jumping
		if (IsOnFloor())
		{
			if (Input.IsActionJustPressed("movement_jump"))
				_vel.y = JumpSpeed;
		}
		//  -------------------------------------------------------------------

		//  -------------------------------------------------------------------
		//  Capturing/Freeing the cursor
		if (Input.IsActionJustPressed("ui_cancel"))
		{
			if (Input.GetMouseMode() == Input.MouseMode.Visible)
				Input.SetMouseMode(Input.MouseMode.Captured);
			else
				Input.SetMouseMode(Input.MouseMode.Visible);
		}
		//  -------------------------------------------------------------------

		// Changing Weapons
		var weaponChangeNumber = WEAPON_NAME_TO_NUMBER[currentWeaponName];

		if (Input.IsKeyPressed((int)KeyList.Key1))
		{
			weaponChangeNumber = 0;
		}
		else if (Input.IsKeyPressed((int)KeyList.Key2))
		{
			weaponChangeNumber = 1;
		}
		else if (Input.IsKeyPressed((int)KeyList.Key3))
		{
			weaponChangeNumber = 2;
		}
		else if (Input.IsKeyPressed((int)KeyList.Key4))
		{
			weaponChangeNumber = 3;
		}

		if (Input.IsActionJustPressed("Shift_weapon_positive"))
		{
			weaponChangeNumber += 1;
		}
		else if (Input.IsActionJustPressed("Shift_weapon_negative"))
		{
			weaponChangeNumber -= 1;
		}

		weaponChangeNumber = Mathf.Clamp(weaponChangeNumber, 0, WEAPON_NAME_TO_NUMBER.Count - 1);

		if (!changingWeapon && !reloadingWeapon && WEAPON_NUMBER_TO_NAME[weaponChangeNumber] != currentWeaponName)
		{
			changingWeaponName = WEAPON_NUMBER_TO_NAME[weaponChangeNumber];
			changingWeapon = true;
		}
		//  -------------------------------------------------------------------

		// Firing the weapons
		if (Input.IsActionPressed("fire") && !changingWeapon && !reloadingWeapon)
		{
			var currentWeapon = weapons[currentWeaponName];
			if (currentWeapon != null)
			{
				if(currentWeapon.ammoInWeapon > 0)
				{
					if(animManager.currentState == currentWeapon.IDLE_ANIM_NAME)
					{
						animManager.setAnimation(currentWeapon.FIRE_ANIM_NAME);
					}
				}
				else
				{
					reloadingWeapon = true;
				}
			}
		}
		//  -------------------------------------------------------------------
		// Reloading

		if(!reloadingWeapon && !changingWeapon && Input.IsActionJustPressed("reload"))
		{
			var currentWeapon = weapons[currentWeaponName];
			if(currentWeapon != null && currentWeapon.CAN_RELOAD)
			{
				var currentAnimState = animManager.currentState;
				var isReloading = false;

				foreach(var weapon in weapons)
				{
					var weaponNode = weapons[weapon.Key];
					if(weaponNode != null && currentAnimState == weaponNode.RELOADING_ANIM_NAME)
					{
						isReloading = true;
					}

					if(!isReloading)
					{
						reloadingWeapon = true;
					}
				}
			}
		}
	}

	private void processMovement(float delta)
	{
		_dir.y = 0;
		_dir = _dir.Normalized();

		_vel.y += delta * Gravity;

		Vector3 hvel = _vel;
		hvel.y = 0;

		Vector3 target = _dir;

		target *= MaxSpeed;

		float accel;
		if (_dir.Dot(hvel) > 0)
			accel = Accel;
		else
			accel = Deaccel;

		hvel = hvel.LinearInterpolate(target, accel * delta);
		_vel.x = hvel.x;
		_vel.z = hvel.z;
		_vel = MoveAndSlide(_vel, new Vector3(0, 1, 0), false, 4, Mathf.Deg2Rad(MaxSlopeAngle));
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion && Input.GetMouseMode() == Input.MouseMode.Captured)
		{
			InputEventMouseMotion mouseEvent = @event as InputEventMouseMotion;
			_rotationHelper.RotateX(Mathf.Deg2Rad(mouseEvent.Relative.y * MouseSensitivity));
			RotateY(Mathf.Deg2Rad(-mouseEvent.Relative.x * MouseSensitivity));

			Vector3 cameraRot = _rotationHelper.RotationDegrees;
			cameraRot.x = Mathf.Clamp(cameraRot.x, -70, 70);
			_rotationHelper.RotationDegrees = cameraRot;
		}
	}

	private void processChangingWeapons(float delta)
	{
		if (changingWeapon)
		{
			var weaponUnequipped = false;
			var currentWeapon = weapons[currentWeaponName];

			if (currentWeapon == null)
			{
				weaponUnequipped = true;
			}
			else
			{
				if (currentWeapon.isWeaponEnabled)
				{
					weaponUnequipped = currentWeapon.unequipWeapon();
				}
				else
				{
					weaponUnequipped = true;
				}
			}

			if (weaponUnequipped)
			{
				var weaponEquipped = false;
				var weaponToEquip = weapons[changingWeaponName];

				if (weaponToEquip == null)
				{
					weaponEquipped = true;
				}
				else
				{
					if (!weaponToEquip.isWeaponEnabled)
					{
						weaponEquipped = weaponToEquip.equipWeapon();
					}
					else
					{
						weaponEquipped = true;
					}
				}

				if (weaponEquipped)
				{
					changingWeapon = false;
					currentWeaponName = changingWeaponName;
					changingWeaponName = String.Empty;
				}
			}
		}
	}

	public void fire_bullet()
	{
		if (changingWeapon)
		{
			return;
		}

		weapons[currentWeaponName].fireWeapon();
	}

	private void processUI(float delta)
	{
		if (currentWeaponName == "UNARMED" || currentWeaponName == "KNIFE")
		{
			UIStatusLabel.Text = "HEALTH: " + health;
		}
		else
		{
			var currentWeapon = weapons[currentWeaponName];
			UIStatusLabel.Text = "HEALTH: " + health + System.Environment.NewLine + "AMMO: " + currentWeapon.ammoInWeapon + "/" + currentWeapon.spareAmmo;
		}
	}

	private void processReloading(float delta)
	{
		if(reloadingWeapon)
		{
			var currentWeapon = weapons[currentWeaponName];
			if(currentWeapon != null)
			{
				currentWeapon.reloadWeapon();
			}
			reloadingWeapon = false;
		}
	}
}