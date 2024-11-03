using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace CourseWork.Models
{
    public class Technique : Good
    {
        private double _width;

        public double Width
        {
            get => _width;
            set
            {
                if (value <= 0.0)
                {
                    throw new ArgumentException("Ширина має бути додатною");
                }

                _width = value;
            }
        }

        private double _length;

        public double Length
        {
            get => _length;
            set
            {
                if (value <= 0.0)
                {
                    throw new ArgumentException("Довжина має бути додатною");
                }

                _length = value;
            }
        }

        private double _height;

        public double Height
        {
            get => _height;
            set
            {
                if (value <= 0.0)
                {
                    throw new ArgumentException("Висота має бути додатною");
                }

                _height = value;
            }
        }

        public string Dimensions => $"{Width}x{Length}x{Height}";

        private double _weight;

        public double Weight
        {
            get => _weight;
            set
            {
                if (value <= 0.0)
                {
                    throw new ArgumentException("Вага має бути додатною");
                }

                _weight = value;
            }
        }

        private DateTime? _warranty;

        public DateTime? Warranty
        {
            get => _warranty;
            set
            {
                if (value is not null && value <= DateTime.Now)
                {
                    throw new ArgumentException("Гарантія має бути дійсною");
                }

                _warranty = value;
            }
        }

        public string WarrantyString => Warranty?.ToString("dd.MM.yyyy") ?? "Гарантія відсутня";

        public string[] Configuration { get; set; } = [];

        public string[] DisplayedConfiguration
            => Configuration.Length == 0
                ? ["-"]
                : Configuration;

        public string ConfigurationString => string.Join(", ", DisplayedConfiguration);

        public Technique()
        {
            Width = 1.0;
            Length = 1.0;
            Height = 1.0;
            Weight = 1.0;
            StaticGoodType = "Техніка";
        }

        public Technique(
            int code,
            double price,
            string name,
            string manufacturerCountry,
            double width,
            double length,
            double height,
            double weight,
            DateTime? warranty,
            string[] configuration)
            : base(code, price, name, manufacturerCountry)
        {
            Width = width;
            Length = length;
            Height = height;
            Weight = weight;
            Warranty = warranty;
            StaticGoodType = "Техніка";

            Configuration = new string[configuration.Length];
            for (int i = 0; i < Configuration.Length; i++)
            {
                Configuration[i] = configuration[i];
            }
        }

        public Technique(Technique technique) : base(technique)
        {
            Width = technique.Width;
            Length = technique.Length;
            Height = technique.Height;
            Weight = technique.Weight;
            Warranty = technique.Warranty;
            StaticGoodType = "Техніка";

            Configuration = new string[technique.Configuration.Length];
            for (int i = 0; i < Configuration.Length; i++)
            {
                Configuration[i] = technique.Configuration[i];
            }
        }

        public override GoodType GetGoodType() => GoodType.Technique;

        public override string ToString()
            => base.ToString() + $"|{Width,-7}|{Length,-7}|{Height,-7}|{Weight,-7}"
                               + $"|{WarrantyString,-18}|{ConfigurationString}";

        private bool ConfigurationEquals(Technique technique)
        {
            if (Configuration.Length != technique.Configuration.Length)
            {
                return false;
            }

            for (int i = 0; i < Configuration.Length; i++)
            {
                if (Configuration[i] != technique.Configuration[i])
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode() => base.GetHashCode();

        public override bool Equals(object? obj)
        {
            if (obj is Technique technique)
            {
                return base.Equals(technique)
                    && Math.Abs(Width - technique.Width) <= Constants.DoublePrecision
                    && Math.Abs(Length - technique.Length) <= Constants.DoublePrecision
                    && Math.Abs(Height - technique.Height) <= Constants.DoublePrecision
                    && Math.Abs(Weight - technique.Weight) <= Constants.DoublePrecision
                    && Warranty == technique.Warranty && ConfigurationEquals(technique);
            }

            return false;
        }
    }
}
