using System.Collections.Generic;
using UnityEngine;

public class RankedController : MonoBehaviour
{
    [SerializeField]
    public GameObject playerRankContainer;

    private const string playerRankPrefsKey = "player_rank";
    private PlayerRank playerRank;

    private List<GameObject> playerRanks;

    // Start is called before the first frame update
    void Start()
    {
        playerRanks = new List<GameObject>();
        foreach (Transform child in playerRankContainer.transform) {
            playerRanks.Add(child.gameObject);
        }

        int r = PlayerPrefs.GetInt(playerRankPrefsKey);
        playerRanks[r].SetActive(true);
        playerRank = (PlayerRank)r;
    }

    public void EnableRankedMode() {
        playerRankContainer.SetActive(true);
    }

    public void DisableRankedMode() {
        playerRankContainer.SetActive(false);
    }
}
