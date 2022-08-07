using UnityEngine;

public class PlayerView : EntityView
{
    [SerializeField] float movementSpeed;

    new IPlayerModel model;

    Vector2 moveVector;
    bool fire;

    public override void Initialize (IEntityModel model)
    {
        base.Initialize(model);
        this.model = (IPlayerModel)model;
    }

    public override void OnPrePhysicsStep ()
    {
        base.OnPrePhysicsStep();
        WriteInputs();
    }

    public override void OnViewUpdate ()
    {
        base.OnViewUpdate();
        ReadInputs();
    }

    void ReadInputs ()
    {
        moveVector = new Vector2(Input.GetAxis("Horizontal1"), Input.GetAxis("Vertical1"));
        fire = Input.GetButton("Fire1");
    }

    void WriteInputs ()
    {
        WriteVelocity();
        WriteRotation();
        WriteProjectile();
    }

    void WriteVelocity ()
    {
        model.RigidBody.Velocity = moveVector.normalized * movementSpeed;
    }

    void WriteRotation ()
    {
        float AngleRad = Mathf.Atan2(
            moveVector.y,
            moveVector.x
        );

        float angle = AngleRad * Mathf.Rad2Deg;

        if (moveVector.sqrMagnitude > 0)
            model.RigidBody.Rotation = angle - 90;
    }

    void WriteProjectile ()
    {
        if (fire)
            model.FireProjectile();
    }
}
