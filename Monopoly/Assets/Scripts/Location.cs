using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

namespace Monopoly
{
    public class Location
    {
        public string name;
        public Vector3 gridPoint;

        // Put these here for now for convenience
        public Vector3[] houseLocations;
        public Quaternion houseRotation;

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
        public int baseRent;
        public int numHouses;
        public Player owner;

        public int[] propertyGroup;
        public House[] houses; // Store gameobjects of houses so we can destroy them if this property is sold
        
        public Property(string name, int price, int housePrice, int rent, Vector3 gridPoint, int[] propertyGroup) : base(name, gridPoint)
        {
            this.price = price;
            this.housePrice = housePrice;
            this.baseRent = rent;
            this.numHouses = 0;
            this.owner = null;

            this.propertyGroup = propertyGroup;
            this.houses = new House[] {null, null, null, null};
        }

        public int GetRent()
        {
            switch (this.numHouses) {
            case 0:
                int numOwned = 0;
                foreach (int propertyNum in this.propertyGroup) {
                    Property property = (Property)GameManager.instance.board.locations[propertyNum];
                    if (property.owner == this.owner) {
                        numOwned++;
                    }
                }
                if (numOwned == propertyGroup.Length) { // Owns all properties in group
                    return baseRent * 2;
                }
                else {
                    return baseRent;
                }
            case 1:
                return baseRent * 5;
            case 2:
                return baseRent * 5 * 3;
            case 3:
                return baseRent * 5 * 3 * 3;
            case 4:
                return (baseRent * 5 * 3 * 3) + 200;
            case 5:
                return ((baseRent * 5 * 3 * 3) + 200) + 200;
            }

            return -1; // Should not get here (placeholder since we need to cover all paths)
        }

        public void BuildHouse()
        {
            this.numHouses++;
        }

        public void SellHouse()
        {
            this.numHouses--;
        }

        public void Reset()
        {
            // Destroy house gameobjects
            if (numHouses == 5) {
                if (this.houses[0] != null) { // If player left game they are automatically destroyed
                    this.houses[0].DestroyHouse();
                }
            }
            else {
                for (int i = 0; i < this.numHouses; i++) {
                    if (this.houses[i] != null) {
                        this.houses[i].DestroyHouse();
                    }
                }
            }

            this.owner = null;
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