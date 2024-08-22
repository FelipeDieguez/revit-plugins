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
            // Define the tab name where the panels will be created.
            string tabName = "FX";

            // Attempt to create the tab if it doesn't exist.
            try
            {
                application.CreateRibbonTab(tabName);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Tab already exists: " + ex.Message);
            }

            // Create the first panel on the tab.
            RibbonPanel panel1 = application.CreateRibbonPanel(tabName, "General");

            // Create the second panel on the tab.
            RibbonPanel panel2 = application.CreateRibbonPanel(tabName, "Advanced");

            // Get the path of the current assembly.
            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

            // Add a button to the first panel.
            PushButtonData buttonData1 = new PushButtonData("Info", "Info", thisAssemblyPath, "MyRevitPlugin.Command");
            PushButton button1 = panel1.AddItem(buttonData1) as PushButton;
            button1.ToolTip = "General information about the plugin";

            // Set the icon for the first button.
            Uri uri1 = new Uri(@"D:\PROGRAMAÇÃO (GIT)\revit-plugins\MyRevitPlugin\MyRevitPlugin\Resources\information.png");
            BitmapImage bitmap1 = new BitmapImage(uri1);
            button1.LargeImage = bitmap1;

            // Add a second button to the second panel.
            PushButtonData buttonData2 = new PushButtonData("Settings", "Settings", thisAssemblyPath, "MyRevitPlugin.Command");
            PushButton button2 = panel2.AddItem(buttonData2) as PushButton;
            button2.ToolTip = "Plugin settings";

            // Set the icon for the second button.
            Uri uri2 = new Uri(@"D:\PROGRAMAÇÃO (GIT)\revit-plugins\MyRevitPlugin\MyRevitPlugin\Resources\information.png");
            BitmapImage bitmap2 = new BitmapImage(uri2);
            button2.LargeImage = bitmap2;

            return Result.Succeeded;
        }
    }
}
