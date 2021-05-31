using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreCellController : MonoBehaviour
{
    //Stores current selected cell within key of item type
    private static Dictionary<StoreItemType, StoreCellController> selectedCells;
    
    private StoreUIController storeUiController;
    private Button cellButton;
    public StoreItem cellItem { get; private set; }

    public Image buttonImage;
    public Image containerImage;

    public Color selectedColor = Color.red;
    public Color unselectedColor = Color.white;

    private StoreItemType cellItemType;
    
    
    public void BuildCell(StoreItem cellItem, StoreItemType type , StoreUIController storeUiController)
    {
        if (selectedCells == null)
        {
            selectedCells=new Dictionary<StoreItemType, StoreCellController>();
        }
        
        cellButton = GetComponent<Button>();
        this.cellItem = cellItem;
        this.storeUiController = storeUiController;
        this.cellItemType = type;
        
        //Add onclick event to button
        cellButton.onClick.AddListener(onClick);

        if (cellItem.purchased)
        {
            if (cellItem.storeIcon == null)
            {
                Debug.Log("Item "+cellItem.itemKey+" does not have any store icon");
                return;
            }
          
            buttonImage.sprite = cellItem.storeIcon;
        }
        else
        {
            buttonImage.sprite = storeUiController.unkownSprite;
        }
        
        //If this item is selected call select Cell function
        if (storeUiController.storeController.getSelectedItemOfType(cellItemType) == cellItem)
        {
            selectCell();
        }
        
    }

    public void onClick()
    {
        if(storeUiController.shuffling) return;
        
        if (cellItem.purchased)
        {
            selectCell();
            storeUiController.itemSelected(cellItem,cellItemType);
            
        }
    }

    public void unlockElement()
    {
        if (cellItem.purchased == false)
        {
            storeUiController.itemBought(cellItem);
            buttonImage.sprite = cellItem.storeIcon;
        }
    }

    public void selectCell()
    {
        if (selectedCells.ContainsKey(cellItemType))
        {
            selectedCells[cellItemType].unselectCell();
        }
        
        containerImage.color = selectedColor;
        selectedCells[cellItemType] = this;
    }

    public void unselectCell()
    {
        if (selectedCells.ContainsKey(cellItemType) == this)
        {
            selectedCells.Remove(cellItemType);
        }
        
        containerImage.color = unselectedColor;
    }
    
    
}
