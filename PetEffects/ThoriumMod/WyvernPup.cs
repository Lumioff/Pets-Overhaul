using Terraria;
using Terraria.ID;
using PetsOverhaul.Systems;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.Localization;
using PetsOverhaul.Config;
using Terraria.GameInput;
using PetsOverhaul.ModSupport;
using System;

namespace PetsOverhaul.PetEffects.ThoriumMod
{
    sealed public class WyvernPup : ModPlayer
    {
        GlobalPet Pet { get => Player.GetModPlayer<GlobalPet>(); }
        public override void PostUpdateEquips()
        {
            
        }
    }
    sealed public class CloudyChewToy : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            if(ModManager.ThoriumMod == null) return false;
            if(ModManager.ThoriumMod.InternalNameToModdedItemId == null) return false;
            Console.WriteLine(ModManager.ThoriumMod.InternalNameToModdedItemId);
            if(!ModManager.ThoriumMod.InternalNameToModdedItemId.ContainsKey("CloudyChewToy")) return false;

            return entity.type == ModManager.ThoriumMod.InternalNameToModdedItemId["CloudyChewToy"];
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (ModContent.GetInstance<Personalization>().TooltipsEnabledWithShift && !PlayerInput.Triggers.Current.KeyStatus[TriggerNames.Down]) return;
            WyvernPup wyvernPup = Main.LocalPlayer.GetModPlayer<WyvernPup>();
            tooltips.Add(new(Mod, "Tooltip0", "Unimplemented"/*Language.GetTextValue("Mods.PetsOverhaul.CloudyChewToyTooltips.CloudyChewToy")*/
                
            ));
        }
    }
}
