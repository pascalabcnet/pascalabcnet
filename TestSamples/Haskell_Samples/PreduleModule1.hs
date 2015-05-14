module PreduleModule1 where

take n _      | n <= 0 =  []
take _ []              =  []
take n x:xs          =  x:take ((n-1) xs)

drop n xs     | n <= 0 =  xs
drop _ []              =  []
drop n x:xs           =  drop((n-1) xs)

splitAt n xs             =  (take(n xs), drop(n xs))

[] `plusplus` ys   = ys
x:xs `plusplus` ys = x:(xs `plusplus` ys)