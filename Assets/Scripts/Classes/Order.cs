using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order {

    /// <summary>
    ///  List of items being ordered.
    /// </summary>
    public List<Item> items = new List<Item>();
    private const int item_number = 5;

    public Order() {

        for (int i = 0; i < item_number; i++) {
            items.Add(RandomItem());
        }
        Debug.Log(items[0].QuantityRequired.ToString() + " " + items[1].QuantityRequired.ToString() + " " + items[2].QuantityRequired.ToString() + " " + items[3].QuantityRequired.ToString() + " " + items[4].QuantityRequired.ToString() + " ");
    }

    public Item RandomItem() {
        int aisle = Random.Range(1, 3);
        int bin = Random.Range(1, 9);
        //int quantityRequired = 5;
        int quantityRequired = Random.Range(2, 6);
        string name = "Aisle " + aisle + ";Bin " + bin;

        return new Item(aisle, bin, quantityRequired, name);
    }

    public string GetHudValuesText(int currentIndex) {
        return $"                          {currentIndex + 1}/{item_number}            {items[currentIndex].Aisle}         {items[currentIndex].Bin}          {items[currentIndex].QuantityRequired}";

    }
}