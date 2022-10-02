#!/usr/bin/python3
import serial
import time
import argparse
import serial.tools.list_ports
import packet

scannerId='10C4:EA60'
comPort=''
ports = serial.tools.list_ports.grep(scannerId)
for port, desc, hwid in sorted(ports):
   comPort=port
   print("ChryslerScanner using port {}: {} [{}]".format(port, desc, hwid))

com = serial.Serial()
com.port = comPort
com.baudrate = 250000
com.timeout = 20
com.dtr = None  # Disable auto-reset upon USB-connection
com.open()


def send_requests():
    time.sleep(4)  # wait for reset

    # Purge stale data
    if com.inWaiting():
        data = None
        try:
            data = com.read(com.inWaiting())
        except TypeError:
            print("Error")
            pass
        print(type(data))
        print(data.hex())
        rdPacket = packet.PacketRx()
        rdPacket.decode(data) 

    # Open the request file and feed it one line at a time
    with open('autocross.txt', "r") as f:
        for line in f:
            request = bytes.fromhex(line.replace('\n', ''))
#            packet_length = len(request)
#            packet_length_hb = (packet_length >> 8) & 0xFF
#            packet_length_lb = packet_length & 0xFF
#            packet = bytearray([0x3D, packet_length_hb, packet_length_lb ])
#            packet.extend(request)
#
#            # Checksum is a simple byte-adder discarding overflow
#            checksum = 0
#            i = 0
#            while i < len(packet):
#                checksum += packet[i]
#                i += 1
#
#            checksum = (checksum & 0xFF).to_bytes(1, 'little')
#            packet.extend(checksum)
            myPacket = packet.PacketTx()
            com.write(myPacket.buildPacket(request))
#            com.write(myPacket)
#            print("Sent " + " ".join(["{:02x}".format(x) for x in myPacket]).upper())

            echo = False
            timeout = time.time() + 4
            while not echo:
#                print(".")
                if time.time() > timeout:
                    break
                if com.inWaiting():
                    data = None
                    try:
#                        print("reading...")
                        data = com.read(com.inWaiting())
                        timeout = time.time() + 2
                    except TypeError:
                        print("error")
                        pass
                    print("Return: " + data.hex())
            if not echo:
                print(" ".join(["{:02x}".format(x) for x in request]).upper() + ": request timeout")
#                print(" ".join(["{:02x}".format(x) for x in packet]).upper() + ": packet timeout")
    com.close()



parser = argparse.ArgumentParser(description='CCD-bus reader script')
parser.add_argument('operation')
args = vars(parser.parse_args())

if args['operation'] == 'write':
    send_requests()

