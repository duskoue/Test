using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ApiDocument
{
  public  class CIniFile
    {

        string Path;
        string EXE = Assembly.GetExecutingAssembly().GetName().Name;

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);


        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

       

        public CIniFile(string IniPath = null) 
        {

            //     Path = new FileInfo(IniPath ?? EXE + ".ini").FullName.ToString();
            string ProjectPath = System.IO.Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
            string directiry = System.IO.Path.Combine(ProjectPath, @"Konfiguracija\MojNoviIniFajl.ini");
            Path = directiry;
            //  Path = @"C:\Users\Dusko Marinkovic\Documents\Visual Studio 2015\Projects\ApiDocument\ApiDocument\Konfiguracija\MojNoviIniFajl.ini";





        }

      
          
              
        public string Read(string Key, string Section = null)
        {
            var RetVal = new StringBuilder(255);
            GetPrivateProfileString(Section ?? EXE, Key, "", RetVal, 255, Path);
            return RetVal.ToString();
        }

        public void Write(string Key, string Value, string Section = null)
        {
            WritePrivateProfileString(Section ?? EXE, Key, Value, Path);
        }

        public void DeleteKey(string Key, string Section = null)
        {
            Write(Key, null, Section ?? EXE);
        }

        public void DeleteSection(string Section = null)
        {
            Write(null, null, Section ?? EXE);
        }

        public bool KeyExists(string Key, string Section = null)
        {
            return Read(Key, Section).Length > 0;
        }

        public bool SectionExist(string section)
        {
            return Read(null, section).Length > 0;
        }


    }
}
