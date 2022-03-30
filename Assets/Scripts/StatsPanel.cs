using TMPro;
using UnityEngine;

public class StatsPanel : MonoBehaviour
{
    [SerializeField]
    public TMP_Text hitConfirmCountText;

    [SerializeField]
    public TMP_Text blockConfirmCountText;

    [SerializeField]
    public TMP_Text confirmFrameText;

    private int hitConfirmCount = 0;
    private int blockConfirmCount = 0;

    public int HitConfirmCount {
        get {
            return hitConfirmCount;
        }
        set {
            hitConfirmCount = value;
            hitConfirmCountText.SetText(hitConfirmCount.ToString());
        }
    }

    public int BlockConfirmCount {
        get {
            return blockConfirmCount;
        }
        set {
            blockConfirmCount = value;
            confirmFrameText.SetText("");
            blockConfirmCountText.SetText(blockConfirmCount.ToString());
        }
    }

    public void ResetScore() {
        HitConfirmCount = 0;
        BlockConfirmCount = 0;
    }

    public void UpdateConfirmFrameText(int playerSpecialActivateFrame, CharacterState opponentState) {
        if (playerSpecialActivateFrame == 0) {
            confirmFrameText.SetText("");
        } else {
            // Frame
            confirmFrameText.SetText((playerSpecialActivateFrame + 1).ToString());

            // Color
            if (opponentState == CharacterState.SpecialHitStun) {
                confirmFrameText.color = Color.green;
            } else {
                confirmFrameText.color = Color.red;
            }
        }
    }
}
