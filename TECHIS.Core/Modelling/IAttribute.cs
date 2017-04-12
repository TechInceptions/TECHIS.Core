using System;
using System.Collections.Generic;
using System.Text;

namespace TECHIS.Core.Modelling
{
    public interface IAttribute
    {
        string Name
        {
            get;
            set;
        }

        string Value
        {
            get;
            set;
        }

        string Description
        {
            get;
            set;
        }

        string Id
        {
            get;
            set;
        }
    }
}
