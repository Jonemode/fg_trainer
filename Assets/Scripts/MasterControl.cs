using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MasterControl : MonoBehaviour
{
    public Button normalButton;
    public Button specialButton;
    public TMP_Text confirmCountText;

    public OpponentStun opponentStun;
    public CounterHit counterHit;

    private const int startupFrames = 7;
    private const int activeFrames = 3;
    private const int recoveryFrames = 14;
    private const int confirmWindowFrames = 16;

    private const int opponentDefendPercentage = 50;

    private CharacterState playerState = CharacterState.Neutral;
    private CharacterState opponentState = CharacterState.Neutral;

    private int playerStartupFrame = 0;
    private int playerActiveFrame = 0;
    private int playerRecoveryFrame = 0;
    private int opponentRecoveryFrame = 0;
    private int confirmCount = 0;

    private Move currentMove = Move.None;

    private static System.Random rnd = new System.Random();

    // Start is called before the first frame update
    void Start() {
        Application.targetFrameRate = 60;

        // Setup button clicks
        createButtonBinding(normalButton, normalButtonClick);
        createButtonBinding(specialButton, specialButtonClick);
    }

    // Update is called once per frame
    void Update() {
        if (playerState == CharacterState.Startup) {
            playerStartupFrame += 1;
            if (playerStartupFrame >= startupFrames) {
                // Active frames start
                playerStartupFrame = 0;
                opponentStun.StunRetainmentFrame = 0;
                playerState = CharacterState.Active;
                if (rnd.Next(1, 100) <= opponentDefendPercentage) {
                    // Opponent blocked
                    opponentState = CharacterState.Neutral;
                    Debug.Log("Opponent Blocked");
                } else {
                    // Opponent got hit
                    opponentStun.UpdateStunOnHit();
                    counterHit.UpdateCounterHitTextOnHit();
                    opponentState = CharacterState.HitStun;
                    Debug.Log("Opponent Hit!");
                }
            }
        }

        if (playerState == CharacterState.Active) {
            playerActiveFrame += 1;
            if (playerActiveFrame >= activeFrames) {
                playerActiveFrame = 0;
                playerState = CharacterState.Recovery;
            }
        }

        if (playerState == CharacterState.Recovery) {
            playerRecoveryFrame += 1;
            if (playerRecoveryFrame >= recoveryFrames) {
                playerRecoveryFrame = 0;
                playerState = CharacterState.Neutral;
                if (currentMove == Move.Normal && opponentState == CharacterState.HitStun) {
                    confirmCount = 0;
                    confirmCountText.SetText(confirmCount.ToString());
                }
                currentMove = Move.None;
            }
        }

        if (opponentState == CharacterState.HitStun) {
            opponentRecoveryFrame += 1;
            if (opponentRecoveryFrame >= confirmWindowFrames) {
                opponentRecoveryFrame = 0;
                opponentState = CharacterState.Neutral;
                Debug.Log("Opponent Recovered!");
            }
        }
    }

    private void normalButtonClick(BaseEventData e) {
        if (playerState == CharacterState.Neutral) {
            currentMove = Move.Normal;
            playerState = CharacterState.Startup;
        }
    }

    private void specialButtonClick(BaseEventData e) {
        if (playerState == CharacterState.Startup) {
            confirmCount = 0;
        } else if (opponentState == CharacterState.HitStun && currentMove == Move.Normal) {
            playerState = CharacterState.Active;
            currentMove = Move.Special;
            confirmCount++;
        } else {
            playerState = CharacterState.Active;
            currentMove = Move.Special;
            confirmCount = 0;
        }
        confirmCountText.SetText(confirmCount.ToString());
    }

    private void createButtonBinding(Button button, Action<BaseEventData> cb) {
        EventTrigger trigger = button.gameObject.AddComponent<EventTrigger>();
        var pointerDown = new EventTrigger.Entry();
        pointerDown.eventID = EventTriggerType.PointerDown;
        pointerDown.callback.AddListener((e) => cb(e));
        trigger.triggers.Add(pointerDown);
    }
}
