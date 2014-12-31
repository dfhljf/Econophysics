# 金融物理模拟交易实验平台
## 说明
### 欢迎界面
初始时，用户会被导向欢迎界面。

该界面将提供有关于实验的说明，用户应当仔细阅读信息，然后登陆进入实验平台
### 实验平台
#### 初始
在初始时，用户拥有一定数量的**现金**和**股票**，在实验进行时，可以使用这些初始
值来进行交易。
#### 运行
在每轮用户仅允许**提交一次一定数量**（数量受限）的交易（买或者卖），
提交后不允许更改。

交易后，用户所拥有的现金和股票**立即改变**

*现金计算公式：现在的现金=过去的现金+（当前每股价格+交易费用）\*交易数量*

*股票计算公式：现在的股票=过去的股票+交易数量*

用户在每轮都会有一定的分红值，分红也会持续一定的时间。分红将在下一轮开始时分配。

*分红计算公式：本轮的现金=上轮的现金+分红\*股票数目*

当现金数目不足时，将以本轮价格卖掉合适数量的股票来补贴分红。这部分股票的卖出自动
进行，不收取交易费用。

每轮都重复上述操作，我们将在一定的轮次之后结束实验

结束轮次的前一轮的交易还将被记录，最后的价格取结束轮次的价格。
#### 结束
实验结束时，将按照用户的总资产分配奖励

*总资产计算公式=现金+股票数目\*结束轮次的价格*

分配奖励给**总资产>平均总资产的用户**
### 管理界面
1. 设定参数，建立实验，等待用户加入。动态显示加入的用户的数量和当前参数
2. 开始实验，掉线监测
3. 暂停实验，实验将在一轮结束后，新一轮即将开始时暂停。确保轮数的原子性
4. 继续实验，立即开始下一轮。
5. 结束实验，计算部分实验结果。
6. 重置实验

每步都弹框确认，防止误点击
### 数据处理
独立的应用程序，用来导出需要的数据
1. 选定某个实验
2. 提供导出接口

## 平台架构
### 界面
#### 欢迎界面
#### 用户界面
#### 管理界面
### 核心库
公开数据结构和操作方法
### 数据IO库
提供抽象数据读写 方便扩展到其它数据库，暂时只实现mysql，每个对象维护自己的数据读写权限和方法。
##技术细节
### 核心库
#### 时序
0. 关闭交易系统
1. 更新市场价格
2. 更新市场状态
3. 给代理人分配分红，存储数据，获取新的分红
4. 存储市场数据
5. 画图，结束触发事件加载图像
6. 进入下一轮
7. 打开交易系统
每一轮都是原子操作
### 界面
使用UpdatePanel提供异步更新
### 图片加载
使用事件保持时序关系

## 实验细节
