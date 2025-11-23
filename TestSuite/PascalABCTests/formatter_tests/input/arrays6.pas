
procedure Test;
var b1 : set of byte := [-1..3];
    b2 : array[1..5] of integer := (1,2,3,4,5);
    b3 : array of real := (2.4,3.5,1.2);
    b4 : array[1..3] of set of char := (['a','j'],['l','b','c'],[]);
    
begin
  var a1 : set of byte := [-1..3];
  assert(a1=[0..3]);
  assert(2 in a1);
  var a2 : array[1..5] of integer := (1,2,3,4,5);
  assert(a2[3] = 3);
  var a3 : array of real := (2.4,3.5,1.2);
  assert(a3[1] = 3.5);
  var a4 : array[1..3,1..2] of integer := ((1,2),(2,3),(3,4));
  assert(a4[2,2] = 3);
  var a5 : array[0..2] of set of char := (['a','j'],['l','b','c'],[]);
  assert(a5[0] = ['a','j']);
  assert(a5[2]=[]);
  assert(b1 = [0..3]);
  assert(1 in b1);
  assert(b2[2] = 2);
  assert(b3[2] = 1.2);
  assert(b4[1] = ['a','j']);
  Include(b1,4); assert(b1=[0..4]);
  Exclude(b1,4); assert(b1=[0..3]);
  assert(2 in b1);
end;

procedure Test2;
var b1 : set of byte := [-1..3];
    b2 : array[1..5] of integer := (1,2,3,4,5);
    b3 : array of real := (2.4,3.5,1.2);
    b4 : array[1..3] of set of char := (['a','j'],['l','b','c'],[]);

procedure Nested;
begin
  var a1 : set of byte := [-1..3];
  assert(a1=[0..3]);
  assert(2 in a1);
  var a2 : array[1..5] of integer := (1,2,3,4,5);
  assert(a2[3] = 3);
  var a3 : array of real := (2.4,3.5,1.2);
  assert(a3[1] = 3.5);
  var a4 : array[1..3,1..2] of integer := ((1,2),(2,3),(3,4));
  assert(a4[2,2] = 3);
  var a5 : array[0..2] of set of char := (['a','j'],['l','b','c'],[]);
  assert(a5[0] = ['a','j']);
  assert(a5[2]=[]);
  assert(b1 = [0..3]);
  assert(1 in b1);
  assert(b2[2] = 2);
  assert(b3[2] = 1.2);
  assert(b4[1] = ['a','j']);
  Include(b1,4); assert(b1=[0..4]);
  Exclude(b1,4); assert(b1=[0..3]);
  assert(2 in b1);
end;
    
begin
  var a1 : set of byte := [-1..3];
  assert(a1=[0..3]);
  assert(2 in a1);
  var a2 : array[1..5] of integer := (1,2,3,4,5);
  assert(a2[3] = 3);
  var a3 : array of real := (2.4,3.5,1.2);
  assert(a3[1] = 3.5);
  var a4 : array[1..3,1..2] of integer := ((1,2),(2,3),(3,4));
  assert(a4[2,2] = 3);
  var a5 : array[0..2] of set of char := (['a','j'],['l','b','c'],[]);
  assert(a5[0] = ['a','j']);
  assert(a5[2]=[]);
  assert(b1 = [0..3]);
  assert(1 in b1);
  assert(b2[2] = 2);
  assert(b3[2] = 1.2);
  assert(b4[1] = ['a','j']);
  Include(b1,4); assert(b1=[0..4]);
  Exclude(b1,4); assert(b1=[0..3]);
  assert(2 in b1);
  Nested;
end;

var b1 : set of byte := [-1..3];
    b2 : array[1..5] of integer := (1,2,3,4,5);
    b3 : array of real := (2.4,3.5,1.2);
    b4 : array[1..3] of set of char := (['a','j'],['l','b','c'],[]);
    
begin
  var a5 : array[0..2] of set of char := (['a','j'],['l','b','c'],[]);
  assert(a5[0] = ['a','j']);
  assert(a5[2]=[]);
  var a1 : set of byte := [-1..3];
  assert(a1=[0..3]);
  assert(2 in a1);
  var a2 : array[1..5] of integer := (1,2,3,4,5);
  assert(a2[3] = 3);
  var a3 : array of real := (2.4,3.5,1.2);
  assert(a3[1] = 3.5);
  var a4 : array[1..3,1..2] of integer := ((1,2),(2,3),(3,4));
  assert(a4[2,2] = 3);
  
  assert(b1 = [0..3]);
  assert(1 in b1);
  assert(b2[2] = 2);
  assert(b3[2] = 1.2);
  assert(b4[1] = ['a','j']);
  Include(b1,4); assert(b1=[0..4]);
  Exclude(b1,4); assert(b1=[0..3]);
  assert(2 in b1);
  Test;
  Test2;
end.