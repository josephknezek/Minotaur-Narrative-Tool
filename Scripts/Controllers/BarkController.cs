using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
public class BarkController : MonoBehaviour
{
    private void Start()
    {
        BarkingCharacter playerBarker = GetComponentInParent<BarkingCharacter>();

        if (playerBarker == null)
            return;

        RuntimeBarks.Barks.AddViableCharacter(playerBarker);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out BarkingCharacter NPC))
        {
            RuntimeBarks.Barks.AddViableCharacter(NPC);

            if (NPC.ApproachEvent != null)
                RuntimeBarks.Barks.PromptBark(NPC.ApproachEvent);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out BarkingCharacter NPC))
            RuntimeBarks.Barks.RemoveViableCharacter(NPC);
    }

}
