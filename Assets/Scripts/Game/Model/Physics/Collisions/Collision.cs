public readonly struct Collision
{
    public readonly IEntityModel Self;
    public readonly IColliderModel Other;

    public Collision (IEntityModel self, IColliderModel other)
    {
        Self = self;
        Other = other;
    }
}
