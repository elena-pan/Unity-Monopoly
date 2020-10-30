using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Board board;
    public Dice dice;

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
        NextPlayer();
    }

    public void AddPiece(GameObject prefab, Player player, int location)
    {
        GameObject pieceObject = board.AddPiece(prefab, location);
        player.piece = pieceObject;
    }

    public void Move(Player player, int steps)
    {
        int location = player.location;
        location += steps;
        if (location > 39) {
            location = location - 39;
        }
        board.MovePiece(player.piece, location);
        player.location = location;
        NextPlayer();
    }

    public void NextPlayer()
    {
        Player tempPlayer = currentPlayer;
        currentPlayer = otherPlayer;
        otherPlayer = tempPlayer;

        StartCoroutine(WaitForDiceRoll());
    }
    
    private IEnumerator WaitForDiceRoll() {
    
        // wait for player to press space
        yield return WaitForKeyPress(KeyCode.Space); // wait for this function to return
    
        // Roll dice after player presses space
        dice.RollDice();
        while (dice.currentNum == -1) {
            yield return new WaitForSeconds(1);
        }
        Move(currentPlayer, dice.currentNum);
    }
    
    private IEnumerator WaitForKeyPress(KeyCode key)
    {
        bool done = false;
        while(!done) // essentially a "while true", but with a bool to break out naturally
        {
            if(Input.GetKeyDown(key))
            {
                done = true; // breaks the loop
            }
            yield return null; // wait until next frame, then continue execution from here (loop continues)
        }
    
        // now this function returns
    }
}
