using UnityEngine;
using System.Collections;

public class DefeatTargetAndNext : TokenEffect
{
    public virtual void ActivateEffect(int specialAmount = 0, GameObject target = null)
    {
        // new TokenNegateCommand(target.ID, this.ID);
        // new EnemyDieCommand(target.ID).AddToQueue();
        Debug.Log("DefeatTargetAndNext");
    }
}
