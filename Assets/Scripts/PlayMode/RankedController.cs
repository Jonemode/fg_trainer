using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankedController : MonoBehaviour
{
    [SerializeField]
    public GameObject playerRankContainer;

    [SerializeField]
    public StateController stateController;

    [SerializeField]
    public TMP_Dropdown simDropdown;

    [SerializeField]
    public SimulationController simulationController;

    [SerializeField]
    public Toggle stunBarToggle;

    [SerializeField]
    public TMP_Text LpText;

    [SerializeField]
    public TMP_Text LpChangeText;

    [SerializeField]
    public Button resetRankButton;

    [SerializeField]
    public GameObject leagueContainer;

    private GameObject leagueUpText;
    private GameObject leagueDownText;
    private GameObject upArrow;
    private GameObject downArrow;

    private const string playerLpPrefsKey = "player_lp";

    // set high and ticks towards -1
    private int lpChangeTextFrame = -1;
    
    // tick up towards target to animate the LP number
    private int targetLp;
    private int displayLp;

    private PlayerRank currentRank;
    private List<GameObject> playerRanks;

    // Start is called before the first frame update
    void Start()
    {
        playerRanks = new List<GameObject>();
        foreach (Transform child in playerRankContainer.transform) {
            playerRanks.Add(child.gameObject);
        }

        leagueUpText = leagueContainer.transform.Find("LeagueUpText").gameObject;
        leagueDownText = leagueContainer.transform.Find("LeagueDownText").gameObject;
        upArrow = leagueContainer.transform.Find("UpArrow").gameObject;
        downArrow = leagueContainer.transform.Find("DownArrow").gameObject;

        displayLp = PlayerPrefs.GetInt(playerLpPrefsKey);
        targetLp = displayLp;
        currentRank = GetRank();
        playerRanks[(int)currentRank].SetActive(true);
        UpdateLpText();
    }

    void Update() {
        if (lpChangeTextFrame >= 0) {
            lpChangeTextFrame -= 1;
        }
        if (lpChangeTextFrame == 0) {
            LpChangeText.gameObject.SetActive(false);
        }
        

        if (displayLp < targetLp) {
            if (targetLp - displayLp > 20) {
                displayLp += 5;
            } else {
                displayLp += 1;
            }
            UpdateLpText();
        }
        if (displayLp > targetLp) {
            if (displayLp - targetLp > 20) {
                displayLp -= 5;
            } else {
                displayLp -= 1;
            }            
            UpdateLpText();
        }
    }

    public void EnableRankedMode() {
        playerRankContainer.SetActive(true);
        LpText.gameObject.SetActive(true);
        leagueContainer.SetActive(true);
        resetRankButton.gameObject.SetActive(true);
        resetRankButton.onClick.AddListener(ResetRank);
        stateController.PlayerError += onPlayerError;
        stateController.HitConfirm += onConfirm;
        stateController.BlockConfirm += onConfirm;
        ConfigureGame();
    }

    public void DisableRankedMode() {
        playerRankContainer.SetActive(false);
        LpText.gameObject.SetActive(false);
        leagueContainer.SetActive(false);
        resetRankButton.onClick.RemoveAllListeners();
        resetRankButton.gameObject.SetActive(false);
        stateController.PlayerError -= onPlayerError;
        stateController.HitConfirm -= onConfirm;
        stateController.BlockConfirm -= onConfirm;
    }

    private void UpdateLpText() {
        LpText.SetText("{0} LP", displayLp);
    }

    private void ResetRank() {
        playerRanks[(int)currentRank].SetActive(false);
        displayLp = 0;
        targetLp = 0;
        currentRank = GetRank();
        playerRanks[(int)currentRank].SetActive(true);
        PlayerPrefs.SetInt(playerLpPrefsKey, 0);
        UpdateLpText();
        ConfigureGame();
    }

    private void onPlayerError(object sender, EventArgs e) {
        if (targetLp != displayLp) {
            // Cannot lose too much lp at once. PlayerError can trigger multiple times in one attack.
            return;
        }
        int change = -(110 + (10 * (int)currentRank));
        targetLp += change;
        ShowLpChange(change);
        if (targetLp < 0) {
            targetLp = 0;
        }
        PlayerRank r = GetRank();
        if (r != currentRank) {
            // rank down
            playerRanks[(int)r + 1].SetActive(false);
            playerRanks[(int)r].SetActive(true);
            currentRank = r;
            ConfigureGame();
            ShowLeagueDown();
        } else {
            HideLeagueMessage();
        }
        
        PlayerPrefs.SetInt(playerLpPrefsKey, targetLp);
    }

    private void onConfirm(object sender, EventArgs e) {
        int change = 100 - (5 * (int)currentRank);
        targetLp += change;
        ShowLpChange(change);
        PlayerRank r = GetRank();
        if (r != currentRank) {
            // rank up
            playerRanks[(int)r - 1].SetActive(false);
            playerRanks[(int)r].SetActive(true);
            currentRank = r;
            ConfigureGame();
            ShowLeagueUp();
        } else {
            HideLeagueMessage();
        }
        PlayerPrefs.SetInt(playerLpPrefsKey, targetLp);
    }

    private void ShowLpChange(int change) {
        lpChangeTextFrame = 100;
        if (change > 0) {
            LpChangeText.SetText(string.Format("+{0}", change));
            LpChangeText.color = Color.green;
        } else {
            LpChangeText.SetText(change.ToString());
            LpChangeText.color = Color.red;
        }
        LpChangeText.gameObject.SetActive(true);
    }

    private void ShowLeagueUp() {
        leagueUpText.SetActive(true);
        upArrow.SetActive(true);
        leagueDownText.SetActive(false);
        downArrow.SetActive(false);
    }

    private void ShowLeagueDown() {
        leagueDownText.SetActive(true);
        downArrow.SetActive(true);
        leagueUpText.SetActive(false);
        upArrow.SetActive(false);
    }

    private void HideLeagueMessage() {
        leagueUpText.SetActive(false);
        leagueDownText.SetActive(false);
        upArrow.SetActive(false);
        downArrow.SetActive(false);
    }

    private PlayerRank GetRank() {
        if (targetLp < 500) {
            return PlayerRank.Rookie;
        } else if (targetLp < 1000) {
            return PlayerRank.Bronze;
        } else if (targetLp < 1500) {
            return PlayerRank.SuperBronze;
        } else if (targetLp < 2000) {
            return PlayerRank.UltraBronze;
        } else if (targetLp < 3000) {
            return PlayerRank.Silver;
        } else if (targetLp < 3500) {
            return PlayerRank.SuperSilver;
        } else if (targetLp < 4000) {
            return PlayerRank.UltraSilver;
        } else if (targetLp < 5500) {
            return PlayerRank.Gold;
        } else if (targetLp < 6500) {
            return PlayerRank.SuperGold;
        } else if (targetLp < 7500) {
            return PlayerRank.UltraGold;
        } else if (targetLp < 10000) {
            return PlayerRank.Platinum;
        } else if (targetLp < 12000) {
            return PlayerRank.SuperPlatinum;
        } else if (targetLp < 14000) {
            return PlayerRank.UltraPlatinum;
        } else if (targetLp < 20000) {
            return PlayerRank.Diamond;
        } else if (targetLp < 25000) {
            return PlayerRank.SuperDiamond;
        } else if (targetLp < 30000) {
            return PlayerRank.UltraDiamond;
        } else if (targetLp < 35000) {
            return PlayerRank.Master;
        } else if (targetLp < 100000) {
            return PlayerRank.GrandMaster;
        } else if (targetLp < 300000) {
            return PlayerRank.UltimateGrandMaster;
        } else {
            return PlayerRank.Warlord;
        }
    }

    private void ConfigureGame() {
        switch (currentRank) {
            case PlayerRank.Rookie:
                simDropdown.value = (int)SimMode.PC;
                stunBarToggle.isOn = true;
                simulationController.SetSimulationSpeed(0.3f);
                break;
            case PlayerRank.Bronze:
                simDropdown.value = (int)SimMode.PC;
                stunBarToggle.isOn = true;
                simulationController.SetSimulationSpeed(0.4f);
                break;
            case PlayerRank.SuperBronze:
                simDropdown.value = (int)SimMode.PC;
                stunBarToggle.isOn = true;
                simulationController.SetSimulationSpeed(0.5f);
                break;
            case PlayerRank.UltraBronze:
                simDropdown.value = (int)SimMode.PC;
                stunBarToggle.isOn = true;
                simulationController.SetSimulationSpeed(0.6f);
                break;
            case PlayerRank.Silver:
                simDropdown.value = (int)SimMode.PC;
                stunBarToggle.isOn = true;
                simulationController.SetSimulationSpeed(0.7f);
                break;
            case PlayerRank.SuperSilver:
                simDropdown.value = (int)SimMode.PC;
                stunBarToggle.isOn = true;
                simulationController.SetSimulationSpeed(0.8f);
                break;
            case PlayerRank.UltraSilver:
                simDropdown.value = (int)SimMode.PC;
                stunBarToggle.isOn = true;
                simulationController.SetSimulationSpeed(0.9f);
                break;
            case PlayerRank.Gold:
                simDropdown.value = (int)SimMode.PC;
                stunBarToggle.isOn = true;
                simulationController.SetSimulationSpeed(1);
                break;
            case PlayerRank.SuperGold:
                simDropdown.value = (int)SimMode.PC;
                stunBarToggle.isOn = true;
                simulationController.SetSimulationSpeed(1);
                break;
            case PlayerRank.UltraGold:
            case PlayerRank.Platinum:
            case PlayerRank.SuperPlatinum:
            case PlayerRank.UltraPlatinum:
            case PlayerRank.Diamond:
            case PlayerRank.SuperDiamond:
            case PlayerRank.UltraDiamond:
            case PlayerRank.Master:
            case PlayerRank.GrandMaster:
            case PlayerRank.UltimateGrandMaster:
                simDropdown.value = (int)SimMode.PC;
                stunBarToggle.isOn = false;
                simulationController.SetSimulationSpeed(1);
                break;
            case PlayerRank.Warlord:
                simDropdown.value = (int)SimMode.PS4;
                stunBarToggle.isOn = false;
                simulationController.SetSimulationSpeed(1);
                break;
        }
    }
}
