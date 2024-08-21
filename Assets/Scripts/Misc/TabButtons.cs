using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class TabButtons : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public TabGroup tabGrroup;

    public Image backGround;

    public void OnPointerClick(PointerEventData eventData)
    {
        tabGrroup.OnTabSelected(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tabGrroup.OnTabEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tabGrroup.OnTabExit(this);
    }

    void Start()
    {
        backGround = GetComponent<Image>();
        tabGrroup.Subscribe(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
