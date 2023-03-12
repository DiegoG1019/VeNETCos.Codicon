using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
using System.Windows.Shapes;
using Microsoft.EntityFrameworkCore;
using VeNETCos.Codicon.Database.Contexts;
using VeNETCos.Codicon.Database.Models;
using VeNETCos.Codicon.Services.Containers;
using VeNETCos.Codicon.Types;
using VeNETCos.Codicon.UI.Pages;
using VeNETCos.Codicon.UI.ViewModels;

namespace VeNETCos.Codicon.UI.Windows;
/// <summary>
/// Lógica de interacción para BoxWindow.xaml
/// </summary>
public partial class BoxWindow : Window
{
    [MemberNotNull(nameof(activeInstance))]
    public static BoxWindow ActiveInstance
        => activeInstance ?? throw new InvalidOperationException("The BoxWindow has not been initialized");
    
    readonly ILogger Log;
    private static BoxWindow? activeInstance;

    public MainModel DataModel => (MainModel)DataContext;

    readonly object MainContent;

    public BoxWindow()
    {
        InitializeComponent();
        activeInstance = this;

        Log = LoggerStore.GetLogger(this);
        Log.Information("Initialized BoxWindow");
        DataContext = new MainModel(null);
        MainContent = Content;

        Content = new UserLoginView() { DataContext = DataModel };
        AllowDrop = false;

        Drop += BoxWindow_Drop;

        DataModel.NavigatingToMainScreen += DataModel_NavigatingToMainScreen;
    }

    private void BoxWindow_Drop(object sender, DragEventArgs e)
    {
        if (e.Data.GetDataPresent(DataFormats.FileDrop))
        {
            // Note that you can have more than one file.
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            using (AppServices.GetDbContext(out var context))
            {
                var cbi = DataModel.CurrentBox!.CurrentBoxId;
                var cb = context.Boxes.First(x => x.Id == cbi);

                foreach (var file in files)
                {
                    FileLink? nfl;
                    nfl = context.FileLinks.FirstOrDefault(x => x.Path == file);
                    if (nfl is null)
                    {
                        nfl = new FileLink(Guid.NewGuid(), file);
                        context.FileLinks.Add(nfl);
                        cb.FileLinks.Add(nfl);
                        nfl.Boxes.Add(cb);
                    }
                    else
                    {
                        if (cb.FileLinks.Contains(nfl) is false)
                            cb.FileLinks.Add(nfl);
                        if (nfl.Boxes.Contains(cb) is false)
                            nfl.Boxes.Add(cb);
                    }
                }

                context.SaveChanges();
            }

            DataModel.CurrentBox.Update();
        }
    }

    private void DataModel_NavigatingToMainScreen()
    {
        Content = MainContent;
        AllowDrop = true;
        using (AppServices.GetServices<AppDbContext>().Get(out var context))
            DataModel.CurrentBox = new BoxViewModel(AppDbContext.PrimaryBoxGuid);
    }

    private void Image_MouseDown(object sender, MouseButtonEventArgs e)
    {
        using (AppServices.GetServices<AppDbContext>().Get(out var context))
        {
            Guid cboxId = BoxWindow.ActiveInstance.DataModel!.CurrentBox!.CurrentBoxId;
            Box currentBox = context.Boxes.Include(x => x.Parent).First(x => x.Id == cboxId);

            if (currentBox.Parent is null) 
                return;

            BoxWindow.ActiveInstance.DataModel!.CurrentBox = new BoxViewModel(currentBox.Parent.Id);
        }
    }

    private void Image_MouseDown_1(object sender, MouseButtonEventArgs e)
    {
        new CreateBoxWindow(new CreateBoxViewModel()).Show();
    }

    private void RandomBtn_MouseDown(object sender, MouseButtonEventArgs e)
    {
        using (AppServices.GetServices<AppDbContext>().Get(out var context))
        {
            Guid cboxId = BoxWindow.ActiveInstance.DataModel!.CurrentBox!.CurrentBoxId;
            Box currentBox = context.Boxes.Include(x => x.FileLinks).First(x => x.Id == cboxId);

            var rnd = new Random();

            if (currentBox.FileLinks.Count <= 0)
                return;

            currentBox.FileLinks.OrderBy(x => rnd.Next()).Take(1).First().Open();
        }
    }
}
