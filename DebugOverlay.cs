using System;
using Godot;

public partial class DebugOverlay : CanvasLayer
{
	public static Control Draw {get; set; } = null;
	// Called when the node enters the scene tree for the first time.

	private Label _velocity;
	private Label _altitude;
	private Label _aoa;
	private Label _fps;
	public override void _Ready()
	{
		Draw = GetNode<Control>("Control");
		_velocity = GetNode<Label>("DebugContainer/Velocity");
		_altitude = GetNode<Label>("DebugContainer/Altitude");
		_aoa = GetNode<Label>("DebugContainer/AoA");
		_fps = GetNode<Label>("DebugContainer/FPS");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnPhysicsUpdate(float velocity, float altitude, float aoa)
	{
		this._velocity.Text = $"Velocity: {velocity:f1}";
		this._altitude.Text = $"Altitude: {altitude:f1}";
		this._aoa.Text = $"AoA: {aoa:f1}";
		this._fps.Text = $"FPS: {Engine.GetFramesPerSecond()}";
	}
	
}
