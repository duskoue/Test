using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace ApiDocument
{
    [ValueConversion(typeof(string), typeof(bool))]
    public class HeaderToImageConverter : IValueConverter
    {
        public static HeaderToImageConverter Instance = new HeaderToImageConverter();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
           
                if ((value as string).Contains(@"\"))
                {
                string projectPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    Uri uri = new Uri
                   (projectPath + @"\Hard Disk.png");

                    BitmapImage source = new BitmapImage(uri);
                    return source;

                }
                else
                {
                    string projectPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); 
                    Uri uri = new Uri(projectPath + @"\folder.png");
                    BitmapImage source = new BitmapImage(uri);
                    return source;
                }
            
           
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("Konverzija nije uspesna");
        }
    }
}
