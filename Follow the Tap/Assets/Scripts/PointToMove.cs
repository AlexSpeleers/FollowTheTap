using UnityEngine;

public class PointToMove : ICommand
{
    public Vector3 Destination { get; set; }
    public PointToMove(Vector3 point)
    {
        Destination = point;
    }
    public void Execute()
    {
        ImageMover.instance.Move(Destination);        
    }
}
