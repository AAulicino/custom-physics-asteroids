using System.Collections.Generic;
using UnityEngine;

public interface IQuadTree<T>
{
    int MaxObjects { get; }
    int MaxDepthLevel { get; }
    int ObjectCount { get; }
    QuadTreeRect MainRect { get; }

    IEnumerable<QuadTreeRect> GetGrid ();
    IEnumerable<T> GetNearestObjects (T obj);
    bool Insert (T obj);
    void InsertRange (IEnumerable<T> objects);
    void Clear ();
    void ClearAndUpdateMainRect (Rect mainRect);
}
