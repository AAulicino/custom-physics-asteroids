public class Collision
{
    public readonly IEntityModel Self;
    public readonly IColliderModel Other;

    public Collision (IEntityModel a, IColliderModel b)
    {
        Self = a;
        Other = b;
    }
}
