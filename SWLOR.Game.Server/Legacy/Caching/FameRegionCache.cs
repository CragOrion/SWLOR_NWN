using SWLOR.Game.Server.Legacy.Data.Entity;

namespace SWLOR.Game.Server.Legacy.Caching
{
    public class FameRegionCache: CacheBase<FameRegion>
    {
        protected override void OnCacheObjectSet(FameRegion entity)
        {
        }

        protected override void OnCacheObjectRemoved(FameRegion entity)
        {
        }

        protected override void OnSubscribeEvents()
        {
        }

        public FameRegion GetByID(int id)
        {
            return (FameRegion)ByID[id].Clone();
        }
    }
}