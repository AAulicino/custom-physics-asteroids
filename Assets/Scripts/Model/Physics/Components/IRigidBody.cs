using UnityEngine;

public interface IRigidBody
{
    Vector2 Position { get; set; }
    Vector2 Velocity { get; set; }
    float AngularVelocity { get; set; }
    float Rotation { get; set; }

    void Step (float deltaTime);
    void SyncComponents ();
}
