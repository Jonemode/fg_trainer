using UnityEngine;

public class OpponentStun : MonoBehaviour
{
    public GameObject stunBar;
    public GameObject stunBarBackground;

    private RectTransform stunBarRT;

    // Using Street Fighter units
    private const float maxStun = 1000;
    private const float stunIncrement = 100;

    // Using world space units
    private float stunBarMaxPts;
    private float stunBarIncrementPts;
    private const float stunBarPaddingPts = 3;

    private const int stunRetainmentFrames = 100;
    private const float stunDecay = 1.6f;

    public int StunRetainmentFrame = stunRetainmentFrames;

    // Start is called before the first frame update
    void Start()
    {
        stunBarRT = stunBar.GetComponent<RectTransform>();
        RectTransform stunBarBackgroundRT = stunBarBackground.GetComponent<RectTransform>();
        stunBarMaxPts = stunBarBackgroundRT.rect.width - (2 * stunBarPaddingPts);
        stunBarIncrementPts = (stunIncrement / maxStun) * stunBarMaxPts;

        // Start the stun bar at 0
        stunBarRT.offsetMax = new Vector2(-stunBarBackgroundRT.rect.width + stunBarPaddingPts, stunBarRT.offsetMax.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (stunBarRT.rect.width > 0 && StunRetainmentFrame == stunRetainmentFrames) {
            float newX = stunBarRT.offsetMax.x - stunDecay;
            stunBarRT.offsetMax = new Vector2(newX, stunBarRT.offsetMax.y);
        }

        if (StunRetainmentFrame < stunRetainmentFrames) {
            StunRetainmentFrame++;
        }
    }

    public void UpdateStunOnHit() {
        float currentOffsetMax = stunBarRT.offsetMax.x;
        float newX;
        if (stunBarRT.rect.width + stunBarIncrementPts > stunBarMaxPts) {
            newX = -stunBarPaddingPts;
        } else {
            newX = currentOffsetMax + stunBarIncrementPts;
        }
        stunBarRT.offsetMax = new Vector2(newX, stunBarRT.offsetMax.y);
    }
}
