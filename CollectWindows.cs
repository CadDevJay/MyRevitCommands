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
    public class CollectWindows : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Get UIDocument
            UIDocument uidoc = commandData.Application.ActiveUIDocument;

            //Get Document
            Document doc = uidoc.Document;

            //Create Filtered Element Collector
            FilteredElementCollector collector = new FilteredElementCollector(doc);

            //Creating Filter
            ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_Windows);

            //Apply Filter
            IList<Element> Windows = collector.WherePasses(filter).WhereElementIsNotElementType().ToElements();

            TaskDialog.Show("Count of Windows", String.Format("we have Counted {0} number of Windows", Windows.Count));

            return Result.Succeeded;
        }
    }
}
