using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiDocument
{
   public class CSettingsHelp
    {
        private string _putanjafoldera;

        public string PutanjaFoldera
        {
            get { return _putanjafoldera; }
            set { _putanjafoldera = value; }
        }

        public ObservableCollection<CSettingsHelp> ListaPutanjaFoldera = new ObservableCollection<CSettingsHelp>();
        public void Add(CSettingsHelp pfoldera)
        {
            ListaPutanjaFoldera.Add(pfoldera);
            
        }
    }
}
