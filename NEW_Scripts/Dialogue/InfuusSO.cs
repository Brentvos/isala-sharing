using UnityEngine;

namespace New
{
    [CreateAssetMenu(fileName = "New Infuus", menuName = "Dialogue/Add infuus")]
    public class InfuusSO : DialogueSO
    {
        [SerializeField] private IntakeSO intakeDialogue;
        public IntakeSO GetIntakeDialogue { get { return intakeDialogue; } }
    }
}
