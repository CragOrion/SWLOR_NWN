using SWLOR.Game.Server.Enumeration;
using SWLOR.Game.Server.Quest;

namespace SWLOR.Game.Server.Scripts.Quest.GuildTasks.EngineeringGuild
{
    public class TrainingFoilStaffIYellow: AbstractQuest
    {
        public TrainingFoilStaffIYellow()
        {
            CreateQuest(444, "Engineering Guild Task: 1x Training Foil Staff I (Yellow)", "eng_tsk_444")
                .IsRepeatable()

                .AddObjectiveCollectItem(1, "saberstaff_y_1", 1, true)

                .AddRewardGold(225)
                .AddRewardGuildPoints(GuildType.EngineeringGuild, 50);
        }
    }
}
