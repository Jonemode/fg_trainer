public enum CharacterState
{
    Neutral,
    Startup,
    Active,
    Recovery,
    SpecialStartup,
    SpecialRecovery,
    HitStun,
    BlockStun,
    SpecialHitStun,
    WakeUp,
    Crouch
}

public enum Move
{
    None,
    Normal,
    Special
}

public enum SimMode
{
    PS4 = 0,
    PC = 1,
    HalfSpeed = 2,
    QuarterSpeed = 3
}
