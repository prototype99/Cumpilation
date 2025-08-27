﻿using RimWorld;
using rjw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Cumpilation.Fluids
{
    public class Comp_BiosculpterPod_ResetFluidAmountCycle : CompBiosculpterPod_Cycle
    {
        public override void CycleCompleted(Pawn occupant)
        {
            if (!occupant.ageTracker.Adult)
            {
                ModLog.Debug($"Tried a fluid-cycle for an under-aged pawn {occupant} --- doing nothing");
                return;
            }

            var parts = Genital_Helper.get_AllPartsHediffList(occupant);
            foreach (var part in parts)
            {
                if (part is ISexPartHediff sexPart && sexPart.GetComp<HediffComp_SexPart>().Fluid != null)
                {
                    sexPart.GetComp<HediffComp_SexPart>().Fluid = sexPart.GetComp<HediffComp_SexPart>().Def.fluid;
                    if (sexPart.GetComp<HediffComp_SexPart>().Fluid != null)
                        sexPart.GetComp<HediffComp_SexPart>().partFluidMultiplier = sexPart.GetComp<HediffComp_SexPart>().Def.fluidMultiplier;
                }
            }
        }
    }
}
