using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TestFairyUnity;
using UnityEngine.SceneManagement;

public class InitTestFairy : MonoBehaviour
{
    private void Start()
    {
        TestFairy.begin("SDK-um9XnvKH");
    }

    [HideInInspector] public string output = "";
    [HideInInspector] public string stack = "";

    private void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        output = logString;
        stack = stackTrace;
        TestFairy.log(output);
        TestFairy.log(stack);
    }
}
