using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DeckLogic : MonoBehaviour {
 
    // SINGLETON
    public static DeckLogic Instance;    
    public int DeckSize;

    public List<EnemyCardAsset> cards = new List<EnemyCardAsset>();

    void Awake()
    {
        Instance = this;        
        cards.Shuffle();
        DeckSize = cards.Count;
    }
	
}
