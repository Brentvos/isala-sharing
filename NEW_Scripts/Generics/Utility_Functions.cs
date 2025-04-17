using OLD;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace New
{
    public static class Utility_Functions
    {
        #region PatientInfo
        public static PatientInfo CreateBasicPatientInfo(LAST_NAME[] existingNames, int[] existingCodes, PATIENT_TYPE type)
        {
            int age = UnityEngine.Random.Range(25, 65);

            return new PatientInfo(GenerateUniqueCodeSimple(existingCodes),
                GetRandomWeight(BalancingManager.Instance.GetData.IS_WEIGHT_CALCULATION_SIMPLIFIED),
                age, GetUniqueName(existingNames), type);
        }

        public static int GetAvatarIndex(GENDER gender, int age)
        {
            if (age <= 35)
            {
                if (gender == GENDER.FEMALE)
                    return 0;

                else
                    return 1;
            }

            else if (age > 35 && age < 45)
            {
                if (gender == GENDER.FEMALE)
                    return 2;

                else
                    return 3;
            }

            else if (age >= 45 && age < 55)
            {
                if (gender == GENDER.FEMALE)
                    return 4;

                else
                    return 5;
            }

            else if (age >= 55 && age <= 65)
            {
                if (gender == GENDER.FEMALE)
                    return 6;

                else
                    return 7;
            }

            return 0;
        }

        private static LAST_NAME GetUniqueName(LAST_NAME[] existingNames)
        {
            Array allNames = Enum.GetValues(typeof(LAST_NAME));
            var availableNames = allNames.Cast<LAST_NAME>().Except(existingNames).ToList();

            System.Random random = new System.Random();
            return availableNames[random.Next(availableNames.Count)];
        }

        private static INFUUS_TYPE GetRandomInfuusType()
        {
            Array values = Enum.GetValues(typeof(INFUUS_TYPE));
            int randomIndex = UnityEngine.Random.Range(0, values.Length);
            return (INFUUS_TYPE)values.GetValue(randomIndex);
        }

        public static int GenerateCodeExclude(int codeToExclude)
        {
            System.Random random = new System.Random();
            int min = 100;
            int max = 199;

            int newNumber;

            do
            {
                newNumber = random.Next(min, max + 1);
            } while (newNumber == codeToExclude);

            return newNumber;
        }

        /// <summary>
        /// We use this for now, as we want every number to start with 1 and have a sequence of 3
        /// </summary>
        private static int GenerateUniqueCodeSimple(int[] existingCodes)
        {
            System.Random random = new System.Random();
            int min = 100;
            int max = 199;
            int newNumber;

            do
            {
                newNumber = random.Next(min, max + 1);
            } while (existingCodes.Contains(newNumber));

            return newNumber;
        }

        public static int GetRandomWeight(bool isSimple, int min = 50, int max = 120)
        {
            System.Random rand = new System.Random();
            int randomNumber = rand.Next(min, max + 1);  // +1 because max is inclusive

            if (isSimple)
                return (int)(Math.Round(randomNumber / 10.0) * 10);  // Round the number to the nearest multiple of 10

            else
                return randomNumber;
        }

        public static int GetRandomWeightExclude(int numberToExclude, bool isSimple, int min = 50, int max = 120)
        {
            System.Random rand = new System.Random();
            int randomNumber;

            do
            {
                randomNumber = rand.Next(min, max + 1);
            } while (randomNumber == numberToExclude);


            if (isSimple)
                return (int)(Math.Round(randomNumber / 10.0) * 10);  // Round the number to the nearest multiple of 10

            else
                return randomNumber;
        }

        public static GENDER GetGender()
        {
            Array values = Enum.GetValues(typeof(GENDER));
            System.Random random = new System.Random();
            
            return (GENDER)values.GetValue(random.Next(values.Length));
        }

        public static GENDER GetGenderBasedOnName(LAST_NAME name)
        {
            int totalNames = System.Enum.GetValues(typeof(LAST_NAME)).Length;
            int midpoint = totalNames / 2;

            int nameIndex = (int)name;

            if (nameIndex < midpoint)
                return GENDER.MALE;

            else
                return GENDER.FEMALE;
        }

        private static int GetRandomSugarLevel(int min = 7, int max = 20)
        {
            return UnityEngine.Random.Range(min, max);
        }

        /// <summary>
        /// This can be used later if we want to have variable sequences. Right now we don't, because we just use a number between 100-199
        /// </summary>
        private static int GenerateUniqueCodeCustomLength(int[] existingCodes, int sequenceLength = 3)
        {
            List<int> tempCode = new List<int>();
            string codeInStringFormat = string.Empty;

            while (tempCode.Count <= sequenceLength)
            {
                if (tempCode.Count == sequenceLength)
                {
                    if (existingCodes.Contains(int.Parse(codeInStringFormat)))
                        tempCode.Clear();

                    else
                        break;
                }

                tempCode.Add(UnityEngine.Random.Range(1, 9));
                codeInStringFormat += tempCode[tempCode.Count - 1];
            }

            return int.Parse(codeInStringFormat);
        }
        #endregion

        #region Rooms

        public static PatientRoom GetRandomAvailableRoom()
        {
            PatientRoom[] availableRooms = GameObject.FindObjectsOfType<PatientRoom>()
                .Where(r => r.HasSpace()).ToArray();

            return availableRooms[UnityEngine.Random.Range(0, availableRooms.Length - 1)];      
        }

        public static RoomRoot[] GetAllRoomsOfType(ROOM_TYPE type)
        {
            return GameObject.FindObjectsOfType<RoomRoot>()
                .Where(t => t.GetRoomType == type)
                .ToArray();
        }

        public static PatientRoom[] GetAllPatientRoomsWithUrgency(URGENCY_TYPE urgency)
        {
            RoomRoot[] allPatientRooms = GetAllRoomsOfType(ROOM_TYPE.PATIENT);

            return allPatientRooms
                .OfType<PatientRoom>()
                .Where(room => room.GetUrgencyLevel == urgency)
                .ToArray();
        }
        #endregion


        public static string SecondsToPrintableFormat(int seconds)
        {
            int mins = seconds / 60;
            int secs = seconds % 60;

            return $"{mins:D2}:{secs:D2}";
        }

        public static int GetTimePassedFromTimer(int totalTime, float currentTime)
        {
            return Mathf.CeilToInt(totalTime - currentTime);
        }


        public static int GetNumberDeviated(int number, int deviation, bool onlyUp = false, bool onlyDown = false)
        {
            if (onlyUp)
                return UnityEngine.Random.Range(number, number + deviation); 

            if (onlyDown)
                return UnityEngine.Random.Range(number -deviation, number);

            return UnityEngine.Random.Range(number - deviation, number + deviation);
        }
    }
}
