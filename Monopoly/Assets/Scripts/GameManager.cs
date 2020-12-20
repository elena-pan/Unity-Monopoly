using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

namespace Monopoly
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        public static GameManager instance;

        public Board board;
        public Dice dice;

        public int currentPlayer;
        public List<Player> players = new List<Player>();

        public GameObject cannon;
        public GameObject car;
        public GameObject iron;
        public GameObject shoe;
        public GameObject locomotive;
        public GameObject thimble; 
        public GameObject topHat;
        public Dictionary<GameObject, bool> pieces;

        public GameObject house;
        public GameObject hotel;
        
        void Awake()
        {
            instance = this;
        }

        void Start ()
        {
            currentPlayer = 0;
            pieces = new Dictionary<GameObject, bool>();
            pieces.Add(cannon, false);
            pieces.Add(car, false);
            pieces.Add(iron, false);
            pieces.Add(shoe, false);
            pieces.Add(locomotive, false);
            pieces.Add(thimble, false);
            pieces.Add(topHat, false);
            
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
            if (board.locations[player.location] is Property) {

            } 
            else if (board.locations[player.location] is Railroad) {

            }
            else if (board.locations[player.location] is Utility) {

            }
            else if (board.locations[player.location] is Tax) {
                
            }
            else if (board.locations[player.location] is Location) {
                switch(board.locations[player.location].name) {
                    case "GO":
                        break;
                    case "Jail":
                        break;
                    case "Free Parking":
                        break;
                    case "Go To Jail":
                        break;
                    case "Chance":
                        break;
                    case "Community Chest":
                        break;
                }
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

        public void PayMoney(Player payer, Player recipient, int amount)
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
}