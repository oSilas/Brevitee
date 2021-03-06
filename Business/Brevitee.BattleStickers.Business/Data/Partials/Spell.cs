using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using Brevitee.Logging;

namespace Brevitee.BattleStickers.Business.Data
{
    public partial class Spell: IAttackMethod
    {
        public static Spell Create(string name, int strength, Elements element)
        {
            Spell s = new Spell();
            s.Name = name;
            s.Strength = strength;
            s.Element = element.ToString();
            s.Save();

            return s;
        }

        public AttackType AttackType
        {
            get
            {
                return BattleStickers.Business.Data.AttackType.Spell;
            }
        }
    }
}
