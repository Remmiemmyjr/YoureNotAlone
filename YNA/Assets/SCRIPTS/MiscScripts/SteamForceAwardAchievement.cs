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
            case SteamAchievementIDs.ACH_FOUND_RADIO_TOWER:
                SteamAchievementAwarder.AwardFoundRadioTowers();
                break;

            case SteamAchievementIDs.ACH_FOUND_HELICOPTER:
                SteamAchievementAwarder.AwardFoundHelicopter();
                break;

            case SteamAchievementIDs.ACH_FOUND_RECEIVER:
                SteamAchievementAwarder.AwardCompletedGame();
                break;

            case SteamAchievementIDs.ACH_FOUND_ALL:
                SteamAchievementAwarder.AwardFoundAllRadarPoints();
                break;

            case SteamAchievementIDs.ACH_MISSION_ABANDON:
                SteamAchievementAwarder.AwardAbandonedMission();
                break;

            case SteamAchievementIDs.ACH_NEVER_HIT_WORLD:
                SteamAchievementAwarder.AwardNeverCollidedWithWorld();
                break;

            case SteamAchievementIDs.ACH_HIT_WORLD:
                SteamAchievementAwarder.AwardCollidedWithWorld();
                break;
        }
    }
#endif
}
