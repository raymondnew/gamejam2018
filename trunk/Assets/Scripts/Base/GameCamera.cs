using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Camera))]
public class GameCamera : MonoBehaviour
{
    [SerializeField]
    float m_CameraMovementSpeed = 1.0f;

    private Camera m_ThisCamera;

    void Awake()
    {
        m_ThisCamera = GetComponent<Camera>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        HandleTranslation();
    }

    void HandleTranslation()
    {
        Vector3 camPosition = transform.position;

        float camX = camPosition.x;
        float camZ = camPosition.z;

        camX += Input.GetAxis("Horizontal") * Time.deltaTime * m_CameraMovementSpeed;
        camZ += Input.GetAxis("Vertical") * Time.deltaTime * m_CameraMovementSpeed;

        camPosition = new Vector3(camX, camPosition.y, camZ);
        transform.position = camPosition;
    }
}
