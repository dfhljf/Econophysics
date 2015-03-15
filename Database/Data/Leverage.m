function L = Leverage( r,Start,End )
%Leverage 返回<r|r|>
%   r=输入数据
%   Start=开始时间
%   End=结束时间
for t=1:End
    tmp(t)=(mean((r(1+t:end).^2).*r(1:end-t))-mean(r(1:end-t))*mean(r(1:end-t).^2))/(mean(r(1:end-t).^2)^2);
%     tmp(t)=(mean((r(1+t:end-End+t).^2).*r(1:end-End))-mean(r(1:end-End))*mean(r(1:end-End).^2))/(mean(r(1:end-End).^2)^2);
end
    L=[(Start:End)', tmp(Start:End)'];
end

