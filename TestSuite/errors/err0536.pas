//!Невозможно преобразовать функциональный тип в процедурный тип
uses System.Threading.Tasks;

type
  c1<T> = class
    field1: Task<T>;
    
    procedure pr1(act: System.Action);
    begin
      Task.CompletedTask.ContinueWith(r-> act.Invoke);
    end;
    
    // если закомментировать этот кусок, то перестанет компилироваться pr1 о_О
    procedure pr2<T1>(fun: Func<T, T1>);
    begin
      field1.ContinueWith( e-> fun.Invoke(e.result) );
    end;
  end;
  
begin end.