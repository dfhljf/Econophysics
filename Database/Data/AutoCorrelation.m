function A = AutoCorrelation( x,Start,End )
%AUTOCORRELATION 返回自相关函数
%   x=输入数据
%   Start=开始时间
%   End=结束时间
for t=1:End
%     tmp(t)=(mean(x(1+t:end-End+t).*x(1:end-End))-mean(x(1:end-End))^2)/var(x(1:end-End),1);
    tmp(t)=(mean(x(1+t:end).*x(1:end-t))-mean(x(1:end-t))^2)/var(x(1:end-t),1);
end
A=[(Start:End)', tmp(Start:End)'];
end

