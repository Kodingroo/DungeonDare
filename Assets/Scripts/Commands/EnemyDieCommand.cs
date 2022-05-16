using UnityEngine;
using System.Collections;
using DG.Tweening;

public class EnemyDieCommand : Command 
{
    private int DeadEnemyID;

    public EnemyDieCommand(int EnemyID)
    {
        this.DeadEnemyID = EnemyID;
    }

    public override void StartCommandExecution()
    {
        // Debug.Log("EnemyDieCommand: "+DeadEnemyID);
        // remove and destroy the icon on the table
        DungeonVisual Dungeon = Tabletop.Instance.dungeonVisual;
        GameObject icon = IDHolder.GetGameObjectWithID(DeadEnemyID);
        Dungeon.EnemiesOnTable.Remove(icon);
        GameObject.Destroy(icon);
        // Dungeon.ShiftSlotsGameObjectAccordingToNumberOfEnemies();
        Dungeon.PlaceEnemiesOnNewSlots();
        Sequence s = DOTween.Sequence();
        // s.AppendInterval(0.5f);
        s.OnComplete(Command.CommandExecutionComplete); 
        Player whoseTurn = TurnManager.Instance.whoseTurn;
        // if (Tabletop.Instance.dungeonVisual.EnemiesOnTable.Count == 0 && whoseTurn.otherPlayer.Health > 0)
        // {
        //     Debug.Log("You Won!");
        //     whoseTurn.otherPlayer.Win();
        // }               
    }
}
