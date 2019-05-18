using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    public class Item
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string NamePlural { get; set; }
        public int Prise { get; set; }


        public Item(int id, string name, string namePlural,int prise) 
        {
            ID = id;
            Name = name;
            NamePlural = namePlural;
            Prise = prise;
        }
    }
}
