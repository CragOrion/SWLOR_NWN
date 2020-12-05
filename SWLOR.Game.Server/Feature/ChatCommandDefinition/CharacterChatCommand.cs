﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SWLOR.Game.Server.Core;
using SWLOR.Game.Server.Core.NWNX;
using SWLOR.Game.Server.Core.NWScript.Enum;
using SWLOR.Game.Server.Enumeration;
using SWLOR.Game.Server.Feature.DialogDefinition;
using SWLOR.Game.Server.Legacy.Conversation;
using SWLOR.Game.Server.Service;
using SWLOR.Game.Server.Service.ChatCommandService;
using static SWLOR.Game.Server.Core.NWScript.NWScript;
using Dialog = SWLOR.Game.Server.Service.Dialog;
using Player = SWLOR.Game.Server.Entity.Player;

namespace SWLOR.Game.Server.Feature.ChatCommandDefinition
{
    public class CharacterChatCommand: IChatCommandListDefinition
    {
        public Dictionary<string, ChatCommandDetail> BuildChatCommands()
        {
            var builder = new ChatCommandBuilder();

            builder.Create("cdkey")
                .Description("Displays your public CD key.")
                .Permissions(AuthorizationLevel.All)
                .Action((user, target, location, args) =>
                {
                    var cdKey = GetPCPublicCDKey(user);
                    SendMessageToPC(user, "Your public CD Key is: " + cdKey);
                });

            builder.Create("rest")
                .Description("Opens the rest menu.")
                .Permissions(AuthorizationLevel.Player)
                .Action((user, target, location, args) =>
                {
                    Dialog.StartConversation(user, user, nameof(RestMenu));
                });


            builder.Create("save")
                .Description("Manually saves your character. Your character also saves automatically every few minutes.")
                .Permissions(AuthorizationLevel.Player)
                .Action((user, target, location, args) =>
                {
                    ExportSingleCharacter(user);
                    SendMessageToPC(user, "Character saved successfully.");
                });

            builder.Create("skills")
                .Description("Opens the skills menu.")
                .Permissions(AuthorizationLevel.Player)
                .Action((user, target, location, args) =>
                {
                    Dialog.StartConversation(user, user, nameof(ViewSkillsDialog));
                });

            DeleteCommand(builder);
            LanguageCommand(builder);
            CustomizeCommand(builder);
            ToggleHelmet(builder);

            return builder.Build();
        }

        private static void LanguageCommand(ChatCommandBuilder builder)
        {
            builder.Create("language")
                .Description("Switches the active language. Use /language help for more information.")
                .Permissions(AuthorizationLevel.Player)
                .Validate((user, args) =>
                {
                    if (args.Length < 1)
                    {
                        return ColorToken.Red("Please enter /language help for more information on how to use this command.");
                    }

                    return string.Empty;
                })
                .Action((user, target, location, args) =>
                {
                    var command = args[0].ToLower();
                    var race = GetRacialType(user);
                    var languages = Language.Languages;

                    if (command == "help")
                    {
                        var commands = new List<string>
                        {
                            "help: Displays this help text."
                        };

                        foreach (var language in languages)
                        {
                            var chatText = language.ChatNames.ElementAt(0);
                            var count = language.ChatNames.Count();

                            for (var x = 1; x < count; x++)
                            {
                                chatText += ", " + language.ChatNames.ElementAt(x);
                            }

                            commands.Add($"{chatText}: Sets the active language to {language.ProperName}.");
                        }

                        SendMessageToPC(user, commands.Aggregate((a, b) => a + '\n' + b));
                        return;
                    }

                    // Wookiees cannot speak any language besides Shyriiwook.
                    if (race == RacialType.Wookiee &&
                        command != SkillType.Shyriiwook.ToString().ToLower())
                    {
                        Language.SetActiveLanguage(user, SkillType.Shyriiwook);
                        SendMessageToPC(user, ColorToken.Red("Wookiees can only speak Shyriiwook."));
                        return;
                    }


                    foreach (var language in Language.Languages)
                    {
                        if (language.ChatNames.Contains(command))
                        {
                            Language.SetActiveLanguage(user, language.Skill);
                            SendMessageToPC(user, $"Set active language to {language.ProperName}.");
                            return;
                        }
                    }

                    SendMessageToPC(user, ColorToken.Red($"Unknown language {command}."));
                });
        }

        private static void DeleteCommand(ChatCommandBuilder builder)
        {
            builder.Create("delete")
                .Description("Permanently deletes your character.")
                .Permissions(AuthorizationLevel.Player)
                .Validate((user, args) =>
                {
                    if (!GetIsPC(user) || GetIsDM(user))
                        return "You can only delete a player character.";

                    var cdKey = GetPCPublicCDKey(user);
                    var enteredCDKey = args.Length > 0 ? args[0] : string.Empty;

                    if (cdKey != enteredCDKey)
                    {
                        return "Invalid CD key entered. Please enter the command as follows: \"/delete <CD Key>\". You can retrieve your CD key with the /CDKey chat command.";
                    }

                    return string.Empty;
                })
                .Action((user, target, location, args) =>
                {
                    var lastSubmission = GetLocalString(user, "DELETE_CHARACTER_LAST_SUBMISSION");
                    var isFirstSubmission = true;

                    // Check for the last submission, if any.
                    if (!string.IsNullOrWhiteSpace(lastSubmission))
                    {
                        // Found one, parse it.
                        var dateTime = DateTime.Parse(lastSubmission);
                        if (DateTime.UtcNow <= dateTime.AddSeconds(30))
                        {
                            // Player submitted a second request within 30 seconds of the last one. 
                            // This is a confirmation they want to delete.
                            isFirstSubmission = false;
                        }
                    }

                    // Player hasn't submitted or time has elapsed
                    if (isFirstSubmission)
                    {
                        SetLocalString(user, "DELETE_CHARACTER_LAST_SUBMISSION", DateTime.UtcNow.ToString(CultureInfo.InvariantCulture));
                        FloatingTextStringOnCreature("Please confirm your deletion by entering another \"/delete <CD Key>\" command within 30 seconds.", user, false);
                    }
                    else
                    {
                        var playerID = GetObjectUUID(user);
                        var entity = DB.Get<Player>(playerID);
                        entity.IsDeleted = true;
                        DB.Set(playerID, entity);

                        BootPC(user, "Your character has been deleted.");
                        Administration.DeletePlayerCharacter(user, true);
                    }
                });
        }

        private static void CustomizeCommand(ChatCommandBuilder builder)
        {
            builder.Create("customize", "customise")
                .Description("Customizes your character appearance. Only available for use in the entry area or DM customization area.")
                .Permissions(AuthorizationLevel.Player)
                .Validate((user, args) =>
                {
                    // DMs can use this command anywhere.
                    if (GetIsDM(user)) return string.Empty;

                    // Players can only do this in certain areas.
                    var areaResref = GetResRef(GetArea(user));
                    if (areaResref != "ooc_area" && areaResref != "customize_char")
                    {
                        return "Customization can only occur in the starting area or the DM customization area. You can't use this command here.";
                    }

                    return string.Empty;
                })
                .Action((user, target, location, args) =>
                {
                    Dialog.StartConversation(user, user, nameof(CharacterCustomizationDialog));
                });
        }

        private static void ToggleHelmet(ChatCommandBuilder builder)
        {
            builder.Create("togglehelmet")
                .Description("Toggles whether your helmet will be shown when equipped.")
                .Permissions(AuthorizationLevel.Player)
                .Action((user, target, location, args) =>
                {
                    var playerId = GetObjectUUID(user);
                    var dbPlayer = DB.Get<Player>(playerId);
                    dbPlayer.ShowHelmet = !dbPlayer.ShowHelmet;
                    
                    DB.Set(playerId, dbPlayer);

                    FloatingTextStringOnCreature(
                        dbPlayer.ShowHelmet ? "Now showing equipped helmet." : "Now hiding equipped helmet.",
                        user,
                        false);

                    var helmet = GetItemInSlot(InventorySlot.Head, user);
                    if (GetIsObjectValid(helmet))
                    {
                        SetHiddenWhenEquipped(helmet, !dbPlayer.ShowHelmet);
                    }
                });
        }


    }
}
