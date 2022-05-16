using UnityEngine;
using System.Collections;

public class TokenEffect
{
    public virtual void ActivateEffect(int specialAmount = 0, ICharacter target = null)
    {
        Debug.Log("No Token effect with this name found! Check for typos in TokenAssets");
    }
        
}
