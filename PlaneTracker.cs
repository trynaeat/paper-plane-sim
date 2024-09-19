using Godot;
using System;

public partial class PlaneTracker : Node
{
	private bool _started = false;
	private double _flightTime = 0;
	private double _stoppedTime = 0;
	[Export]
	public Plane Plane { get; set; }

	public double FlightTime { get => _flightTime; }
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (_started)
		{
			_flightTime += delta;
		}
		if (_checkIfStopped())
		{
			_stoppedTime += delta;
		}
		if (_stoppedTime >= 0.5)
		{
			StopTracking();
		}
	}

	public void StartTracking()
	{
		_started = true;
	}

	public void StopTracking()
	{
		_started = false;
	}

	/**
	 *	Check if the plane has crashed/landed (effectively 0 velocity for a while)
	 */
	private bool _checkIfStopped()
	{
		float speed = Plane.LinearVelocity.Length();
		if (Plane.GetCollidingBodies().Count > 0 && speed < 0.1)
		{
			return true;
		}
		return false;
	}
}
