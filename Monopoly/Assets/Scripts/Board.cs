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
        this.locations.Add(new Location("Mediterreanean Avenue", new Vector3(25.0f, 0f, 20.0f)));
        this.locations.Add(new Location("Community Chest", new Vector3(25.0f, 0f, 15.0f)));
        this.locations.Add(new Location("Baltic Avenue", new Vector3(25.0f, 0f, 10.0f)));
        this.locations.Add(new Location("Income Tax", new Vector3(25.0f, 0f, 5.0f)));
        this.locations.Add(new Location("Reading Railroad", new Vector3(25.0f, 0f, 0.0f)));
        this.locations.Add(new Location("Oriental Avenue", new Vector3(25.0f, 0f, -5.0f)));
        this.locations.Add(new Location("Chance", new Vector3(25.0f, 0f, -10.0f)));
        this.locations.Add(new Location("Vermont Avenue", new Vector3(25.0f, 0f, -15.0f)));
        this.locations.Add(new Location("Connecticut Avenue", new Vector3(25.0f, 0f, -20.0f)));
        this.locations.Add(new Location("Jail", new Vector3(25.0f, 0f, -25.0f)));
        this.locations.Add(new Location("St. Charles Place", new Vector3(20.0f, 0f, -25.0f)));
        this.locations.Add(new Location("Electric Company", new Vector3(15.0f, 0f, -25.0f)));
        this.locations.Add(new Location("States Avenue", new Vector3(10.0f, 0f, -25.0f)));
        this.locations.Add(new Location("Virginia Avenue", new Vector3(5.0f, 0f, -25.0f)));
        this.locations.Add(new Location("Pennsylvania Railroad", new Vector3(0.0f, 0f, -25.0f)));
        this.locations.Add(new Location("St. James Place", new Vector3(-5.0f, 0f, -25.0f)));
        this.locations.Add(new Location("Community Chest", new Vector3(-10.0f, 0f, -25.0f)));
        this.locations.Add(new Location("Tennessee Avenue", new Vector3(-15.0f, 0f, -25.0f)));
        this.locations.Add(new Location("New York Avenue", new Vector3(-20.0f, 0f, -25.0f)));
        this.locations.Add(new Location("Free Parking", new Vector3(-25.0f, 0f, -25.0f)));
        this.locations.Add(new Location("Kentucky Avenue", new Vector3(-25.0f, 0f, -20.0f)));
        this.locations.Add(new Location("Chance", new Vector3(-25.0f, 0f, -15.0f)));
        this.locations.Add(new Location("Indiana Avenue", new Vector3(-25.0f, 0f, -10.0f)));
        this.locations.Add(new Location("Illinois Avenue", new Vector3(-25.0f, 0f, -5.0f)));
        this.locations.Add(new Location("B. & O. Railroad", new Vector3(-25.0f, 0f, 0.0f)));
        this.locations.Add(new Location("Atlantic Avenue", new Vector3(-25.0f, 0f, 5.0f)));
        this.locations.Add(new Location("Ventnor Avenue", new Vector3(-25.0f, 0f, 10.0f)));
        this.locations.Add(new Location("Water Works", new Vector3(-25.0f, 0f, 15.0f)));
        this.locations.Add(new Location("Marvin Gardens", new Vector3(-25.0f, 0f, 20.0f)));
        this.locations.Add(new Location("Go To Jail", new Vector3(-25.0f, 0f, 25.0f)));
        this.locations.Add(new Location("Pacific Avenue", new Vector3(-20.0f, 0f, 25.0f)));
        this.locations.Add(new Location("North Carolina Avenue", new Vector3(-15.0f, 0f, 25.0f)));
        this.locations.Add(new Location("Community Chest", new Vector3(-10.0f, 0f, 25.0f)));
        this.locations.Add(new Location("Pennsylvania Avenue", new Vector3(-5.0f, 0f, 25.0f)));
        this.locations.Add(new Location("Short Line", new Vector3(0.0f, 0f, 25.0f)));
        this.locations.Add(new Location("Chance", new Vector3(5.0f, 0f, 25.0f)));
        this.locations.Add(new Location("Park Place", new Vector3(10.0f, 0f, 25.0f)));
        this.locations.Add(new Location("Luxury Tax", new Vector3(15.0f, 0f, 25.0f)));
        this.locations.Add(new Location("Boardwalk", new Vector3(20.0f, 0f, 25.0f)));
    }
}
