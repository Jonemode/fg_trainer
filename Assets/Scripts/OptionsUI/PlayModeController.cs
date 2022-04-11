using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayModeController : MonoBehaviour
{
    [SerializeField]
    public TMP_Dropdown playModeDropdown;

    [SerializeField]
    public Toggle stunBarToggle;

    [SerializeField]
    public GameObject simModeLabel;

    [SerializeField]
    public TMP_Dropdown simDropdown;

    [SerializeField]
    public RankedController rankedController;

    [SerializeField]
    public TrainingController trainingController;

    // Start is called before the first frame update
    void Start()
    {
        playModeDropdown.onValueChanged.AddListener(OnPlayModeSelect);
    }

    private void OnPlayModeSelect(int e) {
        PlayMode m = (PlayMode)e;
        switch (m) {
            case PlayMode.Training:
                trainingController.EnableTrainingMode();
                rankedController.DisableRankedMode();
                simModeLabel.SetActive(true);
                simDropdown.gameObject.SetActive(true);
                stunBarToggle.gameObject.SetActive(true);
                break;
            case PlayMode.Ranked:
                rankedController.EnableRankedMode();
                trainingController.DisableTrainingMode();
                simModeLabel.SetActive(false);
                simDropdown.gameObject.SetActive(false);
                stunBarToggle.gameObject.SetActive(false);
                break;
        }
    }

    public PlayMode GetPlayMode() {
        return (PlayMode)playModeDropdown.value;
    }
}
