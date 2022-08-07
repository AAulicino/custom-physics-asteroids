using UnityEngine;

public class PlayerView : EntityView
{
    new IPlayerModel model;

    Vector2 moveInputs;
    bool firing;

    string horizontalInput;
    string verticalInput;
    string fireInput;

    public override void Initialize (IEntityModel model)
    {
        base.Initialize(model);
        this.model = (IPlayerModel)model;

        horizontalInput = "Horizontal" + this.model.PlayerId;
        verticalInput = "Vertical" + this.model.PlayerId;
        fireInput = "Fire" + this.model.PlayerId;
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
        moveInputs = new Vector2(Input.GetAxis(horizontalInput), Input.GetAxis(verticalInput));
        firing = Input.GetButton(fireInput);
    }

    void WriteInputs ()
    {
        model.RigidBody.Acceleration = moveInputs;

        if (firing)
            model.FireProjectile();
    }
}
