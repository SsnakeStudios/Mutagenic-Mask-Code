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
    public class MaskBreath_NullBeard : MaskBreathNormality
    {


        public MaskBreath_NullBeard()
        {
            DisplayName = "Normality Breath";
			return;
        }

        public override string GetCommandDisplayName()
        {
            return "Breathe Normality Gas";
        }

        public override string GetDescription()
        {
            return "You breathe normality gas.";
        }

        public override string GetLevelText(int Level)
        {
            string Ret = "Breathes normality gas in a cone.\n";
            Ret += "Cone length: " + GetConeLength() + " tiles\n";
            Ret += "Cone angle: " + GetConeAngle() + " degrees\n";
			Ret += $"Cooldown: {TurnNum} rounds\n";
            return Ret;
        }

        public override string GetGasBlueprint() => "NormalityGas80";
        public override void BreatheInCell(Cell C, ScreenBuffer Buffer, bool doEffect = true)
        {
            DrawBreathInCell(C, Buffer, "K", "y", "Y");
			return;
        }

    }

}

