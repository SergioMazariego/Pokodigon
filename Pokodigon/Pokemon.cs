using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Pokodigon
{
    class Pokemon
    {
        public string Name { get; set; }
        public string Type { get; set; }

        private int _hp = 150;
        public int HP
        {
            get { return _hp; }
            set
            {
                if (value < 0)
                {
                    _hp = 0;
                }
                else
                {
                    _hp = value; 
                }
            }
        }
        //Ataques
        public string Attack1 { get; set; }
        public string Attack2 { get; set; }
        public int AttackDamage1 { get; set; }
        public int AttackDamage2 { get; set; }

        public Pokemon(string name)
        {
            Name = name;
        }
        public Pokemon(string name, string type) : this(type)
        {
            Name = name;
            Type = type;
        }
        public void setAttacksNames()
        {
            Attack1 = Attacks.SetAttackName();
            Attack2 = Attacks.SetAttackName();
        }
        public void setAttacksDamage()
        {
            AttackDamage1 = Attacks.SetAttackDamage();
            AttackDamage2 = Attacks.SetAttackDamage();
        }
    }
}
