using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PowerUpDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI labelText;

    private Coroutine countdownCoroutine;

    public void Show(string label, float duration)
    {
        gameObject.SetActive(true);
        if (countdownCoroutine != null)
            StopCoroutine(countdownCoroutine);
        countdownCoroutine = StartCoroutine(UpdateCountdown(label, duration));
    }

    private IEnumerator UpdateCountdown(string label, float duration)
    {
        float remaining = duration;
        while (remaining > 0)
        {
            labelText.SetText($"{label}: {remaining:F1}s");
            yield return null;
            remaining -= Time.deltaTime;
        }
        //gameObject.SetActive(false);
    }

    public void Stop()
    {
        if (countdownCoroutine != null)
            StopCoroutine(countdownCoroutine);
        gameObject.SetActive(false);
    }
}
