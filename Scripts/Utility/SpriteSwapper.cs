using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSwapper : MonoBehaviour
{
    public Sprite SpriteToSwapTo;

    [SerializeField]
    private SpriteRenderer _renderer;

    private void OnValidate()
    {
        if (_renderer == null)
            _renderer = GetComponent<SpriteRenderer>();
    }

    
    public void SwapSprite()
    {
        _renderer.sprite = SpriteToSwapTo;
    }

}
