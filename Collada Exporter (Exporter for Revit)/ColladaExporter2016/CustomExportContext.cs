using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Autodesk.Revit.DB;

namespace ColladaExporter2016
{
    class CustomExportContext : IExportContext
    {
        private Document doc;
        public uint CurrentPolymeshIndex;

        XMLWriter writer;

        //TESTING:  Testing a method that would not require me to work through each element prior to exporting
        private Stack<Transform> transformationStack = new Stack<Transform>();

        public CustomExportContext(Document doc, ElementId matId, string loc)
        {
            this.doc = doc;

            //Create a new insatnce of our xml writer for exporting everything.  Pass in the save location for the file.
            writer = new XMLWriter(loc);

            /*T*/
            transformationStack.Push(Transform.Identity);
        }


        #region IExportContext Members

        
        public bool Start()
        {
            //MessageBox.Show("Start");
            //Begin writing our XML file

            writer.WriteBegin();
            writer.WriteAsset();
            writer.WriteLibraryGeometriesBegin();

            CurrentPolymeshIndex = 0;

            return true;
        }

        public RenderNodeAction OnViewBegin(ViewNode node)
        {
            //MessageBox.Show("OnViewBegin");

            return RenderNodeAction.Proceed;
        }

        //A single element begins exporting
        public RenderNodeAction OnElementBegin(ElementId elementId)
        {
            //MessageBox.Show("OnElementBegin");

            //Make sure that the element is still valid for exporting
            Element element = doc.GetElement(elementId);

            if (!element.IsValidObject)
            {
                return RenderNodeAction.Skip;
            }

            //Make sure that this object isn't hidden
            if (element.IsHidden(doc.ActiveView))
            {
                return RenderNodeAction.Skip;
            }

            //Get the element's type.
            Type elementType = element.GetType();

            MessageBox.Show("Type:" + elementType.Name);

            return RenderNodeAction.Proceed;
        }


        public void OnMaterial(MaterialNode node)
        {
            //MessageBox.Show("OnMaterial");


        }

        public void OnPolymesh(PolymeshTopology node)
        {
            //MessageBox.Show("OnPolymesh");

            writer.WriteGeometrySourcePositions(CurrentPolymeshIndex, node, transformationStack);
        }




        public void Finish()
        {
            writer.Close();
            //MessageBox.Show("Finish");
        }

        public bool IsCanceled()
        {
            //MessageBox.Show("IsCanceled");
            return false;
        }

        public void OnDaylightPortal(DaylightPortalNode node)
        {
            //MessageBox.Show("OnDaylightPortal");
        }

        public void OnElementEnd(ElementId elementId)
        {
            //MessageBox.Show("OnElementEnd");
        }

        public RenderNodeAction OnFaceBegin(FaceNode node)
        {
            //MessageBox.Show("OnFaceBegin");
            return RenderNodeAction.Proceed;
        }

        public void OnFaceEnd(FaceNode node)
        {
            //MessageBox.Show("OnFaceEnd");
        }

        public RenderNodeAction OnInstanceBegin(InstanceNode node)
        {
            //MessageBox.Show("OnInstanceBegin");
            return RenderNodeAction.Proceed;
        }

        public void OnInstanceEnd(InstanceNode node)
        {
            //MessageBox.Show("OnInstanceEnd");
        }

        public void OnLight(LightNode node)
        {
            //MessageBox.Show("OnLight");
        }

        public RenderNodeAction OnLinkBegin(LinkNode node)
        {
            //MessageBox.Show("OnLinkBegin");
            return RenderNodeAction.Proceed;
        }

        public void OnLinkEnd(LinkNode node)
        {
            //MessageBox.Show("OnLinkEnd");
        }

        public void OnRPC(RPCNode node)
        {
            //MessageBox.Show("OnRPC");
        }

        public void OnViewEnd(ElementId elementId)
        {
            //MessageBox.Show("OnViewEnd");
        }

        #endregion







        /****************************************************************************************/
        #region XMLWriter

        /**********************************************************************
         * 
         * XML WRITER
         * 
         * *******************************************************************/

        private void XMLBegin()
        {

        }

        #endregion
    }
}
