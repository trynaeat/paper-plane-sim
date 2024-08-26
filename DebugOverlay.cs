using Godot;

public partial class DebugOverlay : CanvasLayer
{
	public static Control Draw {get; set; } = null;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Draw = GetNode<Control>("Control");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
}
