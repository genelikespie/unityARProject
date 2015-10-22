/*==============================================================================
 * Copyright (c) 2012-2014 Qualcomm Connected Experiences, Inc. All Rights Reserved.
 *==============================================================================*/
using UnityEngine;
using System.Collections;

public class UDTAppManager : AppManager {
    
    public override void InitManager ()
    {
        base.InitManager ();
        mTargetHandler = GameObject.FindObjectOfType(typeof(UserDefinedTargetEventHandler)) as UserDefinedTargetEventHandler;
        mTargetHandler.Init();
    }
    
    public override void Draw ()
    {
        base.Draw ();
        if(mActiveViewType == AppManager.ViewType.ARCAMERAVIEW)
        {
            mTargetHandler.Draw();
        }
    }
    
    public override void UpdateManager ()
    {
        base.UpdateManager ();
    }

    private UserDefinedTargetEventHandler mTargetHandler;
}
