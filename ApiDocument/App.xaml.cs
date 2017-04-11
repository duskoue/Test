using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Diagnostics;

namespace ApiDocument
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public App()
        {
            CPomocniParametri ApiParametri = new CPomocniParametri();
            string ProjectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;

            string foldername = System.IO.Path.Combine(ProjectPath, "Konfiguracija");
            string putanjadofajla = Convert.ToString(System.IO.Path.Combine(foldername, "Default.ini"));
            CIniFile Ini = new CIniFile(putanjadofajla);
            ApiParametri.PutanjadoInifajla = Ini.Read("PutanjaDoIniFajla", "ApiDocument");
            string sacuvan = Ini.Read("sacuvanfajl", "ApiDocument");


        }

        public void Application_Startup(object sender, StartupEventArgs e)
        {
            CPomocniParametri ApiParametri = new CPomocniParametri();
            CIniFile ini = new CIniFile(ApiParametri.PutanjadoInifajla);



            if (e.Args.Length == 1)
            {

                FileInfo file = new FileInfo(e.Args[0]);

                string pokrenutfajl = file.FullName.ToString();
                string path = System.IO.Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
                ini.Write("PutanjaTrenutnoPokrenutogFajla", pokrenutfajl, "ApiDocument");
                string pokrenuto = ini.Read("PutanjaTrenutnoPokrenutogFajla", "ApiDocument");

                ini.Write("sacuvanfajl", "True", "ApiDocument");

            }



        }
    }
}
