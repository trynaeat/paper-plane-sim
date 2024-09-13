using Godot;
using System;

public partial class HitZone : Node3D
{
	[Signal]
	public delegate void PlaneHitEventHandler(HitZone zone);
	[Export]
	public float Debounce = 0.5f;
	private Area3D _hitArea;
	private Timer _debounceTimer;
	private bool _waitDebounce = false;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this._debounceTimer = new Timer();
		this._debounceTimer.OneShot = true;
		this._debounceTimer.WaitTime = Debounce;
		this._debounceTimer.Name = "HitTimer";
		this._debounceTimer.Timeout += _OnTimeout;
		AddChild(this._debounceTimer);
		_hitArea = GetNode<Area3D>("HitArea");
		_hitArea.AreaEntered += OnAreaEntered;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnAreaEntered (Area3D area)
	{
		if (this._waitDebounce)
		{
			return;
		}
		if (area.IsInGroup("planes"))
		{
			this._waitDebounce = true;
			this._debounceTimer.Start();
			EmitSignal(SignalName.PlaneHit, this);
		}
	}

	private void _OnTimeout()
	{
		this._waitDebounce = false;
	}
}
