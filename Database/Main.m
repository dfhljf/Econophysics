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
selectExpDialog = figure('Visible','off','MenuBar','none','Position',[360,300,150,100]);
hexpList=uicontrol(selectExpDialog,'Style','popupmenu',...
           'String',int2str(expId),...
           'Position',[50 70 40 20]);
hSelectButton=uicontrol(selectExpDialog,'Style','pushbutton',...
           'String','确定',...
           'Position',[50 30 40 30],...
           'Callback',@Select_Callback);
set(selectExpDialog,'Visible','on');



%% 函数回调
function Select_Callback(source,eventdata)
%% 载入实验数据
[parameters market agents]=ExtractData(str2num(hexpList.String(hexpList.Value)));
%% 关闭数据库连接
mysql('close');
close(selectExpDialog);
HandleData(parameters,market,agents);
end
%% 数据处理
function HandleData(parameters,market,agents)
    Verify(parameters,market,agents);
    plot(market(:,1),market(:,2),'*-');
end
end