using UnityEngine;
using System.Collections;
using DG.Tweening;

public class PlayAnEnemyCommand : Command
{
    private CardLogic cl;
    private int tablePos;
    private Player p;
    private int enemyID;

    public PlayAnEnemyCommand(CardLogic cl, Player p, int tablePos, int enemyID)
    {
        this.p = p;
        this.cl = cl;
        this.tablePos = tablePos;
        this.enemyID = enemyID;
    }

    public override void StartCommandExecution()
    {
        // remove and destroy the card in hand 
        HandVisual PlayerHand = Tabletop.Instance.handVisual;
        GameObject card = IDHolder.GetGameObjectWithID(cl.UniqueCardID);            
        PlayerHand.RemoveCard(card);
        GameObject.Destroy(card);
        // move this card to the spot 
        Tabletop.Instance.dungeonVisual.AddEnemyAtIndex(cl.ca, enemyID, tablePos);
        // TurnManager.Instance.EndTurn();
    }
}
