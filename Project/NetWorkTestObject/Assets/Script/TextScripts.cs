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
        Debug.Log("修改前："+ textbox.text);
        if (Application.isEditor)
        {
            textbox.text = "此时正在编辑器中运行";
        }
        else
        {
            textbox.text = "此时正在发行版本中运行";
        }

        Debug.Log("修改后：" + textbox.text.ToString());
    }

    // Update is called once per frame
}