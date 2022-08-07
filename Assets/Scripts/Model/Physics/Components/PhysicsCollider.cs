using UnityEngine;

public class PhysicsCollider : ICollider
{
    public Rect Bounds { get; private set; }
    public CollisionLayer Layer { get; set; }

    public PhysicsCollider (CollisionLayer layer)
    {
        Layer = layer;
    }

    public void SetPosition (Vector2 position)
    {
        Rect bounds = Bounds;
        bounds.center = position;
        Bounds = bounds;
    }

    public bool CollidesWith (ICollider other)
    {
        return LayerCollidesWith(other.Layer) && Bounds.Overlaps(other.Bounds);
    }

    bool LayerCollidesWith (CollisionLayer other)
    {
        switch (Layer)
        {
            case CollisionLayer.Player:
                switch (other)
                {
                    case CollisionLayer.Asteroid:
                        return true;
                    case CollisionLayer.Player:
                    case CollisionLayer.Projectile:
                        return false;
                }
                break;

            case CollisionLayer.Asteroid:
                switch (other)
                {
                    case CollisionLayer.Player:
                    case CollisionLayer.Projectile:
                        return true;
                    case CollisionLayer.Asteroid:
                        return false;
                }
                break;

            case CollisionLayer.Projectile:
                switch (other)
                {
                    case CollisionLayer.Player:
                    case CollisionLayer.Projectile:
                        return false;
                    case CollisionLayer.Asteroid:
                        return true;
                }
                break;
        }

        return false;
    }

    public void SetSize (Vector2 size)
    {
        Rect bounds = Bounds;
        bounds.size = size;
        Bounds = bounds;
        SetPosition(Bounds.center);
    }
}
