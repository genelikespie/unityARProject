using UnityEngine;

public class GameControl : MonoBehaviour
{
    public static GameControl instance;

    public bool pauseInARMode = false;
    public GameObject tower;
    public GameObject builderButtons;

    bool isARMode;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    void Start()
    {
        SwitchToARMode();
    }

    public void SwitchToARMode()
    {
        if (isARMode)
            return;

        isARMode = true;

        if (pauseInARMode)
            Time.timeScale = 0f;

        if (builderButtons)
            builderButtons.SetActive(true);
    }

    public void SwitchToVRMode()
    {
        if (!isARMode)
            return;

        isARMode = false;

        if (pauseInARMode)
            Time.timeScale = 1f;

        if (builderButtons)
            builderButtons.SetActive(false);
    }
}
