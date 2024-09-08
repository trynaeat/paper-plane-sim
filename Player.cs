using Godot;
using System;

public partial class Player : Node3D
{
	private Spawner _spawner;
	private CanvasLayer _ui;
	private Camera3D _playerCam;
	private bool _active = false;
	private Plane _activePlane = null;
	[Export]
	public float MouseSensitivity = -0.04f;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this._ui = GetNode<CanvasLayer>("PlayerUI");
		this._spawner = GetNode<Spawner>("Spawner");
		this._playerCam = GetNode<Camera3D>("Camera3D");
		_active = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _Input(InputEvent @event)
    {
		 base._Input(@event);
		// Reset to player
		if (@event.IsActionPressed("respawn") && _activePlane != null)
		{
			// Stop reacting to user inputs on old one
			_activePlane.Deactivate();
			_activePlane = null;
			// Come back to player view
			FollowCamGlobal.Camera.Camera.ClearCurrent();
			_active = true;
			_ui.Visible = true;
			_playerCam.Current = true;
		}
		if (!_active)
		{
			return;
		}
		if (Input.MouseMode != Input.MouseModeEnum.Captured)
		{
			return;
		}
		if (@event is InputEventMouseMotion eventMouseButton)
		{
			RotateY(Mathf.DegToRad(eventMouseButton.Relative.X * MouseSensitivity));
			Rotate(GlobalTransform.Basis.X, Mathf.DegToRad(eventMouseButton.Relative.Y * MouseSensitivity));
		}
		if (@event.IsActionPressed("throw_plane"))
		{
			Plane newPlane = this._spawner.SpawnPlayerPlane();
			FollowCamGlobal.Camera.Target = newPlane;
			FollowCamGlobal.Camera.Camera.Current = true;
			FollowCamGlobal.Camera.GlobalTransform = newPlane.GlobalTransform;
			_active = false;
			_activePlane = newPlane;
			this._playerCam.ClearCurrent();
			this._ui.Visible = false;
		}
    }
}
