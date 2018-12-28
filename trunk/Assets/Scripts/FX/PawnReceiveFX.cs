using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnReceiveFX : MonoBehaviour
{
    Pawn m_Pawn = null;
    public List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();
    ParticleSystem part;

    void Awake()
    {
        part = GetComponent<ParticleSystem>();
        Transform currTransform = transform;

        while (currTransform != null && currTransform.GetComponent<Pawn>() == null)
            currTransform = currTransform.parent;

        if (currTransform != null)
            m_Pawn = currTransform.GetComponent<Pawn>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (m_Pawn == null)
            Destroy(this);
    }

    void OnParticleCollision(GameObject other)
    {
        GunfireFX gunfireFX = other.GetComponent<GunfireFX>();
        if (gunfireFX != null)
        {
            m_Pawn.ReceiveDmg(gunfireFX.Weapon.damage);
        }
    }
}