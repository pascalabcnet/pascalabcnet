//!Невозможно преобразовать функциональный тип в процедурный тип
uses System.Threading.Tasks;

type
  c1<T> = class
    field1: Task<T>;
    
    procedure pr1(act: System.Action);
    begin
      Task.CompletedTask.ContinueWith(r-> act.Invoke);
    end;
    
  end;
  
begin end.