using UnityEngine;
using UnityEngine.UI;

namespace Monopoly
{
    public class PopupWindow : MonoBehaviour
    {

        [SerializeField]
        private Text windowText;

        public void DisplayPropertyInfo(int propertyNum)
        {
            Debug.Log(GameManager.instance.board.locations);
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

            else if (GameManager.instance.board.locations[propertyNum] is Property) {

            }
            else if (GameManager.instance.board.locations[propertyNum] is Utility) {
                
            }
            this.gameObject.SetActive(true);
        }
    }
}