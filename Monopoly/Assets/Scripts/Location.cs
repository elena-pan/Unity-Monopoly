using System.Collections.Generic;
using UnityEngine;

public class Location
{
    public string name;
    public Vector3 gridPoint;
    public Location(string name, Vector3 gridPoint)
    {
        this.name = name;
        this.gridPoint = gridPoint;
    }
}

public class Property : Location
{
    public int price;
    public int housePrice;
    public int rent;
    public int numHouses;
    public Player owner;
    public Property(string name, int price, int housePrice, int rent, Vector3 gridPoint) : base(name, gridPoint)
    {
        this.price = price;
        this.housePrice = housePrice;
        this.rent = rent;
        this.numHouses = 0;
        this.owner = null;
    }
}

public class Railroad : Location
{
    public int price = 200;
    public Player owner;
    public Railroad(string name, Vector3 gridPoint) :  base(name, gridPoint)
    {
        this.owner = null;
    }
    public int getRent()
    {
        return 50;
    }
}

public class Utility : Location
{
    public int price = 150;
    public Player owner;
    public Utility(string name, Vector3 gridPoint) : base(name, gridPoint)
    {
        this.owner = null;
    }
    public int get(int diceRoll)
    {
        return 4;
    }
}

public class Tax : Location
{
    public int tax;
    public Tax(string name, int tax, Vector3 gridPoint) : base(name, gridPoint)
    {
        this.tax = tax;
    }
}