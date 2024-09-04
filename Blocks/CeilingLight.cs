using Godot;

public partial class CeilingLight : StaticBody3D
{
	// Optional channel to hook this to a light switch.
	[Export]
	public int SwitchChannel;

	private Material _materialOff;
	private Material _materialOn;
	private OmniLight3D _light;

	private bool _state;

	// Whether this is on or not
	public bool State {
		get => _state;
		set {
			_state = value;
			_updateState();
		}
	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this._materialOff = GD.Load<Material>("res://Materials/CeilingLightNonEmissive.tres");
		this._materialOn = GD.Load<Material>("res://Materials/CeilingLightEmissive.tres");
		this._light = GetNode<OmniLight3D>("OmniLight3D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnLightSwitchSwitched(int channel, bool state)
	{
		if (SwitchChannel == channel)
		{
			State = state;
		}
	}

	private void _updateState()
	{
		MeshInstance3D mainMesh = GetNode<MeshInstance3D>("FixtureMain");
		if (this._state)
		{
			mainMesh.SetSurfaceOverrideMaterial(1, _materialOn);
			this._light.LightEnergy = 3;
		}
		else
		{
			mainMesh.SetSurfaceOverrideMaterial(1, _materialOff);
			this._light.LightEnergy = 0;
		}
	}
}
