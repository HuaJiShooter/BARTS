using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextScripts : MonoBehaviour
{
    Text textbox = null;
    // Start is called before the first frame update
    void Start()
    {
        textbox = GetComponent<Text>();
        Debug.Log("�޸�ǰ��"+ textbox.text);
        if (Application.isEditor)
        {
            textbox.text = "��ʱ���ڱ༭��������";
        }
        else
        {
            textbox.text = "��ʱ���ڷ��а汾������";
        }

        Debug.Log("�޸ĺ�" + textbox.text.ToString());
    }

    // Update is called once per frame
}