using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;

namespace AutoCADPlugin
{
    public class Commands : IExtensionApplication
    {
        // This method is called when the plugin is loaded
        public void Initialize()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            ed.WriteMessage("\nAutoCAD Plugin loaded successfully!");
            ed.WriteMessage("\nType 'HELLO' or 'DRAWCIRCLE' to test the commands.\n");
        }

        public void Terminate()
        {
            // Cleanup code if needed
        }

        // Simple command to test debugging
        [CommandMethod("HELLO")]
        public void HelloCommand()
        {
            // Set a breakpoint here to test debugging
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            
            // Prompt for user name
            PromptStringOptions pso = new PromptStringOptions("\nEnter your name: ")
            {
                AllowSpaces = true
            };
            
            PromptResult pr = ed.GetString(pso);
            
            if (pr.Status == PromptStatus.OK)
            {
                string name = pr.StringResult;
                ed.WriteMessage($"\nHello, {name}! This is your AutoCAD plugin working.\n");
            }
        }

        // Command to draw a circle
        [CommandMethod("DRAWCIRCLE")]
        public void DrawCircleCommand()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            // Prompt for center point
            PromptPointOptions ppo = new PromptPointOptions("\nSpecify center point for circle: ");
            PromptPointResult ppr = ed.GetPoint(ppo);
            
            if (ppr.Status != PromptStatus.OK)
                return;

            Point3d centerPoint = ppr.Value;

            // Prompt for radius
            PromptDistanceOptions pdo = new PromptDistanceOptions("\nSpecify radius of circle: ")
            {
                BasePoint = centerPoint,
                UseBasePoint = true
            };
            
            PromptDoubleResult pdr = ed.GetDistance(pdo);
            
            if (pdr.Status != PromptStatus.OK)
                return;

            double radius = pdr.Value;

            // Create the circle
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    // Set a breakpoint here to inspect the transaction
                    BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
                    BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);

                    using (Circle circle = new Circle(centerPoint, Vector3d.ZAxis, radius))
                    {
                        btr.AppendEntity(circle);
                        tr.AddNewlyCreatedDBObject(circle, true);
                    }

                    tr.Commit();
                    ed.WriteMessage($"\nCircle created at {centerPoint} with radius {radius}.\n");
                }
                catch (System.Exception ex)
                {
                    ed.WriteMessage($"\nError creating circle: {ex.Message}\n");
                    tr.Abort();
                }
            }
        }
    }
}
