﻿using SWLOR.Game.Server.Core.NWScript.Enum;
using SWLOR.Game.Server.Legacy.GameObject;
using static SWLOR.Game.Server.Core.NWScript.NWScript;

namespace SWLOR.Game.Server.Legacy.AI
{
    public class DancerBehaviour : StandardBehaviour
    {
        public override void OnHeartbeat(NWCreature self)
        {
            base.OnHeartbeat(self);

            Dance(self);

        }

        private void Dance(NWCreature self)
        {
            ActionPlayAnimation(Animation.FireForgetDodgeSide);
            ActionPlayAnimation(Animation.FireForgetSpasm, 3.0F);
            ActionPlayAnimation(Animation.FireForgetVictory3, 3.0F);
            ActionPlayAnimation(Animation.FireForgetDodgeDuck);
            ActionPlayAnimation(Animation.FireForgetDodgeSide);
            ActionPlayAnimation(Animation.FireForgetVictory2, 3.0F);
            ActionPlayAnimation(Animation.FireForgetDodgeDuck);
            ActionPlayAnimation(Animation.FireForgetSpasm, 3.0F);
            ActionPlayAnimation(Animation.LoopingPauseDrunk, 3.0F, 1.0F);
            ActionPlayAnimation(Animation.LoopingConjure1, 3.0F, 0.5F);
            ActionPlayAnimation(Animation.FireForgetDodgeSide);
            ActionPlayAnimation(Animation.LoopingPauseDrunk, 3.0F, 1.0F);
            ActionPlayAnimation(Animation.LoopingConjure2, 3.0F, 0.5F);
            ActionPlayAnimation(Animation.FireForgetDodgeSide);
            ActionPlayAnimation(Animation.FireForgetVictory1, 3.0F);
            ActionDoCommand(() => SetCommandable(true));
            SetCommandable(false);
        }
    }
}