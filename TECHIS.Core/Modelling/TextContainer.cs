using System;
using System.Collections.Generic;
using System.Text;
using TECHIS.Core.Serialization.XML;
using System.Xml.Serialization;

namespace TECHIS.Core.Modelling
{
    public class TextContainer : TECHIS.Core.Modelling.Attribute
    {
        #region Fields
        private TextData _Template;
        #endregion

        #region Properties

        public TextData Template
        {
            get { return _Template; }
            set { _Template = value; }
        }

        #endregion

        #region Constructors
        public TextContainer()
        { }
        public TextContainer(string template)
        {

            if (!string.IsNullOrEmpty(template))
                _Template = new TextData(template);

        }
        #endregion

        #region Public Methods
        public void SetTemplate(string value)
        {
            if (value != null)
                _Template = new TextData(value);
            else
                _Template = null;
        }

        public string ClearTemplate()
        {
            string val = GetTemplate();

            _Template = null;

            return val;
        }

        public string GetTemplate()
        {
            string val = null;

            if (_Template != null)
                val = _Template.Value;

            return val;
        }

        public TextContainer Clone()
        {
            return (TextContainer)(this.MemberwiseClone());
        }

        #endregion
    }
}
