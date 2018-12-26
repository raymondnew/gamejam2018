using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Door : Level_Base
{
    [SerializeField]
    Transform m_DoorTransform;

    [SerializeField]
    Vector3 m_DoorOpenPosition;

    [SerializeField]
    Vector3 m_DoorOpenRotation;

    [SerializeField]
    bool m_Open = false;

    void Awake()
    {
        if (Application.isPlaying)
            ToggleDoor(false);
    }

    protected override void Update()
    {
        base.Update();

        if(!Application.isPlaying)
        {
            ToggleDoor(m_Open);
        }
    }

    public void ToggleDoor(bool open)
    {
        if (m_DoorTransform == null)
            return;

        m_Open = open;

        if (open)
        {
            m_DoorTransform.localPosition = m_DoorOpenPosition;
            m_DoorTransform.localRotation = Quaternion.Euler(m_DoorOpenRotation);
        }
        else
        {
            m_DoorTransform.localPosition = Vector3.zero;
            m_DoorTransform.localRotation = Quaternion.identity;
        }
    }
}