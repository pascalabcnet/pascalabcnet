function i: integer := 2;
function b: byte := 2;
function sm: smallint := 2;
function w: word := 2;
function lw: longword := 2;
function i64: int64 := 2;
function ui64: uint64 := 2;
function r: real := 2;
function f: single := 2;
begin
  assert(i+i = 4);
  assert(i+b = 4);
  assert(i+sm = 4);
  assert(i+w = 4);
  assert(i+lw = 4);
  assert(sm+w = 4);
  assert(sm+b = 4);
  assert(w+b = 4);
  assert(i+i64 = 4);
  assert(w+lw = 4);
  assert(b+i64 = 4);
  assert(w+i64 = 4);
  assert(sm+lw = 4);
  assert(i64+ui64 = 4);
  
  assert(round(r+f) = 4);
  assert(round(r+i) = 4);
  assert(round(r+b) = 4);
  assert(round(r+w) = 4);
  assert(round(r+lw) = 4);
  assert(round(r+sm) = 4);
  assert(round(r+i64) = 4);
  assert(round(r+ui64) = 4);
  
  assert(round(f+r) = 4);
  assert(round(f+i) = 4);
  assert(round(f+b) = 4);
  assert(round(f+w) = 4);
  assert(round(f+lw) = 4);
  assert(round(f+sm) = 4);
  assert(round(f+i64) = 4);
  assert(round(f+ui64) = 4);
  
  assert(i*i = 4);
  assert(i*b = 4);
  assert(i*sm = 4);
  assert(i*w = 4);
  assert(i*lw = 4);
  assert(sm*w = 4);
  assert(sm*b = 4);
  assert(w*b = 4);
  assert(i*i64 = 4);
  assert(w*lw = 4);
  assert(b*i64 = 4);
  assert(w*i64 = 4);
  assert(sm*lw = 4);
  assert(i64*ui64 = 4);
  
  assert(round(r*f) = 4);
  assert(round(r*i) = 4);
  assert(round(r*b) = 4);
  assert(round(r*w) = 4);
  assert(round(r*lw) = 4);
  assert(round(r*sm) = 4);
  assert(round(r*i64) = 4);
  assert(round(r*ui64) = 4);
  
  assert(round(f*r) = 4);
  assert(round(f*i) = 4);
  assert(round(f*b) = 4);
  assert(round(f*w) = 4);
  assert(round(f*lw) = 4);
  assert(round(f*sm) = 4);
  assert(round(f*i64) = 4);
  assert(round(f*ui64) = 4);
  
  assert(i div i = 1);
  assert(i div b = 1);
  assert(i div sm = 1);
  assert(i div w = 1);
  assert(i div lw = 1);
  assert(sm div w = 1);
  assert(sm div b = 1);
  assert(w div b = 1);
  assert(i div i64 = 1);
  assert(w div lw = 1);
  assert(b div i64 = 1);
  assert(w div i64 = 1);
  assert(sm div lw = 1);
  assert(i64 div ui64 = 1);
  
  assert(round(i / i) = 1);
  assert(round(i / b) = 1);
  assert(round(i / sm) = 1);
  assert(round(i / w) = 1);
  assert(round(i / lw) = 1);
  assert(round(sm / w) = 1);
  assert(round(sm / b) = 1);
  assert(round(w / b) = 1);
  assert(round(i / i64) = 1);
  assert(round(w / lw) = 1);
  assert(round(b / i64) = 1);
  assert(round(w / i64) = 1);
  assert(round(sm / lw) = 1);
  assert(round(i64 / ui64) = 1);
  
  var a := r / f;
  assert(round(a) = 1);
  
  assert(round(r/f) = 1);
  assert(round(r/i) = 1);
  assert(round(r/b) = 1);
  assert(round(r/w) = 1);
  assert(round(r/lw) = 1);
  assert(round(r/sm) = 1);
  assert(round(r/i64) = 1);
  assert(round(r/ui64) = 1);
  
  assert(round(f/r) = 1);
  assert(round(f/i) = 1);
  assert(round(f/b) = 1);
  assert(round(f/w) = 1);
  assert(round(f/lw) = 1);
  assert(round(f/sm) = 1);
  assert(round(f/i64) = 1);
  assert(round(f/ui64) = 1);
  
  assert(round(f/r) = 1);
  assert(round(i/r) = 1);
  assert(round(b/r) = 1);
  assert(round(w/r) = 1);
  assert(round(lw/r) = 1);
  assert(round(sm/r) = 1);
  assert(round(i64/r) = 1);
  assert(round(ui64/r) = 1);
  
  assert(round(i/f) = 1);
  assert(round(b/f) = 1);
  assert(round(w/f) = 1);
  assert(round(lw/f) = 1);
  assert(round(sm/f) = 1);
  assert(round(i64/f) = 1);
  assert(round(ui64/f) = 1);
end.