function [p, x]= PDF( r,n )
%PDF 收益率的概率分布
%   r=收益率
%   n=成分数目
r=r(r>0);
tmp=0:mean(r)/n:max(r);
ptmp=histc(r,tmp);
x=(tmp(1:end-1)+tmp(2:end))/2;
x=x';
p=ptmp(1:end-1)*n/sum(ptmp(1:end-1))/mean(r);
end

