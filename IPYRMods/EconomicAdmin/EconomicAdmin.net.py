#! /usr/bin/env python
# -*- coding:utf-8 -*-
import json
import time
import thread
formid = {}
page = {}
toname = {}
def econinputcommand(d):
    if d.cmd == '/econadmin' and IFOP(str(d.playername)):       
        econSendForm(str(d.playername),tool.GetShareFunc('GetUUID')(str(d.playername)),'main')
        return False

def econformselect(d):
    if int(d.formid) == int(formid[d.playername]) and d.selected != 'null':
        if page[d.playername] == 'main':
            j = json.loads(mc.getOnLinePlayers())
            pln = j[int(d.selected)]['playername']
            toname[d.playername] = str(pln)
            gui = mc.creatGUI('经济管理——'+str(pln))
            gui.AddDropdown('模式',0,'[减少,增加]')
            gui.AddInput('操作数值','数值')
            formid[d.playername] = gui.SendToPlayer(d.uuid)
            page[d.playername] = 'main2'
        if page[d.playername] == 'main2':
            jsons = eval('{\'selected\':'+str(d.selected)+'}')
            if jsons['selected'][1].isdigit():
                if jsons['selected'][0] == 0:
                    if mc.getscoreboard(tool.GetShareFunc('GetUUID')(str(toname[d.playername])),'money') > int(jsons['selected'][1]):
                        mc.runcmd('scoreboard players remove \"'+str(toname[d.playername])+'\" money '+str(jsons['selected'][1]))
                        mc.sendText(d.uuid,'[§eEconCore§r] 已为'+str(toname[d.playername])+'移除'+str(jsons['selected'][1])+'金币！')
                    else:
                        mc.sendText(d.uuid,'[§eEconCore§r] '+str(toname[d.playername])+'的钱不够！')
                if jsons['selected'][0] == 1:
                    mc.runcmd('scoreboard players add \"'+str(toname[d.playername])+'\" money '+str(jsons['selected'][1]))
                    mc.sendText(d.uuid,'[§eEconCore§r] 已为'+str(toname[d.playername])+'添加'+str(jsons['selected'][1])+'金币！')
            else:
                mc.sendText(d.uuid,'[§eEconCore§r] §4请输入正确格式的数字')
        formid[d.playername] = '0'
        page[d.playername] = '0'
        toname[d.playername] = '0'

def econSendForm(plname,uuid,pag):
    a = json.loads(mc.getOnLinePlayers())
    monlist = []
    for key in a:
        monlist.append(str(key['playername'])+'\n'+str(mc.getscoreboard(str(key['uuid']),'money'))+'$')
    formid[plname] = mc.sendSimpleForm(uuid,'§3经济管理面板','',str(monlist).replace('\'','\"'))
    page[plname] = pag
    
#########################
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
    
ipyapi.Listen('onInputCommand',econinputcommand)
ipyapi.Listen('onFormSelect',econformselect)
mc.setCommandDescribe('econadmin','打开经济管理面板')
print('[EconAdmin] 装载成功！')
print('[EconAdmin] 作者：Sbaoor')
print('[EconAdmin] 当前版本：1.2.0')
