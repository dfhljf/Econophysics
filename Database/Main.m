function Main
%% ������������
addpath('D:\Files\Document\Code\Git\Econophysics\Database');
addpath('D:\Files\Document\Code\Git\Econophysics\Database\Data');
addpath('D:\Files\Document\Code\Git\Econophysics\Database\Initial');

%% ���ݿ�����
mysql('open','localhost','root','jianghui');
mysql('use econophysics');

%% ѡ��ʵ����
expId=mysql('select id from parameters');
selectExpDialog = figure('Visible','off','MenuBar','none','Position',[360,300,150,100]);
hexpList=uicontrol(selectExpDialog,'Style','popupmenu',...
           'String',int2str(expId),...
           'Position',[50 70 40 20]);
hSelectButton=uicontrol(selectExpDialog,'Style','pushbutton',...
           'String','ȷ��',...
           'Position',[50 30 40 30],...
           'Callback',@Select_Callback);
set(selectExpDialog,'Visible','on');



%% �����ص�
function Select_Callback(source,eventdata)
%% ����ʵ������
[parameters market agents]=ExtractData(str2num(hexpList.String(hexpList.Value)));
%% �ر����ݿ�����
mysql('close');
close(selectExpDialog);
HandleData(parameters,market,agents);
end
%% ���ݴ���
function HandleData(parameters,market,agents)
    if any(Verify(parameters,market,agents))
        errordlg('������֤δͨ��');
        return;
    end
    subplot(2,2,1);
    plot(market(:,1),market(:,2),'*-');
    r=Returns(market(:,2),1,0);
    [p x]=PDF(r,5);
    subplot(2,2,2);
    plot(x,p,'.');
    A=AutoCorrelation(abs(r),1,100);
    subplot(2,2,3);
    plot(A(:,1),A(:,2),'o');
    L=Leverage(r,1,100);
    subplot(2,2,4);
    plot(L(:,1),L(:,2),'^');
end
end