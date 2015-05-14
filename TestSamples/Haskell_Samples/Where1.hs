module Main where

g z = 2+x+(let x=1; y=2+x; f z = z*t where t=5;s=5 in x+(q y)+(f 4))
where x=555; q z = z*z where t=4;s=4
g 2 = 2+x+let x=1; y=2+x; f z = z*t where t=4;s=4 in x+(q y)+(f 3)
where x=555; q z = z*t where t=4;s=4

w 1 = 2+x+let x=1; y=2+x; f z = z*t where t=5;s=5 in x+(q y)+(f 4)
where x=555; q z = z*t where t=4;s=4+((\z->z*z*t*x)5)
w 2 = 2+x+let x=1; y=2+x; f z = z*t where t=4;s=4 in x+(q y)+(f 3)
where x=555; q z = z*t where t=4;s=4

main = do { 
              (print(w 1));
              (print(g 2));
          }