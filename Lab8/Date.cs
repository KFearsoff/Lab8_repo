using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab8
{
    [Serializable]
    class Date : IComparable<Date>
    {
        int? Month { get; set; }
        int? Year { get; set; }

        public Date(int Year, int Month)
        {
            this.Year = Year;
            this.Month = Month;
        }

        public Date(Date date)
        {
            Year = date.Year;
            Month = date.Month;
        }

        #region Equals, ToString, GetHashCode
        public override string ToString()
        {
            if (Month < 10) return "0" + Month + "." + Year;
            else return Month + "." + Year;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!(obj is Date)) return false;
            return Year == ((Date)obj).Year && Month == ((Date)obj).Month;
        }

        public override int GetHashCode()
        {
            return (12 * Year + Month).GetHashCode();
        }
        #endregion

        #region Operators
        public static bool operator ==(Date date1, Date date2)
        {
            if ((object)date1 == null || (object)date2 == null)
                return Equals(date1, date2);
            return date1.Equals(date2);
        }

        public static bool operator !=(Date date1, Date date2)
        {
            if ((object)date1 == null || (object)date2 == null)
                return !Equals(date1, date2);
            return !date1.Equals(date2);
        }

        public static Date operator ++(Date d)
        {
            if (d.Month == 12)
            {
                d.Month = 1;
                d.Year++;
                return d;
            }
            else
            {
                d.Month++;
                return d;
            }
        }

        public static Date operator --(Date d)
        {
            if (d.Month == 1)
            {
                d.Month = 12;
                d.Year--;
                return d;
            }
            else
            {
                d.Month--;
                return d;
            }
        }
        #endregion

        public int CompareTo(Date date)
        {
            if (Year * 12 + Month > date.Year * 12 + date.Month) return 1;
            else if (Year * 12 + Month < date.Year * 12 + date.Month) return -1;
            else return 0;
        }
    }
}
