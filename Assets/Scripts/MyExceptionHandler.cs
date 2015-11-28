using System;
using UnityEngine;
using UnityEngine.UI;

public class MyExceptionHandler : MonoBehaviour
{
    GameManager gameManager;
    public void setGameManager(GameManager gameManager)
    {
        this.gameManager = gameManager;
    //    Application.logMessageReceivedThreaded += HandleUnityLog;
    }
    /* void OnEnable()
     {
         Application.RegisterLogCallback(HandleUnityLog);
     }

     void OnDisable()
     {
         Application.RegisterLogCallback(null);
     }

     void HandleUnityLog(string condition, string stacktrace, LogType type)
     {
         if (type != LogType.Error && type != LogType.Exception)
             return;

         Debug.Log("zzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzz");
     }*/
    void OnEnable()
    {
        Debug.Log("MyExceptionHandler OnEnable");
        Application.logMessageReceived += HandleUnityLog;
     //   Application.logMessageReceivedThreaded += HandleUnityLog;
        

    }
  
     void OnDisable()
    {
        Debug.Log("MyExceptionHandler OnDisable");
        Application.logMessageReceived -= HandleUnityLog;
     //   Application.logMessageReceivedThreaded -= HandleUnityLog;
    }

     void HandleUnityLog(string condition, string stacktrace, LogType type)
    {
        Debug.Log("MyExceptionHandler HandleUnityLog");
        if (type != LogType.Error && type != LogType.Exception)
            return;
        //       GameObject button = GameObject.Find("ErrorButton");
        /*    if (ErrButton != null)
            {
                Debug.Log("Button ok");


            }
            else
            {
                Debug.Log("Button null");
              //  Debug.Break();
            }*/
        gameManager.SetState(gameManager.errorState);
    }
}