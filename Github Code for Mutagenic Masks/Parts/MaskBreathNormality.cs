using ConsoleLib.Console;
using System.Collections.Generic;
using System;
using XRL.UI;
using XRL.World.Anatomy;

namespace XRL.World.Parts.Mutation
{

    [Serializable]
    public class MaskBreathNormality : BaseMutation
    {
        public string BodyPartType = "Face";
        public bool CreateObject = true;


        public GameObject FaceObject = null;

        public Guid ActivatedAbilityId = Guid.Empty;

        public override bool GeneratesEquipment()
        {
            return CreateObject && GetFaceObject() != null;
        }

        public MaskBreathNormality()
        {
            DisplayName = "[MaskBreathNormality::MaskBreathNormality()]";
		}

        [NonSerialized]
        public string _CommandID = null;
        public string CommandID
        {
            get
            {
                if (_CommandID == null)
                {
                    _CommandID = "CommandBreathe_" + GetType().Name;
                }
                return _CommandID;
            }
        }

        public virtual string GetFaceObject()
        {
            return null;
        }

        public virtual string GetGasBlueprint()
        {
            return null;
        }

        public virtual string GetCommandDisplayName()
        {
            return "[MaskBreathBase::GetCommandDisplayName]";
        }

        public override bool WantEvent(int ID, int cascade)
        {
            return
                base.WantEvent(ID, cascade)
                || ID == AIGetOffensiveAbilityListEvent.ID
            ;
        }

        public override bool HandleEvent(AIGetOffensiveAbilityListEvent E)
        {
            if (
                E.Distance <= GetConeLength() - 2
                && IsMyActivatedAbilityAIUsable(ActivatedAbilityId)
                && GameObject.Validate(E.Target)
                && E.Actor.HasLOSTo(E.Target, UseTargetability: true)
            )
            {
                E.Add(CommandID);
            }
            return base.HandleEvent(E);
        }

        public override void Register(GameObject Object)
        {
            Object.RegisterPartEvent(this, CommandID);
            base.Register(Object);
        }

        public override string GetDescription()
        {
            return "[MaskBreathNormality::GetDescription]";
        }

        public override string GetLevelText(int Level)
        {
            string Ret = "[MaskBreathNormality::GetLevelText]\n";
            return Ret;
        }
		
        public virtual void BreatheInCell(Cell C, ScreenBuffer Buffer, bool doEffect = true )
        {
        }

        public void DrawBreathInCell(Cell C, ScreenBuffer Buffer, string color1, string color2, string color3)
        {
            Buffer.Goto(C.X, C.Y);

            string sColor = "&G";
            int r = Rules.Stat.Random(1, 3);
            if (r == 1) sColor = "&" + color1;
            if (r == 2) sColor = "&" + color2;
            if (r == 3) sColor = "&" + color3;

            r = Rules.Stat.Random(1, 3);
            if (r == 1) sColor += "^" + color1;
            if (r == 2) sColor += "^" + color2;
            if (r == 3) sColor += "^" + color3;

            if (C.ParentZone == XRL.Core.XRLCore.Core.Game.ZoneManager.ActiveZone)
            {
                r = Rules.Stat.Random(1, 3);
                Buffer.Write(sColor + (char)(219 + Rules.Stat.Random(0, 4)));

                Popup._TextConsole.DrawBuffer(Buffer);
                System.Threading.Thread.Sleep(Math.Max(1,15-Level));
            }
        }

        public virtual Gas BreatheGasInCell(Cell C, Event CreatorEvent)
        {
            var blueprint = GetGasBlueprint();
            if (blueprint == null) return null;

            var obj = C.AddObject(blueprint);
            var gas = obj.GetPart<Gas>();
            gas.Creator = ParentObject;

            CreatorEvent.SetParameter("Gas", gas);
            return gas;
        }

        public virtual int SortCells( Cell c1, Cell c2 )
        {
            return c1.PathDistanceTo(ParentObject.pPhysics.CurrentCell).CompareTo(c2.PathDistanceTo(ParentObject.pPhysics.CurrentCell));
        }

		public static int TurnNum => 150;
		
	    public int GetConeLength(int L = -1)
			{
				if( L == -1 ) return 4+Level;
				return 4 + L;
			}        

		public int GetConeAngle(int L = -1)
			{
				if( L == -1 ) return 20+2*Level;
				return 20 + 2 * L;
			}

        public static bool Cast(MaskBreathNormality mutation = null)
        {
            if (mutation == null)
            {
                return false;
            }
            ScreenBuffer Buffer = ScreenBuffer.GetScrapBuffer1(true);
            Core.XRLCore.Core.RenderMapToBuffer(Buffer);
            List<Cell> TargetCell = mutation.PickCone(mutation.GetConeLength(), mutation.GetConeAngle(), AllowVis.Any, Label:mutation?.GetCommandDisplayName()??"Breathe"); //TODO:TARGETLABEL
            if (TargetCell == null || TargetCell.Count <= 0)
            {
                return false;
            }
            if (TargetCell.Count == 1 && mutation.ParentObject.IsPlayer())
            {
                if (UI.Popup.ShowYesNoCancel("Are you sure you want to target " + mutation.ParentObject.itself + "?") != DialogResult.Yes)
                {
                    return false;
                }
            }		
            mutation.CooldownMyActivatedAbility(mutation.ActivatedAbilityId, Turns: TurnNum);
            mutation.UseEnergy(1000);
            TargetCell.Sort(mutation.SortCells);

            mutation?.ParentObject?.PlayWorldSound("Sounds/Abilities/sfx_ability_gasMutation_activeRelease");

            var gas = mutation.GetGasBlueprint();
            var E = gas != null ? Event.New("CreatorModifyGas", "Gas", (Gas) null) : null;
            for (int i = 0, j = TargetCell.Count; i < j; i++)
            {
                if (i == 0 || mutation.ParentObject.HasLOSTo(TargetCell[i]))
                {
                    if (TargetCell.Count == 1 || TargetCell[i] != mutation.ParentObject.CurrentCell)
                    {
                        if (gas != null)
                        {
                            mutation.BreatheGasInCell(TargetCell[i], E);
                            mutation.ParentObject.FireEvent(E);
                        }
                        mutation.BreatheInCell(TargetCell[i], Buffer);

                    }
                }
            }
            return true;
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == CommandID)
            {
                if (!Cast(this))
                {
                    return false;
                }
            }
            return base.FireEvent(E);
        }

        public override bool ChangeLevel(int NewLevel)
        {
            return base.ChangeLevel(NewLevel);
        }

        public virtual bool OnMutate(GameObject GO)
        {
            return true;
        }

        public virtual bool OnUnmutate(GameObject GO)
        {
            return true;
        }

        public override bool Mutate(GameObject GO, int Level)
        {
            Unmutate(GO);
            OnMutate(GO);
            if (CreateObject && GetFaceObject() != null)
            {
                Body pBody = GO.Body;
                if (pBody != null)
                {
                    foreach (BodyPart Part in pBody.GetPart(BodyPartType))
                    {
                        if (Part.ForceUnequip(true))
                        {
                            FaceObject = GameObject.Create(GetFaceObject());
                            FaceObject.GetPart<Armor>().WornOn = Part.Type;
                            GO.ForceEquipObject(FaceObject, Part, true, 0);
                            break;
                        }
                    }
                }
            }
// TODO:ADDABILITYICON
            ActivatedAbilityId = AddMyActivatedAbility(
                Name: GetCommandDisplayName(),
                Command: CommandID,
                // CommandForDescription: "CommandBreatherBase",
                Class: "Physical Mutation",
                Description: GetDescription(),
                Icon: "" + (char) 173
            );
            return base.Mutate(GO, Level);
        }

        public override bool Unmutate(GameObject GO)
        {
            OnUnmutate(GO);
            CleanUpMutationEquipment(GO, ref FaceObject);
            if (GameObject.Validate(ref FaceObject))
            {
                FaceObject.Obliterate();
            }
            RemoveMyActivatedAbility(ref ActivatedAbilityId);
            return base.Unmutate(GO);
        }

    }

}
