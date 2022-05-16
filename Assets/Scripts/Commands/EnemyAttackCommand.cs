using UnityEngine;
using System.Collections;

public class EnemyAttackCommand : Command 
{
    // position of Enemy on enemy`s table that will be attacked
    // if enemyindex == -1 , attack an enemy character 
    private int TargetUniqueID;
    private int AttackerUniqueID;
    private int TargetHealthAfter;
    private int DamageTakenByTarget;

    public EnemyAttackCommand(int targetID, int attackerID, int damageTakenByTarget, int targetHealthAfter)
    {
        this.TargetUniqueID = targetID;
        this.AttackerUniqueID = attackerID;
        this.TargetHealthAfter = targetHealthAfter;
        this.DamageTakenByTarget = damageTakenByTarget;
    }

    public override void StartCommandExecution()
    {
        GameObject Attacker = IDHolder.GetGameObjectWithID(AttackerUniqueID);

        Attacker.GetComponent<EnemyAttackVisual>().AttackTarget(TargetUniqueID, DamageTakenByTarget, TargetHealthAfter);
    }
}
