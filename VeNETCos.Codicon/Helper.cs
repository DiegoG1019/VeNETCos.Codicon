using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace VeNETCos.Codicon;

public static class Helper
{
    public static ImageSource? GetIcon(string path)
    {
        ArgumentException.ThrowIfNullOrEmpty(path);

        if (File.Exists(path))
        {
            var sysicon = Icon.ExtractAssociatedIcon(path);
            if (sysicon is null) return null;
            using (sysicon)
                return Imaging.CreateBitmapSourceFromHIcon(
                    sysicon.Handle,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions()
                );
        }

        return null;
    }
}
