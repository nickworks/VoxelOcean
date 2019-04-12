using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropHandler : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler

{
    public void OnDrop(PointerEventData eventData)
    {
        print(eventData.pointerDrag.name + "was dropped on " + gameObject.name);
        DragHandler d = eventData.pointerDrag.GetComponent<DragHandler>();

        if(d != null)
        {
            d.parentToReturnTo = this.transform;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }

    void Start()
    {

    }

    void Update()
    {

    }
}
