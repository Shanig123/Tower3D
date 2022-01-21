using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    // Start is called before the first frame update
    #region Value

    public Vector3 m_vCursorPos;
    //[SerializeField] private DataEnum.eControl_Mode m_eCurControlState = DataEnum.eControl_Mode.End;

    #endregion

    void Start()
    {
        //m_eCurControlState = DataEnum.eControl_Mode.NoControl;
    }

    // Update is called once per frame
    void Update()
    {
        Set_CursorPos();
 

        KeyDownLeft();
        KeyDownRight();

        transform.rotation = IsBillBoard().rotation;

#if UNITY_EDITOR
#else
          GetComponent<Renderer>().enabled = false;
#endif
    }

    private void Set_CursorPos()
    {
        Vector3 vMousePos = Input.mousePosition;
        vMousePos.z = -Camera.main.transform.position.z;

        m_vCursorPos = Camera.main.ScreenToWorldPoint(vMousePos);
        gameObject.transform.position = m_vCursorPos;
    }

    private void KeyDownLeft()
    {

        if (Input.GetMouseButtonDown(0))
        {
            RayPicking();
        }
    }
    private void KeyDownRight()
    {

        //if (Input.GetMouseButtonDown(1))
        //{
        //    if (m_eCurControlState == DataEnum.eControl_Mode.NoControl)
        //        m_eCurControlState = DataEnum.eControl_Mode.Construction;
        //    else
        //        m_eCurControlState = DataEnum.eControl_Mode.NoControl;
        //}
    }

    private Matrix4x4 IsBillBoard()
    {
        Matrix4x4 matBill = Camera.main.transform.localToWorldMatrix;

        matBill.m30 = 0; matBill.m31 = 0; matBill.m32 = 0; matBill.m33 = 1;
  
        return matBill;
    }

    private Matrix4x4 IsYBillBoard()
    {
        Matrix4x4 matBill = Camera.main.transform.localToWorldMatrix;

        matBill.m01 = 0;
        matBill.m10 = 0; matBill.m11 = 1; matBill.m12 = 0;
        matBill.m21 = 0;
        matBill.m30 = 0; matBill.m31 = 0; matBill.m32 = 0; matBill.m33 = 1;

        return  matBill;
    }

    private void RayPicking()
    {
        //RaycastHit
        //Input.

        //Debug.Log(m_vCursorPos);
        //Debug.Log(Input.mousePosition);

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        string strLayerName = "";

        //if (m_eCurControlState == DataEnum.eControl_Mode.NoControl) //일반 모드
        //{
        //    strLayerName = "Tile";
        //}
        //else
        //{
        //    strLayerName = "Tower";
        //}
        //int iLayerMask = (1 << LayerMask.NameToLayer(strLayerName)) | (1 << LayerMask.NameToLayer("UI"));

        //if (Physics.Raycast(ray, out hit, 20.0f, iLayerMask))
        //{

        //}
    }

}
