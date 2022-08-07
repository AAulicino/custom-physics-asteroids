using UnityEngine;

public class AsteroidView : EntityView
{
    public override void Initialize (IPhysicsEntity physicsEntity)
    {
        base.Initialize(physicsEntity);
        physicsEntity.RigidBody.Velocity = new Vector2(Random.value, Random.value).normalized;
        physicsEntity.RigidBody.AngularVelocity = Random.Range(-200f, 200f);
    }
}
