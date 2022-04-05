using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MasterControl : MonoBehaviour
{
    [SerializeField]
    public Button normalButton;

    [SerializeField]
    public Button specialButton;

    [SerializeField]
    public Button resetHighScoreButton;

    [SerializeField]
    public GameObject player;

    [SerializeField]
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

    [SerializeField]
    public Animator playerAnimator;

    [SerializeField]
    public Animator opponentAnimator;

    private CharacterState playerState;
    private CharacterState opponentState;

    // Internally we index from 0 for the first frame
    private int playerStartupFrame = 0;
    private int playerActiveFrame = 0;
    private int playerRecoveryFrame = 0;
    private int playerSpecialStartupFrame = 0;
    private int playerSpecialRecoveryFrame = 0;
    private int playerSpecialActivateFrame = 0;
    private int playerCrouchFrame = 0;
    private int opponentRecoveryFrame = 0;
    private int opponentSpecialRecoveryFrame = 0;
    private int opponentWakeUpFrame = 0;

    // These handle-in-frame variables are set high and tick towards -1.
    // At 0 they activate the action, and then tick to -1 until the user presses the button again.
    private int handleNormalButtonClickInFrame = -1;
    private int handleSpecialButtonClickInFrame = -1;

    private static System.Random rnd = new System.Random();

    // Start is called before the first frame update
    void Start() {
        Time.fixedDeltaTime = GameConfig.baseFixedDeltaTime;
        Application.targetFrameRate = GameConfig.baseFrameRate;

        playerState = CharacterState.Neutral;
        opponentState = CharacterState.Neutral;

        // Setup button clicks
        createButtonBinding(normalButton, normalButtonClickTrigger);
        createButtonBinding(specialButton, specialButtonClickTrigger);
        resetHighScoreButton.onClick.AddListener(resetHighScoreClick);

        // Setup initial character state
        changePlayerState(CharacterState.Neutral);
        changeOpponentState(CharacterState.Neutral);
    }

    void FixedUpdate() {
        //Debug.Log((int)(1f / Time.unscaledDeltaTime));

        // Process button presses
        if (handleNormalButtonClickInFrame == 0) {
            normalButtonClickAction();
        }
        if (handleSpecialButtonClickInFrame == 0) {
            specialButtonClickAction();
        }
        if (handleNormalButtonClickInFrame >= 0) {
            handleNormalButtonClickInFrame -= 1;
        }
        if (handleSpecialButtonClickInFrame >= 0) {
            handleSpecialButtonClickInFrame -= 1;
        }

        // Process player state
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
            case CharacterState.Crouch:
                handleCrouchFrame();
                break;
            default:
                // in neutral, do nothing
                break;
        }

        // Process opponent state
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
            case CharacterState.WakeUp:
                handleWakeUpFrame();
                break;
            default:
                // in neutral, do nothing
                break;
        }
    }

    // Player is in Crouch state
    private void handleCrouchFrame() {
        if (playerCrouchFrame >= GameConfig.crouchFrames) {
            // Startup frames begin
            changePlayerState(CharacterState.Startup);
        } else {
            playerCrouchFrame += 1;
        }
    }

    // Player is in Startup state
    private void handleStartupFrame() {
        if (playerStartupFrame >= GameConfig.startupFrames) {
            // Active frames start
            changePlayerState(CharacterState.Active);
            if (rnd.Next(1, 100) <= GameConfig.opponentDefendPercentage) {
                // Opponent blocked
                changeOpponentState(CharacterState.BlockStun);
            } else {
                // Opponent got hit
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
                statsPanel.ResetCurrentScore();
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
                statsPanel.AddHitConfirm(simDropdown.GetSimMode());
            } else {
                statsPanel.ResetCurrentScore();
            }
            statsPanel.UpdateConfirmFrameText(playerSpecialActivateFrame, opponentState);
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
            statsPanel.ResetCurrentScore();
        } else {
            opponentRecoveryFrame += 1;
        }
    }

    // Opponent is in BlockStun state
    private void handleBlockStunFrame() {
        if (opponentRecoveryFrame >= GameConfig.blockStunRecoveryFrames) {
            changeOpponentState(CharacterState.Neutral);
            if (playerState == CharacterState.Recovery) {
                // Player successfully did not activate special
                statsPanel.AddBlockConfirm(simDropdown.GetSimMode());
            }
        } else {
            opponentRecoveryFrame += 1;
        }
    }

    // Opponent is in SpecialHitStun state
    private void handleSpecialHitStunFrame() {
        if (opponentSpecialRecoveryFrame >= GameConfig.specialHitStunRecoveryFrames) {
            changeOpponentState(CharacterState.WakeUp);
        } else {
            opponentSpecialRecoveryFrame += 1;
        }
    }

    // Opponent is in WakeUp state
    private void handleWakeUpFrame() {
        if (opponentWakeUpFrame >= GameConfig.wakeUpFrames) {
            changeOpponentState(CharacterState.Neutral);
        } else {
            opponentWakeUpFrame += 1;
        }
    }

    private void normalButtonClickTrigger(BaseEventData e) {
        if (handleNormalButtonClickInFrame == -1) {
            if (simDropdown.GetSimMode() == SimMode.PS4) {
                handleNormalButtonClickInFrame = GameConfig.ps4FrameLag;
            } else {
                handleNormalButtonClickInFrame = 0;
            }
        }
    }

    private void normalButtonClickAction() {
        if (playerState == CharacterState.Neutral && opponentState == CharacterState.Neutral) {
            changePlayerState(CharacterState.Crouch);
        }
    }

    private void specialButtonClickTrigger(BaseEventData e) {
        if (handleSpecialButtonClickInFrame == -1) {
            if (simDropdown.GetSimMode() == SimMode.PS4) {
                handleSpecialButtonClickInFrame = GameConfig.ps4FrameLag;
            } else {
                handleSpecialButtonClickInFrame = 0;
            }
        }
    }

    private void specialButtonClickAction() {
        if (playerState == CharacterState.SpecialStartup || playerState == CharacterState.SpecialRecovery) {
            // User double clicked, just return
            return;
        }

        if (playerState == CharacterState.Neutral ||
            (playerState == CharacterState.Recovery && playerRecoveryFrame < GameConfig.confirmWindowFrames)) {
            playerSpecialActivateFrame = playerRecoveryFrame;
            changePlayerState(CharacterState.SpecialStartup);
        } else {
            statsPanel.ResetCurrentScore();
        }
    }

    private void resetHighScoreClick() {
        statsPanel.ResetHighScore(simDropdown.GetSimMode());
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
        playerCrouchFrame = 0;
        playerState = newState;
        switch (newState) {
            case CharacterState.Neutral:
                playerAnimator.Play("idle");
                break;
            case CharacterState.Startup:
                playerAnimator.Play("cr_mk");
                soundSystem.PlayKarinAttackVoice();
                break;
            case CharacterState.Active:
                break;
            case CharacterState.Recovery:
                break;
            case CharacterState.SpecialStartup:
                playerAnimator.Play("special");
                soundSystem.PlayKarinSpecialAttackVoice();
                break;
            case CharacterState.SpecialRecovery:
                break;
            case CharacterState.Crouch:
                playerAnimator.Play("crouch");
                break;
            default:
                break;
        }
    }

    private void changeOpponentState(CharacterState newState) {
        opponentRecoveryFrame = 0;
        opponentSpecialRecoveryFrame = 0;
        opponentWakeUpFrame = 0;
        opponentState = newState;
        switch (newState) {
            case CharacterState.Neutral:
                opponentAnimator.Play("idle");
                break;
            case CharacterState.BlockStun:
                // Specify layer and normalizedTime to start the animation from the start if it's currently playing
                opponentStun.UpdateStunOnBlock();
                opponentAnimator.Play("block", -1, 0f);
                break;
            case CharacterState.HitStun:
                opponentStun.UpdateStunOnHit(GameConfig.stunAmount);
                opponentAnimator.Play("hit");
                soundSystem.PlayNormalAttackHit();
                soundSystem.PlayDanHitVoice();
                break;
            case CharacterState.SpecialHitStun:
                opponentStun.UpdateStunOnHit(GameConfig.stunAmount);
                opponentAnimator.Play("special_hit");
                soundSystem.PlaySpecialAttackHit();
                soundSystem.PlayDanSpecialHitVoice();
                break;
            case CharacterState.WakeUp:
                opponentAnimator.Play("wake_up");
                break;
            default:
                break;
        }
    }
}
