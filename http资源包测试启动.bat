:: 代码页更改为Unicode(UTF-8)
chcp 65001
@echo OFF

@REM cd /d C:\Program Files\Google\Chrome\Application\chrome.exe
@REM start chrome.exe 127.0.0.1:8000

@REM set path1=%~d0
@REM set path2=%cd%
@REM set path3=%0
@REM set path4=%~dp0
@REM set path5=%~sdp0

@REM echo 当前盘符path1：%path1%
@REM echo 当前路径path2：%path2%
@REM echo 当前执行命令行path3：%path3%
@REM echo 当前bat文件路径path4：%path4%
@REM echo 当前bat文件短路径path5：%path5%

@REM pause>nul 

echo 请用浏览器打开127.0.0:8000
cd /d %~dp0
cd Bundles & python -m http.server --bind 0.0.0.0
pause