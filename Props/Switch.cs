using Godot;

public abstract partial class Switch : Node3D
{
	// Sets starting state
	[Export]
	public bool On = false;
	[Export]
	public int switchChannel;
	[Export]
	public AudioStream SoundHigh = null;
	[Export]
	public AudioStream SoundLow = null;
	[Signal]
	public delegate void SwitchedEventHandler(int switchChannel, bool state);
	protected bool _on = false;
	private bool _waitDebounce = false;
	private Timer _hitTimer;
	private AudioStreamPlayer3D _audio;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this._on = On;
		this._hitTimer = new Timer();
		this._hitTimer.OneShot = true;
		this._hitTimer.WaitTime = 0.5;
		this._hitTimer.Name = "HitTimer";
		AddChild(this._hitTimer);
		this._hitTimer.Timeout += OnHitTimerTimeout;
		if (SoundHigh != null || SoundLow != null)
		{
			this._audio = GetNode<AudioStreamPlayer3D>("AudioStream");
		}
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

	protected abstract void _OnFlip();

	private void _flipSwitch()
	{
		this._on = !this._on;
		this._waitDebounce = true;
		_hitTimer.Start();
		if (this._on && this._audio != null)
		{
			this._audio.Stream = SoundHigh;
		} else if (this._audio != null)
		{
			this._audio.Stream = SoundLow;
		}
		if (this._audio != null && this._audio.Stream != null)
		{
			this._audio.Play();
		}
		EmitSignal(SignalName.Switched, switchChannel, _on);
		_OnFlip();
	}
}
