using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BarkingCharacter : MonoBehaviour
{
    const int FadeIterations = 30;
    const float FadeFraction = 1f / (float)FadeIterations;
    public const float FadeTime = 0.2f;

    [Header("Character Info")]
    public Character MyCharacter;
    public BarkSOEvent ApproachEvent;

    [Header("Object Dependencies")]
    public TMP_Text BarkText;

    [Header("Advanced")]
    [LabelAs("Show Advanced Options?")]
    public bool ShowAdvancedOptions = false;

    [ShowIf(nameof(ShowAdvancedOptions)), Min(0f)]
    public float CharacterCooldownOverride = 30f;


    [HideInInspector]
    public bool OnCooldown = false;


    private GameObject _barkObject;
    private Coroutine _bark;

    private void OnValidate()
    {
        if (TryGetComponent(out TalkableNPC NPC))
            MyCharacter = NPC.MyCharacter;
    }

    private void Start()
    {
        _barkObject = BarkText.gameObject;
    }

    public void SayBark(BarkSO bark)
    {
        if (OnCooldown && !bark.OverrideCooldowns)
            return;

        if (_bark != null)
            StopCoroutine(_bark);

        // Add cooldown + fading times
        StartCoroutine(Cooldown(bark.TimeToDisplay + (2 * FadeTime) + CharacterCooldownOverride));
        _bark = StartCoroutine(DisplayBark(bark));
    }
    
    public IEnumerator DisplayBark(BarkSO bark)
    {
        // Startup
        _barkObject.SetActive(true);
        BarkText.SetText(bark.Response);
        BarkText.ForceMeshUpdate();
        bark.StartCooldown();

        // Fade in
        BarkText.alpha = 0;
        for (int i = 0; i < FadeIterations; i++)
        {
            BarkText.alpha += FadeFraction;
            yield return new WaitForSeconds(FadeTime * FadeFraction);
        }

        // Display for time
        yield return new WaitForSeconds(bark.TimeToDisplay);

        // Fade out
        for (int i = 0; i < FadeIterations; i++)
        {
            BarkText.alpha -= FadeFraction;
            yield return new WaitForSeconds(FadeTime * FadeFraction);
        }

        // Finalize
        _barkObject.SetActive(false);
        bark.OnBarkComplete();
    }

    private IEnumerator Cooldown(float time)
    {
        OnCooldown = true;
        yield return new WaitForSeconds(time);
        OnCooldown = false;
    }
}
