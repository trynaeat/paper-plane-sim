using System;
using Godot;
using Godot.NativeInterop;

public partial class Plane : RigidBody3D
{
    [Signal]
    public delegate void PhysicsUpdateEventHandler(float velocity, float altitude, float aoa);
    [Export]
    public float FallAcceleration { get; set; }

    [Export]
    public float LiftCoeff { get; set; }

    [Export]
    public Vector3 DragCoeff { get; set; }

    [Export]
    public float DragDistance {get; set; }

    [Export]
    public int StartSpeed { get; set; }

    [Export]
    public int CriticalAoA { get; set; }

    [Export]
    public float RollForce { get; set; }
    [Export]
    public float PitchForce { get; set; }
    [Export]
    public Vector3 Lift { get; set; }
    [Export]
    public Vector3 Drag { get; set; }
    [Export]
    public float Thrust { get; set; }
    [Export]
    public Vector3 FGravity { get; set; }
    [Export]
    public Vector3 ThrustVec { get; set; }
    private double _aoa = 0;
    private double _aoaSigned = 0;
    private bool _active = true;
    private CollisionShape3D _collider;
    public CollisionShape3D Collider { get => _collider; }

    private PlaneTracker _tracker;

    public PlaneTracker Tracker { get => _tracker; }

    public override void _Ready()
    {
        base._Ready();
        _tracker = GetNode<PlaneTracker>("PlaneTracker");
        _collider = GetNode<CollisionShape3D>("CollisionShape3D");
        _tracker.StartTracking();
        DebugOverlay.Draw.AddVector(this, RigidBody3D.PropertyName.LinearVelocity, 1f, new Color(0, 1, 0));
        DebugOverlay.Draw.AddVector(this, "Lift", 0.01f, new Color(0, 0, 1));
        DebugOverlay.Draw.AddVector(this, "Drag", 0.01f, new Color(1, 0, 0));
        DebugOverlay.Draw.AddVector(this, "FGravity", 0.01f, new Color(1, 0, 1));
        Vector3 dir = -1 * this.Basis.Z.Normalized();
        LinearVelocity = dir * StartSpeed;
    }

    public override void _PhysicsProcess(double delta)
    {
        float speed = LinearVelocity.Length();
        Vector3 forward = -1 * this.Basis.Z;
        Vector3 forwardGlobal = -1 * GlobalBasis.Z;
        this._aoaSigned = forward.SignedAngleTo(LinearVelocity, GlobalBasis.X * -1) * (180.0 / Math.PI);
        this._aoa = Mathf.Abs(this._aoaSigned);
        if (LinearVelocity.Length() < 0.1) {
            this._aoa = 90;
        }
        // GD.Print(this._aoa);
        DoPitch((float)_aoa);
        DoRoll();
        Vector3 lift = DoLift(speed);
        Vector3 drag = DoDrag();
        Vector3 gravity = FallAcceleration * Mass * Vector3.Down;
        Vector3 thrust = DoThrust();
        this.FGravity = gravity;
        ApplyCentralForce(gravity);
        ApplyForce(lift);
        ApplyForce(drag, -1 * forwardGlobal * DragDistance);
        ApplyForce(thrust);
        EmitSignal(SignalName.PhysicsUpdate, speed, GlobalPosition.Y, _aoaSigned);
    }

    private void DoRoll ()
    {
        // Process user inputs
        if (!this._active)
        {
            return;
        }
        Vector3 forward = -1 * GlobalTransform.Basis.Z;
        if (Input.IsActionPressed("bank_left"))
        {
            ApplyTorque(RollForce * forward * -1);
        }
        else if (Input.IsActionPressed("bank_right"))
        {
            ApplyTorque(RollForce * forward);
        }
    }

    private void DoPitch (float aoa)
    {
       if (aoa > CriticalAoA)
       {
        return;
       }
       float speed = LinearVelocity.Length();
       Vector3 right = GlobalTransform.Basis.X;
       if (!this._active)
       {
        return;
       }
       // Process user inputs
       if (Input.IsActionPressed("pitch_up"))
       {
        ApplyTorque(PitchForce * speed * right);
       }
       else if (Input.IsActionPressed("pitch_down"))
       {
        ApplyTorque(PitchForce * speed * right * -1);
       }
    }

    private Vector3 DoLift (float speed)
    {
        float liftForce = LiftCoeff * speed * speed * getCL((float)this._aoa) / 2;
        Vector3 lift = GlobalTransform.Basis.Y * liftForce;
        // Lift points down if negative aoa
        if (this._aoaSigned < 0)
        {
            lift = -1 * lift;
        }
        this.Lift = lift;
        return lift;
    }

    private Vector3 DoDrag ()
    {
        Vector3 airVelocityVector = ToLocal(GlobalTransform.Origin + LinearVelocity);
        Vector3 drag = new Vector3(
            -1 * Mathf.Sign(airVelocityVector.X) * DragCoeff.X * (airVelocityVector.X * airVelocityVector.X) / 2,
            -1 * Mathf.Sign(airVelocityVector.Y) * DragCoeff.Y * (airVelocityVector.Y * airVelocityVector.Y) / 2,
            -1 * Mathf.Sign(airVelocityVector.Z) * DragCoeff.Z * (airVelocityVector.Z * airVelocityVector.Z) / 2
        );
        drag = ToGlobal(drag) - GlobalTransform.Origin;
        this.Drag = drag;
        AngularDamp = 1 + DragCoeff.LengthSquared() * 0.8f;
        return drag;
    }

    private float getCL (float aoa)
    {
        // Assume a linear function, top out at CL of 1.5 at 20 degrees.
        if (aoa > CriticalAoA)
        {
            return 0;
        }
        // GD.Print(aoa * 0.075f);
        return aoa * 0.075f;
    }

    private Vector3 DoThrust ()
    {
        Vector3 forward = -1 * GlobalTransform.Basis.Z.Normalized();
        this.ThrustVec = Thrust * forward;
        return Thrust * forward;
    }

    public void Destroy ()
    {
        DebugOverlay.Draw.RemoveVectors(this);
        QueueFree();
    }

    public void OnArea3DPushed(float x, float y, float z)
    {
        ApplyForce(new Vector3(x, y, z));
    }

    public void Activate ()
    {
        this._active = true;
    }

    public void Deactivate ()
    {
        this._active = false;
        this._tracker.StopTracking();
    }
}
