function MyScript( path )
%MYSCRIPT ִ�и�����sql�ű��ļ�
%   �ű��ļ����������ݼ�
sql=importdata(path);

for i=1:length(sql)
    mysql(sql{i});
end
end

