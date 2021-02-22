#! /usr/bin/env python
# -*- coding:utf-8 -*-
# author : Sbaoor
# date   : 2021/1/3
import json
import time
import thread
from System import IO
formid = {}
player = 0

def svplayer_left(a):
    global player
    player -= 1

def svsavepack():
    global player
    print '[SaveBag] 线程开启！'
    while True:
        if player > 0:
            for x in json.loads(mc.getOnLinePlayers()):
                i = ipyapi.creatPlayerObject(x['uuid'])
                packdict = json.loads(i.InventoryContainer)
                xx = 0
                pack = ''
                while xx < int(len(packdict)):
                    if not packdict[xx]["id"] == 0:
                        pack += packdict[xx]["item"] + '@' + str(packdict[xx]["count"]) +'\n'
                    xx += 1
                tool.WriteAllText('./plugins/SaveBag/'+str(i.getName())+'.txt',pack)
            print '[SaveBag] 数据保存完成,保存了'+str(player)+'个玩家的背包数据'
        time.sleep(60)
def svrespawn(a):
    global player
    formid[a.playername] = 0
    if player == 0:
        thread.start_new_thread(svsavepack,())
        player += 1
def svinputcommand(a):
    if a.cmd == '/checkbag':
        if IFOP(a.playername):
            thread.start_new_thread(svsendonline,(a.playername,))
        else:
            mc.sendText(GetUUID(a.playername),'莫得权限，爬')
        return False
def svformselect(a):
    if int(a.formid) == formid[a.playername]:
        b = json.loads(mc.getOnLinePlayers())
        i = mc.creatPlayerObject(GetUUID(str(b[int(a.selected)]['playername'])))
        packdict = json.loads(i.InventoryContainer)
        pack = '§3CheckBag§r >>> §4正在展示背包物品!§r [§2'+str(b[int(a.selected)]['playername'])+'§r]\n'
        for xx in packdict:
            if not xx["id"] == 0:
                pack += xx["item"] + '@' + str(xx["count"]) +'\n'
        mc.sendText(GetUUID(a.playername),str(pack))

def GetUUID(plname):
    a = json.loads(mc.getOnLinePlayers())
    for key in a:
        if key['playername'] == plname:
            return key['uuid']
    return 'null'

def IFOP(plname):
    a = json.loads(IO.File.ReadAllText('permissions.json'))
    xuid = GetXUID(plname)
    for key in a:
        if key['xuid'] == xuid:
            if key['permission'] == 'operator':
                return True
    return False

def GetXUID(plname):
    a = json.loads(mc.getOnLinePlayers())
    for key in a:
        if key['playername'] == plname:
            return key['xuid']
    return 'null'

def svsendonline(name):
    lists = []
    a = json.loads(mc.getOnLinePlayers())
    for key in a:
        lists.append(key['playername'])
    formid[name] = mc.sendSimpleForm(GetUUID(name),'查包面板','选择谁？',str(lists).replace('\'','\"'))
ipyapi.lists('onFormSelect',svformselect)
ipyapi.Listen('onInputCommand',svinputcommand)
ipyapi.Listen('onPlayerLeft',svplayer_left)
ipyapi.Listen('onLoadName',svrespawn)
mc.setCommandDescribe('checkbag','管理员查包')
if not tool.IfDir('./plugins/SaveBag'):
    tool.CreateDir('./plugins/SaveBag')
    print('[SaveBag] 文件夹创建成功！')
print('[SaveBag] 背包保存已加载！')
print('[SaveBag] 背包查询已加载！')
