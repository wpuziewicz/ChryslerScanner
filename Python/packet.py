from enum import Enum

class Bus(Enum):
    usb = 0x00
    ccd = 0x01
    pcm = 0x02
    tcm = 0x03
    pci = 0x04

class Command(Enum):
    reset = 0x00
    handshake = 0x01
    status = 0x02
    settings = 0x03
    request = 0x04
    response = 0x05
    msgTx = 0x06
    msgRx = 0x07
    debug = 0x0E
    error = 0x0F

class ResetMode(Enum):
    resetInit = 0x00
    resetDone = 0x01

class HandShake(Enum):
    handshakeOnly = 0x00
    handshakeAndStatus = 0x01



class StatusMode(Enum):
    none = 0x00

class SettingsMode(Enum):
    leds = 0x01
    setCCDBus = 0x02
    setSCIBus = 0x03
    setRepeatBehavior = 0x04
    setLCD = 0x05
    setPCIBus = 0x06
    setProgVolt = 0x07

class RequestMode(Enum):
    hardwareFirmwareInfo = 0x01
    timestamp = 0x02
    batteryVoltage = 0x03
    extEEPROMChecksum = 0x04
    CCDBusVoltages = 0x05
    VBBVolts = 0x06
    VPPVolts = 0x07
    AllVolts = 0x08

class ResponseMode(Enum):
    hardwareFirmwareInfo = 0x01
    timestamp = 0x02
    batteryVoltage = 0x03
    extEEPROMChecksum = 0x04
    CCDBusVoltages = 0x05
    VBBVolts = 0x06
    VPPVolts = 0x07
    AllVolts = 0x08

class MsgTxMode(Enum):
    stop = 0x01
    single = 0x02
    list = 0x03
    repeatedSingle = 0x04
    repeatedList = 0x05
    singleVPP = 0x82

class MsgRxMode(Enum):
    stop = 0x01
    single = 0x02
    list = 0x03
    repeatedSingle = 0x04
    repeatedList = 0x05
    singleVPP = 0x82

class DebugMode(Enum):
    randomCCDBusMessages = 0x01
    readIntEEPROMbyte = 0x02
    readIntEEPROMblock = 0x03
    readExtEEPROMbyte = 0x04
    readExtEEPROMblock = 0x05
    writeIntEEPROMbyte = 0x06
    writeIntEEPROMblock = 0x07
    writeExtEEPROMbyte = 0x08
    writeExtEEPROMblock = 0x09
    setArbitraryUARTSpeed = 0x0A
    initBootstrapMode = 0x0B
    uploadWorkerFunction = 0x0C
    startWorkerFunction = 0x0D
    exitWorkerFunction = 0x0E
    defaultSettings = 0xE0
    getRandomNumber = 0xE1
    restorePCMEEPROM = 0xF0
    getAW9523Data = 0xFE
    test = 0xFF

class ErrorMode(Enum):
    ok = 0x00
    errorLengthInvalidValue = 0x01
    errorDatacodeInvalidCommand = 0x02
    errorSubDatacodeInvalidValue = 0x03
    errorPayloadInvalidValues = 0x04
    errorPacketChecksumInvalidValue = 0x05
    errorPacketTimeoutOccured = 0x06
    errorBufferOverflow = 0x07
    errorInvalidBus = 0x08
    errorSCILsNoResponse = 0xF6
    errorNotEnoughMCURAM = 0xF7
    errorSCIHsMemoryPtrNoResponse = 0xF8
    errorSCIHsInvalidMemoryPtr = 0xF9
    errorSCIHsNoResponse = 0xFA
    errorEEPNotFound = 0xFB
    errorEEPRead = 0xFC
    errorEEPWrite = 0xFD
    errorInternal = 0xFE
    errorFatal = 0xFF

class OnOffMode(Enum):
    off = 0x00
    on = 0x01

class BaudMode(Enum):
    extraLowBaud = 0x01
    lowBaud = 0x02
    highBaud = 0x03
    extraHighBaud = 0x04

class SCISpeedMode(Enum):
    lowSpeed = 0x01
    highSpeed = 0x02

class HwInfo:
    hardwareVer = 0.0
    hardwareDate = bytearray([])
    assemblyDate = bytearray([])  
    firmwareDate = bytearray([])

    def decode(self, payload):
      hardwareVer = (payload[0:1]) / 10
      hardwareDate = payload[2:9] 
      assemblyDate = payload[10:17]
      firmwareDate = payload[18:25]
      print("Hardware Ver = ")
      print(hardwareVer)
      

class PacketRx:
    buffer = bytearray([])
    length = 0
    command = 0x00
    commandEnum = Command(0x00)
    mode = 0x00
    payload = bytearray([])
    checksum = 0x00

      
    def decode(self, data):
      datalen = len(data)
      if data[0] == 0x3d:
        length   = (data[1] << 8) | data[2]
        command  = data[3]
        mode     = data[4]
        payload  = data[5:5+(length - 2)]
        checksum = data[5+(length - 2)]
        busses = command & 0xf0
        print("{:02x} busses = ".format(busses))
       
        print("{:02x} srcbus = ".format(command >> 6))
       # print("{:02x} tgtbus = ".format(command >> 4))
        srcBus = Bus((command >> 6))
        tgtBus = Bus((command >> 4) & 0x3)
        print("{:02x}".format(command).upper() + " command")
        print("{:02x}".format(mode).upper() + " mode")
        print(" ".join(["{:02x}".format(x) for x in payload]).upper() + ": receive payload")
        print("{:02x}".format(checksum).upper() + " checksum")
        print("{:02x}  srcBus = ".format(srcBus.value) + Bus(srcBus).name)
        print("{:02x}  tgtBus = ".format(tgtBus.value) + Bus(tgtBus).name)

        if command & 0x80:
          command ^= 0x80
          print("{:02x}".format(command).upper() + " = command enum :  " + Command(command).name)
          commandEnum = Command(command)
          if commandEnum == Command.response:
            if ((command & 0x05) == 0x05):
              if (mode == 0x01):
                decodeHwinfo(payload)


class PacketTx:
    buffer = bytearray([])
    length = 0x0000
    bus = 0x00
    command = 0x00
    subcommand = 0x00
    mode = 0x00
    payload = bytearray([])
    checksum = 0x00
    srcBus = Bus
    tgtBus = Bus

    def buildPacket(self, request):
            length = len(request)
            packet_length_hb = (length >> 8) & 0xFF
            packet_length_lb = length & 0xFF
            packetTX = bytearray([0x3D, packet_length_hb, packet_length_lb ])
            packetTX.extend(request)

            command = request[0]
            subcommand = request[1]
            
            print(" ".join(["{:02x}".format(x) for x in request]).upper() + ": request payload")
            payload = bytearray([])
            payloadPos = 2

            requestArray=bytearray(request)
            print("Request is type : " + str(type(request)))
            print("Payload is type : " + str(type(payload)))
            print("PayloadPos is type : " + str(type(payloadPos)))
            print("RequestArray is type : " + str(type(requestArray)))
            payload = requestArray[2:(length - 2)]
#            while payloadPos < (length - 2):
#                thisByte=requestArray[payloadPos]
#                payload.extend(thisByte)
#                print("Current pos = " + payloadPos)
#                payloadPos += 1

            # Checksum is a simple byte-adder discarding overflow
            checksum = 0
            i = 0
            while i < len(packetTX):
                checksum += packetTX[i]
                i += 1

            checksum = (checksum & 0xFF).to_bytes(1, 'little')
            packetTX.extend(checksum)
            return packetTX

