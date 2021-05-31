using UnityEngine;
using System.Collections.Generic;

public enum StoreItemType{Texture, Material, Count}

//Generic class is used for creating shared variables through store items! 
[System.Serializable]
public class StoreItem
{
    public string itemKey;
    public Object Item;
    public float price;
    public Sprite storeIcon;
    public Sprite showCaseIcon;
    public bool purchased;
    
}

public enum StorePageType{Random, Acquisition}

[System.Serializable]
public class StorePage
{
    public StorePageType pageType;
    public StoreItem[] pageItems;
    
}

//Class that is used to grouping store elements within their types, for readiblity
[System.Serializable]
public class StoreItemLibrary
{
    public StoreItemType itemType;
    public StorePage[] libraryPages;
    public Sprite libraryIcon;
    
}


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/StoreData", order = 1)]
public class StoreData : ScriptableObject
{
    public StoreItemLibrary[] Libraries;
    
}