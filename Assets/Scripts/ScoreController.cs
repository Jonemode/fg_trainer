using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    [SerializeField]
    public TMP_Text hitConfirmCountText;

    [SerializeField]
    public TMP_Text hitConfirmHighText;

    [SerializeField]
    public TMP_Text blockConfirmCountText;

    [SerializeField]
    public TMP_Text blockConfirmHighText;

    [SerializeField]
    public TMP_Text confirmFrameText;

    [SerializeField]
    public Toggle stunBarToggle;

    [SerializeField]
    public TMP_Dropdown simDropdown;

    [SerializeField]
    public Button resetHighScoreButton;

    private int hitConfirmCount = 0;
    private int blockConfirmCount = 0;
    private int hitConfirmCountHigh = 0;
    private int blockConfirmCountHigh = 0;

    private const string hitConfirmPlayerPrefsKey = "hit_confirm_high_mode_{0}_stunbar_{1}";
    private const string blockConfirmPlayerPrefsKey = "block_confirm_high_mode_{0}_stunbar_{1}";

    void Start() {
        resetHighScoreButton.onClick.AddListener(ResetHighScore);

        LoadHighScores();
    }

    public void AddHitConfirm() {
        hitConfirmCount++;
        if (hitConfirmCount > hitConfirmCountHigh) {
            hitConfirmCountHigh = hitConfirmCount;
            int simMode = simDropdown.value;
            bool stunBarEnabled = stunBarToggle.isOn;
            PlayerPrefs.SetInt(string.Format(hitConfirmPlayerPrefsKey, simMode, stunBarEnabled), hitConfirmCountHigh);
            hitConfirmHighText.SetText(hitConfirmCountHigh.ToString());
        }
        hitConfirmCountText.SetText(hitConfirmCount.ToString());
    }

    public void AddBlockConfirm() {
        blockConfirmCount++;
        if (blockConfirmCount > blockConfirmCountHigh) {
            blockConfirmCountHigh = blockConfirmCount;
            int simMode = simDropdown.value;
            bool stunBarEnabled = stunBarToggle.isOn;
            PlayerPrefs.SetInt(string.Format(blockConfirmPlayerPrefsKey, simMode, stunBarEnabled), blockConfirmCountHigh);
            blockConfirmHighText.SetText(blockConfirmCountHigh.ToString());
        }
        confirmFrameText.SetText("");
        blockConfirmCountText.SetText(blockConfirmCount.ToString());
    }

    public void LoadHighScores() {
        bool stunBarEnabled = stunBarToggle.isOn;
        int simMode = simDropdown.value;
        hitConfirmCountHigh = PlayerPrefs.GetInt(string.Format(hitConfirmPlayerPrefsKey, simMode, stunBarEnabled));
        blockConfirmCountHigh = PlayerPrefs.GetInt(string.Format(blockConfirmPlayerPrefsKey, simMode, stunBarEnabled));
        hitConfirmHighText.SetText(hitConfirmCountHigh.ToString());
        blockConfirmHighText.SetText(blockConfirmCountHigh.ToString());
    }

    public void ResetCurrentScore() {
        hitConfirmCount = 0;
        blockConfirmCount = 0;
        hitConfirmCountText.SetText(hitConfirmCount.ToString());
        blockConfirmCountText.SetText(blockConfirmCount.ToString());
    }

    private void ResetHighScore() {
        hitConfirmCountHigh = 0;
        blockConfirmCountHigh = 0;
        bool stunBarEnabled = stunBarToggle.isOn;
        int simMode = simDropdown.value;
        PlayerPrefs.SetInt(string.Format(hitConfirmPlayerPrefsKey, simMode, stunBarEnabled), 0);
        PlayerPrefs.SetInt(string.Format(blockConfirmPlayerPrefsKey, simMode, stunBarEnabled), 0);
        hitConfirmHighText.SetText(hitConfirmCountHigh.ToString());
        blockConfirmHighText.SetText(blockConfirmCountHigh.ToString());
    }

    public void UpdateConfirmFrameTextSuccess(int playerSpecialActivateFrame) {
        // Frame
        confirmFrameText.SetText((playerSpecialActivateFrame + 1).ToString());
        // Color
        confirmFrameText.color = Color.green;
    }

    public void UpdateConfirmFrameTextFail(int playerSpecialActivateFrame) {
        // Frame
        confirmFrameText.SetText((playerSpecialActivateFrame + 1).ToString());
        // Color
        confirmFrameText.color = Color.red;
    }

    public void ClearConfirmFrameText() {
        confirmFrameText.SetText("");
    }
}
