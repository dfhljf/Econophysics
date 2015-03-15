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
    Verify(parameters,market,agents);
    plot(market(:,1),market(:,2),'*-');
end
end