using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DebugHero : MonoBehaviour
{

    public UnityEvent<Vector3> MouseClickPositionEvent;
    public UnityEvent OnPressA;
    public UnityEvent OnPressS;

    void OnFire()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 pos = new Vector3();

        if (Physics.Raycast(ray, out hit))
            pos = hit.point;

        MouseClickPositionEvent.Invoke(pos);
    }

    void OnA()
    {
        OnPressA.Invoke();
    }

    void OnS()
    {
        OnPressS.Invoke();
    }
}
