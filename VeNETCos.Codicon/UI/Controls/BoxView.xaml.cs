﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VeNETCos.Codicon.Database.Contexts;
using VeNETCos.Codicon.Database.Models;

namespace VeNETCos.Codicon.UI.Controls;
/// <summary>
/// Interaction logic for EntryPage.xaml
/// </summary>
public partial class BoxView : Page
{
    public List<Box> boxes { get; set; }
    public BoxView()
    {
        InitializeComponent();
        using var services = AppServices.GetServices<AppDbContext>().Get(out var context);

        boxes = context.Boxes.First().Children.ToList();
    }
}