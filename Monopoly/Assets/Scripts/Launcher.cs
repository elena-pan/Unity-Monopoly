﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

namespace Monopoly
{
    public class Launcher : MonoBehaviourPunCallbacks
    {

        /// This client's version number. Users are separated from each other by gameVersion
        string gameVersion = "1";
        private int numPlayers = 4;

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
        [SerializeField]
        private Dropdown numPlayersDropdown;

        [SerializeField]
        private Button startGameButton;
        [SerializeField]
        private Button createGameButton;
        [SerializeField]
        private Button joinGameButton;

        public Player[] playerList;

        void Awake()
        {
            // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        void Start()
        {
            SwitchPanels(connectingPanel);
            Connect();
            numPlayersDropdown.ClearOptions();
            List<string> options = new List<string>();
            options.Add("2");
            options.Add("3");
            options.Add("4");
            options.Add("5");
            options.Add("6");
            options.Add("7");
            options.Add("8");

            numPlayersDropdown.AddOptions(options);
            numPlayersDropdown.value = numPlayers-2;
            numPlayersDropdown.RefreshShownValue();
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
            PhotonNetwork.CreateRoom(createGameNameInput.text, new RoomOptions { MaxPlayers = (byte)numPlayers });
        }

        public void StartGame()
        {
            if (PhotonNetwork.IsMasterClient)
            {
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
                if (PhotonNetwork.CurrentRoom.PlayerCount == numPlayers) {
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
            numPlayersText.text = "("+PhotonNetwork.CurrentRoom.PlayerCount.ToString()+"/"+numPlayers.ToString()+")";
            hostText.text = "";
            joinedPlayersText.text = "";
            foreach(Player player in playerList) {
                joinedPlayersText.text = joinedPlayersText.text + player.NickName + "\n";
                if (player.IsMasterClient) {
                    hostText.text = player.NickName;
                }
            }
        }

        private void OnClickDropdownOption(Dropdown change) {
            numPlayers = change.value+2;
        }

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
            UpdateTexts();
        }

        public override void OnPlayerEnteredRoom(Player other)
        {
            playerList = PhotonNetwork.PlayerList;
            UpdateTexts();
        }


        public override void OnPlayerLeftRoom(Player other)
        {
            playerList = PhotonNetwork.PlayerList;
            UpdateTexts();
        }

        public override void OnConnectedToMaster()
        {
            SwitchPanels(startingPanel);
        }

    }
}