using System.Collections;
using System.Collections.Generic;
using Core.Characters.Player;
using UnityEngine;

namespace Utils
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class SkillIncreaserTest : MonoBehaviour
    {

        private void OnTriggerEnter2D(Collider2D collision)
        {
            PlayerQuirks.ModifySkill(EPlayerSkills.Scavanging, 3);
        }
    }
}

