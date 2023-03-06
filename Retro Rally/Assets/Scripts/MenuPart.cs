using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MenuPart : MonoBehaviour
{
    public string menuName;
    public void Open()
    {
        gameObject.SetActive(true);
    }
    public void Close()
    {
        gameObject.SetActive(false); 
    }
}
