using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SimulationController : MonoBehaviour
{
    [SerializeField]
    public TMP_Dropdown simDropdown;

    [SerializeField]
    public Animator playerAnimator;

    [SerializeField]
    public Animator opponentAnimator;

    [SerializeField]
    public ScoreController scoreController;

    // Start is called before the first frame update
    void Start()
    {
        simDropdown.value = (int)SimMode.PC;
        Time.fixedDeltaTime = GameConfig.baseFixedDeltaTime;
        Application.targetFrameRate = GameConfig.baseFrameRate;
        playerAnimator.speed = 1;
        opponentAnimator.speed = 1;
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
        scoreController.LoadHighScores();
        scoreController.ResetCurrentScore();
    }
}
