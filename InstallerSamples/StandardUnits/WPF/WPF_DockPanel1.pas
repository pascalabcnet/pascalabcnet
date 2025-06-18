uses WPF;

begin
  var dpanel := Panels.DockPanel.AsMainContent;
  var b := Controls.Button('One');
  var b1 := Controls.Button('Two');
  var b2 := Controls.Button('Three');
  var b3 := Controls.Button('Four');
  var b4 := Controls.Button('Five');
  dpanel.Children.Add(b);
  dpanel.Children.Add(b1);
  dpanel.Children.Add(b2);
  dpanel.Children.Add(b3);
  dpanel.Children.Add(b4);
  DockPanel.SetDock(b,Dock.Left);
  DockPanel.SetDock(b1,Dock.Right);
  DockPanel.SetDock(b2,Dock.Top);
  DockPanel.SetDock(b3,Dock.Bottom);
end.