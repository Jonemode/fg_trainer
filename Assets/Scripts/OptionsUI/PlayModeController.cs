using TMPro;
using UnityEngine;

public class PlayModeController : MonoBehaviour
{
    [SerializeField]
    public TMP_Dropdown playModeDropdown;

    [SerializeField]
    public RankedController rankedController;

    [SerializeField]
    public TrainingController trainingController;

    // Start is called before the first frame update
    void Start()
    {
        playModeDropdown.onValueChanged.AddListener(OnPlayModeSelect);

        trainingController.EnableTrainingMode();
    }

    private void OnPlayModeSelect(int e) {
        PlayMode m = (PlayMode)e;
        switch (m) {
            case PlayMode.Training:
                trainingController.EnableTrainingMode();
                rankedController.DisableRankedMode();
                break;
            case PlayMode.Ranked:
                rankedController.EnableRankedMode();
                trainingController.DisableTrainingMode();
                break;
        }
    }

    public PlayMode GetPlayMode() {
        return (PlayMode)playModeDropdown.value;
    }
}
