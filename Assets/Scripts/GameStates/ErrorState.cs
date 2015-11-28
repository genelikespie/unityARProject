using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using System.Collections;

public class ErrorState : State
{
    Button ErrButton;
    public virtual void Awake()
    {
        // Call the base class's function to initialize all variables
        base.Awake();

        // Find all UI elements in the scene
        ErrButton = GameObject.Find("ErrorButton").GetComponent<Button>();
        if (!ErrButton)
            Debug.LogError("ErrButton");

    }

    public override void Initialize()
    {
        
    }

    public override void RunState()
    {
        
    }

}
