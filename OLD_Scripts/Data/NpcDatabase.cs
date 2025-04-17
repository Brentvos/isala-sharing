using System;
using System.Collections.Generic;
using UnityEngine;

namespace OLD
{


    /// <summary>
    /// https://www.youtube.com/watch?v=CO_DK75XOl4&list=PL5KbKbJ6Gf99mcmE1ptsn0oXO1_vnKDlS
    /// https://www.youtube.com/watch?v=59RBOBbeJaA
    /// 
    /// I want to create a database of Npc data
    /// I want to get a list of names from the internet and put it into a parsable array
    /// </summary>
    public class NpcDatabase : MonoBehaviour
    {
        [SerializeField] private bool generateNursesOnStart, generatePatientsOnStart;

        private List<int> generatedCodes = new List<int>();

        [Space(10)]

        // !!!!!!!!!!!!! The DelegationGameManager generates SPAWN-POINTS, the AMOUNT should be EQUAL to THIS
        [SerializeField] private int amountOfNursesToGenerate, amountOfPatientsToGenerate;
        [Space(10)]
        [SerializeField] private List<NurseData> nurses = new List<NurseData>(); // Only serialized for TRACKING
        [Space(10)]
        [SerializeField] private List<PatientData> patients = new List<PatientData>(); // Only serialized for TRACKING

        public List<NurseData> GetAllNurses { get { return nurses; } }
        public List<PatientData> GetAllPatients { get { return patients; } }

        public event Action<List<NurseData>> OnNursesGenerated;
        public event Action<List<PatientData>> OnPatientsGenerated;

        public static NpcDatabase Instance;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;

            Invoke("GenerateNPCs", 0.1f);
        }

        private void GenerateNPCs()
        {
            if (generateNursesOnStart)
                GenerateNurses();

            if (generatePatientsOnStart)
                GeneratePatients();
        }

        private void GenerateNurses()
        {
            int currentNurseAmount = 0;

            while (currentNurseAmount < amountOfNursesToGenerate)
            {
                nurses.Add(new NurseData(GenerateFirstName(), GenerateAge(10, 90), GenerateCode(3)));
                currentNurseAmount++;
            }

            OnNursesGenerated?.Invoke(nurses);
        }

        private void GeneratePatients()
        {
            int currentPatientAmount = 0;
            bool hasSpawnedInsulinPatient = false;

            while (currentPatientAmount < amountOfPatientsToGenerate)
            {
                if (!hasSpawnedInsulinPatient)
                {
                    patients.Add(new PatientData(GenerateFirstName(), GenerateAge(),
                        GenerateCode(3), GenerateSugarLevels(), GenerateWeight(), IsInfoHidden(0), IsInfoHidden(0)));

                    hasSpawnedInsulinPatient = true;
                }

                else
                    patients.Add(new PatientData(GenerateFirstName(), GenerateAge(), GenerateCode(3)));

                currentPatientAmount++;
            }

            OnPatientsGenerated?.Invoke(patients);
        }

        private int GenerateAge(int min = 20, int max = 60)
        {
            return UnityEngine.Random.Range(min, max);
        }

        private int GenerateSugarLevels(int min = 7, int max = 20)
        {
            return UnityEngine.Random.Range(min, max);
        }

        private int GenerateWeight(int min = 60, int max = 120)
        {
            System.Random rand = new System.Random();
            int randomNumber = rand.Next(min, max + 1);  // +1 because max is inclusive

            // Round the number to the nearest multiple of 10
            return (int)(Math.Round(randomNumber / 10.0) * 10);
        }

        private bool IsInfoHidden(int chanceToBeHidden, bool guaranteeHidden = false)
        {
            if (guaranteeHidden || chanceToBeHidden == 100)
                return true;

            else if (!guaranteeHidden && chanceToBeHidden == 0)
                return false;

            int randomNumber = UnityEngine.Random.Range(0, 100);

            if (randomNumber <= chanceToBeHidden)
                return true;

            else
                return false;
        }

        private First_Names GenerateFirstName()
        {
            return (First_Names)UnityEngine.Random.Range(0, Enum.GetValues(typeof(First_Names)).Length);
        }

        private int GenerateCode(int sequenceLength)
        {
            List<int> tempCode = new List<int>();
            string codeInStringFormat = string.Empty;

            while (tempCode.Count <= sequenceLength)
            {
                if (tempCode.Count == sequenceLength)
                {
                    if (generatedCodes.Contains(int.Parse(codeInStringFormat)))
                        tempCode.Clear();

                    else
                        break;
                }

                tempCode.Add(UnityEngine.Random.Range(1, 9));
                codeInStringFormat += tempCode[tempCode.Count - 1];
            }

            generatedCodes.Add(int.Parse(codeInStringFormat));
            return int.Parse(codeInStringFormat);

        }
    }
}