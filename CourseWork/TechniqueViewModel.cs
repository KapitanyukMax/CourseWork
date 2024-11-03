using CourseWork.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace CourseWork
{
    public class TechniqueViewModel
    {
        public int Code { get; set; }

        public string Name { get; set; } = string.Empty;

        public double Price { get; set; }

        public string ManufacturerCountry { get; set; } = string.Empty;

        public double Width { get; set; }

        public double Length { get; set; }

        public double Height { get; set; }

        public double Weight { get; set; }

        public bool HasWarranty { get; set; }

        public DateTime Warranty { get; set; } = DateTime.Now;

        public string ConfigurationString { get; set; } = string.Empty;

        public string HeaderText { get; } = "Додати техніку";

        public string ButtonText { get; } = "Додати";

        public ICommand SuccessCommand => new RelayCommand(Success);

        public ICommand CancelCommand => new RelayCommand(() => CloseWindowAction?.Invoke());

        public Action<Technique>? OnTechniqueAdded { get; set; }

        public Action? CloseWindowAction { get; set; }

        public TechniqueViewModel()
        {
        }

        public TechniqueViewModel(Technique technique)
        {
            Code = technique.Code;
            Name = technique.Name;
            Price = technique.Price;
            ManufacturerCountry = technique.ManufacturerCountry;
            Width = technique.Width;
            Length = technique.Length;
            Height = technique.Height;
            Weight = technique.Weight;
            HasWarranty = technique.Warranty is not null;
            Warranty = technique.Warranty ?? DateTime.Now;
            ConfigurationString = string.Join(";", technique.Configuration);

            HeaderText = "Оновити техніку";
            ButtonText = "Оновити";
        }

        public void Success()
        {
            try
            {
                Technique technique = new Technique(
                    Code,
                    Price,
                    Name,
                    ManufacturerCountry,
                    Width,
                    Length,
                    Height,
                    Weight,
                    HasWarranty ? Warranty : null,
                    ConfigurationString.Split(";", StringSplitOptions.RemoveEmptyEntries));
                OnTechniqueAdded?.Invoke(technique);
                CloseWindowAction?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
