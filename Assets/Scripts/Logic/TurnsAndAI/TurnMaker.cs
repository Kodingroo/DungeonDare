using UnityEngine;
using System.Collections;

public abstract class TurnMaker : MonoBehaviour {

    protected Player p;

    void Awake()
    {
        p = GetComponent<Player>();
    }

    public virtual void OnTurnStart()
    {
        // add one mana crystal to the pool;
        p.playerPortraitVisual.PortraitFrame.color = new Color32(251,28,28,255);
        p.otherPlayer.playerPortraitVisual.PortraitFrame.color = new Color32(255,255,255,255);     
        TurnManager.Instance.CheckIfPlayerAI();
        p.OnTurnStart();
    }

}
