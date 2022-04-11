using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrainingController : MonoBehaviour
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
    public GameObject simModeLabel;

    [SerializeField]
    public TMP_Dropdown simDropdown;

    [SerializeField]
    public Button resetHighScoreButton;

    [SerializeField]
    public StateController stateController;

    [SerializeField]
    public GameObject statsPanel;

    private int hitConfirmCount = 0;
    private int blockConfirmCount = 0;
    private int hitConfirmCountHigh = 0;
    private int blockConfirmCountHigh = 0;

    private const string hitConfirmPlayerPrefsKey = "hit_confirm_high_mode_{0}_stunbar_{1}";
    private const string blockConfirmPlayerPrefsKey = "block_confirm_high_mode_{0}_stunbar_{1}";

    void Start() {
        resetHighScoreButton.onClick.AddListener(ResetHighScore);
    }

    public void EnableTrainingMode() {
        SetTrainingObjectVisibility(true);
        ClearConfirmFrame();
        ResetCurrentScore();
        LoadHighScores();
        stateController.PlayerError += onPlayerError;
        stateController.HitConfirm += onHitConfirm;
        stateController.BlockConfirm += onBlockConfirm;
        stateController.ConfirmFrameSuccess += onConfirmFrameSuccess;
        stateController.ConfirmFrameFailure += onConfirmFrameFailure;
        stateController.NormalButtonPress += onNormalButtonPress;
    }

    public void DisableTrainingMode() {
        SetTrainingObjectVisibility(false);
        stateController.PlayerError -= onPlayerError;
        stateController.HitConfirm -= onHitConfirm;
        stateController.BlockConfirm -= onBlockConfirm;
        stateController.ConfirmFrameSuccess -= onConfirmFrameSuccess;
        stateController.ConfirmFrameFailure -= onConfirmFrameFailure;
        stateController.NormalButtonPress -= onNormalButtonPress;
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

    private void SetTrainingObjectVisibility(bool enabled) {
        statsPanel.SetActive(enabled);
        simModeLabel.SetActive(enabled);
        simDropdown.gameObject.SetActive(enabled);
        stunBarToggle.gameObject.SetActive(enabled);
    }

    private void ClearConfirmFrame() {
        confirmFrameText.SetText("");
    }

    private void onConfirmFrameSuccess(object sender, ConfirmFrameEventArgs e) {
        // Frame
        confirmFrameText.SetText((e.SpecialActivateFrame + 1).ToString());
        // Color
        confirmFrameText.color = Color.green;
    }

    private void onConfirmFrameFailure(object sender, ConfirmFrameEventArgs e) {
        // Frame
        confirmFrameText.SetText((e.SpecialActivateFrame + 1).ToString());
        // Color
        confirmFrameText.color = Color.red;
    }

    private void onPlayerError(object sender, EventArgs e) {
        ResetCurrentScore();
    }

    private void onNormalButtonPress(object sender, EventArgs e) {
        ClearConfirmFrame();
    }

    private void onHitConfirm(object sender, EventArgs e) {
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

    private void onBlockConfirm(object sender, EventArgs e) {
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
}
