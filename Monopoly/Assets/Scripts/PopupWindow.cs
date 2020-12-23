using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

namespace Monopoly
{
    public class PopupWindow : MonoBehaviour
    {

        [SerializeField]
        private Text windowText;

        public void DisplayOwnedProperties()
        {
            string text = "<b>My Properties</b>";

            bool[] ownedProperties = (bool[])PhotonNetwork.LocalPlayer.CustomProperties["OwnedProperties"];
            for (int i = 0; i < 40; i++) {
                if (ownedProperties[i] == true) {
                    text = text + "\n" + GameManager.instance.board.locations[i].name;
                }
            }
            windowText.text = text;
            this.gameObject.SetActive(true);
        }
        public void DisplayPropertyInfo(int propertyNum)
        {
            if (propertyNum == -1) {
                string text = "<b>Reading Railroad</b>";
                Railroad readingRailroad = (Railroad)GameManager.instance.board.locations[5];
                if (readingRailroad.owner == null) {
                    text = text + " - Owner: None";
                } else text = text + " - Owner: " + readingRailroad.owner.NickName;

                Railroad pennsylvaniaRailroad = (Railroad)GameManager.instance.board.locations[15];
                text = text + "\n<b>Pennsylvania Railroad</b>";
                if (pennsylvaniaRailroad.owner == null) {
                    text = text + " - Owner: None";
                } else text = text + " - Owner: " + pennsylvaniaRailroad.owner.NickName;

                Railroad bAndORailroad = (Railroad)GameManager.instance.board.locations[25];
                text = text + "\n<b>B & O Railroad</b>";
                if (bAndORailroad.owner == null) {
                    text = text + " - Owner: None";
                } else text = text + " - Owner: " + bAndORailroad.owner.NickName;

                Railroad shortLine = (Railroad)GameManager.instance.board.locations[35];
                text = text + "\n<b>Short Line</b>";
                if (shortLine.owner == null) {
                    text = text + " - Owner: None";
                } else text = text + " - Owner: " + shortLine.owner.NickName;

                text = text + "\n\nPrice: $200\n\nRent:\n1 owned: $25\n2 owned: $50\n3 owned: $100\n4 owned: $200";
                windowText.text = text;
            }

            else if (propertyNum == -2) {
                string text = "<b>Electric Company</b>\n";
                Utility electricCompany = (Utility)GameManager.instance.board.locations[12];
                if (electricCompany.owner == null) {
                    text = text + "Owner: None";
                } else text = text + "Owner: " + electricCompany.owner.NickName;

                text = text + "\n\n<b>Water Works</b>\n";
                Utility waterWorks = (Utility)GameManager.instance.board.locations[28];
                if (waterWorks.owner == null) {
                    text = text + "Owner: None";
                } else text = text + "Owner: " + waterWorks.owner.NickName;

                text = text + "\n\nPrice: $150";
                text = text + "\n\nRent:\n1 utility owned: $4 x diceroll";
                text = text + "\n2 utility owned: $10 x diceroll";
                windowText.text = text;
            }

            else if (GameManager.instance.board.locations[propertyNum] is Property) {
                Property property = (Property)GameManager.instance.board.locations[propertyNum];
                string text = "<b>" + property.name + "</b>\n";
                if (property.owner == null) {
                    text = text + "Owner: None";
                } else text = text + "Owner: " + property.owner.NickName;

                text = text + "\nPrice: $" + property.price;
                text = text + "\nHouse price: $" + property.housePrice;
                text = text + "\nNumber of houses built: " + property.numHouses;
                int baseRent = property.rent;
                switch (property.numHouses) {
                    case 1:
                        baseRent = property.rent / 5;
                        break;
                    case 2:
                        baseRent = property.rent / 15;
                        break;
                    case 3:
                        baseRent = property.rent / 45;
                        break;
                    case 4:
                        baseRent = (property.rent - 200) / 45;
                        break;
                    case 5:
                        baseRent = (property.rent - 400) / 45;
                        break;
                }

                text = text + "\n\nRent:\n0 houses: $" + baseRent;
                text = text + "\n1 house: $" + (baseRent * 5);
                text = text + "\n2 houses: $" + (baseRent * 15);
                text = text + "\n3 houses: $" + (baseRent * 45);
                text = text + "\n4 houses: $" + ((baseRent * 45)+200);
                text = text + "\n5 houses (hotel): $" + ((baseRent * 45)+400);
                windowText.text = text;
            }
            this.gameObject.SetActive(true);
        }
    }
}