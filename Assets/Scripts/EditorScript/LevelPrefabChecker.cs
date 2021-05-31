using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using NaughtyAttributes;

public class LevelPrefabChecker : MonoBehaviour
{
    public LevelData[] LevelDatas;
    public GameObject[] LevelPrefabs;


    [Button("Check Prefabs")]
    private void checkPrefabs()
    { 
        List<GameObject> UsedLevelPrefabs=new List<GameObject>();
        foreach (LevelData data in LevelDatas)
        {
            UsedLevelPrefabs.Add(data.LevelObject);
        }
        
        string result = "Unused Level Prefabs: "+'\n';
        foreach (GameObject prefab in LevelPrefabs)
        {
            if (UsedLevelPrefabs.Contains(prefab) == false)
            {
                result += prefab.gameObject.name + "\n";
            }
            
        }

        writeToTextFile(result);
        Debug.Log(result);
    }

    [Button("Log Levels")]
    private void logLevels()
    {
        //Level data name
        //Level prefab name
        //Level type
        string result = "Levels Summary: "+'\n';
        foreach (LevelData data in LevelDatas)
        {
            result += "Data Name: " + data.name + " Prefab Name: " + data.LevelObject.name +"\n";
        }
        
        writeToTextFile(result);
        Debug.Log(result);
        
    }

    private void writeToTextFile(string content)
    {
        string path = "Assets/rebootLog.txt";

        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);
        writer.Write(content);
        writer.Close();
        
    }
}
