using System;
using UnityEngine;

namespace New
{
    [Serializable]
    public class Option
    {
        [Tooltip("Not more than 3 answer options!")]
        [SerializeField] private string text;
        [SerializeField] private SCORE_TYPES scoreType;

        public string GetText { get { return text; } }
        public SCORE_TYPES GetScoreType { get { return scoreType; } }
    }
}
