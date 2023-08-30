using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public static class PlayerInputExtensions
{
    
    public static bool GetIsInDefaultMap(this PlayerInput input)
    {
        return input.currentActionMap.name == input.defaultActionMap;
    }

}
