using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ApiDocument
{
  public static  class CHelpevent
    {
        public static T Pronadjidete<T>(DependencyObject roditeljime, string deteime)
           where T : DependencyObject
        {
            //Provera da li postoi roditelj i naziv deteta

            if (roditeljime == null) return null;
            T nadjidete = null;
            // for petlja prolazi kroz sve kontrole ma roditeljskoj kontroli
            //i uzima njigova imena
            int brojacdece = VisualTreeHelper.GetChildrenCount(roditeljime);
            for (int i = 0; i < brojacdece; i++)
            {
                var dete = VisualTreeHelper.GetChild(roditeljime, i);

                // Ako tip dete kontrole ne odgovara trazenom tipu 

                T tipdece = dete as T;
                if (tipdece == null)
                {
                    // rekurzija

                    nadjidete = Pronadjidete<T>(dete, deteime);

                    // ako je pronasao dete kontrole i ako naziv dete kontole nije prazan

                    if (nadjidete != null) break;
                }
                else if (!string.IsNullOrEmpty(deteime))
                {
                    var frameworkElement = dete as FrameworkElement;
                    // pretrazivanje imena dete kontrole
                    if (frameworkElement != null && frameworkElement.Name == deteime)
                    {
                        // ako nadjes dete zaustavi
                        nadjidete = (T)dete;
                        break;
                    }
                }
                else
                {
                    // pronasao dete
                    nadjidete = (T)dete;
                    break;
                }
            }
            return nadjidete;
        }
    }
}
