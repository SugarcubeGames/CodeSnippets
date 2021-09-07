using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Autodesk.Revit.DB;

namespace ColladaExporter2016
{
    class XMLWriter
    {
        StreamWriter writer;

        public XMLWriter(string location)
        {
            writer = new StreamWriter(location);
        }

        public void WriteBegin()
        {
            writer.Write("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n");
            writer.Write("<COLLADA xmlns=\"http://www.collada.org/2005/11/COLLADASchema\" version=\"1.4.1\">\n");
        }

        public void WriteAsset()
        {
            writer.Write("\t<asset>\n");
            writer.Write("\t\t<contributor>\n");
            writer.Write("\t\t\t<authoring_tool>Collada Revit Export Tool</authoring_tool>\n");
            writer.Write("\t\t</contributor>\n");
            writer.Write("\t\t<created>" + DateTime.Now.ToString() + "</created>\n");

            //Units
            writer.Write("\t\t<unit name=\"feet\" meter=\"0.3048\"/>\n");
            writer.Write("\t\t<up_axis>Z_UP</up_axis>\n");
            writer.Write("\t</asset>\n");
        }

        public void WriteLibraryGeometriesBegin()
        {
            writer.Write("\t<library_geometries>\n");
        }

        public void WriteGeometryBegin(uint index, string name)
        {
            writer.Write("\t\t<geometry id=\"geom-" + index + "\" name=\"ID_" + name + "\">\n");
            writer.Write("\t\t\t<mesh>\n");
        }

        public void WriteGeometrySourcePositions(uint index, PolymeshTopology polymesh, Stack<Transform> transformationStack)
        {
            writer.Write("<source id=\"geom-" + index + "-positions" + "\">\n");
            writer.Write("<float_array id=\"geom-" + index + "-positions" + "-array" + "\" count=\"" + (polymesh.NumberOfPoints * 3) + "\">\n");

            XYZ point;
            Transform currentTransform = transformationStack.Peek();

            for (int iPoint = 0; iPoint < polymesh.NumberOfPoints; ++iPoint)
            {
                point = polymesh.GetPoint(iPoint);
                point = currentTransform.OfPoint(point);
                writer.Write("{0:0.0000} {1:0.0000} {2:0.0000}\n", point.X, point.Y, point.Z);
            }

            writer.Write("</float_array>\n");
            writer.Write("<technique_common>\n");
            writer.Write("<accessor source=\"#geom-" + index + "-positions" + "-array\"" + " count=\"" + polymesh.NumberOfPoints + "\" stride=\"3\">\n");
            writer.Write("<param name=\"X\" type=\"float\"/>\n");
            writer.Write("<param name=\"Y\" type=\"float\"/>\n");
            writer.Write("<param name=\"Z\" type=\"float\"/>\n");
            writer.Write("</accessor>\n");
            writer.Write("</technique_common>\n");
            writer.Write("</source>\n");
        }

        public void WriteGeometryEnd()
        {
            writer.Write("\t\t\t</mesh>\n");
            writer.Write("\t\t</geometry>\n");
        }



        public void Close()
        {
            writer.Write("</COLLADA>\n");
            writer.Close();
        }
    }
}
