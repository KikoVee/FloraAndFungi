using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShopCustomer
{
    void BoughtItem(UpgradeTypes.ItemType itemType);
    bool TrySpendSugarAmount (int sugarAmount);
}
