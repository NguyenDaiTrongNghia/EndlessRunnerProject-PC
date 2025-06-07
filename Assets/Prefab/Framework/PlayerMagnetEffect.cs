using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMagnetEffect : MonoBehaviour
{
    [SerializeField] float magnetRadius = 10f;

    private bool isMagnetActive = false;
    private Coroutine magnetCoroutine;
    public event Action<float> OnMagnetStarted;
    public event Action OnMagnetEnded;
    void Update()
    {
        if (isMagnetActive)
        {
            AttractCoins();
        }
    }

    public void ActivateMagnet(float duration)
    {
        isMagnetActive = true;
        if (magnetCoroutine != null)
        {
            StopCoroutine(magnetCoroutine);
        }
        OnMagnetStarted?.Invoke(duration);
        magnetCoroutine = StartCoroutine(RemoveMagnet(duration));
    }

    IEnumerator RemoveMagnet(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        isMagnetActive = false;
        magnetCoroutine = null;
        OnMagnetEnded?.Invoke();
    }

    void AttractCoins()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, magnetRadius);
        foreach (Collider hit in hits)
        {
            Coin coin = hit.GetComponent<Coin>();
            if (coin != null)
            {
                coin.MoveTowardsPlayer(transform);
            }
        }
    }
}