using TMPro;
using UnityEngine;

public class SoundSystem : MonoBehaviour
{
    // Attack sounds
    [SerializeField]
    public AudioSource normalAttackHit;

    [SerializeField]
    public AudioSource specialAttackHit;

    // Voices
    [SerializeField]
    public AudioSource danHitVoice;

    [SerializeField]
    public AudioSource danHitSpecialVoice;

    [SerializeField]
    public AudioSource karinAttackVoice;

    [SerializeField]
    public AudioSource karinSpecialAttackVoice;

    [SerializeField]
    public GameObject soundSystem;

    [SerializeField]
    public TMP_Dropdown bgmSelector;

    private AudioSource theGridTheme;
    private AudioSource ringOfGalaxyTheme;
    private AudioSource suzakuCastleTheme;
    private AudioSource airForceBaseTheme;
    private AudioSource fangTheme;
    private AudioSource karinTheme;
    private AudioSource nashTheme;

    private AudioSource currentlyPlayingTheme;

    void Start() {
        GameObject BGM = soundSystem.transform.Find("BGM").gameObject;
        theGridTheme = BGM.transform.Find("TheGridTheme").gameObject.GetComponent<AudioSource>();
        ringOfGalaxyTheme = BGM.transform.Find("RingOfGalaxyTheme").gameObject.GetComponent<AudioSource>();
        suzakuCastleTheme = BGM.transform.Find("SuzakuCastleTheme").gameObject.GetComponent<AudioSource>();
        airForceBaseTheme = BGM.transform.Find("AirForceBaseTheme").gameObject.GetComponent<AudioSource>();
        fangTheme = BGM.transform.Find("FangTheme").gameObject.GetComponent<AudioSource>();
        karinTheme = BGM.transform.Find("KarinTheme").gameObject.GetComponent<AudioSource>();
        nashTheme = BGM.transform.Find("NashTheme").gameObject.GetComponent<AudioSource>();

        System.Random rnd = new System.Random();
        bgmSelector.value = rnd.Next(0, 7);
        OnDropdownSelect(bgmSelector.value);
        bgmSelector.onValueChanged.AddListener(OnDropdownSelect);
    }

    private void OnDropdownSelect(int e) {
        switch (e) {
            case 0:
                PlayTheme(theGridTheme);
                break;
            case 1:
                PlayTheme(ringOfGalaxyTheme);
                break;
            case 2:
                PlayTheme(suzakuCastleTheme);
                break;
            case 3:
                PlayTheme(airForceBaseTheme);
                break;
            case 4:
                PlayTheme(fangTheme);
                break;
            case 5:
                PlayTheme(karinTheme);
                break;
            case 6:
                PlayTheme(nashTheme);
                break;
        }
    }

    private void PlayTheme(AudioSource newTheme) {
        if (currentlyPlayingTheme != null) {
            currentlyPlayingTheme.Stop();
        }
        newTheme.Play();
        currentlyPlayingTheme = newTheme;
    }

    public void PlayNormalAttackHit() {
        normalAttackHit.Play();
    }

    public void PlaySpecialAttackHit() {
        specialAttackHit.Play();
    }

    public void PlayDanHitVoice() {
        danHitVoice.Play();
    }

    public void PlayDanSpecialHitVoice() {
        danHitSpecialVoice.Play();
    }

    public void PlayKarinAttackVoice() {
        karinAttackVoice.Play();
    }

    public void PlayKarinSpecialAttackVoice() {
        karinSpecialAttackVoice.Play();
    }
}
