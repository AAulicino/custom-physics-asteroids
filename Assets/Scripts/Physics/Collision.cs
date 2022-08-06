public class Collision
{
    public readonly IPhysicsEntity Self;
    public readonly ICollider Other;

    public Collision (IPhysicsEntity a, ICollider b)
    {
        Self = a;
        Other = b;
    }
}
