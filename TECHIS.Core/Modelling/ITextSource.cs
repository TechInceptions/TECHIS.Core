using System;
using System.Collections.Generic;
using System.Text;

namespace TECHIS.Core.Modelling
{
    public interface ITextSource
    {
        string GetText();
        void WriteText(System.IO.TextWriter output);
    }
}
