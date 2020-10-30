using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public GameObject piece;

    public string name;

    public Player(string name)
    {
        this.name = name;
        piece = new GameObject();
    }
}
