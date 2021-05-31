using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;


public class StoreUIController : MonoBehaviour
{

    public StoreController storeController;
    
    public GameObject storeTabPrefab;
    public GameObject storePagePrefab;
    public GameObject storeCellPrefab;

    public GridLayoutGroup storeTabPanel;
    public Transform storeInactivePagePanel;

    public Sprite unkownSprite;

    public GameObject buttonPrefab;
    
    private GameObject activeTab;

    private List<StoreTabController> storeTabs;

    public HorizontalScrollSnap scrollController;

    public Image previewPanel;

    public bool shuffling = false;


    
    // Start is called before the first frame update
    void Start()
    {
        buildStore();
    }

    private void buildStore()
    {
        clearStore();
        
        if(StoreItemType.Count==0) return;
        
        
        storeTabs=new List<StoreTabController>();
        
        //Create tab for each store item type
        for (int i = 0; i < (int)StoreItemType.Count; i++)
        {
            StoreItemType type = (StoreItemType) i;
            StoreItemLibrary storeLibrary= storeController.getStoreLibrary(type);
            
            //Create tab object and cell objects 
            GameObject storeTab = Instantiate(storeTabPrefab, storeTabPanel.transform);
            storeTab.name = "Tab " + i;
            StoreTabController storeTabController=storeTab.GetComponent<StoreTabController>();
            storeTabController.BuildTab(storeLibrary,this);
            
            storeTabs.Add(storeTabController);

        }
        
        
        
    }

    private void clearStore()
    {
        if(storeTabs==null) return;
        
        foreach (StoreTabController tab in storeTabs)
        {
            tab.Destroy();
        }
        
        storeTabs.Clear();
    }

    public void Activate()
    {
        for(int i=0;i<transform.childCount;i++) transform.GetChild(i).gameObject.SetActive(true);
       
        //Select first tab by default when build store
        storeTabs[0].Select();
        

    }

    public void Deactivate()
    {
	if(shuffling) return;

        for(int i=0;i<transform.childCount;i++) transform.GetChild(i).gameObject.SetActive(false);
    }

    public void itemSelected(StoreItem selectedItem, StoreItemType type)
    {
        storeController.selectItem(selectedItem,type);
        updatePreviewPanel();
      
    }

    public void updatePreviewPanel()
    {
        if(StoreTabController.selectedTab==null) return;
        
        StoreItemType selectedTabType = StoreTabController.selectedTab.tabType;

        StoreItem selectedItem= storeController.getSelectedItemOfType(selectedTabType);

        if (selectedItem!=null && selectedItem.showCaseIcon != null)
        {
            previewPanel.sprite = selectedItem.showCaseIcon;
            previewPanel.enabled = true;
        }
        else
        {
            previewPanel.enabled = false;
        }
    }

    public void itemBought(StoreItem item)
    {
        storeController.buyItem(item);
    }
    
}

