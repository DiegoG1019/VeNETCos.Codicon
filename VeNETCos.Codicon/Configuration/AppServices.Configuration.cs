using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using VeNETCos.Codicon.Database.Contexts;

namespace VeNETCos.Codicon;

public static partial class AppServices
{
    private static void BuildServices(ServiceCollection s, IConfiguration conf)
    {
        var dbd = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "torako", AppConfiguration.UserProfile);
        Directory.CreateDirectory(dbd);

        s.AddLogging(x => x.AddSerilog());
        s.AddDbContext<AppDbContext>(x => x.UseSqlite($"Data Source={dbd}/user_data.db;"));
    }
}
