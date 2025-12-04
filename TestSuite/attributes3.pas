uses
  System.ComponentModel;

type
  MyClassConverter = class (TypeConverter)
    
  end;
  
  MyClass = class
  private
    fMyField: MyClass;
  public
    [TypeConverterAttribute(typeof(MyClassConverter))]
    property MyProperty: MyClass read fMyField write fMyField;
  end;

begin
  var o := new MyClass;
  o.MyProperty := o;
  var props := o.GetType().GetProperties();
  var attrs := props[0].GetCustomAttributes(false);
  assert((attrs[0] as TypeConverterAttribute).ConverterTypeName.IndexOf('MyClassConverter') <> -1);
end.