using TMPro;
using UnityEngine;

public class CounterHit : MonoBehaviour
{
    [SerializeField]
    public TMP_Text counterHitText;

    private const int counterHitPercentage = 50;
    private const int counterHitTextFrames = 100;

    private int counterHitTextFrame = 0;

    private static System.Random rnd = new System.Random();

    // Start is called before the first frame update
    void Start()
    {
        counterHitText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (counterHitText.gameObject.activeInHierarchy) {
            counterHitTextFrame += 1;
            if (counterHitTextFrame >= counterHitTextFrames) {
                counterHitTextFrame = 0;
                counterHitText.gameObject.SetActive(false);
            }
        }
    }

    public void UpdateCounterHitTextOnHit() {
        if (rnd.Next(1, 100) <= counterHitPercentage) {
            counterHitText.gameObject.SetActive(true);
        }
    }
}
