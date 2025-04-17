using System;
using UnityEngine;
using UnityEngine.Rendering;


namespace New
{
    /// <summary>
    /// This is the most basic patient info
    /// </summary>

    [Serializable]
    public class PatientInfo
    {
        private int code;
        private int age;
        private int weight;
        private LAST_NAME lastName;
        private GENDER gender;
        private PATIENT_TYPE patientType;
        private Sprite closeUpAvatar;
        private Sprite zoomOutAvatar;
        
        private string[] symptoms;
        private string diagnose;

        public int GetCode { get { return code; } }
        public int GetAge { get { return age; } }
        public int GetWeight { get { return weight; } } 
        public LAST_NAME GetLastName { get {  return lastName; } }
        public GENDER GetGender { get { return gender; } }
        public PATIENT_TYPE GetPatientType { get { return patientType; } }
        public Sprite GetCloseUpAvatar { get { return closeUpAvatar; } }
        public Sprite GetZoomOutAvatar { get { return zoomOutAvatar; } }
        public string[] GetSymptoms { get { return symptoms; } }
        public string GetDiagnose { get { return diagnose; } }

        public PatientInfo(int code, int weight, int age, LAST_NAME firstName, PATIENT_TYPE type)
        {
            this.code = code;
            this.age = age;
            this.lastName = firstName;
            this.weight = weight;
            this.patientType = type;

            gender = Utility_Functions.GetGender();

            int avatarIndex = Utility_Functions.GetAvatarIndex(gender, age);

            closeUpAvatar = GameManager.Instance.GetPatientCloseUpSprites[avatarIndex];
            zoomOutAvatar = GameManager.Instance.GetPatientZoomOutSprites[avatarIndex];
        }

        public void SetSymptoms(string[] symptoms)
        {
            this.symptoms = symptoms;
        }

        public void SetDiagnose(string diagnose)
        {
            this.diagnose = diagnose;
        }

        public string GetNameWithTitle(bool addLineBreak = false)
        {
            string name = string.Empty;

            if (gender == GENDER.MALE)
                name = "Mr. ";

            else
                name = "Mw. ";

            if (addLineBreak)
                name += "\n";

            name += lastName.ToString();
            return name;
        }
    }
}