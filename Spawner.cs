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

	public void SpawnPlayerPlane ()
	{
		// Create a new one
		Plane newPlayer = _planeScene.Instantiate<Plane>();
		newPlayer.Transform = Transform;
		this._activePlayer = newPlayer;
		// Attach camera to it
		FollowCamGlobal.Camera.Target = newPlayer;
		FollowCamGlobal.Camera.Camera.Current = true;
		GetTree().CurrentScene.AddChild(newPlayer);
		newPlayer.Name = "Plane";
		newPlayer.PhysicsUpdate += DebugOverlay.Overlay.OnPhysicsUpdate;
	}
}
