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
    private int playerSpecialStartupFrame = 0;
    private int playerSpecialRecoveryFrame = 0;
    private int playerSpecialActivateFrame = 0;
    private int opponentRecoveryFrame = 0;
    private int opponentSpecialRecoveryFrame = 0;

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
            case CharacterState.SpecialStartup:
                handleSpecialStartupFrame();
                break;
            case CharacterState.SpecialRecovery:
                handleSpecialRecoveryFrame();
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
            case CharacterState.SpecialHitStun:
                handleSpecialHitStunFrame();
                break;
            default:
                // in neutral, do nothing
                break;
        }
    }

    // Player is in Startup state
    private void handleStartupFrame() {
        if (playerStartupFrame >= GameConfig.startupFrames) {
            // Active frames start
            opponentStun.StunRetainmentFrame = 0;
            changePlayerState(CharacterState.Active);
            if (rnd.Next(1, 100) <= GameConfig.opponentDefendPercentage) {
                // Opponent blocked
                changeOpponentState(CharacterState.BlockStun);
            } else {
                // Opponent got hit
                opponentStun.UpdateStunOnHit(GameConfig.stunAmount);
                changeOpponentState(CharacterState.HitStun);
                counterHit.UpdateCounterHitTextOnHit();
            }
        } else {
            playerStartupFrame += 1;
        }
    }

    // Player is in Active state
    private void handleActiveFrame() {
        if (playerActiveFrame >= GameConfig.activeFrames) {
            changePlayerState(CharacterState.Recovery);
        } else {
            playerActiveFrame += 1;
        }
    }

    // Player is in Recovery state
    private void handleRecoveryFrame() {
        if (playerRecoveryFrame >= GameConfig.recoveryFrames) {
            changePlayerState(CharacterState.Neutral);
            if (opponentState == CharacterState.BlockStun) {
                // Player successfully did not activate special
                statsPanel.BlockConfirmCount++;
            }
        } else {
            playerRecoveryFrame += 1;
        }
    }

    // Player is in SpecialStartup state
    private void handleSpecialStartupFrame() {
       if (playerSpecialStartupFrame >= GameConfig.specialStartupFrames) {
            changePlayerState(CharacterState.SpecialRecovery);
            if (opponentState == CharacterState.HitStun) {
                changeOpponentState(CharacterState.SpecialHitStun);
            } else {
                changeOpponentState(CharacterState.BlockStun);
                statsPanel.ResetScore();
            }
       } else {
            playerSpecialStartupFrame += 1;
       }
    }

    // Player is in SpecialRecovery state
    private void handleSpecialRecoveryFrame() {
        if (playerSpecialRecoveryFrame >= GameConfig.specialRecoveryFrames) {
            changePlayerState(CharacterState.Neutral);
            if (opponentState == CharacterState.SpecialHitStun) {
                // Player performed special at the right time
                statsPanel.HitConfirmCount++;
            } else {
                statsPanel.ResetScore();
            }
            statsPanel.UpdateConfirmFrameText(playerSpecialActivateFrame, opponentState, simDropdown.IsPS4Mode());
            playerSpecialActivateFrame = 0;
        } else {
            playerSpecialRecoveryFrame += 1;
        }
    }

    // Opponent is in HitStun state
    private void handleHitStunFrame() {
        if (opponentRecoveryFrame >= GameConfig.hitStunRecoveryFrames) {
            changeOpponentState(CharacterState.Neutral);
            // Player didn't execute special and they should have
            statsPanel.ResetScore();
        } else {
            opponentRecoveryFrame += 1;
        }
    }

    // Opponent is in BlockStun state
    private void handleBlockStunFrame() {
        if (opponentRecoveryFrame >= GameConfig.blockStunRecoveryFrames) {
            changeOpponentState(CharacterState.Neutral);
        } else {
            opponentRecoveryFrame += 1;
        }
    }

    // Opponent is in SpecialHitStun state
    private void handleSpecialHitStunFrame() {
        if (opponentSpecialRecoveryFrame >= GameConfig.specialHitStunRecoveryFrames) {
            changeOpponentState(CharacterState.Neutral);
        } else {
            opponentSpecialRecoveryFrame += 1;
        }
    }

    private void normalButtonClick(BaseEventData e) {
        if (playerState == CharacterState.Neutral) {
            changePlayerState(CharacterState.Startup);
        }
    }

    private void specialButtonClick(BaseEventData e) {
        if (playerState == CharacterState.Neutral || 
            (playerState == CharacterState.Recovery && playerRecoveryFrame <= simDropdown.GetSimulatedConfirmWindow())) {
            playerSpecialActivateFrame = playerRecoveryFrame;
            changePlayerState(CharacterState.SpecialStartup);
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
    
    private void changePlayerState(CharacterState newState) {
        playerStartupFrame = 0;
        playerActiveFrame = 0;
        playerRecoveryFrame = 0;
        playerSpecialStartupFrame = 0;
        playerSpecialRecoveryFrame = 0;
        playerState = newState;
        switch (newState) {
            case CharacterState.Neutral:
                playerAnimator.Play("idle");
                break;
            case CharacterState.Startup:
                playerAnimator.Play("cr_mk_startup");
                break;
            case CharacterState.Active:
                playerAnimator.Play("cr_mk_active");
                break;
            case CharacterState.Recovery:
                playerAnimator.Play("cr_mk_recovery");
                break;
            case CharacterState.SpecialStartup:
                playerAnimator.Play("special");
                break;
            case CharacterState.SpecialRecovery:
                break;
            default:
                break;
        }
    }

    private void changeOpponentState(CharacterState newState) {
        opponentRecoveryFrame = 0;
        opponentSpecialRecoveryFrame = 0;
        opponentState = newState;
        switch (newState) {
            case CharacterState.Neutral:
                opponentAnimator.Play("idle");
                break;
            case CharacterState.BlockStun:
                opponentAnimator.Play("block");
                break;
            case CharacterState.HitStun:
                opponentAnimator.Play("hit");
                soundSystem.PlayNormalAttackHit();
                break;
            case CharacterState.SpecialHitStun:
                opponentAnimator.Play("special_hit");
                soundSystem.PlaySpecialAttackHit();
                break;
            default:
                break;
        }
    }
}
