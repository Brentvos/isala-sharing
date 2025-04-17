using UnityEngine;

namespace New
{
    public class Bed : MonoBehaviour
    {
        // Add place for player?

        [Header("Variables")]
        // [SerializeField] private BED_ROTATION rotation;
        [SerializeField] private bool flipped;
        
        private bool isOccupied;
        public bool IsOccupied { get { return isOccupied; } }

        private PatientRoot currentPatient;
        public PatientRoot GetCurrentPatient { get {  return currentPatient; } }

        public void Occupy(PatientRoot patient)
        {
            currentPatient = patient;
            isOccupied = true;

            PatientSpawningManager.Instance.SpawnPatientOnField(currentPatient, transform, flipped);
        }

        public void ReleasePatient()
        {
            // PatientSpawningManager -> Remove?
             // Destroy(currentPatient); Destroy the element that is attached
            currentPatient = null;
            isOccupied = false;
        }

        public void AssignNurse() // Add nurse
        {
            // Nurse.destination = placeForNurse;
        }

        public void ReleaseNurse() // Add nurse
        {
            // Nurse.destination = placeForNurse;
        }
    }
}