using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;

public class Store
{
    public List<Product> Products { get; set; }
    public List<Order> Orders { get; set; }
    public string GetProductStatistics(int year)
    {
        // Формат строки:
        // {№}) - {Название продукта} - {кол-во проданных единиц} item(s)\r\n
        //
        // Пример результирующей строки:
        //
        // 1) Product 3 - 1103 item(s)
        // 2) Product 1 - 800 item(s)
        // 3) Product 2 - 10 item(s)
        string result = "";
        List<Pairs> skipPairs = new List<Pairs>();
        foreach (var it in Orders)
        {
            if (it.OrderDate.Year == year)
            {
                foreach (var jt in it.Items)
                {
                    if (IfContain(skipPairs, jt.ProductId))
                    {
                        AddInList(skipPairs, jt.ProductId, jt.Quantity);
                    }
                    else
                    {
                        skipPairs.Add(new Pairs(jt.ProductId, jt.Quantity));
                    }
                }
            }
        }

        if (skipPairs.Count == 0)
        {
            return "";
        }

        for (int j = 0; j < skipPairs.Count - 1; j++)
        {
            for (int k = 0; k < skipPairs.Count - 1; k++)
            {
                if (skipPairs[j].Quanity < skipPairs[k].Quanity)
                {
                    int tmpQuanity = skipPairs[k].Quanity;
                    int tmpProductId = skipPairs[k].ProductId;
                    skipPairs[j].Quanity = skipPairs[k].Quanity;
                    skipPairs[j].ProductId = skipPairs[k].ProductId;
                    skipPairs[k].Quanity = tmpQuanity;
                    skipPairs[k].ProductId = tmpProductId;
                }
            }
        }
        int i = 0;
        foreach (var it in skipPairs)
        {
            result = result + $"{{i}}) - {NameOfProject(this.Products, it.ProductId)}" + $" - {{it.Quanity}} item(s)\r\n";
            ++i;
        }
        return result;
    }

    public static void AddInList(List<Pairs> list, int id, int quanity)
    {
        foreach (var it in list)
        {
            if (it.ProductId == id)
            {
                it.Quanity += quanity;
                return;
            }
        }

        return;
    }

    public static bool IfContain(List<Pairs> list, int id)
    {
        return false;
    }

    public class Pairs
    {
        public int ProductId { get; set; }

        public int Quanity { get; set; }

        public Pairs(int id, int quanity)
        {
            this.ProductId = id;
            this.Quanity = quanity;
        }

        public int CompareTo(Pairs pair)
        {
            if (this.Quanity >= pair.Quanity)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }

    public string NameOfProject(List<Product> list, int id)
    {
        foreach (var it in list)
        {
            if (it.Id == id)
            {
                return it.Name;
            }
        }

        return "";
    }

    /// <summary>
    /// Формирует строку со статистикой продаж продуктов по годам
    /// Сортировка - по убыванию годов.
    /// Выводятся все года, в которых были продажи продуктов
    /// </summary>
    public string GetYearsStatistics()
    {
        // Формат результата:
        // {Год} - {На какую сумму продано продуктов руб\r\n
        // Most selling: -{Название самого продаваемого продукта} (кол-во проданных единиц самого популярного продукта шт.)\r\n
        // \r\n
        //
        // Пример:
        //
        // 2021 - 630.000 руб.
        // Most selling: Product 1 (380 item(s))
        //
        // 2020 - 630.000 руб.
        // Most selling: Product 1 (380 item(s))
        //
        // 2019 - 130.000 руб.
        // Most selling: Product 3 (10 item(s))
        //
        // 2018 - 50.000 руб.
        // Most selling: Product 3 (5 item(s))

        // TODO Реализовать логику получения и формирования требуемых данных        
        List<YearStruct> list = new List<YearStruct>();
        foreach (var it in this.Orders)
        {
            YearStruct tmp = new YearStruct();
            tmp.year = it.OrderDate.Year;
            if (list.Contains(tmp))
            {
                foreach(var jt in list){
                    if (jt.year == it.OrderDate.Year)
                    {
                       
                    }
                }
            }
            else
            {
                
            }
        }
        return "";
    }

    public class YearStruct
    {
        public int year { get; set; }
        public int mult { get; set; }
        public int productId { get; set; }

        public YearStruct(int mult = 1)
        {
        }
        

    }
}

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
}

public class Order
{
    public int UserId { get; set; }
    public List<OrderItem> Items { get; set; }
    public DateTime OrderDate { get; set; }

    public class OrderItem
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}

public class Program
{
    static void Main(string[] args)
    {
        //
    }
};