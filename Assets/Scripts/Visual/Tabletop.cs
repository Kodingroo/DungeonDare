using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tabletop : MonoBehaviour
{
    public DeckLogic deck;
    public HandLogic hand;
    public CharacterTokenListLogic characterTokenlist;
    public TokenPoolLogic tokenPool;
    public TokenLogic tokenLogic;

    public HandVisual handVisual;
    public DeckVisual deckVisual;   
    public DungeonVisual dungeonVisual;
    public TokenVisual tokenVisual;

    public static Tabletop Instance; 

    public bool ControlsON = true;

    void Awake() {
        Instance = this;
    }    


    // draw a single card from the deck
    public void DrawACard(bool fast = false)
    {
        HoverPreview.PreviewsAllowed = false;
        if (deck.cards.Count > 0)
        {
            if (hand.CardsInHandLogic.Count < handVisual.slots.Children.Length)
            {
                // 1) logic: add card to hand
                CardLogic newCard = new CardLogic(deck.cards[0]);
                hand.CardsInHandLogic.Insert(0, newCard);
                // 2) logic: remove the card from the deck
                deck.cards.RemoveAt(0);
                // 2) create a command
                new DrawACardCommand(hand.CardsInHandLogic[0], this, fast).AddToQueue();
            }
        }
        else
        {
            // there are no cards in the deck, take fatigue damage.
        }
        // HoverPreview.PreviewsAllowed = true;
    }

    public void CreateAToken()
    {
        foreach (TokenAsset ta in characterTokenlist.tokens)
        {     
            TokenLogic newToken = new TokenLogic(ta);
            tokenPool.TokensInPool.Insert(0, newToken);
        }
    }

    public void PlayAnEnemyFromHand(int UniqueID, int tablePos)
    {
        PlayAnEnemyFromHand(CardLogic.CardsCreatedThisGame[UniqueID], tablePos);
    }

    // 2nd overload - by logic units
    public void PlayAnEnemyFromHand(CardLogic playedCard, int tablePos)
    {
        Debug.Log("Card played: "+playedCard.ca.Attack);
        // create a new enemy object and add it to Table
        EnemyLogic newEnemy = new EnemyLogic(TurnManager.Instance.whoseTurn, playedCard.ca);
        // dungeonVisual.EnemiesOnTable.Insert(tablePos, newEnemy);
        // 
        new PlayAnEnemyCommand(playedCard, TurnManager.Instance.whoseTurn, tablePos, newEnemy.UniqueEnemyID).AddToQueue();
        // remove this card from hand
        hand.CardsInHandLogic.Remove(playedCard);
    }    
}
