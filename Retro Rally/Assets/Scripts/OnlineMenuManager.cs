using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OnlineMenuManager : MonoBehaviour
{
    public static OnlineMenuManager instance;
    public enum menuNames { loading, title, createRoom, roomMenu, error }
    [SerializeField] private MenuPart[] menuParts;
    private void Awake()
    {
        if (instance==null)
        {
            instance = this;
        }
    }
    public void OpenMenu(string name)
    {
        foreach (MenuPart menu in menuParts)
        {
            if (menu.menuName == name)
            {
                menu.Open();
            }
            else
            {
                menu.Close();
            }
        }
    }
}
