using Godot;
using System;

public partial class Spawner : Node3D
{
	private PackedScene _planeScene;
	private Plane _activePlayer = null;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_planeScene = GD.Load<PackedScene>("res://plane.tscn");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public Plane SpawnPlayerPlane ()
	{
		// Create a new one
		Plane newPlayer = _planeScene.Instantiate<Plane>();
		newPlayer.GlobalTransform = GlobalTransform;
		this._activePlayer = newPlayer;
		GetTree().CurrentScene.AddChild(newPlayer);
		newPlayer.Name = "Plane";
		newPlayer.PhysicsUpdate += DebugOverlay.Overlay.OnPhysicsUpdate;
		return newPlayer;
	}
}
