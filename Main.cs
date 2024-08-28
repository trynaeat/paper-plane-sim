using Godot;

public partial class Main : Node
{
	[Export]
	public Vector3 SpawnLocation;

	private PackedScene _planeScene;

	private Plane _activePlayer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_activePlayer = GetNode<Plane>("Plane");
		_planeScene = GD.Load<PackedScene>("res://plane.tscn");
		Input.MouseMode = Input.MouseModeEnum.Captured;
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
			FollowCamPivot cam = GetNode<FollowCamPivot>("FollowCam");
			// Destroy old one
			cam.Target = null;
			Plane player = this._activePlayer;
			player.Destroy();
			// Create a new one
			Plane newPlayer = _planeScene.Instantiate<Plane>();
			newPlayer.Position = SpawnLocation;
			this._activePlayer = newPlayer;
			// Attach camera to it
			cam.Target = newPlayer;
			
			AddChild(newPlayer);
			newPlayer.Name = "Plane";
		}
	}
}
