using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ColladaExporter2016
{
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalCommand
    {

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet element)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Autodesk.Revit.ApplicationServices.Application app = uiapp.Application;
            Document doc = uidoc.Document;

            if (doc.ActiveView as View3D != null)
            {
                //MessageBox.Show("You're in a 3d view... good....");

                String exportLoc;
                exportLoc = doc.PathName;

                SaveFileDialog fileDialog = new SaveFileDialog();

                fileDialog.Title = "Select a location to export the model.";
                fileDialog.InitialDirectory = exportLoc;
                fileDialog.Filter = "COLLADA Files(*.dae)|*.dae";

                if (fileDialog.ShowDialog() != DialogResult.OK)
                {
                    return Result.Cancelled;
                }

                exportLoc = fileDialog.FileName;

                MessageBox.Show("Export Location: " + exportLoc);

                return ExportView(doc, doc.ActiveView as View3D, exportLoc);
            }
            else
            {
                MessageBox.Show("You must be in a 3D view to Export.");
                return Result.Cancelled;
            }
        }


        //Set up the export view and commence export
        internal Result ExportView(Document doc, View3D view, string loc)
        {
            CustomExportContext context = new CustomExportContext(doc, getByCategoryMat(doc), loc);

            CustomExporter exporter = new CustomExporter(doc, context);

            exporter.IncludeFaces = false;
            exporter.ShouldStopOnError = false;

            //Certain caegories will cause errors due to a lack of phsyical geometry.  If they are visible in the view, we will turn them
            //off, then turn them back on afterwards;

            //Turn off things in this view that will not export correctly
            using (Transaction t = new Transaction(doc, "Hiding necessary categories"))
            {
                t.Start();

                Category lineCategory = doc.Settings.Categories.get_Item(BuiltInCategory.OST_Lines);
                bool lineVisibility = doc.ActiveView.GetVisibility(lineCategory);
                if (lineVisibility)
                {
                    doc.ActiveView.SetVisibility(lineCategory, false);
                }

                Category massCategory = doc.Settings.Categories.get_Item(BuiltInCategory.OST_Mass);
                bool massVisibility = doc.ActiveView.GetVisibility(massCategory);
                if (massVisibility)
                {
                    doc.ActiveView.SetVisibility(massCategory, false);
                }

                t.Commit();
            }



            try
            {
                if (context == null)
                {
                    MessageBox.Show("There has been a serious error.  Context is null.  Cancelling export.");
                    return Result.Failed;
                }
                else if (doc == null)
                {
                    MessageBox.Show("There has been a serious error.\nThe exporter has lost its association with the documen.  Please try again.");
                    return Result.Failed;
                }
                else if (view == null)
                {
                    MessageBox.Show("You must be in a 3D view to Export.");
                    return Result.Failed;
                }

                exporter.Export(view);
            }
            catch (Exception e)
            {
                MessageBox.Show("There has been an error exporting.  Please refer to the Revit log file for details.");
                return Result.Failed;
            }

            MessageBox.Show("Exporting Completed Successfully");

            return Result.Succeeded;
        }


        internal ElementId getByCategoryMat(Document doc)
        {
            Dictionary<string, Material> materials = new FilteredElementCollector(doc)
                                                         .OfClass(typeof(Material))
                                                         .Cast<Material>()
                                                         .ToDictionary<Material, string>(
                                                         e => e.Name);

            string matName = "By_Category";
            foreach (var pair in materials)
            {
                if (pair.Key.Equals(matName))
                {
                    return pair.Value.Id;
                }
            }

            ElementId newMat;
            using (Transaction t = new Transaction(doc, "Creating \"By_Category\" material."))
            {
                t.Start();
                newMat = Material.Create(doc, matName);
                t.Commit();
            }

            return newMat;
        }
    }
}
