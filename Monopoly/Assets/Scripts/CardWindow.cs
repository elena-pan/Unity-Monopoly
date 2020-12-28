using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Monopoly
{
    public class CardWindow : MonoBehaviour
    {
        [SerializeField]
        private Text titleText;
        [SerializeField]
        private Text bodyText;
        public void DisplayCard(string cardType, string body) {
            titleText.text = cardType;
            bodyText.text = body;
            this.gameObject.SetActive(true);
            StartCoroutine(CloseWindow());
        }

        // Wait before closing window
        private IEnumerator CloseWindow() {
            yield return new WaitForSeconds(5);
            this.gameObject.SetActive(false);
        }
    }
}