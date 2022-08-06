using UnityEngine;

public class PlayerView : EntityView
{
    public override void Sync ()
    {
        base.Sync();
        float h = Input.GetAxis("Horizontal1");
        float v = Input.GetAxis("Vertical1");
        var moveVector = new Vector2(h, v);

        float AngleRad = Mathf.Atan2(
            moveVector.y,
            moveVector.x
        );

        float angle = AngleRad * Mathf.Rad2Deg;

        physicsEntity.RigidBody.Position += moveVector * Time.deltaTime;
        physicsEntity.RigidBody.Rotation = angle - 90;
    }
}
