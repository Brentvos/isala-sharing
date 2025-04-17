using UnityEngine;

namespace New
{
    [CreateAssetMenu(fileName = "New Case", menuName = "Dialogue/Add case")]
    public class CaseSO : DialogueSO
    {
        // public DialogueSO returningToTaskDialogue;
        [SerializeField] private IntakeSO intakeDialogue;

        public IntakeSO GetIntakeDialogue { get { return intakeDialogue; } }
        // public DialogueSO failedToTakeBeeperDialogue; // Maybe from somewhere else?
        // public DialogueSO patientHealthToZeroDialogue; // Maybe from somewhere else?
        // public DialogueSO patientHelpedDialogue; // Maybe from somewhere else?
        // public DialogueSO succeedDialogue;
    }
}
