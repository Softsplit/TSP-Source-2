using System;
using System.Collections.Generic;
using Sandbox;

public sealed class RotatingDoor : HammerEntity, Component.ICollisionListener
{
    public bool PlayerTouchingDoor { get; set; }

    public bool IsMoving
    {
        get
        {
            return currentAngle != targetAngle;
        }
    }

    protected override void OnAwake()
    {
        hasDoorBlocker = doorBlocker != null;

        if (brushAxisFixup)
        {
            dirChange *= -1f;

            if (reverseDirection)
            {
                dirChange *= -1f;
            }
        }

        _renderer = Components.Get<ModelRenderer>();
        _collider = Components.Get<BoxCollider>();

        if (renderColor != Color.White)
        {
            _renderer.Tint = renderColor;
        }

        if (!isDrawing)
        {
            _renderer.Enabled = false;
            _collider.Enabled = false;
        }

        closedRotation = Transform.Rotation;
        openRotation = Rotation.FromAxis(axis, maxAngle * dirChange) * Transform.Rotation;
        currentAngle = spawnAngle * dirChange;
        targetAngle = currentAngle;
        Transform.Rotation = Rotation.FromAxis(axis, currentAngle) * Transform.Rotation;
        source = Components.Get<SoundPointComponent>();
    }

    protected override void OnValidate()
    {
        Rigidbody component = Components.Get<Rigidbody>();

        if (component != null)
        {
            /*
            if (component.interpolation != RigidbodyInterpolation.Interpolate)
            {
                Log.Warning($"Please set door rigidbody interpolation to Interpolate {this}");
            }

            if (component.collisionDetectionMode != CollisionDetectionMode.Continuous && component.collisionDetectionMode != CollisionDetectionMode.ContinuousSpeculative)
            {
                Log.Warning($"Please set door rigidbody collision mode to Continuous Speculative {this}");
            }
            */
        }

        if (doorBlocker != null && doorBlocker.GameObject.Parent.Transform != Transform)
        {
            doorBlocker = null;
        }
    }

    public void SetTargetAngleTo0()
    {
        targetAngle = 0f;
    }

    public void SetTargetAngleTo90()
    {
        targetAngle = 90f;
    }

    protected override void OnUpdate()
    {
        if (!isDrawing)
        {
            return;
        }

        if (currentAngle == targetAngle)
        {
            return;
        }

        if (touchingPlayer)
        {
            return;
        }

        if (PlayerTouchingDoor)
        {
            return;
        }

        if (directions == DoorDirections.BackwardOnly && targetAngle > 0f)
        {
            targetAngle = 0f;
        }

        if (directions == DoorDirections.ForwardOnly && targetAngle < 0f)
        {
            targetAngle = 0f;
        }

        currentAngle = MathXHelper.MoveTowards(currentAngle, targetAngle, speed * Time.Delta /* Singleton<GameMaster>.Instance.GameDeltaTime */);
        Transform.Rotation = Rotation.FromAxis(axis, currentAngle) * closedRotation;

        if (currentAngle == targetAngle)
        {
            if (MathF.Abs(currentAngle) == maxAngle)
            {
                FireOutput(Outputs.OnFullyOpen);
                PlaySound(Sounds.Open);
                return;
            }

            if (currentAngle == 0f)
            {
                FireOutput(Outputs.OnFullyClosed);
                PlaySound(Sounds.Close);
            }
        }
    }

    private void UpdateDoorBlockerColliderDymanicPosition()
    {
        float x = Components.Get<BoxCollider>().Scale.x;
        float num = closedRotation.Distance(Transform.Rotation);

        if (reverseDoorBlockerDirection)
        {
            num *= -1f;
        }

        doorBlocker.Transform.LocalPosition = new Vector3(-x, 0f, 0f);
        doorBlocker.Transform.LocalRotation = new Vector3(0f, 0f, (180f - num) / 2f).EulerAngles;
        float num2 = 2f * x * MathF.Sin(0.017453292f * num / 2f);
        Vector3 size = doorBlocker.Scale;
        Vector3 center = doorBlocker.Center;
        size.x = num2;
        center.x = num2 / 2f;
        doorBlocker.Scale = size;
        doorBlocker.Center = center;
    }

    private void DoorBlockerUpdate()
    {
        if (!IsMoving)
        {
            if (doorBlocker.Enabled)
            {
                doorBlocker.Enabled = false;
            }

            return;
        }

        if (!doorBlocker.Enabled)
        {
            doorBlocker.Enabled = true;
        }

        if (doorBlockerMode == DoorBlockerMode.DynamicAndEnabledOnMoving)
        {
            UpdateDoorBlockerColliderDymanicPosition();
        }
    }

    private void PlaySound(Sounds sound)
    {
        if (source == null)
        {
            return;
        }

        switch (sound)
        {
            case Sounds.Open:
                if (openClips.Count > 0)
                {
                    source.Pitch = openPitchRange.Random();
                    source.Volume = openVol;
                    source.SoundEvent = openClips[Game.Random.Next(0, openClips.Count)];
                    source.StartSound();
                    return;
                }

                break;

            case Sounds.Close:
                if (closeClips.Count > 0)
                {
                    source.Pitch = closePitchRange.Random();
                    source.Volume = closeVol;
                    source.SoundEvent = closeClips[Game.Random.Next(0, closeClips.Count)];
                    source.StartSound();
                    return;
                }

                break;

            case Sounds.Move:
                if (moveClips.Count > 0)
                {
                    source.Pitch = movePitchRange.Random();
                    source.Volume = moveVol;
                    source.SoundEvent = moveClips[Game.Random.Next(0, moveClips.Count)];
                    source.StartSound();
                    return;
                }

                break;

            case Sounds.Locked:
                if (lockedClips.Count > 0)
                {
                    source.Pitch = lockedPitchRange.Random();
                    source.Volume = lockedVol;
                    source.SoundEvent = lockedClips[Game.Random.Next(0, lockedClips.Count)];
                    source.StartSound();
                }

                break;

            default:
                return;
        }
    }

    protected override void OnFixedUpdate()
    {
        if (hasDoorBlocker)
        {
            DoorBlockerUpdate();
        }
    }

    public void OnCollisionStart(Collision other)
    {
        if (other.Other.Collider.GameObject.Tags.Has("player"))
        {
            touchingPlayer = true;
        }
    }

    public void OnCollisionStop(CollisionStop other)
    {
        touchingPlayer = false;
    }

    public void OnCollisionUpdate(Collision other)
    {
        if (other.Other.Collider.GameObject.Tags.Has("player"))
        {
            touchingPlayer = true;
        }
    }

    public void Input_EnableDraw()
    {
        _renderer.Enabled = true;
        _collider.Enabled = true;
    }

    public void Input_DisableDraw()
    {
        _renderer.Enabled = false;
        _collider.Enabled = false;
    }

    public override void Use()
    {
        if (usable)
        {
            if (isLocked)
            {
                FireOutput(Outputs.OnLockedUse);
                PlaySound(Sounds.Locked);
            }

            Input_Toggle();
        }
    }

    public void Input_Toggle()
    {
        if (isLocked)
        {
            return;
        }

        if (targetAngle != currentAngle)
        {
            return;
        }

        if (MathF.Abs(targetAngle) == maxAngle)
        {
            Input_Close();
            return;
        }

        if (targetAngle == 0f)
        {
            Input_Open();
        }
    }

    public void Input_Open()
    {
        if (isLocked)
        {
            return;
        }

        FireOutput(Outputs.OnOpen);

        if (directions == DoorDirections.BackwardOnly)
        {
            targetAngle = -maxAngle;
        }
        else
        {
            targetAngle = maxAngle;
        }

        PlaySound(Sounds.Move);
        targetAngle *= dirChange;
    }

    public void Input_Close()
    {
        targetAngle = 0f;
        FireOutput(Outputs.OnClose);

        if (MathF.Abs(currentAngle) == 90f)
        {
            PlaySound(Sounds.Move);
        }
    }

    public void Input_Lock()
    {
        isLocked = true;
    }

    public void Input_Unlock()
    {
        isLocked = false;
    }

    private ModelRenderer _renderer;

    private BoxCollider _collider;

    [Property]
    public bool isDrawing { get; set; } = true;

    [Property]
    public float spawnAngle { get; set; }

    private float currentAngle;

    private float targetAngle;

    [Property]
    public float maxAngle { get; set; } = 90f;

    [Property]
    public bool reverseDirection { get; set; }

    [Property]
    public bool brushAxisFixup { get; set; }

    private float dirChange = 1f;

    [Property]
    public Vector3 axis { get; set; } = Vector3.Up;

    [Property]
    public float speed { get; set; } = 120f;

    [Property]
    public bool isLocked { get; set; }

    [Property]
    public DoorDirections directions { get; set; }

    [Property]
    public Color renderColor { get; set; } = Color.White;

    [Property]
    public bool usable { get; set; }

    private bool touchingPlayer;

    private Rotation closedRotation;

    private Rotation openRotation;

    [Property]
    public List<SoundEvent> openClips { get; set; }

    [Property]
    public float openVol { get; set; } = 1f;

    [Property]
    public MinMax openPitchRange { get; set; }

    [Property]
    public List<SoundEvent> closeClips { get; set; }

    [Property]
    public float closeVol { get; set; } = 1f;

    [Property]
    public MinMax closePitchRange { get; set; }

    [Property]
    public List<SoundEvent> moveClips { get; set; }

    [Property]
    public float moveVol { get; set; } = 1f;

    [Property]
    public MinMax movePitchRange { get; set; }

    [Property]
    public List<SoundEvent> lockedClips { get; set; }

    [Property]
    public float lockedVol { get; set; } = 1f;

    [Property]
    public MinMax lockedPitchRange { get; set; }

    private SoundPointComponent source;

    [Property]
    [Group("Null for no blocker, must be a direct child of this class")]
    public BoxCollider doorBlocker { get; set; }

    private bool hasDoorBlocker;

    [Property]
    [Group("Null for no blocker, must be a direct child of this class")]
    public DoorBlockerMode doorBlockerMode { get; set; }

    [Property]
    [Group("Null for no blocker, must be a direct child of this class")]
    [InspectorButton("MIGRATION_PerformMeshColliderSwapWithBoxCollider", null)]
    public bool reverseDoorBlockerDirection { get; set; }

    public enum DoorDirections
    {
        Both,
        ForwardOnly,
        BackwardOnly
    }

    public enum DoorBlockerMode
    {
        StaticAndEnabledOnMoving,
        DynamicAndEnabledOnMoving
    }

    private enum Sounds
    {
        Open,
        Close,
        Move,
        Locked
    }
}