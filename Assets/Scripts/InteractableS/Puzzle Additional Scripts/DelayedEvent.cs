using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DelayedEvent : MonoBehaviour
{
    Coroutine coroutine;
    public UnityEvent AfterDelay;
    public float delay = 1;

    public void Activate()
    {
        if (coroutine != null) StopCoroutine(coroutine);
        coroutine = StartCoroutine(DelayCoroutine());

        IEnumerator DelayCoroutine()
        {
            yield return new WaitForSeconds(delay);
            AfterDelay.Invoke();
        }
    }


}
