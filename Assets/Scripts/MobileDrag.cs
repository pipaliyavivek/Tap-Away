using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MobileDrag : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public bool isPressing = false;
    public void OnDrag(PointerEventData data)
    {
        isPressing = true;
        //CubesGanerate.Instance.OnDrag(data);
        GridEditor.Instance.OnDrag(data);
    }

    public void OnEndDrag(PointerEventData data)
    {
        isPressing = false;
    }
}
