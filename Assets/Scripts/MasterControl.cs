using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MasterControl : MonoBehaviour
{
    public Button normalButton;
    public Button specialButton;
    public TMP_Text hitConfirmCountText;
    public TMP_Text blockConfirmCountText;
    public TMP_Text confirmFrameText;
    public GameObject player;
    public OpponentStun opponentStun;
    public CounterHit counterHit;
    public SoundSystem soundSystem;

    private Animator playerAnimator;

    private CharacterState playerState;
    private CharacterState opponentState;

    private int playerStartupFrame = 0;
    private int playerActiveFrame = 0;
    private int playerRecoveryFrame = 0;
    private int playerSpecialFrame = 0;
    private int opponentRecoveryFrame = 0;
    private int hitConfirmCount = 0;
    private int blockConfirmCount = 0;

    private static System.Random rnd = new System.Random();

    // Start is called before the first frame update
    void Start() {
        Application.targetFrameRate = 60;

        playerState = CharacterState.Neutral;
        opponentState = CharacterState.Neutral;

        playerAnimator = player.GetComponent<Animator>();

        // Setup button clicks
        createButtonBinding(normalButton, normalButtonClick);
        createButtonBinding(specialButton, specialButtonClick);
    }

    // Update is called once per frame
    void Update() {
        switch(playerState) {
            case CharacterState.Startup:
                handleStartupFrame();
                break;
            case CharacterState.Active:
                handleActiveFrame();
                break;
            case CharacterState.Recovery:
                handleRecoveryFrame();
                break;
            case CharacterState.Special:
                handleSpecialFrame();
                break;
            default:
                // in neutral, do nothing
                break;
        }

        switch(opponentState) {
            case CharacterState.HitStun:
                handleHitStunFrame();
                break;
            case CharacterState.BlockStun:
                handleBlockStunFrame();
                break;
            default:
                // in neutral, do nothing
                break;
        }
    }

    private void handleStartupFrame() {
        playerStartupFrame += 1;
        if (playerStartupFrame >= GameConfig.startupFrames) {
            // Active frames start
            playerStartupFrame = 0;
            opponentStun.StunRetainmentFrame = 0;
            playerState = CharacterState.Active;
            playerAnimator.Play("cr_mk_active");
            if (rnd.Next(1, 100) <= GameConfig.opponentDefendPercentage) {
                // Opponent blocked
                opponentState = CharacterState.BlockStun;
            } else {
                // Opponent got hit
                opponentStun.UpdateStunOnHit(GameConfig.stunAmount);
                counterHit.UpdateCounterHitTextOnHit();
                soundSystem.PlayNormalAttackHit();
                opponentState = CharacterState.HitStun;
            }
        }
    }

    private void handleActiveFrame() {
        playerActiveFrame += 1;
        if (playerActiveFrame >= GameConfig.activeFrames) {
            playerActiveFrame = 0;
            playerState = CharacterState.Recovery;
            playerAnimator.Play("cr_mk_recovery");
        }
    }

    private void handleRecoveryFrame() {
        playerRecoveryFrame += 1;
        if (playerRecoveryFrame >= GameConfig.recoveryFrames) {
            playerRecoveryFrame = 0;
            playerState = CharacterState.Neutral;
            playerAnimator.Play("idle");
        }
    }

    private void handleSpecialFrame() {
        playerSpecialFrame += 1;
        if (playerSpecialFrame >= GameConfig.specialFrames) {
            playerSpecialFrame = 0;
            playerState = CharacterState.Neutral;
            playerAnimator.Play("idle");
        }
    }

    private void handleHitStunFrame() {
        opponentRecoveryFrame += 1;
        if (opponentRecoveryFrame >= GameConfig.hitStunRecoveryFrames) {
            opponentRecoveryFrame = 0;
            opponentState = CharacterState.Neutral;
            Debug.Log("Opponent Recovered!");
        }
    }

    private void handleBlockStunFrame() {
        opponentRecoveryFrame += 1;
        if (opponentRecoveryFrame >= GameConfig.blockStunRecoveryFrames) {
            opponentRecoveryFrame = 0;
            opponentState = CharacterState.Neutral;
            Debug.Log("Opponent Recovered!");
            if (playerState != CharacterState.Special) {
                // Player successfully did not activate special
                blockConfirmCount++;
                blockConfirmCountText.SetText(blockConfirmCount.ToString());
                confirmFrameText.SetText("");
            }
        }
    }

    private void normalButtonClick(BaseEventData e) {
        if (playerState == CharacterState.Neutral) {
            playerState = CharacterState.Startup;
            playerAnimator.Play("cr_mk_startup");
        }
    }

    private void specialButtonClick(BaseEventData e) {
        if (playerState == CharacterState.Recovery) {
            if (playerRecoveryFrame <= GameConfig.confirmWindowFrames) {
                // Within cancel window. Opponent may have blocked.
                playerState = CharacterState.Special;
                playerAnimator.Play("special");
                if (opponentState == CharacterState.HitStun) {
                    // Player performed special at the right time
                    hitConfirmCount++;
                    hitConfirmCountText.SetText(hitConfirmCount.ToString());
                    confirmFrameText.SetText(playerRecoveryFrame.ToString());
                    confirmFrameText.color = Color.green;
                }
                return;
            }
        }

        // Player either
        // a) missed cancellable window
        // b) pressed special at completely wrong time
        // Not changing player state in this case
        hitConfirmCount = 0;
        blockConfirmCount = 0;
        hitConfirmCountText.SetText(hitConfirmCount.ToString());
        blockConfirmCountText.SetText(blockConfirmCount.ToString());
        if (playerRecoveryFrame > 0) {
            // Show the player how late they activated special
            confirmFrameText.SetText(playerRecoveryFrame.ToString());
            confirmFrameText.color = Color.red;
        } else {
            confirmFrameText.SetText("");
        }
    }

    private void createButtonBinding(Button button, Action<BaseEventData> cb) {
        EventTrigger trigger = button.gameObject.AddComponent<EventTrigger>();
        var pointerDown = new EventTrigger.Entry();
        pointerDown.eventID = EventTriggerType.PointerDown;
        pointerDown.callback.AddListener((e) => cb(e));
        trigger.triggers.Add(pointerDown);
    }
}
