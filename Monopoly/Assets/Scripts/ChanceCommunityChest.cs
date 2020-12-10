using System.Collections.Generic;
using UnityEngine;

public class Card
{
    public string description;
    public int amountMoney;
    public int? moveTo;
    public Card(string description, int amountMoney, int? moveTo)
    {
        this.description = description;
        this.amountMoney = amountMoney;
        this.moveTo = moveTo;
    }
}