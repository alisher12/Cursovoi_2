using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    public class Weapon : Item
    {
        public int MinimumDamage { get; set; }
        public int MaximumDamage { get; set; }
        public string Description { get; set; }


        public Weapon(int id, string name, string namePlural, int minimumDamage, int maximumDamage, int prise, string description)
            : base(id, name, namePlural, prise)
        {
            MinimumDamage = minimumDamage;
            MaximumDamage = maximumDamage;
            Description = description;
        }
    }
}
