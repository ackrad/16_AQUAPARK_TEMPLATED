using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorePageController : MonoBehaviour
{
    private List<StoreCellController> pageCells;
    private StoreUIController storeUiController;
    private StorePage page;

    public GridLayoutGroup cellGrids;
    public GridLayoutGroup buttonGrids;

    private Button randomBuyButton;

    private GameController gameController;
    
    

    public void BuildPage(StorePage page,  StoreItemType libraryTpe ,StoreUIController storeUiController)
    {
        this.storeUiController = storeUiController;
        this.page = page;
        
        
        pageCells=new List<StoreCellController>();

        StoreItem[] items = page.pageItems;
        
        for (int i = 0; i < items.Length; i++)
        {
            GameObject cell = Instantiate(storeUiController.storeCellPrefab,cellGrids.transform);
            StoreCellController cellController = cell.GetComponent<StoreCellController>();
            cellController.BuildCell(items[i], libraryTpe ,storeUiController);
            pageCells.Add(cellController);
        }

        createRandomButton();

        gameController = GameController.request();
        gameController.onMoneyChanged.AddListener(moneyUpdated);
        moneyUpdated(gameController.Money);
        
        Deactivate();
        
    }
    
    public void Activate()
    {
        
        updateButtonPrice();
        
        gameObject.SetActive(true);   
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private void createRandomButton()
    {
        randomBuyButton = Instantiate(storeUiController.buttonPrefab,buttonGrids.transform).GetComponent<Button>();
        randomBuyButton.gameObject.SetActive(false);
        randomBuyButton.onClick.AddListener(buyLowestPrice);
    }

    //Must be called when money of player is updated
    private void moneyUpdated(float money)
    {
        StoreCellController lowersPriceCell = findCellOfLowestPriceItem();

        if (lowersPriceCell != null)
        {
            float cellPrice = lowersPriceCell.cellItem.price;
            if (money > cellPrice)
                randomBuyButton.interactable = false;
            else
                randomBuyButton.interactable = true;
        }
    }

    private void updateButtonPrice()
    {
        StoreCellController cell= findCellOfLowestPriceItem();
        if (cell != null)
        {
            float cellPrice = cell.cellItem.price;
            randomBuyButton.GetComponentInChildren<Text>().text = "Random Buy " + cellPrice;
            randomBuyButton.gameObject.SetActive(true);

            float gameMoney = gameController.Money;
            if (gameMoney >= cellPrice)
                randomBuyButton.interactable = true;
            else
                randomBuyButton.interactable = false;
            
        }
        else
        {
            //It means there is no unpaurchased cell left!
            randomBuyButton.gameObject.SetActive(false);
        }
    }
    
    private void buyLowestPrice()
    {
        List<StoreCellController> shuffleCells = getUnpurchasedCells();
        if (shuffleCells.Count == 0)
        {
            Debug.LogError("There is no cell that is not bought");
            return;
        }

        StoreCellController lowestPriceCell=findCellOfLowestPriceItem();
        StartCoroutine(_shuffleToButton(shuffleCells, lowestPriceCell));
        gameController.SpendMoney(lowestPriceCell.cellItem.price);
        
        //Disable but button immidieatly
        randomBuyButton.interactable = false;

    }

    IEnumerator _shuffleToButton(List<StoreCellController> shuffleElements, StoreCellController aimElement)
    {

        storeUiController.shuffling = true;
        
        float maxTimeBetweenShuffles = 0.5f;
        float minTimeBetweenShuffles = 0.05f;
    
        int numberOfShuffles = 12;
        
        //If there is only one shuffle element buy it directly after small delay withoput shuffle
        if (shuffleElements.Count == 1)
        {
            yield return new WaitForSeconds(0.5f);
            aimElement.unlockElement();
            aimElement.onClick();
            updateButtonPrice();
	    storeUiController.shuffling = false;
            yield break;
        }
        
        //Shuffle randomly through icons with swithing their sprites
        for (int i = 0; i < numberOfShuffles; i++)
        {
            int index = i % shuffleElements.Count;
            StoreCellController cell = shuffleElements[index];
            cell.selectCell();
            
            // _hapticFeedbackController.vibrateLight();
            yield return new WaitForSeconds(Mathf.Lerp(maxTimeBetweenShuffles,minTimeBetweenShuffles,(float)i/(float)numberOfShuffles ));
            
            cell.unselectCell();
        }
        
        
        aimElement.unlockElement();
        aimElement.onClick();
        updateButtonPrice();
        
        storeUiController.shuffling = false;
        
        yield break;
        
    }

    //Searches the cell having lowest price and not bought yet
    StoreCellController findCellOfLowestPriceItem()
    {
        List<StoreCellController> unboughtItems = getUnpurchasedCells();

        if (unboughtItems.Count == 0) return null;
        
        unboughtItems.Sort(((cell1, cell2) =>cell1.cellItem.price.CompareTo(cell2.cellItem.price) ));

        return unboughtItems[0];
    }

    private List<StoreCellController> getUnpurchasedCells()
    {
        List<StoreCellController> unboughtItems = new List<StoreCellController>();

        foreach (StoreCellController cell in pageCells)
        {
            if (cell.cellItem.purchased)
                continue;
            unboughtItems.Add(cell);
        }

        return unboughtItems;
    }

    void OnDestroy()
    {
        if(gameController!=null) gameController.onMoneyChanged.RemoveListener(moneyUpdated);
    }

}