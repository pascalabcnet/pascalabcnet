type
  T = class
    fX: byte;
    
    property A0: byte write; abstract;
    property B0: byte read; abstract;
    property C0: byte read write; abstract;
    
    property A1: byte write fX; virtual;
    property B1: byte read fX; virtual;
    property C1: byte read fX write fX; virtual;
    
    property A3: byte write begin end; virtual;
    property B3: byte read 0; virtual;
    property C3: byte read 0 write begin end; virtual;
  end;

begin
end.