using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSV_Importer
{
    public static List<Dictionary<string, object>> Spawn_Design = new List<Dictionary<string, object>>(CSVReader.Read("Spawner"));
    public static List<Dictionary<string, object>> Summon_Design = new List<Dictionary<string, object>>(CSVReader.Read("Summon"));
}
