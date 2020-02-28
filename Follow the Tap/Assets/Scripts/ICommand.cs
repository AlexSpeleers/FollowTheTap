using UnityEngine;
public interface ICommand
{
    Vector3 Destination { get; }
    void Execute();
}
