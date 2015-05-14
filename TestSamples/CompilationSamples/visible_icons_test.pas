type
  cc=class
  public
    public_del:procedure;
    public_var:real;    
    function public_method:real; begin end;
    property public_property:real read public_method;
  internal
    internal_del:procedure;
    internal_var:real;
    function internal_method:real; begin end;
    property internal_property:real read internal_method;
  protected
    protected_del:procedure;
    protected_var:real;
    function protected_method:real; begin end;
    property protected_property:real read protected_method;
  private
    privatel_del:procedure;
    private_var:real;
    function private_method:real; begin  end;
    property private_property:real read private_method;
  end;

var c:cc;

begin
  
end.