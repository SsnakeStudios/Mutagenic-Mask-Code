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
    public class MaskBreath_CryoBeard : MaskBreathCryo
    {


        public MaskBreath_CryoBeard()
        {
            DisplayName = "Freezing Breath";
			return;
        }

        public override string GetCommandDisplayName()
        {
            return "Breathe Freeze Gas";
        }

        public override string GetDescription()
        {
            return "You breathe freeze gas.";
        }

        public override string GetLevelText(int Level)
        {
            string Ret = "Breathes freeze gas in a cone.\n";
            Ret += "Cone length: " + GetConeLength() + " tiles\n";
            Ret += "Cone angle: " + GetConeAngle() + " degrees\n";
			Ret += $"Cooldown: {TurnNum} rounds\n";
            return Ret;
        }

        public override string GetGasBlueprint() => "CryoGas80";
        public override void BreatheInCell(Cell C, ScreenBuffer Buffer, bool doEffect = true)
        {
            DrawBreathInCell(C, Buffer, "K", "y", "Y");
			return;
        }

    }

}

