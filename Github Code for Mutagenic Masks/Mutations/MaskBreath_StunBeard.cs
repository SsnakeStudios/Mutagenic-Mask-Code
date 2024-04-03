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
    public class MaskBreath_StunBeard : MaskBreathStun
    {


        public MaskBreath_StunBeard()
        {
            DisplayName = "Stun Breath";
			return;
        }

        public override string GetCommandDisplayName()
        {
            return "Breathe Stun Gas";
        }

        public override string GetDescription()
        {
            return "You breathe stun gas.";
        }

        public override string GetLevelText(int Level)
        {
            string Ret = "Breathes stun gas in a cone.\n";
            Ret += "Cone length: " + GetConeLength() + " tiles\n";
            Ret += "Cone angle: " + GetConeAngle() + " degrees\n";
			Ret += $"Cooldown: {TurnNum} rounds\n";
            return Ret;
        }

        public override string GetGasBlueprint() => "StunGas80";
        public override void BreatheInCell(Cell C, ScreenBuffer Buffer, bool doEffect = true)
        {
            DrawBreathInCell(C, Buffer, "K", "y", "Y");
			return;
        }

    }

}

