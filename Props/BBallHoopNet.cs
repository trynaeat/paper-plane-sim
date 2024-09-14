using Godot;
using System;

public partial class BBallHoopNet : Node3D
{
	private PhysicalBoneSimulator3D _sim;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_sim = GetNode<PhysicalBoneSimulator3D>("Net_Armature/Skeleton3D/PhysicalBoneSimulator3D");
		Godot.Collections.Array<Godot.StringName> bones = new Godot.Collections.Array<Godot.StringName>();
		bones.Add("Bone.001");
		bones.Add("Bone.002");
		bones.Add("Bone.003");
		bones.Add("Bone.004");
		_sim.PhysicalBonesStartSimulation(bones);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
