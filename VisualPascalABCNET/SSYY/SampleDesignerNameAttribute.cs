//------------------------------------------------------------------------------
/// <copyright from='1997' to='2002' company='Microsoft Corporation'>
///    Copyright (c) Microsoft Corporation. All Rights Reserved.
///
///    This source code is intended only as a supplement to Microsoft
///    Development Tools and/or on-line documentation.  See these other
///    materials for detailed information regarding Microsoft code samples.
///
/// </copyright>
//------------------------------------------------------------------------------
namespace SampleDesignerHost
{
    using System;

	/// This attribute is used to mark "Name" properties that have been provided
	/// by our IExtenderProvider in order to differentiate them from other Name properties
	/// which we are not interested in. It's just a bool set to true if it's what we want.
	/// The attribute target is a Method, since the IExtenderProvider uses methods to specify
	/// the get/set for our "Name" property.
    [AttributeUsage(AttributeTargets.Method, AllowMultiple=false, Inherited=true)]
    internal sealed class SampleDesignerNameAttribute : Attribute {
        private bool designerName;

        public static SampleDesignerNameAttribute Default = new SampleDesignerNameAttribute(false);

        public SampleDesignerNameAttribute() : this(false){
        }

        public SampleDesignerNameAttribute(bool designerName) {
            this.designerName = designerName;
        }

        public override bool Equals(object obj) {
            SampleDesignerNameAttribute da = obj as SampleDesignerNameAttribute;

            if (da == null) {
                return false;
            }
            return da.designerName == this.designerName;
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }
    }

}


