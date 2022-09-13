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
    [TransactionAttribute(TransactionMode.ReadOnly)]
    public class GetElementId : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Get UIDocument
            UIDocument uidoc = commandData.Application.ActiveUIDocument;

            //Get Document
            Document  doc = uidoc.Document;
            try
            {
                //Picking The Object
                Reference Pickedobj = uidoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element);

                //Saving Element ID
                ElementId eleId = Pickedobj.ElementId;
                Element ele = doc.GetElement(eleId);
                ElementId eTypeID = ele.GetTypeId();
                ElementType eType = doc.GetElement(eTypeID) as ElementType;

                if (Pickedobj != null)
                {
                    TaskDialog.Show("Element Classification ", "Elemnt ID " + ele.Id.ToString() + Environment.NewLine + 
                        "Category - " + ele.Category.Name + Environment.NewLine 
                        + "Instance - "  + ele.Name + Environment.NewLine
                        + "Family-  "  + eType.FamilyName + Environment.NewLine
                        + "Symbol - "  + eType.Name );
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
