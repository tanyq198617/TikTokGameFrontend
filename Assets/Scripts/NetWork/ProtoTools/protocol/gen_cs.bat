echo off
:: 当前路径
set curdir=%~dp0
:: 进入sprotodump目录
cd /d %curdir%/../sprotodump
:: 将.sproto文件转为C#脚本，存放在gen_cs文件夹中
lua ./sprotodump.lua -cs %curdir%/c2s.sproto -o %curdir%/gen_cs/c2ssproto.cs
lua ./sprotodump.lua -cs %curdir%/s2c.sproto -o %curdir%/gen_cs/s2csproto.cs
:: 输出完成
echo sproto to cs, done
:: 按任意键退出
pause
