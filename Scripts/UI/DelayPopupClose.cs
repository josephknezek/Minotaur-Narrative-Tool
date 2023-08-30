using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DelayPopupClose : MonoBehaviour
{
    public UnityEvent OnComplete = new UnityEvent();

    public void DelayForSeconds(float sec)
    {
        StartCoroutine(Delay(sec));
    }

    IEnumerator Delay(float sec)
    {
        yield return new WaitForSecondsRealtime(sec);
        OnComplete.Invoke();
    }
}
