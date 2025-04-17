using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace New
{
    public class PatientInfoManager : MonoBehaviour
    {
        public static PatientInfoManager Instance;

        private readonly int PATIENTS_PER_PAGE = 8;

        [Header("General Components")]
        //[SerializeField] private TextMeshProUGUI pageStatusDisplay_TXT;
        //[SerializeField] private Button nextPage_BTN;
        //[SerializeField] private Button previousPage_BTN;
        [SerializeField] private Button patientInfoPage_BTN;
        [SerializeField] private GameObject patientActivePageIcon_OBJ;
        [SerializeField] private Transform patientPage;


        [Header("Selected Patient Components")]
        [SerializeField] private GameObject[] infoParents_OBJs;
        [SerializeField] private TextMeshProUGUI selectedPatientName_TXT;
        [SerializeField] private TextMeshProUGUI selectedPatientCode_TXT;
        [SerializeField] private TextMeshProUGUI selectedPatientAge_TXT;
        [SerializeField] private TextMeshProUGUI selectedPatientWeight_TXT;
        [SerializeField] private Image selectedPatientZoomOutAvatar_IMG;
        [Space(5)]
        [Header("Detailed Info Components")]
        [SerializeField] private GameObject selectedPatientSymptom_PARENT;
        [SerializeField] private GameObject selectedPatientSymptom_PREFAB;
        [SerializeField] private TextMeshProUGUI selectedPatientDiagnose_TXT;
        // [SerializeField] private TextMeshProUGUI selectedPatientSex_TXT;

        // [SerializeField] private TextMeshProUGUI selectedPatientTreatment_TXT;
        // [SerializeField] private Image selectedPatientAvatar_IMG;
        // [SerializeField] private Image selectedPatientVloeistofShortage_IMG;

        private int currentPage;
        private int totalPages;
        private List<PatientInfoDisplayElement> infoElements = new List<PatientInfoDisplayElement>();

        private void Awake()
        {
            if (Instance != this)
                Instance = this;
        }

        private void Start()
        {

            PatientSpawningManager.Instance.OnFieldPatientSpawned += OnFieldPatientSpawned;
            PatientSpawningManager.Instance.OnPatientRemoved += OnPatientRemoved;

            //nextPage_BTN.onClick.AddListener(() => NextPage());
           // previousPage_BTN.onClick.AddListener(() => PreviousPage());
            patientInfoPage_BTN.onClick.AddListener(() => OpenPatientInfo());

            patientInfoPage_BTN.interactable = false;
            ResetDetailedView();
            //UpdatePages();
        }

        private void OnPatientRemoved(PatientRoot patient)
        {
            infoElements.Remove(patient.GetPatientInfoElement);
            //UpdatePages();

            if (patient.GetPatientInfo.GetCode.ToString() == selectedPatientCode_TXT.text)
                ResetDetailedView();
        }

        public void ProcessPatient(PatientRoot patient) 
        {
            foreach(Transform transform in selectedPatientSymptom_PARENT.GetComponentsInChildren<Transform>())
            {
                if (transform != selectedPatientSymptom_PARENT.transform)
                {
                    Destroy(transform.gameObject);
                }
            }

            selectedPatientName_TXT.text = patient.GetPatientInfo.GetNameWithTitle();
            selectedPatientCode_TXT.text = patient.GetPatientInfo.GetCode.ToString();
            selectedPatientAge_TXT.text = patient.GetPatientInfo.GetAge.ToString() + " jaar oud";
            selectedPatientWeight_TXT.text = patient.GetPatientInfo.GetWeight.ToString() + " kilo";
            selectedPatientZoomOutAvatar_IMG.sprite = patient.GetPatientInfo.GetZoomOutAvatar;

            if (patient.GetPatientInfo.GetSymptoms != null && patient.GetPatientInfo.GetSymptoms.Length > 0)
                FillSymptomTab(patient.GetPatientInfo.GetSymptoms);

            else
            {
                PatientInfoSymptom sympt = Instantiate(selectedPatientSymptom_PREFAB,
                    selectedPatientSymptom_PARENT.transform, false).GetComponent<PatientInfoSymptom>();

                sympt.Initialize("X");
            }

            if (patient.GetPatientInfo.GetDiagnose != null && patient.GetPatientInfo.GetDiagnose != string.Empty)
                FillDiagnoseTab(patient.GetPatientInfo.GetDiagnose);

            else
                selectedPatientDiagnose_TXT.text = "X";

            foreach (GameObject obj in infoParents_OBJs)
                obj.SetActive(true);
        }

        private void FillSymptomTab(string[] symptoms)
        {
            foreach(string symptom in symptoms)
            {
                PatientInfoSymptom sympt = Instantiate(selectedPatientSymptom_PREFAB, 
                    selectedPatientSymptom_PARENT.transform, false).GetComponent<PatientInfoSymptom>();

                sympt.Initialize(symptom);
            }
        }

        private void FillDiagnoseTab(string diagnose)
        {
            selectedPatientDiagnose_TXT.text = diagnose;
        }

        private void ClosePatientInfoMenu()
        {
            ResetDetailedView();
            patientPage.gameObject.SetActive(false);
            patientActivePageIcon_OBJ.SetActive(false);
        }

        private void ResetDetailedView()
        {
            selectedPatientName_TXT.text = string.Empty;
            selectedPatientCode_TXT.text = string.Empty;

            foreach (Transform transform in selectedPatientSymptom_PARENT.GetComponentsInChildren<Transform>())
            {
                if (transform != selectedPatientSymptom_PARENT.transform)
                {
                    Destroy(transform.gameObject);
                }
            }

            foreach (GameObject obj in infoParents_OBJs)
                obj.SetActive(false);
        }

        public void PatientInfoResponseOnPlayerRoomInteraction(bool active)
        {
            patientInfoPage_BTN.interactable = active;

            if (!active)
            {
                ClosePatientInfoMenu();
            }
        }

        private void OpenPatientInfo()
        {
            patientPage.gameObject.SetActive(true);
        }

        private void OnFieldPatientSpawned(PatientRoot patient)
        {
            infoElements.Add(patient.GetPatientInfoElement);
            // UpdatePages();
        }

        /*
        private void NextPage()
        {
            if (currentPage < totalPages)
                currentPage++;

            else
                currentPage = 0;

            UpdatePages();
        }

        private void PreviousPage()
        {
            if (currentPage > 0)
                currentPage--;

            else
                currentPage = totalPages;

            UpdatePages();
        }
        */

        //private void OnPatientInfoElementRemoved(PatientRoot patient)
        //{
        //    infoElements.Remove(patient.GetPatientInfoElement);
        //    UpdatePages();
        //}

        /*
        private void UpdatePages()
        {
            totalPages = (infoElements.Count + PATIENTS_PER_PAGE - 1) / PATIENTS_PER_PAGE - 1;
            currentPage = Mathf.Clamp(currentPage, 0, totalPages);

            // Determine the range of elements to show
            int startIndex = currentPage * PATIENTS_PER_PAGE;
            int endIndex = Mathf.Min(startIndex + PATIENTS_PER_PAGE, infoElements.Count);

            for (int i = 0; i < infoElements.Count; i++)
            {
                if (i >= startIndex && i < endIndex)
                    infoElements[i].gameObject.SetActive(true);

                else
                    infoElements[i].gameObject.SetActive(false);
            }

            if (infoElements.Count == 0)
                pageStatusDisplay_TXT.text = "Geen data.";
            else
                pageStatusDisplay_TXT.text = (currentPage + 1).ToString() + "/" + (totalPages + 1).ToString();

            if (totalPages == 0)
            {
                nextPage_BTN.interactable = false;
                previousPage_BTN.interactable = false;
                pageStatusDisplay_TXT.text = "Geen patiënten.";
            }

            else
            {
                nextPage_BTN.interactable = true;
                previousPage_BTN.interactable = true;
            }
        }
        */
    }
}
