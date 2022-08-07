using UnityEngine;

public class AsteroidView : EntityView
{
    public override void Initialize (IEntityModel physicsEntity)
    {
        base.Initialize(physicsEntity);

        physicsEntity.RigidBody.Velocity = new Vector2(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        ).normalized;

        physicsEntity.RigidBody.AngularVelocity = Random.Range(-200f, 200f);
    }
}
