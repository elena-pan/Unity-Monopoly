using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;

namespace Monopoly
{
    public class SellPropertyUI : MonoBehaviour
    {
        [SerializeField]
        private Dropdown propertyDropdown;

        private List<int> ownedProperties;
        private int currentDropdown = 0;

        public void SellPropertyClicked()
        {
            GameManager.instance.SellProperty(ownedProperties[currentDropdown]);
            this.gameObject.SetActive(false);
        }

        public void SellPropertyNoMoneyClicked()
        {
            SellPropertyClicked();
            GameManager.instance.SoldPropertyNoMoney();
        }
        public void UpdateDropdownOptions()
        {
            propertyDropdown.ClearOptions();
            List<string> options = new List<string>();
            ownedProperties = new List<int>();

            bool[] ownedProp = (bool[])PhotonNetwork.LocalPlayer.CustomProperties["OwnedProperties"];
            for (int i = 0; i < 40; i++) {
                if (ownedProp[i] == true) {
                    string dropDownText = GameManager.instance.board.locations[i].name;
                    int price = 0;
                    // Calculate sell price
                    if (GameManager.instance.board.locations[i] is Property) {
                        Property property = (Property)GameManager.instance.board.locations[i];
                        price = (int)(0.5*property.price);
                        price += (int)(0.5*property.housePrice*property.numHouses);
                    }
                    else if (GameManager.instance.board.locations[i] is Utility) {
                        Utility property = (Utility)GameManager.instance.board.locations[i];
                        price = (int)(0.5*property.price);
                    }
                    else if (GameManager.instance.board.locations[i] is Railroad) {
                        Railroad property = (Railroad)GameManager.instance.board.locations[i];
                        price = (int)(0.5*property.price);
                    }

                    dropDownText = dropDownText + " - $" + price.ToString();
                    options.Add(dropDownText);
                    ownedProperties.Add(i);
                }
            }

            propertyDropdown.AddOptions(options);
            propertyDropdown.value = 0;
            propertyDropdown.RefreshShownValue();
            currentDropdown = 0;
        }
        public void OnClickDropdownOption(Dropdown dropdown) {
            currentDropdown = dropdown.value;
        }

    }
}