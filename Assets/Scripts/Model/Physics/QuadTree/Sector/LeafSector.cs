using System.Collections.Generic;

public class LeafSector<T> : Sector<T>
{
    readonly HashSet<T> objects = new HashSet<T>();

    public LeafSector (
        Sector<T> parent,
        QuadType quadType,
        int level,
        QuadTreeRect rect,
        IQuadTreeObjectBounds<T> objectBounds,
        int maxObjects,
        int maxLevel
    ) : base(parent, quadType, level, rect, objectBounds, maxObjects, maxLevel)
    {
    }

    public override void Clear () => objects.Clear();

    public override bool TryInsert (T obj)
    {
        if (objects.Count >= MaxObjects && Level < MaxLevel)
            return false;
        objects.Add(obj);
        return true;
    }

    public override Sector<T> Quarter ()
    {
        NodeSector<T> node = new(parent, quadType, Level, Rect, ObjectBounds, MaxObjects, MaxLevel);
        foreach (T o in objects)
            node.TryInsert(o);
        return node;
    }

    public override IEnumerable<T> GetNearestObjects (T obj) => objects;

    public override IEnumerable<QuadTreeRect> GetRects ()
    {
        yield return Rect;
    }
}
