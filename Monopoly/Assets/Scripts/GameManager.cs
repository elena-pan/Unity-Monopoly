using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Board board;

    private Player one;
    private Player two;
    public Player currentPlayer;
    public Player otherPlayer;

    public GameObject cylinder;
    public GameObject cube;

    public GameObject scottieDog;
    public GameObject topHat;
    public GameObject thimble;
    public GameObject shoe;
    public GameObject wheelbarrow;
    public GameObject car; 
    public GameObject battleship;
    void Awake()
    {
        instance = this;
    }

    void Start ()
    {
        one = new Player("1");
        two = new Player("2");
        currentPlayer = one;
        otherPlayer = two;

        InitialSetup();
    }

    private void InitialSetup()
    {
        board.SetUpLocations();
        AddPiece(cylinder, one, 0);
        AddPiece(cube, two, 0);
    }

    public void AddPiece(GameObject prefab, Player player, int location)
    {
        GameObject pieceObject = board.AddPiece(prefab, location);
        player.piece = pieceObject;
    }

    public void Move(Player player, int location)
    {
        board.MovePiece(player.piece, location);
        player.location = location;
    }

    public void NextPlayer()
    {
        Player tempPlayer = currentPlayer;
        currentPlayer = otherPlayer;
        otherPlayer = tempPlayer;
    }
}
