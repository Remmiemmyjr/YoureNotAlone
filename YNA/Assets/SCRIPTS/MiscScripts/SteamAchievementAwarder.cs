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
    ACH_TOGETHER_ONCE_MORE,
    ACH_FAREWELL_FRIEND,
    ACH_YOU_FOUND_HIM,
    ACH_CHAMPION_OF_THE_CRYPT,
    ACH_LEARNING_THE_ROPES,
    ACH_AMONG_THE_DARK,
}

public static class SteamAchievementAwarder
{
#if !DISABLESTEAMWORKS
    public static void AwardYoureNotAlone()
    {
        if (!CheckIfAwardValid())
            return;

        SteamUserStats.SetAchievement("YOURE_NOT_ALONE");
        SteamUserStats.StoreStats();
    }

    public static void AwardIntoTheDepths()
    {
        if (!CheckIfAwardValid())
            return;

        SteamUserStats.SetAchievement("INTO_THE_DEPTHS");
        SteamUserStats.StoreStats();
    }

    public static void AwardWeWereTethered()
    {
        if (!CheckIfAwardValid())
            return;

        SteamUserStats.SetAchievement("WE_WERE_TETHERED");
        SteamUserStats.StoreStats();
    }

    public static void AwardTogetherOnceMore()
    {
        if (!CheckIfAwardValid())
            return;

        SteamUserStats.SetAchievement("TOGETHER_ONCE_MORE");
        SteamUserStats.StoreStats();
    }

    public static void AwardFarewellFriend()
    {
        if (!CheckIfAwardValid())
            return;

        SteamUserStats.SetAchievement("FAREWELL_FRIEND");
        SteamUserStats.StoreStats();
    }

    public static void AwardYouFoundHim()
    {
        if (!CheckIfAwardValid())
            return;

        SteamUserStats.SetAchievement("YOU_FOUND_HIM");
        SteamUserStats.StoreStats();
    }

    public static void AwardChampionOfTheCrypt()
    {
        if (!CheckIfAwardValid())
            return;

        SteamUserStats.SetAchievement("CHAMPION_OF_THE_CRYPT");
        SteamUserStats.StoreStats();
    }

    public static void AwardLearningTheRopes()
    {
        if (!CheckIfAwardValid())
            return;

        SteamUserStats.SetAchievement("LEARNING_THE_ROPES");
        SteamUserStats.StoreStats();
    }

    public static void AwardAmongTheDark()
    {
        if (!CheckIfAwardValid())
            return;

        SteamUserStats.SetAchievement("AMONG_THE_DARK");
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
