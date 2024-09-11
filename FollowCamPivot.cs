using Godot;

enum ChaseState
{
	Waiting,
	Chasing,
	None,
}
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

	[Export]
	public float ChaseDistance;

	[Export]
	public float ChaseSpeed;
	
	public Camera3D Camera;
	private ChaseState _chaseState = ChaseState.None;

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
		if (_chaseState != ChaseState.None)
		{
			_DoChase(delta);
		} else if (Target != null)
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

	private void _DoChase (double delta)
	{
		float distance2 = GlobalPosition.DistanceSquaredTo(Target.GlobalPosition);
		if (distance2 <= 0.01)
		{
			this._chaseState = ChaseState.None;
			return;
		}
		if (distance2 >= ChaseDistance * ChaseDistance)
		{
			this._chaseState = ChaseState.Chasing;
		}
		if (_chaseState == ChaseState.Chasing)
		{
			Vector3 dir = GlobalPosition.DirectionTo(Target.GlobalPosition);
			GlobalPosition += dir * ChaseSpeed * (float)delta;
		}
	}

	public void ResetRotation ()
	{
		RotationDegrees = new Vector3(0, 90, 0);
	}

	/**
	 * Chase after target once it reaches given distance
	 * Chase at given speed
	 */
	public void StartChase ()
	{
		this._chaseState = ChaseState.Waiting;
	}
}
