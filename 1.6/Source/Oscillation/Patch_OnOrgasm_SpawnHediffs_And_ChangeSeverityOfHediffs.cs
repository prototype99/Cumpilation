﻿using HarmonyLib;
using rjw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Cumpilation.Oscillation
{
    [HarmonyPatch(typeof(SexUtility), nameof(SexUtility.SatisfyPersonal))] // Actual on orgasm method
    public static class Patch_OnOrgasm_SpawnHediffs_And_ChangeSeverityOfHediffs
    {
        public static void Postfix(SexProps props)
        {
            if (!Settings.EnableOscillationMechanics) return;

            if (props == null || props.pawn == null) return;
            if (props.orgasms == 0) return;

            Pawn pawn = props.pawn;

            // First: Spawn all Hediffs that spawn on Orgasm. This has to be handled first, as maybe "blocking" hediffs appear, or we want to increase the severity of something that first has to spawn.
            foreach (HediffDef hediffDef in DefDatabase<HediffDef>.AllDefsListForReading.Where(hDef => hDef.HasModExtension<HediffDefModExtension_SpawnOnOrgasm>()))
            {
                // If the pawn has the hediff, do nothing
                if (pawn.health.hediffSet.HasHediff(hediffDef)) continue;

                HediffDefModExtension_SpawnOnOrgasm spawnExt = hediffDef.GetModExtension<HediffDefModExtension_SpawnOnOrgasm>();
                if (spawnExt != null && spawnExt.IsValidPawn(pawn) &&spawnExt.GetSexPartHediffs(pawn).Count() > 0)
                {
                    // Otherwise: Spawn the Hediff with Severity 0
                    Hediff spawned = pawn.health.AddHediff(hediffDef);
                    spawned.Severity = 0;
                    ModLog.Debug($"Spawned {spawned.def.defName} on {pawn} because of orgasm. {pawn} was valid because of Part(s): {spawnExt.GetSexPartHediffs(pawn).Select(hed => hed.Def.defName).Cast<String>().Join()}");
                }
            }

            IEnumerable<Hediff> HediffsToRemove = new List<Hediff>();

            // Second: Increase all Relevant Hediffs
            foreach (Hediff hediff in pawn.health.hediffSet.hediffs)
            {
                if (hediff != null && hediff.TryGetComp<HediffComp_ChangeSeverityOnOrgasm>() != null)
                {
                    var comp = hediff.TryGetComp<HediffComp_ChangeSeverityOnOrgasm>();
                    hediff.Severity += comp.Props.severityChange;

                    ModLog.Debug($"Changed Severity for  {hediff.def.defName} on {pawn} by {comp.Props.severityChange} because of orgasm.");
                    if (hediff.Severity <= 0)
                    {
                        HediffsToRemove.AddItem(hediff);
                    }
                }
            }

            // Last: Cleanup / Remove all now-0-hediffs. This needs to be done to avoid concurrent-modification-exceptions.
            foreach (Hediff hediff in HediffsToRemove)
            {
                pawn.health.RemoveHediff(hediff);
            }
        }
    }
}
