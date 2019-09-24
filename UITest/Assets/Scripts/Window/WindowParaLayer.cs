using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowParaLayer : MonoBehaviour
{
    [SerializeField]
    private GameObject darkenBGObject = null;

    private List<GameObject> containedScreens = new List<GameObject>();

    public void AddScreen(Transform screentRectTransform)
    {
        screentRectTransform.SetParent(transform, false);
        containedScreens.Add(screentRectTransform.gameObject);
    }

    public void RefreshDarken()
    {
        for(int i=0; i < containedScreens.Count; i++)
        {
            if(containedScreens[i] != null)
            {
                if (containedScreens[i].activeSelf)
                {
                    darkenBGObject.SetActive(true);
                    return;
                }
            }
        }

        darkenBGObject.SetActive(false);
    }

    public void DarkenBG()
    {
        darkenBGObject.SetActive(true);
        darkenBGObject.transform.SetAsLastSibling();
    }
}
