module Main where

f n = (let x=1; y=2+x+n; g x = x*x in x+y+(g 3)+n+(let x=1; y=2+x+n in x+y)+(let x=1; y=2+x+n in x+y+n))+
(let x=1; y=2+x+n; g x = x*x in x+y+(g 3)+n+(let x=1; y=2+x+n in x+y)+(let x=1; y=2+x+n in x+y+n))

main = do {
            (print(f 5));
          }