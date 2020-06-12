using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab8
{
    [Serializable]
    class Branch
    {
        public string Name = "Noname";
        public SortedDictionary<Date, int?> Income = new SortedDictionary<Date, int?>();


        public Branch(string Name)
        {
            this.Name = Name;
        }

        public Branch(string Name, SortedDictionary<Date, int?> Income)
        {
            this.Name = Name;
            this.Income = Income;
        }

        public void Add(int income)
        {
            Date date = Income.LastOrDefault().Key;
            Income.Add(++date, income);
        }

        public void AddAt(int income, Date date)
        {
            Income.Add(date, income);
        }

        public int? this[int index]
        {
            get
            {
                return Income.Values.ElementAt(index);
            }
            set
            {
                Income[Income.Keys.ElementAt(index)] = value;
            }
        }
    }
}
