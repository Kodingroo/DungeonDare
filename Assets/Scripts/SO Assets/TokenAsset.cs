using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TokenTargets
{
    NoTarget,
    One,
    Two,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine
}
public enum TokenClass
{
    Warrior
}

[CreateAssetMenu(menuName = "Token Asset")]
public class TokenAsset : ScriptableObject {

    // this object will hold the info about the most general card
    [Header("General info")]
	public Sprite GraphicImage;
	public Sprite StrengthImage;

    [Header("Token Info")]
	public TokenClass Class;
    public int TokenHealth = 0;
    public string TokenEffectName;
    public string Description;
    public TokenTargets Targets;
    public bool MainToken = false;
}
