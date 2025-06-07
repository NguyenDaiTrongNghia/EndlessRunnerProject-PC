using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Pickup
{
    [SerializeField] float CoinSpeed = 20f;
    public void MoveTowardsPlayer(Transform player)
    {
        float CoinDistance = CoinSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, player.position, CoinDistance);
    }
}
