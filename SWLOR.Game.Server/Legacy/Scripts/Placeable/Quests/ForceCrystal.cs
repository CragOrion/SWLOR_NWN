﻿using System;
using SWLOR.Game.Server.Core.NWScript;
using SWLOR.Game.Server.Legacy.GameObject;
using SWLOR.Game.Server.Legacy.Service;
using SWLOR.Game.Server.Service;

namespace SWLOR.Game.Server.Legacy.Scripts.Placeable.Quests
{
    public class ForceCrystal: IScript
    {
        public void SubscribeEvents()
        {
        }

        public void UnsubscribeEvents()
        {
        }

        public void Main()
        {
            const int QuestID = 30;
            NWPlaceable crystal = NWScript.OBJECT_SELF;
            NWPlayer player = NWScript.GetLastUsedBy();

            // Check player's current quest state. If they aren't on stage 2 of the quest only show a message.
            var status = DataService.PCQuestStatus.GetByPlayerAndQuestID(player.GlobalID, QuestID);
            
            if (status.QuestState != 2)
            {
                player.SendMessage("The crystal glows quietly...");
                return;
            }

            // Player is on stage 2, so they're able to click the crystal, get a cluster, complete the quest, and teleport back to the cavern.
            var type = crystal.GetLocalInt("CRYSTAL_COLOR_TYPE");
            string cluster;

            switch (type)
            {
                case 1: cluster = "c_cluster_blue"; break; // Blue
                case 2: cluster = "c_cluster_red"; break; // Red
                case 3: cluster = "c_cluster_green"; break; // Green 
                case 4: cluster = "c_cluster_yellow"; break; // Yellow
                default: throw new Exception("Invalid crystal color type.");
            }

            NWScript.CreateItemOnObject(cluster, player);

            var quest = Quest.GetQuestById(QuestID.ToString()); // todo need to update this to new system
            quest.Advance(player, crystal);

            // Hide the "Source of Power?" placeable so the player can't use it again.
            ObjectVisibilityService.AdjustVisibility(player, "81533EBB-2084-4C97-B004-8E1D8C395F56", false);

            NWObject tpWP = NWScript.GetObjectByTag("FORCE_QUEST_LANDING");
            player.AssignCommand(() => NWScript.ActionJumpToLocation(tpWP.Location));
            
            // Notify the player that new lightsaber perks have unlocked.
            player.FloatingText("You have unlocked the Lightsaber Blueprints perk. Find this under the Engineering category in your perks menu.");
        }
    }
}
