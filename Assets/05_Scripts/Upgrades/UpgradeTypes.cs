using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeTypes : MonoBehaviour
{
    public static Sprite nitrateImage;
    public static Sprite expansionImage;
    public enum ItemType 
    {
        Nitrate,
        Expansion,
    }

    public static int GetCost(ItemType itemType)
    {
        switch (itemType)
        {
           default:
           case ItemType.Nitrate: return 10;
           case ItemType.Expansion: return 10;

        }
    }

    public static Sprite GetSprite(ItemType itemType)
    {
        switch (itemType)
        {
          default:
          case ItemType.Nitrate: return nitrateImage; 
          case ItemType.Expansion: return expansionImage; 

        }
    }
}
