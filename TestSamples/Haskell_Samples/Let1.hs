module Main where

f n = let x=2; y=2+x+n+((\z->z*x)5)+((\z->z*x)5); g x = x*x in x+y+(g 3)+n+((\x->x*y)9)+((\x->x*y)7)

ff 1 = let x=1; y=2+x; g x = x*x in x+y+(g 3)
ff n = (let x=1; y=2+x+n; g x = x*x in x+y+(g 3)+n)+let x=1; y=2+x+n; g x = x*x in x+y+(g 3)+n

main = do {
            (print(f 2));
            (print(ff 1));
            (print(ff 5));
            (print(let x=2; y=2+x+((\z->z*x)5)+((\z->z*x)5); g x = x*x in x+y+(g 3)+((\x->x*y)9)+((\x->x*y)7)));
          }