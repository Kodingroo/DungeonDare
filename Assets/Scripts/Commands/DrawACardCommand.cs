using UnityEngine;
using System.Collections;

public class DrawACardCommand : Command {

    private Tabletop t;
    private CardLogic cl;
    private bool fast;

    public DrawACardCommand(CardLogic cl, Tabletop t, bool fast)
    {        
        this.cl = cl;
        this.t = t;
        this.fast = fast;
    }

    public override void StartCommandExecution()
    {
        t.deckVisual.CardsInDeck--;
        t.handVisual.GivePlayerACard(cl.ca, cl.UniqueCardID, fast);
    }
}
