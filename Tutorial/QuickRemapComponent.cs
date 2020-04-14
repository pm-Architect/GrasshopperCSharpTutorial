using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Tutorial
{
    public class QuickRemapComponent : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the QuickRemapComponent class.
        /// </summary>
        public QuickRemapComponent()
          : base("Quick Remap", "QRemap",
              "A quick way to remap a list of numbers",
              "Maths", "Domain")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Numbers", "N", "List of Numbers to be remapped", GH_ParamAccess.list);
            pManager.AddIntervalParameter("Target Domain", "D", "Domain to remap the numbers to", GH_ParamAccess.item, new Interval(0, 1));
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Mapped", "M", "Remapped Numbers", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<double> numbers = new List<double>();
            Interval target = new Interval(0, 1);
            if (!(DA.GetDataList<double>(0, numbers))) return;
            if (!(DA.GetData<Interval>(1, ref target))) return;

            Interval source;
            double min = numbers[0];
            double max = numbers[0];
            foreach (double v in numbers)
            {
                if (v < min)
                {
                    min = v;
                }
                if (v > max)
                {
                    max = v;
                }
            }
            source = new Interval(min, max);

            List<double> outNumbers = new List<double>();
            foreach (double v in numbers)
            {
                double v1 = RemapNumber(v, source, target);
                outNumbers.Add(v1);
            }

            DA.SetDataList(0, outNumbers);
        }

        private double RemapNumber(double val, Interval s, Interval t)
        {
            double outVal;

            outVal = ((t.Length) * (((val) - (s.Min)) / (s.Length))) + t.Min;

            return outVal;
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return Tutorial.Properties.Resources.QRemap_icon;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("8485356e-3ea3-4374-a91c-01aa3184b133"); }
        }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.primary;
            }
        }
    }
}