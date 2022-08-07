public class EntityBounds : IQuadTreeObjectBounds<IEntityModel>
{
    public float GetLeft (IEntityModel entity) => entity.Collider.Bounds.xMin;
    public float GetRight (IEntityModel entity) => entity.Collider.Bounds.xMax;
    public float GetTop (IEntityModel entity) => entity.Collider.Bounds.yMax;
    public float GetBottom (IEntityModel entity) => entity.Collider.Bounds.yMin;
}
