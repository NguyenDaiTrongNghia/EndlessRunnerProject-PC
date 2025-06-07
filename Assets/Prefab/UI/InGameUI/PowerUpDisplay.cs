using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PowerUpDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI powerupsText;

    private Coroutine countdownCoroutine;

    public void Show(string label, float duration)
    {
        gameObject.SetActive(true);
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
        }
        countdownCoroutine = StartCoroutine(UpdateCountdown(label, duration));
    }

    private IEnumerator UpdateCountdown(string label, float duration)
    {
        //float remaining = duration;
        while (duration > 0)
        {
            powerupsText.SetText($"{label}: {duration:F0}s");
            yield return null;
            duration -= Time.deltaTime;
        }
    }

    public void Stop()
    {
        gameObject.SetActive(false);
    }
}
