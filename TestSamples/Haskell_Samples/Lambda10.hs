module Main where

f = \x y -> x+y+(let z=5 in z*z+x)+(let x=2*d where d=1 in x*x)--+
                --(let x=2*d where d=1+(let y=8 in y*y*y)+(let s=3;v=2*t where t=3 in s*s) in x*x)

main = do {
               (print(f 1 2));
          }