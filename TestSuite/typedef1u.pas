unit typedef1u;

type myint = integer;
     myptrint = ^integer;
     myptrintptr = ^myptrint;
     myarr = array[1..5] of myint;
     mydynarr = array of myarr;
     mydynarr2 = array of myint;
     myset = set of char;
     myset2 = set of 1..4;
     myenum = (one, two, three, four);
     myset3 = set of myenum;
     myset4 = set of one..three;
     mydiap = 1..6;
     mydiap2 = two..four;
     myptrdiap = ^mydiap;
     myptrdiap2 = ^mydiap2;
     myptrenum = ^myenum;
     myrec = record a : integer; end;
     myptrrec = ^myrec;
     myptrrec2 = ^myrec2;
     myrec2 = record a : integer; ptr : myptrrec2; end;
     mystr = string[20];
end.