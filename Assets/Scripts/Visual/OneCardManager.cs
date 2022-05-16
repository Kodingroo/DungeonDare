using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// holds the refs to all the Text, Images on the card
public class OneCardManager : MonoBehaviour {

    public EnemyCardAsset cardAsset;
    public OneCardManager PreviewManager;
    public Text TitleText;
    public Text AttackText;
    [Header("Image References")]
    public Image CardGraphicImage;
    public Image CardBodyImage;
    public Image CardFaceFrameImage;    
    public Image CardWeaknessOneImage;
    public Image CardWeaknessTwoImage;
    public Image CardLowRibbonImage;



    void Awake()
    {
        if (cardAsset != null)
            ReadCardFromAsset();
    }

    // private bool canBePlayedNow = false;
    // public bool CanBePlayedNow
    // {
    //     get
    //     {
    //         return canBePlayedNow;
    //     }

    //     set
    //     {
    //         canBePlayedNow = value;
    //     }
    // }

    public void ReadCardFromAsset()
    {
        TitleText.text = cardAsset.name;
        AttackText.text = cardAsset.Attack.ToString();
        CardGraphicImage.sprite = cardAsset.GraphicImage;
        CardWeaknessOneImage.sprite = cardAsset.WeaknessOneImage;
        CardWeaknessTwoImage.sprite = cardAsset.WeaknessTwoImage;
        if (PreviewManager != null)
        {
            PreviewManager.cardAsset = cardAsset;
            PreviewManager.ReadCardFromAsset();
        }        
    }
}
