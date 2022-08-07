public interface IPlayerModel : IEntityModel
{
    int PlayerId { get; }

    void FireProjectile ();
}
