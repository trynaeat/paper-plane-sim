using Godot;

public partial class LightSwitch : Switch
{
	private Node3D _pivot;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		this._pivot = GetNode<Node3D>("Pivot");
		if (this._on) {
			this._pivot.RotateX(Mathf.DegToRad(-31));
		} else
		{
			this._pivot.RotateX(Mathf.DegToRad(31));
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	protected override void _OnFlip()
	{
		Vector3 targetRotation;
		if (this._on)
		{
			targetRotation = new Vector3(-31, 0, 0);
		}
		else
		{
			targetRotation = new Vector3(31, 0, 0);
		}
		this._pivot.RotationDegrees = targetRotation;
	}
}
