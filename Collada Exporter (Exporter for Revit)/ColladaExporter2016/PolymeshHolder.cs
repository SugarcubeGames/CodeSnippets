using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.DB;

/*
 * Curing hte export process, Polymesh Topologies are generated and delete for each element individually.
 * In order to sort elements by type, material, etc; it is necessary to save their data until every element
 * has been worked through, and then export; rather than expring each element as it comes up.
 * */
namespace ColladaExporter2016
{
    class PolymeshHolder
    {

        IList<PolymeshFacet> facets;
        IList<XYZ> normals;
        IList<XYZ> points;
        IList<UV> uvs;

        Transform transform;

        public DistributionOfNormals DistributionOfNormals { get; set; }

        public int NumberOfFacets { get; set; }
        public int NumberOfNormals { get; set; }
        public int NumberOfPoints { get; set; }
        public int NumberOfUVs { get; set; }

        public PolymeshHolder(PolymeshTopology polymesh)
        {
            facets = polymesh.GetFacets();
            normals = polymesh.GetNormals();
            points = polymesh.GetPoints();
            uvs = polymesh.GetUVs();

            DistributionOfNormals = polymesh.DistributionOfNormals;

            NumberOfFacets = facets.Count;
            NumberOfNormals = normals.Count;
            NumberOfPoints = points.Count;
            NumberOfUVs = uvs.Count;

        }

        public PolymeshFacet GetFacet(int index)
        {
            return facets[index];
        }
        public IList<PolymeshFacet> GetFacets()
        {
            return facets;
        }

        public XYZ GetNormal(int index)
        {
            return normals[index];
        }
        public IList<XYZ> GetNormals()
        {
            return normals;
        }

        public XYZ GetPoint(int index)
        {
            return points[index];
        }
        public IList<XYZ> GetPoints()
        {
            return points;
        }

        public UV GetUV(int index)
        {
            return uvs[index];
        }
        public IList<UV> GetUVs()
        {
            return uvs;
        }

        public void setTransform(Transform transform)
        {
            this.transform = transform;
        }
        public Transform getTransform()
        {
            return transform;
        }
    }
}
