
using Godot;

public partial class Main : Node
{
	private PackedScene _planeScene;

	private Plane _activePlayer;
	private DebugOverlay _debug;

	private Transform3D _startSpawn;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_activePlayer = GetNode<Plane>("Plane");
		_planeScene = GD.Load<PackedScene>("res://plane.tscn");
		_debug = GetNode<DebugOverlay>("DebugOverlay");
		Input.MouseMode = Input.MouseModeEnum.Captured;
		_activePlayer.PhysicsUpdate += _debug.OnPhysicsUpdate;
		this._startSpawn = this._activePlayer.Transform;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// Capture Mouse
		if (Input.IsActionJustPressed("escape_mouse"))
		{
			Input.MouseMode = Input.MouseModeEnum.Visible;
		}
		if (Input.IsActionJustPressed("click_mouse"))
		{
			Input.MouseMode = Input.MouseModeEnum.Captured;
		}

		// Respawn
		if (Input.IsActionJustPressed("respawn"))
		{
			_SpawnPlane();
		}
	}

	private void _SpawnPlane ()
	{
		FollowCamPivot cam = GetNode<FollowCamPivot>("FollowCam");
		// Destroy old one
		cam.Target = null;
		Plane player = this._activePlayer;
		player.Destroy();
		// Create a new one
		Plane newPlayer = _planeScene.Instantiate<Plane>();
		newPlayer.Transform = this._startSpawn;
		this._activePlayer = newPlayer;
		// Attach camera to it
		cam.Target = newPlayer;
		
		AddChild(newPlayer);
		newPlayer.Name = "Plane";
		newPlayer.PhysicsUpdate += _debug.OnPhysicsUpdate;
	}
}
