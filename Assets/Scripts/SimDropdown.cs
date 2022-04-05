using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SimDropdown : MonoBehaviour
{
    [SerializeField]
    public TMP_Dropdown simDropdown;

    [SerializeField]
    public Animator playerAnimator;

    [SerializeField]
    public Animator opponentAnimator;

    [SerializeField]
    public StatsPanel statsPanel;

    // Start is called before the first frame update
    void Start()
    {
        List<TMP_Dropdown.OptionData> simOptions = new List<TMP_Dropdown.OptionData>() {
            new TMP_Dropdown.OptionData("PS4"),
            new TMP_Dropdown.OptionData("PC"),
            new TMP_Dropdown.OptionData("0.5x"),
            new TMP_Dropdown.OptionData("0.25x"),
        };
        simDropdown.AddOptions(simOptions);
        simDropdown.onValueChanged.AddListener(OnDropdownChange);
    }

    public SimMode GetSimMode() {
        return (SimMode)simDropdown.value;
    }

    private void OnDropdownChange(int e) {
        SimMode mode = (SimMode)e;
        switch (mode) {
            case SimMode.PS4:
                Time.fixedDeltaTime = GameConfig.baseFixedDeltaTime;
                Application.targetFrameRate = GameConfig.baseFrameRate;
                playerAnimator.speed = 1;
                opponentAnimator.speed = 1;
                statsPanel.LoadHighScores(mode);
                break;
            case SimMode.PC:
                Time.fixedDeltaTime = GameConfig.baseFixedDeltaTime;
                Application.targetFrameRate = GameConfig.baseFrameRate;
                playerAnimator.speed = 1;
                opponentAnimator.speed = 1;
                break;
            case SimMode.HalfSpeed:
                Time.fixedDeltaTime = 2 * GameConfig.baseFixedDeltaTime;
                Application.targetFrameRate = GameConfig.baseFrameRate / 2;
                playerAnimator.speed = 0.5f;
                opponentAnimator.speed = 0.5f;
                break;
            case SimMode.QuarterSpeed:
                Time.fixedDeltaTime = 4 * GameConfig.baseFixedDeltaTime;
                Application.targetFrameRate = GameConfig.baseFrameRate / 4;
                playerAnimator.speed = 0.25f;
                opponentAnimator.speed = 0.25f;
                break;
        }
        statsPanel.LoadHighScores(mode);
        statsPanel.ResetCurrentScore();
    }
}
