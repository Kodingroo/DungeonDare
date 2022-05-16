using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PossibleTargets
{
    NoTarget,
    YourPlayer, 
    AIPlayer
}

[CreateAssetMenu(menuName = "Card Asset")]
public class EnemyCardAsset : ScriptableObject {

    // this object will hold the info about the most general card
    [Header("General info")]
	public Sprite GraphicImage;
	public Sprite WeaknessOneImage;
	public Sprite WeaknessTwoImage;

    [Header("Enemy Info")]
    public int Attack;
    // public bool Charge;
    public string EnemyScriptName;
    public PossibleTargets Targets;
}
