using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using NaughtyAttributes;
using Debug = UnityEngine.Debug;

public class LevelDuplicateCleaner : MonoBehaviour
{

    [Button("Clear Duplicates")]
    public void clearDuplicates()
    {
        List<Transform> children = GetComponentsInChildren<Transform>().ToList();
        children.Remove(transform);
        
        List<string> occupiedPositions = new List<string>();

        int clearCounter = 0;
        
        foreach (Transform t in children)
        {
            string hash = t.position.ToString("F7");
            
            if (occupiedPositions.Contains(hash))
            {
                Debug.Log("Transform: "+t.gameObject.name+" is removed!");
                clearCounter++;
                DestroyImmediate(t.gameObject);
            }

            else
            {
                occupiedPositions.Add(hash);
            }
        }
        
        Debug.Log("Number of total removed stones "+clearCounter.ToString());
    }
}
