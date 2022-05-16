using UnityEngine;
using System.Collections;

public class PlayerTurnMaker : TurnMaker 
{
    public override void OnTurnStart()
    {
        Tabletop.Instance.ControlsON = true;
        base.OnTurnStart();
        // dispay a message that it is player`s turn
        new ShowMessageCommand($"Your\nTurn", 1.0f).AddToQueue();
        // new ShowMessageCommand($"Player {p.ID}\nTurn", 1.0f).AddToQueue();
        Tabletop.Instance.DrawACard();
    }
}
