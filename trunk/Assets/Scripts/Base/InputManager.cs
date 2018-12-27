using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager g_Inst;

    public Pawn m_TestPawn;

    void Awake()
    {
        g_Inst = this;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
            MovePawnTest();

        /*if (Input.GetKeyUp(KeyCode.Space) && m_TestPawn != null)
            m_TestPawn.Halt();//*/
    }

    void MovePawnTest()
    {
        if (m_TestPawn == null)
            return;


        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(mouseRay, out hitInfo, Mathf.Infinity, LevelManager.WalkableLayer))
        {
            m_TestPawn.MoveTo(hitInfo.point);
        }
    }
}