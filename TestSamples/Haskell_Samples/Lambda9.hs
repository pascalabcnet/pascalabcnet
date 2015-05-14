module Main where

h 1 = ((\x y -> x+y) 3 4) + ((\x -> x*x)3)
h z = ((\x y -> x+y+z)3 4) + ((\x -> x*x)3)

f = \x y -> x+y+(let x=2*d where d=1+(let y=8 in y*y*y+((\x y -> x+y)3 4))+(let s=3;v=2*t where t=3 in s*s) in x*x)+
                (let x=2*d where d=1+(let y=8 in y*y*y)+(let s=3;v=2*t where t=3 in s*s) in x*x)

main = do {
               (print(f 1 2));
               (print(h 5));
          }