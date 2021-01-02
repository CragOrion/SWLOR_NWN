﻿using System;
using SWLOR.Game.Server.Core.NWScript;
using SWLOR.Game.Server.Core.NWScript.Enum;
using SWLOR.Game.Server.Core.NWScript.Enum.VisualEffect;
using SWLOR.Game.Server.Legacy.CustomEffect.Contracts;
using SWLOR.Game.Server.Legacy.Enumeration;
using SWLOR.Game.Server.Legacy.GameObject;
using SWLOR.Game.Server.Legacy.Service;
using PerkType = SWLOR.Game.Server.Legacy.Enumeration.PerkType;

namespace SWLOR.Game.Server.Legacy.CustomEffect
{
    public class MeditateEffect : ICustomEffectHandler
    {
        public CustomEffectCategoryType CustomEffectCategoryType => CustomEffectCategoryType.NormalEffect;
        public CustomEffectType CustomEffectType => CustomEffectType.Meditate;

        public string Apply(NWCreature oCaster, NWObject oTarget, int effectiveLevel)
        {
            NWPlayer player = oTarget.Object;

            player.AssignCommand(() =>
            {
                NWScript.ActionPlayAnimation(Animation.LoopingMeditate, 1.0f, 6.1f);
            });

            player.IsBusy = true;

            var data = $"{player.Position.X},{player.Position.Y},{player.Position.Z}";

            return data;
        }

        public void Tick(NWCreature oCaster, NWObject oTarget, int currentTick, int effectiveLevel, string data)
        {
            AbilityService.EndConcentrationEffect(oCaster);

            NWPlayer player = oTarget.Object;
            var meditateTick = oTarget.GetLocalInt("MEDITATE_TICK") + 1;

            // Pull original position from data
            var values = data.Split(',');
            var originalPosition = NWScript.Vector3
            (
                Convert.ToSingle(values[0]),
                Convert.ToSingle(values[1]),
                Convert.ToSingle(values[2])
            );

            // Check position
            var position = player.Position;

            if ((Math.Abs(position.X - originalPosition.X) > 0.01f ||
                 Math.Abs(position.Y - originalPosition.Y) > 0.01f ||
                 Math.Abs(position.Z - originalPosition.Z) > 0.01f) ||
                !CanMeditate(player) ||
                !player.IsValid)
            {
                player.IsBusy = false;
                CustomEffectService.RemovePCCustomEffect(player, CustomEffectType.Meditate);
                return;
            }

            player.IsBusy = true;

            player.AssignCommand(() =>
            {
                NWScript.ActionPlayAnimation(Animation.LoopingMeditate, 1.0f, 6.1f);
            });
            
            if (meditateTick >= 6)
            {
                var amount = CalculateAmount(player);

                AbilityService.RestorePlayerFP(player, amount);
                var vfx = NWScript.EffectVisualEffect(VisualEffect.Vfx_Imp_Head_Mind);
                NWScript.ApplyEffectToObject(DurationType.Instant, vfx, player);
                meditateTick = 0;
            }

            oTarget.SetLocalInt("MEDITATE_TICK", meditateTick);
        }

        public void WearOff(NWCreature oCaster, NWObject oTarget, int effectiveLevel, string data)
        {
            NWPlayer player = oTarget.Object;
            player.IsBusy = false;
            player.DeleteLocalInt("MEDITATE_TICK");
        }

        private int CalculateAmount(NWPlayer player)
        {
            var effectiveStats = PlayerStatService.GetPlayerItemEffectiveStats(player);
            var perkLevel = PerkService.GetCreaturePerkLevel(player, PerkType.Meditate);
            int amount;
            switch (perkLevel)
            {
                default:
                    amount = 6;
                    break;
                case 4:
                case 5:
                case 6:
                    amount = 10;
                    break;
                case 7:
                    amount = 14;
                    break;
            }
            amount += effectiveStats.Meditate;

            return amount;
        }

        public static bool CanMeditate(NWCreature meditator)
        {
            var canMeditate = !meditator.IsInCombat;

            var area = meditator.Area;
            foreach (var member in meditator.PartyMembers)
            {
                if (!member.Area.Equals(area)) continue;

                if (member.IsInCombat)
                {
                    canMeditate = false;
                    break;
                }
            }

            return canMeditate;
        }


        public string StartMessage => "You begin to meditate...";
        public string ContinueMessage => "";
        public string WornOffMessage => "You stop meditating.";
    }
}