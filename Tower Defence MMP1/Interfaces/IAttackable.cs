﻿//MultiMediaTechnology 
//FHS 45891
//MultiMediaProjekt 1
//Benjamin Kunz

using Tower_Defence.Enums;

namespace Tower_Defence
{
    public interface IAttackable
    {
        void DealDamage(int damagePoints, AttackType attackType);
    }
}
