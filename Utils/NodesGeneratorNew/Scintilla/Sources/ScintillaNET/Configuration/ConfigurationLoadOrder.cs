#region Using Directives

using System;

#endregion Using Directives


namespace ScintillaNET.Configuration
{
    public enum ConfigurationLoadOrder
    {
        BuiltInCustomUser,
        BuiltInUserCustom,
        CustomBuiltInUser,
        CustomUserBuiltIn,
        UserBuiltInCustom,
        UserCustomBuiltIn
    }
}
