﻿using PetsOverhaul.Config;
using PetsOverhaul.Items;
using PetsOverhaul.Systems;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class ShadowMimic : PetEffect
    {
        public override PetClasses PetClassPrimary => PetClasses.Utility;
        public int npcCoin = 15;
        public int npcItem = 8;
        public int bossCoin = 10;
        public int bossItem = 5;
        public int bagCoin = 10;
        public int bagItem = 5;
        private int chanceToRollItem = 0;
        public override void Load()
        {
            PetsOverhaul.OnPickupActions += PreOnPickup;
        }
        public static void PreOnPickup(Item item, Player player)
        {
            GlobalPet PickerPet = player.GetModPlayer<GlobalPet>();
            ShadowMimic mimic = player.GetModPlayer<ShadowMimic>();
            if (PickerPet.PickupChecks(item, ItemID.OrnateShadowKey, out ItemPet itemChck))
            {
                mimic.chanceToRollItem = 0;
                if (itemChck.itemFromNpc == true)
                {
                    mimic.chanceToRollItem += (item.IsACoin ? mimic.npcCoin : mimic.npcItem) * item.stack;
                }
                if (itemChck.itemFromBoss == true && ItemID.Sets.BossBag[item.type] == false)
                {
                    mimic.chanceToRollItem += (item.IsACoin ? mimic.bossCoin : mimic.bossItem) * item.stack;
                }
                if (itemChck.itemFromBag == true)
                {
                    mimic.chanceToRollItem += (item.IsACoin ? mimic.bagCoin : mimic.bagItem) * item.stack;
                }
                for (int i = 0; i < GlobalPet.Randomizer(mimic.chanceToRollItem); i++)
                {
                    player.QuickSpawnItem(GlobalPet.GetSource_Pet(EntitySourcePetIDs.GlobalItem), item.type, 1);
                }
            }
        }
    }
    public sealed class OrnateShadowKey : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.OrnateShadowKey;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<PetPersonalization>().EnableTooltipToggle && !PetKeybinds.PetTooltipHide.Current)
            {
                return;
            }

            ShadowMimic shadowMimic = Main.LocalPlayer.GetModPlayer<ShadowMimic>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.OrnateShadowKey")
                .Replace("<class>", PetTextsColors.ClassText(shadowMimic.PetClassPrimary, shadowMimic.PetClassSecondary))
                        .Replace("<npcCoin>", shadowMimic.npcCoin.ToString())
                        .Replace("<npcItem>", shadowMimic.npcItem.ToString())
                        .Replace("<bossCoin>", shadowMimic.bossCoin.ToString())
                        .Replace("<bossItem>", shadowMimic.bossItem.ToString())
                        .Replace("<bagCoin>", shadowMimic.bagCoin.ToString())
                        .Replace("<bagItem>", shadowMimic.bagItem.ToString())
                        ));
        }
    }
}
