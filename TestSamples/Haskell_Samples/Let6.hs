module Main where

f n = let x=1; y=2+x+n; g x = x*x in x+y+(g 3)+n

main = do {
            (print((f 2)+((\x->x*x+((\z->z*z)5)+((\z->z*z)5))5)+((\x->x*x)5)+let x=1+((\z->z*z)5)+((\z->z*z)5); y=2 in x+y+((\x->x*x)5)+((\x->x*x)5)));
          }