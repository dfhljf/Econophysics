function r = Returns( price,dt,flag )
%RETURNS ���ع�һ����������
%   price=�۸�����
%   flag=���������ͣ�0-log,1-���
if flag==0
    r=log(price(1+dt:end)./price(1:end-dt));
elseif flag==1
    r=price(1+dt:end)-price(1:end-dt);    
end
    r=(r-mean(r))/std(r,1);
end

