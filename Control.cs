using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

partial class Vector
{
	public Node3D Obj { get; }
	private string _property;
	public int width = 5;
	public float scale = 1.0f;
	public Color color = new Color(0, 1, 0);

	public Vector(Node3D obj, string prop, float scale, Color color)
	{
		this.Obj = obj;
		this._property = prop;
		this.scale = scale;
		this.color = color;
	}

	public void DrawVec (Control node, Camera3D camera)
	{
		Vector2 start = camera.UnprojectPosition(Obj.GlobalTransform.Origin);
		Vector2 end = camera.UnprojectPosition(Obj.GlobalTransform.Origin + Obj.Get(_property).As<Vector3>() * scale);
		node.DrawLine(start, end, color, width);
		node.DrawTriangle(end, start.DirectionTo(end), width * 2, color);
	}
}

public partial class Control : Godot.Control
{

	[Export]
    public Camera3D camera;
	[Export]
    public RigidBody3D player;
	[Export]
    public int width = 15;
	private List<Vector> vectors = new List<Vector>();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		QueueRedraw();
	}

    public override void _Draw()
    {
        base._Draw();
		this.camera = GetViewport().GetCamera3D();
		foreach(Vector v in vectors)
		{
			v.DrawVec(this, camera);
		}
    }

	public void DrawTriangle(Vector2 pos, Vector2 dir, int size, Color color)
	{
		Vector2 a = pos + dir * size;
		Vector2 b = pos + dir.Rotated(2 * (float)Math.PI / 3) * size;
		Vector2 c = pos + dir.Rotated(4 * (float)Math.PI / 3) * size;
		Vector2[] points = new Vector2[]{ a, b, c };
		DrawPolygon(points, new Color[]{ color });
	}

	public void AddVector(Node3D obj, string prop, float scale, Color color)
	{
		vectors.Add(new Vector(obj, prop, scale, color));
	}

	public void RemoveVectors(Node3D obj)
	{
		vectors.RemoveAll(v => v.Obj == obj);
	}
}
