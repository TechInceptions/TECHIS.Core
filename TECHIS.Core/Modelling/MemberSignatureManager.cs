using System;
using System.Collections.Generic;
using System.Text;

namespace TECHIS.Core.Modelling
{
    /// <summary>
    /// This class can be used to generate new names for class members with the same signature
    /// </summary>
    public class MemberSignatureManager
    {

        #region Fields
        private List<IList<string>> _ExistingMethodNames;
        //private List<string> _DefinitiveSignature;
        #endregion

        #region Constructors
        public MemberSignatureManager()
        {
            _ExistingMethodNames = new List<IList<string>>(20);
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a valid member name based on the member signatures already defined
        /// </summary>
        public StringBuilder GetUniqueSignature(string NameRoot)
        {
            List<string> definitiveSignature = new List<string>(1);
            
            definitiveSignature.Add(NameRoot);
            return GetUniqueSignature(definitiveSignature, NameRoot);
        }

        /// <summary>
        /// Returns a valid member name based on the member signatures already defined
        /// </summary>
        /// <param name="definitiveSignature">The suggested memeber signature. The member name is the first item in the list</param>
        /// <param name="nameRoot">This is the ideal name of the member. The pure unadjusted name</param>
        /// <returns>A member name that would be valid for the type. This is the NameRoot + a numeric count</returns>
        public StringBuilder GetUniqueSignature(List<string> definitiveSignature, string nameRoot)
        {
            while (DoesMethodSignatureExist(definitiveSignature))
            {
                int SignCount;

                string derivedPortion = definitiveSignature[0].Replace(nameRoot, string.Empty);

                if (derivedPortion.Length > 0)
                {
                    if (int.TryParse(derivedPortion, out SignCount))
                    {
                        definitiveSignature[0] = nameRoot + ((SignCount + 1).ToString());
                    }
                    else
                    {
                        definitiveSignature[0] = definitiveSignature[0] + "1";
                    }
                }
                else
                {
                    definitiveSignature[0] = definitiveSignature[0] + "1";
                }
            }

            _ExistingMethodNames.Add(definitiveSignature);

            return (new StringBuilder(definitiveSignature[0]));
        }

        /// <summary>
        /// Clear the list of names that should be excluded
        /// </summary>
        public void ClearExistingNames()
        {
            _ExistingMethodNames.Clear();
        }

        /// <summary>
        /// Add a list of names that should be excluded
        /// </summary>
        public void SetExistingNames(List<string> names)
        {
            foreach (string s in names)
            {
                List<string> name = new List<string>(1);
                name.Add(s);
                _ExistingMethodNames.Add(name);
            }
        }

        /// <summary>
        /// Add a name that should be excluded
        /// </summary>
        public void SetExistingName(string name)
        {
            List<string> lNname = new List<string>(1);
            lNname.Add(name);
            _ExistingMethodNames.Add(lNname);
        }

        public void SetExistingSignatures(IList<string> signatures)
        {
            _ExistingMethodNames.Add(signatures);
        }
        #endregion

        #region Private Methods

        public bool DoesMethodSignatureExist(IList<string> DefinitiveSignature)
        {
            bool retval = false;
            List<string> CopyOfDefinitiveSignature = new List<string>(DefinitiveSignature);

            CopyOfDefinitiveSignature.Sort();

            foreach (List<string> predefinedSign in _ExistingMethodNames)
            {
                if (predefinedSign.Count != CopyOfDefinitiveSignature.Count)
                {
                    continue;
                }

                bool IsMatch = true;
                predefinedSign.Sort();

                for (int x = 0; x < predefinedSign.Count; x++)
                {
                    if (!  CopyOfDefinitiveSignature[x].Equals( predefinedSign[x], StringComparison.Ordinal) )
                    {
                        IsMatch = false;
                        break;
                    }
                }

                if (IsMatch)
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
