function Main
%% 环境变量设置
addpath('D:\Files\Document\Code\Git\Econophysics\Database');
addpath('D:\Files\Document\Code\Git\Econophysics\Database\Data');
addpath('D:\Files\Document\Code\Git\Econophysics\Database\Initial');

%% 数据库连接
mysql('open','localhost','root','jianghui');
mysql('use econophysics');

%% 选择实验编号
expId=mysql('select id from parameters');
selectExpDialog = figure('Visible','off','Position',[360,300,450,285]);
hexpList=uicontrol('Style','popupmenu',...
           'String',int2str(expId),...
           'Position',[130,250,100,25],'Callback',@expList_Callback);
hParameters=uicontrol()
set(selectExpDialog,'Visible','on');
%% 载入实验数据
%% 关闭数据库连接
mysql('close');

%% 函数回调
function expList_Callback(source,eventdata)
str=get(source,'String');
val=get(source,'Value');
selectExp=str2num(str(val));
end
end