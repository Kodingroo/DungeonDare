using UnityEngine;
using System.Collections;

public class TokenNegateCommand : Command 
{
    // position of Enemy on enemy`s table that will be attacked
    // if enemyindex == -1 , attack an enemy character 
    private int TargetUniqueID;
    private int TokenUniqueID;

    public TokenNegateCommand(int targetID, int tokenID)
    {
        this.TargetUniqueID = targetID;
        this.TokenUniqueID = tokenID;
    }

    public override void StartCommandExecution()
    {
        GameObject Token = IDHolder.GetGameObjectWithID(TokenUniqueID);
        Token.GetComponent<TokenNegateVisual>().NegateTarget(TargetUniqueID);
    }
}
