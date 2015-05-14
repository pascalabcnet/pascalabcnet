module Main where

f 1 h1:t1 = 3
f 2 h2:t2 = 2+x+(g 1 [1,2,3])+(gg [4,5,6])
where x=555; q h:z = h; g j h:t:t1 = h+j; gg h2:t2:t3 = h2+x3+(gg3 [8,5,2])
where x3=555; q3 h:z = h; g3 j h:t:t1 = h+j; gg3 h2:t2:t3 = h2

main = do { 
              (print(f 2 [7,8,9]));
          }