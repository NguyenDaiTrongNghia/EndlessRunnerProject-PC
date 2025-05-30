using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : Spawnable
{
    [SerializeField] int coinEffect;

    bool bAdjustedHigher = false;

    private void OnTriggerEnter(Collider other)//
    {
        if(other.gameObject.tag == "Player")
        {
            ScoreKeeper scoreKeeper = FindObjectOfType<ScoreKeeper>();
            if (scoreKeeper != null && coinEffect != 0)
            {
                scoreKeeper.ChangeCoin(coinEffect);
            }

            PickedUpBy(other.gameObject);
        }

        if(other.gameObject.tag == "Threat" && !bAdjustedHigher)
        {
            //Debug.Log("Over lap");
            Collider col = other.GetComponent<Collider>();
            if(col!=null)
            {
                transform.position = col.bounds.center + col.bounds.extents.y  * Vector3.up;
                bAdjustedHigher = true;
            }
        } 
    }

    protected virtual void PickedUpBy(GameObject picker)
    {
        Destroy(gameObject);
    }
}
