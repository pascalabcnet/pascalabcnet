module Main where

f n = let x=1; y=2+x+n; g x = x*x in x+y+(g 3)+n

ff 1 = let x=1; y=2+x; g x = x*x in x+y+(g 3)
ff n = (let x=1; y=2+x+n; g x = x*x in x+y+(g 3)+n)+let x=1; y=2+x+n; g x = x*x in x+y+(g 3)+n

main = do {
            (print(f 2));
            (print(ff 1));
            (print(ff 5));
          }