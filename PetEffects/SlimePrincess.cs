﻿using PetsOverhaul.Config;
using PetsOverhaul.Items;
using PetsOverhaul.Systems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace PetsOverhaul.PetEffects
{
    public sealed class SlimePrincess : PetEffect //Pet will be reworked post 3.0 update
    {
        public float slow = 0.65f;
        public float haste = 0.3f;
        public int shield = 7;
        public int shieldTime = 240;
        public int hitCounter = 0;
        public float dmgBoost = 1.25f;
        public Player queenSlime;
        public Player dualSlime;
        public int baseCounterChnc = 100;

        public override PetClasses PetClassPrimary => PetClasses.Supportive;
        public override PetClasses PetClassSecondary => PetClasses.Offensive;
        public override void PostUpdateMiscEffects()
        {
            if (GlobalPet.QueenSlimePetActive(out queenSlime) && Player.HasBuff(BuffID.GelBalloonBuff))
            {
                Pet.abilityHaste += queenSlime.GetModPlayer<SlimePrincess>().haste;
            }
            else if (GlobalPet.DualSlimePetActive(out dualSlime) && Player.HasBuff(BuffID.GelBalloonBuff))
            {
                Pet.abilityHaste += dualSlime.GetModPlayer<DualSlime>().haste;
            }
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (GlobalPet.QueenSlimePetActive(out queenSlime) && Player.HasBuff(BuffID.GelBalloonBuff))
            {
                hitCounter += GlobalPet.Randomizer(queenSlime.GetModPlayer<SlimePrincess>().baseCounterChnc + (int)(Pet.abilityHaste * 100));
                if (hitCounter >= 6)
                {
                    modifiers.FinalDamage *= queenSlime.GetModPlayer<SlimePrincess>().dmgBoost;
                    Pet.AddShield(queenSlime.GetModPlayer<SlimePrincess>().shield, queenSlime.GetModPlayer<SlimePrincess>().shieldTime);
                    hitCounter -= 6;
                }
            }
            else if (GlobalPet.DualSlimePetActive(out dualSlime) && Player.HasBuff(BuffID.GelBalloonBuff))
            {
                hitCounter += GlobalPet.Randomizer(dualSlime.GetModPlayer<DualSlime>().baseCounterChnc + (int)(Pet.abilityHaste * 100));
                if (hitCounter >= 6)
                {
                    modifiers.FinalDamage *= dualSlime.GetModPlayer<DualSlime>().dmgBoost;
                    Pet.AddShield(dualSlime.GetModPlayer<DualSlime>().shield, dualSlime.GetModPlayer<DualSlime>().shieldTime);

                    hitCounter -= 6;
                }
            }
        }
    }
    public sealed class QueenSlimePetItem : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.QueenSlimePetItem;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<PetPersonalization>().EnableTooltipToggle && !PetKeybinds.PetTooltipHide.Current)
            {
                return;
            }

            SlimePrincess slimePrincess = Main.LocalPlayer.GetModPlayer<SlimePrincess>();
            tooltips.Add(new(Mod, "Tooltip0", Language.GetTextValue("Mods.PetsOverhaul.PetItemTooltips.QueenSlimePetItem")
                .Replace("<class>", PetTextsColors.ClassText(slimePrincess.PetClassPrimary, slimePrincess.PetClassSecondary))
                        .Replace("<slow>", Math.Round(slimePrincess.slow * 100, 2).ToString())
                        .Replace("<haste>", Math.Round(slimePrincess.haste * 100, 2).ToString())
                        .Replace("<dmgBonus>", slimePrincess.dmgBoost.ToString())
                        .Replace("<shield>", slimePrincess.shield.ToString())
                        .Replace("<shieldTime>", Math.Round(slimePrincess.shieldTime / 60f, 2).ToString())
                        .Replace("<endless>", ModContent.ItemType<EndlessBalloonSack>().ToString())
                        ));
        }
    }
}
