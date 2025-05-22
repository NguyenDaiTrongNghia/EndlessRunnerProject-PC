using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMagnetEffect : MonoBehaviour
{
    [SerializeField] float magnetDuration = 5f;
    [SerializeField] float magnetRadius = 10f;

    private bool isMagnetActive = false;
    private float timer = 0f;
    public event Action<float> OnMagnetStarted;
    public event Action OnMagnetEnded;
    void Update()
    {
        if (isMagnetActive)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                isMagnetActive = false;
                OnMagnetEnded?.Invoke();
                return;
            }

            AttractCoins();
        }
    }

    public void ActivateMagnet()
    {
        isMagnetActive = true;
        timer = magnetDuration;
        OnMagnetStarted?.Invoke(magnetDuration);
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