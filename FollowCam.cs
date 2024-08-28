using Godot;

public partial class FollowCam : Camera3D
{
	[Export]
	public bool LockZ = false;

	[Export]
	public float ScrollSensitivity = 0.1f;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _PhysicsProcess(double delta)
    {
		// Lock along local Z axis
		if (LockZ)
		{
			Vector3 parentRotation = GetParent<Node3D>().Rotation;
			Vector3 newRotation = Rotation;
			newRotation.Z = -1 * parentRotation.Z;
			Rotation = newRotation;
		}
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
		if (Input.MouseMode != Input.MouseModeEnum.Captured)
		{
			return;
		}
		if (@event is InputEventMouseButton eventMouseButton)
		{
			Vector3 newPos = Position;
			switch (eventMouseButton.ButtonIndex)
			{
				case MouseButton.WheelUp:
					newPos.Z -= ScrollSensitivity;
					break;
				case MouseButton.WheelDown:
					newPos.Z += ScrollSensitivity;
					break;
			}
			Position = newPos;
		}
    }
}
