using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript:MonoBehaviour
{
    public TestScript()
    {
        if (Application.isEditor)
        {
            Debug.Log("此时正在编辑器中运行");
        }
        else
        {
            Debug.Log("此时正在发行版本中运行");
        }
    }
}
