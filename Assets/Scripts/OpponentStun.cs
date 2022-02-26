using UnityEngine;

public class OpponentStun : MonoBehaviour
{
    public GameObject stunBar;
    public GameObject stunBarBackground;

    private RectTransform stunBarRT;

    // Using Street Fighter units
    private const float maxStun = 1000;
    private float currentStun = 0;

    // Using world space units
    private float stunBarMaxPts;
    private const float stunBarPaddingPts = 6;

    private const int stunRetainmentFrames = 100;
    private const float stunDecay = 1.6f;

    // StunRetainmentFrame default value is high. Set it low and it will grow back high.
    public int StunRetainmentFrame = stunRetainmentFrames;

    // Start is called before the first frame update
    void Start()
    {
        stunBarRT = stunBar.GetComponent<RectTransform>();

        // Set the left offset (padding)
        stunBarRT.offsetMin = new Vector2(stunBarPaddingPts, stunBarRT.offsetMin.y);

        RectTransform stunBarBackgroundRT = stunBarBackground.GetComponent<RectTransform>();
        stunBarMaxPts = stunBarBackgroundRT.rect.width - (2 * stunBarPaddingPts);

        redrawStunBar();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentStun > 0 && StunRetainmentFrame == stunRetainmentFrames) {
            currentStun -= stunDecay;
            if (currentStun < 0) {
                currentStun = 0;
            }
            redrawStunBar();
        }

        if (StunRetainmentFrame < stunRetainmentFrames) {
            StunRetainmentFrame++;
        }
    }

    public void UpdateStunOnHit(int stunAmount) {
        currentStun += stunAmount;
        if (currentStun > maxStun) {
            currentStun = maxStun;
        }

        redrawStunBar();
    }

    public void redrawStunBar() {
        // The offsetMax should be the distance in world points from the right side of the bar to the stun level
        // [ [----currentStun--]<---offsetMax---->]
        //  ^                           ^
        //  1x padding              includes 1x padding
        float newX = ((currentStun / maxStun) * stunBarMaxPts) - stunBarMaxPts - stunBarPaddingPts;
        stunBarRT.offsetMax = new Vector2(newX, stunBarRT.offsetMax.y);
    }
}
