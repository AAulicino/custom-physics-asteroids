using System;
using UnityEngine;

public interface IRigidBodyModel
{
    event Action OnOutOfBounds;

    Vector2 Position { get; set; }
    Vector2 Velocity { get; set; }
    Vector2 Acceleration { get; set; }

    bool WrapOnScreenEdge { get; }

    float MaxSpeed { get; }
    float AngularVelocity { get; set; }
    float Rotation { get; set; }
    float Drag { get; }

    void Step (float deltaTime);
    void SyncComponents ();
}
