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

            // Add Panels
            CreateGeneralPanel(application, tabName);
            //CreateModelPanel(application, tabName);
            //CreateDetailPanel(application, tabName);
            //CreateTablePanel(application, tabName);
            CreatePlotPanel(application, tabName);

            return Result.Succeeded;
        }

        void CreateButton(RibbonPanel panel, string assemblyPath, string buttonName, string buttonClass, string buttonDescription, string buttonImage)
        {
            // Add button to the panel
            PushButtonData buttonData = new PushButtonData(buttonName, buttonName, assemblyPath, buttonClass);
            PushButton button = panel.AddItem(buttonData) as PushButton;
            button.ToolTip = buttonDescription;

            Uri uri = new Uri(Path.Combine(@"D:\PROGRAMAÇÃO (GIT)\revit-plugins\MyRevitPlugin\MyRevitPlugin\Resources\", buttonImage));
            BitmapImage bitmap = new BitmapImage(uri);
            button.LargeImage = bitmap;
        }

        void CreateGeneralPanel(UIControlledApplication application, string tabName)
        {
            // Get the path of the current assembly.
            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

            // Create a panel on the tab
            RibbonPanel generalPanel = application.CreateRibbonPanel(tabName, "General");

            // Add buttons to the panel
            CreateButton(generalPanel, thisAssemblyPath, "Info", "MyRevitPlugin.OpenInfoForm", "Informations about FX plugin", "info.png");
        }

        void CreateModelPanel(UIControlledApplication application, string tabName)
        {
            // Get the path of the current assembly.
            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

            // Create a panel on the tab
            RibbonPanel modelPanel = application.CreateRibbonPanel(tabName, "Model");

            // Add buttons to the panel
            CreateButton(modelPanel, thisAssemblyPath, "Fill parameter", "MyRevitPlugin.Command", "Fills a chosen parameter according to an excel table", "maintenance.png");
            CreateButton(modelPanel, thisAssemblyPath, "Slab rotator", "MyRevitPlugin.Command", "Rotates slab direction according to parameter", "maintenance.png");
            CreateButton(modelPanel, thisAssemblyPath, "Fill Column Parameter", "MyRevitPlugin.Command", "Fills a PILAR RED parameter according to the geometry", "maintenance.png");
            CreateButton(modelPanel, thisAssemblyPath, "Create trusses", "MyRevitPlugin.Command", "Creation of trusses according to modeled lines", "maintenance.png");
            CreateButton(modelPanel, thisAssemblyPath, "Renumber", "MyRevitPlugin.Command", "Renumbers element titles according to geometric position", "maintenance.png");
            CreateButton(modelPanel, thisAssemblyPath, "Get coordinates", "MyRevitPlugin.Command", "Collects the coordinates of all columns and fills in the chosen parameters", "maintenance.png");
        }

        void CreateDetailPanel(UIControlledApplication application, string tabName)
        {
            // Get the path of the current assembly.
            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

            // Create a panel on the tab
            RibbonPanel detailPanel = application.CreateRibbonPanel(tabName, "Detail");

            // Add buttons to the panel
            CreateButton(detailPanel, thisAssemblyPath, "Model dimension", "MyRevitPlugin.Command", "Put dimensions in all columns of the model in a view", "maintenance.png");
            CreateButton(detailPanel, thisAssemblyPath, "Link dimension", "MyRevitPlugin.Command", "Put dimensions in all columns from a link in a view", "maintenance.png");
            CreateButton(detailPanel, thisAssemblyPath, "Change column tag", "MyRevitPlugin.Command", "Change columns tag if the column geometry changes from a level to another", "maintenance.png");
            CreateButton(detailPanel, thisAssemblyPath, "Creating views", "MyRevitPlugin.Command", "Creates drawing views and links DWG's according to the selected folder", "maintenance.png");
            CreateButton(detailPanel, thisAssemblyPath, "Config views", "MyRevitPlugin.Command", "Configures the layer of views created in Creating views, for reinforcement documentation", "maintenance.png");
            CreateButton(detailPanel, thisAssemblyPath, "Creating sheets", "MyRevitPlugin.Command", "Creates sheets according to Creating views and fits the relevant sheet size", "maintenance.png");
        }

        void CreateTablePanel(UIControlledApplication application, string tabName)
        {
            // Get the path of the current assembly.
            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

            // Create a panel on the tab
            RibbonPanel tablePanel = application.CreateRibbonPanel(tabName, "Table");

            // Add buttons to the panel
            CreateButton(tablePanel, thisAssemblyPath, "Duplicate concrete tables", "MyRevitPlugin.Command", "Duplicate, rename and change filters and table parameters", "maintenance.png");
            CreateButton(tablePanel, thisAssemblyPath, "Duplicate steel tables", "MyRevitPlugin.Command", "Duplicate, rename and change filters and table parameters", "maintenance.png");
        }

        void CreatePlotPanel(UIControlledApplication application, string tabName)
        {
            // Get the path of the current assembly.
            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

            // Create a panel on the tab
            RibbonPanel plotPanel = application.CreateRibbonPanel(tabName, "Plot");

            // Add buttons to the panel
            CreateButton(plotPanel, thisAssemblyPath, "Revision", "MyRevitPlugin.OpenRevisionForm", "Insert chosen revision into sheet list", "revision.png");
            CreateButton(plotPanel, thisAssemblyPath, "Plot dwg's", "MyRevitPlugin.Command", "Plot and rename dwg's according to configuration", "plot-dwg.png");
            CreateButton(plotPanel, thisAssemblyPath, "Plot pdf's", "MyRevitPlugin.Command", "Plot and rename pdf's according to configuration", "plot-pdf.png");
            CreateButton(plotPanel, thisAssemblyPath, "Purge line patterns", "MyRevitPlugin.OpenPurgeLinePatternsForm", "Deletes line patterns that have a name starting in (input)", "purge.png");
            CreateButton(plotPanel, thisAssemblyPath, "Sheets ids", "MyRevitPlugin.Command", "Extracts an .xls listing all sheets and their respective IDs", "sheet-id.png");
        }
    }
}
