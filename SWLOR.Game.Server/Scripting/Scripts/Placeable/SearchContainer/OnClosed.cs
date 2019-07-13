﻿using SWLOR.Game.Server.Event;
using NWN;
using SWLOR.Game.Server.Scripting.Contracts;
using SWLOR.Game.Server.Service;


namespace SWLOR.Game.Server.Placeable.SearchContainer
{
    public class OnClosed: IScript
    {
        public void SubscribeEvents()
        {
        }

        public void UnsubscribeEvents()
        {
        }

        public void Main()
        {
            SearchService.OnChestClose((NWGameObject.OBJECT_SELF));
        }
    }
}
