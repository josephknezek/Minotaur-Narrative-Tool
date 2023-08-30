using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class StringEventTMPSetter : MonoBehaviour
{
    public TypedSOEvent<string> ListenEvent;

    [SerializeField]
    private TMP_Text text;

    private void OnValidate()
    {
        if (text == null)
            text = GetComponent<TMP_Text>();
    }

    // Start is called before the first frame update
    void Awake()
    {
        ListenEvent.AddListener((x) => text.SetText(x));
    }
}
