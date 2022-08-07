using System.Collections.Generic;
using System.Linq;

public class NodeSector<T> : Sector<T>
{
    readonly Sector<T>[] sectors;

    public NodeSector (
        Sector<T> parent,
        QuadType quadType,
        int level,
        QuadTreeRect rect,
        IQuadTreeObjectBounds<T> objectBounds,
        int maxObjects,
        int maxLevel
    ) : base(parent, quadType, level, rect, objectBounds, maxObjects, maxLevel)
    {
        int nextLevel = Level + 1;

        sectors = new Sector<T>[4];

        sectors[TOP_RIGHT] = new LeafSector<T>(
            this,
            QuadType.TopRight,
            nextLevel,
            Rect.TopRightQuarter,
            ObjectBounds,
            MaxObjects,
            MaxLevel
        );
        sectors[BOTTOM_RIGHT] = new LeafSector<T>(
            this,
            QuadType.BottomRight,
            nextLevel,
            Rect.BottomRightQuarter,
            ObjectBounds,
            MaxObjects,
            MaxLevel
        );
        sectors[BOTTOM_LEFT] = new LeafSector<T>(
            this,
            QuadType.BottomLeft,
            nextLevel,
            Rect.BottomLeftQuarter,
            ObjectBounds,
            MaxObjects,
            MaxLevel
        );
        sectors[TOP_LEFT] = new LeafSector<T>(
            this,
            QuadType.TopLeft,
            nextLevel,
            Rect.TopLeftQuarter,
            ObjectBounds,
            MaxObjects,
            MaxLevel
        );
    }

    public override bool TryInsert (T obj)
    {
        bool result = false;

        if (IsLeft(obj))
        {
            if (IsTop(obj))
                result |= Insert(ref sectors[TOP_LEFT], obj);
            if (IsBottom(obj))
                result |= Insert(ref sectors[BOTTOM_LEFT], obj);
        }

        if (IsRight(obj))
        {
            if (IsTop(obj))
                result |= Insert(ref sectors[TOP_RIGHT], obj);
            if (IsBottom(obj))
                result |= Insert(ref sectors[BOTTOM_RIGHT], obj);
        }

        return result;
    }

    static bool Insert (ref Sector<T> sector, T obj)
    {
        if (sector.TryInsert(obj))
            return true;
        sector = sector.Quarter();
        return sector.TryInsert(obj);
    }

    public override Sector<T> Quarter () => this;

    public override void Clear ()
    {
        sectors[TOP_LEFT].Clear();
        sectors[TOP_LEFT] = null;

        sectors[BOTTOM_LEFT].Clear();
        sectors[BOTTOM_LEFT] = null;

        sectors[TOP_RIGHT].Clear();
        sectors[TOP_RIGHT] = null;

        sectors[BOTTOM_RIGHT].Clear();
        sectors[BOTTOM_RIGHT] = null;
    }

    public override IEnumerable<T> GetNearestObjects (T obj)
        => GetSectors(obj).SelectMany(s => s.GetNearestObjects(obj));

    public override IEnumerable<QuadTreeRect> GetRects ()
    {
        yield return Rect;

        foreach (QuadTreeRect rect in sectors[TOP_LEFT].GetRects())
            yield return rect;

        foreach (QuadTreeRect rect in sectors[BOTTOM_LEFT].GetRects())
            yield return rect;

        foreach (QuadTreeRect rect in sectors[TOP_RIGHT].GetRects())
            yield return rect;

        foreach (QuadTreeRect rect in sectors[BOTTOM_RIGHT].GetRects())
            yield return rect;
    }

    IEnumerable<Sector<T>> GetSectors (T obj)
    {
        if (IsLeft(obj))
        {
            if (IsTop(obj))
                yield return sectors[TOP_LEFT];
            if (IsBottom(obj))
                yield return sectors[BOTTOM_LEFT];
        }

        if (IsRight(obj))
        {
            if (IsTop(obj))
                yield return sectors[TOP_RIGHT];
            if (IsBottom(obj))
                yield return sectors[BOTTOM_RIGHT];
        }
    }

    bool IsTop (T obj) => IsTop(ObjectBounds.GetTop(obj));
    bool IsBottom (T obj) => IsBottom(ObjectBounds.GetBottom(obj));
    bool IsLeft (T obj) => IsLeft(ObjectBounds.GetLeft(obj));
    bool IsRight (T obj) => IsRight(ObjectBounds.GetRight(obj));

    bool IsTop (float y) => y >= Rect.MidY;
    bool IsBottom (float y) => y <= Rect.MidY;
    bool IsLeft (float x) => x <= Rect.MidX;
    bool IsRight (float x) => x >= Rect.MidX;
}
