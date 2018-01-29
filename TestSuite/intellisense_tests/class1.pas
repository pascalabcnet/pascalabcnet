uses System.Text;

type TClass = class
end;

type
  TArr{@type TArr = array of integer@} = array of integer;
  TSet{@type TSet = set of byte@} = set of byte;
  TArr2{@type TArr2 = array[,] of TArr@} = array[,] of TArr;
  TArr3{@type TArr3 = array[1..10] of string@} = array[1..10] of string;
  TStr{@type TStr = string[5]@} = string[5];
  
begin
  var t: TClass{@class TClass@};
  var t1: TArr{@type TArr = array of integer@};
  var t2: TSet{@type TSet = set of byte@};
  var t3: TArr2{@type TArr2 = array[,] of TArr@};
  var t4: TArr3{@type TArr3 = array[1..10] of string@};
  var t5: TStr{@type TStr = string[5]@};
  var sb: StringBuilder{@sealed class System.Text.StringBuilder@};
end.