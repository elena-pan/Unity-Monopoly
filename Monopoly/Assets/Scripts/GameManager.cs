using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Board board;
    public Dice dice;

    public int currentPlayer;
    public List<Player> players = new List<Player>();
    
    public GameObject cylinder;
    public GameObject cube;
    public GameObject capsule;
    public GameObject sphere;
    public List<GameObject> pieces = new List<GameObject>();

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
        currentPlayer = 0;

        pieces.Add(cylinder);
        pieces.Add(cube);
        pieces.Add(capsule);
        pieces.Add(sphere);

        Player one = new Player("1");
        Player two = new Player("2");
        Player three = new Player("3");
        Player four = new Player("4");
        Player five = new Player("5");
        Player six = new Player("6");
        Player seven = new Player("7");
        Player eight = new Player("8");

        players.Add(one);
        players.Add(two);
        players.Add(three);
        players.Add(four);

        InitialSetup();
    }

    private void InitialSetup()
    {
        board.SetUpLocations();
        board.SetUpCards();
        for (int i = 0; i < players.Count; i++) {
            AddPiece(pieces[i], players[i], 0);
        }
        NextPlayer();
    }

    public void AddPiece(GameObject prefab, Player player, int location)
    {
        GameObject pieceObject = board.AddPiece(prefab, location);
        player.piece = pieceObject;
    }

    public void Move(Player player, int steps)
    {
        int location;
        
        location = player.location;
        location+=steps;
        if (location > 39)
        {
            location = location - 39;
        }
        board.MovePiece(player.piece, location);
        player.location = location;
        
        NextPlayer();
    }

    public void LandedOn(Player player)
    {
        switch (typeof(player.location))
        {
            case Property:
                break;
            case Railroad:
                break;
            case Utility:
                break;
            case Tax:
                break;
            case Location:
                break;
        }
    }

    public void NextPlayer()
    {
        if (currentPlayer == players.Count-1) {
            currentPlayer = 0;
        } else {
            currentPlayer++;
        }

        if (players[currentPlayer].isBankrupt) {
            NextPlayer();
            return;
        }

        StartCoroutine(WaitForDiceRoll(diceNum => {
            Move(players[currentPlayer], diceNum);
        }));
    }

    public void ReceiveMoneyFromBank(Player recipient, int amount)
    {
        recipient.balance += amount;
    }

    public void PayMoney(Player payer, Player? recipient, int amount)
    {
        if (payer.balance < amount) {
            NoMoney(payer, amount);
        }

        payer.balance -= amount;
        if (recipient != null) {
            recipient.balance += amount;
        }

    }

    public void NoMoney(Player player, int amount)
    {

    }

    public void DisplayText(string message)
    {
        
    }
    
    private IEnumerator WaitForDiceRoll(System.Action<int> callback) {
    
        // wait for player to press space
        yield return WaitForKeyPress(KeyCode.Space); // wait for this function to return
    
        // Roll dice after player presses space
        dice.RollDice();
        while (dice.currentNum == -1) {
            yield return new WaitForSeconds(3);
        }

        callback(dice.currentNum); // Use callback to do something with result
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
