module Main where

g z = (let x=2;f x = x*x in x*x+z+q)
where q = 4+(let y=8 in y*y*y)+(let x=2*d where d=1+(let y=8 in y*y*y)+(let s=3;v=2*t where t=3 in s*s) in x*x)

main = do { 
              (print(g 1));
              (print(g 3));
              (print(let s=3;v=2*t where t=3 in s*v));
          }