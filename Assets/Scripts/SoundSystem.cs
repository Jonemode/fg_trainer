using UnityEngine;

public class SoundSystem : MonoBehaviour
{
    // Attack sounds
    public AudioSource normalAttackHit;
    public AudioSource specialAttackHit;

    // Voices
    public AudioSource danHitVoice;
    public AudioSource danHitSpecialVoice;
    public AudioSource karinAttackVoice;
    public AudioSource karinSpecialAttackVoice;

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
