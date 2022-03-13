using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SimDropdown : MonoBehaviour
{
    public TMP_Dropdown simDropdown;

    // Start is called before the first frame update
    void Start()
    {
        List<TMP_Dropdown.OptionData> simOptions = new List<TMP_Dropdown.OptionData>() {
            new TMP_Dropdown.OptionData("PS4"),
            new TMP_Dropdown.OptionData("PC"),
        };
        simDropdown.AddOptions(simOptions);
    }

    public int GetSimulatedConfirmWindow() {
        if (simDropdown.value == 0) {
            // PS4
            return GameConfig.confirmWindowFrames;
        }
        // PC
        return GameConfig.confirmWindowFrames + GameConfig.ps4FrameLag;
    }

    public bool IsPS4Mode () {
        return simDropdown.value == 0;
    }

    public bool IsPCMode() {
        return simDropdown.value == 1;
    }
}
