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
    public AudioSource girlfriendPunch;

    [SerializeField]
    public AudioSource girlfriendRoundOneFight;

    [SerializeField]
    public GameObject soundSystem;

    [SerializeField]
    public TMP_Dropdown bgmSelector;

    [SerializeField]
    public TMP_Dropdown soundModeSelector;

    private AudioSource theGridTheme;
    private AudioSource ringOfGalaxyTheme;
    private AudioSource suzakuCastleTheme;
    private AudioSource airForceBaseTheme;
    private AudioSource fangTheme;
    private AudioSource karinTheme;
    private AudioSource nashTheme;

    private AudioSource currentlyPlayingTheme;

    private List<AudioSource> girlfriendAttackList;
    private List<AudioSource> girlfriendLevelDownList;
    private List<AudioSource> girlfriendLevelUpList;

    private SoundMode currentSoundMode;

    private static System.Random rnd = new System.Random();

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

        currentSoundMode = SoundMode.Girlfriend;
        soundModeSelector.onValueChanged.AddListener((e) => {
            currentSoundMode = (SoundMode)e;
        });

        // Girlfriend
        GameObject gf = soundSystem.transform.Find("Voice").transform.Find("Girlfriend").gameObject;
        GameObject gfAttacks = gf.transform.Find("Attacks").gameObject;
        GameObject gfLevelDown = gf.transform.Find("LevelDown").gameObject;
        GameObject gfLevelUp = gf.transform.Find("LevelUp").gameObject;
        girlfriendAttackList = new List<AudioSource>();
        foreach (Transform attack in gfAttacks.transform) {
            girlfriendAttackList.Add(attack.GetComponent<AudioSource>());
        }
        girlfriendLevelDownList = new List<AudioSource>();
        foreach (Transform levelDown in gfLevelDown.transform) {
            girlfriendLevelDownList.Add(levelDown.GetComponent<AudioSource>());
        }
        girlfriendLevelUpList = new List<AudioSource>();
        foreach (Transform levelUp in gfLevelUp.transform) {
            girlfriendLevelUpList.Add(levelUp.GetComponent<AudioSource>());
        }
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
        if (currentSoundMode == SoundMode.Normal) {
            karinAttackVoice.Play();
        } else {
            int i = rnd.Next(0, girlfriendAttackList.Count);
            girlfriendAttackList[i].Play();
        }
    }

    public void PlayKarinSpecialAttackVoice() {
        if (currentSoundMode == SoundMode.Normal) {
            karinSpecialAttackVoice.Play();
        } else {
            girlfriendPunch.Play();
        }
    }

    public void PlayLevelDown() {
        if (currentSoundMode != SoundMode.Normal) {
            int i = rnd.Next(0, girlfriendLevelDownList.Count);
            girlfriendLevelDownList[i].Play();
        }
    }

    public void PlayLevelUp() {
        if (currentSoundMode != SoundMode.Normal) {
            int i = rnd.Next(0, girlfriendLevelUpList.Count);
            girlfriendLevelUpList[i].Play();
        }
    }
}
