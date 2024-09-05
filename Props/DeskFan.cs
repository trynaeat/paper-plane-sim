using Godot;
using Godot.Collections;
[Tool]
public partial class DeskFan : StaticBody3D
{
	[Export]
	public float FanForce;
	[Export]
	public Vector3 FanRotation;
	private Area3D _fanArea;
	private Node3D _origin;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this._fanArea = GetNode<Area3D>("FanOrigin/FanArea");
		this._origin = GetNode<Node3D>("FanOrigin");
		this._origin.RotationDegrees = FanRotation;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Engine.IsEditorHint())
		{
			this._origin.RotationDegrees = FanRotation;
		}
	}

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
		Vector3 dir = _origin.GlobalBasis.Z;
		Array<Area3D> areas = this._fanArea.GetOverlappingAreas();
		foreach (Area3D a in areas)
		{
			if (!a.IsInGroup("pushable")) {
				continue;
			}
			Pushable p = a as Pushable;
			float distance = a.GlobalPosition.DistanceSquaredTo(_origin.GlobalPosition);
			float forceScale = Mathf.Min(FanForce / distance, FanForce);
			Vector3 forceVector = dir * forceScale;
			p.Push(forceVector);
		}
    }
}
