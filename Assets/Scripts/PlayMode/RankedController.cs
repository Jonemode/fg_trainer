using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RankedController : MonoBehaviour
{
    [SerializeField]
    public GameObject playerRankContainer;

    [SerializeField]
    public StateController stateController;

    [SerializeField]
    public TMP_Text LpText;

    private const string playerLpPrefsKey = "player_lp";
    
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

        displayLp = PlayerPrefs.GetInt(playerLpPrefsKey);
        targetLp = displayLp;
        currentRank = GetRank();
        playerRanks[(int)currentRank].SetActive(true);
        UpdateLpText();
    }

    void Update() {
        if (displayLp < targetLp) {
            displayLp += 1;
            UpdateLpText();
        }
        if (displayLp > targetLp) {
            displayLp -= 1;
            UpdateLpText();
        }
    }

    public void EnableRankedMode() {
        playerRankContainer.SetActive(true);
        LpText.gameObject.SetActive(true);
        stateController.PlayerError += onPlayerError;
        stateController.HitConfirm += onConfirm;
        stateController.BlockConfirm += onConfirm;
    }

    public void DisableRankedMode() {
        playerRankContainer.SetActive(false);
        LpText.gameObject.SetActive(false);
        stateController.PlayerError -= onPlayerError;
        stateController.HitConfirm -= onConfirm;
        stateController.BlockConfirm -= onConfirm;
    }

    private void UpdateLpText() {
        LpText.SetText("{0} LP", displayLp);
    }

    private void onPlayerError(object sender, EventArgs e) {
        targetLp -= (25 + (10 * (int)currentRank));
        if (targetLp < 0) {
            targetLp = 0;
        }
        PlayerRank r = GetRank();
        if (r != currentRank) {
            // rank down
            playerRanks[(int)r + 1].SetActive(false);
            playerRanks[(int)r].SetActive(true);
            currentRank = r;
        }
        PlayerPrefs.SetInt(playerLpPrefsKey, targetLp);
    }

    private void onConfirm(object sender, EventArgs e) {
        targetLp += 75 - (3 * (int)currentRank);
        PlayerRank r = GetRank();
        if (r != currentRank) {
            // rank up
            playerRanks[(int)r - 1].SetActive(false);
            playerRanks[(int)r].SetActive(true);
            currentRank = r;
        }
        PlayerPrefs.SetInt(playerLpPrefsKey, targetLp);
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
}
