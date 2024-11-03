using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CourseWork.Models;

namespace CourseWork
{
    public class GoodViewModel
    {
        public int Code { get; set; }

        public string Name { get; set; } = string.Empty;

        public double Price { get; set; }

        public string ManufacturerCountry { get; set; } = string.Empty;

        public string HeaderText { get; } = "Додати товар";

        public string ButtonText { get; } = "Додати";

        public ICommand SuccessCommand => new RelayCommand(Success);

        public ICommand CancelCommand => new RelayCommand(() => CloseWindowAction?.Invoke());

        public Action<Good>? OnGoodAdded { get; set; }

        public Action? CloseWindowAction { get; set; }

        public GoodViewModel()
        {
        }

        public GoodViewModel(Good good)
        {
            Code = good.Code;
            Name = good.Name;
            Price = good.Price;
            ManufacturerCountry = good.ManufacturerCountry;
            HeaderText = "Оновити товар";
            ButtonText = "Оновити";
        }

        public void Success()
        {
            try
            {
                Good good = new Good(Code, Price, Name, ManufacturerCountry);
                OnGoodAdded?.Invoke(good);
                CloseWindowAction?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
