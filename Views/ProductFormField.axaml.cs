using System;
using System.Collections.Generic;
using System.IO;
using Avalonia.Controls;
using Newtonsoft.Json;
using PrazoCerto.Models;

namespace PrazoCerto.Views;

public partial class ProductFormField : UserControl
{
    public ProductFormField()
    {
        InitializeComponent();
    }
    
    // Save Product
    public static void SaveProduct(string name, 
        string inputCodeBar, 
        string day, 
        string month,
        string year, 
        string inputAmount, 
        string filePath)
    {
        var date = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day));
        var codeBar = long.Parse(inputCodeBar);
        var amount = int.Parse(inputAmount);
        
        
        // Create a new product
        var product = new Product(name.ToUpper(), codeBar, date, amount);
        
        // Save in json
        var stringFromJson = File.ReadAllText(filePath);
        var productsFromJson = JsonConvert.DeserializeObject<List<Product>>(stringFromJson);
        productsFromJson?.Add(product);
        var stringToJson = JsonConvert.SerializeObject(productsFromJson, Formatting.Indented);
        File.WriteAllText(filePath,stringToJson);
        
    }
}