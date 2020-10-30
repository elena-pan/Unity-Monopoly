using System.Collections.Generic;
using UnityEngine;

public enum PieceType {Cylinder, Cube, ScottieDog, TopHat, Thimble, Shoe, Wheelbarrow, Car, Battleship};

public abstract class Piece : MonoBehaviour
{
    public PieceType type;
}