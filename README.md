# GStar-Adventurer

<div align=center>
  <img height="150" src="https://github.com/2585479524/git_pic/blob/master/GStar-Adventurer/logo.jpg"/><br>
  
  <img height="150" src="https://github.com/2585479524/git_pic/blob/master/GStar-Adventurer/ChooseLevel.jpg"/><br>
  
  <img height="150" src="https://github.com/2585479524/git_pic/blob/master/GStar-Adventurer/Level.jpg"/><br>
</div>

<hr>

游戏为一款箱庭式沙盒游戏，保留沙盒元素：建造与破坏，结合箱庭闯关模式

关卡内散布着“布丁”和各种怪物，玩家扮演小恐龙通过吃掉方块、设置陷阱、吐出方块等方式完成关卡。

游戏提供关卡编辑器，允许玩家在全素材的情况下构建自己的沙盒世界

（当然，游戏玩法是策划的功劳）

## MVC架构

由Controller层处理吃掉方块、吐出方块等逻辑

由Model层定义沙盒地图，从CSV文件动态读取关卡信息（包括方块信息、怪物信息、布丁信息等）

由view层更新数据，控制显示内容

单例模式统一管理角色控制、声音、特效、UI

构造对象池，减少重复方块占用的内存，同时，通过将方块的渲染分解为六个面的渲染，只显示摄像机视界内的面，极大优化内存

## 关卡编辑器

  <img height="150" src="https://github.com/2585479524/git_pic/blob/master/GStar-Adventurer/Edit.jpg"/><br>

## 图鉴系统

  <img height="150" src="https://github.com/2585479524/git_pic/blob/master/GStar-Adventurer/Illustration.jpg"/><br>
