using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISwitcher : MonoBehaviour
{
    [SerializeField] Transform DefaultSubUI;

    Transform currentActivateUI;
    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform child in transform)
        {
            if(child.parent == transform)
            {
                child.gameObject.SetActive(false);
            }
        }
        SetActiveUI(DefaultSubUI);
    }

    public void SetActiveUI(Transform newActiveUI)
    {
        if (newActiveUI == currentActivateUI)
        {
            return;
        }

        if (currentActivateUI != null)
        {
            currentActivateUI.gameObject.SetActive(false);
        }

        newActiveUI.gameObject.SetActive(true);
        currentActivateUI = newActiveUI;
    }
}
