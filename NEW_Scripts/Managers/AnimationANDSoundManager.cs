
using UnityEngine;

namespace New
{
    public class AnimationANDSoundManager : MonoBehaviour
    {
        [Header("Beeper")]
        [SerializeField] private Animator beeperActivate_ANIM;
        [SerializeField] private AudioSource beeperActivate_AUDIOSOURCE;
        [SerializeField] private AudioSource background_AUDIOSOURCE;


        public Animator GetBeeperActivate_ANIM { get {  return beeperActivate_ANIM; } }
        public AudioSource GetBeeperActivate_AUDIOSOURCE { get {  return beeperActivate_AUDIOSOURCE; } }

        private float beeperOriginalVolume;
        private float backgroundOriginalVolume;
        public float GetBeeperOriginalVolume { get { return beeperOriginalVolume; } }
        public float GetBackgroundOriginalVolume { get { return backgroundOriginalVolume; } }

        public static AnimationANDSoundManager instance;

        private void Awake()
        {
            if (instance != this)
                instance = this;


            beeperOriginalVolume = beeperActivate_AUDIOSOURCE.volume;
            backgroundOriginalVolume = background_AUDIOSOURCE.volume;

            GameManager.Instance.SoundButtonPressed += Instance_SoundButtonPressed;
        }

        private void Instance_SoundButtonPressed()
        {
            if (GameManager.Instance.SFXEnabled)
                beeperActivate_AUDIOSOURCE.volume = beeperOriginalVolume;

            else
                beeperActivate_AUDIOSOURCE.volume = 0;

            if (GameManager.Instance.backgroundSFXEnabled)
                background_AUDIOSOURCE.volume = backgroundOriginalVolume;

            else
                background_AUDIOSOURCE.volume = 0;
        }

        public void ResetRect(RectTransform rect, bool resetRot)
        {
            if (resetRot)
            {
                rect.rotation = Quaternion.identity;
            }
        }

        public void Animate(Animator anim, bool enable, float speed)
        {
            anim.enabled = enable;
            anim.speed = speed;
        }

        public void Sound(AudioSource source, bool enable) 
        {
            if (enable)
                source.Play();

            else
                source.Stop();
        }
    }
}
