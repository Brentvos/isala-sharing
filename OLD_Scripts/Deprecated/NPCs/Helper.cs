using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OLD
{

    public class Helper : MonoBehaviour
    {
        [SerializeField] private NurseInteractiveMoraleElement morale;
        [SerializeField] private HelperProfileDisplayComponent profileDisplay;

        public HelperProfileDisplayComponent GetProfileDisplay { get { return profileDisplay; } }
        public NurseInteractiveMoraleElement GetMorale { get { return morale; } }

        [SerializeField] private NpcProfile profile;
        public NpcProfile GetProfile { get { return profile; } }

        [SerializeField] private HelperState state;
        public HelperState GetHelperState { get { return state; } }

        private void Awake()
        {
            state = HelperState.InDatabase;
            // profile.RandomizeNpcProfile(NpcDatabase.Instance.GetNpcs.IndexOf(this));
        }

        public void Initialize()
        {
            profile.RandomizeNpcProfile();
            state = HelperState.Default;

            if (profileDisplay == null)
                profileDisplay = GetComponent<HelperProfileDisplayComponent>();

            if (profileDisplay != null)
                profileDisplay.Initialize(this);

            if (morale == null)
                morale = GetComponent<NurseInteractiveMoraleElement>();

            //if (morale != null)
            //    morale.Initialize(profile.GetName.ToString());
        }

        public enum HelperState
        {
            InDatabase, // Used to catch them and put them on playing field
            Default, // When nothing is happening
            OpenForTreatment,
            InTreatment,
            RequiredAdditionalTreatment,
            RecentlyTreated
        }
    }
}