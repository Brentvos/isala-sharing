using OLD;
using UnityEditor;
using UnityEngine;


namespace New
{
    /// <summary>
    /// These are the patients that go through the 'normal' process
    /// </summary>
    public class PatientDefault : PatientRoot
    {
        public override void Initialize(PatientInfo basicInfo)
        {
            base.Initialize(basicInfo);
        }
    }
}