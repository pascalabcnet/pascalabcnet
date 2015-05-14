unit test0074u;
type TRec = record
     a : integer;
     end;

type TArr = array[1..3] of TRec;
     TArr2 = array of TRec;
     TSet = set of TRec;
     TFile = file of TRec;
     
procedure Test(a : TArr);
begin

end;

procedure Test(a : TArr2);
begin

end;

procedure Test(a : TSet);
begin

end;

procedure Test(a : TFile);
begin

end;
end.