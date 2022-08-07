using UnityEngine;

public class PlayerView : EntityView
{
    public override void Sync ()
    {
        base.Sync();

        Vector2 moveVector = new Vector2(Input.GetAxis("Horizontal1"), Input.GetAxis("Vertical1"));

        float AngleRad = Mathf.Atan2(
            moveVector.y,
            moveVector.x
        );

        float angle = AngleRad * Mathf.Rad2Deg;

        physicsEntity.RigidBody.Position += moveVector * Time.deltaTime;

        if (moveVector.sqrMagnitude > 0)
            physicsEntity.RigidBody.Rotation = angle - 90;
    }
}
