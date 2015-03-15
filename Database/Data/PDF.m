function [p, x]= PDF( r,n )
%PDF �����ʵĸ��ʷֲ�
%   r=������
%   n=�ɷ���Ŀ
r=r(r>0);
tmp=0:mean(r)/n:max(r);
ptmp=histc(r,tmp);
x=(tmp(1:end-1)+tmp(2:end))/2;
x=x';
p=ptmp(1:end-1)*n/sum(ptmp(1:end-1))/mean(r);
end

