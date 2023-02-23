using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomNumber
{
    [Transaction(TransactionMode.Manual)]

    public class NumberOfRoom : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;
            List<Room> rooms = FindRooms(doc);

            if (rooms.Count == 0)
            {
                TaskDialog.Show("Ошибка", "Не найдены помещения");
                return Result.Cancelled;
            }

            CreateNumber(doc, rooms);

            return Result.Succeeded;
        }
        private List<Room> FindRooms(Document doc)              
        {
            List<Room> rooms = new FilteredElementCollector(doc)
              .OfClass(typeof(SpatialElement))
              .Cast<Room>()
              .ToList();

            return rooms;
        }

        private void CreateNumber(Document doc, List<Room> rooms) //нумерация помещений
        {
            Transaction transaction = new Transaction(doc);       
            transaction.Start("Нумерация помещений");

            for (int i = 0; i < rooms.Count; i++)
            {
                Room room = rooms[i];
                Parameter numberOfRoom = room.get_Parameter(BuiltInParameter.ROOM_NUMBER);
                numberOfRoom.Set($"{1 + i}");
            }
            transaction.Commit();
        }
    }
}
