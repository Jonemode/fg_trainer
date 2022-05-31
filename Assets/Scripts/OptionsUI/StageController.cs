using TMPro;
using UnityEngine;

public class StageController : MonoBehaviour
{
    [SerializeField]
    public GameObject backgrounds;

    [SerializeField]
    public TMP_Dropdown stageSelector;

    private GameObject theGrid;
    private GameObject airForceBase;
    private GameObject suzakuCastle;

    private GameObject currentStage;

    // Start is called before the first frame update
    void Start()
    {
        theGrid = backgrounds.transform.Find("TheGrid").gameObject;
        airForceBase = backgrounds.transform.Find("AirForceBase").gameObject;
        suzakuCastle = backgrounds.transform.Find("SuzakuCastle").gameObject;

        System.Random rnd = new System.Random();
        stageSelector.value = rnd.Next(0, 3);
        OnDropdownChange(stageSelector.value);
        stageSelector.onValueChanged.AddListener(OnDropdownChange);
    }

    private void OnDropdownChange(int e) {
        switch (e) {
            case 0:
                ChangeStage(theGrid);
                break;
            case 1:
                ChangeStage(airForceBase);
                break;
            case 2:
                ChangeStage(suzakuCastle);
                break;
        }
    }

    private void ChangeStage(GameObject newStage) {
        if (currentStage != null) {
            currentStage.SetActive(false);
        }
        newStage.SetActive(true);
        currentStage = newStage;
    }
}
