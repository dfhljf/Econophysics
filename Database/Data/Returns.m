function r = Returns( price,dt,flag )
%RETURNS 返回归一化的收益率
%   price=价格序列
%   flag=收益率类型，0-log,1-相减
if flag==0
    r=log(price(1+dt:end)./price(1:end-dt));
elseif flag==1
    r=price(1+dt:end)-price(1:end-dt);    
end
    r=(r-mean(r))/std(r,1);
end

