// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Text;

namespace VisualPascalABC.OptionsContent
{
    public enum OptionsContentAction {Ok,Cancel,Show,Hide};
    public interface IOptionsContent
    {
        string ContentName
        {
            get;
        }
        string Description
        {
            get;
        }
        System.Windows.Forms.UserControl Content
        {
            get;
        }
        void Action(OptionsContentAction action);
    }
}
