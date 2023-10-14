using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if !DISABLESTEAMWORKS
using Steamworks;
#endif

public enum SteamAchievementIDs
{
    ACH_YOURE_NOT_ALONE,
    ACH_INTO_THE_DEPTHS,
    ACH_WE_WERE_TETHERED,
    ACH_TETHERED_ONCE_MORE,
    ACH_TIL_THE_END_TOGETHER,
    ACH_YOU_FOUND_HIM,
    ACH_CHAMPION_OF_THE_CRYPT,
    ACH_LEARNING_THE_ROPES,
    ACH_INTO_THE_DARKNESS,
}

public static class SteamAchievementAwarder
{
#if !DISABLESTEAMWORKS
    public static void AwardYoureNotAlone()
    {
        if (!CheckIfAwardValid())
            return;

        SteamUserStats.SetAchievement("ACH_YOURE_NOT_ALONE");
        SteamUserStats.StoreStats();
    }

    public static void AwardIntoTheDepths()
    {
        if (!CheckIfAwardValid())
            return;

        SteamUserStats.SetAchievement("ACH_FOUND_HELICOPTER");
        SteamUserStats.StoreStats();
    }

    public static void AwardWeWereTethered()
    {
        if (!CheckIfAwardValid())
            return;

        SteamUserStats.SetAchievement("ACH_FOUND_RECEIVER");
        SteamUserStats.StoreStats();
    }

    public static void AwardTetheredOnceMore()
    {
        if (!CheckIfAwardValid())
            return;

        SteamUserStats.SetAchievement("ACH_FOUND_ALL");
        SteamUserStats.StoreStats();
    }

    public static void AwardTilTheEndTogether()
    {
        if (!CheckIfAwardValid())
            return;

        SteamUserStats.SetAchievement("ACH_MISSION_ABANDON");
        SteamUserStats.StoreStats();
    }

    public static void AwardYouFoundHim()
    {
        if (!CheckIfAwardValid())
            return;

        SteamUserStats.SetAchievement("ACH_NEVER_HIT_WORLD");
        SteamUserStats.StoreStats();
    }

    public static void AwardChampionOfTheCrypt()
    {
        if (!CheckIfAwardValid())
            return;

        SteamUserStats.SetAchievement("ACH_HIT_WORLD");
        SteamUserStats.StoreStats();
    }

    public static void AwardLearningTheRopes()
    {
        if (!CheckIfAwardValid())
            return;

        SteamUserStats.SetAchievement("ACH_HIT_WORLD");
        SteamUserStats.StoreStats();
    }

    public static void AwardIntoTheDarkness()
    {
        if (!CheckIfAwardValid())
            return;

        SteamUserStats.SetAchievement("ACH_HIT_WORLD");
        SteamUserStats.StoreStats();
    }

    static bool CheckIfAwardValid()
    {
        if (!SteamManager.Initialized)
        {
            return false;
        }

        //if (GlobalPlayerSettings.PlayerSettings.WereCheatsEverActive())
        //    return false;

        return true;
    }
#endif
}
