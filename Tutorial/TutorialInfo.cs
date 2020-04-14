using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace Tutorial
{
    public class TutorialInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "XBox Controller Plugin";
            }
        }
        public override Bitmap Icon
        {
            get
            {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return Tutorial.Properties.Resources.Xbox_icon;
            }
        }
        public override string Description
        {
            get
            {
                //Return a short string describing the purpose of this GHA library.
                return "";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("2baac05b-e850-4291-a113-2ca65fd2cfe7");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "ARPM";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "";
            }
        }
    }
}
