function MyScript( path )
%MYSCRIPT 执行给定的sql脚本文件
%   脚本文件不返回数据集
sql=importdata(path);

for i=1:length(sql)
    mysql(sql{i});
end
end

