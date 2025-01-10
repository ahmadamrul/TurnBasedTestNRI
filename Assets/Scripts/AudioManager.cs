using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource audioSource;
    public Slider volumeSlider;

    public Image musicOnImage;
    public Image musicOffImage;

    public Image sfxOnImage;
    public Image sfxOffImage;
    public Button[] muteButtons;
    public Button[] muteSfxButtons;
    private bool isMuted;
    private bool sfxIsMuted;

    SfxManager sfxManager;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (sfxManager == null)
        {
            sfxManager = GetComponentInChildren<SfxManager>();
        }
        if (sfxManager != null)
        {
            sfxIsMuted = sfxManager.audioSource.mute;
            UpdateSfxButtonImage();
        }
        if (audioSource != null && volumeSlider != null)
        {
            volumeSlider.value = audioSource.volume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
        if (muteButtons != null && muteButtons.Length > 0)
        {
            foreach (Button btn in muteButtons)
            {
                btn.onClick.AddListener(ToggleMute);
            }
        }
        if (muteSfxButtons != null && muteSfxButtons.Length > 0)
        {
            foreach (Button btn in muteSfxButtons)
            {
                btn.onClick.AddListener(ToggleSfxMute);
            }
        }


        isMuted = audioSource.mute;
        UpdateButtonImage();


        sfxIsMuted = sfxManager.audioSource.mute;
        UpdateSfxButtonImage();
    }

    public void SetVolume(float volume)
    {
        if (audioSource != null)
        {
            audioSource.volume = volume;
            if (sfxManager != null)
            {
                sfxManager.UpdateVolume(volume);
            }
        }
    }

    public void ToggleMute()
    {
        isMuted = !isMuted;
        audioSource.mute = isMuted;
        UpdateButtonImage();
    }
    public void ToggleSfxMute()
    {
        sfxIsMuted = !sfxIsMuted;
        sfxManager.audioSource.mute = sfxIsMuted;
        UpdateSfxButtonImage();
    }
    private void UpdateSfxButtonImage()
    {
        if (sfxOnImage != null && sfxOffImage != null)
        {
            sfxOnImage.gameObject.SetActive(!sfxIsMuted);
            sfxOffImage.gameObject.SetActive(sfxIsMuted);
        }
    }
    private void UpdateButtonImage()
    {
        if (musicOnImage != null && musicOffImage != null)
        {
            musicOnImage.gameObject.SetActive(!isMuted);
            musicOffImage.gameObject.SetActive(isMuted);
        }
    }
}
