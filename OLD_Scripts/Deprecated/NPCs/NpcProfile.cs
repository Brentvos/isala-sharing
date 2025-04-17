using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace OLD
{


    [Serializable]
    public class NpcProfile
    {
        public List<Stat> stats = new List<Stat>();

        [SerializeField] private int code;
        [SerializeField] private int age;
        [SerializeField] private Names name;
        //[SerializeField] private Gender gender;
        //[SerializeField] private Liking liking;
        //[SerializeField] private Disliking disliking;

        /// <summary>
        /// TO DO: Change this later, cannot use constructor with randomizer though I think?
        /// </summary>
        public void RandomizeNpcProfile()
        {
            name = (Names)UnityEngine.Random.Range(0, Enum.GetValues(typeof(Names)).Length);
            age = UnityEngine.Random.Range(10, 90);
            code = RandomCodeGenerator(5);


            //gender = (Gender) UnityEngine.Random.Range(0, Enum.GetValues(typeof(Gender)).Length);
            //liking = (Liking) UnityEngine.Random.Range(0, Enum.GetValues(typeof(Liking)).Length);
            //disliking = (Disliking) UnityEngine.Random.Range(0, Enum.GetValues(typeof(Disliking)).Length);
        }

        public int RandomCodeGenerator(int sequenceLength)
        {
            List<int> tempCode = new List<int>();
            string codeInStringFormat = string.Empty;

            while (tempCode.Count < sequenceLength)
            {
                tempCode.Add(UnityEngine.Random.Range(1, 9));
                codeInStringFormat += tempCode[tempCode.Count - 1];
            }

            return int.Parse(codeInStringFormat);
        }

        public int GetCode { get { return code; } }
        public Names GetName { get { return name; } }
        //public Gender GetGender { get {  return gender; } }
        //public Liking GetLiking { get {  return liking; } }
        //public Disliking GetDisliking { get { return disliking; } }
    }

    public class Stat
    {
        public bool isFilled;
        public string data;
    }

    public enum Names
    {
        Alison,
        Bert,
        Chris,
        Danielle,
        Edward
    }
    public enum Liking
    {
        Dogs,
        Cats,
        Cows,
        Rats
    }

    public enum Disliking
    {
        Spiders,
        Ants,
        Heights,
        Fish
    }

    public enum Gender
    {
        Male,
        Female
    }
}