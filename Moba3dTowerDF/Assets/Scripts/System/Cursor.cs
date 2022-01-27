using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    // Start is called before the first frame update
    #region Value

    public Vector3 m_vCursorPos;

    #endregion

    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        Set_CursorPos();

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
}
