module Main where

f n m = let x=1;g t = (let z=5;g t s = (let z=5 in z*t*s) in (g z t)*t);z=1 in (g 3)+n+x
+(let x=1;g t = (let z=5;g t = (let z=5 in z*t) in z*t);z=1 in (g 3)+n+x)

main = do {
            (print(f 5 8));
          }