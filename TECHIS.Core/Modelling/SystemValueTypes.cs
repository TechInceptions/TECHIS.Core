using System;
using System.Collections.Generic;
using System.Text;

namespace TECHIS.Core.Modelling
{
    /// <summary>
    /// A StringArrayConstant that lists the system types
    /// </summary>
    public sealed class SystemValueTypes : StringArrayConstant
    {
        public static readonly string[] Values = new string[]{
        MetaType.BYTE_ARRAY, //1
        MetaType.INT64, //2
        MetaType.STRING, //3
        MetaType.DATETIME, //4
        MetaType.DECIMAL, //5
        MetaType.DOUBLE, //6
        MetaType.BYTE_ARRAY, //7
        MetaType.INT32, //8
        MetaType.DECIMAL, //9
        MetaType.STRING, //10
        MetaType.STRING, //11
        MetaType.STRING, //12
        MetaType.DECIMAL, //13
        MetaType.SINGLE, //14
        MetaType.DATETIME, //15
        MetaType.INT16, //16
        MetaType.DECIMAL, //17
        MetaType.OBJECT, //18
        MetaType.STRING, //19
        MetaType.BYTE_ARRAY, //20
        MetaType.BYTE, //21
        MetaType.BYTE_ARRAY, //22
        MetaType.STRING, //23
        MetaType.GUID, //24
        MetaType.BOOLEAN, //25
        MetaType.STRING, //26
        MetaType.STRING, //27
        MetaType.DATETIME, //28
        MetaType.DATETIMEOFFSET, //29
        MetaType.TIMESPAN, //30
        MetaType.DATETIME, //31
        MetaType.OBJECT, //32
        MetaType.OBJECT, //33
        MetaType.OBJECT, //34
        MetaType.OBJECT, //35
        };



        public override string[] GetValues()
        {
            return Values;
        }

        public SystemValueTypes(string value) : base(value) { }

        public SystemValueTypes() { }

    }
}