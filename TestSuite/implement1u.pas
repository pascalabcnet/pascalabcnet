unit implement1u;

type 
  Digits = (one, two, three, four, five);
  TDiap = 1..5;
  TFunc = function(d : Digits): TDiap;
  TArr = array[TDiap,'b'..'e',one..three,false..true] of set of 'a'..'k';
  TRec2 = record
    a1 : byte;
    a2 : integer;
    a3 : char;
    a4 : real;
    a5 : string[4];
    a6 : string;
    a7 : 1..4;
    a8 : Digits;
    a9 : two..five;
    a10 : array[one..three] of 1..6;
    a11 : array[false..true,'a'..'d'] of string[3];
    a12 : set of one..three;
    a13 : set of char;
    a14 : file of string[3];
    a16 : TFunc;
    a17 : array of Digits;
    a18 : array[TDiap] of file of real;
    a19 : TArr;
  end;

type TRec = record
a1 : byte;
a2 : integer;
a3 : char;
a4 : real;
a5 : string[4];
a6 : string;
a7 : 1..4;
a8 : Digits;
a9 : two..five;
a10 : array[one..three] of 1..6;
a11 : array[false..true,'a'..'d'] of string[3];
a12 : set of one..three;
a13 : set of char;
a14 : file of string[3];
a15 : TRec2;
a16 : TFunc;
a17 : array of Digits;
a18 : array[TDiap] of file of real;
a19 : array[TDiap,'b'..'e',one..three,false..true] of set of 'a'..'k';
a20 : array of TFunc;
a21 : ^real;   
end;

TRec3 = record
a1 : byte := 3;
a2 : integer := 5;
a3 : char := 'a';
a4 : real := a1+a2;
a5 : string[4] := 'abcde';
a6 : string := a5+'ef';
a7 : 1..4;
a8 : Digits := three;
a9 : two..five;
a10 : array[one..three] of 1..6 := (2,1,5);
a11 : array[false..true,'a'..'b'] of string[3]:=(('ab','bc'),('cd','de'));
a12 : set of one..three:=[two,three];
a13 : set of char:=[a3,'k',a5[2]];
a14 : file of string[3];
a15 : TRec2;
a16 : TFunc;
a17 : array of Digits:=(two,two,four,one);
a18 : array[TDiap] of file of real;
a19 : array[TDiap,'b'..'e',one..three,false..true] of set of 'a'..'k';
a20 : array of TFunc;
a21 : ^real;   
end;

begin

end.