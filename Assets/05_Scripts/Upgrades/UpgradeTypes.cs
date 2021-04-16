using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeTypes : MonoBehaviour
{
    public static Sprite nutrientImage;
    public static Sprite expansionImage;
    public enum ItemType 
    {
        Nutrient,
        Expansion,
    }

    public static int GetCost(ItemType itemType)
    {
        switch (itemType)
        {
           default:
           case ItemType.Nutrient: return NutrientManager.currentNutrientManager.nutrientCost;
           case ItemType.Expansion: return 10;

        }
    }

    public static Sprite GetSprite(ItemType itemType)
    {
        switch (itemType)
        {
          default:
          case ItemType.Nutrient: return nutrientImage; 
          case ItemType.Expansion: return expansionImage; 

        }
    }
}
