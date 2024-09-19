using Godot;
using System;

public partial class UiOverlay : CanvasLayer
{
	private static UiOverlay _instance;
	public static UiOverlay Instance { get => _instance; }
	private Label _scoreLabel;
	private Label _timeLabel;
	private Plane _trackedPlane = null;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_instance = this;
		_scoreLabel = GetNode<Label>("UIContainer/ScoreLabel");
		_timeLabel = GetNode<Label>("UIContainer/TimeLabel");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		int score = Scoring.Instance.Score;
		_scoreLabel.Text = $"Score: {score}";
		if (_trackedPlane != null)
		{
			double time = _trackedPlane.Tracker.FlightTime;
			_timeLabel.Text = $"Time: {time:f2}";
		}
	}

	public void SetTrackedPlane(Plane plane)
	{
		_trackedPlane = plane;
	}

	public void UnsetTrackedPlane()
	{
		_trackedPlane = null;
	}
}
