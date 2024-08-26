using System;
using Godot;

public partial class Plane : CharacterBody3D
{
    [Export]
    public float FallAcceleration { get; set; } = 9.8f;

    [Export]
    public float Mass { get; set; } = 5;

    [Export]
    public float LiftCoeff { get; set; } = 20;

    [Export]
    public float DragCoeff { get; set; } = 20;

    [Export]
    public int StartSpeed { get; set; } = 5;

    [Export]
    public int CriticalAoA { get; set; } = 20;

    // Degrees/Second
    [Export]
    public float MaxRollRate { get; set; } = 90;
    [Export]
    public float RollForce { get; set; } = 0.1f;
    [Export]
    public float PitchForce { get; set; } = 0.1f;
    [Export]
    public float MaxPitchRate { get; set; } = 35;
    [Export]
    public Vector3 Lift { get; set; }
    [Export]
    public Vector3 Drag { get; set; }
    [Export]
    public Vector3 FGravity { get; set; }
    private Vector3 _targetVelocity = Vector3.Zero;
    private float _rollSpeed = 0;
    private float _pitchSpeed = 0;
    private double _aoa = 0;

    public override void _Ready()
    {
        base._Ready();
        _targetVelocity = StartSpeed * this.Basis.Z * -1;
        Velocity = _targetVelocity;
        DebugOverlay.Draw.AddVector(this, CharacterBody3D.PropertyName.Velocity, 1f, new Color(0, 1, 0));
        DebugOverlay.Draw.AddVector(this, "Lift", 0.01f, new Color(0, 0, 1));
        DebugOverlay.Draw.AddVector(this, "Drag", 0.01f, new Color(1, 0, 0));
        DebugOverlay.Draw.AddVector(this, "FGravity", 0.01f, new Color(1, 0, 1));
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector3 forward = -1 * this.Basis.Z;
        this._aoa = Mathf.Abs(forward.AngleTo(Velocity) * (180.0 / Math.PI));
        if (Velocity.Length() < 0.1) {
            this._aoa = 90;
        }
        GD.Print(this._aoa);
        DoPitch(delta);
        DoRoll(delta);
        Vector3 lift = DoLift();
        Vector3 drag = DoDrag();
        Vector3 gravity = FallAcceleration * Mass * Vector3.Down;
        this.FGravity = gravity;
        Vector3 fNet = lift + drag + gravity;
        Vector3 acceleration = fNet / Mass;
        _targetVelocity += acceleration * (float)delta;
        Velocity = _targetVelocity;
        MoveAndSlide();
    }

    private void DoRoll (double delta)
    {
        if (Input.IsActionPressed("bank_left"))
        {
            this._rollSpeed = Mathf.Min(this._rollSpeed + RollForce, MaxRollRate);
        }
        else if (Input.IsActionPressed("bank_right"))
        {
            this._rollSpeed = Mathf.Max(this._rollSpeed - RollForce, -1 * MaxRollRate);
        }
        else
        {
            float newSpeed = Mathf.Max(0, Mathf.Abs(this._rollSpeed) - RollForce);
            if (this._rollSpeed < 0)
            {
                newSpeed = -1 * newSpeed;
            }
            this._rollSpeed = newSpeed;
        }
        RotateZ(this._rollSpeed * (float)delta);
    }

    private void DoPitch (double delta)
    {
       if (Input.IsActionPressed("pitch_up"))
       {
        this._pitchSpeed = Mathf.Min(this._pitchSpeed + PitchForce, MaxPitchRate);
       }
       else if (Input.IsActionPressed("pitch_down"))
       {
        this._pitchSpeed = Mathf.Max(this._pitchSpeed - PitchForce, -1 * MaxPitchRate);
       }
       else {
        float newSpeed = Mathf.Max(0, Mathf.Abs(this._pitchSpeed) - PitchForce);
        if (this._pitchSpeed < 0)
        {
            newSpeed = -1 * newSpeed;
        }
        this._pitchSpeed = newSpeed;
       }
       Vector3 rotAxis = GetNode<Node3D>("COMPivot").Basis.X;
       RotateObjectLocal(rotAxis, this._pitchSpeed * (float)delta);
    }

    private Vector3 DoLift ()
    {
        float speed = Velocity.Length();
        float liftForce = speed * speed * getCL((float)this._aoa) / 2;
        Vector3 dragDir = Velocity.Normalized() * -1;
        Vector3 lift = liftForce * dragDir.Cross(Basis.Y).Cross(dragDir).Normalized();
        this.Lift = lift;
        return lift;
    }

    private Vector3 DoDrag ()
    {
        float speed = Velocity.Length();
        float dragForce = DragCoeff * speed * speed * (float)Mathf.Abs(this._aoa) / 2;
        Vector3 drag = dragForce * -1 * Velocity.Normalized();
        this.Drag = drag;
        return drag;
    }

    private float getCL (float aoa)
    {
        // Assume a linear function, top out at CL of 1.5 at 20 degrees.
        if (aoa > 20)
        {
            return 0;
        }
        // GD.Print(aoa * 0.075f);
        return aoa * 0.075f;
    }
}
