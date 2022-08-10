// https://stackoverflow.com/a/48375726 possible optimizations for QuadTree

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuadTree<T> : IQuadTree<T>
{
    public int MaxObjects { get; }
    public int MaxDepthLevel { get; }
    public int ObjectCount { get; private set; }
    public QuadTreeRect MainRect { get; private set; }

    readonly IQuadTreeObjectBounds<T> objectBounds;
    Sector<T> rootSector;

    public QuadTree (
        Vector2 min,
        Vector2 max,
        IQuadTreeObjectBounds<T> bounds,
        int maxObjects = 5,
        int maxDepthLevel = 5
    )
    {
        MaxObjects = maxObjects;
        MaxDepthLevel = maxDepthLevel;
        MainRect = new QuadTreeRect(min.x, min.y, max.x - min.x, max.y - min.y);
        this.objectBounds = bounds ?? throw new ArgumentNullException(nameof(bounds));
        rootSector = new LeafSector<T>(
            null,
            QuadType.None,
            0,
            MainRect,
            bounds,
            maxObjects,
            maxDepthLevel
        );
    }

    public bool Insert (T obj)
    {
        if (obj == null)
            throw new ArgumentNullException(nameof(obj));

        if (!IsObjectInMainRect(obj))
            return false;

        if (rootSector.TryInsert(obj))
        {
            ObjectCount++;
            return true;
        }

        rootSector = rootSector.Quarter();
        rootSector.TryInsert(obj);

        ObjectCount++;

        return true;
    }

    public void InsertRange (IEnumerable<T> objects)
    {
        if (objects == null)
            throw new ArgumentNullException(nameof(objects));

        foreach (T obj in objects)
            Insert(obj);
    }

    public void ClearAndUpdateMainRect (Rect mainRect)
    {
        MainRect = new QuadTreeRect(mainRect.xMin, mainRect.yMin, mainRect.width, mainRect.height);
        Clear();
    }

    public void Clear ()
    {
        rootSector.Clear();
        rootSector = new LeafSector<T>(
            null,
            QuadType.None,
            0,
            MainRect,
            objectBounds,
            MaxObjects,
            MaxDepthLevel
        );
        ObjectCount = 0;
    }

    public void GetNearestObjects (T obj, HashSet<T> objects)
    {
        if (obj == null)
            throw new ArgumentNullException(nameof(obj));

        objects.Clear();
        rootSector.GetNearestObjects(obj, objects);
    }

    public IEnumerable<QuadTreeRect> GetGrid () => rootSector.GetRects();

    bool IsObjectInMainRect (T obj)
    {
        if (objectBounds.GetTop(obj) < MainRect.Bottom)
            return false;
        if (objectBounds.GetBottom(obj) > MainRect.Top)
            return false;
        if (objectBounds.GetLeft(obj) > MainRect.Right)
            return false;
        if (objectBounds.GetRight(obj) < MainRect.Left)
            return false;

        return true;
    }
}
