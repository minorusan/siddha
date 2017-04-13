using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities.UI;


namespace Core.Characters.Player
{
    public enum EPlayerCharachteristic
    {
        Erudition,
        Empathy,
        Reflection,
        SelfCare,
        Prowlness
    }

    public enum EPlayerSkills
    {
        Hiding,
        Lockpicking,
        Scavanging,
        Throwing
    }

    public static class PlayerQuirks
	{
        private static Dictionary<EPlayerCharachteristic, int> _characteristics = new Dictionary<EPlayerCharachteristic, int>();
        private static Dictionary<EPlayerSkills, int> _skils = new Dictionary<EPlayerSkills, int>();

        static PlayerQuirks()
        {
            _skils.Add(EPlayerSkills.Hiding, 0);
            _skils.Add(EPlayerSkills.Lockpicking, 1);
            _skils.Add(EPlayerSkills.Scavanging, 20);
            _skils.Add(EPlayerSkills.Throwing, 10);

            _characteristics.Add(EPlayerCharachteristic.Empathy, 1);
            _characteristics.Add(EPlayerCharachteristic.Erudition, 0);
            _characteristics.Add(EPlayerCharachteristic.Prowlness, 5);
            _characteristics.Add(EPlayerCharachteristic.Reflection, 2);
            _characteristics.Add(EPlayerCharachteristic.SelfCare, 5);

        }

        public static void ModifyCharachteristic(EPlayerCharachteristic characteristic, int value)
        {
            _characteristics[characteristic] = Mathf.Clamp(_characteristics[characteristic] + value, 0, 100);
        }

        public static void ModifySkill(EPlayerSkills skill, int value)
        {
            _skils[skill] = Mathf.Clamp(_skils[skill] + value, 0, 100);
            FanfareMessage.ShowWithText("You improved in " + skill.ToString());
        }

        public static float GetSkill(EPlayerSkills skill)
        {
            return _skils[skill]/100f;
        }

        public static int GetCharactheristic(EPlayerCharachteristic characteristic)
        {
            return _characteristics[characteristic];
        }

        public static bool Hidden
		{
			get;
			set;
		}

		public static bool Attacked
		{
			get;
			set;
		}

	    public static bool Drags { get; set; }
	    public static bool Shadowed { get; set; }
        public static bool Moves { get; set; }
	    public static bool StoppedBreathing { get; set; }
	}
}

