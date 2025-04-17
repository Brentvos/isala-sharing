using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OLD
{


    public class NurseInteractiveMoraleManager : MonoBehaviour
    {
        [SerializeField] private GameObject nurseInteractiveMoraleElement;
        [SerializeField] private Transform moraleParent;


        private void Start()
        {
            NpcDatabase.Instance.OnNursesGenerated += OnNursesGenerated;
        }

        private void OnNursesGenerated(List<NurseData> nurses)
        {
            for (int i = 0; i < nurses.Count; i++)
            {
                GameObject nurseInteractiveElement = Instantiate(nurseInteractiveMoraleElement, moraleParent, false);

                nurseInteractiveElement.GetComponent<NurseInteractiveMoraleElement>().Initialize(nurses[i]);
            }
        }
    }
}