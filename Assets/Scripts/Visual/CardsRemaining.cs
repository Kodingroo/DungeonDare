using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CardsRemaining : MonoBehaviour
{
    public Text CardCounterText;

    void Update()
    {
        CardCounterText.text = $"{DeckLogic.Instance.cards.Count.ToString()}/{DeckLogic.Instance.DeckSize.ToString()}";        
    }
}
