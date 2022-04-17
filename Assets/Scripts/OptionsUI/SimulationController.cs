using TMPro;
using UnityEngine;

public class SimulationController : MonoBehaviour
{
    [SerializeField]
    public TMP_Dropdown simModeDropdown;

    [SerializeField]
    public TMP_Dropdown simSpeedDropdown;

    [SerializeField]
    public Animator playerAnimator;

    [SerializeField]
    public Animator opponentAnimator;

    [SerializeField]
    public TrainingController trainingController;

    public SimMode CurrentSimMode;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = GameConfig.baseFrameRate;
        Time.fixedDeltaTime = GameConfig.baseFixedDeltaTime;
        CurrentSimMode = SimMode.PC;
        simModeDropdown.value = (int)SimMode.PC;
        simSpeedDropdown.value = (int)SimSpeed.OneHundredPercent;
        simSpeedDropdown.onValueChanged.AddListener(OnSimSpeedChange);
        simModeDropdown.onValueChanged.AddListener(OnSimModeChange);
    }

    /*
    void FixedUpdate() {
        Debug.Log((int)(1f / Time.unscaledDeltaTime));
    }
    */

    public SimMode GetSimMode() {
        return CurrentSimMode;
    }

    private void OnSimModeChange(int e) {
        CurrentSimMode = (SimMode)e;
        trainingController.LoadHighScores();
        trainingController.ResetCurrentScore();
    }

    public void OnSimSpeedChange(int e) {
        // Map SimSpeed to float value
        SetSimulationSpeed(1 - (e / 10f));
        trainingController.LoadHighScores();
        trainingController.ResetCurrentScore();
    }

    private void SetSimulationSpeed(float speed) {
        Time.timeScale = speed;
    }
}
