//winonly
{$reference 'PresentationFramework.dll'}
{$reference 'WindowsBase.dll'}
{$reference 'PresentationCore.dll'}
{$reference 'WindowsFormsIntegration.dll'}

{$apptype windows}
uses System.Windows; 
uses System.Windows.Media; 

type 
  MyVisualHostBase = class(FrameworkElement)
    public function getCnt: integer;
    begin
      Result := VisualChildrenCount;
    end;
  end;
  MyVisualHost = class(MyVisualHostBase)
    
    function getVisualChildrenCount: integer;
    begin
      Result := 1;
    end;
    protected property VisualChildrenCount: integer read getVisualChildrenCount; override;
  end;
  
  TBaseClass = class
  function getA: integer;
  begin
    Result := 1;
  end;
  public property A: integer read getA; virtual;
  end;
  
  TDerClass = class(TBaseClass)
  function getA: integer;
  begin
    Result := 2;
  end;
  public property A: integer read getA; override;
  end;
  
begin
var obj: MyVisualHostBase := new MyVisualHost;
assert(obj.getCnt=1);
var obj2: TBaseClass := new TDerClass;
assert(obj2.A = 2);
end.