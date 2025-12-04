procedure Test;
var 
  b : byte;
  sh : shortint;
  sm : smallint;
  w : word;
  i : integer;
  lw : longword;
  li : int64;
  ui: uint64;
  
begin
  assert(b and b = 0);
  assert(b and sh = 0);
  assert(b and sm = 0);
  assert(b and w = 0);
  assert(b and i = 0);
  assert(b and lw = 0);
  assert(b and li = 0);
  assert(b and ui = 0);
  
  assert(sh and sh = 0);
  assert(sh and sm = 0);
  assert(sh and w = 0);
  assert(sh and i = 0);
  assert(sh and lw = 0);
  assert(sh and li = 0);
  assert(sh and ui = 0);
  
  assert(sm and sm = 0);
  assert(sm and w = 0);
  assert(sm and i = 0);
  assert(sm and lw = 0);
  assert(sm and li = 0);
  assert(sm and ui = 0);
  
  assert(w and w = 0);
  assert(w and i = 0);
  assert(w and lw = 0);
  assert(w and li = 0);
  assert(w and ui = 0);

  assert(i and i = 0);
  assert(i and lw = 0);
  assert(i and li = 0);
  assert(i and ui = 0);
  
  assert(lw and lw = 0);
  assert(lw and li = 0);
  assert(lw and ui = 0);
  
  assert(li and li = 0);
  assert(li and ui = 0);

  assert(ui and ui = 0);
  
  
  assert(b or b = 0);
  assert(b or sh = 0);
  assert(b or sm = 0);
  assert(b or w = 0);
  assert(b or i = 0);
  assert(b or lw = 0);
  assert(b or li = 0);
  assert(b or ui = 0);
  
  assert(sh or sh = 0);
  assert(sh or sm = 0);
  assert(sh or w = 0);
  assert(sh or i = 0);
  assert(sh or lw = 0);
  assert(sh or li = 0);
  assert(sh or ui = 0);
  
  assert(sm or sm = 0);
  assert(sm or w = 0);
  assert(sm or i = 0);
  assert(sm or lw = 0);
  assert(sm or li = 0);
  assert(sm or ui = 0);
  
  assert(w or w = 0);
  assert(w or i = 0);
  assert(w or lw = 0);
  assert(w or li = 0);
  assert(w or ui = 0);

  assert(i or i = 0);
  assert(i or lw = 0);
  assert(i or li = 0);
  assert(i or ui = 0);
  
  assert(lw or lw = 0);
  assert(lw or li = 0);
  assert(lw or ui = 0);
  
  assert(li or li = 0);
  assert(li or ui = 0);

  assert(ui or ui = 0);
  
  assert(b xor b = 0);
  assert(b xor sh = 0);
  assert(b xor sm = 0);
  assert(b xor w = 0);
  assert(b xor i = 0);
  assert(b xor lw = 0);
  assert(b xor li = 0);
  assert(b xor ui = 0);
  
  assert(sh xor sh = 0);
  assert(sh xor sm = 0);
  assert(sh xor w = 0);
  assert(sh xor i = 0);
  assert(sh xor lw = 0);
  assert(sh xor li = 0);
  assert(sh xor ui = 0);
  
  assert(sm xor sm = 0);
  assert(sm xor w = 0);
  assert(sm xor i = 0);
  assert(sm xor lw = 0);
  assert(sm xor li = 0);
  assert(sm xor ui = 0);
  
  assert(w xor w = 0);
  assert(w xor i = 0);
  assert(w xor lw = 0);
  assert(w xor li = 0);
  assert(w xor ui = 0);

  assert(i xor i = 0);
  assert(i xor lw = 0);
  assert(i xor li = 0);
  assert(i xor ui = 0);
  
  assert(lw xor lw = 0);
  assert(lw xor li = 0);
  assert(lw xor ui = 0);
  
  assert(li xor li = 0);
  assert(li xor ui = 0);

  assert(ui xor ui = 0);
  
  assert(b shl b = 0);
  assert(b shl sh = 0);
  assert(b shl sm = 0);
  assert(b shl w = 0);
  assert(b shl i = 0);
  assert(b shl lw = 0);
  assert(b shl li = 0);
  assert(b shl ui = 0);
  
  assert(sh shl sh = 0);
  assert(sh shl sm = 0);
  assert(sh shl w = 0);
  assert(sh shl i = 0);
  assert(sh shl lw = 0);
  assert(sh shl li = 0);
  assert(sh shl ui = 0);
  
  assert(sm shl sm = 0);
  assert(sm shl w = 0);
  assert(sm shl i = 0);
  assert(sm shl lw = 0);
  assert(sm shl li = 0);
  assert(sm shl ui = 0);
  
  assert(w shl w = 0);
  assert(w shl i = 0);
  assert(w shl lw = 0);
  assert(w shl li = 0);
  assert(w shl ui = 0);

  assert(i shl i = 0);
  assert(i shl lw = 0);
  assert(i shl li = 0);
  assert(i shl ui = 0);
  
  assert(lw shl lw = 0);
  assert(lw shl li = 0);
  assert(lw shl ui = 0);
  
  assert(li shl li = 0);
  assert(li shl ui = 0);

  assert(ui shl ui = 0);
  
  assert(b shr b = 0);
  assert(b shr sh = 0);
  assert(b shr sm = 0);
  assert(b shr w = 0);
  assert(b shr i = 0);
  assert(b shr lw = 0);
  assert(b shr li = 0);
  assert(b shr ui = 0);
  
  assert(sh shr sh = 0);
  assert(sh shr sm = 0);
  assert(sh shr w = 0);
  assert(sh shr i = 0);
  assert(sh shr lw = 0);
  assert(sh shr li = 0);
  assert(sh shr ui = 0);
  
  assert(sm shr sm = 0);
  assert(sm shr w = 0);
  assert(sm shr i = 0);
  assert(sm shr lw = 0);
  assert(sm shr li = 0);
  assert(sm shr ui = 0);
  
  assert(w shr w = 0);
  assert(w shr i = 0);
  assert(w shr lw = 0);
  assert(w shr li = 0);
  assert(w shr ui = 0);

  assert(i shr i = 0);
  assert(i shr lw = 0);
  assert(i shr li = 0);
  assert(i shr ui = 0);
  
  assert(lw shr lw = 0);
  assert(lw shr li = 0);
  assert(lw shr ui = 0);
  
  assert(li shr li = 0);
  assert(li shr ui = 0);

  assert(ui shr ui = 0);
end;

var 
  b : byte;
  sh : shortint;
  sm : smallint;
  w : word;
  i : integer;
  lw : longword;
  li : int64;
  ui: uint64;
  
begin
  assert(b and b = 0);
  assert(b and sh = 0);
  assert(b and sm = 0);
  assert(b and w = 0);
  assert(b and i = 0);
  assert(b and lw = 0);
  assert(b and li = 0);
  assert(b and ui = 0);
  
  assert(sh and sh = 0);
  assert(sh and sm = 0);
  assert(sh and w = 0);
  assert(sh and i = 0);
  assert(sh and lw = 0);
  assert(sh and li = 0);
  assert(sh and ui = 0);
  
  assert(sm and sm = 0);
  assert(sm and w = 0);
  assert(sm and i = 0);
  assert(sm and lw = 0);
  assert(sm and li = 0);
  assert(sm and ui = 0);
  
  assert(w and w = 0);
  assert(w and i = 0);
  assert(w and lw = 0);
  assert(w and li = 0);
  assert(w and ui = 0);

  assert(i and i = 0);
  assert(i and lw = 0);
  assert(i and li = 0);
  assert(i and ui = 0);
  
  assert(lw and lw = 0);
  assert(lw and li = 0);
  assert(lw and ui = 0);
  
  assert(li and li = 0);
  assert(li and ui = 0);

  assert(ui and ui = 0);
  
  
  assert(b or b = 0);
  assert(b or sh = 0);
  assert(b or sm = 0);
  assert(b or w = 0);
  assert(b or i = 0);
  assert(b or lw = 0);
  assert(b or li = 0);
  assert(b or ui = 0);
  
  assert(sh or sh = 0);
  assert(sh or sm = 0);
  assert(sh or w = 0);
  assert(sh or i = 0);
  assert(sh or lw = 0);
  assert(sh or li = 0);
  assert(sh or ui = 0);
  
  assert(sm or sm = 0);
  assert(sm or w = 0);
  assert(sm or i = 0);
  assert(sm or lw = 0);
  assert(sm or li = 0);
  assert(sm or ui = 0);
  
  assert(w or w = 0);
  assert(w or i = 0);
  assert(w or lw = 0);
  assert(w or li = 0);
  assert(w or ui = 0);

  assert(i or i = 0);
  assert(i or lw = 0);
  assert(i or li = 0);
  assert(i or ui = 0);
  
  assert(lw or lw = 0);
  assert(lw or li = 0);
  assert(lw or ui = 0);
  
  assert(li or li = 0);
  assert(li or ui = 0);

  assert(ui or ui = 0);
  
  assert(b xor b = 0);
  assert(b xor sh = 0);
  assert(b xor sm = 0);
  assert(b xor w = 0);
  assert(b xor i = 0);
  assert(b xor lw = 0);
  assert(b xor li = 0);
  assert(b xor ui = 0);
  
  assert(sh xor sh = 0);
  assert(sh xor sm = 0);
  assert(sh xor w = 0);
  assert(sh xor i = 0);
  assert(sh xor lw = 0);
  assert(sh xor li = 0);
  assert(sh xor ui = 0);
  
  assert(sm xor sm = 0);
  assert(sm xor w = 0);
  assert(sm xor i = 0);
  assert(sm xor lw = 0);
  assert(sm xor li = 0);
  assert(sm xor ui = 0);
  
  assert(w xor w = 0);
  assert(w xor i = 0);
  assert(w xor lw = 0);
  assert(w xor li = 0);
  assert(w xor ui = 0);

  assert(i xor i = 0);
  assert(i xor lw = 0);
  assert(i xor li = 0);
  assert(i xor ui = 0);
  
  assert(lw xor lw = 0);
  assert(lw xor li = 0);
  assert(lw xor ui = 0);
  
  assert(li xor li = 0);
  assert(li xor ui = 0);

  assert(ui xor ui = 0);
  
  assert(b shl b = 0);
  assert(b shl sh = 0);
  assert(b shl sm = 0);
  assert(b shl w = 0);
  assert(b shl i = 0);
  assert(b shl lw = 0);
  assert(b shl li = 0);
  assert(b shl ui = 0);
  
  assert(sh shl sh = 0);
  assert(sh shl sm = 0);
  assert(sh shl w = 0);
  assert(sh shl i = 0);
  assert(sh shl lw = 0);
  assert(sh shl li = 0);
  assert(sh shl ui = 0);
  
  assert(sm shl sm = 0);
  assert(sm shl w = 0);
  assert(sm shl i = 0);
  assert(sm shl lw = 0);
  assert(sm shl li = 0);
  assert(sm shl ui = 0);
  
  assert(w shl w = 0);
  assert(w shl i = 0);
  assert(w shl lw = 0);
  assert(w shl li = 0);
  assert(w shl ui = 0);

  assert(i shl i = 0);
  assert(i shl lw = 0);
  assert(i shl li = 0);
  assert(i shl ui = 0);
  
  assert(lw shl lw = 0);
  assert(lw shl li = 0);
  assert(lw shl ui = 0);
  
  assert(li shl li = 0);
  assert(li shl ui = 0);

  assert(ui shl ui = 0);
  
  assert(b shr b = 0);
  assert(b shr sh = 0);
  assert(b shr sm = 0);
  assert(b shr w = 0);
  assert(b shr i = 0);
  assert(b shr lw = 0);
  assert(b shr li = 0);
  assert(b shr ui = 0);
  
  assert(sh shr sh = 0);
  assert(sh shr sm = 0);
  assert(sh shr w = 0);
  assert(sh shr i = 0);
  assert(sh shr lw = 0);
  assert(sh shr li = 0);
  assert(sh shr ui = 0);
  
  assert(sm shr sm = 0);
  assert(sm shr w = 0);
  assert(sm shr i = 0);
  assert(sm shr lw = 0);
  assert(sm shr li = 0);
  assert(sm shr ui = 0);
  
  assert(w shr w = 0);
  assert(w shr i = 0);
  assert(w shr lw = 0);
  assert(w shr li = 0);
  assert(w shr ui = 0);

  assert(i shr i = 0);
  assert(i shr lw = 0);
  assert(i shr li = 0);
  assert(i shr ui = 0);
  
  assert(lw shr lw = 0);
  assert(lw shr li = 0);
  assert(lw shr ui = 0);
  
  assert(li shr li = 0);
  assert(li shr ui = 0);

  assert(ui shr ui = 0);
  Test;
end.