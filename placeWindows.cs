using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using System.Security.Cryptography.X509Certificates;

namespace MyRevitCommands
{
    [TransactionAttribute(TransactionMode.Manual)]
    public class placeWindows : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Get UIDocument and Document
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            try
            {
                //Pick Object
                Reference pickedObj = uidoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element);


                FilteredElementCollector Collector = new FilteredElementCollector(doc);
                IList<Element> symbols = Collector.OfClass(typeof(FamilySymbol)).WhereElementIsElementType().ToElements();
                FamilySymbol symbol = null;

                foreach (Element ele1 in symbols)
                {
                    if (ele1.Name == "36\" x 48\"")
                    {
                        symbol = ele1 as FamilySymbol;
                        break;
                    }
                }


                //Display Element Id
                if (pickedObj != null)
                {
                    //Retrieve Element
                    ElementId eleId = pickedObj.ElementId;
                    Element ele = doc.GetElement(eleId);
                    //Set Location
                    LocationCurve locCurve1 = ele.Location as LocationCurve;

                    XYZ pt1 = locCurve1.Curve.GetEndPoint(0);
                    XYZ pt2 = locCurve1.Curve.GetEndPoint(1);
                    XYZ Base1 = new XYZ(0, 0, 4);
                    XYZ pt3 = (pt1 / 2 + pt2 / 2) + Base1;
                    
                   
                    double Length1 = pt1.DistanceTo(pt3);
                    double Length2 = locCurve1.Curve.Length;
                    TaskDialog.Show("Wall Length", String.Format("Wall Length 1 is {0} and Length 2 is {1}", Length1, Length2));



                    using (Transaction trans = new Transaction(doc, "Change Location"))
                    {
                        trans.Start();

                        
                            if (!symbol.IsActive)
                            {
                                symbol.Activate();
                            }

                            doc.Create.NewFamilyInstance(pt3, symbol, ele, Autodesk.Revit.DB.Structure.StructuralType.NonStructural);

                        
                        trans.Commit();
                  

                    }
                    
                }

                return Result.Succeeded;
            }
            catch (Exception e)
            {
                message = e.Message;
                return Result.Failed;
            }

        }
    } 
}
