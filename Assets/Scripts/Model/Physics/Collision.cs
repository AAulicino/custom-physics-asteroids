public class Collision
{
    public readonly IEntityModel Self;
    public readonly ICollider Other;

    public Collision (IEntityModel a, ICollider b)
    {
        Self = a;
        Other = b;
    }
}
