using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceResult : MonoBehaviour
{
    public int diceNum;
	void OnTriggerStay(Collider col)
	{
        switch (col.gameObject.name) {
        case "Side 1":
            diceNum = 6;
            break;
        case "Side 2":
            diceNum = 5;
            break;
        case "Side 3":
            diceNum = 4;
            break;
        case "Side 4":
            diceNum = 3;
            break;
        case "Side 5":
            diceNum = 2;
            break;
        case "Side 6":
            diceNum = 1;
            break;
        }
	}
}