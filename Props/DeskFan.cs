using Godot;
using Godot.Collections;
[Tool]
public partial class DeskFan : Node3D
{
	[Export]
	public float FanForce;
	// X = Rotation up/down
	// Y = rotation around vertical base
	[Export]
	public Vector2 FanRotation;
	private Area3D _fanArea;
	private Node3D _origin;
	private AnimationPlayer _animPlayer;
	private int _pivotTop;
	private int _pivotBtm;
	private Skeleton3D _skeleton;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this._skeleton = GetNode<Skeleton3D>("PivotTop/Skeleton3D");
		this._pivotTop = this._skeleton.FindBone("Bone.PivotTop");
		this._pivotBtm = this._skeleton.FindBone("Bone.PivotBottom");
		this._fanArea = GetNode<Area3D>("FanOrigin/FanArea");
		this._origin = GetNode<Node3D>("FanOrigin");
		this._animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		this._origin.RotationDegrees = new Vector3(FanRotation.X, FanRotation.Y, 0);
		this._skeleton.SetBonePoseRotation(this._pivotBtm, Quaternion.FromEuler(new Vector3(0, Mathf.DegToRad(FanRotation.Y), 0)));
		this._skeleton.SetBonePoseRotation(this._pivotTop, Quaternion.FromEuler(new Vector3(Mathf.DegToRad(FanRotation.X), 0, 0)));
		this._animPlayer.Play("fan_spin");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Engine.IsEditorHint())
		{
			this._skeleton.SetBonePoseRotation(this._pivotBtm, Quaternion.FromEuler(new Vector3(0, Mathf.DegToRad(FanRotation.Y), 0)));
			this._skeleton.SetBonePoseRotation(this._pivotTop, Quaternion.FromEuler(new Vector3(Mathf.DegToRad(FanRotation.X), 0, 0)));
			this._origin.RotationDegrees = new Vector3(FanRotation.X, FanRotation.Y, 0);
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
