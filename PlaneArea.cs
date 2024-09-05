using Godot;
using System;

public partial class PlaneArea : Area3D, Pushable
{
	[Signal]
	public delegate void PushedEventHandler(float x, float y, float z);
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void Push(Vector3 force)
	{
		EmitSignal(SignalName.Pushed, force.X, force.Y, force.Z);
	}
}
