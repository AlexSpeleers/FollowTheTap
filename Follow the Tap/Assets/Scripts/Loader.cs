using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public static class Loader
{
    private static string commandPath;
    private static string positionPath;
    public static Queue<PointToMove> LoadCommands()
    {        
        commandPath = Path.Combine(Application.dataPath, "queue.json");
        if (File.Exists(commandPath))
        {
            using (StreamReader sr = new StreamReader(commandPath))
            {
                var line = sr.ReadToEnd();
                return JsonConvert.DeserializeObject<Queue<PointToMove>>(line);
            }
        }
        else return null;
    }

    public static void SaveCommands(Queue<ICommand> dataToSave)
    {
        File.WriteAllText(commandPath, JsonConvert.SerializeObject(dataToSave, Formatting.Indented));
    }

    public static Vector3 LoadPos()
    {
        positionPath = Path.Combine(Application.dataPath, "position.json");
        if (File.Exists(positionPath))
        {
            using (StreamReader sr = new StreamReader(commandPath))
            {
                var line = sr.ReadToEnd();
                if (string.IsNullOrEmpty(line))
                {
                    return ImageMover.instance.transform.position;
                }
                return JsonConvert.DeserializeObject<Vector3>(line);
            }
        }
        else return ImageMover.instance.transform.position;
    }
    /// <summary>
    /// Save position passed in the parameter.
    /// </summary>
    /// <param name="dataToSave"></param>
    public static void SavePos(Vector3 dataToSave)
    {
        File.WriteAllText(commandPath, JsonConvert.SerializeObject(dataToSave, Formatting.Indented));
    }
}
