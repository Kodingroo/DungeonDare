using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public class CardLogic
{
    // an ID of this card
    public int UniqueCardID; 
    // a reference to the card asset that stores all the info about this card
    public EnemyCardAsset ca;

    // STATIC (for managing IDs)
    public static Dictionary<int, CardLogic> CardsCreatedThisGame = new Dictionary<int, CardLogic>();


    // PROPERTIES
    public int ID
    {
        get{ return UniqueCardID; }
    }

    // CONSTRUCTOR
    public CardLogic(EnemyCardAsset ca)
    {
        // set the CardAsset reference
        this.ca = ca;
        // get unique int ID
        UniqueCardID = IDFactory.GetUniqueID();
        // add this card to a dictionary with its ID as a key
        CardsCreatedThisGame.Add(UniqueCardID, this);
    }
}
