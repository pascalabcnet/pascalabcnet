uses vcl2;

var MainForm:Form;
    Label1:TextLabel;
    Edit1:Edit;
    Memo1: TextBox;
    
procedure SetFormCaption;
begin
  MainForm.Caption:=Edit1.Text;
end;

begin
  MainForm:=Form.Create(200,200,500,500);
  Label1:=TextLabel.Create(5,20,'Hello');
  Edit1:=Edit.Create(140,20);
  Edit1.OnChange:=SetFormCaption;
  Memo1:=new TextBox(5,200,200,100,'Super!!!');
end.
