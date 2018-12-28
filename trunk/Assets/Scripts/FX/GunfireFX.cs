using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunfireFX : MonoBehaviour
{
    public MainWeapon Weapon { get; private set; }

    void Awake()
    {
        Transform currTransform = transform;

        while (currTransform != null && currTransform.GetComponent<Agent>() == null)
            currTransform = currTransform.parent;

        if (currTransform != null)
            Weapon = currTransform.GetComponent<Agent>().MainWeapon;
        else
            Destroy(gameObject);
    }
}