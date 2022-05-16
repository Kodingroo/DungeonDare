using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerClass
{
    Princess,
    Necromancer,
    Bard,
    Ninja
}

[CreateAssetMenu(menuName = "Player Asset")]
public class PlayerAsset : ScriptableObject 
{
	public PlayerClass Class;
	public string ClassName;
	public int BaseHealth;
    public string TitleText;
	public Sprite AvatarImage;
    public Sprite AvatarBGImage;
    public Sprite TitleBackgroundImage;
    public Color32 AvatarBGTint;
}
