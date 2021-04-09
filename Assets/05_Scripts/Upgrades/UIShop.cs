using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIShop : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private Transform shopItemTemplate;
    private IShopCustomer shopCustomer;

    private GameManager _gameManager;

    private void Awake()
    {
       // container = transform.Find("Container");
       // shopItemTemplate = container.Find("ShopItemTemplate");
       // shopItemTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        _gameManager = GameManager.currentManager;
        shopCustomer = _gameManager.getCustomer(shopCustomer); //registers player as shopper 
        CreateItemButton(UpgradeTypes.ItemType.Nitrate, UpgradeTypes.GetSprite(UpgradeTypes.ItemType.Nitrate), "Nitrate", UpgradeTypes.GetCost(UpgradeTypes.ItemType.Nitrate), 0);
        CreateItemButton(UpgradeTypes.ItemType.Expansion, UpgradeTypes.GetSprite(UpgradeTypes.ItemType.Expansion), "Expansion", UpgradeTypes.GetCost(UpgradeTypes.ItemType.Expansion), 2);

        Hide();
    }

    private void CreateItemButton(UpgradeTypes.ItemType itemType, Sprite itemSprite, string itemName, int itemCost, int positionIndex)
    {
        Transform shopItemTransform = Instantiate(shopItemTemplate, container);
        RectTransform shopItemRectTransform = shopItemTransform.GetComponent<RectTransform>();

        float shopItemHeight = 30f;
        shopItemRectTransform.anchoredPosition = new Vector2(0, -shopItemHeight * positionIndex);
        
        shopItemTransform.Find("itemName").GetComponent<TextMeshProUGUI>().SetText(itemName);
        shopItemTransform.Find("itemCost").GetComponent<TextMeshProUGUI>().SetText(itemCost.ToString());
        shopItemTransform.Find("itemImage").GetComponent<Image>().sprite = itemSprite;

        //shopItemTransform.GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
        shopItemTransform.GetComponent<Button>().onClick.AddListener(delegate { TryBuyItems(itemType); });

    }

    private void TryBuyItems(UpgradeTypes.ItemType itemType)
    {
        if (shopCustomer.TrySpendSugarAmount(UpgradeTypes.GetCost(itemType)))
        {
            shopCustomer.BoughtItem(itemType);
        }
        else
        {
            Debug.Log("cannot afford upgrade");
            //add warning if can't afford

        }
    }

    public void Show(IShopCustomer iShopCustomer)
    {
        //this.shopCustomer = shopCustomer;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
