function L = Leverage( r,Start,End )
%Leverage ����<r|r|>
%   r=��������
%   Start=��ʼʱ��
%   End=����ʱ��
for t=1:End
    tmp(t)=(mean((r(1+t:end).^2).*r(1:end-t))-mean(r(1:end-t))*mean(r(1:end-t).^2))/(mean(r(1:end-t).^2)^2);
%     tmp(t)=(mean((r(1+t:end-End+t).^2).*r(1:end-End))-mean(r(1:end-End))*mean(r(1:end-End).^2))/(mean(r(1:end-End).^2)^2);
end
    L=[(Start:End)', tmp(Start:End)'];
end

