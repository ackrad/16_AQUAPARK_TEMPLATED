using UnityEngine;
using System.Collections.Generic;



[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/LevelData", order = 1)]
public class LevelData : ScriptableObject
{
    public string LevelName;
    
    //------- Example level related fields. Can be removed during mechanic implementation! --------
    
    public Material LevelMaterial;
    public GameObject LevelObject;
    public float Price = 100;

    //------------------------------------------------------

}