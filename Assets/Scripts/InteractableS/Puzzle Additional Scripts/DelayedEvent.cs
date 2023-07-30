using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DelayedEvent : MonoBehaviour
{
    Coroutine coroutine;
    public UnityEvent AfterDelay;
    public float delay = 1;
    public bool activateOnce = true;
    private bool active = false;

    public void Activate()
    {
        if (activateOnce && active)
            return;

        if (coroutine != null) StopCoroutine(coroutine);
        coroutine = StartCoroutine(DelayCoroutine());

        IEnumerator DelayCoroutine()
        {
            yield return new WaitForSeconds(delay);
            AfterDelay.Invoke();
            active = true;
        }
    }


}
