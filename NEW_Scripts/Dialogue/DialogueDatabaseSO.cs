using System.Collections.Generic;
using UnityEngine;

namespace New
{

    [CreateAssetMenu(fileName = "casesDatabase", menuName = "scriptableObjects/casesDB")]

    public class DialogueDatabaseSO : ScriptableObject
    {
        [SerializeField] private List<CaseSO> cases;
        public List<CaseSO> GetCases { get { return cases; } }  
    }
}