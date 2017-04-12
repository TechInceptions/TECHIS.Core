using System;
using System.Collections.Generic;
using System.Text;

namespace TECHIS.Core.Modelling
{
    public abstract class StringArrayConstant
    {
        #region Fields 
        private string _Value;
        private bool _IgnoreCase;
        #endregion

        #region Properties 
        public string Value
        {
            get
            {
                return _Value;
            }
            set
            {
                SetValue(value);
            }
        }
        #endregion

        #region Constructors 
        public StringArrayConstant() { }

        public StringArrayConstant(string value, bool IgnoreCase)
        {
            _IgnoreCase = IgnoreCase;
            SetValue(value);
        }

        public StringArrayConstant(string value)
            : this(value, true)
        {

        }
        #endregion

        #region Public Methods 
        public void SetValue(string value)
        {
            if (IsInArray(value))
            {
                _Value = value;
            }
            else
            {
                throw new InvalidOperationException(String.Format("The value '{0}' is not in the array.", (null != value) ? value : "<null>"));
            }
        }

        public abstract string[] GetValues();

        private bool IsInArray(string value)
        {
            //string[] values = GetValues();

            bool retval = false;

            string[] values = GetValues();

            foreach (string s in values)
            {
                if (string.Compare(s, value, _IgnoreCase) == 0)
                {
                    retval = true;
                    break;
                }
            }

            return retval;
        }
        #endregion
    }
}