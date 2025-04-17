using UnityEngine;
using UnityEngine.UI;

namespace New
{
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager Instance;

        [SerializeField] private Sprite[] nurseSprites;

        [SerializeField] private DialogueDatabaseSO basicDialogueDatabase;
        [SerializeField] private DialogueDatabaseSO symptomDialogueDatabase;
        [SerializeField] private DialogueDatabaseSO infuusDialogueDatabase;


        [SerializeField] private CodeSO genericCodeSO;
        [SerializeField] private InfuusSO genericInfuusWeightSO;
        public CodeSO GetGenericCodeSO { get { return genericCodeSO; } }
        public InfuusSO GetGenericInfuusWeightSO { get { return genericInfuusWeightSO; } }

        private void Awake()
        {
            if (Instance != this)
                Instance = this;
        }

        public CaseSO GetCase(PATIENT_TYPE type)
        {
            switch (type)
            {
                case PATIENT_TYPE.DEFAULT:
                    return basicDialogueDatabase.GetCases[UnityEngine.Random.Range(0, basicDialogueDatabase.GetCases.Count)];

                case PATIENT_TYPE.SYMPTOM:
                    return symptomDialogueDatabase.GetCases[UnityEngine.Random.Range(0, symptomDialogueDatabase.GetCases.Count)];

                case PATIENT_TYPE.INFUUS:
                    return infuusDialogueDatabase.GetCases[UnityEngine.Random.Range(0, infuusDialogueDatabase.GetCases.Count)];
            }

            return basicDialogueDatabase.GetCases[UnityEngine.Random.Range(0, basicDialogueDatabase.GetCases.Count)];
        }

        public Sprite GetRandomNurseSprite()
        {
            return nurseSprites[Random.Range(0, nurseSprites.Length - 1)];
        }
    }
}