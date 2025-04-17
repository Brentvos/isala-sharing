using TMPro;
using UnityEngine;

namespace New
{
    public class PatientInfoSymptom : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI symptomTXT;

        public void Initialize(string symptom)
        {
            symptomTXT.text = symptom;
        }
    }
}
