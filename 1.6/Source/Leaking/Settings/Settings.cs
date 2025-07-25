﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Cumpilation.Leaking
{
    public class Settings : ModSettings
    {
        /// <summary>
        /// When enabled, pawns with an inflation hediff will leak cum(or other relevent fluids) over the floor as they slowly deflate.
        /// </summary>
        public static bool EnableFilthGeneration = true;
        public static bool EnableAutoDeflateBucket = false;
        public static bool EnableAutoDeflateClean = false;
        public static bool EnableAutoDeflateDirty = false;
        public static bool EnablePrivacy = true;
        public static float AutoDeflateMinSeverity = 0.4f;
        public static float AutoDeflateMaxDistance = 100f;
        public static float LeakMult = 5.0f;
        public static float LeakRate = 1.0f;
        public static float DeflateMult = 5.0f;
        public static float DeflateRate = 1.0f;

        public override void ExposeData()
        {

            base.ExposeData();
            Scribe_Values.Look<bool>(ref EnableFilthGeneration, "EnableFilthGeneration", true);
            Scribe_Values.Look<bool>(ref EnableAutoDeflateBucket, "EnableAutoDeflateBucket", true);
            Scribe_Values.Look<bool>(ref EnableAutoDeflateClean, "EnableAutoDeflateClean", false);
            Scribe_Values.Look<bool>(ref EnableAutoDeflateDirty, "EnableAutoDeflateDirty", false);
            Scribe_Values.Look<bool>(ref EnablePrivacy, "EnablePrivacy", true);

            Scribe_Values.Look<float>(ref AutoDeflateMinSeverity, "AutoDeflateMinSeverity", 0.4f);
            Scribe_Values.Look<float>(ref AutoDeflateMaxDistance, "AutoDeflateMaxDistance", 100f);

            Scribe_Values.Look<float>(ref LeakMult, "LeakMult", 5.0f);
            Scribe_Values.Look<float>(ref LeakRate, "LeakRate", 1.0f);
            Scribe_Values.Look<float>(ref DeflateMult, "DeflateMult", 5.0f);
            Scribe_Values.Look<float>(ref DeflateRate, "DeflateRate", 1.0f);
        }

        private static readonly float height_modifier = 100f;

        public static void DoWindowContents(Rect inRect)
        {
            Rect viewRect = new Rect(0f, 0f, inRect.width - 16f, inRect.height + height_modifier);

            Listing_Standard listingStandard = new Listing_Standard
            {
                maxOneColumn = true,
                ColumnWidth = viewRect.width / 2.05f
            };

            listingStandard.Begin(inRect);
            listingStandard.Gap(4f);

            listingStandard.CheckboxLabeled("Enable generation of Filth while deflating over time.", ref EnableFilthGeneration, "If disabled, pawns that are filled with cum will no longer generate filth as they naturaly deflate.");
            listingStandard.Gap(4f);

            listingStandard.CheckboxLabeled("Enable auto deflate into buckets", ref EnableAutoDeflateBucket, "If enabled, pawns will automatically deflate themselves into buckets when cumflated.");
            listingStandard.Gap(4f);

            listingStandard.CheckboxLabeled("Enable clean auto deflate", ref EnableAutoDeflateClean, "If enabled, pawns will automatically deflate themselves into toilets, showers, etc. when cumflated (Requires Dubs Bad Hygiene).\nNote that pawns will prioritize buckets and toilets when they are available.");
            listingStandard.Gap(4f);

            listingStandard.CheckboxLabeled("Enable auto deflate anywhere", ref EnableAutoDeflateDirty, "If enabled, pawns will automatically deflate themselves anywhere when cumflated.\nNote that pawns will prioritize buckets and toilets when they are available.");
            listingStandard.Gap(4f);

            listingStandard.CheckboxLabeled("Enable privacy checks", ref EnablePrivacy, "If enabled, pawns will get upset if they are seen while deflating themselves. They will also try to find a private spot to do so.");
            listingStandard.Gap(4f);

            listingStandard.Label("Minimum severity for auto deflate: " + Math.Round(AutoDeflateMinSeverity, 2).ToString(), tooltip: "Pawns will only automatically deflate themselves when cumflation severity is above this value.\nFor reference, severity below 0.2 usually has no effect, and severity above 0.9 will usually immobilize the pawn. Severity will almost never exceed 1.0.\nDefault: 0.4");
            AutoDeflateMinSeverity = listingStandard.Slider(AutoDeflateMinSeverity, 0f, 3f);
            listingStandard.Gap(4f);

            listingStandard.Label("Maximum distance for auto deflate: " + Math.Round(AutoDeflateMaxDistance, 1).ToString(), tooltip: "The maximum distance a pawn will travel when looking for a spot to deflate.\nDefault: 100");
            AutoDeflateMaxDistance = listingStandard.Slider(AutoDeflateMaxDistance, 0f, 1000f);
            listingStandard.Gap(4f);

            listingStandard.Label("Leak amount multiplier: " + Math.Round(LeakMult, 1).ToString(), tooltip: "The amount of filth generated by leaking cum will be multiplied by this value.\nDefault: 5.0");
            LeakMult = listingStandard.Slider(LeakMult, 0.1f, 10f);
            listingStandard.Gap(4f);

            listingStandard.Label("Leak speed multiplier: " + Math.Round(LeakRate, 1).ToString(), tooltip: "The speed of leaking cum will be multiplied by this value.\nDefault: 1.0");
            LeakRate = listingStandard.Slider(LeakRate, 0.1f, 10f);
            listingStandard.Gap(4f);

            listingStandard.Label("Deflate cum multiplier: " + Math.Round(DeflateMult, 1).ToString(), tooltip: "The amount of filth generated by deflating into buckets will be multiplied by this value.\nDefault: 5.0");
            DeflateMult = listingStandard.Slider(DeflateMult, 0.1f, 10f);
            listingStandard.Gap(4f);

            listingStandard.Label("Deflate speed multiplier: " + Math.Round(DeflateRate, 1).ToString(), tooltip: "The speed of deflating cum will be multiplied by this value.\nDefault: 1.0");
            DeflateRate = listingStandard.Slider(DeflateRate, 0.1f, 10f);
            listingStandard.Gap(4f);

            listingStandard.End();
        }

    }
}