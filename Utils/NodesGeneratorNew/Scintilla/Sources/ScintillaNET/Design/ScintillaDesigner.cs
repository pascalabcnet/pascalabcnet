#region Using Directives

using System;
using System.Collections;
using System.Windows.Forms.Design;
using System.ComponentModel;

#endregion Using Directives


namespace ScintillaNET.Design
{
    // Provides additional design-time support for the Scintilla control
    internal sealed class ScintillaDesigner : ControlDesigner
    {
        #region Methods

        public override void InitializeNewComponent(IDictionary defaultValues)
        {
            base.InitializeNewComponent(defaultValues);

            // By default, the VS control designer will set a control's Text property to the
            // name of the control. This the accepted method of clearing the Text property
            // when the control is dropped on the form.
            PropertyDescriptor descriptor = TypeDescriptor.GetProperties(base.Component)["Text"];
            if (descriptor != null && descriptor.PropertyType == typeof(String) && !descriptor.IsReadOnly && descriptor.IsBrowsable)
                descriptor.SetValue(base.Component, String.Empty);
        }

        #endregion Methods
    }
}
