/*===============================================================================
Copyright (c) 2012-2015 Qualcomm Connected Experiences, Inc. All Rights Reserved.
 
Confidential and Proprietary - Qualcomm Connected Experiences, Inc.
Vuforia is a trademark of QUALCOMM Incorporated, registered in the United States 
and other countries. Trademarks of QUALCOMM Incorporated are used with permission.
===============================================================================*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Vuforia;

public class UserDefinedTargetEventHandler : MonoBehaviour, IUserDefinedTargetEventHandler
{
    #region PUBLIC_MEMBERS
    /// <summary>
    /// Can be set in the Unity inspector to reference a ImageTargetBehaviour that is instanciated for augmentations of new user defined targets.
    /// </summary>
    public ImageTargetBehaviour ImageTargetTemplate;
	public int maxNumTargets = 5;
	public bool ExtendedTrackingIsEnabled = true;
    public int IndexForMostRecentlyAddedTrackable
    {
        get
        {
            return ( mTargetCounter - 1 ) % maxNumTargets;
        }
    }

    #endregion PUBLIC_MEMBERS

    #region PRIVATE_MEMBERS

    private UserDefinedTargetBuildingBehaviour mTargetBuildingBehaviour;
    private ObjectTracker mObjectTracker;
    // DataSet that newly defined targets are added to
    private DataSet mBuiltDataSet;
    // currently observed frame quality
    private ImageTargetBuilder.FrameQuality mFrameQuality = ImageTargetBuilder.FrameQuality.FRAME_QUALITY_NONE;
    // counter variable used to name duplicates of the image target template
    private int mTargetCounter;

    #endregion PRIVATE_MEMBERS

    /// <summary>
    /// Registers this component as a handler for UserDefinedTargetBuildingBehaviour events
    /// </summary>
    public void Start()
    {
        mTargetBuildingBehaviour = GetComponent<UserDefinedTargetBuildingBehaviour>();
        if (mTargetBuildingBehaviour)
        {
            mTargetBuildingBehaviour.RegisterEventHandler(this);
            Debug.Log ("Registering to the events of IUserDefinedTargetEventHandler");
        }

        //---------------------------start edit
        //bomb count sensitive to display the button to take picture
        if (GameObject.Find("DefusingBOMB").GetComponent<BombState>().numberOfBombs == 0)
        {
			Debug.Log("Make some UI here.");
        }
	}

    #region IUserDefinedTargetEventHandler implementation
    /// <summary>
    /// Called when UserDefinedTargetBuildingBehaviour has been initialized successfully
    /// </summary>
    public void OnInitialized ()
    {
        mObjectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
        if (mObjectTracker != null)
        {
            // create a new dataset
            mBuiltDataSet = mObjectTracker.CreateDataSet();
            mObjectTracker.ActivateDataSet(mBuiltDataSet);
        }
    }

    /// <summary>
    /// Updates the current frame quality
    /// </summary>
    public void OnFrameQualityChanged(ImageTargetBuilder.FrameQuality frameQuality)
    {
        mFrameQuality = frameQuality;
    }

    /// <summary>
    /// Takes a new trackable source and adds it to the dataset
    /// This gets called automatically as soon as you 'BuildNewTarget with UserDefinedTargetBuildingBehaviour
    /// </summary>
    public void OnNewTrackableSource(TrackableSource trackableSource)
    {
        mTargetCounter++;
        
        // deactivates the dataset first
        mObjectTracker.DeactivateDataSet(mBuiltDataSet);

        // Destroy the oldest target if the dataset is full or the dataset 
        // already contains five user-defined targets.
        if (mBuiltDataSet.HasReachedTrackableLimit() || mBuiltDataSet.GetTrackables().Count() >= maxNumTargets)
        {
            IEnumerable<Trackable> trackables = mBuiltDataSet.GetTrackables();
            Trackable oldest = null;
            foreach (Trackable trackable in trackables)
                if (oldest == null || trackable.ID < oldest.ID)
                    oldest = trackable;
            
            if (oldest != null)
            {
                Debug.Log("Destroying oldest trackable in UDT dataset: " + oldest.Name);
                mBuiltDataSet.Destroy(oldest, true);
            }
        }
        
        // get predefined trackable and instantiate it
        ImageTargetBehaviour imageTargetCopy = (ImageTargetBehaviour)Instantiate(ImageTargetTemplate);
        //imageTargetCopy.gameObject.name = "UserDefinedTarget-" + mTargetCounter;
        imageTargetCopy.gameObject.name = "ActiveBomb";

        // add the duplicated trackable to the data set and activate it
        mBuiltDataSet.CreateTrackable(trackableSource, imageTargetCopy.gameObject);
        
        // activate the dataset again
        mObjectTracker.ActivateDataSet(mBuiltDataSet);

        //Extended Tracking with user defined targets only works with the most recently defined target.
        //If tracking is enabled on previous target, it will not work on newly defined target.
        //Don't need to call this if you don't care about extended tracking.
        StopExtendedTracking();
        mObjectTracker.Stop();
        mObjectTracker.ResetExtendedTracking();
        mObjectTracker.Start();

    }
    #endregion IUserDefinedTargetEventHandler implementation

    #region PRIVATE_METHODS
    /// <summary>
    /// Instantiates a new user-defined target and is also responsible for dispatching callback to 
    /// IUserDefinedTargetEventHandler::OnNewTrackableSource
    /// </summary>
    private void BuildNewTarget()
    {
        // create the name of the next target.
        // the TrackableName of the original, linked ImageTargetBehaviour is extended with a continuous number to ensure unique names
        string targetName = string.Format("{0}-{1}", ImageTargetTemplate.TrackableName, mTargetCounter);
        
        // generate a new target name:
        
        mTargetBuildingBehaviour.BuildNewTarget(targetName, ImageTargetTemplate.GetSize().x);
        
        //---------------------------start edit
        //Adds a bomb count when the bomb is taken a picture of. 
        GameObject.Find("DefusingBOMB").GetComponent<BombState>().numberOfBombs++;
        // Tell the GameManager that the bomb has been planted
        GameManager gameManager = GameManager.Instance();
        gameManager.bombPlanted = true;
        //Sends the buildnewtarget 
        //---------------------------end edit
    }

    public void OnTappedOnNewTargetButton()
    {
        BuildNewTarget();
    }

    /// <summary>
    /// This method only demonstrates how to handle extended tracking feature when you have multiple targets in the scene
    /// So, this method could be removed otherwise
    /// </summary>
   private void StopExtendedTracking()
    {
        StateManager stateManager = TrackerManager.Instance.GetStateManager();
        
        //If the extended tracking is enabled, we first disable OTT for all the trackables
        //and then enable it for the newly created target
        //UDTUIEventHandler uiMenuEventHandler = FindObjectOfType(typeof(UDTUIEventHandler)) as UDTUIEventHandler;
        if(ExtendedTrackingIsEnabled)
        {
            //Stop extended tracking on all the trackables
            foreach(var behaviour in stateManager.GetTrackableBehaviours())
            {
                var imageBehaviour = behaviour as ImageTargetBehaviour;
                if(imageBehaviour != null)
                {
                    imageBehaviour.ImageTarget.StopExtendedTracking();
                }
            }
    
            List<TrackableBehaviour> list =  stateManager.GetTrackableBehaviours().ToList();

            ImageTargetBehaviour bhvr = list[IndexForMostRecentlyAddedTrackable] as ImageTargetBehaviour;
            if(bhvr != null)
            {
                Debug.Log("ExtendedTracking enabled for " + bhvr.name);
                bhvr.ImageTarget.StartExtendedTracking();
            }
        }
    }
    #endregion PRIVATE_METHODS
}



