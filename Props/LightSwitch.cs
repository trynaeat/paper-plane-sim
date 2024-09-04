using Godot;

public partial class LightSwitch : Node3D
{
	// Sets starting state
	[Export]
	public bool On = false;
	[Export]
	public int switchChannel;
	[Signal]
	public delegate void SwitchedEventHandler(int switchChannel, bool state);
	private bool _on = false;
	private bool _waitDebounce = false;
	private Node3D _pivot;
	private AudioStreamPlayer3D _audio;

	private AudioStream _soundHigh;
	private AudioStream _soundLow;

	private Timer _hitTimer;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this._on = On;
		this._pivot = GetNode<Node3D>("Pivot");
		this._hitTimer = GetNode<Timer>("HitTimer");
		if (this._on) {
			this._pivot.RotateX(Mathf.DegToRad(-31));
		} else
		{
			this._pivot.RotateX(Mathf.DegToRad(31));
		}
		this._audio = GetNode<AudioStreamPlayer3D>("AudioStream");
		this._soundHigh = GD.Load<AudioStream>("res://Sounds/Light_Click.wav");
		this._soundLow = GD.Load<AudioStream>("res://Sounds/Light_Click_Low.wav");
		EmitSignal(SignalName.Switched, switchChannel, _on);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnAreaEntered(Area3D area)
	{
		if(!_waitDebounce)
		{
			_flipSwitch();
		}
	}

	public void OnHitTimerTimeout()
	{
		this._waitDebounce = false;
	}

	private void _flipSwitch ()
	{
		this._waitDebounce = true;
		_hitTimer.Start();
		Vector3 targetRotation;
		this._on = !this._on;
		if (this._on)
		{
			this._audio.Stream = this._soundHigh;
			targetRotation = new Vector3(-31, 0, 0);
		}
		else
		{
			this._audio.Stream = this._soundLow;
			targetRotation = new Vector3(31, 0, 0);
		}
		this._pivot.RotationDegrees = targetRotation;
		EmitSignal(SignalName.Switched, switchChannel, _on);
		this._audio.Play();
	}
}
