using UnityEngine;
using System.Collections;

public class PopulateTokensCommand : Command {

    public PopulateTokensCommand()
    {
    }

    public override void StartCommandExecution()
    {
        Tabletop.Instance.CreateAToken();
        Tabletop.Instance.tokenVisual.PopulateTokensInGame(); 
        // this command is completed instantly
        CommandExecutionComplete();
    }
}
