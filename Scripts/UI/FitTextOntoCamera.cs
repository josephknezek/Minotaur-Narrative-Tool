using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


[RequireComponent(typeof(TMP_Text))]
public class FitTextOntoCamera : MonoBehaviour
{
    [SerializeField]
    private Vector2 _padding = new Vector2(0.25f, 0.25f);

    [SerializeField, HideInInspector]
    private Transform _myTransform;

    [SerializeField, HideInInspector]
    private TMP_Text _myText;

    private Vector3 _anchorPoint;

    private void OnValidate()
    {
        if (_myTransform == null)
            _myTransform = GetComponent<Transform>();

        if (_myText == null)
            _myText = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        _anchorPoint = transform.position;
    }

    private void Update()
    {
        // Try to set our position to the anchor
        transform.position = _anchorPoint;

        // Make sure it actually fits
        Camera.main.FitAreaIntoCamera(_myText.GetWorldspaceTextBounds(_padding), ref _myTransform);
    }

}
