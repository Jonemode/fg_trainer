using System;

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

public enum PlayMode
{
    Training = 0,
    Ranked = 1
}

public enum SimMode
{
    PS4 = 0,
    PC = 1
}

public enum SimSpeed
{
    OneHundredPercent = 0,
    NinetyPercent = 1,
    EightyPercent = 2,
    SeventyPercent = 3,
    SixtyPercent = 4,
    FiftyPercent = 5,
    FortyPercent = 6,
}

public enum PlayerRank
{
    Rookie = 0,
    Bronze = 1,
    SuperBronze = 2,
    UltraBronze = 3,
    Silver = 4,
    SuperSilver = 5,
    UltraSilver = 6,
    Gold = 7,
    SuperGold = 8,
    UltraGold = 9,
    Platinum = 10,
    SuperPlatinum = 11,
    UltraPlatinum = 12,
    Diamond = 13,
    SuperDiamond = 14,
    UltraDiamond = 15,
    Master = 16,
    GrandMaster = 17,
    UltimateGrandMaster = 18,
    Warlord = 19
}

public class ConfirmFrameEventArgs : EventArgs
{
    public ConfirmFrameEventArgs(int specialActivateFrame) : base() {
        SpecialActivateFrame = specialActivateFrame;
    }

    public int SpecialActivateFrame { get; set; }
}
