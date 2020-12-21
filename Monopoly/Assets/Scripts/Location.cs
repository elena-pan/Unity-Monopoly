using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

namespace Monopoly
{
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

        public void BuildHouse()
        {
            switch (this.numHouses) {
            case 0:
                rent = rent * 5;
                break;
            case 1:
                rent = rent * 3;
                break;
            case 2:
                rent = rent * 3;
                break;
            case 3:
                rent = rent + 200;
                break;
            case 4:
                rent = rent + 200;
                break;
            }
            this.numHouses++;
        }

        public void SellHouse()
        {
            switch (this.numHouses) {
            case 1:
                this.rent = this.rent / 5;
                break;
            case 2:
                this.rent = this.rent / 3;
                break;
            case 3:
                this.rent = this.rent / 3;
                break;
            case 4:
                this.rent = this.rent - 200;
                break;
            case 5:
                this.rent = this.rent - 200;
                break;
            }
            this.numHouses--;
        }

        public void Reset()
        {
            this.owner = null;
            switch (this.numHouses) {
            case 0:
                break;
            case 1:
                this.rent = this.rent / 5;
                break;
            case 2:
                this.rent = this.rent / 15;
                break;
            case 3:
                this.rent = this.rent / 45;
                break;
            case 4:
                this.rent = (this.rent - 200) / 45;
                break;
            case 5:
                this.rent = (this.rent - 400) / 45;
                break;
            }
	        this.numHouses = 0;
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
        public int GetRent()
        {
            int numRailroadsOwned = 0;
            bool[] ownedProperties = (bool[])owner.CustomProperties["OwnedProperties"];
            if (ownedProperties[5]) numRailroadsOwned++;
            if (ownedProperties[15]) numRailroadsOwned++;
            if (ownedProperties[25]) numRailroadsOwned++;
            if (ownedProperties[35]) numRailroadsOwned++;

            switch (numRailroadsOwned) {
                case 1:
                    return 25;
                case 2:
                    return 50;
                case 3:
                    return 100;
                case 4:
                    return 200;
            }

            return -1; // Error
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
        public int GetRent(int diceRoll)
        {
            int numUtilitiesOwned = 0;
            bool[] ownedProperties = (bool[])owner.CustomProperties["OwnedProperties"];
            if (ownedProperties[12]) numUtilitiesOwned++;
            if (ownedProperties[28]) numUtilitiesOwned++;

            switch (numUtilitiesOwned) {
                case 1:
                    return 4*diceRoll;
                case 2:
                    return 10*diceRoll;
            }

            return -1; // Error
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
}