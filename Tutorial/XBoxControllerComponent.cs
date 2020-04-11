using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;
using BrandonPotter.XBox;

namespace Tutorial
{
    public class XBoxControllerComponent : GH_Component
    {
        private XBoxController Controller { get; set; }

        /// <summary>
        /// Initializes a new instance of the XBoxControllerComponent class.
        /// </summary>
        public XBoxControllerComponent()
          : base("XBox Controller", "Nickname",
              "Outputs Data from a connected XBox Controller",
              "Firefly", "Arduino & I/O Boards")
        {
            var connectedControllers = XBoxController.GetConnectedControllers();
            XBoxControllerWatcher watcher = new XBoxControllerWatcher();
            watcher.ControllerConnected += (c) => Connected(c);
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddBooleanParameter("A", "A", "A", GH_ParamAccess.item); //0
            pManager.AddBooleanParameter("B", "B", "B", GH_ParamAccess.item); //1
            pManager.AddBooleanParameter("X", "X", "X", GH_ParamAccess.item); //2
            pManager.AddBooleanParameter("Y", "Y", "Y", GH_ParamAccess.item); //3
            pManager.AddBooleanParameter("ButtonShoulderLeft", "ButtonShoulderLeft", "ButtonShoulderLeft", GH_ParamAccess.item); //4
            pManager.AddBooleanParameter("ButtonShoulderRight", "ButtonShoulderRight", "ButtonShoulderRight", GH_ParamAccess.item); //5
            pManager.AddBooleanParameter("Up", "Up", "Up", GH_ParamAccess.item); //6
            pManager.AddBooleanParameter("Down", "Down", "Down", GH_ParamAccess.item); //7
            pManager.AddBooleanParameter("Left", "Left", "Left", GH_ParamAccess.item); //8
            pManager.AddBooleanParameter("Right", "Right", "Right", GH_ParamAccess.item); //9
            pManager.AddBooleanParameter("Start", "Start", "Start", GH_ParamAccess.item); //10
            pManager.AddBooleanParameter("Back", "Back", "Back", GH_ParamAccess.item); //11
            pManager.AddBooleanParameter("TriggerLeft", "TriggerLeft", "TriggerLeft", GH_ParamAccess.item); //12
            pManager.AddBooleanParameter("TriggerRight", "TriggerRight", "TriggerRight", GH_ParamAccess.item); //13
            pManager.AddBooleanParameter("ThumbpadLeft", "ThumbpadLeft", "ThumbpadLeft", GH_ParamAccess.item); //14
            pManager.AddBooleanParameter("ThumbpadRight", "ThumbpadRight", "ThumbpadRight", GH_ParamAccess.item); //15
            pManager.AddNumberParameter("LeftThumbstickX", "LeftThumbstickX", "LeftThumbstickX", GH_ParamAccess.item); //16
            pManager.AddNumberParameter("LeftThumbstickY", "LeftThumbstickY", "LeftThumbstickY", GH_ParamAccess.item); //17
            pManager.AddNumberParameter("RightThumbstickX", "RightThumbstickX", "RightThumbstickX", GH_ParamAccess.item); //18
            pManager.AddNumberParameter("RightThumbstickY", "RightThumbstickY", "RightThumbstickY", GH_ParamAccess.item); //19
            pManager.AddNumberParameter("TiggerLeftPosition", "TiggerLeftPosition", "TiggerLeftPosition", GH_ParamAccess.item); //20
            pManager.AddNumberParameter("TiggerRightPosition", "TiggerRightPosition", "TiggerRightPosition", GH_ParamAccess.item); //21
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (Controller != null)
            {
                DA.SetData(0, Controller.ButtonAPressed);
                DA.SetData(1, Controller.ButtonBPressed);
                DA.SetData(2, Controller.ButtonXPressed);
                DA.SetData(3, Controller.ButtonYPressed);
                DA.SetData(4, Controller.ButtonShoulderLeftPressed);
                DA.SetData(5, Controller.ButtonShoulderRightPressed);
                DA.SetData(6, Controller.ButtonUpPressed);
                DA.SetData(7, Controller.ButtonDownPressed);
                DA.SetData(8, Controller.ButtonLeftPressed);
                DA.SetData(9, Controller.ButtonRightPressed);
                DA.SetData(10, Controller.ButtonStartPressed);
                DA.SetData(11, Controller.ButtonBackPressed);
                DA.SetData(12, Controller.TriggerLeftPressed);
                DA.SetData(13, Controller.TriggerRightPressed);
                DA.SetData(14, Controller.ThumbpadLeftPressed);
                DA.SetData(15, Controller.ThumbpadRightPressed);
                DA.SetData(16, Controller.ThumbLeftX);
                DA.SetData(17, Controller.ThumbLeftY);
                DA.SetData(18, Controller.ThumbRightX);
                DA.SetData(19, Controller.ThumbRightY);
                DA.SetData(20, Controller.TriggerLeftPosition);
                DA.SetData(21, Controller.TriggerRightPosition);
            }
        }

        private void Connected(XBoxController c)
        {
            Console.WriteLine("Controller " + c.PlayerIndex.ToString() + " Connected.");
            Controller = c;
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
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("1b0ff7e3-61bc-488d-adb2-8295800cf164"); }
        }
    }
}