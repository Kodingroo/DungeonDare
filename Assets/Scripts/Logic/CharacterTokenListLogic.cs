using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterTokenListLogic : MonoBehaviour {
 
    // SINGLETON
    public static CharacterTokenListLogic Instance;    

    public List<TokenAsset> tokens = new List<TokenAsset>();

    void Awake()
    {
        Instance = this;
    }
	
}
