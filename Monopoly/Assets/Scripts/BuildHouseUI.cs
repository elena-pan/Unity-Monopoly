using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;

namespace Monopoly
{
    public class BuildHouseUI : MonoBehaviour
    {
        [SerializeField]
        private Dropdown propertyDropdown;

        private List<int> validProperties;
        private int currentDropdown = 0;

        public void BuildHouseClicked()
        {
            GameManager.instance.BuildHouse(validProperties[currentDropdown]);
            this.gameObject.SetActive(false);
        }

        public void UpdateDropdownOptions()
        {
            propertyDropdown.ClearOptions();
            List<string> options = new List<string>();
            validProperties = new List<int>();

            bool[] ownedProp = (bool[])PhotonNetwork.LocalPlayer.CustomProperties["OwnedProperties"];
            foreach (int[] group in GameManager.instance.propertyGroups) {
                // Check each property group and see if player owns the entire group
                int numOwned = 0;
                for (int i = 0; i < group.Length; i++) {
                    if (ownedProp[group[i]] == true) {
                        numOwned++;
                    }
                }

                if (numOwned == group.Length) {
                    // Get number of houses built for each property
                    int housePrice = 0; // House price is same for all properties in group
                    List<int> numHouses = new List<int>();
                    for (int i = 0; i < group.Length; i++) {
                        Property property = (Property)GameManager.instance.board.locations[group[i]];
                        numHouses.Add(property.numHouses);
                        housePrice = property.housePrice;
                    }

                    if (housePrice > PlayerManager.balance) { // Check if we can afford to build a house
                        continue;
                    }

                    // Compare each to smallest number of houses built
                    int minNum = numHouses.Min();
                    if (minNum == 5) { // Cannot build house if we already have 5
                        continue;
                    }
                    for (int i = 0; i < group.Length; i++) {
                        if (numHouses[i] == minNum) {
                            string text = GameManager.instance.board.locations[group[i]].name + " - $" + housePrice;
                            options.Add(text);
                            validProperties.Add(group[i]);
                        }
                    }
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