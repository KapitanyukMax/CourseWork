using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Models
{
    public class Product : Good
    {
        private double _fats;

        public double Fats
        {
            get => _fats;
            set
            {
                if (value < 0.0)
                {
                    throw new ArgumentException("Уміст жирів не може бути від'ємним");
                }

                _fats = value;
            }
        }

        private double _proteins;

        public double Proteins
        {
            get => _proteins;
            set
            {
                if (value < 0.0)
                {
                    throw new ArgumentException("Уміст білків не може бути від'ємним");
                }

                _proteins = value;
            }
        }

        private double _carbs;

        public double Carbs
        {
            get => _carbs;
            set
            {
                if (value < 0.0)
                {
                    throw new ArgumentException("Уміст вуглеводів не може бути від'ємним");
                }

                _carbs = value;
            }
        }

        public bool IsGeneticallyModified { get; set; }

        public string IsGeneticallyModifiedUkr => IsGeneticallyModified
            ? "Так"
            : "Ні";

        private DateTime _expiryDate;

        public DateTime ExpiryDate
        {
            get => _expiryDate;
            set
            {
                if (value <= DateTime.Now)
                {
                    throw new ArgumentException("Термін придатності має бути дійсним");
                }

                _expiryDate = value;
            }
        }

        public string ExpiryDateString => ExpiryDate.ToString("dd.MM.yyyy");

        public string[] Vitamins { get; set; } = [];

        public string VitaminsString => Vitamins.Length == 0 ? "-"
            : string.Join(", ", Vitamins);

        public Product()
        {
            Fats = 1.0;
            Proteins = 1.0;
            Carbs = 1.0;
            IsGeneticallyModified = false;
            ExpiryDate = DateTime.Now;
            StaticGoodType = "Продукт";
        }

        public Product(
            int code,
            double price,
            string name,
            string manufacturerCountry,
            double fats,
            double proteins,
            double carbs,
            bool isGeneticallyModified,
            DateTime expiryDate,
            string[] vitamins)
            : base(code, price, name, manufacturerCountry)
        {
            Fats = fats;
            Proteins = proteins;
            Carbs = carbs;
            IsGeneticallyModified = isGeneticallyModified;
            ExpiryDate = expiryDate;
            StaticGoodType = "Продукт";

            Vitamins = new string[vitamins.Length];
            for (int i = 0; i < Vitamins.Length; i++)
            {
                Vitamins[i] = vitamins[i];
            }
        }

        public Product(Product product) : base(product)
        {
            Fats = product.Fats;
            Proteins = product.Proteins;
            Carbs = product.Carbs;
            IsGeneticallyModified = product.IsGeneticallyModified;
            ExpiryDate = product.ExpiryDate;
            StaticGoodType = "Продукт";

            Vitamins = new string[product.Vitamins.Length];
            for (int i = 0; i < Vitamins.Length; i++)
            {
                Vitamins[i] = product.Vitamins[i];
            }
        }

        public override GoodType GetGoodType() => GoodType.Product;

        public override string ToString()
            => base.ToString() + $"|{Fats,-7}|{Proteins,-7}|{Carbs,-7}|{IsGeneticallyModifiedUkr,-3}"
                               + $"|{ExpiryDateString,-11}|{VitaminsString}";

        private bool VitaminsEquals(Product product)
        {
            if (Vitamins.Length != product.Vitamins.Length)
            {
                return false;
            }

            for (int i = 0; i < Vitamins.Length; i++)
            {
                if (Vitamins[i] != product.Vitamins[i])
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode() => base.GetHashCode();

        public override bool Equals(object? obj)
        {
            if (obj is Product product)
            {
                return base.Equals(product)
                       && Math.Abs(Fats - product.Fats) <= Constants.DoublePrecision
                       && Math.Abs(Proteins - product.Proteins) <= Constants.DoublePrecision
                       && Math.Abs(Carbs - product.Carbs) <= Constants.DoublePrecision
                       && IsGeneticallyModified == product.IsGeneticallyModified
                       && ExpiryDate == product.ExpiryDate && VitaminsEquals(product);
            }

            return false;
        }
    }
}
