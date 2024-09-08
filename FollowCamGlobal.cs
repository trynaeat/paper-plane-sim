using Godot;
using System;

public partial class FollowCamGlobal : Node
{
	public static FollowCamPivot Camera { get => _camera; }
	private static FollowCamPivot _camera;
	private PackedScene _camScene = null;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_camScene = GD.Load<PackedScene>("res://follow_cam.tscn");
		_camera = this._camScene.Instantiate<FollowCamPivot>();
		GetTree().CurrentScene.AddChild(_camera);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
