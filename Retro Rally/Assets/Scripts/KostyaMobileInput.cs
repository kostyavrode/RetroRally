using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KostyaMobileInput : MonoBehaviour
{
    //[SerializeField] private GameObject RCCCanvas;
    private void Start()
    {
        if (!Application.isMobilePlatform)
        {
            //RCCCanvas.SetActive(false);
            RCC.SetController(0);
        }
        else
        {
            RCC.SetController(1);
        }
    }
}
