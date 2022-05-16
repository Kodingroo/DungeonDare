using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class HandVisual : MonoBehaviour
{
    // PUBLIC FIELDS
    public bool TakeCardsOpenly = true;
    public SameDistanceChildren slots;
    private BoxCollider col;
    private bool _cursorOverThisCard = false;   
    public bool CursorOverThisCard
    {
        get{ return _cursorOverThisCard; }
    }     

    [Header("Transform References")]
    public Transform DrawPreviewSpot;
    public Transform DeckTransform;

    void Awake()
    {
        col = GetComponent<BoxCollider>();
    }

    // CURSOR/MOUSE DETECTION
    void Update()
    {
        RaycastHit[] hits;
        // raycst to mousePosition and store all the hits in the array
        hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition), 30f);

        bool passedThroughTableCollider = false;
        foreach (RaycastHit h in hits)
        {
            // check if the collider that we hit is the collider on this GameObject
            if (h.collider == col)
            {
                passedThroughTableCollider = true;
            }
        }
        _cursorOverThisCard = passedThroughTableCollider;
    }    

    // PRIVATE : a list of all card visual representations as GameObjects
    public List<GameObject> CardsInHand = new List<GameObject>();

    // ADDING OR REMOVING CARDS FROM HAND

    // add a new card GameObject to hand
    public void AddCard(GameObject card)
    {
        // we allways insert a new card as 0th element in CardsInHand List 
        CardsInHand.Insert(0, card);

        // parent this card to our Slots GameObject
        card.transform.SetParent(slots.transform);

        // re-calculate the position of the hand
        PlaceCardsOnNewSlots();
        UpdatePlacementOfSlots();
    }

    // remove a card GameObject from hand
    public void RemoveCard(GameObject card)
    {
        // remove a card from the list
        CardsInHand.Remove(card);

        // re-calculate the position of the hand
        PlaceCardsOnNewSlots();
        UpdatePlacementOfSlots();
    }

    // remove card with a given index from hand
    public void RemoveCardAtIndex(int index)
    {
        CardsInHand.RemoveAt(index);
        // re-calculate the position of the hand
        PlaceCardsOnNewSlots();
        UpdatePlacementOfSlots();
    }

    // get a card GameObject with a given index in hand
    public GameObject GetCardAtIndex(int index)
    {
        return CardsInHand[index];
    }
        
    // MANAGING CARDS AND SLOTS

    // move Slots GameObject according to the number of cards in hand
    void UpdatePlacementOfSlots()
    {
        float posX;
        if (CardsInHand.Count > 0)
            posX = (slots.Children[0].transform.localPosition.x - slots.Children[CardsInHand.Count - 1].transform.localPosition.x) / 2f;
        else
            posX = 0f;

        // tween Slots GameObject to new position in 0.3 seconds
        slots.gameObject.transform.DOLocalMoveX(posX, 0.3f);  
    }

    // shift all cards to their new slots
    void PlaceCardsOnNewSlots()
    {
        foreach (GameObject g in CardsInHand)
        {
            // tween this card to a new Slot
            g.transform.DOLocalMoveX(slots.Children[CardsInHand.IndexOf(g)].transform.localPosition.x, 0.3f);

            // apply correct sorting order and HandSlot value for later 
            WhereIsTheCardOrEnemy w = g.GetComponent<WhereIsTheCardOrEnemy>();
            w.Slot = CardsInHand.IndexOf(g);
            w.SetHandSortingOrder();
        }
    }

    // CARD DRAW METHODS

    // creates a card and returns a new card as a GameObject
    GameObject CreateACardAtPosition(EnemyCardAsset c, Vector3 position, Vector3 eulerAngles)
    {
        // Instantiate a card depending on its type
        GameObject card;
        // this card is a Enemy card
        card = GameObject.Instantiate(GlobalSettings.Instance.EnemyCardPrefab, position, Quaternion.Euler(eulerAngles)) as GameObject;
        // apply the look of the card based on the info from EnemyCardAsset
        OneCardManager manager = card.GetComponent<OneCardManager>();
        manager.cardAsset = c;
        manager.ReadCardFromAsset();

        return card;
    }

    // gives player a new card from a given position
    public void GivePlayerACard(EnemyCardAsset c, int UniqueID, bool fast = false)
    {
        GameObject card;
        card = CreateACardAtPosition(c, DeckTransform.position, new Vector3(0f, -179f, 0f));
        // pass this card to HandVisual class
        AddCard(card);

        // Bring card to front while it travels from draw spot to hand
        WhereIsTheCardOrEnemy w = card.GetComponent<WhereIsTheCardOrEnemy>();
        w.BringToFront();
        w.Slot = 0; 
        w.VisualState = VisualStates.Transition;

        // pass a unique ID to this card.
        IDHolder id = card.AddComponent<IDHolder>();
        id.UniqueID = UniqueID;

        Debug.Log("TurnManager.Instance.IsPlayerAI: "+TurnManager.Instance.IsPlayerAI);
        TakeCardsOpenly = !TurnManager.Instance.IsPlayerAI;
        Debug.Log("TakeCardsOpenly: "+TakeCardsOpenly);

        // move card to the hand;
        Sequence s = DOTween.Sequence();
        if (!fast)
        {
            // Debug.Log ("Not fast!!!");
            s.Append(card.transform.DOMove(DrawPreviewSpot.position, GlobalSettings.Instance.CardTransitionTime));
            if (TakeCardsOpenly)
                s.Insert(0f, card.transform.DORotate(Vector3.zero, GlobalSettings.Instance.CardTransitionTime)); 
            //else 
                //s.Insert(0f, card.transform.DORotate(new Vector3(0f, -179f, 0f), GlobalSettings.Instance.CardTransitionTime)); 
            s.AppendInterval(GlobalSettings.Instance.CardPreviewTime);
            // displace the card so that we can select it in the scene easier.
            s.Append(card.transform.DOLocalMove(slots.Children[0].transform.localPosition, GlobalSettings.Instance.CardTransitionTime));
        }
        else
        {
            // displace the card so that we can select it in the scene easier.
            s.Append(card.transform.DOLocalMove(slots.Children[0].transform.localPosition, GlobalSettings.Instance.CardTransitionTimeFast));
            if (TakeCardsOpenly)    
                s.Insert(0f,card.transform.DORotate(Vector3.zero, GlobalSettings.Instance.CardTransitionTimeFast)); 
        }

        s.OnComplete(()=>ChangeLastCardStatusToInHand(card, w));
    }

    // this method will be called when the card arrived to hand 
    void ChangeLastCardStatusToInHand(GameObject card, WhereIsTheCardOrEnemy w)
    {
        //Debug.Log("Changing state to Hand for card: " + card.gameObject.name);
        // set correct sorting order
        w.SetHandSortingOrder();
        // end command execution for DrawACArdCommand
        Command.CommandExecutionComplete();
    }
}
