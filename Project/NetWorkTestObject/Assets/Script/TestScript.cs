using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript:MonoBehaviour
{
    public TestScript()
    {
        if (Application.isEditor)
        {
            Debug.Log("��ʱ���ڱ༭��������");
        }
        else
        {
            Debug.Log("��ʱ���ڷ��а汾������");
        }
    }
}
