using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Engine
{
    public class HealingPotion : Item
    {
        public int AmountToHeal { get; set; }

        public HealingPotion(int id, string name, string namePlural, int amountToHeal,int prise) :base(id,name,namePlural,prise)
        {
            AmountToHeal = amountToHeal;
        }
    }
}
