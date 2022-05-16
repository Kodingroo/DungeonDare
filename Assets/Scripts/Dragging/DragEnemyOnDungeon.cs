using UnityEngine;
using System.Collections;
using DG.Tweening;

public class DragEnemyOnDungeon : DraggingActions {

    private int savedHandSlot;
    private WhereIsTheCardOrEnemy whereIsCard;
    private VisualStates tempState;
    private OneIconManager manager;

    public override bool CanDrag
    {
        get
        { 
            // TEST LINE: this is just to test playing creatures before the game is complete 
            // return true;

            // TODO : include full field check
            return base.CanDrag;
        }
    }

    void Awake()
    {
        whereIsCard = GetComponent<WhereIsTheCardOrEnemy>();
        manager = GetComponent<OneIconManager>();
    }

    public override void OnStartDrag()
    {
        savedHandSlot = whereIsCard.Slot;
        tempState = whereIsCard.VisualState;
        whereIsCard.VisualState = VisualStates.Dragging;
        whereIsCard.BringToFront();

    }

    public override void OnDraggingInUpdate()
    {

    }

    public override void OnEndDrag()
    {
        
        // 1) Check if we are holding a card over the table
        if (DragSuccessful() && Tabletop.Instance.ControlsON == true)
        {
            // play this card
            Tabletop.Instance.PlayAnEnemyFromHand(GetComponent<IDHolder>().UniqueID, 0);
            TurnManager.Instance.EndTurn();
        }
        else
        {
            // Set old sorting order 
            whereIsCard.SetHandSortingOrder();
            whereIsCard.VisualState = tempState;
            // Move this card back to its slot position
            HandVisual PlayerHand = Tabletop.Instance.handVisual;
            Vector3 oldCardPos = PlayerHand.slots.Children[savedHandSlot].transform.localPosition;
            transform.DOLocalMove(oldCardPos, 1f);
        } 
    }

    protected override bool DragSuccessful()
    {
        bool TableNotFull = (Tabletop.Instance.dungeonVisual.EnemiesOnTable.Count < Tabletop.Instance.dungeonVisual.slots.Children.Length);

        return Tabletop.Instance.dungeonVisual.CursorOverThisTable && TableNotFull;
    }
}
