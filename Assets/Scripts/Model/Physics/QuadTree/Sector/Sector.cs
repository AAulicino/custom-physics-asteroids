using System.Collections.Generic;
using UnityEngine;

public abstract class Sector<T>
{
    protected const int TOP_RIGHT = 0;
    protected const int BOTTOM_RIGHT = 1;
    protected const int BOTTOM_LEFT = 2;
    protected const int TOP_LEFT = 3;

    public readonly QuadTreeRect Rect;
    public readonly QuadType quadType;

    protected readonly Sector<T> parent;
    protected readonly int MaxObjects;
    protected readonly int MaxLevel;
    protected readonly int Level;

    protected readonly IQuadTreeObjectBounds<T> ObjectBounds;

    protected Sector (
        Sector<T> parent,
        QuadType quadType,
        int level,
        QuadTreeRect rect,
        IQuadTreeObjectBounds<T> objectBounds,
        int maxObjects,
        int maxLevel
    )
    {
        this.parent = parent;
        this.quadType = quadType;
        Level = level;
        Rect = rect;
        ObjectBounds = objectBounds;
        MaxObjects = maxObjects;
        MaxLevel = maxLevel;
    }

    public bool Contains (Vector2 position) => Rect.Contains(position);

    public abstract void Clear ();
    public abstract bool TryInsert (T obj);
    public abstract Sector<T> Quarter ();

    public abstract IEnumerable<T> GetNearestObjects (T obj);
    public abstract IEnumerable<QuadTreeRect> GetRects ();
}
