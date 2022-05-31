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
    public AudioSource girlfriendPunch;

    [SerializeField]
    public AudioSource girlfriendRoundOneFight;

    [SerializeField]
    public GameObject soundSystem;

    [SerializeField]
    public TMP_Dropdown bgmSelector;

    private AudioSource nashTheme;

    private AudioSource currentlyPlayingTheme;

    private List<AudioSource> attackList;
    private List<AudioSource> levelDownList;
    private List<AudioSource> levelUpList;

    private static System.Random rnd = new System.Random();

    void Start() {
        /*
        GameObject BGM = soundSystem.transform.Find("BGM").gameObject;
        nashTheme = BGM.transform.Find("NashTheme").gameObject.GetComponent<AudioSource>();

        System.Random rnd = new System.Random();
        bgmSelector.value = rnd.Next(0, 7);
        OnDropdownSelect(bgmSelector.value);
        bgmSelector.onValueChanged.AddListener(OnDropdownSelect);
        */

        // Girlfriend
        GameObject voice = soundSystem.transform.Find("Voice").gameObject;
        GameObject attacks = voice.transform.Find("Attacks").gameObject;
        GameObject levelDowns = voice.transform.Find("LevelDown").gameObject;
        GameObject levelUps = voice.transform.Find("LevelUp").gameObject;
        attackList = new List<AudioSource>();
        foreach (Transform attack in attacks.transform) {
            attackList.Add(attack.GetComponent<AudioSource>());
        }
        levelDownList = new List<AudioSource>();
        foreach (Transform levelDown in levelDowns.transform) {
            levelDownList.Add(levelDown.GetComponent<AudioSource>());
        }
        levelUpList = new List<AudioSource>();
        foreach (Transform levelUp in levelUps.transform) {
            levelUpList.Add(levelUp.GetComponent<AudioSource>());
        }
    }

    private void OnDropdownSelect(int e) {
        switch (e) {
            case 0:
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
        int i = rnd.Next(0, attackList.Count);
        attackList[i].Play();
    }

    public void PlayKarinSpecialAttackVoice() {
        girlfriendPunch.Play();
    }

    public void PlayLevelDown() {
        int i = rnd.Next(0, levelDownList.Count);
        levelDownList[i].Play();
    }

    public void PlayLevelUp() {
        int i = rnd.Next(0, levelUpList.Count);
        levelUpList[i].Play();
    }
}
