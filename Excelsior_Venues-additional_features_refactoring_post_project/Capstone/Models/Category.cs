using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Category
    {
        public int Id { get;}

        public string Name { get;}

        public Category (int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public override string ToString()
        {
            return $"{Id.ToString().PadLeft(2)}) {Name}";
        }

    }
}
