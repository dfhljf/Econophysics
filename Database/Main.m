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
selectExpDialog = figure('Visible','off','Position',[360,300,450,285]);
hexpList=uicontrol('Style','popupmenu',...
           'String',int2str(expId),...
           'Position',[130,250,100,25],'Callback',@expList_Callback);
hParameters=uicontrol()
set(selectExpDialog,'Visible','on');
%% ����ʵ������
%% �ر����ݿ�����
mysql('close');

%% �����ص�
function expList_Callback(source,eventdata)
str=get(source,'String');
val=get(source,'Value');
selectExp=str2num(str(val));
end
end