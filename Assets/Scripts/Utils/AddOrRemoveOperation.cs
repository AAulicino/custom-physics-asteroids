public readonly struct AddOrRemoveOperation<T>
{
    public readonly bool IsAdd;
    public readonly T Value;

    public AddOrRemoveOperation (bool isAdd, T value)
    {
        IsAdd = isAdd;
        Value = value;
    }
}
