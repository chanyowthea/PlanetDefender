# Planet Defender Version 0.1.5 

# 代码设计

Framework任何framework都不可以引用外部类，保证本framework内聚，不依赖外部类
Common作为最基本的类库同样不可引用外部类

Core游戏核心，可以引Army和Enemy等的基类
Ore继承于Enemy
SpacePlane继承于Army
Player可以应用任意类

## Entity 一个单位【箱子，金币，陨石】
HP
MaxHP
Defense
MoveDir
MoveSpeed
Faction

## Army 一个战斗单位【炮塔，航天飞机】
Attack
VisualField


