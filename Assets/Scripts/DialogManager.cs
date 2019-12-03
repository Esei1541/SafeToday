using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    private Animator animator;
    private bool isDialogActive;
    public TextMeshProUGUI nameText, dialogText;
    public Image charImage;
    public Sprite[] CharSprite;

    // Start is called before the first frame update
    void Start()
    {
        isDialogActive = false;
        animator = GetComponent<Animator>();
        DialogActive();
    }

    public void DialogActive()
    {
        animator.SetBool("Appear", !animator.GetBool("Appear"));
        isDialogActive = !isDialogActive;
    }

    public void SetDialogText(string name, string text)
    {
        nameText.text = name;
        dialogText.text = text;
    }

    public void SetCharImage(int num)
    {
        charImage.GetComponent<Image>().sprite = CharSprite[num];
    }
    
}
