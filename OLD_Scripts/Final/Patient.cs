using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OLD_2
{
    public class Patient : MonoBehaviour
    {
        [SerializeField] private PatientType patientType;
        [SerializeField] private List<PatientState> patientStates = new List<PatientState>();
        private PatientState currentPatientState;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
