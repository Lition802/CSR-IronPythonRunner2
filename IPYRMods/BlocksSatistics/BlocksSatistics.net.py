#! /usr/bin/env python
# -*- coding:utf-8 -*-
import json
import time
import thread
from System import IO
from System import *
if not IO.File.Exists('./plugins/BlocksSatistics/data.json'):
    IO.Directory.CreateDirectory('./plugins/BlocksSatistics')
    IO.File.WriteAllText('./plugins/BlocksSatistics/data.json','{}')
countblock =json.loads(IO.File.ReadAllText('./plugins/BlocksSatistics/data.json'))
def Blodestroyblock(d):
    countblock[d.playername] += 1   
def Bloplayer_left(a):
    tool.WriteAllText('./plugins/BlocksSatistics/data.json',json.dumps(countblock, sort_keys=True, indent=4, separators=(',', ': ')))
    
def Bloload_name(d):
    global countblock
    if not countblock.has_key(d.playername):
        countblock[str(d.playername)] = 0
        
def Bloinputcommand(d):
    if d.cmd == '/blsat':
        bb = ''
        for key in countblock:
            bb += '§3'+key +'§r : §e'+str(countblock[key])+'\n'
        mc.sendModalForm(BloGetUUID(d.playername),'§6方块挖掘榜',bb,'§2OK','§4取消')
        return False
def BloGetUUID(plname):
    a = json.loads(mc.getOnLinePlayers())
    for key in a:
        if key['playername'] == plname:
            return key['uuid']
    return 'null'
ipyapi.Listen('onLoadName',Bloload_name)
ipyapi.Listen('onInputCommand',Bloinputcommand)
ipyapi.Listen('onPlayerLeft',Bloplayer_left)
ipyapi.Listen('onDestroyBlock',Blodestroyblock)
mc.setCommandDescribe('blsat','查看挖掘榜')
print '[BlocksSatistics] 装载成功'
print '[BlocksSatistics] 作者：Sbaoor'
print '[BlocksSatistics] 当前版本：1.2.0'
