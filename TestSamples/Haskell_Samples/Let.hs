module Main where

f n = (let x=1; y=2+x+n; g x = x*x in x+(g y)+n+ (let z=3 in z+2+n+x)) + (let x=1; y=2+x+n in x+y+n+ (let z=3 in z+2+n+x))

g n = let z=1;g x = (let k=3 in k*k)+(let k=3 in k*k) in (g 2)+(let z=5 in z*z)+z

main = do {
            (print(f 5));
            (print(g 5));
            (print(((\a b g -> a*(g 3)*b) 5 2 \x -> x*x)));
          }