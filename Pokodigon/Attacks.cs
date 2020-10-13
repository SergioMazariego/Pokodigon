using System;
using System.Collections.Generic;
using System.Text;

namespace Pokodigon
{
    class Attacks
    {
        public static string[] AttacksName = { "Ala de Acero", "Cola Dragón", "Cola Ferrea", "Lanzarrocas", "Contraataque",
                                                "Filo del Abismo", "Megacuerno", "Esfera Aural", "Cascada", "Encanto",
                                                "Giro Fuego", "Mordisco", "Destructor", "Garra Metal","Finta", "Paranormal",
                                                "Patada Baja", "Picotazo", "Canto Helado", "Placaje", "Voltiocambio", "Disparo Lodo",
                                                "Psicocorte", "Picotazo Veneno", "Corte Furia"};
        public string AttackName { get; set; }
        public int AttackDamage { get; set; }

        public Attacks() {}
        public static string SetAttackName()
        {
            return AttacksName[randomPosition()];
        }
        public static int randomPosition()
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());
            int attackName = random.Next(0, 25);
            return attackName;
        }
        public static int SetAttackDamage()
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());
            int attackDamage = random.Next(15, 66);
            return attackDamage;
        }
    }
}
