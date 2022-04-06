using System.Collections.Generic;
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
        List<TMP_Dropdown.OptionData> opts = new List<TMP_Dropdown.OptionData>() {
            new TMP_Dropdown.OptionData("The Grid"),
            new TMP_Dropdown.OptionData("Ring Of Galaxy"),
            new TMP_Dropdown.OptionData("Suzaku Castle"),
            new TMP_Dropdown.OptionData("AirForce Base"),
            new TMP_Dropdown.OptionData("FANG Theme"),
            new TMP_Dropdown.OptionData("Karin Theme"),
            new TMP_Dropdown.OptionData("Nash Theme"),
        };
        bgmSelector.AddOptions(opts);
        bgmSelector.onValueChanged.AddListener(OnDropdownSelect);

        GameObject BGM = soundSystem.transform.Find("BGM").gameObject;
        theGridTheme = BGM.transform.Find("TheGridTheme").gameObject.GetComponent<AudioSource>();
        ringOfGalaxyTheme = BGM.transform.Find("RingOfGalaxyTheme").gameObject.GetComponent<AudioSource>();
        suzakuCastleTheme = BGM.transform.Find("SuzakuCastleTheme").gameObject.GetComponent<AudioSource>();
        airForceBaseTheme = BGM.transform.Find("AirForceBaseTheme").gameObject.GetComponent<AudioSource>();
        fangTheme = BGM.transform.Find("FangTheme").gameObject.GetComponent<AudioSource>();
        karinTheme = BGM.transform.Find("KarinTheme").gameObject.GetComponent<AudioSource>();
        nashTheme = BGM.transform.Find("NashTheme").gameObject.GetComponent<AudioSource>();

        currentlyPlayingTheme = theGridTheme;
        theGridTheme.Play();
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
        currentlyPlayingTheme.Stop();
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
