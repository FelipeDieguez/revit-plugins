using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Windows.Media.Imaging;
using Autodesk.Revit.Attributes;
using System.Windows.Forms;
using ComboBox = System.Windows.Forms.ComboBox;
using RadioButton = System.Windows.Forms.RadioButton;
using Button = System.Windows.Forms.Button;

namespace MyRevitPlugin
{
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiapp = commandData.Application;
            var app = uiapp.Application;
            var uidoc = uiapp.ActiveUIDocument;
            var doc = uidoc.Document;

            TaskDialog.Show("Fx", "Essa funcionalidade ainda está em desenvolvimento.");

            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    public class OpenInfoForm : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            InfoForm infoForm = new InfoForm();

            infoForm.ShowDialog();

            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    public class OpenRevisionForm : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiapp = commandData.Application;
            var app = uiapp.Application;
            var uidoc = uiapp.ActiveUIDocument;
            var doc = uidoc.Document;

            RevisionForm revisionForm = new RevisionForm();

            // Get visual interface variables
            ComboBox comboBox1 = (ComboBox)revisionForm.Controls["comboBox1"];
            ComboBox comboBox2 = (ComboBox)revisionForm.Controls["comboBox2"];
            RadioButton radioButton1 = (RadioButton)revisionForm.Controls["radioButton1"];
            RadioButton radioButton2 = (RadioButton)revisionForm.Controls["radioButton2"];
            Button button1 = (Button)revisionForm.Controls["button1"];

            // Get lists of ViewSets and Revisions
            List<ViewSheetSet> viewSets = GetAllViewSets(doc);
            List<Revision> revisions = GetAllRevisions(doc);

            // Populate comboBox with the names of the ViewSets
            foreach (var viewSet in viewSets)
            {
                comboBox1.Items.Add(viewSet.Name);
            }

            foreach (var revision in revisions)
            {
                comboBox2.Items.Add(revision.Description);
            }

            // Set elements
            if (comboBox1.Items.Count > 0)
            {
                comboBox1.SelectedIndex = 0;
            }

            if (comboBox2.Items.Count > 0)
            {
                comboBox2.SelectedIndex = 0;
            }

            radioButton1.Checked = true;

            // Declare events
            button1.Click += (sender, e) => OnClick(doc, revisionForm, comboBox1, comboBox2, radioButton1, radioButton2);

            revisionForm.ShowDialog();

            return Result.Succeeded;
        }

        List<ViewSheetSet> GetAllViewSets(Document doc)
        {
            // Create a collector to gather elements of type ViewSheetSet
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            ICollection<Element> viewSets = collector.OfClass(typeof(ViewSheetSet)).ToElements();

            // Cast the elements to ViewSheetSet and return as a list
            return viewSets.Cast<ViewSheetSet>().ToList();
        }

        List<Revision> GetAllRevisions(Document doc)
        {
            // Create a collector to gather elements of type Revision
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            ICollection<Element> revisions = collector.OfClass(typeof(Revision)).ToElements();

            // Cast the elements to Revision and return as a list
            return revisions.Cast<Revision>().ToList();
        }

        void OnClick(Document doc, RevisionForm revisionForm, ComboBox comboBox1, ComboBox comboBox2, RadioButton radioButton1, RadioButton radioButton2)
        {
            // Declare variables
            string selectedViewSet = comboBox1.SelectedItem.ToString();
            string selectedRevision = comboBox2.SelectedItem.ToString();
            string selectedOption = string.Empty;

            if (radioButton1.Checked)
            {
                selectedOption = radioButton1.Text;
            }
            else if (radioButton2.Checked)
            {
                selectedOption = radioButton2.Text;
            }

            // Get the selected ViewSheetSet by name
            ViewSheetSet viewSheetSet = new FilteredElementCollector(doc).OfClass(typeof(ViewSheetSet)).Cast<ViewSheetSet>().FirstOrDefault(vss => vss.Name == selectedViewSet);

            if (viewSheetSet == null)
            {
                TaskDialog.Show("Error", "ViewSheetSet not found.");
                return;
            }

            // Get the ViewSheets in the selected ViewSheetSet
            List<ViewSheet> viewSheets = new List<ViewSheet>();
            foreach (ViewSheet view in viewSheetSet.Views)
            {
                if (view != null)
                {
                    viewSheets.Add(view);
                }
            }

            // Get the selected Revision by Description
            Revision revision = new FilteredElementCollector(doc).OfClass(typeof(Revision)).Cast<Revision>().FirstOrDefault(r => r.Description == selectedRevision);

            // Start a transaction to modify the Revit Document
            using (Transaction trans = new Transaction(doc, "Update Revisions in ViewSheets"))
            {
                trans.Start();

                foreach (ViewSheet viewSheet in viewSheets)
                {
                    if (viewSheet != null)
                    {
                        // Get the Revision Id
                        ElementId revisionId = revision.Id;

                        // Get all ViewSheets revisions id
                        IList<ElementId> sheet_revisionIds = viewSheet.GetAdditionalRevisionIds().ToList();

                        if (selectedOption == "ADICIONAR")
                        {
                            // Check if the Revision is already on the sheet
                            if (!sheet_revisionIds.Contains(revisionId))
                            {
                                // Add the revision to the sheet
                                sheet_revisionIds.Add(revisionId);
                                viewSheet.SetAdditionalRevisionIds(sheet_revisionIds);
                            }
                        }
                        else if (selectedOption == "REMOVER")
                        {
                            // Check if the Revision is on the sheet
                            if (sheet_revisionIds.Contains(revisionId))
                            {
                                // Add the revision to the sheet
                                sheet_revisionIds.Remove(revisionId);
                                viewSheet.SetAdditionalRevisionIds(sheet_revisionIds);
                            }
                        }
                    }
                }

                trans.Commit();
            }

            revisionForm.Close();
        }
    }

    [Transaction(TransactionMode.Manual)]
    public class OpenPurgeLinePatternsForm : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var uiapp = commandData.Application;
            var app = uiapp.Application;
            var uidoc = uiapp.ActiveUIDocument;
            var doc = uidoc.Document;

            //RevisionForm revisionForm = new RevisionForm();

            //revisionForm.ShowDialog();

            return Result.Succeeded;
        }
    }
}
