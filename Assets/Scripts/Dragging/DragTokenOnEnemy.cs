using UnityEngine;
using System.Collections;
using DG.Tweening;

public class DragTokenOnEnemy : DraggingActions {

    private int savedTokenSlot;
    private WhereIsTheCardOrEnemy whereIsToken;
    private VisualStates tempState;
    private OneTokenManager manager;

    public override bool CanDrag
    {
        get
        { 
            // TEST LINE: this is just to test playing creatures before the game is complete 
            // return true;

            // TODO : include full field check
            if (!this.manager.MainToken)
            {
                return base.CanDrag;
            }
            else 
            {
                return false;
            }
        }
    }

    void Awake()
    {
        whereIsToken = GetComponent<WhereIsTheCardOrEnemy>();
        manager = GetComponent<OneTokenManager>();
    }

    public override void OnStartDrag()
    {
        savedTokenSlot = whereIsToken.Slot;
        tempState = whereIsToken.VisualState;
        whereIsToken.VisualState = VisualStates.Dragging;
        whereIsToken.BringToFront();

    }

    public override void OnDraggingInUpdate()
    {

    }

    public override void OnEndDrag()
    {
        GameObject CardTarget = null;
        RaycastHit[] hits;
        TokenVisual tokenVisual = Tabletop.Instance.tokenVisual;
        // TODO: raycast here anyway, store the results in 
        hits = Physics.RaycastAll(origin: Camera.main.transform.position, 
            direction: (-Camera.main.transform.position + this.transform.position).normalized, 
            maxDistance: 30f) ;

        foreach (RaycastHit h in hits)
        {
            if (h.transform.tag == "Card")
                CardTarget = h.transform.gameObject; 
        }

        // 1) Check if we are holding a card over the table
        if (DragSuccessful() && Tabletop.Instance.ControlsON == true)
        {
            if (CardTarget != null)
            {
                // int cardID = CardTarget.GetComponent<IDHolder>().UniqueID;
                // HandVisual PlayerHand = Tabletop.Instance.handVisual;
                // HandLogic handLogic = Tabletop.Instance.hand;
                // GameObject card = IDHolder.GetGameObjectWithID(cardID);
                // handLogic.CardsInHandLogic.Remove(CardLogic.CardsCreatedThisGame[cardID]);
                // PlayerHand.RemoveCard(card);
                // GameObject.Destroy(card);

                // int uniqueTokenID = this.GetComponent<IDHolder>().UniqueID;
                // GameObject token = IDHolder.GetGameObjectWithID(uniqueTokenID);

                // TokenLogic.RemoveHPIfTokenRemoved(uniqueTokenID);
                
                // tokenVisual.RemoveToken(token);
                // GameObject.Destroy(token);
                // TurnManager.Instance.EndTurn();
                int cardID = CardTarget.GetComponent<IDHolder>().UniqueID;
                GameObject card = IDHolder.GetGameObjectWithID(cardID);
                int uniqueTokenID = this.GetComponent<IDHolder>().UniqueID;
                GameObject token = IDHolder.GetGameObjectWithID(uniqueTokenID);

                new PlayATokenCommand(cardID, card, uniqueTokenID, token).AddToQueue();
                Command.CommandExecutionComplete();      
                TurnManager.Instance.EndTurn();
            }
        }
        else
        {
            // Set old sorting order 
            whereIsToken.SetHandSortingOrder();
            whereIsToken.VisualState = tempState;
            // Move this card back to its slot position
            Vector3 oldTokenPos = tokenVisual.slots.Children[savedTokenSlot].transform.localPosition;
            transform.DOLocalMove(oldTokenPos, 1f);
        } 
    }

    protected override bool DragSuccessful()
    {
        // bool TableNotFull = (Tabletop.Instance.tokenVisual.EnemiesOnTable.Count < Tabletop.Instance.tokenVisual.slots.Children.Length);

        return Tabletop.Instance.handVisual.CursorOverThisCard;
    }
}
