using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    public List<TabButtons> buttons;
    public Sprite tabIdle;
    public Sprite tabActive;
    public Sprite tabHover;
    public TabButtons selectedTab;
    public List<GameObject> objectToSwap;

    public void Subscribe(TabButtons button)
    {
        if (buttons == null)
        {
            buttons = new List<TabButtons>();
        }
        buttons.Add(button);
    }

    public void OnTabEnter(TabButtons button)
    {
        ResetTab();
        if (selectedTab == null || button != selectedTab)
        {
            button.backGround.sprite = tabHover;

        }
    }

    public void OnTabExit(TabButtons button)
    {
        ResetTab();
    }

    public void OnTabSelected(TabButtons button)
    {
        selectedTab = button;
        ResetTab();
        button.backGround.sprite = tabActive;
        int index = button.transform.GetSiblingIndex();
        for (int i = 0; i < objectToSwap.Count; i++)
        {
            if (i == index)
            {
                objectToSwap[i].gameObject.SetActive(true);
            } else
            {
                objectToSwap[i].gameObject.SetActive(false);
            }
        }
    }

    public void ResetTab()
    {
        foreach (TabButtons button in buttons)
        {
            if (selectedTab != null && button == selectedTab)
            {
                continue;
            }
            button.backGround.sprite = tabIdle;
        }
    }
    public
    void Start()
    {
       
    }

    void Update()
    {

    }
}
