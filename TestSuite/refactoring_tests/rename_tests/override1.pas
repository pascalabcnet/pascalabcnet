type
  T0 = class
    procedure {@}P(); virtual := exit;
  end;
  
  T1 = class(T0)
    procedure {!}P(); override := exit;
  end;
  
  T2 = class(T0)
    procedure {!}P(); override := exit;
  end;
  
  T3 = class(T0)
    procedure {!}P(); override := exit;
  end;

begin
end.