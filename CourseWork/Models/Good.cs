using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CourseWork.Models
{
    public class Good
    {
        public int Code { get; init; }

        protected double _price;

        public double Price
        {
            get => _price;
            set
            {
                if (value <= 0.0)
                {
                    throw new ArgumentException("Ціна має бути додатною");
                }

                _price = value;
            }
        }

        protected string _name;

        public string Name
        {
            get => _name;
            set
            {
                if (value is null || value.Length == 0)
                {
                    throw new ArgumentException("Ім'я не може бути порожнім");
                }

                _name = value;
            }
        }

        protected string _manufacturerCountry;

        public string ManufacturerCountry
        {
            get => _manufacturerCountry;
            set
            {
                if (value is null || value.Length == 0)
                {
                    throw new ArgumentException("Країна не може бути порожнньою");
                }

                _manufacturerCountry = value;
            }
        }

        public string StaticGoodType { get; set; } = "Товар";

        public Good()
        {
            Code = 0;
            Price = 100.0;
            Name = GetGoodTypeUkr();
            ManufacturerCountry = "Україна";
        }

        public Good(int code, double price, string name, string manufacturerCountry)
        {
            Code = code;
            Price = price;
            Name = name;
            ManufacturerCountry = manufacturerCountry;
        }

        public Good(Good good)
        {
            if (good is null)
            {
                throw new ArgumentNullException(nameof(good),
                    "Не можливо скопіювати порожній товар");
            }

            Code = good.Code;
            Price = good.Price;
            Name = good.Name;
            ManufacturerCountry = good.ManufacturerCountry;
        }

        public virtual GoodType GetGoodType() => GoodType.Good;

        public string GetGoodTypeUkr()
        {
            return GetGoodType() switch
            {
                GoodType.Good => "Товар",
                GoodType.Technique => "Техніка",
                GoodType.Product => "Продукт",
                _ => "Невідомий"
            };
        }

        private string _toString()
            => $"{Code,-3}|{Price,-10:0.00}|{Name,-41}|{ManufacturerCountry,-30}";

        public string ToGeneralString() => $"{GetGoodTypeUkr(),-10}|{_toString()}";

        public override string ToString() => _toString();

        public override int GetHashCode() => Code.GetHashCode();

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj is Good good)
            {
                return Code == good.Code
                    && Math.Abs(good.Price - Price) <= Constants.DoublePrecision
                       && good.Name == Name
                       && good.ManufacturerCountry == ManufacturerCountry;
            }

            return false;
        }

        public static bool operator ==(Good? good1, Good? good2)
            => good1?.Equals(good2) ?? false;

        public static bool operator !=(Good? good1, Good? good2)
            => !(good1 == good2);

        public static bool operator >(Good? good1, Good? good2)
            => string.Compare(good1?.Name, good2?.Name,
                StringComparison.OrdinalIgnoreCase) > 0;

        public static bool operator <(Good? good1, Good? good2)
            => string.Compare(good1?.Name, good2?.Name,
                StringComparison.OrdinalIgnoreCase) < 0;

        public static bool operator >=(Good? good1, Good? good2)
            => string.Compare(good1?.Name, good2?.Name,
                StringComparison.OrdinalIgnoreCase) >= 0;

        public static bool operator <=(Good? good1, Good? good2)
            => string.Compare(good1?.Name, good2?.Name,
                StringComparison.OrdinalIgnoreCase) <= 0;
    }
}
