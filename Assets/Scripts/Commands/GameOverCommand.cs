using UnityEngine;
using System.Collections;

public class GameOverCommand : Command{

    private Player loser;
    string message;

    public GameOverCommand(Player loser, string message)
    {
        this.loser = loser;
        this.message = message;
    }

    public override void StartCommandExecution()
    {
        if (loser.Health <= 0)
            loser.playerPortraitVisual.Explode();
            
        MessageManager.Instance.GameOverMessage(loser, message);
    }
}
