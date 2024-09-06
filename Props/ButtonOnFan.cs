using Godot;
using System;

public partial class ButtonOnFan : Switch
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	protected override void _OnFlip()
	{
		Vector3 targetTransform;
		if (this.On)
		{
			targetTransform = new Vector3(0, -0.07f, 0);
		} else
		{
			targetTransform = new Vector3(0, 0, 0);
		}
		Position = targetTransform;
	}
}
