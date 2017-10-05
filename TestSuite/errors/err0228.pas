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
    protected property VisualChildrenCount2: integer read getVisualChildrenCount; override;
  end;
begin
var obj: MyVisualHostBase := new MyVisualHost;
assert(obj.getCnt=1);
end.