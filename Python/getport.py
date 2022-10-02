import serial


#scannerId='10C4:EA60'
scannerID=serial.tools.list_ports( '10C4:EA60' )
#com = serial.Serial()
# com.port = 'COM3'
#com.port = '/dev/ttyUSB0'
print (scannerID)
