using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public GameObject piece;
    public string name;
    public int location;

    public Player(string name)
    {
        this.name = name;
        piece = new GameObject();
        location = 0;
    }
}
