module Main where

f 1 (h1:t1) = 3
f n (h:t) = let x=1; y=2+x+n+h; g j h:t:t1 = h+j; gg h2:t2:t3 = h2+(g1 2 [1,2,3]) 
                                                            where x1=555;
                                                                  g1 i h:t:t1 = h+i; 
                                                                  gg1 h2:t2:t3 = h2 
          in x+(g 4 [1,2,3])+ (gg [4,5,6])+n+x5 
where x5=555; q h:z = h; g j h:t:t1 = h+j; gg h2:t2:t3 = h2+x3+(gg3 [8,5,2])
where x3=555; q3 h:z = h; g3 j h:t:t1 = h+j; gg3 h2:t2:t3 = h2

main = do {
            (print(f 5 [1,2,3]));
          }