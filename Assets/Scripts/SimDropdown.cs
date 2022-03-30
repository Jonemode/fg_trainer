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

    public bool IsPS4Mode () {
        return simDropdown.value == 0;
    }

    public bool IsPCMode() {
        return simDropdown.value == 1;
    }

    private void OnDropdownChange(int e) {
        switch (e) {
            case 0:
                // PS4
                Time.fixedDeltaTime = GameConfig.baseFixedDeltaTime;
                Application.targetFrameRate = GameConfig.baseFrameRate;
                playerAnimator.speed = 1;
                opponentAnimator.speed = 1;
                break;
            case 1:
                // PC
                Time.fixedDeltaTime = GameConfig.baseFixedDeltaTime;
                Application.targetFrameRate = GameConfig.baseFrameRate;
                playerAnimator.speed = 1;
                opponentAnimator.speed = 1;
                break;
            case 2:
                // 0.5x
                Time.fixedDeltaTime = 2 * GameConfig.baseFixedDeltaTime;
                Application.targetFrameRate = GameConfig.baseFrameRate / 2;
                playerAnimator.speed = 0.5f;
                opponentAnimator.speed = 0.5f;
                break;
            case 3:
                // 0.25x
                Time.fixedDeltaTime = 4 * GameConfig.baseFixedDeltaTime;
                Application.targetFrameRate = GameConfig.baseFrameRate / 4;
                playerAnimator.speed = 0.25f;
                opponentAnimator.speed = 0.25f;
                break;
        }
    }
}
