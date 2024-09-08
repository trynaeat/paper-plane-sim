using Godot;

public partial class FollowCamPivot : Node3D
{
	[Export]
	public Node3D Target = null;

	[Export]
	public bool LockZ {
		get => GetNode<FollowCam>("FollowCam").LockZ;
		set => GetNode<FollowCam>("FollowCam").LockZ = value;
	}

	[Export]
	public float MouseSensitivity = -0.04f;

	[Export]
	public float ScrollSensitivity {
		get => GetNode<FollowCam>("FollowCam").ScrollSensitivity;
		set => GetNode<FollowCam>("FollowCam").ScrollSensitivity = value;
	}
	
	public Camera3D Camera;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Camera = GetNode<Camera3D>("FollowCam");
		RotationDegrees = new Vector3(0, 90, 0);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
		if (Target != null)
		{
			Position = Target.Position;
		}
    }

	public override void _Input(InputEvent @event)
    {
        base._Input(@event);
		if (Input.MouseMode != Input.MouseModeEnum.Captured)
		{
			return;
		}
		if (@event is InputEventMouseMotion eventMouseButton)
		{
			RotateY(Mathf.DegToRad(eventMouseButton.Relative.X * MouseSensitivity));
			Rotate(GlobalTransform.Basis.X, Mathf.DegToRad(eventMouseButton.Relative.Y * MouseSensitivity));
		}
    }

	public void ResetRotation ()
	{
		RotationDegrees = new Vector3(0, 90, 0);
	}
}
