using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.DB;

namespace ColladaExporter2016
{
    class PolymeshGrouping
    {
        IList<PolymeshFacet> facets;
        IList<XYZ> points;
        IList<XYZ> normals;
        IList<UV> uvs;

        //Unfortunately, it is necessary to track the transform per point rather than per polymesh
        IList<Transform> transformPerPoint;

        public DistributionOfNormals DistributionOfNormals { get; set; }

        public int NumberOfFacets { get; set; }
        public int NumberOfPoints { get; set; }
        public int NumberOfNormals { get; set; }
        public int NumberOfUVs { get; set; }

        //Track the number of polymeshes that go into this.
        public int NumberOfPolymeshes { get; set; }

        //Becuase the number of facets and points in a given polymesh can vary, 
        //we need to have a way to associate teh appropriate normals with each point/facet grouping
        public IList<int> normalPerFacet;
        public IList<int> pointsPerFacet;

        public IEnumerator<DistributionOfNormals> distributionList;

        //The materialId associated with this polymesh Grouping
        private ElementId matId;

        public PolymeshGrouping()
        {
            facets = new List<PolymeshFacet>();
            points = new List<XYZ>();
            normals = new List<XYZ>();
            uvs = new List<UV>();
            transformPerPoint = new List<Transform>();

            normalPerFacet = new List<int>();
            pointsPerFacet = new List<int>();

            NumberOfPolymeshes = 0;
        }

        public void addPolymeshData(PolymeshHolder polymesh)
        {

            DistributionOfNormals = polymesh.DistributionOfNormals;

            //Add the polymesh's facets

            foreach (PolymeshFacet f in polymesh.GetFacets())
            {
                facets.Add(f);

                normalPerFacet.Add(NumberOfPolymeshes);
                pointsPerFacet.Add(NumberOfPoints);
            }
            //Update teh number of facets
            NumberOfFacets = facets.Count;

            //Add the polymesh's points
            foreach (XYZ p in polymesh.GetPoints())
            {
                points.Add(p);
                //It is necessary to track each point's transform for proper offsetting
                //for specific types of elemnts (railings)
                if (polymesh.getTransform() != null)
                {
                    transformPerPoint.Add(polymesh.getTransform());
                }
            }
            //Update the number of points
            NumberOfPoints = points.Count;

            //Add the polymesh's normal;
            foreach (XYZ n in polymesh.GetNormals())
            {
                normals.Add(n);
            }
            //update teh number of normals
            NumberOfNormals = normals.Count;

            //Add Polymesh's UVs
            foreach (UV u in polymesh.GetUVs())
            {
                uvs.Add(u);
            }
            //update the number of uvs
            NumberOfUVs = uvs.Count;

            NumberOfPolymeshes++;
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

        public void setMaterialId(ElementId matId)
        {
            this.matId = matId;
        }
        public ElementId getMaterialId()
        {
            return matId;
        }

        public IList<Transform> getTransforms()
        {
            return transformPerPoint;
        }
    }
}
