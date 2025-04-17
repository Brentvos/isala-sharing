using UnityEngine;

namespace New
{
    public class DialogueSO : ScriptableObject
    {
        [TextArea]
        [SerializeField] protected string dialogueTxt;
        [SerializeField] protected Option[] options;

        [SerializeField] protected bool hasSymptoms;
        [SerializeField] protected string[] symptoms;

        [SerializeField] protected bool hasDiagnose;
        [SerializeField] protected string diagnose;

        public string GetDialogueTxt(params object[] args)
        {
            return string.Format(dialogueTxt, args);
        }

        public Option[] GetOptions { get { return options; } }
        public bool HasSymptoms { get {  return hasSymptoms; } }
        public string[] GetSymptoms { get { return symptoms; } }
        public bool HasDiagnose { get { return hasDiagnose; } }
        public string GetDiagnose { get { return diagnose; } }
    }
}
