using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    public Alignment alignment;

    private void OnTriggerEnter(Collider other)
    {
        DealContactDamage(other);
    }

    private void OnCollisionEnter(Collision collision)
    {
        DealContactDamage(collision.collider);
    }

    private void DealContactDamage(Collider collider)
    {
        TankController tank = collider.GetComponent<TankController>();
        if (tank && tank.alignment != alignment)
        {
            tank.Kill();
        }
    }
}
