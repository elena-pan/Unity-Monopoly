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
