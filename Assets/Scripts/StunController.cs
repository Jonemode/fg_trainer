using UnityEngine;
using UnityEngine.UI;

public class StunController : MonoBehaviour
{
    [SerializeField]
    public GameObject stunBar;

    [SerializeField]
    public GameObject stunBarContainer;

    [SerializeField]
    public Toggle stunBarToggle;

    [SerializeField]
    public TrainingController trainingController;

    private RectTransform stunBarRT;

    // Using Street Fighter units
    private const float maxStun = 1000;
    private float currentStun = 0;

    // Using world space units
    private float stunBarMaxPts;
    private const float stunBarPaddingPts = 6;

    private const int stunRetainmentFrames = 100;
    private const float stunDecay = 2.5f;

    // StunRetainmentFrame default value is high, when setting low it will grow back high as frames pass
    public int StunRetainmentFrame = stunRetainmentFrames;

    // Start is called before the first frame update
    void Start()
    {
        stunBarRT = stunBar.GetComponent<RectTransform>();

        // Set the left offset (padding)
        stunBarRT.offsetMin = new Vector2(stunBarPaddingPts, stunBarRT.offsetMin.y);

        RectTransform stunBarBackgroundRT = stunBarContainer.GetComponent<RectTransform>();
        stunBarMaxPts = stunBarBackgroundRT.rect.width - (2 * stunBarPaddingPts);

        redrawStunBar();

        stunBarToggle.onValueChanged.AddListener((isVisible) => {
            SetStunBarEnabled(isVisible);
            trainingController.LoadHighScores();
            trainingController.ResetCurrentScore();
        });
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

    public void UpdateStunOnBlock() {
        StunRetainmentFrame = 0;
    }

    public void UpdateStunOnHit(int stunAmount) {
        StunRetainmentFrame = 0;
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

    public void SetStunBarEnabled(bool enabled) {
        stunBarContainer.SetActive(enabled);
    }
}
