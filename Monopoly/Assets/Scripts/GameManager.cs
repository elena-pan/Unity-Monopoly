using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

namespace Monopoly
{
    public class GameManager : MonoBehaviourPunCallbacks, IOnEventCallback
    {
        public static GameManager instance;

        public const byte SendNewActivityLineCode = 1;
        public const byte PlayerTurnCode = 2;
        public const byte ReceiveMoneyCode = 3;
        public const byte PropertyChangeCode = 4;
    
        public Dice dice;
        public Board board;

        public int currentPlayer;
        public Player[] players;

        public GameObject cannon;
        public GameObject car;
        public GameObject iron;
        public GameObject locomotive;
        public GameObject thimble; 
        public GameObject topHat;
        public GameObject wheelbarrow;
        public GameObject[] pieces;

        public GameObject house;
        public GameObject hotel;

        public GameObject PlayerUi;
        
        void Awake()
        {
            instance = this;
        }

        void Start ()
        {
            pieces = new GameObject[] {cannon, car, iron, locomotive, thimble, topHat, wheelbarrow};
            currentPlayer = 0;
            players = PhotonNetwork.PlayerList;

            DisableButton("buyPropertyButton");
            DisableButton("buildHouseButton");
            DisableButton("sellHouseButton");
            DisableButton("endTurnButton");

            if (PlayerManager.LocalPlayerInstance == null)
            {
                // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                // int pieceNum = (int)PhotonNetwork.LocalPlayer.CustomProperties["Gamepiece"];
                // PhotonNetwork.Instantiate(pieces[pieceNum].name, board.locations[0].gridPoint, Quaternion.identity, 0);
            }

            SetupBoard();

            if (PhotonNetwork.IsMasterClient) {
                NextPlayer();
            }
        }

        private void SetupBoard()
        {
            board.SetUpLocations();
            board.SetUpCards();
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
            MoveToLocation(location);
            
            LandedOn(steps);
        }

        public void MoveToLocation(int location)
        {
            board.MovePiece(PlayerManager.LocalPlayerInstance, location);
            PlayerManager.location = location;
        }

        public void LandedOn(int? diceRoll = null)
        {
            string landedOnText = "landed on " + board.locations[PlayerManager.location].name;
            SendActivityMessage(landedOnText, currentPlayer);

            if (board.locations[PlayerManager.location] is Property) {
                Property currentLocation = (Property)board.locations[PlayerManager.location];
                // Unowned
                if (currentLocation.owner == null) {
                    if (PlayerManager.balance >= currentLocation.price) {
                        EnableButton("buyPropertyButton");
                    }
                } else { // Owned
                    if (currentLocation.owner != players[currentPlayer]) { // By other
                        bool paid = PayMoney(currentLocation.rent, currentLocation.owner);
                        if (paid) {
                            string text = "paid $" + currentLocation.rent + " in rent to " + currentLocation.owner.NickName;
                            SendActivityMessage(text, currentPlayer);
                        }
                    } else { // By self
                        if (PlayerManager.balance >= currentLocation.price) {
                            if (currentLocation.numHouses < 5) {
                                EnableButton("buildHouseButton");
                            }
                        }
                    }
                }
            } 
            else if (board.locations[PlayerManager.location] is Railroad) {
                Railroad currentLocation = (Railroad)board.locations[PlayerManager.location];
                if (currentLocation.owner == null) {
                    if (PlayerManager.balance >= currentLocation.price) {
                        EnableButton("buyPropertyButton");
                    }
                } else { // Owned
                    if (currentLocation.owner != players[currentPlayer]) { // By other
                        int rent = currentLocation.GetRent();
                        bool paid = PayMoney(rent, currentLocation.owner);
                        if (paid) {
                            string text = "paid $" + rent + " in rent to " + currentLocation.owner.NickName;
                            SendActivityMessage(text, currentPlayer);
                        }
                    }
                }
            }
            else if (board.locations[PlayerManager.location] is Utility) {
                Utility currentLocation = (Utility)board.locations[PlayerManager.location];
                if (currentLocation.owner == null) {
                    if (PlayerManager.balance >= currentLocation.price) {
                        EnableButton("buyPropertyButton");
                    }
                } else { // Owned
                    if (currentLocation.owner != players[currentPlayer]) { // By other
                        int rent = currentLocation.GetRent((int)diceRoll);
                        bool paid = PayMoney(rent, currentLocation.owner);
                        if (paid) {
                            string text = "paid $" + rent + " in rent to " + currentLocation.owner.NickName;
                            SendActivityMessage(text, currentPlayer);
                        }
                    }
                }
            }
            else if (board.locations[PlayerManager.location] is Tax) {
                Tax currentLocation = (Tax)board.locations[PlayerManager.location];
                PayMoney(currentLocation.tax);
            }
            else if (board.locations[PlayerManager.location] is Location) {
                switch(board.locations[PlayerManager.location].name) {
                    case "GO":
                        ReceiveMoney(200);
                        break;
                    case "Jail":
                        break;
                    case "Free Parking":
                        break;
                    case "Go To Jail":
                        bool paid = PayMoney(50);
                        if (paid) {
                            MoveToLocation(10);
                            LandedOn();

                            int amountPaid = 50;
                            string paidMessage = "paid $" + amountPaid.ToString();
                            SendActivityMessage(paidMessage, currentPlayer);
                        }
                        break;
                    case "Chance":
                        Card drawnCard = board.DrawChance();
                        string chanceCardText = "Chance card: " + drawnCard.description;
                        SendActivityMessage(chanceCardText);
                        
                        // If receive money
                        if (drawnCard.amountMoney > 0) {
                            ReceiveMoney(drawnCard.amountMoney);
                        } else if (drawnCard.amountMoney < 0) {
                            bool paid2 = PayMoney(Math.Abs(drawnCard.amountMoney));
                            if (!paid2) return;
                        }

                        if (drawnCard.moveTo != null) {
                            MoveToLocation((int)drawnCard.moveTo);
                            LandedOn();
                        }
                        break;
                    case "Community Chest":
                        Card drawnCard2 = board.DrawCommunityChest();
                        string communityChestCardText = "Community Chest card: " + drawnCard2.description;
                        SendActivityMessage(communityChestCardText);
                        
                        // If receive money
                        if (drawnCard2.amountMoney > 0) {
                            ReceiveMoney(drawnCard2.amountMoney);
                        } else if (drawnCard2.amountMoney < 0) {
                            bool paid3 = PayMoney(Math.Abs(drawnCard2.amountMoney));
                            if (!paid3) return;
                        }

                        if (drawnCard2.moveTo != null) {
                            MoveToLocation((int)drawnCard2.moveTo);
                            LandedOn();
                        }
                        break;
                }
            }
        }

        public void NextPlayer()
        {
            DisableButton("buyPropertyButton");
            DisableButton("buildHouseButton");
            DisableButton("sellHouseButton");
            DisableButton("endTurnButton");

            // Check how many players are bankrupt
            List<Player> notBankrupt = new List<Player>();
            foreach (Player player in players) {
                if ((bool)player.CustomProperties["Bankrupt"] == false) {
                    notBankrupt.Add(player);
                }
            }

            if (notBankrupt.Count == 0) {
                EndGame(null);
                return;
            } else if (notBankrupt.Count == 1) { // Only one person left, they are the winner
                EndGame(notBankrupt[0]);
                return;
            }

            if (currentPlayer == PhotonNetwork.CurrentRoom.PlayerCount-1) {
                currentPlayer = 0;
            } else {
                currentPlayer++;
            }

            if ((bool)players[currentPlayer].CustomProperties["Bankrupt"]) {
                NextPlayer();
                return;
            }

            SendEvent(PlayerTurnCode, currentPlayer);
        }

        public void StartTurn()
        {
            if (players[currentPlayer] == PhotonNetwork.LocalPlayer)
            {
                StartCoroutine(WaitForDiceRoll(diceNum => {
                    EnableButton("endTurnButton");
                    Move(diceNum);
                }));
            }
        }

        public void ReceiveMoney(int amount)
        {
            PlayerManager.balance += amount;
        }

        public bool PayMoney(int amount, Player recipient = null)
        {

            if (PlayerManager.balance < amount) {
                bool paid = NoMoney(amount);
                if (paid == false) return false;
            }

            PlayerManager.balance -= amount;
            if (recipient != null) {
                int[] targetActors = {recipient.ActorNumber};
                SendEvent(ReceiveMoneyCode, amount, targetActors);
            }
            return true;
        }

        public bool NoMoney(int amount)
        {
            return true;
        }

        public void Bankrupt()
        {
            // Set custom player property as bankrupt
            ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
            hash.Add("Bankrupt", true);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
            SendActivityMessage("went bankrupt!", currentPlayer);
        }

        public void EndGame(Player winner)
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

        public void SendActivityMessage(string content, int? playerNum = null)
        {
            object[] data = new object[] {playerNum, content};
            SendEvent(SendNewActivityLineCode, data);
        }

        public void ReceiveActivityMessage(object[] arr)
        {
            string content = (string)arr[1];
            if (arr[0] == null) {
                PlayerUi.SendMessage ("AddActivityText", content, SendMessageOptions.RequireReceiver);
            } else {
                int playerNum = (int)arr[0];
                string text;
                if (players[playerNum] == PhotonNetwork.LocalPlayer) {
                    text = "You " + content;
                } else {
                    text = players[playerNum].NickName + " " + content;
                }
                PlayerUi.SendMessage ("AddActivityText", text, SendMessageOptions.RequireReceiver);
            }
        }

        public void SendEvent(byte eventCode, object content, int[] targetActors = null)
        {
            RaiseEventOptions raiseEventOptions = raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // Set Receivers to All to receive this event on the local client as well
            if (targetActors != null && targetActors.Length != 0) {
                raiseEventOptions = new RaiseEventOptions { TargetActors = targetActors }; // Send to specific clients
            }
            PhotonNetwork.RaiseEvent(eventCode, content, raiseEventOptions, SendOptions.SendReliable);
        }
        public void OnEvent(EventData photonEvent)
        {
            byte eventCode = photonEvent.Code;
            switch (eventCode) {
                case SendNewActivityLineCode:
                    object[] arr = (object[])photonEvent.CustomData;
                    ReceiveActivityMessage(arr);
                    break;
                case PlayerTurnCode:
                    currentPlayer = (int)photonEvent.CustomData;
                    StartTurn();
                    break;
                case ReceiveMoneyCode:
                    int amount = (int)photonEvent.CustomData;
                    PlayerManager.balance += amount;
                    break;
                case PropertyChangeCode:
                    object[] data = (object[])photonEvent.CustomData;
                    int location = (int)data[0];
                    string property = (string)data[1];

                    if (board.locations[location] is Utility) {
                        Utility temp = (Utility)board.locations[location];
                        if (property == "owner") {
                            if (data[2] == null) {
                                temp.owner = null;
                            } else temp.owner = (Player)data[2];
                        }
                        board.locations[location]= temp;
                    } else if (board.locations[location] is Railroad) {
                        Railroad temp = (Railroad)board.locations[location];
                        if (property == "owner") {
                            if (data[2] == null) {
                                temp.owner = null;
                            } else temp.owner = (Player)data[2];
                        }
                        board.locations[location]= temp;
                    } else if (board.locations[location] is Property) {
                        Property temp = (Property)board.locations[location];
                        if (property == "owner") {
                            if (data[2] == null) {
                                temp.owner = null;
                            } else temp.owner = (Player)data[2];
                            board.locations[location]= temp;
                        } else if (property == "numHouses") {
                            temp.numHouses = (int)data[2];
                        }
                        board.locations[location]= temp;
                    }
                    break;
            }
        }

        public void buyProperty()
        {

        }

        public void buildHouse()
        {

        }

        public void sellProperty(int property)
        {

        }

        private void DisableButton(string button)
        {
            PlayerUi.SendMessage ("DisableButton", button, SendMessageOptions.RequireReceiver);
        }

        private void EnableButton(string button)
        {
            PlayerUi.SendMessage ("EnableButton", button, SendMessageOptions.RequireReceiver);
        }

        private void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        private void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }
    }
}