uses WPF;

begin
  var dpanel := DockPanel.Create.AsMainContent;
  var b := Controls.Button('One',Width := 100).AddTo(dpanel,Dock.Left);
  var b1 := Controls.Button('Two',Width := 100).AddTo(dpanel,Dock.Right);
  var b2 := Controls.Button('Three',Height := 50).AddTo(dpanel,Dock.Top);
  var b3 := Controls.Button('Four',Height := 50).AddTo(dpanel,Dock.Bottom);
  var b4 := Controls.Button('Five').AddTo(dpanel);
  b.Click += (o,e) -> begin
    MainWindow.Close
  end;
end.