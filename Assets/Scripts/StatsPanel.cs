using TMPro;
using UnityEngine;

public class StatsPanel : MonoBehaviour
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

    private int hitConfirmCount = 0;
    private int blockConfirmCount = 0;
    private int hitConfirmCountHigh = 0;
    private int blockConfirmCountHigh = 0;

    private const string hitConfirmPlayerPrefsKey = "hit_confirm_high_mode_{0}";
    private const string blockConfirmPlayerPrefsKey = "block_confirm_high_mode_{0}";

    void Start() {
        LoadHighScores((int)SimMode.PS4);
    }

    public void AddHitConfirm(SimMode simMode) {
        hitConfirmCount++;
        if (hitConfirmCount > hitConfirmCountHigh) {
            hitConfirmCountHigh = hitConfirmCount;
            PlayerPrefs.SetInt(string.Format(hitConfirmPlayerPrefsKey, (int)simMode), hitConfirmCountHigh);
            hitConfirmHighText.SetText(hitConfirmCountHigh.ToString());
        }
        hitConfirmCountText.SetText(hitConfirmCount.ToString());
    }

    public void AddBlockConfirm(SimMode simMode) {
        blockConfirmCount++;
        if (blockConfirmCount > blockConfirmCountHigh) {
            blockConfirmCountHigh = blockConfirmCount;
            PlayerPrefs.SetInt(string.Format(blockConfirmPlayerPrefsKey, (int)simMode), blockConfirmCountHigh);
            blockConfirmHighText.SetText(blockConfirmCountHigh.ToString());
        }
        confirmFrameText.SetText("");
        blockConfirmCountText.SetText(blockConfirmCount.ToString());
    }

    public void LoadHighScores(SimMode simMode) {
        hitConfirmCountHigh = PlayerPrefs.GetInt(string.Format(hitConfirmPlayerPrefsKey, (int)simMode));
        blockConfirmCountHigh = PlayerPrefs.GetInt(string.Format(blockConfirmPlayerPrefsKey, (int)simMode));
        hitConfirmHighText.SetText(hitConfirmCountHigh.ToString());
        blockConfirmHighText.SetText(blockConfirmCountHigh.ToString());
    }

    public void ResetCurrentScore() {
        hitConfirmCount = 0;
        blockConfirmCount = 0;
        hitConfirmCountText.SetText(hitConfirmCount.ToString());
        blockConfirmCountText.SetText(blockConfirmCount.ToString());
    }

    public void ResetHighScore(SimMode simMode) {
        hitConfirmCountHigh = 0;
        blockConfirmCountHigh = 0;
        PlayerPrefs.SetInt(string.Format(hitConfirmPlayerPrefsKey, simMode), 0);
        PlayerPrefs.SetInt(string.Format(blockConfirmPlayerPrefsKey, simMode), 0);
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
