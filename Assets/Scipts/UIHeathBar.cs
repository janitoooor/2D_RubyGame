using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHeathBar : MonoBehaviour
{
    //открытый доступ к скрипту
    //—войство статического экземпл€ра UIHealthBar €вл€етс€ статическим и общедоступным,
    //поэтому вы можете написать UIHealthBar.instance в любом скрипте,
    //и он вызовет это свойство get. —войство set €вл€етс€ закрытым, потому что мы не хотим,
    //чтобы люди могли измен€ть его вне этого скрипта
    public static UIHeathBar instance { get; private set; }

    public Image mask;
    float originalSize;

    //Awake вызываетс€ сразу после создани€ объекта
    private void Awake()
    {
        //вы сохран€ете в статическом экземпл€ре this , которое €вл€етс€ специальным ключевым
        //словом C# , означающим Ђобъект, который в данный момент запускает эту функциюї.
        instance = this;
    }

    void Start()
    {
        //получени€ размера на экране с помощью rect.width
        originalSize = mask.rectTransform.rect.width;
    }

    public void SetValue(float value)
    {
        //установки размера и прив€зки из кода с помощью SetSizeWithCurrentAnchors.
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * value);
    }
}
