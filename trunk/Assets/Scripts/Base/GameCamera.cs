using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Camera))]
public class GameCamera : MonoBehaviour
{
    [SerializeField]
    float m_CameraMovementSpeed = 10.0f;

    [SerializeField]
    float m_CameraRotateSpeed = 20.0f;

    private Camera m_ThisCamera;

    private float m_InitialHeight;
    private int m_CurrentLevel;

    void Awake()
    {
        m_CurrentLevel = 1;
        m_ThisCamera = GetComponent<Camera>();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_InitialHeight = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        HandleTranslation();
        HandleRotation();
        HandleLevel();
    }

    void HandleTranslation()
    {
        Vector3 camPosition = transform.position;

        float camZ = Input.GetAxis("Horizontal") * Time.deltaTime * m_CameraMovementSpeed;
        float camX = Input.GetAxis("Vertical") * Time.deltaTime * m_CameraMovementSpeed;

        Vector3 forwardDir = (new Vector3(transform.forward.x, 0.0f, transform.forward.z)).normalized;
        Vector3 newCamPosition = camPosition + (forwardDir * camX);
        newCamPosition += transform.right * camZ;
        transform.position = newCamPosition;
    }

    void HandleRotation()
    {
        float rotation = 0.0f;
        if (Input.GetKey(KeyCode.Q))
            rotation++;
        if (Input.GetKey(KeyCode.E))
            rotation--;

        transform.RotateAround(GetCamLookPoint(), Vector3.up, rotation * Time.deltaTime * m_CameraRotateSpeed);
    }

    Vector3 GetCamLookPoint()
    {
        float angle = Vector3.Angle(transform.forward, Vector3.forward);
        //Debug.Log("Cam angle: " + angle);

        float length = Mathf.Atan(angle * Mathf.Deg2Rad) * transform.position.y;
        //Debug.Log("Cam length: " + length);

        float distance = Mathf.Sqrt((length * length) + (transform.position.y * transform.position.y));
        //Debug.Log("Cam distance: " + distance);

        Vector3 lookPoint = transform.position + (transform.forward * distance);
        //Debug.Log("Cam look point: " + lookPoint);

        return lookPoint;
    }

    void HandleLevel()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            if (LevelManager.FloorNum > m_CurrentLevel)
                m_CurrentLevel++;

            transform.position = new Vector3(transform.position.x, m_InitialHeight + (Level_Base.FloorHeight * m_CurrentLevel), transform.position.z);
        }
        else if (Input.GetKeyUp(KeyCode.F))
        {
            if (m_CurrentLevel > 1)
                m_CurrentLevel--;

            transform.position = new Vector3(transform.position.x, m_InitialHeight + (Level_Base.FloorHeight * m_CurrentLevel), transform.position.z);
        }
    }
}
