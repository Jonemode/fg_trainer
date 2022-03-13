using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MasterControl : MonoBehaviour
{
    public Button normalButton;
    public Button specialButton;
    public GameObject player;
    public GameObject opponent;

    [SerializeField]
    public OpponentStun opponentStun;
    
    [SerializeField]
    public CounterHit counterHit;
    
    [SerializeField]
    public SoundSystem soundSystem;
    
    [SerializeField]
    public SimDropdown simDropdown;
    
    [SerializeField]
    public StatsPanel statsPanel;

    private Animator playerAnimator;
    private Animator opponentAnimator;

    private CharacterState playerState;
    private CharacterState opponentState;

    // Internally we index from 0 for the first frame
    private int playerStartupFrame = 0;
    private int playerActiveFrame = 0;
    private int playerRecoveryFrame = 0;
    private int playerSpecialFrame = 0;
    private int playerSpecialActivateFrame = 0;
    private int opponentRecoveryFrame = 0;
    private bool normalAttackHit = false;

    private static System.Random rnd = new System.Random();

    // Start is called before the first frame update
    void Start() {
        Time.fixedDeltaTime = 0.0167f;
        Application.targetFrameRate = 60;

        playerState = CharacterState.Neutral;
        opponentState = CharacterState.Neutral;

        playerAnimator = player.GetComponent<Animator>();
        opponentAnimator = opponent.GetComponent<Animator>();

        // Setup button clicks
        createButtonBinding(normalButton, normalButtonClick);
        createButtonBinding(specialButton, specialButtonClick);
    }

    void FixedUpdate() {
        //Debug.Log((int)(1f / Time.unscaledDeltaTime));
        switch (playerState) {
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
        if (playerStartupFrame >= GameConfig.startupFrames) {
            // Active frames start
            playerStartupFrame = 0;
            opponentStun.StunRetainmentFrame = 0;
            playerState = CharacterState.Active;
            playerAnimator.Play("cr_mk_active");
            if (rnd.Next(1, 100) <= GameConfig.opponentDefendPercentage) {
                // Opponent blocked
                opponentAnimator.Play("block");
                opponentState = CharacterState.BlockStun;
            } else {
                // Opponent got hit
                opponentAnimator.Play("hit");
                opponentStun.UpdateStunOnHit(GameConfig.stunAmount);
                opponentState = CharacterState.HitStun;
                counterHit.UpdateCounterHitTextOnHit();
                soundSystem.PlayNormalAttackHit();
                normalAttackHit = true;
            }
        } else {
            playerStartupFrame += 1;
        }
    }

    private void handleActiveFrame() {
        if (playerActiveFrame >= GameConfig.activeFrames) {
            playerActiveFrame = 0;
            playerState = CharacterState.Recovery;
            playerAnimator.Play("cr_mk_recovery");
        } else {
            playerActiveFrame += 1;
        }
    }

    private void handleRecoveryFrame() {
        if (playerRecoveryFrame >= GameConfig.recoveryFrames) {
            playerRecoveryFrame = 0;
            playerState = CharacterState.Neutral;
            playerAnimator.Play("idle");
            if (normalAttackHit) {
                // Player didn't execute special and they should have
                statsPanel.ResetScore();
            } else {
                // Player successfully did not activate special
                statsPanel.BlockConfirmCount++;
            }
            normalAttackHit = false;
        } else {
            playerRecoveryFrame += 1;
        }
    }

    private void handleSpecialFrame() {
        if (playerSpecialFrame >= GameConfig.specialFrames) {
            playerSpecialFrame = 0;
            playerState = CharacterState.Neutral;
            playerAnimator.Play("idle");
            if (normalAttackHit) {
                // Player performed special at the right time
                statsPanel.HitConfirmCount++;
            } else {
                statsPanel.ResetScore();
            }
            statsPanel.UpdateConfirmFrameText(playerSpecialActivateFrame, normalAttackHit, simDropdown.IsPS4Mode());
            normalAttackHit = false;
            playerSpecialActivateFrame = 0;
        } else {
            playerSpecialFrame += 1;
        }
    }

    private void handleHitStunFrame() {
        if (opponentRecoveryFrame >= GameConfig.hitStunRecoveryFrames) {
            opponentRecoveryFrame = 0;
            opponentState = CharacterState.Neutral;
            opponentAnimator.Play("idle");
        } else {
            opponentRecoveryFrame += 1;
        }
    }

    private void handleBlockStunFrame() {
        if (opponentRecoveryFrame >= GameConfig.blockStunRecoveryFrames) {
            opponentRecoveryFrame = 0;
            opponentState = CharacterState.Neutral;
            opponentAnimator.Play("idle");
        } else {
            opponentRecoveryFrame += 1;
        }
    }

    private void normalButtonClick(BaseEventData e) {
        if (playerState == CharacterState.Neutral) {
            playerState = CharacterState.Startup;
            playerAnimator.Play("cr_mk_startup");
        }
    }

    private void specialButtonClick(BaseEventData e) {
        if (playerState == CharacterState.Neutral || 
            (playerState == CharacterState.Recovery && playerRecoveryFrame <= simDropdown.GetSimulatedConfirmWindow())) {
            playerSpecialActivateFrame = playerRecoveryFrame;
            playerRecoveryFrame = 0;
            playerState = CharacterState.Special;
            playerAnimator.Play("special");
        } else {
            statsPanel.ResetScore();
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
