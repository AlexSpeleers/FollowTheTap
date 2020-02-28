using System.Collections.Generic;
using UnityEngine;
using UniRx;
public class CommandInvoker : MonoBehaviour
{
    public static Queue<ICommand> commandBuffer;
    private void Awake()
    {
        commandBuffer = new Queue<ICommand>();
        var items = Loader.LoadCommands();
        if (items != null)
            PopulateCommands(items);
        var ExecuteStream = Observable.EveryUpdate().
            Where(_ => commandBuffer.Count > 0).
            ObserveOnMainThread().
            Subscribe(_=>ExecuteCommands());
    }
    private void PopulateCommands(Queue<PointToMove> commands)
    {
        foreach (var command in commands)
        {            
            commandBuffer.Enqueue(new PointToMove(command.Destination));
        }
    }
    public static void AddCommand(ICommand command)
    {
        commandBuffer.Enqueue(command);
    }

    public static void ClearCommand()
    {
        commandBuffer.Clear();
        Loader.SaveCommands(commandBuffer);
    }

    private void ExecuteCommands()
    {
        if (Vector3.Distance(commandBuffer.Peek().Destination, ImageMover.instance.transform.position) >= 0.1f)        
            commandBuffer.Peek().Execute();        
        else
            commandBuffer.Dequeue();        
    }
}