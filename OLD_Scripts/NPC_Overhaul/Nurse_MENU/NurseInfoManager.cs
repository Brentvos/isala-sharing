using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OLD
{
    public class NurseInfoManager : MonoBehaviour
    {
        [SerializeField] private GameObject nurseInfoElement;
        [SerializeField] private Transform nurseParent;

        private void Start()
        {
            NpcDatabase.Instance.OnNursesGenerated += OnNursesGenerated;
        }

        private void OnNursesGenerated(List<NurseData> nurses)
        {
            for (int i = 0; i < nurses.Count; i++)
            {
                GameObject nurseElement = Instantiate(nurseInfoElement, nurseParent, false);
                //GameObject nurseDraggableElem = Instantiate(nurseDraggableElement, nurseDraggableParent, false);

                nurseElement.GetComponent<NurseInfoElement>().Initialize(nurses[i]);
                //nurseDraggableElem.GetComponent<DraggableNurseElement>().Initialize(nurses[i]);
            }
        }
    }
}