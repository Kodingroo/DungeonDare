using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// holds the refs to all the Text, Images on the card
public class OneIconManager : MonoBehaviour {

    public EnemyCardAsset cardAsset;
    // public OneCardManager PreviewManager;
    [Header("Text Component References")]
    public Text AttackText;
    [Header("Image References")]
    public Image IconGraphicImage;
    public Image IconFaceFrameImage;    
    public Image IconWeaknessOneImage;
    public Image IconWeaknessTwoImage;

    void Awake()
    {
        if (cardAsset != null)
            ReadIconFromAsset();
    }

    private bool canAttackNow = false;
    public bool CanAttackNow
    {
        get
        {
            return canAttackNow;
        }

        set
        {
            canAttackNow = value;
        }
    }

    public void ReadIconFromAsset()
    {
        // Change the card graphic sprite
        IconGraphicImage.sprite = cardAsset.GraphicImage;
        IconWeaknessOneImage.sprite = cardAsset.WeaknessOneImage;
        IconWeaknessTwoImage.sprite = cardAsset.WeaknessTwoImage;

        AttackText.text = cardAsset.Attack.ToString();

        // if (PreviewManager != null)
        // {
        //     PreviewManager.cardAsset = cardAsset;
        //     PreviewManager.ReadCardFromAsset();
        // }
    }	
}
