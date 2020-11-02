﻿using System;

namespace SWLOR.Game.Server.Enumeration
{
    public enum KeyItemType
    {
        [KeyItem(KeyItemCategoryType.Invalid, "Invalid", false, "")]
        Invalid = 0,
        [KeyItem(KeyItemCategoryType.Invalid, "Avix Tatham's Work Receipt", true, "You received this work receipt from Avix Tatham, mining coordinator on CZ-220.")]
        AvixTathamsWorkReceipt = 1,
        [KeyItem(KeyItemCategoryType.Invalid, "Halron Linth's Work Receipt", true, "You received this work receipt from Halron Linth, security officer on CZ-220.")]
        HalronLinthsWorkReceipt = 2,
        [KeyItem(KeyItemCategoryType.Invalid, "Crafting Terminal Droid Operator's Work Receipt", true, "You received this work receipt from the Crafting Terminal Droid Operator on CZ-220.")]
        CraftingTerminalDroidOperatorsWorkReceipt = 3,
        [KeyItem(KeyItemCategoryType.Invalid, "Crafting Terminal Droid Operator's Work Order", true, "This is a work order you received from the droid responsible for item construction on CZ-220. Obtain the item(s) requested and return them to him.")]
        CraftingTerminalDroidOperatorsWorkOrder = 4,
        [KeyItem(KeyItemCategoryType.Invalid, "CZ-220 Shuttle Pass", true, "This shuttle pass enables you to travel between CZ-220 and planet Viscara.")]
        CZ220ShuttlePass = 5,
        [KeyItem(KeyItemCategoryType.Invalid, "CZ-220 Experiment Room Key", true, "This unlocks the door leading to the experiment room, where the Colicoid should be located.")]
        CZ220ExperimentRoomKey = 6,
        [KeyItem(KeyItemCategoryType.Invalid, "Mandalorian Facility Key", true, "This key unlocks the door to the Mandalorian facility in the Viscara Wildlands.")]
        MandalorianFacilityKey = 7,
        [KeyItem(KeyItemCategoryType.Invalid, "Yellow Key Card", true, "This yellow key card can be used somewhere in the Mandalorian facility on Viscara.")]
        YellowKeyCard = 8,
        [KeyItem(KeyItemCategoryType.Invalid, "Red Key Card", true, "This red key card can be used somewhere in the Mandalorian facility on Viscara.")]
        RedKeyCard = 9,
        [KeyItem(KeyItemCategoryType.Invalid, "Blue Key Card", true, "This blue key card can be used somewhere in the Mandalorian facility on Viscara.")]
        BlueKeyCard = 10,
        [KeyItem(KeyItemCategoryType.Invalid, "Slicing Program", true, "A data disc with a program used to slice the terminals in the Mandalorian facility.")]
        SlicingProgram = 11,
        [KeyItem(KeyItemCategoryType.Invalid, "Data Disc #1", true, "The first disc containing data on the Mandalorian Facility.")]
        DataDisc1 = 12,
        [KeyItem(KeyItemCategoryType.Invalid, "Data Disc #2", true, "The second disc containing data on the Mandalorian Facility.")]
        DataDisc2 = 13,
        [KeyItem(KeyItemCategoryType.Invalid, "Data Disc #3", true, "The third disc containing data on the Mandalorian Facility.")]
        DataDisc3 = 14,
        [KeyItem(KeyItemCategoryType.Invalid, "Data Disc #4", true, "The fourth disc containing data on the Mandalorian Facility.")]
        DataDisc4 = 15,
        [KeyItem(KeyItemCategoryType.Invalid, "Data Disc #5", true, "The fifth disc containing data on the Mandalorian Facility.")]
        DataDisc5 = 16,
        [KeyItem(KeyItemCategoryType.Invalid, "Data Disc #6", true, "The sixth disc containing data on the Mandalorian Facility.")]
        DataDisc6 = 17,
        [KeyItem(KeyItemCategoryType.Invalid, "Package for Denam Reyholm", true, "Roy Moss gave you this package to deliver to Denam Reyholm.")]
        PackageForDenamReyholm = 18,
        [KeyItem(KeyItemCategoryType.Invalid, "Old Tome", true, "A man known only as \"L\" gave you this tome. It's very old and the words have faded.")]
        OldTome = 19,
        [KeyItem(KeyItemCategoryType.Invalid, "Coxxion Base Key", true, "This key will unlock the doors to the Coxxion base located in the deep mountains of Viscara.")]
        CoxxionBaseKey = 20,
    }

    public class KeyItemAttribute : Attribute
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public KeyItemCategoryType Category { get; set; }
        public bool IsActive { get; set; }

        public KeyItemAttribute(KeyItemCategoryType category, string name, bool isActive, string description)
        {
            Category = category;
            Name = name;
            IsActive = isActive;
            Description = description;
        }
    }
}
