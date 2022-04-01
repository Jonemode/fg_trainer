public static class GameConfig
{
    public const float baseFixedDeltaTime = 0.0167f;
    public const int baseFrameRate = 60;

    // Player frames
    public const int startupFrames = 6;
    public const int activeFrames = 2;
    public const int recoveryFrames = 25;
    public const int specialStartupFrames = 6;
    public const int specialRecoveryFrames = 34;
    public const int crouchFrames = 7;

    // Opponent frames
    public const int hitStunRecoveryFrames = 41;
    public const int blockStunRecoveryFrames = 23;
    public const int specialHitStunRecoveryFrames = 99;
    public const int wakeUpFrames = 51;

    public const int opponentDefendPercentage = 50;
    public const int ps4FrameLag = 3;

    // Window after player recovery start, which you can activate special and have it connect
    public const int confirmWindowFrames = 17;
    public const int stunAmount = 70;
}
