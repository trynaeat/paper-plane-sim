using Godot;
using System;

public partial class Scoring : Node
{
	private static Scoring _instance;
	public static Scoring Instance { get => _instance; }

	public int Score { get => _score; }

	private int _score = 0;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_instance = this;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void AddScore (int score)
	{
		_score += score;
		// TODO actual scoring
	}
}
