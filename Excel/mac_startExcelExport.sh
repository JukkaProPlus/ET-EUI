#!/bin/sh

# 设置脚本的工作目录为脚本所在目录
BASEDIR=$(dirname "$0")
cd "$BASEDIR"

# 根据实际路径更新 Bin 目录路径
cd ../Bin

# 根据实际路径更新 dotnet 命令的路径
/usr/local/share/dotnet/dotnet Tools.dll --AppType=ExcelExporter

