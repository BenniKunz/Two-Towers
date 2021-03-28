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
