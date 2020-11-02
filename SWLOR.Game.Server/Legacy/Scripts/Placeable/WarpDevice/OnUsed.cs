﻿using System.Linq;
using SWLOR.Game.Server.Core.NWScript.Enum;
using SWLOR.Game.Server.Core.NWScript.Enum.VisualEffect;
using SWLOR.Game.Server.Enumeration;
using SWLOR.Game.Server.Legacy.GameObject;
using SWLOR.Game.Server.Legacy.Service;
using SWLOR.Game.Server.Service;
using static SWLOR.Game.Server.Core.NWScript.NWScript;

namespace SWLOR.Game.Server.Legacy.Scripts.Placeable.WarpDevice
{
    public class OnUsed: IScript
    {
        public void SubscribeEvents()
        {
        }

        public void UnsubscribeEvents()
        {
        }

        public void Main()
        {
            NWPlayer oPC = GetLastUsedBy();

            if (GetIsInCombat(oPC) == true)
            {
                SendMessageToPC(oPC, "You are in combat.");
                return;
            }

            NWPlaceable self = OBJECT_SELF;
            var destination = self.GetLocalString("DESTINATION");
            var visualEffectID = self.GetLocalInt("VISUAL_EFFECT") > 0 ? (VisualEffect)self.GetLocalInt("VISUAL_EFFECT") : VisualEffect.None;
            var keyItemID = self.GetLocalInt("KEY_ITEM_ID");
            var missingKeyItemMessage = self.GetLocalString("MISSING_KEY_ITEM_MESSAGE");
            var isInstance = GetLocalBool(self, "INSTANCE") == true;
            var personalInstanceOnly = GetLocalBool(self, "PERSONAL_INSTANCE_ONLY");

            if (keyItemID > 0)
            {
                var keyItemType = (KeyItemType) keyItemID;
                if (!KeyItem.HasKeyItem(oPC, keyItemType))
                {
                    if (!string.IsNullOrWhiteSpace(missingKeyItemMessage))
                    {
                        oPC.SendMessage(missingKeyItemMessage);
                    }
                    else
                    {
                        oPC.SendMessage("You don't have the necessary key item to access that object.");
                    }

                    return;
                }
            }

            if (visualEffectID > 0)
            {
                ApplyEffectToObject(DurationType.Instant, EffectVisualEffect(visualEffectID), oPC.Object);
            }

            NWObject entranceWP = GetWaypointByTag(destination);
            var entranceArea = GetArea(entranceWP);
            var entranceName = GetName(entranceArea);
            var entranceResref = GetResRef(entranceArea);
            NWLocation location = GetLocation(entranceWP);

            if (!entranceWP.IsValid)
            {
                oPC.SendMessage("Cannot locate entrance waypoint. Inform an admin.");
                return;
            }

            if (isInstance)
            {
                var members = oPC.PartyMembers.Where(x =>
                {
                    var area = GetArea(x);
                    return GetLocalString(area, "ORIGINAL_RESREF") == entranceResref;
                }).ToList();

                // A party member is in an instance of this type already.
                // Prompt player to select which instance to enter.
                if (members.Count >= 1 && !personalInstanceOnly)
                {
                    oPC.SetLocalString("INSTANCE_RESREF", entranceResref);
                    oPC.SetLocalString("INSTANCE_DESTINATION_TAG", destination);
                    DialogService.StartConversation(oPC, self, "InstanceSelection");
                    return;
                }

                // Otherwise no instance exists yet or this instance only allows one player. Make a new one for this player.
                var instance = AreaService.CreateAreaInstance(oPC, entranceResref, entranceName, destination);
                location = GetLocalLocation(instance, "INSTANCE_ENTRANCE");
                PlayerService.SaveLocation(oPC);
            }

            oPC.AssignCommand(() =>
            {
                ActionJumpToLocation(location);
            });
        }
    }
}
