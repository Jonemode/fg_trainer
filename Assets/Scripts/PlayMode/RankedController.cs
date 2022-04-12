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
    private int currentLp;

    private List<GameObject> playerRanks;

    // Start is called before the first frame update
    void Start()
    {
        playerRanks = new List<GameObject>();
        foreach (Transform child in playerRankContainer.transform) {
            playerRanks.Add(child.gameObject);
        }

        currentLp = PlayerPrefs.GetInt(playerLpPrefsKey);
        UpdateUI();
    }

    void Update() {
        if (currentLp < targetLp) {
            currentLp += 1;
            UpdateUI();
        }
    }

    public void EnableRankedMode() {
        playerRankContainer.SetActive(true);
        LpText.gameObject.SetActive(true);
        stateController.PlayerError += onPlayerError;
        stateController.HitConfirm += onHitConfirm;
        stateController.BlockConfirm += onBlockConfirm;
    }

    public void DisableRankedMode() {
        playerRankContainer.SetActive(false);
        LpText.gameObject.SetActive(false);
        stateController.PlayerError -= onPlayerError;
        stateController.HitConfirm -= onHitConfirm;
        stateController.BlockConfirm -= onBlockConfirm;
    }

    private void UpdateUI() {
        LpText.SetText("{0} LP", currentLp);
    }

    private void onPlayerError(object sender, EventArgs e) {

    }

    private void onHitConfirm(object sender, EventArgs e) {

    }

    private void onBlockConfirm(object sender, EventArgs e) {

    }
}
