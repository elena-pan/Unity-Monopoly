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
        private Button sellPropertyButton;
        [SerializeField]
        private Dropdown propertyDropdown;

        public void updateDropdownOptions()
        {
            propertyDropdown.ClearOptions();
            List<string> options = new List<string>();
            options.Add("2");

            propertyDropdown.AddOptions(options);
            propertyDropdown.value = 0;
            propertyDropdown.RefreshShownValue();
        }
        public void OnClickDropdownOption(Dropdown change) {
            Debug.Log(change);
        }

    }
}