using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Door_Behavior : MonoBehaviour
{
    private Level_Door m_Door;

    private bool m_Triggered = false;

    // Start is called before the first frame update
    void Start()
    {
        Transform currTransform = transform;

        while (currTransform != null && currTransform.GetComponent<Level_Door>() == null)
            currTransform = currTransform.parent;

        if (currTransform != null)
            m_Door = currTransform.GetComponent<Level_Door>();
        else
            Destroy(this);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!m_Triggered)
        {
            m_Triggered = true;
            m_Door.ToggleDoor(true);
            Destroy(this);
        }
    }
}