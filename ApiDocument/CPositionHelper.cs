using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ApiDocument
{
    public class CPositionHelper
    {
        public static string RegPath = @"Software\ApiDocument\WindowBounds\";
        public static void SaveSize(Window win)
        {
            
            RegistryKey key;
            key = Registry.CurrentUser.CreateSubKey(RegPath + win.Name);

            key.SetValue("Bounds", win.RestoreBounds.ToString());
            key.SetValue("Bounds",
            win.RestoreBounds.ToString(CultureInfo.InvariantCulture));
        }
        public static void SetSize(Window win)
        {
            RegistryKey key;
            key = Registry.CurrentUser.OpenSubKey(RegPath + win.Name);
            if (key != null)
            {
                Rect bounds = Rect.Parse(key.GetValue("Bounds").ToString());
                win.Top = bounds.Top;
                win.Left = bounds.Left;
               
                if (win.SizeToContent == SizeToContent.Manual)
                {
                    if (win.Width >100)
                    {
                        win.Width = bounds.Width;
                    }
                   else
                    {
                        win.Width = 100;
                    }

                    if (win.Height >100)
                    {
                        win.Height = bounds.Height;
                    }
                    else
                    {
                        win.Height = 100;
                    }
                    
                    

                }
            }
        }
    }
}
            
        
    

    

