using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Board : MonoBehaviour
{
    public Material defaultMaterial;
    public Material selectedMaterial;
    public List<Location> locations;

    public GameObject AddPiece(GameObject piece, int location)
    {
        //print("a");
        GameObject newPiece = Instantiate(piece, locations[location].gridPoint, Quaternion.identity, gameObject.transform);
        return newPiece;
    }

    public void RemovePiece(GameObject piece)
    {
        Destroy(piece);
    }

    public void MovePiece(GameObject piece, int location)
    {
        piece.transform.position = locations[location].gridPoint;
    }

    public void SetUpLocations()
    {
        this.locations = new List<Location>();
        this.locations.Add(new Location("GO", new Vector3(25.0f, 0f, 25.0f)));
        this.locations.Add(new Property("Mediterreanean Avenue", 60, 50, 2, new Vector3(25.0f, 0f, 20.0f)));
        this.locations.Add(new Location("Community Chest", new Vector3(25.0f, 0f, 15.0f)));
        this.locations.Add(new Property("Baltic Avenue", 60, 50, 4, new Vector3(25.0f, 0f, 10.0f)));
        this.locations.Add(new Tax("Income Tax", 200, new Vector3(25.0f, 0f, 5.0f)));
        this.locations.Add(new Railroad("Reading Railroad", new Vector3(25.0f, 0f, 0.0f)));
        this.locations.Add(new Property("Oriental Avenue", 100, 50, 6, new Vector3(25.0f, 0f, -5.0f)));
        this.locations.Add(new Location("Chance", new Vector3(25.0f, 0f, -10.0f)));
        this.locations.Add(new Property("Vermont Avenue", 100, 50, 6, new Vector3(25.0f, 0f, -15.0f)));
        this.locations.Add(new Property("Connecticut Avenue", 120, 50, 8, new Vector3(25.0f, 0f, -20.0f)));
        this.locations.Add(new Location("Jail", new Vector3(25.0f, 0f, -25.0f)));
        this.locations.Add(new Property("St. Charles Place", 140, 100, 10, new Vector3(20.0f, 0f, -25.0f)));
        this.locations.Add(new Utility("Electric Company", new Vector3(15.0f, 0f, -25.0f)));
        this.locations.Add(new Property("States Avenue", 140, 100, 10, new Vector3(10.0f, 0f, -25.0f)));
        this.locations.Add(new Property("Virginia Avenue", 160, 100, 12, new Vector3(5.0f, 0f, -25.0f)));
        this.locations.Add(new Railroad("Pennsylvania Railroad", new Vector3(0.0f, 0f, -25.0f)));
        this.locations.Add(new Property("St. James Place", 180, 100, 14, new Vector3(-5.0f, 0f, -25.0f)));
        this.locations.Add(new Location("Community Chest", new Vector3(-10.0f, 0f, -25.0f)));
        this.locations.Add(new Property("Tennessee Avenue", 180, 100, 14, new Vector3(-15.0f, 0f, -25.0f)));
        this.locations.Add(new Property("New York Avenue", 200, 100, 16, new Vector3(-20.0f, 0f, -25.0f)));
        this.locations.Add(new Location("Free Parking", new Vector3(-25.0f, 0f, -25.0f)));
        this.locations.Add(new Property("Kentucky Avenue", 220, 150, 18, new Vector3(-25.0f, 0f, -20.0f)));
        this.locations.Add(new Location("Chance", new Vector3(-25.0f, 0f, -15.0f)));
        this.locations.Add(new Property("Indiana Avenue", 220, 150, 18, new Vector3(-25.0f, 0f, -10.0f)));
        this.locations.Add(new Property("Illinois Avenue", 240, 150, 20, new Vector3(-25.0f, 0f, -5.0f)));
        this.locations.Add(new Railroad("B. & O. Railroad", new Vector3(-25.0f, 0f, 0.0f)));
        this.locations.Add(new Property("Atlantic Avenue", 260, 150, 22, new Vector3(-25.0f, 0f, 5.0f)));
        this.locations.Add(new Property("Ventnor Avenue", 260, 150, 22, new Vector3(-25.0f, 0f, 10.0f)));
        this.locations.Add(new Utility("Water Works", new Vector3(-25.0f, 0f, 15.0f)));
        this.locations.Add(new Property("Marvin Gardens", 280, 150, 24, new Vector3(-25.0f, 0f, 20.0f)));
        this.locations.Add(new Location("Go To Jail", new Vector3(-25.0f, 0f, 25.0f)));
        this.locations.Add(new Property("Pacific Avenue", 300, 200, 26, new Vector3(-20.0f, 0f, 25.0f)));
        this.locations.Add(new Property("North Carolina Avenue", 300, 200, 26, new Vector3(-15.0f, 0f, 25.0f)));
        this.locations.Add(new Location("Community Chest", new Vector3(-10.0f, 0f, 25.0f)));
        this.locations.Add(new Property("Pennsylvania Avenue", 320, 200, 28, new Vector3(-5.0f, 0f, 25.0f)));
        this.locations.Add(new Railroad("Short Line", new Vector3(0.0f, 0f, 25.0f)));
        this.locations.Add(new Location("Chance", new Vector3(5.0f, 0f, 25.0f)));
        this.locations.Add(new Property("Park Place", 350, 200, 35, new Vector3(10.0f, 0f, 25.0f)));
        this.locations.Add(new Tax("Luxury Tax", 100, new Vector3(15.0f, 0f, 25.0f)));
        this.locations.Add(new Property("Boardwalk", 400, 200, 50, new Vector3(20.0f, 0f, 25.0f)));
    }
}
