using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item {

    /// <summary>
    /// Aisle number where the item is stored.
    /// </summary>
    /// 

    internal string ItemName;

    public int Aisle { get; set; }

    /// <summary>
    /// Bin number where the item is stored.
    /// </summary>
    public int Bin { get; set; }

    /// <summary>
    /// Quantity required by the order.
    /// </summary>
    public int QuantityRequired { get; set; }

    public int QuantityPicked { get; set; } = default;


    public Item(int aisle, int bin, int quantityRequired, string name) {
        Aisle = aisle;
        Bin = bin;
        QuantityRequired = quantityRequired;
        ItemName = name;
    }

}
