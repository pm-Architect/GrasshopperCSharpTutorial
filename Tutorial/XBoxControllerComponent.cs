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
          : base("XBox Controller", "XBoxCont",
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
            pManager.AddIntegerParameter("ControllerRefreshInterval", "ControllerRefreshInterval", "ControllerRefreshInterval", GH_ParamAccess.item, 50); //0
            pManager.AddNumberParameter("LeftMotorVibrationSpeed", "LeftMotorVibrationSpeed", "LeftMotorVibrationSpeed", GH_ParamAccess.item, 0.0); //1
            pManager.AddNumberParameter("RightMotorVibrationSpeed", "RightMotorVibrationSpeed", "RightMotorVibrationSpeed", GH_ParamAccess.item, 0.0); //2
            pManager.AddNumberParameter("SnapDeadZoneCenter", "SnapDeadZoneCenter", "SnapDeadZoneCenter", GH_ParamAccess.item, 50.0); //3
            pManager.AddNumberParameter("SnapDeadZoneTolerance", "SnapDeadZoneTolerance", "SnapDeadZoneTolerance", GH_ParamAccess.item, 10.0); //4
            pManager.AddNumberParameter("LeftTriggerThreshold", "LeftTriggerThreshold", "LeftTriggerThreshold", GH_ParamAccess.item, 10.0); //5
            pManager.AddNumberParameter("RightTriggerThreshold", "RightTriggerThreshold", "RightTriggerThreshold", GH_ParamAccess.item, 10.0); //6
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
            pManager.AddBooleanParameter("AllButtons", "AllButtons", "AllButton", GH_ParamAccess.list); //22
            pManager.AddNumberParameter("AllThumbsticksAndTriggers", "AllThumbsticksAndTriggers", "AllThumbsticksAndTriggers", GH_ParamAccess.list); //23
            pManager.AddVectorParameter("LeftThumbstickVector", "LeftThumbstickVector", "LeftThumbstickVector", GH_ParamAccess.item); //24
            pManager.AddVectorParameter("RightThumbstickVector", "RightThumbstickVector", "RightThumbstickVector", GH_ParamAccess.item); //25
            pManager.AddIntegerParameter("PlayerIndex", "PlayerIndex", "PlayerIndex", GH_ParamAccess.item); //26
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (Controller != null)
            {
                List<bool> buttons = new List<bool>();
                List<double> thumbsttriggers = new List<double>();
                Vector3d lt = new Vector3d(Controller.ThumbLeftX, Controller.ThumbLeftY, 0);
                Vector3d rt = new Vector3d(Controller.ThumbRightX, Controller.ThumbRightY, 0);

                int refreshInt = 0;
                double leftVib = 0.0;
                double rightVib = 0.0;
                double sdzCent = 10.0;
                double sdzThr = 10.0;
                double ltThr = 10.0;
                double rtThr = 10.0;

                if (!(DA.GetData(0, ref refreshInt))) return;
                if (!(DA.GetData(1, ref leftVib))) return;
                if (!(DA.GetData(2, ref rightVib))) return;
                if (!(DA.GetData(3, ref sdzCent))) return;
                if (!(DA.GetData(4, ref sdzThr))) return;
                if (!(DA.GetData(5, ref ltThr))) return;
                if (!(DA.GetData(6, ref rtThr))) return;

                Controller.RefreshIntervalMilliseconds = refreshInt;
                Controller.SetLeftMotorVibrationSpeed(leftVib);
                Controller.SetRightMotorVibrationSpeed(rightVib);
                Controller.SnapDeadZoneCenter = sdzCent;
                Controller.SnapDeadZoneTolerance = sdzThr;
                Controller.TriggerLeftPressThreshold = ltThr;
                Controller.TriggerRightPressThreshold = rtThr;

                DA.SetData(0, Controller.ButtonAPressed);
                buttons.Add(Controller.ButtonAPressed);
                DA.SetData(1, Controller.ButtonBPressed);
                buttons.Add(Controller.ButtonBPressed);
                DA.SetData(2, Controller.ButtonXPressed);
                buttons.Add(Controller.ButtonXPressed);
                DA.SetData(3, Controller.ButtonYPressed);
                buttons.Add(Controller.ButtonXPressed);
                DA.SetData(4, Controller.ButtonShoulderLeftPressed);
                buttons.Add(Controller.ButtonShoulderLeftPressed);
                DA.SetData(5, Controller.ButtonShoulderRightPressed);
                buttons.Add(Controller.ButtonShoulderRightPressed);
                DA.SetData(6, Controller.ButtonUpPressed);
                buttons.Add(Controller.ButtonUpPressed);
                DA.SetData(7, Controller.ButtonDownPressed);
                buttons.Add(Controller.ButtonDownPressed);
                DA.SetData(8, Controller.ButtonLeftPressed);
                buttons.Add(Controller.ButtonLeftPressed);
                DA.SetData(9, Controller.ButtonRightPressed);
                buttons.Add(Controller.ButtonRightPressed);
                DA.SetData(10, Controller.ButtonStartPressed);
                buttons.Add(Controller.ButtonStartPressed);
                DA.SetData(11, Controller.ButtonBackPressed);
                buttons.Add(Controller.ButtonBackPressed);
                DA.SetData(12, Controller.TriggerLeftPressed);
                buttons.Add(Controller.TriggerLeftPressed);
                DA.SetData(13, Controller.TriggerRightPressed);
                buttons.Add(Controller.TriggerRightPressed);
                DA.SetData(14, Controller.ThumbpadLeftPressed);
                buttons.Add(Controller.ThumbpadLeftPressed);
                DA.SetData(15, Controller.ThumbpadRightPressed);
                buttons.Add(Controller.ThumbpadRightPressed);
                DA.SetData(16, Controller.ThumbLeftX);
                thumbsttriggers.Add(Controller.ThumbLeftX);
                DA.SetData(17, Controller.ThumbLeftY);
                thumbsttriggers.Add(Controller.ThumbLeftY);
                DA.SetData(18, Controller.ThumbRightX);
                thumbsttriggers.Add(Controller.ThumbRightX);
                DA.SetData(19, Controller.ThumbRightY);
                thumbsttriggers.Add(Controller.ThumbRightY);
                DA.SetData(20, Controller.TriggerLeftPosition);
                thumbsttriggers.Add(Controller.TriggerLeftPosition);
                DA.SetData(21, Controller.TriggerRightPosition);
                thumbsttriggers.Add(Controller.TriggerRightPosition);
                DA.SetDataList(22, buttons);
                DA.SetDataList(23, thumbsttriggers);
                DA.SetData(24, lt);
                DA.SetData(25, rt);
                DA.SetData(26, Controller.PlayerIndex);

                this.Message = ("Connected: " + Controller.IsConnected + Environment.NewLine + "Refresh Interval: " + Controller.RefreshIntervalMilliseconds);
            }
            else
            {
                this.Message = "No Controller Connected";
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
                return Tutorial.Properties.Resources.Xbox_icon;
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