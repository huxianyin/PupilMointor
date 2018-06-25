#!/usr/bin/env python
# -*- coding: utf-8 -*-
import time
import socket
import threading
import json
import random
import math

ip_adress = "127.0.0.1"
ip_port = 10086
max_clients = 5

chart_config_bar = {
  "name": "Energy Consumption bar",
  "axis": "kW",
  "type": "bar",
  "groups":["Room 217","Room 301"],
  "categorys":["Light","Aircon","Freezer"]
  }

chart_config_pie = {
  "name": "Energy Consumption pie",
  "axis": "kW",
  "type": "pie",
  "groups":[],
  "categorys":["Light","Aircon","Freezer"]
  }

chart_config_graph = {
  "name": "Co2 Intensity",
  "axis": "ppm",
  "type": "graph",
  "groups": [],
  "categorys":["Room 217","Room 301"]
  }

chart_config = {"title":"Sensing Room Monitor","info_list":[chart_config_bar,chart_config_pie,chart_config_graph]}

class SendDataThread(threading.Thread):     
  def __init__(self,client_executor,addr,stopevt = None):     
    threading.Thread.__init__(self)     
    self.client_executor = client_executor
    self.addr = addr
    self.stopevt = stopevt

  def send_data(self,chart,group,category,value):
    data = {"chart":chart,"group":group,"category":category,"value":value}
    repred_data = repr(json.dumps(data,ensure_ascii=False,sort_keys=True)).encode('utf-8')[1:-1] + ";"
    self.client_executor.send(bytes(repred_data))

  def run(self):
    print('Accept new connection from %s' % (self.addr,))
    #start sending a message including information of chart configuration
    self.client_executor.sendall(bytes(repr(json.dumps(chart_config,sort_keys=True)).encode('utf-8')[1:-1]))   #发送json信息
    time.sleep(1)
    self.client_executor.send(bytes("start send data".encode('utf-8')))
    time.sleep(1)
    t = 0
    interval = 0.5
    while not (self.stopevt.isSet()):
      #sending realtime data!
      try:
        self.send_data("Energy Consumption bar","Room 217","Light",[0.,random.uniform(0, 1)*10])
        self.send_data("Energy Consumption bar","Room 301","Light",[0.,random.uniform(0, 1)*10])
        self.send_data("Energy Consumption bar","Room 217","Aircon",[0.,random.uniform(0, 1)*10])
        self.send_data("Energy Consumption bar","Room 301","Aircon",[0.,random.uniform(0, 1)*10])
        self.send_data("Energy Consumption bar","Room 217","Freezer",[0.,random.uniform(0, 1)*10])
        self.send_data("Energy Consumption bar","Room 301","Freezer",[0.,random.uniform(0, 1)*10])
        self.send_data("Energy Consumption pie",None,"Light",[0.,random.uniform(0, 1)*10])
        self.send_data("Energy Consumption pie",None,"Aircon",[0.,random.uniform(0, 1)*10])
        self.send_data("Energy Consumption pie",None,"Freezer",[0.,random.uniform(0, 1)*10])
        
        self.send_data("Co2 Intensity",None,"Room 217",[t,math.sin(t)+2.+random.uniform(0, 0.3)])
        self.send_data("Co2 Intensity",None,"Room 301",[t,math.cos(t)+random.uniform(0, 0.3)])
        #print(t)
        t += 0.1
        time.sleep(interval)

      except Exception,e:
        print("exception happen:",e)
        break
      
    self.client_executor.close()
    print('Connection from %s closed.' % (self.addr,))

  def stop(self):
    self.stopevt.set()


def main():
  # 构建Socket实例、设置端口号  和监听队列大小
  listener = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
  listener.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
  listener.bind((ip_adress, ip_port))
  listener.listen(max_clients)
  print('Waiting for connect...')

  # 进入死循环，等待新的客户端连入。一旦有客户端连入，就分配一个线程去做专门处理。然后自己继续等待。
  while True:
    try:
      client_executor, addr = listener.accept()
      t = SendDataThread(client_executor,addr,threading.Event())
      t.start()
    except KeyboardInterrupt:
      print("Server shutdown...")
      listener.close()
      t.stop()
      break

if __name__ == '__main__':
    main()




