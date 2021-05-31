using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;


public class StoreTabController:MonoBehaviour
{

    public static StoreTabController selectedTab = null;
    public List<StorePageController> pages { get; private set; }
    
    private StoreItemLibrary library;
    private StoreUIController storeUiController;
    private Button tabButton;
    
    public StoreItemType tabType { get; private set; }

    public Image iconField;
    public Image tabImage;
    
    private Color tabColor;
    
    public float delselctedColorFactor = 1.1f;

    
    
    
    public void BuildTab(StoreItemLibrary library , StoreUIController storeUiController)
    {
        tabButton = GetComponent<Button>();

        
        this.storeUiController = storeUiController;
        this.library = library;

        tabType = library.itemType;

        pages=new List<StorePageController>();
        
        for (int i=0;i<library.libraryPages.Length;i++)
        {
            GameObject page = Instantiate(storeUiController.storePagePrefab, storeUiController.storeInactivePagePanel);
            StorePageController pageController = page.GetComponent<StorePageController>();
            pageController.BuildPage(library.libraryPages[i], this.library.itemType ,storeUiController);
            pages.Add(pageController);
        }

        
        if (library.libraryIcon != null)
        {
            
            iconField.sprite = library.libraryIcon;
        }
        
        
        tabColor = tabImage.color;
        tabImage.color = tabColor*delselctedColorFactor;
        
        //Add event to button
        tabButton.onClick.AddListener(Select);
        
        
    }
    
    public void Select()
    {
	if(storeUiController.shuffling) return;

        if (selectedTab != null)
        {
            selectedTab.Deselect();
        }

        foreach (StorePageController storePage in pages)
        {
            storePage.transform.localScale = Vector3.one;
            storeUiController.scrollController.AddChild(storePage.gameObject);
            storePage.Activate();
        }
        
        selectedTab = this;
        
        storeUiController.updatePreviewPanel();
        
        tabImage.color = tabColor;
    }

    public void Deselect()
    {
        
        storeUiController.scrollController.RemoveAllChildren(out GameObject[] removed);
        foreach (StorePageController storePage in pages)
        {
            storePage.Deactivate();
            storePage.transform.parent = storeUiController.storeInactivePagePanel;
        }
        
        selectedTab = null;

        tabImage.color = tabColor*delselctedColorFactor;

    }

    public void Destroy()
    {
        foreach (StorePageController storePageController in pages)
        {
            Destroy(storePageController.gameObject);
        }
        
        Destroy(gameObject);
        
        
    }
    
}