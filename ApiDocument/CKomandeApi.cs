using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ApiDocument
{
    class CKomandeApi
    {
        public static RoutedUICommand SaveAs { get; private set; }

        public static RoutedUICommand OpenFile { get; private set; }

        public static RoutedUICommand Izlaz { get; private set; }

        public static RoutedUICommand izvestaj { get; private set; }

        public static RoutedUICommand Zakljucajsve { get; private set; }
        public static RoutedUICommand Save { get; private set; }

        public static RoutedUICommand SaveCopyAs { get; private set; }

        public static RoutedUICommand Podesavanje { get; private set; }
       public static RoutedUICommand Novaforma { get; private set; }
        public static RoutedUICommand Skupiexp { get; private set; }
        public static RoutedUICommand Prikaziexp { get; private set; }

        public static RoutedUICommand OtkljucajSve { get; private set; }

        public static RoutedUICommand Objavljeno { get; private set; }
        public static RoutedUICommand KreirajPDF { get; private set; }

        public static RoutedUICommand Ucitajfajl { get; private set; }
       public static RoutedUICommand IniFajl { get; private set; }

        public static RoutedUICommand Sinhronizacija { get; private set; }

        public static RoutedUICommand Arhiviranje { get; private set; }

        public static RoutedUICommand Dearhiviranje { get; private set; }
        static CKomandeApi()
        {
           
            SaveAs = new RoutedUICommand("Save As", "SaveAs", typeof(CKomandeApi));
            Skupiexp = new RoutedUICommand("Skupiexp", "Skupi expander", typeof(CKomandeApi));
            Prikaziexp = new RoutedUICommand("Prikaziexp", "Prikazi expander", typeof(CKomandeApi));
            KreirajPDF = new RoutedUICommand("KreirajPDF", "kreirajPDF", typeof(CKomandeApi));
            Ucitajfajl = new RoutedUICommand("Ucitajfajl", "Ucitajfajl", typeof(CKomandeApi));
            Arhiviranje = new RoutedUICommand("Arhiviranje", "Arhiviranje", typeof(CKomandeApi));
            Dearhiviranje = new RoutedUICommand("Dearhiviranje", "Dearhiviranje", typeof(CKomandeApi));
            InputGestureCollection CtrlO = new InputGestureCollection();
            CtrlO.Add(new KeyGesture(Key.O, ModifierKeys.Control, "CTRL+O"));
            OpenFile = new RoutedUICommand("Open File", "openFile", typeof(CKomandeApi), CtrlO);

            InputGestureCollection CtrlS = new InputGestureCollection();
            CtrlO.Add(new KeyGesture(Key.S, ModifierKeys.Control, "CTRL+S"));
            Save = new RoutedUICommand("Save", "Save", typeof(CKomandeApi), CtrlS);

            InputGestureCollection F9 = new InputGestureCollection();
            F9.Add(new KeyGesture(Key.F9, ModifierKeys.None, "F9"));
            izvestaj = new RoutedUICommand("izvestaj", "Prikazi dokument", typeof(CKomandeApi),F9);

          



            InputGestureCollection altf4 = new InputGestureCollection();
            altf4.Add(new KeyGesture(Key.F4, ModifierKeys.None, "ALT+F4"));
            Izlaz = new RoutedUICommand("Izlaz", "Izlaz", typeof(CKomandeApi), altf4);

           

            InputGestureCollection f10 = new InputGestureCollection();
            f10.Add(new KeyGesture(Key.F10, ModifierKeys.Control, "F10"));
            Zakljucajsve = new RoutedUICommand("Zakljucajsve", "Zaklljucajsve", typeof(CKomandeApi), f10);

            InputGestureCollection CtrC = new InputGestureCollection();
            CtrC.Add(new KeyGesture(Key.C, ModifierKeys.Control, "CTRL+C"));
            SaveCopyAs = new RoutedUICommand("Duplikat", "SaveCopyAs", typeof(CKomandeApi), CtrC);

            InputGestureCollection f2 = new InputGestureCollection();
            f2.Add(new KeyGesture(Key.F2, ModifierKeys.None, "F2"));
            Podesavanje = new RoutedUICommand("Podesavanja", "Podesavanja", typeof(CKomandeApi), f2);

            InputGestureCollection CtrlN = new InputGestureCollection();
            CtrlN.Add(new KeyGesture(Key.N, ModifierKeys.Control, "CTRL+N"));
            Novaforma = new RoutedUICommand("Novaforma", "Novaforma", typeof(CKomandeApi), CtrlN);

            InputGestureCollection ops = new InputGestureCollection();
            ops.Add(new KeyGesture(Key.Q, ModifierKeys.Control, "CtrlN+Q"));
            OtkljucajSve = new RoutedUICommand("OtkljucajSve", "OtkljucajSve", typeof(CKomandeApi), ops);

            InputGestureCollection obj = new InputGestureCollection();
            obj.Add(new KeyGesture(Key.W, ModifierKeys.Control, "CtrlN+W"));
            Objavljeno = new RoutedUICommand("Objavljeno", "Objavljeno", typeof(CKomandeApi), obj);

            InputGestureCollection inif = new InputGestureCollection();
            inif.Add(new KeyGesture(Key.I, ModifierKeys.Control, "Ctrl +I"));
            IniFajl = new RoutedUICommand("IniFajl", "IniFajl", typeof(CKomandeApi), inif);

            InputGestureCollection sinhro = new InputGestureCollection();
            sinhro.Add(new KeyGesture(Key.F5, ModifierKeys.None, "F5"));
            Sinhronizacija = new RoutedUICommand("Sinhronizacija", "Sinhronizacija", typeof(CKomandeApi), sinhro);



        }

    }
}

