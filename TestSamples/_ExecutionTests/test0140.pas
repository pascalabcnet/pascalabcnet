type TRec = record
s : string[3];
end;

type mystr = string[3];

type TRec2 = record
s : string[3];
s2 : set of 1..3;
s3 : set of mystr;
end;

const str : string[3] = 'abcd';
      arr_str : array of string[3] = ('abcd','abc','de');
      sz_arr_str : array[1..3] of string[3] = ('abcd','abc','de');
      rec : TRec = (s:'abcd');
      _rec : TRec2 = (s:'abcd';s2:[1,2,6];s3:['abcd','kl']);
      rec_arr : array of TRec = ((s:'abcd'),(s:'abcd'));
      sz_rec_arr : array[0..1] of TRec = ((s:'abcd'),(s:'abcd'));
      
var str2 : string[3] := 'abcd';
    arr_str2 : array of string[3] := ('abcd','abc','de');
    sz_arr_str2 : array[1..3] of string[3] := ('abcd','abc','de');
    rec2 : TRec := (s:'abcd');
    _rec2 : TRec2 := (s:'abcd';s2:[1,2,6];s3:['abcd','kl']);
    rec_arr2 : array of TRec := ((s:'abcd'),(s:'abcd'));
    sz_rec_arr2 : array[0..1] of TRec := ((s:'abcd'),(s:'abcd'));
    var1 : array of TRec := rec_arr;
    var2 : TRec2 := _rec;
    
procedure Test;
const str : string[3] = 'abcd';
var str2 : string[3] := 'abcd';
begin
var str3 : string[3] := 'abcd';
assert(str='abc');
assert(str2='abc');
assert(str3='abc');
end;

procedure Test2;
const str : string[3] = 'abcd';
var str2 : string[3] := 'abcd';
procedure Test;
begin
var str3 : string[3] := 'abcd';
assert(str='abc');
assert(str2='abc');
assert(str3='abc');
end;
begin
Test;
end;

begin
var str3 : string[3] := 'abcd';
var arr_str3 : array of string[3] := ('abcd','abc','de');
var sz_arr_str3 : array[1..3] of string[3] := ('abcd','abc','de');
var rec3 : TRec := (s:'abcd');
var _rec3 : TRec2 := (s:'abcd';s2:[1,2,6];s3:['abcd','kl']);
var rec_arr3 : array of TRec := ((s:'abcd'),(s:'abcd'));
var sz_rec_arr3 : array[0..1] of TRec := ((s:'abcd'),(s:'abcd'));
assert(rec.s='abc');
assert(rec2.s = 'abc');
assert(rec3.s = 'abc');
assert(_rec.s='abc');
assert(_rec2.s = 'abc');
assert(_rec3.s = 'abc');
assert(_rec.s2=[1,2]);
assert(_rec2.s2=[1,2]);
assert(_rec3.s2=[1,2]);
assert(_rec.s3=['abc','kl']);
assert(_rec2.s3=['abc','kl']);
assert(_rec3.s3=['abc','kl']);
assert(rec_arr[0].s='abc');
assert(rec_arr2[0].s='abc');
assert(rec_arr3[0].s='abc');
assert(var1[0].s='abc');
assert(sz_rec_arr[0].s='abc');
assert(sz_rec_arr2[0].s='abc');
assert(sz_rec_arr3[0].s='abc');
assert(str='abc');
assert(str2='abc');
assert(str3='abc');
assert(arr_str[0]='abc');
assert(arr_str2[0]='abc');
assert(arr_str3[0]='abc');
assert(sz_arr_str[1]='abc');
assert(sz_arr_str2[1]='abc');
assert(sz_arr_str3[1]='abc');
assert(var2=_rec);
Test;
Test2;
end.