namespace New
{
    public class DoctorRoom : RoomRoot
    {        
        public override void OnPlayerEnter()
        {
            base.OnPlayerEnter();
            PatientInfoManager.Instance.PatientInfoResponseOnPlayerRoomInteraction(true);
            // Enable patient info
        }

        public override void OnPlayerExit()
        {
            base.OnPlayerExit();
            PatientInfoManager.Instance.PatientInfoResponseOnPlayerRoomInteraction(false);
            // Disable patient info
        }
    }
}