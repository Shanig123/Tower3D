using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityController : MonoBehaviour
{
    AbilityController()
        : base(){ m_listAbility = new List<int>(); }
    [SerializeField] private List<int> m_listAbility;
    [SerializeField] PlayerController m_playerController;
    #region Property

    public bool Find_Ability(int _iAbilityCode)
    {
        // m_playerController   m_playerController.Get_AbilityArr;
        if (_iAbilityCode < 1)
            return false;
        for(int i= 0; i<m_listAbility.Count;++i)
        {
            if (_iAbilityCode == m_listAbility[i])
                return true;
        }
        return false;

        //foreach(int iter in m_listAbility)
        //{
        //    if (iter == _iAbilityCode)
        //        return true;
        //}
        //return false;
    }

    public int Add_Ability { set { if (value > 0) m_listAbility.Add(value); m_listAbility.Sort(); } }
    public List<int> Get_AbilityList { get { return m_listAbility; } }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        if (m_playerController == null)
            return;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
