using UnityEngine;
using System.Collections;
using DG.Tweening;

public class PlayerTakeDamageCommand : Command 
{
    private EnemyLogic target;
    private int attack;

    public PlayerTakeDamageCommand(EnemyLogic target, int attack)
    {
        this.target = target;
        this.attack = attack;
    }

    public override void StartCommandExecution()
    {
        Sequence s = DOTween.Sequence();
        s.AppendInterval(0.5f); 
        s.OnComplete(() =>
            { 
                // Command.CommandExecutionComplete();
               target.Health -= attack;         
            }
         );        
    }
}
