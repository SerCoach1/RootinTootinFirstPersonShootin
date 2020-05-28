using Godot;
using System;

public class HealthPickup : Spatial
{
	[Export(PropertyHint.Enum, "fullsize, small")]
	int kitSize = 0;

	readonly int[] HEALTH_AMOUNTS = { 70, 30 };

	const int RESPAWN_TIME = 20;
    float respawnTimer = 0;

	bool isReady = false;

	public override void _Ready()
	{
		GetNode<Area>("Holder/Health_Pickup_Trigger").Connect("body_entered", this, "trigger_body_entered");

		isReady = true;

		kitSizeChangeValues(0, false);
		kitSizeChangeValues(1, false);
		kitSizeChangeValues(kitSize, true);
	}

	public override void _PhysicsProcess(float delta)
    {
        if(respawnTimer > 0)
        {
            respawnTimer -= delta;

            if(respawnTimer <= 0)
            {
                kitSizeChangeValues(kitSize, true);
            }
        }
    }

	private void kitSizeChangeValues(int size, bool enable)
	{
		if (size == 0)
		{
			GetNode<CollisionShape>("Holder/Health_Pickup_Trigger/Shape_Kit").Disabled = !enable;
			GetNode<Spatial>("Holder/Health_Kit.visible").Visible = enable;
		}
		else if (size == 1)
		{
			GetNode<CollisionShape>("Holder/Health_Pickup_Trigger/Shape_Kit_Small").Disabled = !enable;
			GetNode<Spatial>("Holder/Health_Kit_Small.visible").Visible = enable;
		}
	}
}
