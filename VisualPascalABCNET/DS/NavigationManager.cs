// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using PascalABCCompiler;

namespace VisualPascalABC
{
    public class NavigationManager
    {
        Dictionary<int, SourceLocation> lines_to_locations = new Dictionary<int, SourceLocation>();
        List<SourceLocation> locations = new List<SourceLocation>();
        int navigatePosition = -1;
        VisualEnvironmentCompiler.ExecuteSourceLocationActionDelegate ExecuteSourceLocationAction;
        public NavigationManager(VisualEnvironmentCompiler.ExecuteSourceLocationActionDelegate ExecuteSourceLocationAction)
        {
            this.ExecuteSourceLocationAction = ExecuteSourceLocationAction;
        }
        bool changed_loaction = false;
        public void LocationChanged(int LineNum, int ColumnNum, string FileName)
        {
            SourceLocation sl = null;
            /*if (lines_to_locations.TryGetValue(LineNum, out sl))
                if (sl.file_name == file_name)
                {
                    sl.BeginPosition.Column = sl.EndPosition.Column = ColumnNum;
                    locations.Remove(sl);
                    locations.Add(sl);
                    return;
                }*/
            if (locations.Count > 0)
            {
                sl = locations[navigatePosition];
                if (sl.FileName == FileName && Math.Abs(sl.BeginPosition.Line-LineNum)<2)
                {
                    sl.BeginPosition.Column = sl.EndPosition.Column = ColumnNum;
                    sl.BeginPosition.Line = sl.EndPosition.Line = LineNum;
                    return;
                }
            }
            sl = new SourceLocation(FileName, LineNum, ColumnNum, LineNum, ColumnNum);
            if (locations.Count == 0)
            {
                locations.Add(sl);
                navigatePosition = 0;
            }
            else
            {
                locations.Insert(navigatePosition+1, sl);
                navigatePosition++;
            }
            stateChanged();
        }
        public delegate void NavigationManagerStateChanged(NavigationManager sender);
        public event NavigationManagerStateChanged StateChanged;
        public bool CanNavigateForward
        {
            get
            {
                return locations.Count>0 && navigatePosition < locations.Count - 1;
            }
        }
        public bool CanNavigateBackward
        {
            get
            {
                return navigatePosition > 0;
            }
        }
        public void NavigateForward()
        {
            if (CanNavigateForward)
            {
                navigatePosition++;
                ExecuteSourceLocationAction(locations[navigatePosition], VisualPascalABCPlugins.SourceLocationAction.NavigationGoto);
            }
            stateChanged();
        }
        public void NavigateBackward()
        {
            if (CanNavigateBackward)
            {
                navigatePosition--;
                ExecuteSourceLocationAction(locations[navigatePosition], VisualPascalABCPlugins.SourceLocationAction.NavigationGoto);
            }
            stateChanged();
        }
        void stateChanged()
        {
            if (StateChanged != null)
                StateChanged(this);
        }
    }
}
