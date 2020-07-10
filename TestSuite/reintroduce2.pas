var s: string;

type
  T1 = abstract class
    procedure Invoke; abstract;
  end;
  
  T2 = class(T1)
    procedure Invoke; override;
    begin
      s := 'true';
    end;
  end;
  
  T3 = abstract class(T2)
    procedure Invoke; reintroduce; abstract;
  end;
  
  T4 = class(T3)
    procedure Invoke; override;
    begin
      s := 'false';
    end;
  end;

begin
  var x4 := new T4();
  (x4 as T1).Invoke;
  assert(s = 'true');
end.