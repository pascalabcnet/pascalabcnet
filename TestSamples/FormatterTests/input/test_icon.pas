#resource 'test_icon.ico'
#resource 'test_icon1.ico'

uses VCL;

var myForm: Form;
 
begin
  myForm := new Form;
  myForm.Icon := new Icon(GetResourceStream('test_icon1.ico'));
  Application.Run(myForm);
end.