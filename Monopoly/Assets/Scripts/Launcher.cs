using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

namespace Monopoly
{
    public enum GamePieces
    {
        cannon, car, iron, locomotive, thimble, topHat, wheelbarrow
    }
    public class Launcher : MonoBehaviourPunCallbacks, IOnEventCallback
    {

        /// This client's version number. Users are separated from each other by gameVersion
        string gameVersion = "1";
        private int maxPlayers = 7; // Only have 7 gamepieces
        public const byte PiecesUpdateCode = 5;
        public const byte PieceAssignmentCode = 6;

        [SerializeField]
        private GameObject startingPanel;
        [SerializeField]
        private GameObject createGamePanel;
        [SerializeField]
        private GameObject joinGamePanel;
        [SerializeField]
        private GameObject connectingPanel;
        [SerializeField]
        private GameObject lobbyPanel;
        [SerializeField]
        private GameObject textPanel;
        [SerializeField]
        private Text textPanelText;

        [SerializeField]
        private Text joinedPlayersText;
        [SerializeField]
        private Text numPlayersText;
        [SerializeField]
        private Text hostText;

        [SerializeField]
        private InputField joinGameNameInput;
        [SerializeField]
        private InputField createGameNameInput;
        /*[SerializeField]
        private Dropdown numPlayersDropdown;*/

        [SerializeField]
        private Button startGameButton;
        [SerializeField]
        private Button createGameButton;
        [SerializeField]
        private Button joinGameButton;

        public Player[] playerList;
        public List<int> pieces;

        void Awake()
        {
            // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        void Start()
        {
            SwitchPanels(connectingPanel);
            Connect();

            /*
            numPlayersDropdown.ClearOptions();
            List<string> options = new List<string>();
            options.Add("2");
            options.Add("3");
            options.Add("4");
            options.Add("5");
            options.Add("6");
            options.Add("7");

            numPlayersDropdown.AddOptions(options);
            numPlayersDropdown.value = numPlayers-2;
            numPlayersDropdown.RefreshShownValue();*/
        }

        void Update()
        {
            ToggleButtons();
        }

        public void SwitchPanels(GameObject panel)
        {
            // Set all panels as invisible and reveal selected panel
            startingPanel.SetActive(false);
            createGamePanel.SetActive(false);
            joinGamePanel.SetActive(false);
            lobbyPanel.SetActive(false);
            textPanel.SetActive(false);
            connectingPanel.SetActive(false);

            panel.SetActive(true);
        }

        public void Connect()
        {
            if (!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }
            else {
                SwitchPanels(startingPanel);
            }
        }

        public void JoinGame()
        {
            Connect();
            SwitchPanels(connectingPanel);
            PhotonNetwork.JoinRoom(joinGameNameInput.text);
        }

        public void CreateGame()
        {
            Connect();
            SwitchPanels(connectingPanel);
            PhotonNetwork.CreateRoom(createGameNameInput.text, new RoomOptions { MaxPlayers = (byte)maxPlayers });
        }

        public void StartGame()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false; // Close room so nobody can join
                SwitchPanels(connectingPanel);
                PhotonNetwork.LoadLevel("Game");
            }
        }

        private void ToggleButtons()
        {
            if (string.IsNullOrEmpty(joinGameNameInput.text) || string.IsNullOrEmpty(PhotonNetwork.NickName)) {
                joinGameButton.interactable = false;
            } else {
                joinGameButton.interactable = true;
            }

            if (string.IsNullOrEmpty(createGameNameInput.text) || string.IsNullOrEmpty(PhotonNetwork.NickName)) {
                createGameButton.interactable = false;
            } else {
                createGameButton.interactable = true;
            }

            if (PhotonNetwork.IsMasterClient) {
                startGameButton.gameObject.SetActive(true);
                if (PhotonNetwork.CurrentRoom.PlayerCount >= 2) {
                    startGameButton.interactable = true;
                } else {
                    startGameButton.interactable = false;
                }
            } else {
                startGameButton.gameObject.SetActive(false);
            }
        }

        private void UpdateTexts()
        {
            numPlayersText.text = "("+PhotonNetwork.CurrentRoom.PlayerCount.ToString()+"/7)";
            hostText.text = "";
            joinedPlayersText.text = "";
            foreach(Player player in playerList) {
                joinedPlayersText.text = joinedPlayersText.text + player.NickName + "\n";
                if (player.IsMasterClient) {
                    hostText.text = player.NickName;
                }
            }
        }

        /*
        private void OnClickDropdownOption(Dropdown change) {
            numPlayers = change.value+2;
        }*/

        public override void OnDisconnected(DisconnectCause cause)
        {
            SwitchPanels(textPanel);
            textPanelText.text = cause.ToString();
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            SwitchPanels(textPanel);
            textPanelText.text = message;
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            SwitchPanels(textPanel);
            textPanelText.text = message;
        }

        public override void OnJoinedRoom()
        {
            SwitchPanels(lobbyPanel);
            playerList = PhotonNetwork.PlayerList;

            // Set custom player properties
            ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
            hash.Add("Bankrupt", false);
            hash.Add("OwnedProperties", new bool[40]);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

            if (PhotonNetwork.IsMasterClient) {
                pieces = new List<int>();
                //pieces.Add(0); // Comment out since master client will take this piece
                pieces.Add(1);
                pieces.Add(2);
                pieces.Add(3);
                pieces.Add(4);
                pieces.Add(5);
                pieces.Add(6);

                ExitGames.Client.Photon.Hashtable hash2 = new ExitGames.Client.Photon.Hashtable();
                hash2.Add("Gamepiece", 0);
                PhotonNetwork.LocalPlayer.SetCustomProperties(hash2);
            }
            
            UpdateTexts();
        }

        public override void OnPlayerEnteredRoom(Player other)
        {
            if (PhotonNetwork.IsMasterClient) {
                int[] target = new int[] {other.ActorNumber};
                SendPhotonEvent(PieceAssignmentCode, pieces[0], target);
                pieces.RemoveAt(0);
                SendPhotonEvent(PiecesUpdateCode, pieces.ToArray());
            }
            playerList = PhotonNetwork.PlayerList;
            UpdateTexts();
        }


        public override void OnPlayerLeftRoom(Player other)
        {
            if (PhotonNetwork.IsMasterClient) {
                pieces.Add((int)other.CustomProperties["Gamepiece"]);
                SendPhotonEvent(PiecesUpdateCode, pieces);
            }
            playerList = PhotonNetwork.PlayerList;
            UpdateTexts();
        }

        public override void OnConnectedToMaster()
        {
            SwitchPanels(startingPanel);
        }

        public void OnEvent(EventData photonEvent)
        {
            byte eventCode = photonEvent.Code;
            switch (eventCode) {
                case PiecesUpdateCode:
                    int[] temp = (int[])photonEvent.CustomData;
                    pieces = temp.ToList();
                    break;
                case PieceAssignmentCode:
                    ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
                    hash.Add("Gamepiece", (int)photonEvent.CustomData);
                    PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
                    break;
            }
        }

        private void SendPhotonEvent(byte eventCode, object content, int[] targetActors = null)
        {
            RaiseEventOptions raiseEventOptions = raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others }; // Set Receivers to All to receive this event on the local client as well
            if (targetActors != null && targetActors.Length != 0) {
                raiseEventOptions = new RaiseEventOptions { TargetActors = targetActors }; // Send to specific clients
            }
            PhotonNetwork.RaiseEvent(eventCode, content, raiseEventOptions, SendOptions.SendReliable);
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