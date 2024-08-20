// Project: We're Tethered Together
// File: SteamForceAchievement.cs
// Author/s: Chris Onorati
//           Emmy Berg
//
// Desc: Award achievements
//
// Notes:
//  - 
//
// Last Edit: 2/14/2024
//
//*************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamForceAwardAchievement : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Award the achievement on start?")]
    bool m_bAwardOnStart = true;

    [SerializeField]
    [Tooltip("Achievement to award.")]
    SteamAchievementIDs m_AchievementToAward;

#if !DISABLESTEAMWORKS
    void Start()
    {
        if (m_bAwardOnStart)
            AwardAchievement();
    }

    public void AwardAchievement()
    {
        switch (m_AchievementToAward)
        {
            case SteamAchievementIDs.ACH_YOURE_NOT_ALONE:
                SteamAchievementAwarder.AwardYoureNotAlone();
                break;

            case SteamAchievementIDs.ACH_INTO_THE_DEPTHS:
                SteamAchievementAwarder.AwardIntoTheDepths();
                break;

            case SteamAchievementIDs.ACH_WE_WERE_TETHERED:
                SteamAchievementAwarder.AwardWeWereTethered();
                break;

            case SteamAchievementIDs.ACH_TOGETHER_ONCE_MORE:
                SteamAchievementAwarder.AwardTogetherOnceMore();
                break;

            case SteamAchievementIDs.ACH_FAREWELL_FRIEND:
                SteamAchievementAwarder.AwardFarewellFriend();
                break;

            case SteamAchievementIDs.ACH_YOU_FOUND_HIM:
                SteamAchievementAwarder.AwardYouFoundHim();
                break;

            case SteamAchievementIDs.ACH_CHAMPION_OF_THE_CRYPT:
                SteamAchievementAwarder.AwardChampionOfTheCrypt();
                break;

            case SteamAchievementIDs.ACH_LEARNING_THE_ROPES:
                SteamAchievementAwarder.AwardLearningTheRopes();
                break;

            case SteamAchievementIDs.ACH_AMONG_THE_DARK:
                SteamAchievementAwarder.AwardAmongTheDark();
                break;

            case SteamAchievementIDs.ACH_GOODBYE_KISS:
                SteamAchievementAwarder.AwardGoodbyeKiss();
                break;
        }

        Debug.Log(m_AchievementToAward);
    }
#endif
}
