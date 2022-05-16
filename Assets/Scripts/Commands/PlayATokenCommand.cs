using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayATokenCommand : Command
{
    private int uniqueCardID;
    private GameObject card;
    private int uniqueTokenID;
    private GameObject token;

    public PlayATokenCommand(int uniqueCardID, GameObject card, int uniqueTokenID, GameObject token)
    {
        this.uniqueCardID = uniqueCardID;
        this.card = card;
        this.uniqueTokenID = uniqueTokenID;
        this.token = token;
    }
    public override void StartCommandExecution()
    {
        HandVisual PlayerHand = Tabletop.Instance.handVisual;
        HandLogic handLogic = Tabletop.Instance.hand;
        handLogic.CardsInHandLogic.Remove(CardLogic.CardsCreatedThisGame[uniqueCardID]);
        PlayerHand.RemoveCard(card);
        GameObject.Destroy(card);

        TokenLogic.RemoveHPIfTokenRemoved(uniqueTokenID);

        TokenVisual tokenVisual = Tabletop.Instance.tokenVisual;
        tokenVisual.RemoveToken(token);
        GameObject.Destroy(token);

        CardLogic cardLogic = CardLogic.CardsCreatedThisGame[uniqueCardID];
        handLogic.CardsInHandLogic.Remove(cardLogic);          
        Command.CommandExecutionComplete();        
    }
}
