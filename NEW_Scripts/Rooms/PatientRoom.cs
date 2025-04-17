using System.Linq;
using UnityEngine;

namespace New
{
    public class PatientRoom : RoomRoot
    {
        [SerializeField] private URGENCY_TYPE urgencyLevel;
        [SerializeField] private Bed[] beds; 

        public URGENCY_TYPE GetUrgencyLevel { get {  return urgencyLevel; } }

        public bool HasSpace()
        {
            return beds.Any(bed => !bed.IsOccupied);
        }

        public void AssignPatient(PatientRoot patient)
        {
            Bed availableBed = beds.FirstOrDefault(bed => !bed.IsOccupied);

            if (availableBed == null)
                return;

            availableBed.Occupy(patient);

            if (IsPlayerInside)
                patient.GetClinicalStatus.OverrideDegression(true);
        }

        public bool IsPatientInRoom(PatientRoot patient)
        {
            return beds.Any(bed => bed.GetCurrentPatient == patient);

        }
        public void ReleasePatient(PatientRoot patient)
        {
            beds.FirstOrDefault(bed => bed.GetCurrentPatient == patient)
                .ReleasePatient();
        }

        public override void OnPlayerEnter()
        {
            base.OnPlayerEnter();

            foreach(var bed in beds)
            {
                if (bed.IsOccupied)
                {
                    bed.GetCurrentPatient.GetClinicalStatus.OverrideDegression(true);
                }
            }
        }

        public override void OnPlayerExit()
        {
            base.OnPlayerExit();

            foreach (var bed in beds)
            {
                if (bed.IsOccupied)
                    bed.GetCurrentPatient.GetClinicalStatus.OverrideDegression(false);
            }
        }
    }
}