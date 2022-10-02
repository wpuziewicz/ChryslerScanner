import time
import sys
from datetime import datetime
import serial
import logging
import queue
import socket
import fcntl
import struct

from pynbp import *

import argparse
import serial.tools.list_ports



innoId='067B:2303'
comPort=''
ports = serial.tools.list_ports.grep(innoId)
for port, desc, hwid in sorted(ports):
   comPort=port
   print("Gauges using port {}: {} [{}]".format(port, desc, hwid))



def get_ip_address(ifname):
    s = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    return socket.inet_ntoa(fcntl.ioctl(
        s.fileno(),
        0x8915,  # SIOCGIFADDR
        struct.pack('256s',  bytes(ifname[:15], 'utf-8'))
    )[20:24])

myIP=get_ip_address('eth0')



serialByte = 0  # Used to hold data coming over UART
prevByte = 0    # Previous byte read
packetLen = 0
serialWord = 0
couldBeHeader = 0

nbp_queue=queue.Queue()

mypynbp = WifiPyNBP(ip=myIP , port=35000, nbpqueue=nbp_queue, device_name='Testing')
#mypynbp = WifiPyNBP(ip='192.168.8.2', port=35000, nbpqueue=nbp_queue, device_name='Testing')
#mypynbp = WifiPyNBP( port=35000, nbpqueue=nbp_queue, device_name='Testing')

mypynbp.daemon = True
mypynbp.min_update_interval = 0.05

mypynbp.start()

#inSrc = open("inno2022.bin", "rb")
inSrc = serial.Serial(comPort , 19200, timeout=10 )
serialByte = inSrc.read(1)
testkpis=[]
while serialByte:
      
#        print(serialByte.hex())
#        print("{:08b}".format(int(serialByte.hex(),16)))
        packetLen = 0
        # If bit 8 is set, then this may be part of a header packet
        if (int.from_bytes(serialByte, sys.byteorder) & 0b10000000) == 0b10000000:
            packetLen = 0
            if couldBeHeader == 0:
              prevByte = serialByte
            couldBeHeader += 1
            #print("Could be header ", couldBeHeader)
        else: 
            couldBeHeader = 0
            prevByte = 0
            
        if couldBeHeader  == 2:
            #packetLen = (128 * (int.from_bytes(prevByte, sys.byteorder) & 1)) + (int.from_bytes(serialByte, sys.byteorder) & 0b01111111 ),1,sys.byteorder
            #print("{:08b}".format(int(prevByte.hex(),16)),"{:08b}".format(int(serialByte.hex(),16)) )
            packetLen = int.from_bytes(prevByte, sys.byteorder) * 256
            packetLen = packetLen + int.from_bytes(serialByte, sys.byteorder)
            #print("{:16b}".format(packetLen))

            testbit = (packetLen & 0b1010001010000000)
            #print("{:16b}".format(testbit))

            if testbit == 0b1010001010000000:
                if packetLen & 0b0000001000000000 == 0b0000001000000000:
                   hiBit = (int.from_bytes(prevByte, sys.byteorder) & 0b1) 
                   loBit = (int.from_bytes(serialByte ,sys.byteorder) & 0b01111111)
                   packetLen = (hiBit << 6) + loBit
                   payload = NbpPayload(timestamp=time.time(), packettype='UPDATE', nbpkpilist=testkpis)
                   nbp_queue.put(payload)
                   testkpis=[]
#                   print("published")

#            print("Header says packet len is ", packetLen, '\n')
        
        channel = 0
        while packetLen > 0 :
            channel += 1
            couldBeHeader = 0
            serialWord = inSrc.read(2)
            if (packetLen == 9):
               print()
               print( datetime.now(), end='\t')
            readWord = int.from_bytes(serialWord, 'big')
#            print("Packet ", packetLen, end = '\t')
#            print(hex(readWord), end ='\t')
#            print("{:#018b}".format(int(serialWord.hex(),16)), end='\t')
#            print("{:#018b}".format(readWord,16), end='\t')

            # If either byte of the word has the header bit set, exit the while condition and set
            if (int.from_bytes(serialWord, 'big') & 0b1000000010000000 > 0): 
#                print("\n Header bit set")
                packetLen = 0
                prevByte = int.to_bytes(packetLen,2,sys.byteorder)[1]
                couldBeHeader = 1
                continue

            # If the Lamba bit is set, this is the first word of a two word AFR reading
#            if (int.from_bytes(serialWord, sys.byteorder) & 0b0100001000000000 == 0b0100001000000000 ):
            if (readWord &  0b0100001000000000 == 0b0100001000000000 ):
            #  if (int.from_bytes(serialWord, sys.byteorder) & 0b1010000010000000 == 0): 
#                lambdaVal = file.read(2)

                lambdaStat = (serialWord[0] & 0b00011100) >> 2
                afr =  (serialWord[0] & 0b00000001) << 7
                afr += (serialWord[1] & 0b01111111)
                afr = afr / 10
#                print('\nLambdaStat = ' , lambdaStat)
#                print('AFR = ', afr)

                lambdaVal = inSrc.read(2)
                lmb = (lambdaVal[0] & 0b00111111) << 7
                lmb += (lambdaVal[1] & 0b01111111)

 #               print('lmb = ', lmb)

                afrReading = ((lmb +  500) * afr ) / 1000
#                print('AFR Reading = ', afrReading)
                print("{:.2f}".format(afrReading), end='\t')
                packetLen -= 2
                testkpis.append(NbpKPI(name='AFR'+str(packetLen), unit="L", value=afrReading))
                continue


            # Otherwise, assume we have read a data word
            dataValue = ((int(serialWord[0]) & 0b1111111100000000 ) >> 1) + int(serialWord[1])
            if (packetLen == 9): 
                # print()
                # Oil pressure   
                offset = 0
                slope = 145/255
                dataValue = offset + (slope * dataValue)
                testkpis.append(NbpKPI(name='Oil', unit="PSI", value=dataValue))

            elif (packetLen == 7):
                # Fuel pressure   
                offset = 0
                slope = 145/255
                dataValue = offset + (slope * dataValue)
                testkpis.append(NbpKPI(name='Fuel', unit="PSI", value=dataValue))

            elif (packetLen == 5):
                # Intake air pressure
                offset = -1
                slope = 4/255
                dataValue = offset + (slope * dataValue)
                testkpis.append(NbpKPI(name='Map', unit="bar", value=dataValue))

            packetLen -= 1

             
            # Print the contents of the serial data
            try:
                #print(packetLen, '\t', dataValue)
                #print(dataValue, end='\t')
                print("{:.2f}".format(dataValue), end='\t')
            except:
                pass

#            print("packet len" , packetLen)

        serialByte = inSrc.read(1)
inSrc.close()

