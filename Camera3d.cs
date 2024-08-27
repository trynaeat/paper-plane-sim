using Godot;

public partial class Camera3d : Camera3D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _PhysicsProcess(double delta)
    {
		Vector3 newRotation = new Vector3(Rotation.X, Rotation.Y, Rotation.Z);
        Vector3 planeRotation = GetParent().GetNode<RigidBody3D>("Plane").Rotation;
		newRotation.X = planeRotation.X;
		newRotation.Y = planeRotation.Y;
		Rotation = newRotation;
    }
}
