using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TitleManager : UIHandler
{
    /* 타이틀 화면과 관련된 기능을 정의하는 클래스 */
    public TextMeshProUGUI titleText;

    protected override void Start()
    {
        base.Start();
        InvokeRepeating("BlinkText", 0.5f, 0.5f);
    }

     public void BlinkText()
    {
        titleText.gameObject.SetActive(!titleText.gameObject.activeInHierarchy);
    }
}
