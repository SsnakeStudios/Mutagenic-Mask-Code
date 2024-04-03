using ConsoleLib.Console;
using System.Collections.Generic;
using System;
using XRL.UI;
using System.Threading;
using XRL.Core;
using XRL.Rules;
using XRL.World.Anatomy;

namespace XRL.World.Parts.Mutation
{

    [Serializable]
    public class MaskBreath_ConfusionBeard : MaskBreathConfusion
    {


        public MaskBreath_ConfusionBeard()
        {
            DisplayName = "Confusing Breath";
			return;
        }

        public override string GetCommandDisplayName()
        {
            return "Breathe Confusion Gas";
        }

        public override string GetDescription()
        {
            return "You breathe confusion gas.";
        }

        public override string GetLevelText(int Level)
        {
            string Ret = "Breathes confusion gas in a cone.\n";
            Ret += "Cone length: " + GetConeLength() + " tiles\n";
            Ret += "Cone angle: " + GetConeAngle() + " degrees\n";
			Ret += $"Cooldown: {TurnNum} rounds\n";
            return Ret;
        }

        public override string GetGasBlueprint() => "ConfusionGas80";
        public override void BreatheInCell(Cell C, ScreenBuffer Buffer, bool doEffect = true)
        {
            DrawBreathInCell(C, Buffer, "K", "y", "Y");
			return;
        }

    }

}

