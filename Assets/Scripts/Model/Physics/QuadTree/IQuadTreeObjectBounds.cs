public interface IQuadTreeObjectBounds<in T>
{
    float GetLeft (T obj);
    float GetRight (T obj);
    float GetTop (T obj);
    float GetBottom (T obj);
}
