using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using  NaughtyAttributes;
using  UnityEngine.Events;

/*[System.Serializable]*/ public class UnityEventStoreItem:UnityEvent<StoreItemType, StoreItem>{}

public class StoreController : Singleton<StoreController>
{
    public StoreData storeData;
    
    //Hash map points item keys to their elements.
    private Dictionary<string, StoreItem> itemsMap;

    public UnityEventStoreItem onItemSelected;
    
    new void Awake()
    {
        base.Awake();
        onItemSelected=new UnityEventStoreItem();
        
        //Copy store data to prevent writing original store data
        storeData= Instantiate(storeData);
        itemsMap=new Dictionary<string, StoreItem>();
        readPlayerData();
        
    }
    

    private void readPlayerData()
    {

        //Iterate over each librry item and set it weather or not they are purchased, with using player preffs
        foreach (StoreItemLibrary library in storeData.Libraries)
        {
            StorePage[] pages = library.libraryPages;

            StoreItemType libraryType = library.itemType;
            
            foreach (StorePage storePage in pages)
            {
                StoreItem[] items = storePage.pageItems;

                
                    
                foreach (StoreItem item in items)
                {
                    string itemKey = item.itemKey;
                    int isBought=PlayerPrefs.GetInt(itemKey, 0);
                    if (isBought==1)
                    {
                        item.purchased = true;
                    }

                    itemsMap[itemKey] = item;

                }
            }
        }
        
        
        //Loop through each item type and check weather or not there is a selected item for that type.
        //If not select first item of this type
        for (int i = 0; i < (int)StoreItemType.Count; i++)
        {
            StoreItemType type = (StoreItemType) i;

            if (PlayerPrefs.HasKey(type.ToString()) == false)
            {
                //Get first item of this type
                StoreItem[] items = getItemsByType(type);
                if (items.Length == 0)
                {
                    Debug.LogError("There is no element of this item type " + type.ToString());
                    continue;
                }

                if (items[0].purchased == false)
                {
                    Debug.LogError("First element of every item library must be purchased by default!");
                    PlayerPrefs.SetInt(items[0].itemKey,1);
                    items[0].purchased = true;
                }
                
                PlayerPrefs.SetString(type.ToString(),items[0].itemKey);
            }
        }
        

    }
    
    public StoreItemLibrary getStoreLibrary(StoreItemType libraryType)
    {
        return getLibraryByType(libraryType);
    }

    public void buyItem(StoreItem item)
    {
        item.purchased = true;
        PlayerPrefs.SetInt(item.itemKey, 1);
    }
    
    
    public StoreItemLibrary getLibraryByType(StoreItemType libraryType)
    {
        foreach (StoreItemLibrary storeItemLibrary in storeData.Libraries)
        {
            if (storeItemLibrary.itemType == libraryType)
            {
                return storeItemLibrary;
            }
        }
        
        Debug.LogError("Library of type "+libraryType.ToString()+" couldnt be found in data sturcture!");
        
        return new StoreItemLibrary();
    }

    public StoreItem[] getItemsByType(StoreItemType itemType)
    {
        StoreItemLibrary itemLibrary= getLibraryByType(itemType);
        
        List<StoreItem> items=new List<StoreItem>();
        
        foreach (StorePage page in itemLibrary.libraryPages)
        {
            foreach (StoreItem item in page.pageItems)
            {
                items.Add(item);
            }
        }

        return items.ToArray();
    }

    public StoreItem getSelectedItemOfType(StoreItemType itemType)
    {
        string key=PlayerPrefs.GetString(itemType.ToString(), "null");
        if (key == "null")
        {
            Debug.LogError("There is no selected item of type "+itemType.ToString()+" it should not be possible!");
            return null;
        }

        if (itemsMap.ContainsKey(key) == false)
        {
            Debug.LogError("There is no item with key "+key);
            return null;
        }
        
        // Debug.Log("Seleted item of type "+itemType.ToString()+" is "+itemsMap[key]);

        return itemsMap[key];
    }

    public void selectItem(StoreItem item, StoreItemType type)
    {
        //Verify weather or not this item is under of this type
        StoreItem[] items= getItemsByType(type);
        if (items.Contains(item) == false)
        {
            Debug.LogError("Item "+item.itemKey+" is not type of "+type.ToString());
            return;
        }

        if (item.purchased == false)
        {
            Debug.LogError("Item that is not purchased can not be selected!");
            return;
        }
        
        PlayerPrefs.SetString(type.ToString(),item.itemKey);
        
        onItemSelected.Invoke(type,item);
    }
    
    [Button("Cleare Store Data")]
    private void clearStoreData()
    {
        PlayerPrefs.DeleteAll();
    }
    
    
}
