using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;

namespace MyRevitCommands
{
    [TransactionAttribute(TransactionMode.Manual)]
    public class DeleteElement : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Get UIDocument
            UIDocument uidoc = commandData.Application.ActiveUIDocument;

            //Get Document
            Document doc = uidoc.Document;

            try
            {
                //Picking The Object
                Reference Pickedobj = uidoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element);

                if (Pickedobj != null)
                {
                   using (Transaction trans = new Transaction(doc, "Delete Element"))
                    {
                        trans.Start();
                        doc.Delete(Pickedobj.ElementId);

                        TaskDialog TaskDia1 = new TaskDialog("Delete Element");
                        TaskDia1.MainContent = "Are You Sure you want to Delete Selected Elements";
                        TaskDia1.CommonButtons = TaskDialogCommonButtons.Ok | TaskDialogCommonButtons.Cancel;

                        if(TaskDia1.Show()== TaskDialogResult.Ok)
                        {
                            trans.Commit();
                            TaskDialog.Show("Element Delete", Pickedobj.ElementId.ToString() + " is Deleted !");
                        }
                        else
                        {
                            trans.RollBack();
                            TaskDialog.Show("Element Delete", Pickedobj.ElementId.ToString() + " is Not Deleted !");

                        }
                    }
                }
            }
            catch (Exception e)
            {
                message = e.Message;
                return Result.Failed;
            }
            return Result.Succeeded;
        }
    }
}
