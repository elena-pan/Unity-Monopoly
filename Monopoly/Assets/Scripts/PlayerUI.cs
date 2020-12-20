using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

namespace Monopoly
{
    public class PlayerUI : MonoBehaviour, IOnEventCallback
    {
        public const byte SendNewActivityLineCode = 1;

        [SerializeField]
        private Text activityPanelLine1;
        [SerializeField]
        private Text activityPanelLine2;
        [SerializeField]
        private Text activityPanelLine3;
        [SerializeField]
        private Text activityPanelLine4;
        [SerializeField]
        private Text statsBody;
        private PlayerManager target;
        void Awake()
        {
            this.transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
        }

        void Update()
        {
            // Update player stats
            if (statsBody != null) {
                string temp = "Balance: " + PlayerManager.balance.ToString() + "\nGamepiece: " + PlayerManager.LocalPlayerInstance.name;
                if ((bool)PhotonNetwork.LocalPlayer.CustomProperties["Bankrupt"] == false) {
                    temp = temp + "\nBankrupt: " + "No";
                } else {
                    temp = temp + "\nBankrupt: " + "Yes";
                }
                statsBody.text = temp;
            }
        }

        public void AddActivityText(string text) {
            activityPanelLine4.text = activityPanelLine3.text;
            activityPanelLine3.text = activityPanelLine2.text;
            activityPanelLine2.text = activityPanelLine1.text;
            activityPanelLine1.text = text;
        }

        public void SetTarget(PlayerManager _target)
        {
            if (_target == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> PlayMakerManager target for PlayerUI.SetTarget.", this);
                return;
            }
            // Cache references for efficiency
            target = _target;
            //playerNameText.text = target.photonView.Owner.NickName;
        }
        public void SendNewActivityLine(string content)
        {
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // Set Receivers to All to receive this event on the local client as well
            PhotonNetwork.RaiseEvent(SendNewActivityLineCode, content, raiseEventOptions, SendOptions.SendReliable);
        }
        public void OnEvent(EventData photonEvent)
        {
            byte eventCode = photonEvent.Code;
            if (eventCode == SendNewActivityLineCode)
            {
                string data = (string)photonEvent.CustomData;
                AddActivityText(data);
            }
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