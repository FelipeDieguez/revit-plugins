using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Autodesk.Revit.UI;
using System.Reflection;
using System.Windows.Media.Imaging;
using System.IO;

namespace MyRevitPlugin
{
    // Implements the IExternalApplication interface, which defines the OnStartup and OnShutdown methods.
    public class Application : IExternalApplication
    {
        // Method executed when the plugin is shut down.
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        // Method executed when the plugin is started.
        public Result OnStartup(UIControlledApplication application)
        {
            RibbonPanel panel = RibbonPanel(application);
            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

            if (panel.AddItem(new PushButtonData("Informações", "Informações", thisAssemblyPath, "MyRevitPlugin.Command"))
                is PushButton button)
            {
                button.ToolTip = "Informações gerais sobre o plugin";

                Uri uri = new Uri(@"D:\PROGRAMAÇÃO (GIT)\myrevitplugin\MyRevitPlugin\MyRevitPlugin\Resources\information.png");
                //Uri uri = new Uri(Path.Combine(Path.GetDirectoryName(thisAssemblyPath), "Resources", "information.png"));
                BitmapImage bitmap = new BitmapImage(uri);
                button.LargeImage = bitmap;
            }

            return Result.Succeeded;
        }

        public RibbonPanel RibbonPanel(UIControlledApplication a)
        {
            string tab = "FX";
            RibbonPanel ribbonPanel = null;

            try
            {
                a.CreateRibbonTab(tab);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            try
            {
                a.CreateRibbonPanel(tab, "Geral");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            List<RibbonPanel> panels = a.GetRibbonPanels(tab);
            foreach (RibbonPanel p in panels.Where(p => p.Name == "Geral"))
            {
                ribbonPanel = p;
            }

            return ribbonPanel;
        }
    }
}
