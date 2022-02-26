using UnityEngine;

public class SoundSystem : MonoBehaviour
{
    public AudioSource normalAttackHit;

    public void PlayNormalAttackHit() {
        normalAttackHit.Play();
    }
}
