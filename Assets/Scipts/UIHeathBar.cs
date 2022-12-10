using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHeathBar : MonoBehaviour
{
    //�������� ������ � �������
    //�������� ������������ ���������� UIHealthBar �������� ����������� � �������������,
    //������� �� ������ �������� UIHealthBar.instance � ����� �������,
    //� �� ������� ��� �������� get. �������� set �������� ��������, ������ ��� �� �� �����,
    //����� ���� ����� �������� ��� ��� ����� �������
    public static UIHeathBar instance { get; private set; }

    public Image mask;
    float originalSize;

    //Awake ���������� ����� ����� �������� �������
    private void Awake()
    {
        //�� ���������� � ����������� ���������� this , ������� �������� ����������� ��������
        //������ C# , ���������� �������, ������� � ������ ������ ��������� ��� ��������.
        instance = this;
    }

    void Start()
    {
        //��������� ������� �� ������ � ������� rect.width
        originalSize = mask.rectTransform.rect.width;
    }

    public void SetValue(float value)
    {
        //��������� ������� � �������� �� ���� � ������� SetSizeWithCurrentAnchors.
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * value);
    }
}
