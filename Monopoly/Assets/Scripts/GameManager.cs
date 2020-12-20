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
        public Player[] players;

        public GameObject cannon;
        public GameObject car;
        public GameObject iron;
        public GameObject locomotive;
        public GameObject thimble; 
        public GameObject topHat;
        public GameObject wheelbarrow;
        public List<GameObject> pieces;

        public GameObject house;
        public GameObject hotel;

        public GameObject PlayerUi;
        
        void Awake()
        {
            instance = this;
        }

        void Start ()
        {
            currentPlayer = 0;
            players = PhotonNetwork.PlayerList;

            pieces = new List<GameObject>();
            pieces.Add(cannon);
            pieces.Add(car);
            pieces.Add(iron);
            pieces.Add(locomotive);
            pieces.Add(thimble);
            pieces.Add(topHat);
            pieces.Add(wheelbarrow);

            if (PlayerManager.LocalPlayerInstance == null)
            {
                // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                PhotonNetwork.Instantiate(pieces[0].name, board.locations[0].gridPoint, Quaternion.identity, 0);
            }

            SetupBoard();
        }

        private void SetupBoard()
        {
            board.SetUpLocations();
            board.SetUpCards();
            NextPlayer();
        }

        public void Move(int steps)
        {
            int location;
            
            location = PlayerManager.location;
            location+=steps;
            if (location > 39)
            {
                location = location - 39;
            }
            board.MovePiece(PlayerManager.LocalPlayerInstance, location);
            PlayerManager.location = location;
            
            NextPlayer();
        }

        public void LandedOn()
        {
            if (board.locations[PlayerManager.location] is Property) {

            } 
            else if (board.locations[PlayerManager.location] is Railroad) {

            }
            else if (board.locations[PlayerManager.location] is Utility) {

            }
            else if (board.locations[PlayerManager.location] is Tax) {
                
            }
            else if (board.locations[PlayerManager.location] is Location) {
                switch(board.locations[PlayerManager.location].name) {
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
            if (currentPlayer == PhotonNetwork.CurrentRoom.PlayerCount-1) {
                currentPlayer = 0;
            } else {
                currentPlayer++;
            }

            if ((bool)players[currentPlayer].CustomProperties["Bankrupt"]) {
                NextPlayer();
                return;
            }

            if (players[currentPlayer] == PhotonNetwork.LocalPlayer)
            {
                StartCoroutine(WaitForDiceRoll(diceNum => {
                    Move(diceNum);
                }));
            }
        }

        public void ReceiveMoneyFromBank(int amount)
        {
            PlayerManager.balance += amount;
        }

        public void PayMoney(Player recipient, int amount)
        {

            if (PlayerManager.balance < amount) {
                NoMoney(amount);
            }

            PlayerManager.balance -= amount;
            if (recipient != null) {
                //recipient.balance += amount;
            }
        }

        public void NoMoney(int amount)
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

        public override void OnPlayerLeftRoom(Player other)
        {
            players = PhotonNetwork.PlayerList;
            string text = other.NickName + " has left the game.";
            PlayerUi.SendMessage ("AddActivityText", text, SendMessageOptions.RequireReceiver);
        }
    }
}