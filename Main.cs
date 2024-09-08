
using Godot;

public partial class Main : Node
{
	private PackedScene _planeScene;

	private DebugOverlay _debug;
	private Spawner _spawner;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_planeScene = GD.Load<PackedScene>("res://plane.tscn");
		_debug = GetNode<DebugOverlay>("DebugOverlay");
		_spawner = GetNode<Spawner>("Spawner");
		Input.MouseMode = Input.MouseModeEnum.Captured;
		_spawner.SpawnPlayerPlane();
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
			_spawner.SpawnPlayerPlane();
		}
	}
}
