using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace New   
{

    public class CharacterCreator : MonoBehaviour
    {
        public static CharacterCreator instance;

        public Sprite[] uiAvatarsZoomIn;
        public Sprite[] uiAvatarsZoomOut;
        public Sprite[] fieldAvatars;
        public Button[] avatarButtons;

        public TMP_InputField nameInputField;
        public Button playButton;
        public Image selectedCharacter;

        private string playerName;
        private int selectedAvatarIndex = 0;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            for (int i = 0; i < avatarButtons.Length; i++)
            {
                int index = i;
                avatarButtons[i].onClick.AddListener(() => SelectAvatar(index));
                avatarButtons[i].gameObject.GetComponent<Image>().sprite = fieldAvatars[i];
            }

            nameInputField.onValueChanged.AddListener(ValidateName);
            selectedCharacter.gameObject.SetActive(true);
        }

        private void SelectAvatar(int index)
        {
            selectedAvatarIndex = index;
            selectedCharacter.gameObject.SetActive(true);
            selectedCharacter.sprite = uiAvatarsZoomIn[selectedAvatarIndex];
            CheckIfReadyToPlay();
        }

        private void ValidateName(string input)
        {
            playButton.onClick.RemoveAllListeners();

            if (input.Length >= 3 && input.Length <= 16 && System.Text.RegularExpressions.Regex.IsMatch(input, @"^[a-zA-Z]+$"))
            {
                playerName = input;
                playButton.onClick.AddListener(StartGame);
                CheckIfReadyToPlay();
            }
            else
            {
                playButton.onClick.AddListener(Error);
            }
        }

        private void CheckIfReadyToPlay()
        {
            playButton.interactable = !string.IsNullOrEmpty(playerName);
        }

        private void StartGame()
        {
            PlayerStatsManager.instance.InitializePlayer(playerName, selectedAvatarIndex, uiAvatarsZoomIn[selectedAvatarIndex],
                uiAvatarsZoomOut[selectedAvatarIndex], fieldAvatars[selectedAvatarIndex]);
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        }

        private void Error()
        {
            nameInputField.gameObject.GetComponent<Image>().DOColor(new Color(1, .72f, .72f), 0.25f).OnComplete(() => ResetColor());
        }

        private void ResetColor()
        {
            nameInputField.gameObject.GetComponent<Image>().DOColor(Color.white, 0.05f);
        }
    }
}