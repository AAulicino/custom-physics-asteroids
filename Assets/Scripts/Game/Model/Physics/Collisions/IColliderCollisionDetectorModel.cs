public interface IColliderCollisionDetectorModel
{
    bool LayerCollidesWith (CollisionLayer a, CollisionLayer b);

    bool IsColliding (ICircleColliderModel a, ICircleColliderModel b);
    bool IsColliding (ISquareColliderModel a, ISquareColliderModel b);
    bool IsColliding (ISquareColliderModel square, ICircleColliderModel circle);
}
