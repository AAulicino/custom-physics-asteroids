using UnityEngine;

public interface ISquareColliderModel : IColliderModel
{
    float OuterRadius { get; set; }
    float InnerRadius { get; set; }
}
