//MultiMediaTechnology 
//FHS 45891
//MultiMediaProjekt 1
//Benjamin Kunz

using System;
using System.Collections.Generic;
using System.Text;

namespace Tower_Defence
{
    public interface IAttackable
    {
        void DealDamage(int damagePoints, AttackType attackType);
    }
}
