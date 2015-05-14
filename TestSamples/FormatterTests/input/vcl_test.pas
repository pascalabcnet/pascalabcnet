uses VCL2;

var frm : Form;
    btn : Button;
    edt : Edit;
    
procedure FormClickExt(sender: Component);
begin
writeln(sender.GetType());
end;

procedure FormMouseDown(x,y,button : integer);
begin
writeln(x,' ',y,' ',button);
end;

procedure FormKeyDownExt(sender: Component; key: integer; shift : ShiftState);
begin
 writeln(key);
 writeln(ssAlt in shift);
end;

procedure FormClosed;
begin
	writeln('Closed');
end;

procedure SetFormCaption;
begin
  frm.Caption := edt.Text;
end;

begin
frm := new Form(10,10,'Privet!!');
frm.Cursor := crArrow;
frm.Width := 400;
frm.Height := 750;
frm.Position := poScreenCenter;
frm.BorderIcons := [biMinimize,biHelp];
frm.OnClickExt := FormClickExt;
frm.OnMouseDown := FormMouseDown;
frm.OnKeyDownExt := FormKeyDownExt;
frm.OnClose := FormClosed;

btn := new Button(200,200,'Test');
btn.OnClickExt := FormClickExt;

edt := new Edit(80,80,70,10);
edt.OnChange := SetFormCaption;
frm.ShowModal();

end.