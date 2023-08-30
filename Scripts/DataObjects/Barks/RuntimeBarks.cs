using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeBarks : MonoBehaviour
{
    public static ManualBarkList Barks;
    public static BarkEventHolder BarkEvents;

    [SerializeField]
    private ManualBarkList _barks;

    [SerializeField]
    private BarkEventHolder _barkEvents;

    void Awake()
    {
        Barks = _barks;
        BarkEvents = _barkEvents;
    }


    private void Start()
    {
        Barks.Initialize();
    }


    void Update()
    {
        Barks.CountdownCooldowns();
    }
}
