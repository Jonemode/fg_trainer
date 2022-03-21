using UnityEngine;

public class SoundSystem : MonoBehaviour
{
    public AudioSource normalAttackHit;
    public AudioSource specialAttackHit;

    public void PlayNormalAttackHit() {
        normalAttackHit.Play();
    }

    public void PlaySpecialAttackHit() {
        specialAttackHit.Play();
    }
}
