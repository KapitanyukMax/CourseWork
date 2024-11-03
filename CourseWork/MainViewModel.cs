using CourseWork.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CourseWork
{
    public class MainViewModel
    {
        public ObservableCollection<Good> Goods { get; set; } = [];
        public ObservableCollection<Technique> Techniques { get; set; } = [];
        public ObservableCollection<Product> Products { get; set; } = [];

        public ICommand AddGoodCommand => new RelayCommand(AddGood);

        public ICommand AddTechniqueCommand => new RelayCommand(AddTechnique);

        public ICommand AddProductCommand => new RelayCommand(AddProduct);

        public ICommand SaveGoodsCommand => new RelayCommand(SaveGoods);

        public ICommand SaveTechniquesCommand => new RelayCommand(SaveTechniques);

        public ICommand SaveProductsCommand => new RelayCommand(SaveProducts);

        public ICommand ReadGoodsCommand => new RelayCommand(ReadGoods);

        public ICommand ReadTechniquesCommand => new RelayCommand(ReadTechniques);

        public ICommand ReadProductsCommand => new RelayCommand(ReadProducts);

        public MainViewModel()
        {
            ReadGoodsFromFile();
            ReadTechniquesFromFile();
            ReadProductsFromFile();
        }

        public void InsertGood(Good good)
        {
            for (int i = 0; i < Goods.Count; i++)
            {
                if (Goods[i].Code == good.Code)
                {
                    throw new InvalidDataException("Товар із заданим кодом уже є в базі");
                }
            }

            switch (good.GetGoodType())
            {
                case GoodType.Good:
                    Goods.Add(good);
                    break;
                case GoodType.Technique:
                    Goods.Add(good);
                    Techniques.Add((good as Technique)!);
                    break;
                case GoodType.Product:
                    Goods.Add(good);
                    Products.Add((good as Product)!);
                    break;
                default:
                    throw new ArgumentException("Невідомий тип товару");
            }
        }

        public bool WriteGoodsToFile()
        {
            try
            {
                using StreamWriter writer = new StreamWriter(
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files", "Товари.txt"));

                writer.WriteLine($"{"Тип",-10}|{"Код",-3}|{"Ціна",-10}|{"Назва",-41}|{"Країна-виробник",-30}");

                for (int i = 0; i < Goods.Count; i++)
                {
                    writer.WriteLine(Goods[i].ToGeneralString());
                }
            }
            catch (DirectoryNotFoundException ex)
            {
                MessageBox.Show("Вказаний шлях до файлу не існує", "Помилка", MessageBoxButton.OK,
                    MessageBoxImage.Error);

                return false;
            }

            return true;
        }

        public bool ReadGoodsFromFile()
        {
            try
            {
                List<Good> fileGoods = [];

                using (StreamReader reader = new StreamReader(
                           Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files", "Товари.txt")))
                {
                    reader.ReadLine();

                    while (reader.ReadLine() is { } line)
                    {
                        string[] splitLine = line.Split('|', StringSplitOptions.RemoveEmptyEntries)
                            .Select(part => part.Trim()).ToArray();

                        if (splitLine.Length != 5)
                        {
                            throw new FormatException("Некоректний формат товару в файлі");
                        }

                        string goodType = splitLine[0];

                        int code = int.Parse(splitLine[1]);
                        double price = double.Parse(splitLine[2]);
                        string name = splitLine[3];
                        string manufacturerCountry = splitLine[4];

                        Good newGood = new Good(code, price, name, manufacturerCountry);
                        newGood.StaticGoodType = goodType;
                        fileGoods.Add(newGood);
                    }
                }

                Goods = [];

                foreach (Good good in fileGoods)
                {
                    InsertGood(good);
                }
            }
            catch (Exception ex) when (ex is DirectoryNotFoundException or FileNotFoundException)
            {
                MessageBox.Show("Вказаний шлях до файлу не існує", "Помилка", MessageBoxButton.OK,
                    MessageBoxImage.Error);

                return false;
            }
            catch (FormatException ex)
            {
                MessageBox.Show(ex.Message, "Помилка", MessageBoxButton.OK,
                    MessageBoxImage.Error);

                return false;
            }

            return true;
        }

        public bool WriteTechniquesToFile()
        {
            try
            {
                using StreamWriter writer = new StreamWriter(
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files", "Техніка.txt"));

                writer.WriteLine($"Код|{"Ціна",-10}|{"Назва",-41}|{"Країна-виробник",-30}|"
                                 + $"{"Ширина",-7}|{"Довжина",-7}|{"Висота",-7}|{"Вага",-7}|"
                                 + $"{"Гарантія",-19}|Комплектація");

                for (int i = 0; i < Techniques.Count; i++)
                {
                    writer.WriteLine(Techniques[i].ToString());
                }
            }
            catch (DirectoryNotFoundException ex)
            {
                MessageBox.Show("Вказаний шлях до файлу не існує", "Помилка", MessageBoxButton.OK,
                    MessageBoxImage.Error);

                return false;
            }

            return true;
        }

        public bool ReadTechniquesFromFile()
        {
            try
            {
                List<Technique> fileTechniques = [];

                using (StreamReader reader = new StreamReader(
                           Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files", "Техніка.txt")))
                {
                    reader.ReadLine();

                    while (reader.ReadLine() is { } line)
                    {
                        string[] splitLine = line.Split('|', StringSplitOptions.RemoveEmptyEntries)
                            .Select(part => part.Trim()).ToArray();

                        if (splitLine.Length != 10)
                        {
                            throw new FormatException("Некоректний формат техніки в файлі");
                        }

                        int code = int.Parse(splitLine[0]);
                        double price = double.Parse(splitLine[1]);
                        string name = splitLine[2];
                        string manufacturerCountry = splitLine[3];
                        double width = double.Parse(splitLine[4]);
                        double length = double.Parse(splitLine[5]);
                        double height = double.Parse(splitLine[6]);
                        double weight = double.Parse(splitLine[7]);
                        DateTime? warranty = splitLine[8] == "Гарантія відсутня"
                            ? null
                            : DateTime.Parse(splitLine[8]);
                        string[] configuration = splitLine[9] == "-"
                            ? []
                            : splitLine[9].Split(',', StringSplitOptions.RemoveEmptyEntries)
                                .Select(part => part.Trim()).ToArray();

                        fileTechniques.Add(new Technique(
                            code,
                            price,
                            name,
                            manufacturerCountry,
                            width,
                            length,
                            height,
                            weight,
                            warranty,
                            configuration));
                    }
                }

                List<Good> savedGoods = Goods.Where(good => good.StaticGoodType != "Техніка")
                    .ToList();

                Goods = [];
                Techniques = [];
                Products = [];

                foreach (Good good in savedGoods)
                {
                    InsertGood(good);
                }

                foreach (Technique technique in fileTechniques)
                {
                    InsertGood(technique);
                }
            }
            catch (Exception ex) when (ex is DirectoryNotFoundException or FileNotFoundException)
            {
                MessageBox.Show("Вказаний шлях до файлу не існує", "Помилка", MessageBoxButton.OK,
                    MessageBoxImage.Error);

                return false;
            }
            catch (FormatException ex)
            {
                MessageBox.Show(ex.Message, "Помилка", MessageBoxButton.OK,
                    MessageBoxImage.Error);

                return false;
            }

            return true;
        }

        public bool WriteProductsToFile()
        {
            try
            {
                using StreamWriter writer = new StreamWriter(
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files", "Продукти.txt"));

                writer.WriteLine($"Код|{"Ціна",-10}|{"Назва",-41}|{"Країна-виробник",-30}|"
                                 + $"{"Жири",-7}|{"Білки",-7}|{"Вуглеводи",-7}|{"ГМО",-3}|"
                                 + $"{"Термін придатності",-19}|Вітаміни");

                for (int i = 0; i < Products.Count; i++)
                {
                    writer.WriteLine(Products[i].ToString());
                }
            }
            catch (DirectoryNotFoundException ex)
            {
                MessageBox.Show("Вказаний шлях до файлу не існує", "Помилка", MessageBoxButton.OK,
                    MessageBoxImage.Error);

                return false;
            }

            return true;
        }

        public bool ReadProductsFromFile()
        {
            try
            {
                List<Product> fileProducts = [];

                using (StreamReader reader = new StreamReader(
                           Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files", "Продукти.txt")))
                {
                    reader.ReadLine();

                    while (reader.ReadLine() is { } line)
                    {
                        string[] splitLine = line.Split('|', StringSplitOptions.RemoveEmptyEntries)
                            .Select(part => part.Trim()).ToArray();

                        if (splitLine.Length != 10)
                        {
                            throw new FormatException("Некоректний формат продукта в файлі");
                        }

                        int code = int.Parse(splitLine[0]);
                        double price = double.Parse(splitLine[1]);
                        string name = splitLine[2];
                        string manufacturerCountry = splitLine[3];
                        double fats = double.Parse(splitLine[4]);
                        double proteins = double.Parse(splitLine[5]);
                        double carbs = double.Parse(splitLine[6]);
                        bool isGeneticallyModified = splitLine[7] == "Так" ||
                                                     (splitLine[7] == "Ні"
                                                         ? false
                                                         : throw new FormatException(
                                                             "Некоректний формат техніки в файлі"));
                        DateTime expiryDate = DateTime.Parse(splitLine[8]);
                        string[] vitamins = splitLine[9] == "-"
                            ? []
                            : splitLine[9].Split(',', StringSplitOptions.RemoveEmptyEntries)
                                .Select(part => part.Trim()).ToArray();

                        fileProducts.Add(new Product(
                            code,
                            price,
                            name,
                            manufacturerCountry,
                            fats,
                            proteins,
                            carbs,
                            isGeneticallyModified,
                            expiryDate,
                            vitamins));
                    }
                }

                List<Good> savedGoods = Goods.Where(good => good.StaticGoodType != "Продукт")
                    .ToList();

                Goods = [];
                Techniques = [];
                Products = [];

                foreach (Good good in savedGoods)
                {
                    InsertGood(good);
                }

                foreach (Product product in fileProducts)
                {
                    InsertGood(product);
                }
            }
            catch (Exception ex) when (ex is DirectoryNotFoundException or FileNotFoundException)
            {
                MessageBox.Show("Вказаний шлях до файлу не існує", "Помилка", MessageBoxButton.OK,
                    MessageBoxImage.Error);

                return false;
            }
            catch (FormatException ex)
            {
                MessageBox.Show(ex.Message, "Помилка", MessageBoxButton.OK,
                    MessageBoxImage.Error);

                return false;
            }

            return true;
        }

        public void AddGood()
        {
            GoodWindow goodWindow = new GoodWindow(
                new GoodViewModel
                {
                    OnGoodAdded = InsertGood
                });

            goodWindow.ShowDialog();
        }

        public void AddTechnique()
        {
            TechniqueWindow goodWindow = new TechniqueWindow(
                new TechniqueViewModel
                {
                    OnTechniqueAdded = InsertGood
                });

            goodWindow.ShowDialog();
        }

        public void AddProduct()
        {
            
        }

        public void SaveGoods()
        {
            if (WriteGoodsToFile())
            {
                MessageBox.Show("Товари успішно збережено", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void SaveTechniques()
        {
            if (WriteTechniquesToFile())
            {
                MessageBox.Show("Техніку успішно збережено", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void SaveProducts()
        {
            if (WriteProductsToFile())
            {
                MessageBox.Show("Продукти успішно збережено", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void ReadGoods()
        {
            if (ReadGoodsFromFile())
            {
                MessageBox.Show("Товари успішно зчитано", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void ReadTechniques()
        {
            if (ReadTechniquesFromFile())
            {
                MessageBox.Show("Техніку успішно зчитано", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void ReadProducts()
        {
            if (ReadProductsFromFile())
            {
                MessageBox.Show("Продукти успішно зчитано", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
