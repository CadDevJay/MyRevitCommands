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
    internal class placeFamily : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;
            FilteredElementCollector Collector = new FilteredElementCollector(doc);
            IList<Element> symbols = Collector.OfClass(typeof(FamilySymbol)).WhereElementIsElementType().ToElements();

            FamilySymbol symbol = null;
            foreach(Element ele in symbols)
            {
                if (ele.Name == "60\" x 30\" Student")
                {
                    symbol = ele as FamilySymbol;
                    break;
                }
            }


            try
            {
                using (Transaction trans = new Transaction(doc, "Place a Family"))
                {
                    trans.Start();
                    if (!symbol.IsActive)
                    {
                        symbol.Activate();
                    }

                    doc.Create.NewFamilyInstance(new XYZ(0, 0, 45), symbol, Autodesk.Revit.DB.Structure.StructuralType.NonStructural);
                    
                    trans.Commit();

                    return Result.Succeeded;

                }
            }
            catch (Exception e)
            {
                message = e.Message;
                return Result.Failed;
            }
        }
    }
}
