#! /usr/bin/env python
# -*- coding:utf-8 -*-
# date 2021/1/6
uuid = {}
name = []
player = 0
import time
import thread
def healthcmdoutput(d):
    if d.output.startswith('Set [health]'):
        return False
     
def healthname(d):
    name.append(d.playername)
    uuid[d.playername] = d.uuid
def healthleft(d):
    uuid.pop(d.playername)
    name.remove(d.playername)
    
def healthrespawn(d):
    if not tool.IfDir('./plugins/health'):
        mc.runcmd('scoreboard objectives add health dummy §4血量')
        mc.runcmd('scoreboard objectives setdisplay belowname health')
        tool.CreateDir('./plugins/health')
        tool.AppendAllText('./plugins/health/health.txt','done!')
        print('[ShowHealth] 前置计分板创建完成')       
    mc.runcmd('scoreboard players add \"'+str(d.playername)+'\" health 0')
    global player
    if player == 0:
        thread.start_new_thread(showhealth,())
        player += 1
    
def showhealth():
    print '[ShowHealth] 线程开启'
    while len(name) > 0:
        for x in range(len(name)):
            i = ipyapi.creatPlayerObject(uuid[name[x]])
            he = eval(str(i.Health))
            mc.runcmd("scoreboard players set \""+name[x]+"\" health "+str(int(he['value'])))
        time.sleep(1)
           
ipyapi.Listen('onLoadName',healthname)
ipyapi.Listen('onPlayerLeft',healthleft)
ipyapi.Listen('onRespawn',healthrespawn)
ipyapi.Listen('onServerCmdOutput',healthcmdoutput)
thread.start_new_thread(gengxin,())
print('[ShowHealth] 血量插件已加载')
