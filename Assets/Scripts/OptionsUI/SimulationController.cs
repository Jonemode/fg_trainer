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
    public TrainingController trainingController;

    // Start is called before the first frame update
    void Start()
    {
        simDropdown.value = (int)SimMode.PC;
        SetSimulationSpeed(1);
        simDropdown.onValueChanged.AddListener(OnDropdownChange);
    }

    public SimMode GetSimMode() {
        return (SimMode)simDropdown.value;
    }

    private void OnDropdownChange(int e) {
        trainingController.LoadHighScores();
        trainingController.ResetCurrentScore();
    }

    public void SetSimulationSpeed(float speed) {
        Time.fixedDeltaTime = GameConfig.baseFixedDeltaTime / speed;
        Application.targetFrameRate = (int)(GameConfig.baseFrameRate * speed);
        playerAnimator.speed = speed;
        opponentAnimator.speed = speed;
    }
}
