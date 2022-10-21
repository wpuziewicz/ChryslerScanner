﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ChryslerScanner
{
    public class SCIPCM
    {
        public SCIPCMDiagnosticsTable Diagnostics = new SCIPCMDiagnosticsTable();
        public DataTable MessageDatabase = new DataTable("PCMDatabase");
        public DataTable EngineDTC = new DataTable("EngineDTC");
        public List<byte> engineFaultCodeList = new List<byte>();
        public bool engineFaultCodesSaved = true;
        public ushort[] IDList;
        public byte[] engineDTCList;
        public DataColumn column;
        public DataRow row;

        private const int hexBytesColumnStart = 2;
        private const int descriptionColumnStart = 28;
        private const int valueColumnStart = 82;
        private const int unitColumnStart = 108;

        public string state = null;
        public string speed = null;
        public string logic = null;
        public string configuration = null;

        public string HeaderUnknown  = "│ SCI-BUS ENGINE          │ STATE: N/A                                                                                   ";
        public string HeaderDisabled = "│ SCI-BUS ENGINE          │ STATE: DISABLED                                                                              ";
        public string HeaderEnabled  = "│ SCI-BUS ENGINE          │ STATE: ENABLED @ BAUD | LOGIC: | CONFIGURATION:                                              ";
        public string EmptyLine      = "│                         │                                                     │                         │             │";
        public string HeaderModified = string.Empty;

        public string LastROMInfoText = string.Empty;

        public SCIPCM()
        {
            column = new DataColumn();
            column.DataType = typeof(ushort);
            column.ColumnName = "id";
            column.ReadOnly = true;
            column.Unique = true;
            MessageDatabase.Columns.Add(column);

            column = new DataColumn();
            column.DataType = typeof(byte);
            column.ColumnName = "length";
            column.ReadOnly = true;
            column.Unique = false;
            MessageDatabase.Columns.Add(column);

            column = new DataColumn();
            column.DataType = typeof(byte);
            column.ColumnName = "parameterCount";
            column.ReadOnly = true;
            column.Unique = false;
            MessageDatabase.Columns.Add(column);

            column = new DataColumn();
            column.DataType = typeof(string);
            column.ColumnName = "message";
            column.ReadOnly = false;
            column.Unique = false;
            MessageDatabase.Columns.Add(column);

            column = new DataColumn();
            column.DataType = typeof(string);
            column.ColumnName = "description";
            column.ReadOnly = false;
            column.Unique = false;
            MessageDatabase.Columns.Add(column);

            column = new DataColumn();
            column.DataType = typeof(string);
            column.ColumnName = "value";
            column.ReadOnly = false;
            column.Unique = false;
            MessageDatabase.Columns.Add(column);

            column = new DataColumn();
            column.DataType = typeof(string);
            column.ColumnName = "unit";
            column.ReadOnly = false;
            column.Unique = false;
            MessageDatabase.Columns.Add(column);

            DataColumn[] PrimaryKeyColumns = new DataColumn[1];
            PrimaryKeyColumns[0] = MessageDatabase.Columns["id"];
            MessageDatabase.PrimaryKey = PrimaryKeyColumns;

            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(MessageDatabase);

            #region SCI-bus (PCM) messages

            row = MessageDatabase.NewRow();
            row["id"] = 0x00;
            row["length"] = 1;
            row["parameterCount"] = 0;
            row["message"] = string.Empty;
            row["description"] = "PCM WAKE UP";
            row["value"] = string.Empty;
            row["unit"] = string.Empty;
            MessageDatabase.Rows.Add(row);

            row = MessageDatabase.NewRow();
            row["id"] = 0x06;
            row["length"] = 1;
            row["parameterCount"] = 0;
            row["message"] = string.Empty;
            row["description"] = "SET BOOTSTRAP BAUDRATE TO 62500 BAUD";
            row["value"] = "SUCCESS";
            row["unit"] = string.Empty;
            MessageDatabase.Rows.Add(row);

            row = MessageDatabase.NewRow();
            row["id"] = 0x10;
            row["length"] = 3;
            row["parameterCount"] = 1;
            row["message"] = string.Empty;
            row["description"] = "ENGINE FAULT CODE LIST";
            row["value"] = string.Empty;
            row["unit"] = string.Empty;
            MessageDatabase.Rows.Add(row);

            row = MessageDatabase.NewRow();
            row["id"] = 0x11;
            row["length"] = 2;
            row["parameterCount"] = 1;
            row["message"] = string.Empty;
            row["description"] = "FAULT BIT LIST";
            row["value"] = string.Empty;
            row["unit"] = string.Empty;
            MessageDatabase.Rows.Add(row);

            row = MessageDatabase.NewRow();
            row["id"] = 0x12;
            row["length"] = 1;
            row["parameterCount"] = 1;
            row["message"] = string.Empty;
            row["description"] = "SELECT HIGH-SPEED MODE";
            row["value"] = string.Empty;
            row["unit"] = string.Empty;
            MessageDatabase.Rows.Add(row);

            row = MessageDatabase.NewRow();
            row["id"] = 0x13;
            row["length"] = 3;
            row["parameterCount"] = 1;
            row["message"] = string.Empty;
            row["description"] = "ACTUATOR TEST";
            row["value"] = string.Empty;
            row["unit"] = string.Empty;
            MessageDatabase.Rows.Add(row);

            row = MessageDatabase.NewRow();
            row["id"] = 0x14;
            row["length"] = 3;
            row["parameterCount"] = 1;
            row["message"] = string.Empty;
            row["description"] = "REQUEST DIAGNOSTIC DATA";
            row["value"] = string.Empty;
            row["unit"] = string.Empty;
            MessageDatabase.Rows.Add(row);

            row = MessageDatabase.NewRow();
            row["id"] = 0x15;
            row["length"] = 4;
            row["parameterCount"] = 1;
            row["message"] = string.Empty;
            row["description"] = "ROM VALUE";
            row["value"] = string.Empty;
            row["unit"] = string.Empty;
            MessageDatabase.Rows.Add(row);

            row = MessageDatabase.NewRow();
            row["id"] = 0x16;
            row["length"] = 3;
            row["parameterCount"] = 1;
            row["message"] = string.Empty;
            row["description"] = "ROM CONSTANT VALUE";
            row["value"] = string.Empty;
            row["unit"] = string.Empty;
            MessageDatabase.Rows.Add(row);

            row = MessageDatabase.NewRow();
            row["id"] = 0x17;
            row["length"] = 2;
            row["parameterCount"] = 1;
            row["message"] = string.Empty;
            row["description"] = "ERASE FAULT CODES";
            row["value"] = string.Empty;
            row["unit"] = string.Empty;
            MessageDatabase.Rows.Add(row);

            row = MessageDatabase.NewRow();
            row["id"] = 0x18;
            row["length"] = 3;
            row["parameterCount"] = 1;
            row["message"] = string.Empty;
            row["description"] = "CONTROL ASD RELAY";
            row["value"] = string.Empty;
            row["unit"] = string.Empty;
            MessageDatabase.Rows.Add(row);

            row = MessageDatabase.NewRow();
            row["id"] = 0x19;
            row["length"] = 2;
            row["parameterCount"] = 1;
            row["message"] = string.Empty;
            row["description"] = "SET IDLE SPEED";
            row["value"] = string.Empty;
            row["unit"] = string.Empty;
            MessageDatabase.Rows.Add(row);

            row = MessageDatabase.NewRow();
            row["id"] = 0x1A;
            row["length"] = 3;
            row["parameterCount"] = 1;
            row["message"] = string.Empty;
            row["description"] = "SWITCH TEST";
            row["value"] = string.Empty;
            row["unit"] = string.Empty;
            MessageDatabase.Rows.Add(row);

            row = MessageDatabase.NewRow();
            row["id"] = 0x1B;
            row["length"] = 1;
            row["parameterCount"] = 1;
            row["message"] = string.Empty;
            row["description"] = "INIT BYTE MODE DOWNLOAD";
            row["value"] = string.Empty;
            row["unit"] = string.Empty;
            MessageDatabase.Rows.Add(row);

            row = MessageDatabase.NewRow();
            row["id"] = 0x1C;
            row["length"] = 4;
            row["parameterCount"] = 1;
            row["message"] = string.Empty;
            row["description"] = "WRITE MEMORY";
            row["value"] = string.Empty;
            row["unit"] = string.Empty;
            MessageDatabase.Rows.Add(row);

            row = MessageDatabase.NewRow();
            row["id"] = 0x1F;
            row["length"] = 4;
            row["parameterCount"] = 1;
            row["message"] = string.Empty;
            row["description"] = "WRITE RAM";
            row["value"] = string.Empty;
            row["unit"] = string.Empty;
            MessageDatabase.Rows.Add(row);

            row = MessageDatabase.NewRow();
            row["id"] = 0x21;
            row["length"] = 3;
            row["parameterCount"] = 1;
            row["message"] = string.Empty;
            row["description"] = "SET SYNC / TIMING / SPARK SCATTER";
            row["value"] = string.Empty;
            row["unit"] = string.Empty;
            MessageDatabase.Rows.Add(row);

            row = MessageDatabase.NewRow();
            row["id"] = 0x22;
            row["length"] = 3;
            row["parameterCount"] = 1;
            row["message"] = string.Empty;
            row["description"] = "SEND ENGINE PARAMETERS";
            row["value"] = string.Empty;
            row["unit"] = string.Empty;
            MessageDatabase.Rows.Add(row);

            row = MessageDatabase.NewRow();
            row["id"] = 0x23;
            row["length"] = 3;
            row["parameterCount"] = 1;
            row["message"] = string.Empty;
            row["description"] = "RESET MEMORY";
            row["value"] = string.Empty;
            row["unit"] = string.Empty;
            MessageDatabase.Rows.Add(row);

            row = MessageDatabase.NewRow();
            row["id"] = 0x25;
            row["length"] = 4;
            row["parameterCount"] = 1;
            row["message"] = string.Empty;
            row["description"] = "WRITE ROM SETTING";
            row["value"] = string.Empty;
            row["unit"] = string.Empty;
            MessageDatabase.Rows.Add(row);

            row = MessageDatabase.NewRow();
            row["id"] = 0x26;
            row["length"] = 5;
            row["parameterCount"] = 1;
            row["message"] = string.Empty;
            row["description"] = "READ FLASH MEMORY";
            row["value"] = string.Empty;
            row["unit"] = string.Empty;
            MessageDatabase.Rows.Add(row);

            row = MessageDatabase.NewRow();
            row["id"] = 0x27;
            row["length"] = 5;
            row["parameterCount"] = 1;
            row["message"] = string.Empty;
            row["description"] = "WRITE EEPROM";
            row["value"] = string.Empty;
            row["unit"] = string.Empty;
            MessageDatabase.Rows.Add(row);

            row = MessageDatabase.NewRow();
            row["id"] = 0x28;
            row["length"] = 4;
            row["parameterCount"] = 1;
            row["message"] = string.Empty;
            row["description"] = "READ EEPROM";
            row["value"] = string.Empty;
            row["unit"] = string.Empty;
            MessageDatabase.Rows.Add(row);

            row = MessageDatabase.NewRow();
            row["id"] = 0x29;
            row["length"] = 5;
            row["parameterCount"] = 1;
            row["message"] = string.Empty;
            row["description"] = "WRITE RAM";
            row["value"] = string.Empty;
            row["unit"] = string.Empty;
            MessageDatabase.Rows.Add(row);

            row = MessageDatabase.NewRow();
            row["id"] = 0x2A;
            row["length"] = 3;
            row["parameterCount"] = 1;
            row["message"] = string.Empty;
            row["description"] = "INFORMATION REQUEST";
            row["value"] = string.Empty;
            row["unit"] = string.Empty;
            MessageDatabase.Rows.Add(row);

            row = MessageDatabase.NewRow();
            row["id"] = 0x2B;
            row["length"] = 4;
            row["parameterCount"] = 1;
            row["message"] = string.Empty;
            row["description"] = "GET SECURITY SEED";
            row["value"] = string.Empty;
            row["unit"] = string.Empty;
            MessageDatabase.Rows.Add(row);

            row = MessageDatabase.NewRow();
            row["id"] = 0x2C;
            row["length"] = 5;
            row["parameterCount"] = 1;
            row["message"] = string.Empty;
            row["description"] = "SEND SECURITY KEY";
            row["value"] = string.Empty;
            row["unit"] = string.Empty;
            MessageDatabase.Rows.Add(row);

            row = MessageDatabase.NewRow();
            row["id"] = 0x2D;
            row["length"] = 3;
            row["parameterCount"] = 1;
            row["message"] = string.Empty;
            row["description"] = "MIN/MAX ENGINE PARAMETER VALUE";
            row["value"] = string.Empty;
            row["unit"] = string.Empty;
            MessageDatabase.Rows.Add(row);

            row = MessageDatabase.NewRow();
            row["id"] = 0x2E;
            row["length"] = 2;
            row["parameterCount"] = 1;
            row["message"] = string.Empty;
            row["description"] = "ONE-TRIP FAULT CODES";
            row["value"] = string.Empty;
            row["unit"] = string.Empty;
            MessageDatabase.Rows.Add(row);

            row = MessageDatabase.NewRow();
            row["id"] = 0x31;
            row["length"] = 6;
            row["parameterCount"] = 1;
            row["message"] = string.Empty;
            row["description"] = "WRITE FLASH BLOCK";
            row["value"] = string.Empty;
            row["unit"] = string.Empty;
            MessageDatabase.Rows.Add(row);

            row = MessageDatabase.NewRow();
            row["id"] = 0x32;
            row["length"] = 2;
            row["parameterCount"] = 1;
            row["message"] = string.Empty;
            row["description"] = "ENGINE FAULT CODES";
            row["value"] = string.Empty;
            row["unit"] = string.Empty;
            MessageDatabase.Rows.Add(row);

            row = MessageDatabase.NewRow();
            row["id"] = 0x34;
            row["length"] = 6;
            row["parameterCount"] = 1;
            row["message"] = string.Empty;
            row["description"] = "READ FLASH BLOCK";
            row["value"] = string.Empty;
            row["unit"] = string.Empty;
            MessageDatabase.Rows.Add(row);

            row = MessageDatabase.NewRow();
            row["id"] = 0x35;
            row["length"] = 5;
            row["parameterCount"] = 1;
            row["message"] = string.Empty;
            row["description"] = "GET SECURITY SEED";
            row["value"] = string.Empty;
            row["unit"] = string.Empty;
            MessageDatabase.Rows.Add(row);

            row = MessageDatabase.NewRow();
            row["id"] = 0x37;
            row["length"] = 6;
            row["parameterCount"] = 1;
            row["message"] = string.Empty;
            row["description"] = "WRITE EEPROM BLOCK";
            row["value"] = string.Empty;
            row["unit"] = string.Empty;
            MessageDatabase.Rows.Add(row);

            row = MessageDatabase.NewRow();
            row["id"] = 0x3A;
            row["length"] = 5;
            row["parameterCount"] = 1;
            row["message"] = string.Empty;
            row["description"] = "READ EEPROM BLOCK";
            row["value"] = string.Empty;
            row["unit"] = string.Empty;
            MessageDatabase.Rows.Add(row);

            row = MessageDatabase.NewRow();
            row["id"] = 0x46;
            row["length"] = 7;
            row["parameterCount"] = 2;
            row["message"] = string.Empty;
            row["description"] = "READ FLASH BLOCK";
            row["value"] = string.Empty;
            row["unit"] = string.Empty;
            MessageDatabase.Rows.Add(row);

            row = MessageDatabase.NewRow();
            row["id"] = 0x47;
            row["length"] = 4;
            row["parameterCount"] = 1;
            row["message"] = string.Empty;
            row["description"] = "START BOOTLOADER";
            row["value"] = string.Empty;
            row["unit"] = string.Empty;
            MessageDatabase.Rows.Add(row);

            row = MessageDatabase.NewRow();
            row["id"] = 0x4C;
            row["length"] = 5;
            row["parameterCount"] = 2;
            row["message"] = string.Empty;
            row["description"] = "UPLOAD BOOTLOADER";
            row["value"] = string.Empty;
            row["unit"] = string.Empty;
            MessageDatabase.Rows.Add(row);

            row = MessageDatabase.NewRow();
            row["id"] = 0xDB;
            row["length"] = 5;
            row["parameterCount"] = 1;
            row["message"] = string.Empty;
            row["description"] = "BOOTSTRAP MODE NOT PROTECTED";
            row["value"] = string.Empty;
            row["unit"] = string.Empty;
            MessageDatabase.Rows.Add(row);

            row = MessageDatabase.NewRow();
            row["id"] = 0xF0;
            row["length"] = 3;
            row["parameterCount"] = 1;
            row["message"] = string.Empty;
            row["description"] = "F0 RAM TABLE VALUE(S)";
            row["value"] = string.Empty;
            row["unit"] = string.Empty;
            MessageDatabase.Rows.Add(row);

            row = MessageDatabase.NewRow();
            row["id"] = 0xF1;
            row["length"] = 3;
            row["parameterCount"] = 1;
            row["message"] = string.Empty;
            row["description"] = "F1 RAM TABLE TABLE VALUE(S)";
            row["value"] = string.Empty;
            row["unit"] = string.Empty;
            MessageDatabase.Rows.Add(row);

            row = MessageDatabase.NewRow();
            row["id"] = 0xF2;
            row["length"] = 3;
            row["parameterCount"] = 1;
            row["message"] = string.Empty;
            row["description"] = "F2 RAM TABLE VALUE(S)";
            row["value"] = string.Empty;
            row["unit"] = string.Empty;
            MessageDatabase.Rows.Add(row);

            row = MessageDatabase.NewRow();
            row["id"] = 0xF3;
            row["length"] = 3;
            row["parameterCount"] = 1;
            row["message"] = string.Empty;
            row["description"] = "F3 RAM TABLE VALUE(S)";
            row["value"] = string.Empty;
            row["unit"] = string.Empty;
            MessageDatabase.Rows.Add(row);

            row = MessageDatabase.NewRow();
            row["id"] = 0xF4;
            row["length"] = 3;
            row["parameterCount"] = 1;
            row["message"] = string.Empty;
            row["description"] = "F4 RAM TABLE VALUE(S)";
            row["value"] = string.Empty;
            row["unit"] = string.Empty;
            MessageDatabase.Rows.Add(row);

            row = MessageDatabase.NewRow();
            row["id"] = 0xF5;
            row["length"] = 3;
            row["parameterCount"] = 1;
            row["message"] = string.Empty;
            row["description"] = "F5 RAM TABLE VALUE(S)";
            row["value"] = string.Empty;
            row["unit"] = string.Empty;
            MessageDatabase.Rows.Add(row);

            row = MessageDatabase.NewRow();
            row["id"] = 0xF6;
            row["length"] = 3;
            row["parameterCount"] = 1;
            row["message"] = string.Empty;
            row["description"] = "F6 RAM TABLE VALUE(S)";
            row["value"] = string.Empty;
            row["unit"] = string.Empty;
            MessageDatabase.Rows.Add(row);

            row = MessageDatabase.NewRow();
            row["id"] = 0xF7;
            row["length"] = 3;
            row["parameterCount"] = 1;
            row["message"] = string.Empty;
            row["description"] = "F7 RAM TABLE VALUE(S)";
            row["value"] = string.Empty;
            row["unit"] = string.Empty;
            MessageDatabase.Rows.Add(row);

            row = MessageDatabase.NewRow();
            row["id"] = 0xF8;
            row["length"] = 3;
            row["parameterCount"] = 1;
            row["message"] = string.Empty;
            row["description"] = "F8 RAM TABLE VALUE(S)";
            row["value"] = string.Empty;
            row["unit"] = string.Empty;
            MessageDatabase.Rows.Add(row);

            row = MessageDatabase.NewRow();
            row["id"] = 0xF9;
            row["length"] = 3;
            row["parameterCount"] = 1;
            row["message"] = string.Empty;
            row["description"] = "F9 RAM TABLE VALUE(S)";
            row["value"] = string.Empty;
            row["unit"] = string.Empty;
            MessageDatabase.Rows.Add(row);

            row = MessageDatabase.NewRow();
            row["id"] = 0xFA;
            row["length"] = 3;
            row["parameterCount"] = 1;
            row["message"] = string.Empty;
            row["description"] = "FA RAM TABLE VALUE(S)";
            row["value"] = string.Empty;
            row["unit"] = string.Empty;
            MessageDatabase.Rows.Add(row);

            row = MessageDatabase.NewRow();
            row["id"] = 0xFB;
            row["length"] = 3;
            row["parameterCount"] = 1;
            row["message"] = string.Empty;
            row["description"] = "FB RAM TABLE VALUE(S)";
            row["value"] = string.Empty;
            row["unit"] = string.Empty;
            MessageDatabase.Rows.Add(row);

            row = MessageDatabase.NewRow();
            row["id"] = 0xFC;
            row["length"] = 3;
            row["parameterCount"] = 1;
            row["message"] = string.Empty;
            row["description"] = "FC RAM TABLE VALUE(S)";
            row["value"] = string.Empty;
            row["unit"] = string.Empty;
            MessageDatabase.Rows.Add(row);

            row = MessageDatabase.NewRow();
            row["id"] = 0xFD;
            row["length"] = 3;
            row["parameterCount"] = 1;
            row["message"] = string.Empty;
            row["description"] = "FD RAM TABLE VALUE(S)";
            row["value"] = string.Empty;
            row["unit"] = string.Empty;
            MessageDatabase.Rows.Add(row);

            row = MessageDatabase.NewRow();
            row["id"] = 0xFE;
            row["length"] = 1;
            row["parameterCount"] = 1;
            row["message"] = string.Empty;
            row["description"] = "SELECT LOW-SPEED MODE";
            row["value"] = string.Empty;
            row["unit"] = string.Empty;
            MessageDatabase.Rows.Add(row);

            row = MessageDatabase.NewRow();
            row["id"] = 0xFF;
            row["length"] = 1;
            row["parameterCount"] = 1;
            row["message"] = string.Empty;
            row["description"] = "PCM WAKE UP";
            row["value"] = string.Empty;
            row["unit"] = string.Empty;
            MessageDatabase.Rows.Add(row);

            #endregion

            IDList = MessageDatabase.AsEnumerable().Select(r => r.Field<ushort>("id")).ToArray();

            column = new DataColumn();
            column.DataType = typeof(byte);
            column.ColumnName = "id";
            column.ReadOnly = true;
            column.Unique = true;
            EngineDTC.Columns.Add(column);

            column = new DataColumn();
            column.DataType = typeof(string);
            column.ColumnName = "description";
            column.ReadOnly = true;
            column.Unique = false;
            EngineDTC.Columns.Add(column);

            DataColumn[] PrimaryKeyColumnsDTC = new DataColumn[1];
            PrimaryKeyColumnsDTC[0] = EngineDTC.Columns["id"];
            EngineDTC.PrimaryKey = PrimaryKeyColumnsDTC;

            DataSet dataSetDTC = new DataSet();
            dataSetDTC.Tables.Add(EngineDTC);

            #region SCI-bus (PCM) fault codes

            row = EngineDTC.NewRow();
            row["id"] = 0x00;
            row["description"] = "UNRECOGNIZED DTC";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x01;
            row["description"] = "NO CAM SIGNAL AT PCM";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x02;
            row["description"] = "INTERNAL CONTROLLER FAILURE";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x03;
            row["description"] = "LEFT BANK O2 SENSOR STAYS ABOVE CENTER (RICH)";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x04;
            row["description"] = "LEFT BANK O2 SENSOR STAYS BELOW CENTER (LEAN)";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x05;
            row["description"] = "CHARGING SYSTEM VOLTAGE TOO LOW";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x06;
            row["description"] = "CHARGING SYSTEM VOLTAGE TOO HIGH";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x07;
            row["description"] = "TURBO BOOST LIMIT EXCEEDED";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x08;
            row["description"] = "RIGHT BANK O2 SENSOR STAYS ABOVE CENTER (RICH)";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x09;
            row["description"] = "RIGHT BANK O2 SENSOR STAYS BELOW CENTER (LEAN)";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x0A;
            row["description"] = "AUTO SHUTDOWN RELAY CONTROL CIRCUIT";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x0B;
            row["description"] = "GENERATOR FIELD NOT SWITCHING PROPERLY";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x0C;
            row["description"] = "TORQUE CONVERTER CLUTCH SOLENOID / TRANS RELAY CIRCUITS";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x0D;
            row["description"] = "TURBOCHARGER WASTEGATE SOLENOID CIRCUIT";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x0E;
            row["description"] = "LOW SPEED FAN CONTROL RELAY CIRCUIT";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x0F;
            row["description"] = "SPEED CONTROL SOLENOID CIRCUITS";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x10;
            row["description"] = "A/C CLUTCH RELAY CIRCUIT";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x11;
            row["description"] = "EGR SOLENOID CIRCUIT";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x12;
            row["description"] = "EVAP PURGE SOLENOID CIRCUIT";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x13;
            row["description"] = "INJECTOR #3 CONTROL CIRCUIT";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x14;
            row["description"] = "INJECTOR #2 CONTROL CIRCUIT";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x15;
            row["description"] = "INJECTOR #1 CONTROL CIRCUIT";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x16;
            row["description"] = "INJECTOR #3 PEAK CURRENT NOT REACHED";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x17;
            row["description"] = "INJECTOR #2 PEAK CURRENT NOT REACHED";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x18;
            row["description"] = "INJECTOR #1 PEAK CURRENT NOT REACHED";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x19;
            row["description"] = "IDLE AIR CONTROL MOTOR CIRCUITS";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x1A;
            row["description"] = "THROTTLE POSITION SENSOR VOLTAGE LOW";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x1B;
            row["description"] = "THROTTLE POSITION SENSOR VOLTAGE HIGH";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x1C;
            row["description"] = "THROTTLE BODY TEMP SENSOR VOLTAGE LOW";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x1D;
            row["description"] = "THROTTLE BODY TEMP SENSOR VOLTAGE HIGH";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x1E;
            row["description"] = "COOLANT TEMPERATURE SENSOR VOLTAGE TOO LOW";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x1F;
            row["description"] = "COOLANT TEMPERATURE SENSOR VOLTAGE TOO HIGH";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x20;
            row["description"] = "UPSTREAM O2 SENSOR STAYS AT CENTER";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x21;
            row["description"] = "ENGINE IS COLD TOO LONG";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x22;
            row["description"] = "SKIP SHIFT SOLENOID CIRCUIT";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x23;
            row["description"] = "NO VEHICLE SPEED SENSOR SIGNAL";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x24;
            row["description"] = "MAP SENSOR VOLTAGE TOO LOW";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x25;
            row["description"] = "MAP SENSOR VOLTAGE TOO HIGH";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x26;
            row["description"] = "SLOW CHANGE IN IDLE MAP SENSOR SIGNAL";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x27;
            row["description"] = "NO CHANGE IN MAP FROM START TO RUN";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x28;
            row["description"] = "NO CRANKSHAFT REFERENCE SIGNAL AT PCM";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x29;
            row["description"] = "IGNITION COIL #3 PRIMARY CIRCUIT";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x2A;
            row["description"] = "IGNITION COIL #2 PRIMARY CIRCUIT";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x2B;
            row["description"] = "IGNITION COIL #1 PRIMARY CIRCUIT";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x2C;
            row["description"] = "NO ASD RELAY OUTPUT VOLTAGE AT PCM";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x2D;
            row["description"] = "SYSTEM RICH, L-IDLE ADAPTIVE AT LEAN LIMIT";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x2E;
            row["description"] = "EGR SYSTEM FAILURE";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x2F;
            row["description"] = "BAROMETRIC READ SOLENOID CIRCUIT";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x30;
            row["description"] = "PCM FAILURE SRI MILE NOT STORED";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x31;
            row["description"] = "PCM FAILURE EEPROM WRITE DENIED";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x32;
            row["description"] = "TRANSMISSION 3-4 SHIFT SOLENOID / TRANSMISSION RELAY CIRCUITS";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x33;
            row["description"] = "SECONDARY AIR SOLENOID CIRCUIT";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x34;
            row["description"] = "IDLE SWITCH SHORTED TO GROUND";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x35;
            row["description"] = "IDLE SWITCH OPEN CIRCUIT";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x36;
            row["description"] = "SURGE VALVE SOLENOID CIRCUIT";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x37;
            row["description"] = "INJECTOR #9 CONTROL CIRCUIT";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x38;
            row["description"] = "INJECTOR #10 CONTROL CIRCUIT";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x39;
            row["description"] = "INTAKE AIR TEMPERATURE SENSOR VOLTAGE LOW";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x3A;
            row["description"] = "INTAKE AIR TEMPERATURE SENSOR VOLTAGE HIGH";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x3B;
            row["description"] = "KNOCK SENSOR CIRCUIT";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x3C;
            row["description"] = "BAROMETRIC PRESSURE OUT OF RANGE";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x3D;
            row["description"] = "INJECTOR #4 CONTROL CIRCUIT";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x3E;
            row["description"] = "LEFT BANK UPSTREAM O2 SENSOR SHORTED TO VOLTAGE";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x3F;
            row["description"] = "FUEL SYSTEM RICH, R-IDLE ADAPTIVE AT LEAN LIMIT";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x40;
            row["description"] = "WASTEGATE #2 CIRCUIT";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x41;
            row["description"] = "RIGHT BANK UPSTREAM O2 SENSOR STAYS AT CENTER";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x42;
            row["description"] = "RIGHT BANK UPSTREAM O2 SENSOR SHORTED TO VOLTAGE";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x43;
            row["description"] = "FUEL SYSTEM LEAN, R-IDLE ADAPTIVE AT RICH LIMIT";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x44;
            row["description"] = "PCM FAILURE SPI COMMUNICATIONS";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x45;
            row["description"] = "INJECTOR #5 CONTROL CIRCUIT";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x46;
            row["description"] = "INJECTOR #6 CONTROL CIRCUIT";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x47;
            row["description"] = "BATTERY TEMPERATURE SENSOR VOLTS OUT OF LIMIT";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x48;
            row["description"] = "NO CMP AT IGNITION / INJ DRIVER MODULE";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x49;
            row["description"] = "NO CKP AT IGNITION/ INJ DRIVER MODULE";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x4A;
            row["description"] = "TRANSMISSION TEMPERATURE SENSOR VOLTAGE TOO LOW";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x4B;
            row["description"] = "TRANSMISSION TEMPERATURE SENSOR VOLTAGE TOO HIGH";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x4C;
            row["description"] = "IGNITION COIL #4 PRIMARY CIRCUIT";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x4D;
            row["description"] = "IGNITION COIL #5 PRIMARY CIRCUIT";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x4E;
            row["description"] = "FUEL SYSTEM LEAN, L-IDLE ADAPTIVE AT RICH LIMIT";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x4F;
            row["description"] = "INJECTOR #7 CONTROL CIRCUIT";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x50;
            row["description"] = "INJECTOR #8 CONTROL CIRCUIT";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x51;
            row["description"] = "FUEL PUMP RESISTOR BYPASS RELAY CIRCUIT";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x52;
            row["description"] = "SPEED CONTROL POWER RELAY; OR S/C 12V DRIVER CIRCUIT";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x53;
            row["description"] = "KNOCK SENSOR #2 CIRCUIT";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x54;
            row["description"] = "FLEX FUEL SENSOR VOLTS TOO HIGH";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x55;
            row["description"] = "FLEX FUEL SENSOR VOLTS TOO LOW";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x56;
            row["description"] = "SPEED CONTROL SWITCH ALWAYS HIGH";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x57;
            row["description"] = "SPEED CONTROL SWITCH ALWAYS LOW";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x58;
            row["description"] = "MANIFOLD TUNE VALVE SOLENOID CIRCUIT";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x59;
            row["description"] = "NO BUS MESSAGES";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x5A;
            row["description"] = "A/C PRESSURE SENSOR VOLTS TOO HIGH";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x5B;
            row["description"] = "A/C PRESSURE SENSOR VOLTS TOO LOW";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x5C;
            row["description"] = "LOW SPEED FAN CONTROL RELAY CIRCUIT";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x5D;
            row["description"] = "HIGH SPEED CONDENSER FAN CTRL RELAY CIRCUIT";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x5E;
            row["description"] = "CNG TEMPERATURE SENSOR VOLTAGE TOO LOW";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x5F;
            row["description"] = "CNG TEMPERATURE SENSOR VOLTAGE TOO HIGH";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x60;
            row["description"] = "NO CCD/PCI BUS MESSAGES FROM TCM";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x61;
            row["description"] = "NO CCD/PCI BUS MESSAGE FROM BCM";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x62;
            row["description"] = "CNG PRESSURE SENSOR VOLTAGE TOO HIGH";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x63;
            row["description"] = "CNG PRESSURE SENSOR VOLTAGE TOO LOW";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x64;
            row["description"] = "LOSS OF FLEX FUEL CALIBRATION SIGNAL";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x65;
            row["description"] = "FUEL PUMP RELAY CONTROL CIRCUIT";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x66;
            row["description"] = "LEFT BANK UPSTREAM O2 SENSOR SLOW RESPONSE";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x67;
            row["description"] = "LEFT BANK UPSTREAM O2 SENSOR HEATER FAILURE";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x68;
            row["description"] = "DOWNSTREAM O2 SENSOR UNABLE TO SWITCH RICH/LEAN";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x69;
            row["description"] = "DOWNSTREAM O2 SENSOR HEATER FAILURE";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x6A;
            row["description"] = "MULTIPLE CYLINDER MISFIRE";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x6B;
            row["description"] = "CYLINDER #1 MISFIRE";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x6C;
            row["description"] = "CYLINDER #2 MISFIRE";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x6D;
            row["description"] = "CYLINDER #3 MISFIRE";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x6E;
            row["description"] = "CYLINDER #4 MISFIRE";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x6F;
            row["description"] = "TOO LITTLE SECONDARY AIR";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x70;
            row["description"] = "CATALYTIC CONVERTER EFFICIENCY FAILURE";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x71;
            row["description"] = "EVAP PURGE FLOW MONITOR FAILURE";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x72;
            row["description"] = "P/N SWITCH STUCK IN PARK OR IN GEAR";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x73;
            row["description"] = "POWER STEERING SWITCH FAILURE";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x74;
            row["description"] = "DESIRED FUEL TIMING ADVANCE NOT REACHED";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x75;
            row["description"] = "LOST FUEL INJECTION TIMING SIGNAL";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x76;
            row["description"] = "LEFT BANK FUEL SYSTEM RICH";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x77;
            row["description"] = "LEFT BANK FUEL SYSTEM LEAN";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x78;
            row["description"] = "RIGHT BANK FUEL SYSTEM RICH";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x79;
            row["description"] = "RIGHT BANK FUEL SYSTEM LEAN";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x7A;
            row["description"] = "RIGHT BANK UPSTREAM O2 SENSOR SLOW RESPONSE";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x7B;
            row["description"] = "RIGHT BANK DOWNSTREAM O2 SENSOR SLOW RESPONSE";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x7C;
            row["description"] = "RIGHT BANK UPSTREAM O2 SENSOR HEATER FAILURE";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x7D;
            row["description"] = "RIGHT BANK DOWNSTREAM O2 SENSOR HEATER FAILURE";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x7E;
            row["description"] = "DOWNSTREAM O2 SENSOR SHORTED TO VOLTAGE";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x7F;
            row["description"] = "RIGHT BANK DOWNSTREAM O2 SENSOR SHORTED TO VOLTAGE";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x80;
            row["description"] = "CLOSED LOOP TEMPERATURE NOT REACHED";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x81;
            row["description"] = "LEFT BANK DOWNSTREAM O2 SENSOR STAYS AT CENTER";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x82;
            row["description"] = "RIGHT BANK DOWNSTREAM O2 SENSOR STAYS AT CENTER";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x83;
            row["description"] = "LEAN OPERATION AT WIDE OPEN THROTTLE";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x84;
            row["description"] = "TPS VOLTAGE DOES NOT AGREE WITH MAP";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x85;
            row["description"] = "TIMING BELT SKIPPED 1 TOOTH OR MORE";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x86;
            row["description"] = "NO 5 VOLTS TO A/C PRESSURE SENSOR";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x87;
            row["description"] = "NO 5 VOLTS TO MAP SENSOR";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x88;
            row["description"] = "NO 5 VOLTS TO TPS";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x89;
            row["description"] = "EATX CONTROLLER DTC PRESENT";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x8A;
            row["description"] = "TARGET IDLE NOT REACHED";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x8B;
            row["description"] = "HIGH SPEED RADIATOR FAN CONTROL RELAY CIRCUIT";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x8C;
            row["description"] = "DIESEL EGR SYSTEM FAILURE";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x8D;
            row["description"] = "GOVERNOR PRESSURE NOT EQUAL TO TARGET @ 15 - 20 PSI";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x8E;
            row["description"] = "GOVERNOR PRESSURE ABOVE 3 PSI IN GEAR WITH 0 MPH";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x8F;
            row["description"] = "STARTER RELAY CONTROL CIRCUIT";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x90;
            row["description"] = "DOWNSTREAM O2 SENSOR SHORTED TO GROUND";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x91;
            row["description"] = "VACUUM LEAK FOUND (IAC FULLY SEATED)";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x92;
            row["description"] = "5 VOLT SUPPLY, OUTPUT TOO LOW";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x93;
            row["description"] = "DOWNSTREAM O2 SENSOR SHORTED TO VOLTAGE";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x94;
            row["description"] = "TORQUE CONVERTER CLUTCH, NO RPM DROP AT LOCKUP";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x95;
            row["description"] = "FUEL LEVEL SENDING UNIT VOLTS TOO LOW";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x96;
            row["description"] = "FUEL LEVEL SENDING UNIT VOLTS TOO HIGH";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x97;
            row["description"] = "FUEL LEVEL UNIT NO CHANGE OVER MILES";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x98;
            row["description"] = "BRAKE SWITCH STUCK PRESSED OR RELEASED";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x99;
            row["description"] = "BATTERY TEMPERATURE SENSOR VOLTS TOO LOW";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x9A;
            row["description"] = "BATTERY TEMPERATURE SENSOR VOLTS TOO HIGH";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x9B;
            row["description"] = "LEFT BANK UPSTREAM O2 SENSOR SHORTED TO GROUND";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x9C;
            row["description"] = "DOWNSTREAM O2 SENSOR SHORTED TO GROUND";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x9D;
            row["description"] = "INTERMITTENT LOSS OF CMP OR CKP";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x9E;
            row["description"] = "TOO MUCH SECONDARY AIR";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0x9F;
            row["description"] = "DOWNSTREAM O2 SENSOR SLOW RESPONSE";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xA0;
            row["description"] = "EVAP LEAK MONITOR SMALL LEAK DETECTED";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xA1;
            row["description"] = "EVAP LEAK MONITOR LARGE LEAK DETECTED";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xA2;
            row["description"] = "NO TEMPERATURE RISE SEEN FROM INTAKE HEATERS";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xA3;
            row["description"] = "WAIT TO START LAMP CIRCUIT";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xA4;
            row["description"] = "TRANSMISSION TEMPERATURE SENSOR, NO TEMPERATURE RISE AFTR START";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xA5;
            row["description"] = "3-4 SHIFT SOLENOID, NO RPM DROP @ 3-4 SHIFT";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xA6;
            row["description"] = "LOW OUTPUT SPEED SENSOR RPM, ABOVE 15 MPH";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xA7;
            row["description"] = "GOVERNOR PRESSURE SENSOR VOLTS TOO LOW";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xA8;
            row["description"] = "GOVERNOR PRESSURE SENSOR VOLTS TOO HIGH";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xA9;
            row["description"] = "GOVERNOR PRESSURE SENSOR OFFSET VOLTS TOO LOW OR HIGH";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xAA;
            row["description"] = "PCM NOT PROGRAMMED";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xAB;
            row["description"] = "GOVERNOR PRESSURE SOLENOID CONTROL / TRANSMISSION RELAY CIRCUITS";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xAC;
            row["description"] = "DOWNSTREAM O2 SENSOR STUCK AT CENTER";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xAD;
            row["description"] = "TRANSMISSION 12 VOLT SUPPLY RELAY CONTROL CIRCUIT";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xAE;
            row["description"] = "CYLINDER #5 MIS-FIRE";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xAF;
            row["description"] = "CYLINDER #6 MIS-FIRE";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xB0;
            row["description"] = "CYLINDER #7 MIS-FIRE";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xB1;
            row["description"] = "CYLINDER #8 MIS-FIRE";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xB2;
            row["description"] = "CYLINDER #9 MIS-FIRE";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xB3;
            row["description"] = "CYLINDER #10 MIS-FIRE";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xB4;
            row["description"] = "RIGHT BANK CATALYST EFFICIENCY FAILURE";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xB5;
            row["description"] = "REAR BANK UPSTREAM O2 SENSOR SHORTED TO GROUND";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xB6;
            row["description"] = "REAR BANK DOWNSTREAM O2 SENSOR SHORTED TO GROUND";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xB7;
            row["description"] = "LEAK DETECTION PUMP SOLENOID CIRCUIT";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xB8;
            row["description"] = "LEAK DETECT PUMP SWITCH OR MECHANICAL FAULT";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xB9;
            row["description"] = "AUXILIARY 5 VOLT SUPPLY OUTPUT TOO LOW";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xBA;
            row["description"] = "MISFIRE ADAPTIVE NUMERATOR AT LIMIT";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xBB;
            row["description"] = "EVAP LEAK MONITOR PINCHED HOSE FOUND";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xBC;
            row["description"] = "O/D SWITCH PRESSED (LOW) MORE THAN 5 MIN";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xBD;
            row["description"] = "DOWNSTREAM O2 SENSOR HEATER FAILURE";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xC5;
            row["description"] = "HIGH SPEED RADIATOR FAN GROUND CONTROL RELAY CIRCUIT";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xC6;
            row["description"] = "ONE OF THE IGNITION COILS DRAWS TOO MUCH CURRENT";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xC7;
            row["description"] = "AW4 TRANSMISSION SHIFT SOLENOID B FUNCTIONAL FAILURE";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xC8;
            row["description"] = "RADIATOR TEMPERATURE SENSOR VOLTS TOO LOW";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xC9;
            row["description"] = "RADIATOR TEMPERATURE SENSOR VOLTS TOO HIGH";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xCA;
            row["description"] = "NO I/P CLUSTER CCD/PCI BUS MESSAGES RECEIVED";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xCB;
            row["description"] = "AW4 TRANSMISSION INTERNAL FAILURE (ROM CHECK)";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xCC;
            row["description"] = "UPSTREAM O2 SENSOR SLOW RESPONSE";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xCD;
            row["description"] = "UPSTREAM O2 SENSOR HEATER FAILURE";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xCE;
            row["description"] = "UPSTREAM O2 SENSOR SHORTED TO VOLTAGE";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xCF;
            row["description"] = "UPSTREAM O2 SENSOR SHORTED TO GROUND";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xD0;
            row["description"] = "NO CAM SYNC SIGNAL AT PCM";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xD1;
            row["description"] = "GLOW PLUG RELAY CONTROL CIRCUIT";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xD2;
            row["description"] = "HIGH SPEED CONDENSER FAN CONTROL RELAY CIRCUIT";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xD3;
            row["description"] = "AW4 TRANSMISSION SHIFT SOLENOID B (2-3) SHORTED TO VOLTAGE (12V)";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xD4;
            row["description"] = "EGR POSITION SENSOR VOLTS TOO LOW";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xD5;
            row["description"] = "EGR POSITION SENSOR VOLTS TOO HIGH";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xD6;
            row["description"] = "NO 5 VOLTS TO EGR POSITION SENSOR";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xD7;
            row["description"] = "EGR POSITION SENSOR RATIONALITY FAILURE";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xD8;
            row["description"] = "IGNITION COIL #6 PRIMARY CIRCUIT";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xD9;
            row["description"] = "INTAKE MANIFOLD SHORT RUNNER SOLENOID CIRCUIT";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xDA;
            row["description"] = "AIR ASSIST INJECTION SOLENOID CIRCUIT";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xDB;
            row["description"] = "CATALYST TEMPERATURE SENSOR VOLTS TOO HIGH";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xDC;
            row["description"] = "CATALYST TEMPERATURE SENSOR VOLTS TOO LOW";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xDD;
            row["description"] = "EATX RPM PULSE PERFORMANCE CONDITION";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xDE;
            row["description"] = "NO BUS MESSAGE RECEIVED FROM COMPANION MODULE";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xDF;
            row["description"] = "MIL FAULT IN COMPANION MODULE";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xE0;
            row["description"] = "COOLANT TEMPERATURE SENSOR PERFORMANCE";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xE1;
            row["description"] = "NO MIC BUS MESSAGE";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xE2;
            row["description"] = "NO SKIM BUS MESSAGE RECEIVED";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xE3;
            row["description"] = "IGNITION COIL #7 PRIMARY CIRCUIT";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xE4;
            row["description"] = "IGNITION COIL #8 PRIMARY CIRCUIT";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xE5;
            row["description"] = "PCV SOLENOID CIRCUIT";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xE6;
            row["description"] = "TRANSMISSION FAN RELAY CIRCUIT";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xE7;
            row["description"] = "TCC OR O/D SOLENOID PERFORMANCE";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xE8;
            row["description"] = "WRONG OR INVALID KEY MESSAGE RECEIVED FROM SKIM";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xEA;
            row["description"] = "AW4 TRANSMISSION SOLENOID A 1-2/3-4 OR TCC SOLENOID C FUNCTIONAL FAIL";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xEB;
            row["description"] = "AW4 TRANSMISSION TCC SOLENOID C SHORTED TO GROUND";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xEC;
            row["description"] = "AW4 TRANSMISSION TCC SOLENOID C SHORTED TO VOLTAGE (12V)";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xED;
            row["description"] = "AW4 TRANSMISSION BATTERY VOLTS SENSE TOO LOW";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xEE;
            row["description"] = "AW4 TRANSMISSION BATTERY VOLTS SENSE TOO HIGH";
            EngineDTC.Rows.Add(row);

            row = EngineDTC.NewRow();
            row["id"] = 0xEF;
            row["description"] = "AISIN AW4 TRANSMISSION DTC PRESENT";
            EngineDTC.Rows.Add(row);

            #endregion

            engineDTCList = EngineDTC.AsEnumerable().Select(r => r.Field<byte>("id")).ToArray();
        }

        public void UpdateHeader(string state = "enabled", string speed = null, string logic = null, string configuration = null)
        {
            if (state != null) this.state = state;
            if (speed != null) this.speed = speed;
            if (logic != null) this.logic = logic;
            if (configuration != null) this.configuration = configuration;

            if ((this.state == "enabled")&& (this.speed != null) && (this.logic != null) && (this.configuration != null))
            {
                HeaderModified = HeaderEnabled.Replace("@ BAUD", "@ " + this.speed.ToUpper()).Replace("LOGIC:", "LOGIC: " + this.logic.ToUpper()).Replace("CONFIGURATION: ", "CONFIGURATION: " + this.configuration);
                HeaderModified = Util.TruncateString(HeaderModified, EmptyLine.Length);
                Diagnostics.UpdateHeader(HeaderModified);
            }
            else if (this.state == "disabled")
            {
                Diagnostics.UpdateHeader(HeaderDisabled);
            }
            else
            {
                Diagnostics.UpdateHeader(HeaderUnknown);
            }
        }

        public void AddMessage(byte[] data)
        {
            if ((data == null) || (data.Length < 5)) return;
            
            StringBuilder rowToAdd = new StringBuilder(EmptyLine); // add empty line first
            string hexBytesToInsert = string.Empty;
            string descriptionToInsert = string.Empty;
            string valueToInsert = string.Empty;
            string unitToInsert = string.Empty;
            byte[] timestamp = new byte[4];
            byte[] message = new byte[] { };
            byte[] payload = new byte[] { };
            byte[] engineFaultCodePayload = new byte[] { };

            if (data.Length > 3)
            {
                Array.Copy(data, 0, timestamp, 0, 4);
            }

            if (data.Length > 4)
            {
                message = new byte[data.Length - 4];
                Array.Copy(data, 4, message, 0, message.Length); // copy message from the input byte array
            }

            if (data.Length > 5)
            {
                payload = new byte[data.Length - 5];
                Array.Copy(data, 5, payload, 0, payload.Length); // copy payload from the input byte array (without ID)
            }

            if (message.Length > 2)
            {
                engineFaultCodePayload = new byte[data.Length - 6];
                Array.Copy(data, 5, engineFaultCodePayload, 0, engineFaultCodePayload.Length); // copy payload from the input byte array (without ID and checksum byte)
            }

            byte ID = message[0];
            ushort modifiedID;

            if ((ID == 0x14)  || (ID == 0x22) || ((ID >= 0xF0) && (ID < 0xFE)))
            {
                if (payload.Length > 0) modifiedID = (ushort)(((ID << 8) & 0xFF00) + payload[0]);
                else modifiedID = (ushort)((ID << 8) & 0xFF00);
            }
            else 
            {
                modifiedID = (ushort)((ID << 8) & 0xFF00);
            }

            int rowIndex = MessageDatabase.Rows.IndexOf(MessageDatabase.Rows.Find(ID)); // search ID byte among known messages and get row index

            if (rowIndex != -1) // row found
            {
                descriptionToInsert = MessageDatabase.Rows[rowIndex]["description"].ToString();

                int minLength = Convert.ToInt32(MessageDatabase.Rows[rowIndex]["length"]);

                if ((speed == "976.5 baud") || (speed == "7812.5 baud"))
                {
                    switch (ID)
                    {
                        case 0x00: // PCM wake up
                            descriptionToInsert = "PCM WAKE UP";
                            valueToInsert = string.Empty;
                            unitToInsert = string.Empty;
                            break;
                        case 0x10: // engine fault code list
                            if (message.Length >= minLength)
                            {
                                byte checksum = 0;
                                int checksumLocation = message.Length - 1;

                                for (int i = 0; i < checksumLocation; i++)
                                {
                                    checksum += message[i];
                                }

                                if (checksum == message[checksumLocation])
                                {
                                    engineFaultCodeList.Clear();
                                    engineFaultCodeList.AddRange(engineFaultCodePayload);
                                    engineFaultCodeList.Remove(0xFD); // not fault code related
                                    engineFaultCodeList.Remove(0xFE); // end of fault code list signifier

                                    if (engineFaultCodeList.Count > 0)
                                    {
                                        valueToInsert = Util.ByteToHexStringSimple(engineFaultCodeList.ToArray());
                                        engineFaultCodesSaved = false;
                                    }
                                    else
                                    {
                                        valueToInsert = "NO FAULT CODES";
                                        engineFaultCodesSaved = false;
                                    }
                                }
                                else
                                {
                                    valueToInsert = "CHECKSUM ERROR";
                                    engineFaultCodesSaved = true;
                                }
                            }
                            else // error
                            {
                                valueToInsert = "ERROR";
                                engineFaultCodesSaved = true;
                            }
                            unitToInsert = string.Empty;
                            break;
                        case 0x11: // fault bit list
                            if (message.Length >= minLength)
                            {
                                valueToInsert = Util.ByteToHexStringSimple(payload);
                            }
                            else // error
                            {
                                valueToInsert = "ERROR";
                            }
                            unitToInsert = string.Empty;
                            break;
                        case 0x12: // select high-speed mode
                            if (message.Length >= minLength)
                            {
                                valueToInsert = string.Empty;
                            }
                            else // error
                            {
                                valueToInsert = "ERROR";
                            }
                            unitToInsert = string.Empty;
                            break;
                        case 0x13: // actuator test
                            if (message.Length >= minLength)
                            {
                                switch (payload[0])
                                {
                                    case 0x00:
                                        descriptionToInsert = "ACTUATOR TEST";
                                        valueToInsert = "STOPPED";
                                        break;
                                    case 0x01:
                                        descriptionToInsert = "ACTUATOR TEST | IGNITION COIL BANK #1";
                                        break;
                                    case 0x02:
                                        descriptionToInsert = "ACTUATOR TEST | IGNITION COIL BANK #2";
                                        break;
                                    case 0x03:
                                        descriptionToInsert = "ACTUATOR TEST | IGNITION COIL BANK #3";
                                        break;
                                    case 0x04:
                                        descriptionToInsert = "ACTUATOR TEST | FUEL INJECTOR BANK #1";
                                        break;
                                    case 0x05:
                                        descriptionToInsert = "ACTUATOR TEST | FUEL INJECTOR BANK #2";
                                        break;
                                    case 0x06:
                                        descriptionToInsert = "ACTUATOR TEST | FUEL INJECTOR BANK #3";
                                        break;
                                    case 0x07:
                                        descriptionToInsert = "ACTUATOR TEST | IDLE AIR CONTROL STEPPER MOTOR";
                                        break;
                                    case 0x08:
                                        descriptionToInsert = "ACTUATOR TEST | RADIATOR FAN RELAY";
                                        break;
                                    case 0x09:
                                        descriptionToInsert = "ACTUATOR TEST | A/C CLUTCH RELAY";
                                        break;
                                    case 0x0A:
                                        descriptionToInsert = "ACTUATOR TEST | AUTOMATIC SHUTDOWN (ASD) RELAY";
                                        break;
                                    case 0x0B:
                                        descriptionToInsert = "ACTUATOR TEST | EVAP PURGE SOLENOID";
                                        break;
                                    case 0x0C:
                                        descriptionToInsert = "ACTUATOR TEST | SPEED CONTROL SOLENOID";
                                        break;
                                    case 0x0D:
                                        descriptionToInsert = "ACTUATOR TEST | ALTERNATOR FIELD";
                                        break;
                                    case 0x0E:
                                        descriptionToInsert = "ACTUATOR TEST | TACHOMETER OUTPUT";
                                        break;
                                    case 0x0F:
                                        descriptionToInsert = "ACTUATOR TEST | TORQUE CONVERTER CLUTCH";
                                        break;
                                    case 0x10:
                                        descriptionToInsert = "ACTUATOR TEST | EGR SOLENOID";
                                        break;
                                    case 0x11:
                                        descriptionToInsert = "ACTUATOR TEST | WASTEGATE SOLENOID";
                                        break;
                                    case 0x12:
                                        descriptionToInsert = "ACTUATOR TEST | BAROMETER SOLENOID";
                                        break;
                                    case 0x14:
                                        descriptionToInsert = "ACTUATOR TEST | ALL SOLENOIDS / RELAYS";
                                        break;
                                    case 0x16:
                                        descriptionToInsert = "ACTUATOR TEST | TRANSMISSION O/D SOLENOID";
                                        break;
                                    case 0x17:
                                        descriptionToInsert = "ACTUATOR TEST | SHIFT INDICATOR LAMP";
                                        break;
                                    case 0x19:
                                        descriptionToInsert = "ACTUATOR TEST | SURGE VALVE SOLENOID";
                                        break;
                                    case 0x1A:
                                        descriptionToInsert = "ACTUATOR TEST | SPEED CONTROL VENT SOLENOID";
                                        break;
                                    case 0x1B:
                                        descriptionToInsert = "ACTUATOR TEST | SPEED CONTROL VACUUM SOLENOID";
                                        break;
                                    case 0x1C:
                                        descriptionToInsert = "ACTUATOR TEST | ASD FUEL SYSTEM";
                                        break;
                                    case 0x1D:
                                        descriptionToInsert = "ACTUATOR TEST | FUEL INJECTOR BANK #4";
                                        break;
                                    case 0x1E:
                                        descriptionToInsert = "ACTUATOR TEST | FUEL INJECTOR BANK #5";
                                        break;
                                    case 0x1F:
                                        descriptionToInsert = "ACTUATOR TEST | FUEL INJECTOR BANK #6";
                                        break;
                                    case 0x23:
                                        descriptionToInsert = "ACTUATOR TEST | IGNITION COIL BANK #4";
                                        break;
                                    case 0x24:
                                        descriptionToInsert = "ACTUATOR TEST | IGNITION COIL BANK #5";
                                        break;
                                    case 0x25:
                                        descriptionToInsert = "ACTUATOR TEST | FUEL INJECTOR BANK #7";
                                        break;
                                    case 0x26:
                                        descriptionToInsert = "ACTUATOR TEST | FUEL INJECTOR BANK #8";
                                        break;
                                    case 0x28:
                                        descriptionToInsert = "ACTUATOR TEST | INTAKE HEATER BANK #1";
                                        break;
                                    case 0x29:
                                        descriptionToInsert = "ACTUATOR TEST | INTAKE HEATER BANK #2";
                                        break;
                                    case 0x2C:
                                        descriptionToInsert = "ACTUATOR TEST | SPEED CONTROL 12 VOLT FEED";
                                        break;
                                    case 0x2D:
                                        descriptionToInsert = "ACTUATOR TEST | INTAKE MANIFOLD TUNE VALVE";
                                        break;
                                    case 0x2E:
                                        descriptionToInsert = "ACTUATOR TEST | LOW SPEED RADIATOR FAN RELAY";
                                        break;
                                    case 0x2F:
                                        descriptionToInsert = "ACTUATOR TEST | HIGH SPEED RADIATOR FAN RELAY";
                                        break;
                                    case 0x30:
                                        descriptionToInsert = "ACTUATOR TEST | FUEL INJECTOR BANK #9";
                                        break;
                                    case 0x31:
                                        descriptionToInsert = "ACTUATOR TEST | FUEL INJECTOR BANK #10";
                                        break;
                                    case 0x32:
                                        descriptionToInsert = "ACTUATOR TEST | 2-3 LOCKOUT SOLENOID";
                                        break;
                                    case 0x33:
                                        descriptionToInsert = "ACTUATOR TEST | FUEL PUMP RELAY";
                                        break;
                                    case 0x3B:
                                        descriptionToInsert = "ACTUATOR TEST | IAC STEPPER MOTOR STEP UP";
                                        break;
                                    case 0x3C:
                                        descriptionToInsert = "ACTUATOR TEST | IAC STEPPER MOTOR STEP DOWN";
                                        break;
                                    case 0x3D:
                                        descriptionToInsert = "ACTUATOR TEST | LEAK DETECTION PUMP SOLENOID";
                                        break;
                                    case 0x40:
                                        descriptionToInsert = "ACTUATOR TEST | O2 SENSOR HEATER RELAY";
                                        break;
                                    case 0x41:
                                        descriptionToInsert = "ACTUATOR TEST | OVERDRIVE LAMP";
                                        break;
                                    case 0x43:
                                        descriptionToInsert = "ACTUATOR TEST | TRANSMISSION 12 VOLT RELAY";
                                        break;
                                    case 0x44:
                                        descriptionToInsert = "ACTUATOR TEST | REVERSE LOCKOUT SOLENOID";
                                        break;
                                    case 0x46:
                                        descriptionToInsert = "ACTUATOR TEST | SHORT RUNNER VALVE";
                                        break;
                                    case 0x49:
                                        descriptionToInsert = "ACTUATOR TEST | WAIT TO START LAMP";
                                        break;
                                    case 0x52:
                                        descriptionToInsert = "ACTUATOR TEST | 1/1 2/1 O2 SENSOR HEATER RELAY";
                                        break;
                                    case 0x53:
                                        descriptionToInsert = "ACTUATOR TEST | 1/2 2/2 O2 SENSOR HEATER RELAY";
                                        break;
                                    case 0x56:
                                        descriptionToInsert = "ACTUATOR TEST | 1/1 O2 SENSOR HEATER RELAY";
                                        break;
                                    case 0x57:
                                        descriptionToInsert = "ACTUATOR TEST | O2 SENSOR HEATER RELAY";
                                        break;
                                    case 0x5A:
                                        descriptionToInsert = "ACTUATOR TEST | RADIATOR FAN SOLENOID";
                                        break;
                                    case 0x5B:
                                        descriptionToInsert = "ACTUATOR TEST | 1/2 O2 SENSOR HEATER RELAY";
                                        break;
                                    case 0x5D:
                                        descriptionToInsert = "ACTUATOR TEST | EXHAUST BRAKE";
                                        break;
                                    case 0x5E:
                                        descriptionToInsert = "ACTUATOR TEST | FUEL CONTROL";
                                        break;
                                    case 0x5F:
                                        descriptionToInsert = "ACTUATOR TEST | PWM RADIATOR FAN";
                                        break;
                                    default:
                                        descriptionToInsert = "ACTUATOR TEST | OFFSET: " + Util.ByteToHexString(payload, 0, 1);
                                        break;
                                }

                                if (payload[0] != 0)
                                {
                                    if (payload[0] == payload[1])
                                    {
                                        valueToInsert = "RUNNING";
                                    }
                                    else
                                    {
                                        valueToInsert = "MODE NOT AVAILABLE";
                                    }
                                }
                            }
                            else // error
                            {
                                descriptionToInsert = "ACTUATOR TEST";
                                valueToInsert = "ERROR";
                            }
                            unitToInsert = string.Empty;
                            break;
                        case 0x14: // request diagnostic data
                            if (message.Length >= minLength)
                            {
                                switch (payload[0])
                                {
                                    case 0x01: // ambient air temperature sensor voltage
                                        descriptionToInsert = "BATTERY/AMBIENT AIR TEMPERATURE SENSOR VOLTAGE";
                                        valueToInsert = Math.Round(payload[1] * 0.0196D, 3).ToString("0.000").Replace(",", ".");
                                        unitToInsert = "V";
                                        break;
                                    case 0x02: // upstream (pre-cat) o2 sensor voltage
                                        descriptionToInsert = "UPSTREAM O2 SENSOR VOLTAGE (PRE-CATALISATOR)";
                                        valueToInsert = Math.Round(payload[1] * 0.0196D, 3).ToString("0.000").Replace(",", ".");
                                        unitToInsert = "V";
                                        break;
                                    case 0x05: // engine coolant temperature
                                        descriptionToInsert = "ENGINE COOLANT TEMPERATURE";

                                        if (Properties.Settings.Default.Units == "imperial")
                                        {
                                            valueToInsert = Math.Round((payload[1] * 1.8D) - 198.4D).ToString("0");
                                            unitToInsert = "°F";
                                        }
                                        else if (Properties.Settings.Default.Units == "metric")
                                        {
                                            valueToInsert = (payload[1] - 128).ToString("0");
                                            unitToInsert = "°C";
                                        }
                                        break;
                                    case 0x06: // engine coolant temperature sensor voltage
                                        descriptionToInsert = "ENGINE COOLANT TEMPERATURE SENSOR VOLTAGE";
                                        valueToInsert = Math.Round(payload[1] * 0.0196D, 3).ToString("0.000").Replace(",", ".");
                                        unitToInsert = "V";
                                        break;
                                    case 0x07: // throttle position sensor voltage
                                        descriptionToInsert = "THROTTLE POSITION SENSOR VOLTAGE";
                                        valueToInsert = Math.Round(payload[1] * 0.0196D, 3).ToString("0.000").Replace(",", ".");
                                        unitToInsert = "V";
                                        break;
                                    case 0x08: // minimum throttle position sensor voltage
                                        descriptionToInsert = "MINIMUM TPS VOLTAGE";
                                        valueToInsert = Math.Round(payload[1] * 0.0196D, 3).ToString("0.000").Replace(",", ".");
                                        unitToInsert = "V";
                                        break;
                                    case 0x09: // knock sensor voltage
                                        descriptionToInsert = "KNOCK SENSOR VOLTAGE";
                                        valueToInsert = Math.Round(payload[1] * 0.0196D, 3).ToString("0.000").Replace(",", ".");
                                        unitToInsert = "V";
                                        break;
                                    case 0x0A: // battery voltage
                                        descriptionToInsert = "BATTERY VOLTAGE";
                                        valueToInsert = Math.Round(payload[1] * 0.0625D, 3).ToString("0.000").Replace(",", ".");
                                        unitToInsert = "V";
                                        break;
                                    case 0x0B: // map value
                                        descriptionToInsert = "INTAKE MANIFOLD ABSOLUTE PRESSURE (MAP)";

                                        if (Properties.Settings.Default.Units == "imperial")
                                        {
                                            valueToInsert = Math.Round(payload[1] * 0.059756D, 1).ToString("0.0").Replace(",", ".");
                                            unitToInsert = "PSI";
                                        }
                                        else if (Properties.Settings.Default.Units == "metric")
                                        {
                                            valueToInsert = Math.Round(payload[1] * 0.059756D * 6.894757D, 1).ToString("0.0").Replace(",", ".");
                                            unitToInsert = "KPA";
                                        }

                                        break;
                                    case 0x0C: // target iac stepper motor position
                                        descriptionToInsert = "TARGET IAC STEPPER MOTOR POSITION";
                                        valueToInsert = payload[1].ToString("0");
                                        unitToInsert = string.Empty;
                                        break;
                                    case 0x0E: // Long Term Fuel Trim Bank 1
                                        descriptionToInsert = "LONG TERM FUEL TRIM BANK 1";
                                        valueToInsert = payload[1].ToString("0");
                                        unitToInsert = string.Empty;
                                        break;
                                    case 0x0F: // barometric pressure sensor
                                        descriptionToInsert = "BAROMETRIC PRESSURE";

                                        if (Properties.Settings.Default.Units == "imperial")
                                        {
                                            valueToInsert = Math.Round(payload[1] * 0.059756D, 1).ToString("0.0").Replace(",", ".");
                                            unitToInsert = "PSI";
                                        }
                                        else if (Properties.Settings.Default.Units == "metric")
                                        {
                                            valueToInsert = Math.Round(payload[1] * 0.059756D * 6.894757D, 1).ToString("0.0").Replace(",", ".");
                                            unitToInsert = "KPA";
                                        }

                                        break;
                                    case 0x10: // minimum air flow
                                        descriptionToInsert = "MINIMUM AIR FLOW TEST";

                                        if (payload[1] == 0) valueToInsert = "STOPPED";
                                        else valueToInsert = "RUNNING";

                                        unitToInsert = string.Empty;
                                        break;
                                    case 0x11: // engine speed
                                        descriptionToInsert = "ENGINE SPEED";
                                        valueToInsert = (payload[1] * 32).ToString("0");
                                        unitToInsert = "RPM";
                                        break;
                                    case 0x12: // sync sense
                                        descriptionToInsert = "CAM/CRANK SYNC STATE";

                                        if (Util.IsBitSet(payload[1], 4)) valueToInsert = "IN-SYNC";
                                        else valueToInsert = "ENGINE STOPPED";
                                        unitToInsert = string.Empty;

                                        break;
                                    case 0x13: // key-on cycles error 1
                                        descriptionToInsert = "KEY-ON CYCLES ERROR 1";
                                        valueToInsert = payload[1].ToString("0");
                                        unitToInsert = string.Empty;
                                        break;
                                    case 0x15: // spark advance
                                        descriptionToInsert = "SPARK ADVANCE";
                                        valueToInsert = Math.Round((payload[1] * 0.5D), 1).ToString("0.0").Replace(",", ".");
                                        unitToInsert = "DEG";
                                        break;
                                    case 0x16: // cylinder 1 retard
                                        descriptionToInsert = "CYLINDER 1 RETARD";
                                        valueToInsert = payload[1].ToString("0");
                                        unitToInsert = string.Empty;
                                        break;
                                    case 0x17: // cylinder 2 retard
                                        descriptionToInsert = "CYLINDER 2 RETARD";
                                        valueToInsert = payload[1].ToString("0");
                                        unitToInsert = string.Empty;
                                        break;
                                    case 0x18: // cylinder 3 retard
                                        descriptionToInsert = "CYLINDER 3 RETARD";
                                        valueToInsert = payload[1].ToString("0");
                                        unitToInsert = string.Empty;
                                        break;
                                    case 0x19: // cylinder 4 retard
                                        descriptionToInsert = "CYLINDER 4 RETARD";
                                        valueToInsert = payload[1].ToString("0");
                                        unitToInsert = string.Empty;
                                        break;
                                    case 0x1A: // target boost
                                        descriptionToInsert = "TARGET BOOST";

                                        if (Properties.Settings.Default.Units == "imperial")
                                        {
                                            valueToInsert = Math.Round(payload[1] * 0.115294117D, 1).ToString("0.0").Replace(",", ".");
                                            unitToInsert = "PSI";
                                        }
                                        else if (Properties.Settings.Default.Units == "metric")
                                        {
                                            valueToInsert = Math.Round((payload[1] * 0.115294117D) * 6.89475729D, 1).ToString("0.0").Replace(",", ".");
                                            unitToInsert = "KPA";
                                        }

                                        break;
                                    case 0x1B: // intake air temperature
                                        descriptionToInsert = "INTAKE AIR TEMPERATURE";

                                        if (Properties.Settings.Default.Units == "imperial")
                                        {
                                            valueToInsert = Math.Round((payload[1] * 1.8D) - 198.4D).ToString("0");
                                            unitToInsert = "°F";
                                        }
                                        else if (Properties.Settings.Default.Units == "metric")
                                        {
                                            valueToInsert = (payload[1] - 128).ToString("0");
                                            unitToInsert = "°C";
                                        }
                                        break;
                                    case 0x1C: // intake air temperature sensor voltage
                                        descriptionToInsert = "INTAKE AIR TEMPERATURE SENSOR VOLTAGE";
                                        valueToInsert = Math.Round(payload[1] * 0.0196D, 3).ToString("0.000").Replace(",", ".");
                                        unitToInsert = "V";
                                        break;
                                    case 0x1D: // cruise target speed
                                        descriptionToInsert = "CRUISE TARGET SPEED";
                                        valueToInsert = payload[1].ToString("0");
                                        unitToInsert = string.Empty;
                                        break;
                                    case 0x1E: // key-on cycles error 2
                                        descriptionToInsert = "KEY-ON CYCLES ERROR 2";
                                        valueToInsert = payload[1].ToString("0");
                                        unitToInsert = string.Empty;
                                        break;
                                    case 0x1F: // key-on cycles error 3
                                        descriptionToInsert = "KEY-ON CYCLES ERROR 3";
                                        valueToInsert = payload[1].ToString("0");
                                        unitToInsert = string.Empty;
                                        break;
                                    case 0x20: // speed control status
                                        string lastCruiseCutoutReason = string.Empty;
                                        string cruiseDeniedReason = string.Empty;

                                        switch (payload[1] & 0xF0) // upper 4 bits encode last cutout reason 
                                        {
                                            case 0x00:
                                                lastCruiseCutoutReason = "ON/OFF SW";
                                                break;
                                            case 0x10:
                                                lastCruiseCutoutReason = "SPEED SEN";
                                                break;
                                            case 0x20:
                                                lastCruiseCutoutReason = "RPM LIMIT";
                                                break;
                                            case 0x30:
                                                lastCruiseCutoutReason = "BRAKE SW";
                                                break;
                                            case 0x40:
                                                lastCruiseCutoutReason = "P/N SW";
                                                break;
                                            case 0x50:
                                                lastCruiseCutoutReason = "RPM/SPEED";
                                                break;
                                            case 0x60:
                                                lastCruiseCutoutReason = "CLUTCH";
                                                break;
                                            case 0x70:
                                                lastCruiseCutoutReason = "S/C DTC";
                                                break;
                                            case 0x80:
                                                lastCruiseCutoutReason = "KEY OFF";
                                                break;
                                            case 0x90:
                                                lastCruiseCutoutReason = "ACTIVE";
                                                break;
                                            case 0xA0:
                                                lastCruiseCutoutReason = "CLUTCH UP";
                                                break;
                                            case 0xB0:
                                                lastCruiseCutoutReason = "N/A";
                                                break;
                                            case 0xC0:
                                                lastCruiseCutoutReason = "SW DTC";
                                                break;
                                            case 0xD0:
                                                lastCruiseCutoutReason = "CANCEL SW";
                                                break;
                                            case 0xE0:
                                                lastCruiseCutoutReason = "LIMP-IN";
                                                break;
                                            case 0xF0:
                                                lastCruiseCutoutReason = "12V DTC";
                                                break;
                                            default:
                                                lastCruiseCutoutReason = "N/A";
                                                break;
                                        }

                                        switch (payload[1] & 0x0F) // lower 4 bits encode denied reason 
                                        {
                                            case 0x00:
                                                cruiseDeniedReason = "ON/OFF SW";
                                                break;
                                            case 0x01:
                                                cruiseDeniedReason = "SPEED SEN";
                                                break;
                                            case 0x02:
                                                cruiseDeniedReason = "RPM LIMIT";
                                                break;
                                            case 0x03:
                                                cruiseDeniedReason = "BRAKE SW";
                                                break;
                                            case 0x04:
                                                cruiseDeniedReason = "P/N SW";
                                                break;
                                            case 0x05:
                                                cruiseDeniedReason = "RPM/SPEED";
                                                break;
                                            case 0x06:
                                                cruiseDeniedReason = "CLUTCH";
                                                break;
                                            case 0x07:
                                                cruiseDeniedReason = "S/C DTC";
                                                break;
                                            case 0x08:
                                                cruiseDeniedReason = "ALLOWED";
                                                break;
                                            case 0x09:
                                                cruiseDeniedReason = "ACTIVE";
                                                break;
                                            case 0x0A:
                                                cruiseDeniedReason = "CLUTCH UP";
                                                break;
                                            case 0x0B:
                                                cruiseDeniedReason = "N/A";
                                                break;
                                            case 0x0C:
                                                cruiseDeniedReason = "SW DTC";
                                                break;
                                            case 0x0D:
                                                cruiseDeniedReason = "CANCEL SW";
                                                break;
                                            case 0x0E:
                                                cruiseDeniedReason = "LIMP-IN";
                                                break;
                                            case 0x0F:
                                                cruiseDeniedReason = "12V DTC";
                                                break;
                                            default:
                                                cruiseDeniedReason = "N/A";
                                                break;
                                        }

                                        if ((payload[1] & 0x0F) == 0x08)
                                        {
                                            descriptionToInsert = "CRUISE | LAST CUTOUT: " + lastCruiseCutoutReason + " | STATE: " + cruiseDeniedReason;
                                            valueToInsert = "STOPPED";
                                        }
                                        else if ((payload[1] & 0x0F) == 0x09)
                                        {
                                            descriptionToInsert = "CRUISE | LAST CUTOUT: " + lastCruiseCutoutReason + " | STATE: " + cruiseDeniedReason;
                                            valueToInsert = "ENGAGED";
                                        }
                                        else
                                        {
                                            descriptionToInsert = "CRUISE | LAST CUTOUT: " + lastCruiseCutoutReason + " | DENIED: " + cruiseDeniedReason;
                                            valueToInsert = "STOPPED";
                                        }
                                        unitToInsert = string.Empty;
                                        break;
                                    case 0x24: // target battery charging voltage
                                        descriptionToInsert = "TARGET BATTERY CHARGING VOLTAGE";
                                        valueToInsert = Math.Round(payload[1] * 0.0625D, 3).ToString("0.000").Replace(",", ".");
                                        unitToInsert = "V";
                                        break;
                                    case 0x25: // over 5 psi boost timer
                                        descriptionToInsert = "OVER 5 PSI BOOST TIMER";
                                        valueToInsert = payload[1].ToString("0");
                                        unitToInsert = string.Empty;
                                        break;
                                    case 0x26:
                                    case 0x28: // wastegate duty cycle
                                        descriptionToInsert = "WASTEGATE DUTY CYCLE";
                                        valueToInsert = Math.Round(payload[1] * 0.5D, 1).ToString("0.0").Replace(",", ".");
                                        unitToInsert = "%";
                                        break;
                                    case 0x27: // theft alarm status
                                        descriptionToInsert = "THEFT ALARM STATUS";
                                        valueToInsert = Convert.ToString(payload[1], 2).PadLeft(8, '0');
                                        unitToInsert = string.Empty;
                                        break;
                                    case 0x29: // read fuel setting
                                        descriptionToInsert = "READ FUEL SETTING";
                                        valueToInsert = Util.ByteToHexString(payload, 1, 1);
                                        unitToInsert = string.Empty;
                                        break;
                                    case 0x2A: // read set sync
                                        descriptionToInsert = "READ SET SYNC";
                                        valueToInsert = Util.ByteToHexString(payload, 1, 1);
                                        unitToInsert = string.Empty;
                                        break;
                                    case 0x2F: // upstream (pre-cat) o2 sensor voltage
                                        descriptionToInsert = "UPSTREAM O2 SENSOR VOLTAGE (PRE-CATALISATOR)";
                                        valueToInsert = Math.Round(payload[1] * 0.0196D, 3).ToString("0.000").Replace(",", ".");
                                        unitToInsert = "V";
                                        break;
                                    case 0x31: // Long Term Fuel Trim Bank 2
                                        descriptionToInsert = "LONG TERM FUEL TRIM BANK 2";
                                        valueToInsert = payload[1].ToString("0");
                                        unitToInsert = string.Empty;
                                        break;
                                    case 0x32: // A/C high side pressure sensor voltage
                                        descriptionToInsert = "A/C HIGH SIDE PRESSURE SENSOR VOLTAGE";
                                        valueToInsert = Math.Round(payload[1] * 0.0196D, 3).ToString("0.000").Replace(",", ".");
                                        unitToInsert = "V";
                                        break;
                                    case 0x33: // A/C high side pressure
                                        descriptionToInsert = "A/C HIGH SIDE PRESSURE SENSOR";

                                        if (Properties.Settings.Default.Units == "imperial")
                                        {
                                            valueToInsert = Math.Round(payload[1] * 1.961D, 1).ToString("0.0").Replace(",", ".");
                                            unitToInsert = "PSI";
                                        }
                                        else if (Properties.Settings.Default.Units == "metric")
                                        {
                                            valueToInsert = Math.Round(payload[1] * 1.961D * 6.894757D, 1).ToString("0.0").Replace(",", ".");
                                            unitToInsert = "KPA";
                                        }

                                        break;
                                    case 0x3B: // Fuel system status 1
                                        descriptionToInsert = "FUEL SYSTEM STATUS 1";
                                        valueToInsert = Util.ByteToHexString(payload, 1, 1);
                                        unitToInsert = string.Empty;
                                        break;
                                    case 0x3E: // Read fuel factor LH
                                        descriptionToInsert = "READ FUEL FACTOR LH";
                                        valueToInsert = Util.ByteToHexString(payload, 1, 1);
                                        unitToInsert = string.Empty;
                                        break;
                                    case 0x3F: // downstream (post-cat) o2 sensor voltage
                                        descriptionToInsert = "DOWNSTREAM O2 SENSOR VOLTAGE (POST-CATALISATOR)";
                                        valueToInsert = Math.Round(payload[1] * 0.0196D, 3).ToString("0.000").Replace(",", ".");
                                        unitToInsert = "V";
                                        break;
                                    case 0x40: // intake map sensor volts
                                        descriptionToInsert = "INTAKE MAP SENSOR VOLTAGE";
                                        valueToInsert = Math.Round(payload[1] * 0.0196D, 3).ToString("0.000").Replace(",", ".");
                                        unitToInsert = "V";
                                        break;
                                    case 0x41: // vehicle speed
                                        descriptionToInsert = "VEHICLE SPEED";

                                        if (Properties.Settings.Default.Units == "imperial")
                                        {
                                            valueToInsert = Math.Round(payload[1] / 2.0D).ToString("0");
                                            unitToInsert = "MPH";
                                        }
                                        else if (Properties.Settings.Default.Units == "metric")
                                        {
                                            valueToInsert = Math.Round(payload[1] / 2.0D * 1.609344D).ToString("0");
                                            unitToInsert = "KM/H";
                                        }

                                        break;
                                    case 0x42: // upstream (pre-cat) o2 sensor level
                                        descriptionToInsert = "UPSTREAM O2 SENSOR LEVEL (PRE-CATALISATOR)";

                                        switch (payload[1])
                                        {
                                            case 0xA0:
                                                valueToInsert = "LEAN";
                                                break;
                                            case 0xB1:
                                                valueToInsert = "RICH";
                                                break;
                                            case 0xFF:
                                                valueToInsert = "CENTER";
                                                break;
                                            default:
                                                valueToInsert = "N/A";
                                                break;
                                        }

                                        unitToInsert = string.Empty;
                                        break;
                                    case 0x46: // throttle position sensor
                                        descriptionToInsert = "THROTTLE POSITION SENSOR";
                                        valueToInsert = Math.Round(payload[1] * 0.3922D, 1).ToString("0.0").Replace(",", ".");
                                        unitToInsert = "%";
                                        break;
                                    case 0x48: // downstream (post-cat) o2 sensor level
                                        descriptionToInsert = "DOWNSTREAM O2 SENSOR LEVEL (POST-CATALISATOR)";

                                        switch (payload[1])
                                        {
                                            case 0xA0:
                                                valueToInsert = "LEAN";
                                                break;
                                            case 0xB1:
                                                valueToInsert = "RICH";
                                                break;
                                            case 0xFF:
                                                valueToInsert = "CENTER";
                                                break;
                                            default:
                                                valueToInsert = "N/A";
                                                break;
                                        }

                                        unitToInsert = string.Empty;
                                        break;
                                    case 0x4E: // fuel level sensor voltage
                                        descriptionToInsert = "FUEL LEVEL SENSOR VOLTAGE";
                                        valueToInsert = Math.Round(payload[1] * 0.0196D, 3).ToString("0.000").Replace(",", ".");
                                        unitToInsert = "V";
                                        break;
                                    case 0x4F: // fuel level
                                        descriptionToInsert = "FUEL LEVEL";

                                        if (Properties.Settings.Default.Units == "imperial")
                                        {
                                            valueToInsert = Math.Round(payload[1] * 0.125D, 1).ToString("0.0").Replace(",", ".");
                                            unitToInsert = "GALLON";
                                        }
                                        else if (Properties.Settings.Default.Units == "metric")
                                        {
                                            valueToInsert = Math.Round(payload[1] * 0.125D * 3.785412D, 1).ToString("0.0").Replace(",", ".");
                                            unitToInsert = "LITER";
                                        }

                                        break;
                                    case 0x57: // Fuel system status 2
                                        descriptionToInsert = "FUEL SYSTEM STATUS 2";
                                        valueToInsert = Util.ByteToHexString(payload, 1, 1);
                                        unitToInsert = string.Empty;
                                        break;
                                    case 0x5C: // calculated engine load
                                        descriptionToInsert = "CALCULATED ENGINE LOAD";
                                        valueToInsert = Math.Round(payload[1] * 0.3922D, 1).ToString("0.0").Replace(",", ".");
                                        unitToInsert = "%";
                                        break;
                                    case 0x5A: // output shaft speed
                                        descriptionToInsert = "OUTPUT SHAFT SPEED";
                                        valueToInsert = (payload[1] * 20).ToString("0.0").Replace(",", ".");
                                        unitToInsert = "RPM";
                                        break;
                                    case 0x5B: // governor pressure duty cycle
                                        descriptionToInsert = "GOVERNOR PRESSURE DUTY CYCLE";
                                        valueToInsert = Math.Round(payload[1] * 0.3922D, 1).ToString("0.0").Replace(",", ".");
                                        unitToInsert = "%";
                                        break;
                                    case 0x6D: // T-case switch voltage
                                        descriptionToInsert = "T-CASE SWITCH VOLTAGE";
                                        valueToInsert = Math.Round(payload[1] * 0.0196D, 3).ToString("0.000").Replace(",", ".");
                                        unitToInsert = "V";
                                        break;
                                    case 0x7A: // FCA current
                                        descriptionToInsert = "FCA CURRENT";
                                        valueToInsert = Math.Round(payload[1] * 0.0196D, 1).ToString("0.0").Replace(",", ".");
                                        unitToInsert = "A";
                                        break;
                                    case 0x7C: // oil temperature sensor voltage
                                        descriptionToInsert = "OIL TEMPERATURE SENSOR VOLTAGE";
                                        valueToInsert = Math.Round(payload[1] * 0.0196D, 3).ToString("0.000").Replace(",", ".");
                                        unitToInsert = "V";
                                        break;
                                    case 0x7D: // oil temperature sensor
                                        descriptionToInsert = "OIL TEMPERATURE SENSOR";

                                        if (Properties.Settings.Default.Units == "imperial")
                                        {
                                            valueToInsert = Math.Round((payload[1] * 1.8D) - 83.2D).ToString("0");
                                            unitToInsert = "°F";
                                        }
                                        else if (Properties.Settings.Default.Units == "metric")
                                        {
                                            valueToInsert = (payload[1] - 64).ToString("0");
                                            unitToInsert = "°C";
                                        }

                                        break;
                                    default:
                                        descriptionToInsert = "REQUEST DIAGNOSTIC DATA | OFFSET: " + Util.ByteToHexString(payload, 0, 1);
                                        valueToInsert = Util.ByteToHexString(payload, 1, payload.Length - 1);
                                        unitToInsert = string.Empty;
                                        break;
                                }
                            }
                            else // error
                            {
                                descriptionToInsert = "REQUEST DIAGNOSTIC DATA";
                                valueToInsert = "ERROR";
                                unitToInsert = string.Empty;
                            }
                            break;
                        case 0x15: // ROM value
                            if (message.Length >= minLength)
                            {
                                descriptionToInsert = "ROM VALUE | OFFSET: " + Util.ByteToHexString(payload, 0, 2);
                                valueToInsert = Util.ByteToHexString(payload, 2, 1);
                            }
                            else // error
                            {
                                descriptionToInsert = "ROM VALUE";
                                valueToInsert = "ERROR";
                            }
                            unitToInsert = string.Empty;
                            break;
                        case 0x16: // ROM constant value
                            if (message.Length >= minLength)
                            {
                                ushort offset = (ushort)(payload[0] + 0x8000);
                                byte[] offsetArray = new byte[2];
                                offsetArray[0] = (byte)((offset >> 8) & 0xFF);
                                offsetArray[1] = (byte)(offset & 0xFF);

                                descriptionToInsert = "ROM CONSTANT VALUE | OFFSET: " + Util.ByteToHexStringSimple(offsetArray);
                                valueToInsert = Util.ByteToHexString(payload, 1, 1);
                            }
                            else // error
                            {
                                descriptionToInsert = "ROM CONSTANT VALUE";
                                valueToInsert = "ERROR";
                            }
                            unitToInsert = string.Empty;
                            break;
                        case 0x17: // erase fault codes
                            if (message.Length >= minLength)
                            {
                                if (payload[0] == 0xE0) valueToInsert = "ERASED";
                                else valueToInsert = "FAILED";
                            }
                            else // error
                            {
                                valueToInsert = "ERROR";
                            }
                            unitToInsert = string.Empty;
                            break;
                        case 0x18: // control ASD relay
                            if (message.Length >= minLength)
                            {
                                valueToInsert = Util.ByteToHexString(payload, 1, 1);
                            }
                            else // error
                            {
                                valueToInsert = "ERROR";
                            }
                            unitToInsert = string.Empty;
                            break;
                        case 0x19: // set idle speed
                            if (message.Length >= minLength)
                            {
                                valueToInsert = Math.Round(payload[0] * 7.85D).ToString("0");

                                if (payload[0] < 0x42)
                                {
                                    valueToInsert += " - TOO LOW!";
                                }

                                unitToInsert = "RPM";
                            }
                            else // error
                            {
                                valueToInsert = "ERROR";
                                unitToInsert = string.Empty;
                            }
                            break;
                        case 0x1A: // switch test
                            if (message.Length >= minLength)
                            {
                                List<string> switchList = new List<string>();

                                switch (payload[0])
                                {
                                    case 0x01:
                                        switchList.Clear();
                                        if (Util.IsBitSet(payload[1], 0)) switchList.Add("WAIT TO START LAMP");
                                        if (Util.IsBitSet(payload[1], 1)) switchList.Add("INTAKE HEATER #1");
                                        if (Util.IsBitSet(payload[1], 2)) switchList.Add("INTAKE HEATER #2");
                                        if (Util.IsBitSet(payload[1], 3)) switchList.Add("IDLE VALIDATION SW1");
                                        if (Util.IsBitSet(payload[1], 4)) switchList.Add("IDLE VALIDATION SW2");
                                        if (Util.IsBitSet(payload[1], 5)) switchList.Add("IDLE SELECT");
                                        if (Util.IsBitSet(payload[1], 6)) switchList.Add("TRANSFER PMPDR");

                                        
                                        if (switchList.Count > 0)
                                        {
                                            descriptionToInsert = "SWITCH TEST | ";

                                            foreach (string s in switchList)
                                            {
                                                descriptionToInsert += s + " | ";
                                            }

                                            if (descriptionToInsert.Length > 2) descriptionToInsert = descriptionToInsert.Remove(descriptionToInsert.Length - 3); // remove last "|" character
                                        }
                                        else
                                        {
                                            descriptionToInsert = "SWITCH TEST";
                                        }
                                        break;
                                    case 0x02:
                                        switchList.Clear();
                                        if (Util.IsBitSet(payload[1], 0)) switchList.Add("INJ PUMP");
                                        if (Util.IsBitSet(payload[1], 2)) switchList.Add("A/C CLUTCH");
                                        if (Util.IsBitSet(payload[1], 3)) switchList.Add("EXHAUST BRAKE");
                                        if (Util.IsBitSet(payload[1], 4)) switchList.Add("BRAKE");
                                        if (Util.IsBitSet(payload[1], 5)) switchList.Add("EVAP PURGE");
                                        if (Util.IsBitSet(payload[1], 7)) switchList.Add("LOW OIL");

                                        if (switchList.Count > 0)
                                        {
                                            descriptionToInsert = "SWITCH TEST | ";

                                            foreach (string s in switchList)
                                            {
                                                descriptionToInsert += s + " | ";
                                            }

                                            descriptionToInsert = descriptionToInsert.Remove(descriptionToInsert.Length - 3); // remove last "|" character
                                        }
                                        else
                                        {
                                            descriptionToInsert = "SWITCH TEST";
                                        }
                                        break;
                                    case 0x03:
                                        switchList.Clear();
                                        if (Util.IsBitSet(payload[1], 1)) switchList.Add("MIL");
                                        if (Util.IsBitSet(payload[1], 2)) switchList.Add("GENERATOR LAMP");
                                        if (Util.IsBitSet(payload[1], 3)) switchList.Add("GENERATOR FIELD");
                                        if (Util.IsBitSet(payload[1], 4)) switchList.Add("12V FEED");
                                        if (Util.IsBitSet(payload[1], 6)) switchList.Add("TRANS O/D");
                                        if (Util.IsBitSet(payload[1], 7)) switchList.Add("TRANS TOW MODE");

                                        if (switchList.Count > 0)
                                        {
                                            descriptionToInsert = "SWITCH TEST | ";

                                            foreach (string s in switchList)
                                            {
                                                descriptionToInsert += s + " | ";
                                            }

                                            descriptionToInsert = descriptionToInsert.Remove(descriptionToInsert.Length - 3); // remove last "|" character
                                        }
                                        else
                                        {
                                            descriptionToInsert = "SWITCH TEST";
                                        }
                                        break;
                                    case 0x04:
                                        switchList.Clear();
                                        if (Util.IsBitSet(payload[1], 1)) switchList.Add("TRD LINK");
                                        if (Util.IsBitSet(payload[1], 4)) switchList.Add("ASD");
                                        if (Util.IsBitSet(payload[1], 6)) switchList.Add("IGNITION");

                                        if (switchList.Count > 0)
                                        {
                                            descriptionToInsert = "SWITCH TEST | ";

                                            foreach (string s in switchList)
                                            {
                                                descriptionToInsert += s + " | ";
                                            }

                                            descriptionToInsert = descriptionToInsert.Remove(descriptionToInsert.Length - 3); // remove last "|" character
                                        }
                                        else
                                        {
                                            descriptionToInsert = "SWITCH TEST";
                                        }
                                        break;
                                    default:
                                        descriptionToInsert = "SWITCH TEST | OFFSET: " + Util.ByteToHexString(payload, 0, 1);
                                        break;
                                }

                                valueToInsert = string.Empty;
                            }
                            else // error
                            {
                                valueToInsert = "ERROR";
                            }
                            unitToInsert = string.Empty;
                            break;
                        case 0x1B: // init byte mode download
                            valueToInsert = string.Empty;
                            unitToInsert = string.Empty;
                            break;
                        case 0x1C: // write memory
                            if (message.Length >= minLength)
                            {
                                switch (payload[0])
                                {
                                    case 0x10: // reset emr1
                                        descriptionToInsert = "WRITE MEMROY | RESET EMR 1";
                                        if ((payload[1] == 0x00) && (payload[2] == 0xFF)) valueToInsert = "OK";
                                        else valueToInsert = "FAILED";
                                        break;
                                    case 0x11: // reset emr2
                                        descriptionToInsert = "WRITE MEMROY | RESET EMR 2";
                                        if ((payload[1] == 0x00) && (payload[2] == 0xFF)) valueToInsert = "OK";
                                        else valueToInsert = "FAILED";
                                        break;
                                    case 0x1A: // var idle
                                        if (payload[1] == 0xFF)
                                        {
                                            descriptionToInsert = "WRITE MEMROY | ENABLE VAR IDLE";

                                            if (payload[2] == 0xFF)
                                            {
                                                valueToInsert = "OK";
                                            }
                                            else
                                            {
                                                valueToInsert = "FAILED";
                                            }
                                        }
                                        else if (payload[1] == 0x00)
                                        {
                                            descriptionToInsert = "WRITE MEMROY | DISABLE VAR IDLE";

                                            if (payload[2] == 0xFF)
                                            {
                                                valueToInsert = "OK";
                                            }
                                            else
                                            {
                                                valueToInsert = "FAILED";
                                            }
                                        }
                                        break;
                                    default:
                                        descriptionToInsert = "WRITE MEMROY | OFFSET: " + Util.ByteToHexString(payload, 0, 1);
                                        valueToInsert = Util.ByteToHexString(payload, 1, 1);
                                        break;
                                }
                            }
                            else // error
                            {
                                descriptionToInsert = "WRITE MEMROY";
                                valueToInsert = "ERROR";
                            }
                            unitToInsert = string.Empty;
                            break;
                        case 0x1F: // write RAM
                            if (message.Length >= minLength)
                            {
                                descriptionToInsert = "WRITE RAM | OFFSET: 07 " + Util.ByteToHexString(payload, 0, 1);

                                switch (payload[2])
                                {
                                    case 0xE5:
                                        valueToInsert = Util.ByteToHexString(payload, 1, 1);
                                        unitToInsert = "OK";
                                        break;
                                    case 0x00:
                                        valueToInsert = "DENIED (INVALID OFFSET)";
                                        unitToInsert = string.Empty;
                                        break;
                                    case 0xF1:
                                        valueToInsert = "DENIED (SECURITY LEVEL)";
                                        unitToInsert = string.Empty;
                                        break;
                                    default:
                                        valueToInsert = Util.ByteToHexString(payload, 2, 1);
                                        unitToInsert = string.Empty;
                                        break;
                                }
                            }
                            else // error
                            {
                                descriptionToInsert = "WRITE RAM";
                                valueToInsert = "ERROR";
                                unitToInsert = string.Empty;
                            }
                            break;
                        case 0x21: // set sync / timing / spark scatter
                            if (message.Length >= minLength)
                            {
                                switch (payload[0])
                                {
                                    case 0x00:
                                        descriptionToInsert = "UNKILL SPARK SCATTER";

                                        switch (payload[1])
                                        {
                                            case 0x00:
                                                valueToInsert = "TIMING ABOLISHED";
                                                break;
                                            case 0x01:
                                                valueToInsert = "TIMING INITIATED";
                                                break;
                                            case 0x02:
                                                valueToInsert = "REJECTED (OPEN THROTTLE)";
                                                break;
                                            case 0x03:
                                                valueToInsert = "REJECTED (SLP)";
                                                break;
                                            case 0x10:
                                                valueToInsert = "SYNC INITIATED";
                                                break;
                                            default:
                                                valueToInsert = "UNDEFINED";
                                                break;
                                        }

                                        break;
                                    case 0x01:
                                        descriptionToInsert = "KILL SPARK SCATTER";

                                        switch (payload[1])
                                        {
                                            case 0x00:
                                                valueToInsert = "TIMING ABOLISHED";
                                                break;
                                            case 0x01:
                                                valueToInsert = "TIMING INITIATED";
                                                break;
                                            case 0x02:
                                                valueToInsert = "REJECTED (OPEN THROTTLE)";
                                                break;
                                            case 0x03:
                                                valueToInsert = "REJECTED (SLP)";
                                                break;
                                            case 0x10:
                                                valueToInsert = "SYNC INITIATED";
                                                break;
                                            default:
                                                valueToInsert = "UNDEFINED";
                                                break;
                                        }

                                        break;
                                    case 0x10:
                                        descriptionToInsert = "SET SYNC MODE";

                                        switch (payload[1])
                                        {
                                            case 0x00:
                                                valueToInsert = "TIMING ABOLISHED";
                                                break;
                                            case 0x01:
                                                valueToInsert = "TIMING INITIATED";
                                                break;
                                            case 0x02:
                                                valueToInsert = "REJECTED (OPEN THROTTLE)";
                                                break;
                                            case 0x03:
                                                valueToInsert = "REJECTED (SLP)";
                                                break;
                                            case 0x10:
                                                valueToInsert = "SYNC INITIATED";
                                                break;
                                            default:
                                                valueToInsert = "UNDEFINED";
                                                break;
                                        }

                                        break;
                                    default:
                                        descriptionToInsert = "SET ENGINE PARAMETER | OFFSET: " + Util.ByteToHexString(payload, 0, 1);
                                        valueToInsert = Util.ByteToHexString(payload, 1, 1);
                                        break;
                                }
                            }
                            else // error
                            {
                                descriptionToInsert = "SET ENGINE PARAMETER";
                                valueToInsert = "ERROR";
                            }
                            unitToInsert = string.Empty;
                            break;
                        case 0x22: // send engine parameters
                            if (message.Length >= minLength)
                            {
                                switch (payload[0])
                                {
                                    case 0x01: // RPM
                                        descriptionToInsert = "ENGINE SPEED";
                                        valueToInsert = (payload[1] * 32).ToString("0");
                                        unitToInsert = "RPM";
                                        break;
                                    case 0x02: // injector pulsewidth
                                        descriptionToInsert = "INJECTOR PULSEWIDTH";
                                        valueToInsert = payload[1].ToString("0");
                                        unitToInsert = string.Empty;
                                        break;
                                    case 0x03: // target idle speed
                                        descriptionToInsert = "TARGET IDLE SPEED";
                                        valueToInsert = (payload[1] * 32).ToString("0");
                                        unitToInsert = "RPM";
                                        break;
                                    default:
                                        descriptionToInsert = "SEND ENGINE PARAMETER | OFFSET: " + Util.ByteToHexString(payload, 0, 1);
                                        valueToInsert = payload[1].ToString("0");
                                        unitToInsert = string.Empty;
                                        break;
                                }
                            }
                            else // error
                            {
                                descriptionToInsert = "SEND ENGINE PARAMETER";
                                valueToInsert = "ERROR";
                                unitToInsert = string.Empty;
                            }
                            break;
                        case 0x23: // reset memory
                            if (message.Length >= minLength)
                            {
                                switch (payload[0])
                                {
                                    case 0x01:
                                        descriptionToInsert = "ERASE ENGINE FAULT CODES";
                                        break;
                                    case 0x02:
                                        descriptionToInsert = "RESET ADAPTIVE FUEL FACTOR (LTFT)";
                                        break;
                                    case 0x03:
                                        descriptionToInsert = "RESET IAC COUNTER";
                                        break;
                                    case 0x04:
                                        descriptionToInsert = "RESET MINIMUM TPS VOLTS";
                                        break;
                                    case 0x05:
                                        descriptionToInsert = "RESET FLEX FUEL PERCENT";
                                        break;
                                    case 0x06:
                                        descriptionToInsert = "RESET CAM/CRANK SYNC";
                                        break;
                                    case 0x07:
                                        descriptionToInsert = "RESET FUEL SHUTOFF";
                                        break;
                                    case 0x08:
                                        descriptionToInsert = "RESET RUNTIME AT STALL";
                                        break;
                                    case 0x09:
                                        descriptionToInsert = "DOOR LOCK ENABLE";
                                        break;
                                    case 0x0A:
                                        descriptionToInsert = "DOOR LOCK DISABLE";
                                        break;
                                    case 0x0B:
                                        descriptionToInsert = "RESET CAM/CRANK TIMING REFERENCE";
                                        break;
                                    case 0x0C:
                                        descriptionToInsert = "A/C FAULT ENABLE";
                                        break;
                                    case 0x0D:
                                        descriptionToInsert = "A/C FAULT DISABLE";
                                        break;
                                    case 0x0E:
                                        descriptionToInsert = "S/C FAULT ENABLE";
                                        break;
                                    case 0x0F:
                                        descriptionToInsert = "S/C FAULT DISABLE";
                                        break;
                                    case 0x10:
                                        descriptionToInsert = "PS FAULT ENABLE";
                                        break;
                                    case 0x11:
                                        descriptionToInsert = "PS FAULT DISABLE";
                                        break;
                                    case 0x12:
                                        descriptionToInsert = "RESET EEPROM / ADAPTIVE NUMERATOR";
                                        break;
                                    case 0x13:
                                        descriptionToInsert = "RESET SKIM F4";
                                        break;
                                    case 0x14:
                                        descriptionToInsert = "RESET DUTY CYCLE MONITOR";
                                        break;
                                    case 0x15:
                                        descriptionToInsert = "RESET TRIP/IDLE/CRUISE/INJ/O/D OFF/WATER IN FUEL";
                                        break;
                                    case 0x20:
                                        descriptionToInsert = "RESET TPS ADAPTATION FOR ETC";
                                        break;
                                    case 0x21:
                                        descriptionToInsert = "RESET MIN PEDAL VALUE";
                                        break;
                                    case 0x22:
                                        descriptionToInsert = "RESET LEARNED KNOCK CORRECTION";
                                        break;
                                    case 0x23:
                                        descriptionToInsert = "RESET LEARNED MISFIRE CORRECTION";
                                        break;
                                    case 0x24:
                                        descriptionToInsert = "RESET IDLE ADAPTATION";
                                        break;
                                    default:
                                        descriptionToInsert = "RESET MEMORY | OFFSET: " + Util.ByteToHexString(payload, 0, 1);
                                        break;
                                }

                                switch (payload[1])
                                {
                                    case 0x00:
                                        valueToInsert = "TURN OFF ENGINE";
                                        break;
                                    case 0x01:
                                        valueToInsert = "ERROR";
                                        break;
                                    case 0x02:
                                        valueToInsert = "DENIED (MODULE BUSY)";
                                        break;
                                    case 0x03:
                                        valueToInsert = "DENIED (SECURITY LEVEL)";
                                        break;
                                    case 0xF0:
                                        valueToInsert = "OK";
                                        break;
                                    default:
                                        valueToInsert = Util.ByteToHexString(payload, 1, 1);
                                        break;
                                }

                                unitToInsert = string.Empty;
                            }
                            else // error
                            {
                                descriptionToInsert = "RESET MEMORY";
                                valueToInsert = "ERROR";
                                unitToInsert = string.Empty;
                            }
                            break;
                        case 0x25: // write ROM setting
                            if (message.Length >= minLength)
                            {
                                switch (payload[0])
                                {
                                    case 0x01:
                                        descriptionToInsert = "WRITE ROM SETTING | CALPOT LEAD";
                                        break;
                                    case 0x02:
                                        descriptionToInsert = "WRITE ROM SETTING | CALPOT MEX";
                                        break;
                                    case 0x04:
                                        descriptionToInsert = "WRITE ROM SETTING | EGR SYSTEM";
                                        break;
                                    case 0x05:
                                        descriptionToInsert = "WRITE ROM SETTING | FUEL INJECTOR #1";
                                        break;
                                    case 0x0F:
                                        descriptionToInsert = "WRITE ROM SETTING | MINIMUM AIR FLOW";
                                        break;
                                    case 0x10:
                                        descriptionToInsert = "WRITE ROM SETTING | CALPOT LHBL";
                                        break;
                                    case 0x1C:
                                        descriptionToInsert = "WRITE ROM SETTING | MISFIRE MONITOR";
                                        break;
                                    case 0x21:
                                        descriptionToInsert = "WRITE ROM SETTING | LINEAR IAC MOTOR";
                                        break;
                                    case 0x25:
                                        descriptionToInsert = "WRITE ROM SETTING | CYLINDER PERFORMANCE TEST";
                                        break;
                                    case 0x26:
                                        descriptionToInsert = "WRITE ROM SETTING | HIGH-PRESSURE SAFETY VALVE TEST";
                                        break;
                                    default:
                                        descriptionToInsert = "WRITE ROM SETTING | OFFSET: " + Util.ByteToHexString(payload, 0, 1);
                                        break;
                                }

                                switch (payload[1])
                                {
                                    case 0x00:
                                        valueToInsert = "RESET";
                                        break;
                                    case 0x01:
                                        valueToInsert = "ENABLED";
                                        break;
                                    case 0x02:
                                        valueToInsert = "DISABLED";
                                        break;
                                    default:
                                        valueToInsert = "UNKNOWN";
                                        break;
                                }

                                switch (payload[2])
                                {
                                    case 0x01:
                                        unitToInsert = "OK";
                                        break;
                                    default:
                                        unitToInsert = "DENIED (" + Util.ByteToHexString(payload, 2, 1) + ")";
                                        break;
                                }
                            }
                            else // error
                            {
                                descriptionToInsert = "WRITE ROM SETTING";
                                valueToInsert = "ERROR";
                                unitToInsert = string.Empty;
                            }
                            break;
                        case 0x26: // read flash memory
                            if (message.Length >= minLength)
                            {
                                descriptionToInsert = "READ FLASH MEMORY | OFFSET: " + Util.ByteToHexString(payload, 0, 3);
                                valueToInsert = Util.ByteToHexString(payload, 3, 1);
                            }
                            else // error
                            {
                                descriptionToInsert = "READ FLASH MEMORY";
                                valueToInsert = "ERROR";
                            }
                            unitToInsert = string.Empty;
                            break;
                        case 0x27: // write EEPROM
                            if (message.Length >= minLength)
                            {
                                descriptionToInsert = "WRITE EEPROM | OFFSET: " + Util.ByteToHexString(payload, 0, 2);
                                
                                switch (payload[3])
                                {
                                    case 0xE2:
                                        valueToInsert = Util.ByteToHexString(payload, 2, 1);
                                        unitToInsert = "OK";
                                        break;
                                    case 0xE4:
                                        valueToInsert = "UNKNOWN RESULT";
                                        unitToInsert = string.Empty;
                                        break;
                                    case 0xE5:
                                        valueToInsert = "UNKNOWN RESULT";
                                        unitToInsert = string.Empty;
                                        break;
                                    case 0xF0:
                                        valueToInsert = "DENIED (INVALID OFFSET)";
                                        unitToInsert = string.Empty;
                                        break;
                                    case 0xF1:
                                        valueToInsert = "DENIED (SECURITY LEVEL)";
                                        unitToInsert = string.Empty;
                                        break;
                                    default:
                                        valueToInsert = Util.ByteToHexString(payload, 3, 1);
                                        unitToInsert = string.Empty;
                                        break;
                                }
                            }
                            else // error
                            {
                                descriptionToInsert = "WRITE EEPROM";
                                valueToInsert = "ERROR";
                                unitToInsert = string.Empty;
                            }
                            break;
                        case 0x28: // read EEPROM
                            if (message.Length >= minLength)
                            {
                                descriptionToInsert = "READ EEPROM | OFFSET: " + Util.ByteToHexString(payload, 0, 2);
                                valueToInsert = Util.ByteToHexString(payload, 2, 1);
                            }
                            else // error
                            {
                                descriptionToInsert = "READ EEPROM";
                                valueToInsert = "ERROR";
                            }
                            unitToInsert = string.Empty;
                            break;
                        case 0x29: // write RAM
                            if (message.Length >= minLength)
                            {
                                descriptionToInsert = "WRITE RAM | OFFSET: " + Util.ByteToHexString(payload, 0, 2);

                                switch (payload[3])
                                {
                                    case 0xE5:
                                        valueToInsert = Util.ByteToHexString(payload, 2, 1);
                                        unitToInsert = "OK";
                                        break;
                                    case 0xF0:
                                        valueToInsert = "DENIED (INVALID OFFSET)";
                                        unitToInsert = string.Empty;
                                        break;
                                    case 0xF1:
                                        valueToInsert = "DENIED (SECURITY LEVEL)";
                                        unitToInsert = string.Empty;
                                        break;
                                    default:
                                        valueToInsert = Util.ByteToHexString(payload, 3, 1);
                                        unitToInsert = string.Empty;
                                        break;
                                }
                            }
                            else // error
                            {
                                descriptionToInsert = "WRITE RAM";
                                valueToInsert = "ERROR";
                                unitToInsert = string.Empty;
                            }
                            break;
                        case 0x2A: // information request
                            if (message.Length >= minLength)
                            {
                                switch (payload[0])
                                {
                                    case 0x00:
                                        valueToInsert = LastROMInfoText;
                                        unitToInsert = string.Empty;
                                        break;
                                    case 0x01:
                                        descriptionToInsert = "INFORMATION REQUEST | PCM PART NUMBER 1-2";
                                        valueToInsert = Util.ByteToHexString(payload, 1, 1);
                                        unitToInsert = string.Empty;
                                        break;
                                    case 0x02:
                                        descriptionToInsert = "INFORMATION REQUEST | PCM PART NUMBER 3-4";
                                        valueToInsert = Util.ByteToHexString(payload, 1, 1);
                                        unitToInsert = string.Empty;
                                        break;
                                    case 0x03:
                                        descriptionToInsert = "INFORMATION REQUEST | PCM PART NUMBER 5-6";
                                        valueToInsert = Util.ByteToHexString(payload, 1, 1);
                                        unitToInsert = string.Empty;
                                        break;
                                    case 0x04:
                                        descriptionToInsert = "INFORMATION REQUEST | PCM PART NUMBER 7-8";
                                        valueToInsert = Util.ByteToHexString(payload, 1, 1);
                                        unitToInsert = string.Empty;
                                        break;
                                    case 0x0A:
                                        descriptionToInsert = "INFORMATION REQUEST | FUEL TYPE";
                                        switch (payload[1])
                                        {
                                            case 0x01:
                                                valueToInsert = "UNLEADED GAS";
                                                break;
                                            case 0x02:
                                                valueToInsert = "DIESEL";
                                                break;
                                            case 0x03:
                                                valueToInsert = "PROPANE";
                                                break;
                                            case 0x04:
                                                valueToInsert = "METHANOL";
                                                break;
                                            case 0x05:
                                                valueToInsert = "LEADED GAS";
                                                break;
                                            case 0x06:
                                                valueToInsert = "FLEX";
                                                break;
                                            case 0x07:
                                                valueToInsert = "CNG";
                                                break;
                                            case 0x08:
                                                valueToInsert = "ELECTRIC";
                                                break;
                                            default:
                                                valueToInsert = "N/A";
                                                break;
                                        }
                                        unitToInsert = string.Empty;
                                        break;
                                    case 0x0B:
                                        descriptionToInsert = "INFORMATION REQUEST | MODEL YEAR";
                                        switch (payload[1])
                                        {
                                            case 0x01:
                                                valueToInsert = "1991";
                                                break;
                                            case 0x02:
                                                valueToInsert = "1992";
                                                break;
                                            case 0x03:
                                                valueToInsert = "1993";
                                                break;
                                            case 0x04:
                                                valueToInsert = "1994";
                                                break;
                                            case 0x05:
                                                valueToInsert = "1995";
                                                break;
                                            case 0x06:
                                                valueToInsert = "1996";
                                                break;
                                            case 0x07:
                                                valueToInsert = "1997";
                                                break;
                                            case 0x08:
                                                valueToInsert = "1998";
                                                break;
                                            case 0x09:
                                                valueToInsert = "1999";
                                                break;
                                            case 0x0A:
                                                valueToInsert = "2000";
                                                break;
                                            case 0x0B:
                                                valueToInsert = "2001";
                                                break;
                                            case 0x0C:
                                                valueToInsert = "2002";
                                                break;
                                            case 0x0D:
                                                valueToInsert = "2003";
                                                break;
                                            case 0x0E:
                                                valueToInsert = "2004";
                                                break;
                                            case 0x0F:
                                                valueToInsert = "2005";
                                                break;
                                            default:
                                                valueToInsert = "N/A";
                                                break;
                                        }
                                        unitToInsert = string.Empty;
                                        break;
                                    case 0x0C:
                                        descriptionToInsert = "INFORMATION REQUEST | ENGINE DISPLACEMENT/CONFIGURATION/ORIENTATION";
                                        switch (payload[1])
                                        {
                                            case 0x01:
                                                valueToInsert = "2.2 LITER";
                                                unitToInsert = "I4 E-W";
                                                break;
                                            case 0x02:
                                                valueToInsert = "2.5 LITER";
                                                unitToInsert = "I4 E-W";
                                                break;
                                            case 0x03:
                                                valueToInsert = "3.0 LITER";
                                                unitToInsert = "V6 E-W";
                                                break;
                                            case 0x04:
                                                valueToInsert = "3.3 LITER";
                                                unitToInsert = "V6 E-W";
                                                break;
                                            case 0x05:
                                                valueToInsert = "3.9 LITER";
                                                unitToInsert = "V6 N-S";
                                                break;
                                            case 0x06:
                                                valueToInsert = "5.2 LITER";
                                                unitToInsert = "V8 N-S";
                                                break;
                                            case 0x07:
                                                valueToInsert = "5.9 LITER";
                                                unitToInsert = "V8 N-S";
                                                break;
                                            case 0x08:
                                                valueToInsert = "3.8 LITER";
                                                unitToInsert = "V6 E-W";
                                                break;
                                            case 0x09:
                                                valueToInsert = "4.0 LITER";
                                                unitToInsert = "I6 N-S";
                                                break;
                                            case 0x0A:
                                                valueToInsert = "2.0 LITER";
                                                unitToInsert = "I4 E-W SOHC";
                                                break;
                                            case 0x0B:
                                                valueToInsert = "3.5 LITER";
                                                unitToInsert = "V6 N-S";
                                                break;
                                            case 0x0C:
                                                valueToInsert = "8.0 LITER";
                                                unitToInsert = "V10 N-S";
                                                break;
                                            case 0x0D:
                                                valueToInsert = "2.4 LITER";
                                                unitToInsert = "I4 E-W";
                                                break;
                                            case 0x0E:
                                                valueToInsert = "2.5 LITER";
                                                unitToInsert = "I4 N-S";
                                                break;
                                            case 0x0F:
                                                valueToInsert = "2.5 LITER";
                                                unitToInsert = "V6 N-S";
                                                break;
                                            case 0x10:
                                                valueToInsert = "2.0 LITER";
                                                unitToInsert = "I4 E-W DOHC";
                                                break;
                                            case 0x11:
                                                valueToInsert = "2.5 LITER";
                                                unitToInsert = "V6 E-W";
                                                break;
                                            case 0x12:
                                                valueToInsert = "5.9 LITER";
                                                unitToInsert = "I6 N-S";
                                                break;
                                            case 0x13:
                                                valueToInsert = "3.3 LITER";
                                                unitToInsert = "V6 N-S";
                                                break;
                                            case 0x14:
                                                valueToInsert = "2.7 LITER";
                                                unitToInsert = "V6 N-S";
                                                break;
                                            case 0x15:
                                                valueToInsert = "3.2 LITER";
                                                unitToInsert = "V6 N-S";
                                                break;
                                            case 0x16:
                                                valueToInsert = "1.8 LITER";
                                                unitToInsert = "I4 E-W";
                                                break;
                                            case 0x17:
                                                valueToInsert = "3.7 LITER";
                                                unitToInsert = "V6 N-S";
                                                break;
                                            case 0x18:
                                                valueToInsert = "4.7 LITER";
                                                unitToInsert = "V8 N-S";
                                                break;
                                            case 0x19:
                                                valueToInsert = "1.9 LITER";
                                                unitToInsert = "I4 E-W";
                                                break;
                                            case 0x1A:
                                                valueToInsert = "3.1 LITER";
                                                unitToInsert = "I5 N-S";
                                                break;
                                            case 0x1B:
                                                valueToInsert = "1.6 LITER";
                                                unitToInsert = "I4 E-W";
                                                break;
                                            case 0x1C:
                                                valueToInsert = "2.7 LITER";
                                                unitToInsert = "V6 E-W";
                                                break;
                                            default:
                                                valueToInsert = "N/A";
                                                unitToInsert = string.Empty;
                                                break;
                                        }
                                        break;
                                    case 0x0F:
                                        descriptionToInsert = "INFORMATION REQUEST | SBEC INFO";
                                        valueToInsert = Util.ByteToHexString(payload, 1, 1);
                                        break;
                                    case 0x17:
                                        descriptionToInsert = "INFORMATION REQUEST | PCM PART NUMBER 9";
                                        valueToInsert = Util.ByteToHexString(payload, 1, 1);
                                        unitToInsert = string.Empty;
                                        break;
                                    case 0x18:
                                        descriptionToInsert = "INFORMATION REQUEST | PCM PART NUMBER 10";
                                        valueToInsert = Util.ByteToHexString(payload, 1, 1);
                                        unitToInsert = string.Empty;
                                        break;
                                    default:
                                        descriptionToInsert = "INFORMATION REQUEST | " + "OFFSET: " + Util.ByteToHexString(payload, 0, 1);
                                        valueToInsert = Util.ByteToHexString(payload, 1, 1);
                                        unitToInsert = string.Empty;
                                        break;
                                }

                                LastROMInfoText = valueToInsert;
                            }
                            else // error
                            {
                                descriptionToInsert = "INFORMATION REQUEST";
                                valueToInsert = "ERROR";
                                unitToInsert = string.Empty;
                            }
                            break;
                        case 0x2B: // get security seed
                            if (message.Length >= minLength)
                            {
                                byte checksum = (byte)(0x2B + payload[0] + payload[1]);

                                if (payload[2] == checksum)
                                {
                                    ushort seed = (ushort)((payload[0] << 8) + payload[1]);

                                    if (seed != 0)
                                    {
                                        ushort key = (ushort)((seed << 2) + 0x9018);
                                        byte keyHB = (byte)(key >> 8);
                                        byte keyLB = (byte)(key);
                                        byte keyChecksum = (byte)(0x2C + keyHB + keyLB);
                                        byte[] keyArray = { 0x2C, keyHB, keyLB, keyChecksum };
                                        descriptionToInsert = "GET SECURITY SEED | KEY: " + Util.ByteToHexStringSimple(keyArray);
                                    }
                                    else
                                    {
                                        descriptionToInsert = "GET SECURITY SEED | PCM ALREADY UNLOCKED";
                                    }

                                    valueToInsert = Util.ByteToHexString(payload, 0, 2);
                                }
                                else
                                {
                                    descriptionToInsert = "GET SECURITY SEED";
                                    valueToInsert = "CHECKSUM ERROR";
                                }
                            }
                            else // error
                            {
                                descriptionToInsert = "GET SECURITY SEED";
                                valueToInsert = "ERROR";
                            }
                            unitToInsert = string.Empty;
                            break;
                        case 0x2C: // send security key
                            if (message.Length >= minLength)
                            {
                                switch (payload[3])
                                {
                                    case 0x00:
                                        valueToInsert = "ACCEPTED";
                                        break;
                                    case 0x01:
                                        valueToInsert = "INCORRECT KEY";
                                        break;
                                    case 0x02:
                                        valueToInsert = "CHECKSUM ERROR";
                                        break;
                                    case 0x03:
                                        valueToInsert = "BLOCKED | RESTART PCM";
                                        break;
                                    default:
                                        valueToInsert = Util.ByteToHexString(payload, 3, 1);
                                        break;
                                }
                            }
                            else // error
                            {
                                valueToInsert = "ERROR";
                            }
                            unitToInsert = string.Empty;
                            break;
                        case 0x2D: // min-max engine parameter value
                            if (message.Length >= minLength)
                            {
                                descriptionToInsert = "MIN/MAX ENGINE PARAMETER VALUE | OFFSET: " + Util.ByteToHexString(payload, 0, 1);
                                valueToInsert = Util.ByteToHexString(payload, 1, 1);
                            }
                            else // error
                            {
                                descriptionToInsert = "MIN/MAX ENGINE PARAMETER VALUE";
                                valueToInsert = "ERROR";
                            }
                            unitToInsert = string.Empty;
                            break;
                        case 0x2E: // engine fault codes
                        case 0x32:
                        case 0x33:
                            if (message.Length >= minLength)
                            {
                                valueToInsert = Util.ByteToHexStringSimple(payload);
                            }
                            else // error
                            {
                                valueToInsert = "ERROR";
                            }
                            unitToInsert = string.Empty;
                            break;
                        case 0x35: // get security seed #1 or #2
                            if (message.Length >= minLength)
                            {
                                byte checksum = (byte)(0x35 + payload[0] + payload[1] + payload[2]);

                                if (payload[3] == checksum)
                                {
                                    ushort seed = (ushort)((payload[1] << 8) + payload[2]);

                                    if (payload[0] == 1)
                                    {
                                        if (seed != 0)
                                        {
                                            ushort key = (ushort)((seed << 2) + 0x9018);
                                            byte keyHB = (byte)(key >> 8);
                                            byte keyLB = (byte)(key);
                                            byte keyChecksum = (byte)(0x2C + keyHB + keyLB);
                                            byte[] keyArray = { 0x2C, keyHB, keyLB, keyChecksum };
                                            descriptionToInsert = "GET SECURITY SEED #1 | KEY: " + Util.ByteToHexStringSimple(keyArray);
                                        }
                                        else
                                        {
                                            descriptionToInsert = "GET SECURITY SEED #1 | PCM ALREADY UNLOCKED";
                                        }
                                    }
                                    else if (payload[0] == 2)
                                    {
                                        if (seed != 0)
                                        {
                                            ushort key = (ushort)(seed & 0xFF00);
                                            key |= (ushort)(key >> 8);

                                            ushort mask = (ushort)(seed & 0xFF);
                                            mask |= (ushort)(mask << 8);
                                            key ^= 0x9340; // polinom
                                            key += 0x1010;
                                            key ^= mask;
                                            key += 0x1911;
                                            uint tmp = (uint)((key << 16) | key);
                                            key += (ushort)(tmp >> 3);
                                            byte keyHB = (byte)(key >> 8);
                                            byte keyLB = (byte)(key);
                                            byte keyChecksum = (byte)(0x2C + keyHB + keyLB);
                                            byte[] keyArray = { 0x2C, keyHB, keyLB, keyChecksum };
                                            descriptionToInsert = "GET SECURITY SEED #2 | KEY: " + Util.ByteToHexStringSimple(keyArray);
                                        }
                                        else
                                        {
                                            descriptionToInsert = "GET SECURITY SEED #2 | PCM ALREADY UNLOCKED";
                                        }
                                    }

                                    valueToInsert = Util.ByteToHexString(payload, 1, 2);
                                }
                                else
                                {
                                    if (payload[0] == 1)
                                    {
                                        descriptionToInsert = "GET SECURITY SEED #1";
                                    }
                                    else if (payload[0] == 2)
                                    {
                                        descriptionToInsert = "GET SECURITY SEED #2";
                                    }
                                    
                                    valueToInsert = "CHECKSUM ERROR";
                                }
                            }
                            else // error
                            {
                                descriptionToInsert = "GET SECURITY SEED";
                                valueToInsert = "ERROR";
                            }
                            unitToInsert = string.Empty;
                            break;
                        case 0xFE: // select low-speed mode
                            if (message.Length >= minLength)
                            {
                                valueToInsert = string.Empty;
                            }
                            else // error
                            {
                                valueToInsert = "ERROR";
                            }
                            unitToInsert = string.Empty;
                            break;
                        case 0xFF: // PCM wake up
                            descriptionToInsert = "PCM  WAKE UP";
                            valueToInsert = string.Empty;
                            unitToInsert = string.Empty;
                            break;
                        default:
                            descriptionToInsert = string.Empty;
                            valueToInsert = string.Empty;
                            unitToInsert = string.Empty;
                            break;
                    }
                }
                else if ((speed == "62500 baud") || (speed == "125000 baud"))
                {
                    switch (ID)
                    {
                        case 0x00: // PCM wake up
                            descriptionToInsert = "PCM WAKE UP";
                            valueToInsert = string.Empty;
                            unitToInsert = string.Empty;
                            break;
                        case 0x06: // set bootstrap baudrate result
                            descriptionToInsert = "SET BOOTSTRAP BAUDRATE TO 62500 BAUD";
                            valueToInsert = "OK";
                            unitToInsert = string.Empty;
                            break;
                        case 0x11: // upload worker function result
                            if (message.Length >= 3)
                            {
                                ushort size = (ushort)((payload[0] << 8) + payload[1]);
                                ushort echoCount = (ushort)(payload.Length - 3);

                                descriptionToInsert = "UPLOAD WORKER FUNCTION | SIZE: " + size.ToString() + " BYTES";
                                valueToInsert = Util.ByteToHexString(payload, 2, payload.Length - 3);

                                if ((echoCount == size) && (payload[payload.Length - 1] == 0x14))
                                {
                                    unitToInsert = "OK";
                                }
                                else
                                {
                                    unitToInsert = "ERROR";
                                }
                            }
                            else
                            {
                                descriptionToInsert = "UPLOAD WORKER FUNCTION";
                                valueToInsert = "ERROR";
                                unitToInsert = string.Empty;
                            }
                            break;
                        case 0x21: // start worker function result
                            descriptionToInsert = "START WORKER FUNCTION";

                            if (message.Length == 1)
                            {
                                valueToInsert = string.Empty;
                                
                            }
                            else if (message.Length > 1)
                            {
                                descriptionToInsert += " | RESULT";
                                valueToInsert = Util.ByteToHexStringSimple(payload.ToArray());
                            }
                            else if ((message.Length == 2) && (payload[0] == 0x22))
                            {
                                valueToInsert = "FINISHED";
                            }

                            unitToInsert = string.Empty;
                            break;
                        case 0x22: // exit worker function
                            descriptionToInsert = "EXIT WORKER FUNCTION";
                            valueToInsert = string.Empty;
                            unitToInsert = string.Empty;
                            break;
                        case 0x24: // request bootstrap security seed / send bootstrap security key
                            if (message.Length >= 5)
                            {
                                if (message.Length == 5)
                                {
                                    byte checksum = (byte)(message[0] + message[1] + message[2] + message[3]);

                                    descriptionToInsert = "REQUEST BOOTSTRAP SECURITY SEED";

                                    if (message[4] == checksum)
                                    {
                                        if ((message[2] == 0x27) && (message[3] == 0xC1))
                                        {
                                            valueToInsert = "OK";
                                        }
                                        else
                                        {
                                            valueToInsert = "ERROR";
                                        }
                                    }
                                    else
                                    {
                                        valueToInsert = "CHECKSUM ERROR";
                                    }
                                }
                                else if (message.Length == 7)
                                {
                                    byte checksum = (byte)(message[0] + message[1] + message[2] + message[3] + message[4] + message[5]);

                                    descriptionToInsert = "SEND BOOTSTRAP SECURITY KEY";

                                    if (message[6] == checksum)
                                    {
                                        if ((message[2] == 0x27) && (message[3] == 0xC2))
                                        {
                                            valueToInsert = Util.ByteToHexString(message, 4, 2);
                                        }
                                        else
                                        {
                                            valueToInsert = "ERROR";
                                        }
                                    }
                                    else
                                    {
                                        valueToInsert = "CHECKSUM ERROR";
                                    }
                                }
                                else
                                {
                                    descriptionToInsert = "REQUEST BOOTSTRAP SECURITY SEED";
                                    valueToInsert = string.Empty;
                                }
                            }
                            else
                            {
                                descriptionToInsert = "REQUEST BOOTSTRAP SECURITY SEED";
                                valueToInsert = string.Empty;
                            }
                            
                            unitToInsert = string.Empty;
                            break;
                        case 0x26: // bootstrap security seed received
                            if (message.Length >= 5)
                            {
                                if (message.Length == 5)
                                {
                                    byte checksum = (byte)(message[0] + message[1] + message[2] + message[3]);

                                    descriptionToInsert = "BOOTSTRAP SECURITY STATUS";

                                    if (message[4] == checksum)
                                    {
                                        if ((message[2] == 0x67) && (message[3] == 0xC2))
                                        {
                                            valueToInsert = "UNLOCKED";
                                        }
                                        else
                                        {
                                            valueToInsert = "LOCKED";
                                        }
                                    }
                                    else
                                    {
                                        valueToInsert = "CHECKSUM ERROR";
                                    }
                                }
                                else if (message.Length == 7)
                                {
                                    byte checksum = (byte)(message[0] + message[1] + message[2] + message[3] + message[4] + message[5]);

                                    descriptionToInsert = "BOOTSTRAP SECURITY SEED RECEIVED";

                                    if (message[6] == checksum)
                                    {
                                        if ((message[2] == 0x67) && (message[3] == 0xC1))
                                        {
                                            valueToInsert = Util.ByteToHexString(payload, 3, 2);
                                        }
                                    }
                                    else
                                    {
                                        valueToInsert = "CHECKSUM ERROR";
                                    }
                                    
                                }
                                else
                                {
                                    descriptionToInsert = "BOOTSTRAP SECURITY STATUS";
                                    valueToInsert = string.Empty;
                                }
                            }

                            unitToInsert = string.Empty;
                            break;
                        case 0x31: // write flash block
                            if (message.Length >= minLength)
                            {
                                List<byte> offset = new List<byte>();
                                List<byte> length = new List<byte>();
                                List<byte> values = new List<byte>();
                                offset.Clear();
                                length.Clear();
                                values.Clear();
                                offset.AddRange(payload.Take(3));
                                length.AddRange(payload.Skip(3).Take(2));
                                values.AddRange(payload.Skip(5));

                                ushort blockSize = (ushort)((payload[3] << 8) + payload[4]);
                                ushort echoCount = (ushort)(payload.Length - 5);

                                descriptionToInsert = "WRITE FLASH BLOCK | OFFSET: " + Util.ByteToHexStringSimple(offset.ToArray()) + " | SIZE: " + Util.ByteToHexStringSimple(length.ToArray());

                                if (echoCount == blockSize)
                                {
                                    valueToInsert = Util.ByteToHexStringSimple(values.ToArray());
                                    unitToInsert = "OK";
                                }
                                else
                                {
                                    switch (message[message.Length - 1]) // last payload byte stores error status
                                    {
                                        case 0x01:
                                            valueToInsert = "WRITE ERROR";
                                            break;
                                        case 0x80:
                                            valueToInsert = "INVALID BLOCK SIZE";
                                            break;
                                        default:
                                            valueToInsert = "UNKNOWN ERROR";
                                            break;
                                    }

                                    unitToInsert = string.Empty;
                                }
                            }
                            else
                            {
                                descriptionToInsert = "WRITE FLASH BLOCK";
                                valueToInsert = string.Empty;
                                unitToInsert = string.Empty;
                            }
                            break;
                        case 0x34: // read flash block
                            if (message.Length >= minLength)
                            {
                                List<byte> offset = new List<byte>();
                                List<byte> length = new List<byte>();
                                List<byte> values = new List<byte>();
                                offset.Clear();
                                length.Clear();
                                values.Clear();
                                offset.AddRange(payload.Take(3));
                                length.AddRange(payload.Skip(3).Take(2));
                                values.AddRange(payload.Skip(5));

                                descriptionToInsert = "READ FLASH BLOCK | OFFSET: " + Util.ByteToHexStringSimple(offset.ToArray()) + " | SIZE: " + Util.ByteToHexStringSimple(length.ToArray());

                                ushort blockSize = (ushort)((payload[3] << 8) + payload[4]);
                                ushort echoCount = (ushort)(payload.Length - 5);

                                if (echoCount == blockSize)
                                {
                                    valueToInsert = Util.ByteToHexStringSimple(values.ToArray());
                                    unitToInsert = "OK";
                                }
                                else
                                {
                                    switch (message[message.Length - 1]) // last payload byte stores error status
                                    {
                                        case 0x80:
                                            valueToInsert = "INVALID BLOCK SIZE";
                                            break;
                                        default:
                                            valueToInsert = "UNKNOWN ERROR";
                                            break;
                                    }

                                    unitToInsert = string.Empty;
                                }
                            }
                            else
                            {
                                descriptionToInsert = "READ FLASH BLOCK";
                                valueToInsert = string.Empty;
                                unitToInsert = string.Empty;
                            }                         
                            break;
                        case 0x37: // write EEPROM block
                            if (message.Length >= minLength)
                            {
                                List<byte> offset = new List<byte>();
                                List<byte> length = new List<byte>();
                                List<byte> values = new List<byte>();
                                offset.Clear();
                                length.Clear();
                                values.Clear();
                                offset.AddRange(payload.Take(2));
                                length.AddRange(payload.Skip(2).Take(2));
                                values.AddRange(payload.Skip(4));

                                descriptionToInsert = "WRITE EEPROM BLOCK | OFFSET: " + Util.ByteToHexStringSimple(offset.ToArray()) + " | SIZE: " + Util.ByteToHexStringSimple(length.ToArray());

                                ushort blockSize = (ushort)((payload[2] << 8) + payload[3]);
                                ushort echoCount = (ushort)(payload.Length - 4);

                                if ((echoCount == blockSize) && (offset[0] < 2))
                                {
                                    valueToInsert = Util.ByteToHexStringSimple(values.ToArray());
                                    unitToInsert = "OK";
                                }
                                else
                                {
                                    switch (message[message.Length - 1]) // last payload byte stores error status
                                    {
                                        case 0x80:
                                            valueToInsert = "INVALID BLOCK SIZE";
                                            break;
                                        case 0x83:
                                            valueToInsert = "INVALID OFFSET";
                                            break;
                                        default:
                                            valueToInsert = "UNKNOWN ERROR";
                                            break;
                                    }

                                    unitToInsert = string.Empty;
                                }
                            }
                            else
                            {
                                descriptionToInsert = "WRITE EEPROM BLOCK";
                                valueToInsert = string.Empty;
                                unitToInsert = string.Empty;
                            }
                            break;
                        case 0x3A: // read EEPROM block
                            if (message.Length >= minLength)
                            {
                                List<byte> offset = new List<byte>();
                                List<byte> length = new List<byte>();
                                List<byte> values = new List<byte>();
                                offset.Clear();
                                length.Clear();
                                values.Clear();
                                offset.AddRange(payload.Take(2));
                                length.AddRange(payload.Skip(2).Take(2));
                                values.AddRange(payload.Skip(4));

                                descriptionToInsert = "READ EEPROM BLOCK | OFFSET: " + Util.ByteToHexStringSimple(offset.ToArray()) + " | SIZE: " + Util.ByteToHexStringSimple(length.ToArray());

                                ushort blockSize = (ushort)((payload[2] << 8) + payload[3]);
                                ushort echoCount = (ushort)(payload.Length - 4);

                                if ((echoCount == blockSize) && (offset[0] < 2))
                                {
                                    valueToInsert = Util.ByteToHexStringSimple(values.ToArray());
                                    unitToInsert = "OK";
                                }
                                else
                                {
                                    switch (message[message.Length - 1]) // last payload byte stores error status
                                    {
                                        case 0x80:
                                            valueToInsert = "INVALID BLOCK SIZE";
                                            break;
                                        case 0x83:
                                            valueToInsert = "INVALID OFFSET";
                                            break;
                                        default:
                                            valueToInsert = "UNKNOWN ERROR";
                                            break;
                                    }

                                    unitToInsert = string.Empty;
                                }
                            }
                            else
                            {
                                descriptionToInsert = "READ EEPROM BLOCK";
                                valueToInsert = string.Empty;
                                unitToInsert = string.Empty;
                            }
                            break;
                        case 0x46: // read flash block
                            if (message.Length >= minLength)
                            {
                                List<byte> offset = new List<byte>();
                                List<byte> length = new List<byte>();
                                List<byte> values = new List<byte>();
                                offset.Clear();
                                length.Clear();
                                values.Clear();
                                offset.AddRange(payload.Take(3));
                                length.AddRange(payload.Skip(3).Take(2));
                                values.AddRange(payload.Skip(5));
                                descriptionToInsert = "READ FLASH BLOCK | OFFSET: " + Util.ByteToHexStringSimple(offset.ToArray()) + " | SIZE: " + Util.ByteToHexStringSimple(length.ToArray());
                                valueToInsert = Util.ByteToHexStringSimple(values.ToArray());

                                ushort blockSize = (ushort)((payload[3] << 8) + payload[4]);
                                ushort echoCount = (ushort)(payload.Length - 5);

                                if (echoCount == blockSize)
                                {
                                    
                                    unitToInsert = "OK";
                                }
                                else
                                {
                                    unitToInsert = "READ ERROR";
                                }
                            }
                            else
                            {
                                descriptionToInsert = "READ FLASH BLOCK";
                                valueToInsert = string.Empty;
                                unitToInsert = string.Empty;
                            }
                            break;
                        case 0x47: // start bootloader command
                            if (message.Length >= minLength)
                            {
                                List<byte> offset = new List<byte>();
                                offset.Clear();
                                offset.AddRange(payload.Take(2));
                                descriptionToInsert = "START BOOTLOADER | OFFSET: " + Util.ByteToHexStringSimple(offset.ToArray());

                                if (payload[2] == 0x22) valueToInsert = "OK";
                                else valueToInsert = "ERROR";
                            }
                            else
                            {
                                descriptionToInsert = "START BOOTLOADER";
                                valueToInsert = string.Empty;
                            }
                            unitToInsert = string.Empty;
                            break;
                        case 0x4C: // upload bootloader command
                            if (message.Length >= minLength)
                            {
                                List<byte> offsetStart = new List<byte>();
                                List<byte> offsetEnd = new List<byte>();
                                offsetStart.Clear();
                                offsetEnd.Clear();
                                offsetStart.AddRange(payload.Take(2));
                                offsetEnd.AddRange(payload.Skip(2).Take(2));
                                descriptionToInsert = "UPLOAD BOOTLOADER | START: " + Util.ByteToHexStringSimple(offsetStart.ToArray()) + " | END: " + Util.ByteToHexStringSimple(offsetEnd.ToArray());
                                valueToInsert = Util.ByteToHexString(payload, 4, payload.Length - 4);

                                ushort start = (ushort)((payload[0] << 8) + payload[1]);
                                ushort end = (ushort)((payload[2] << 8) + payload[3]);

                                if ((end - start + 1) == (payload.Length - 4))
                                {
                                    unitToInsert = "OK";
                                }
                                else
                                {
                                    unitToInsert = "ERROR";
                                }
                            }
                            else
                            {
                                descriptionToInsert = "UPLOAD BOOTLOADER";
                                valueToInsert = string.Empty;
                                unitToInsert = string.Empty;
                            }
                            break;
                        case 0xDB: // bootstrap mode not protected
                            if (message.Length >= minLength)
                            {
                                if (payload[0] == 0x2F && payload[1] == 0xD8 && payload[2] == 0x3E && payload[3] == 0x23)
                                {
                                    descriptionToInsert = "BOOTSTRAP MODE NOT PROTECTED";
                                }
                                else
                                {
                                    descriptionToInsert = "PING";
                                }
                            }
                            else
                            {
                                descriptionToInsert = string.Empty;
                            }
                            valueToInsert = string.Empty;
                            unitToInsert = string.Empty;
                            break;
                        case 0xF0: // high-speed mode RAM table
                            if (payload.Length > 1)
                            {
                                ushort num = (ushort)(payload.Length / 2);
                                List<byte> offsets = new List<byte>();
                                List<byte> values = new List<byte>();
                                offsets.Clear();
                                values.Clear();

                                for (int i = 0; i < num; i++)
                                {
                                    offsets.Add(payload[i * 2]);
                                    values.Add(payload[(i * 2) + 1]);
                                }

                                descriptionToInsert = "F0 RAM TABLE VALUE | OFFSET: " + Util.ByteToHexStringSimple(offsets.ToArray());
                                valueToInsert = Util.ByteToHexStringSimple(values.ToArray());
                            }
                            else
                            {
                                descriptionToInsert = "F0 RAM TABLE SELECTED";
                                valueToInsert = string.Empty;
                            }
                            unitToInsert = string.Empty;
                            break;
                        case 0xF1: // high-speed mode RAM table
                            if (payload.Length > 1)
                            {
                                ushort num = (ushort)(payload.Length / 2);
                                List<byte> offsets = new List<byte>();
                                List<byte> values = new List<byte>();
                                offsets.Clear();
                                values.Clear();

                                for (int i = 0; i < num; i++)
                                {
                                    offsets.Add(payload[i * 2]);
                                    values.Add(payload[(i * 2) + 1]);
                                }

                                descriptionToInsert = "F1 RAM TABLE VALUE | OFFSET: " + Util.ByteToHexStringSimple(offsets.ToArray());
                                valueToInsert = Util.ByteToHexStringSimple(values.ToArray());
                            }
                            else
                            {
                                descriptionToInsert = "F1 RAM TABLE SELECTED";
                                valueToInsert = string.Empty;
                            }
                            unitToInsert = string.Empty;
                            break;
                        case 0xF2: // high-speed mode RAM table
                            if (payload.Length > 1)
                            {
                                ushort num = (ushort)(payload.Length / 2);
                                List<byte> offsets = new List<byte>();
                                List<byte> values = new List<byte>();
                                offsets.Clear();
                                values.Clear();

                                for (int i = 0; i < num; i++)
                                {
                                    offsets.Add(payload[i * 2]);
                                    values.Add(payload[(i * 2) + 1]);
                                }

                                descriptionToInsert = "F2 RAM TABLE VALUE | OFFSET: " + Util.ByteToHexStringSimple(offsets.ToArray());
                                valueToInsert = Util.ByteToHexStringSimple(values.ToArray());
                            }
                            else
                            {
                                descriptionToInsert = "F2 RAM TABLE SELECTED";
                                valueToInsert = string.Empty;
                            }
                            unitToInsert = string.Empty;
                            break;
                        case 0xF3: // high-speed mode RAM table
                            if (payload.Length > 1)
                            {
                                ushort num = (ushort)(payload.Length / 2);
                                List<byte> offsets = new List<byte>();
                                List<byte> values = new List<byte>();
                                offsets.Clear();
                                values.Clear();

                                for (int i = 0; i < num; i++)
                                {
                                    offsets.Add(payload[i * 2]);
                                    values.Add(payload[(i * 2) + 1]);
                                }

                                descriptionToInsert = "F3 RAM TABLE VALUE | OFFSET: " + Util.ByteToHexStringSimple(offsets.ToArray());
                                valueToInsert = Util.ByteToHexStringSimple(values.ToArray());
                            }
                            else
                            {
                                descriptionToInsert = "F3 RAM TABLE SELECTED";
                                valueToInsert = string.Empty;
                            }
                            unitToInsert = string.Empty;
                            break;
                        case 0xF4: // high-speed mode RAM table
                            if (payload.Length > 1)
                            {
                                switch (payload[0]) // RAM offset
                                {
                                    case 0x01:
                                        descriptionToInsert = "DIAGNOSTIC TROUBLE CODE";

                                        if (payload.Length >= 2)
                                        {
                                            valueToInsert = Util.ByteToHexString(payload, 1, 1);
                                        }
                                        else // error
                                        {
                                            valueToInsert = "ERROR";
                                        }

                                        unitToInsert = string.Empty;
                                        break;
                                    case 0x0A:
                                        descriptionToInsert = "ENGINE SPEED";

                                        if ((payload.Length >= 4) && (payload[2] == 0x0B))
                                        {
                                            valueToInsert = Math.Round(((payload[1] << 8) + payload[3]) * 0.125D, 3).ToString("0.000").Replace(",", ".");
                                            unitToInsert = "RPM";
                                        }
                                        else // error
                                        {
                                            valueToInsert = "ERROR";
                                            unitToInsert = string.Empty;
                                        }
                                        break;
                                    case 0x0C:
                                        descriptionToInsert = "VEHICLE SPEED";

                                        if ((payload.Length >= 4) && (payload[2] == 0x0D))
                                        {
                                            if (Properties.Settings.Default.Units == "imperial")
                                            {
                                                valueToInsert = Math.Round(((payload[1] << 8) + payload[3]) * 0.0156D, 3).ToString("0.000").Replace(",", ".");
                                                unitToInsert = "MPH";
                                            }
                                            else if (Properties.Settings.Default.Units == "metric")
                                            {
                                                valueToInsert = Math.Round(((payload[1] << 8) + payload[3]) * 0.0156D * 1.609344D, 3).ToString("0.000").Replace(",", ".");
                                                unitToInsert = "KM/H";
                                            }
                                        }
                                        else // error
                                        {
                                            valueToInsert = "ERROR";
                                            unitToInsert = string.Empty;
                                        }
                                        break;
                                    case 0x0E:
                                        descriptionToInsert = "CRUISE | CONTROL SWITCH PRESSED";

                                        if (payload.Length >= 2)
                                        {
                                            List<string> switchList = new List<string>();
                                            switchList.Clear();

                                            if (Util.IsBitSet(payload[1], 0)) switchList.Add("CANCEL");
                                            if (Util.IsBitSet(payload[1], 1)) switchList.Add("COAST");
                                            if (Util.IsBitClear(payload[1], 2)) switchList.Add("SET");
                                            if (Util.IsBitSet(payload[1], 3)) switchList.Add("ACC/RES");
                                            if (Util.IsBitSet(payload[1], 4)) switchList.Add("ON/OFF");
                                            if (Util.IsBitSet(payload[1], 7)) switchList.Add("BRAKE");

                                            if (switchList.Count > 0)
                                            {
                                                foreach (string s in switchList)
                                                {
                                                    valueToInsert += s + " | ";
                                                }

                                                if (valueToInsert.Length > 2) valueToInsert = valueToInsert.Remove(valueToInsert.Length - 3); // remove last "|" character
                                            }
                                            else
                                            {
                                                valueToInsert = string.Empty;
                                            }
                                        }
                                        else // error
                                        {
                                            valueToInsert = "ERROR";
                                        }

                                        unitToInsert = string.Empty;
                                        break;
                                    case 0x0F:
                                        descriptionToInsert = "BATTERY VOLTAGE";

                                        if (payload.Length >= 2)
                                        {
                                            valueToInsert = Math.Round(payload[1] * 0.0618D, 3).ToString("0.000").Replace(",", ".");
                                            unitToInsert = "V";
                                        }
                                        else // error
                                        {
                                            valueToInsert = "ERROR";
                                            unitToInsert = string.Empty;
                                        }
                                        break;
                                    case 0x10:
                                        descriptionToInsert = "AMBIENT TEMPERATURE SENSOR VOLTAGE";

                                        if (payload.Length >= 2)
                                        {
                                            valueToInsert = Math.Round(payload[1] * 0.0196D, 3).ToString("0.000").Replace(",", ".");
                                            unitToInsert = "V";
                                        }
                                        else // error
                                        {
                                            valueToInsert = "ERROR";
                                            unitToInsert = string.Empty;
                                        }
                                        break;
                                    case 0x11:
                                        descriptionToInsert = "AMBIENT TEMPERATURE";

                                        if (payload.Length >= 2)
                                        {
                                            if (Properties.Settings.Default.Units == "imperial")
                                            {
                                                valueToInsert = Math.Round((payload[1] * 1.8D) - 198.4D).ToString("0");
                                                unitToInsert = "°F";
                                            }
                                            else if (Properties.Settings.Default.Units == "metric")
                                            {
                                                valueToInsert = (payload[1] - 128).ToString("0");
                                                unitToInsert = "°C";
                                            }
                                        }
                                        else // error
                                        {
                                            valueToInsert = "ERROR";
                                            unitToInsert = string.Empty;
                                        }
                                        break;
                                    case 0x12:
                                        descriptionToInsert = "THROTTLE POSITION SENSOR (TPS) VOLTAGE";

                                        if (payload.Length >= 2)
                                        {
                                            valueToInsert = Math.Round(payload[1] * 0.0196D, 3).ToString("0.000").Replace(",", ".");
                                            unitToInsert = "V";
                                        }
                                        else // error
                                        {
                                            valueToInsert = "ERROR";
                                            unitToInsert = string.Empty;
                                        }
                                        break;
                                    case 0x13:
                                        descriptionToInsert = "MINIMUM TPS VOLTAGE";

                                        if (payload.Length >= 2)
                                        {
                                            valueToInsert = Math.Round(payload[1] * 0.0196D, 3).ToString("0.000").Replace(",", ".");
                                            unitToInsert = "V";
                                        }
                                        else // error
                                        {
                                            valueToInsert = "ERROR";
                                            unitToInsert = string.Empty;
                                        }
                                        break;
                                    case 0x14:
                                        descriptionToInsert = "CALCULATED TPS VOLTAGE";

                                        if (payload.Length >= 2)
                                        {
                                            valueToInsert = Math.Round(payload[1] * 0.0196D, 3).ToString("0.000").Replace(",", ".");
                                            unitToInsert = "V";
                                        }
                                        else // error
                                        {
                                            valueToInsert = "ERROR";
                                            unitToInsert = string.Empty;
                                        }
                                        break;
                                    case 0x15:
                                        descriptionToInsert = "ENGINE COOLANT TEMPERATURE SENSOR VOLTAGE";

                                        if (payload.Length >= 2)
                                        {
                                            valueToInsert = Math.Round(payload[1] * 0.0196D, 3).ToString("0.000").Replace(",", ".");
                                            unitToInsert = "V";
                                        }
                                        else // error
                                        {
                                            valueToInsert = "ERROR";
                                            unitToInsert = string.Empty;
                                        }
                                        break;
                                    case 0x16:
                                        descriptionToInsert = "ENGINE COOLANT TEMPERATURE";

                                        if (payload.Length >= 2)
                                        {
                                            if (Properties.Settings.Default.Units == "imperial")
                                            {
                                                valueToInsert = Math.Round((payload[1] * 1.8D) - 198.4D).ToString("0");
                                                unitToInsert = "°F";
                                            }
                                            else if (Properties.Settings.Default.Units == "metric")
                                            {
                                                valueToInsert = (payload[1] - 128).ToString("0");
                                                unitToInsert = "°C";
                                            }
                                        }
                                        else // error
                                        {
                                            valueToInsert = "ERROR";
                                            unitToInsert = string.Empty;
                                        }
                                        break;
                                    case 0x21:
                                        descriptionToInsert = "CRUISE | SWITCH VOLTAGE";

                                        if (payload.Length >= 2)
                                        {
                                            valueToInsert = Math.Round(payload[1] * 0.0196D, 3).ToString("0.000").Replace(",", ".");
                                            unitToInsert = "V";
                                        }
                                        else // error
                                        {
                                            valueToInsert = "ERROR";
                                            unitToInsert = string.Empty;
                                        }
                                        break;
                                    case 0x35:
                                        descriptionToInsert = "TARGET IDLE SPEED";

                                        if ((payload.Length >= 4) && (payload[2] == 0x36))
                                        {
                                            valueToInsert = Math.Round(((payload[1] << 8) + payload[3]) * 0.125D, 3).ToString("0.000").Replace(",", ".");
                                            unitToInsert = "RPM";
                                        }
                                        else // error
                                        {
                                            valueToInsert = "ERROR";
                                            unitToInsert = string.Empty;
                                        }
                                        break;
                                    case 0x37:
                                        descriptionToInsert = "TARGET IDLE AIR CONTROL MOTOR STEPS";

                                        if (payload.Length >= 2)
                                        {
                                            valueToInsert = payload[1].ToString();
                                        }
                                        else // error
                                        {
                                            valueToInsert = "ERROR";
                                        }

                                        unitToInsert = string.Empty;
                                        break;
                                    case 0x3A:
                                        descriptionToInsert = "CHARGING VOLTAGE";

                                        if (payload.Length >= 2)
                                        {
                                            valueToInsert = Math.Round(payload[1] * 0.0618D, 3).ToString("0.000").Replace(",", ".");
                                            unitToInsert = "V";
                                        }
                                        else // error
                                        {
                                            valueToInsert = "ERROR";
                                            unitToInsert = string.Empty;
                                        }
                                        break;
                                    case 0x3B:
                                        descriptionToInsert = "CRUISE | SET SPEED";

                                        if (payload.Length >= 2)
                                        {
                                            if (Properties.Settings.Default.Units == "imperial")
                                            {
                                                valueToInsert = Math.Round(payload[1] / 2.0D, 1).ToString("0.0").Replace(",", ".");
                                                unitToInsert = "MPH";
                                            }
                                            else if (Properties.Settings.Default.Units == "metric")
                                            {
                                                valueToInsert = Math.Round((payload[1] / 2.0D) * 1.609344D, 1).ToString("0.0").Replace(",", ".");
                                                unitToInsert = "KM/H";
                                            }
                                        }
                                        else // error
                                        {
                                            valueToInsert = "ERROR";
                                            unitToInsert = string.Empty;
                                        }
                                        break;
                                    case 0x3E:
                                        descriptionToInsert = "IDLE AIR CONTROL MOTOR STEPS";

                                        if (payload.Length >= 2)
                                        {
                                            valueToInsert = payload[1].ToString();
                                        }
                                        else // error
                                        {
                                            valueToInsert = "ERROR";
                                        }

                                        unitToInsert = string.Empty;
                                        break;
                                    case 0x3F:
                                        if (payload.Length >= 2)
                                        {
                                            string lastCruiseCutoutReason = string.Empty;
                                            string cruiseDeniedReason = string.Empty;

                                            switch (payload[1] & 0xF0) // upper 4 bits encode last cutout reason 
                                            {
                                                case 0x00:
                                                    lastCruiseCutoutReason = "ON/OFF SW";
                                                    break;
                                                case 0x10:
                                                    lastCruiseCutoutReason = "SPEED SEN";
                                                    break;
                                                case 0x20:
                                                    lastCruiseCutoutReason = "RPM LIMIT";
                                                    break;
                                                case 0x30:
                                                    lastCruiseCutoutReason = "BRAKE SW";
                                                    break;
                                                case 0x40:
                                                    lastCruiseCutoutReason = "P/N SW";
                                                    break;
                                                case 0x50:
                                                    lastCruiseCutoutReason = "RPM/SPEED";
                                                    break;
                                                case 0x60:
                                                    lastCruiseCutoutReason = "CLUTCH";
                                                    break;
                                                case 0x70:
                                                    lastCruiseCutoutReason = "S/C DTC";
                                                    break;
                                                case 0x80:
                                                    lastCruiseCutoutReason = "KEY OFF";
                                                    break;
                                                case 0x90:
                                                    lastCruiseCutoutReason = "ACTIVE";
                                                    break;
                                                case 0xA0:
                                                    lastCruiseCutoutReason = "CLUTCH UP";
                                                    break;
                                                case 0xB0:
                                                    lastCruiseCutoutReason = "N/A";
                                                    break;
                                                case 0xC0:
                                                    lastCruiseCutoutReason = "SW DTC";
                                                    break;
                                                case 0xD0:
                                                    lastCruiseCutoutReason = "CANCEL";
                                                    break;
                                                case 0xE0:
                                                    lastCruiseCutoutReason = "LIMP-IN";
                                                    break;
                                                case 0xF0:
                                                    lastCruiseCutoutReason = "12V DTC";
                                                    break;
                                                default:
                                                    lastCruiseCutoutReason = "N/A";
                                                    break;
                                            }

                                            switch (payload[1] & 0x0F) // lower 4 bits encode denied reason 
                                            {
                                                case 0x00:
                                                    cruiseDeniedReason = "ON/OFF SW";
                                                    break;
                                                case 0x01:
                                                    cruiseDeniedReason = "SPEED SEN";
                                                    break;
                                                case 0x02:
                                                    cruiseDeniedReason = "RPM LIMIT";
                                                    break;
                                                case 0x03:
                                                    cruiseDeniedReason = "BRAKE SW";
                                                    break;
                                                case 0x04:
                                                    cruiseDeniedReason = "P/N SW";
                                                    break;
                                                case 0x05:
                                                    cruiseDeniedReason = "RPM/SPEED";
                                                    break;
                                                case 0x06:
                                                    cruiseDeniedReason = "CLUTCH";
                                                    break;
                                                case 0x07:
                                                    cruiseDeniedReason = "S/C DTC";
                                                    break;
                                                case 0x08:
                                                    cruiseDeniedReason = "ALLOWED";
                                                    break;
                                                case 0x09:
                                                    cruiseDeniedReason = "ACTIVE";
                                                    break;
                                                case 0x0A:
                                                    cruiseDeniedReason = "CLUTCH UP";
                                                    break;
                                                case 0x0B:
                                                    cruiseDeniedReason = "N/A";
                                                    break;
                                                case 0x0C:
                                                    cruiseDeniedReason = "SW DTC";
                                                    break;
                                                case 0x0D:
                                                    cruiseDeniedReason = "CANCEL";
                                                    break;
                                                case 0x0E:
                                                    cruiseDeniedReason = "LIMP-IN";
                                                    break;
                                                case 0x0F:
                                                    cruiseDeniedReason = "12V DTC";
                                                    break;
                                                default:
                                                    cruiseDeniedReason = "N/A";
                                                    break;
                                            }

                                            if ((payload[1] & 0x0F) == 0x08)
                                            {
                                                descriptionToInsert = "CRUISE | LAST CUTOUT: " + lastCruiseCutoutReason + " | STATE: " + cruiseDeniedReason;
                                                valueToInsert = "STOPPED";
                                            }
                                            else if ((payload[1] & 0x0F) == 0x09)
                                            {
                                                descriptionToInsert = "CRUISE | LAST CUTOUT: " + lastCruiseCutoutReason + " | STATE: " + cruiseDeniedReason;
                                                valueToInsert = "ENGAGED";
                                            }
                                            else
                                            {
                                                descriptionToInsert = "CRUISE | LAST CUTOUT: " + lastCruiseCutoutReason + " | DENIED: " + cruiseDeniedReason;
                                                valueToInsert = "STOPPED";
                                            }

                                            unitToInsert = string.Empty;
                                        }
                                        else // error
                                        {
                                            descriptionToInsert = "CRUISE STATE";
                                            valueToInsert = "ERROR";
                                            unitToInsert = string.Empty;
                                        }
                                        break;
                                    case 0x41: // cam/crank sensor state
                                        descriptionToInsert = "CRANKSHAFT/CAMSHAFT POSITION SENSOR STATE";

                                        if (payload.Length >= 2)
                                        {
                                            if (Util.IsBitSet(payload[1], 5)) descriptionToInsert = "CKP: PRESENT | ";
                                            else descriptionToInsert = "CKP: LOST | ";

                                            if (Util.IsBitSet(payload[1], 6)) descriptionToInsert += "CMP: PRESENT | ";
                                            else descriptionToInsert += "CMP: LOST | ";

                                            if (Util.IsBitSet(payload[1], 4)) descriptionToInsert += "CKP/CMP: IN-SYNC";
                                            else descriptionToInsert += "CKP/CMP: OUT-OF-SYNC";

                                            if (Util.IsBitSet(payload[1], 0)) valueToInsert = "HISTORY: IN-SYNC";
                                            else valueToInsert = "HISTORY: OUT-OF-SYNC";


                                            //valueToInsert = Util.ByteToHexString(payload, 1, 1);
                                        }
                                        else // error
                                        {
                                            valueToInsert = "ERROR";
                                        }

                                        unitToInsert = string.Empty;
                                        break;
                                    default:
                                        ushort num = (ushort)(payload.Length / 2);
                                        List<byte> offsets = new List<byte>();
                                        List<byte> values = new List<byte>();
                                        offsets.Clear();
                                        values.Clear();

                                        for (int i = 0; i < num; i++)
                                        {
                                            offsets.Add(payload[i * 2]);
                                            values.Add(payload[(i * 2) + 1]);
                                        }

                                        descriptionToInsert = "F4 RAM TABLE VALUE | OFFSET: " + Util.ByteToHexStringSimple(offsets.ToArray());
                                        valueToInsert = Util.ByteToHexStringSimple(values.ToArray());
                                        break;
                                }
                            }
                            else // RAM table only
                            {
                                descriptionToInsert = "F4 RAM TABLE SELECTED";
                                valueToInsert = string.Empty;
                                unitToInsert = string.Empty;
                            }
                            break;
                        case 0xF5: // high-speed mode RAM table
                            if (payload.Length > 1)
                            {
                                ushort num = (ushort)(payload.Length / 2);
                                List<byte> offsets = new List<byte>();
                                List<byte> values = new List<byte>();
                                offsets.Clear();
                                values.Clear();

                                for (int i = 0; i < num; i++)
                                {
                                    offsets.Add(payload[i * 2]);
                                    values.Add(payload[(i * 2) + 1]);
                                }

                                descriptionToInsert = "F5 RAM TABLE VALUE | OFFSET: " + Util.ByteToHexStringSimple(offsets.ToArray());
                                valueToInsert = Util.ByteToHexStringSimple(values.ToArray());
                            }
                            else
                            {
                                descriptionToInsert = "F5 RAM TABLE SELECTED";
                                valueToInsert = string.Empty;
                            }
                            unitToInsert = string.Empty;
                            break;
                        case 0xF6: // high-speed mode RAM table
                            if (payload.Length > 1)
                            {
                                ushort num = (ushort)(payload.Length / 2);
                                List<byte> offsets = new List<byte>();
                                List<byte> values = new List<byte>();
                                offsets.Clear();
                                values.Clear();

                                for (int i = 0; i < num; i++)
                                {
                                    offsets.Add(payload[i * 2]);
                                    values.Add(payload[(i * 2) + 1]);
                                }

                                descriptionToInsert = "F6 RAM TABLE VALUE | OFFSET: " + Util.ByteToHexStringSimple(offsets.ToArray());
                                valueToInsert = Util.ByteToHexStringSimple(values.ToArray());
                            }
                            else
                            {
                                descriptionToInsert = "F6 RAM TABLE SELECTED";
                                valueToInsert = string.Empty;
                            }
                            unitToInsert = string.Empty;
                            break;
                        case 0xF7: // high-speed mode RAM table
                            if (payload.Length > 1)
                            {
                                ushort num = (ushort)(payload.Length / 2);
                                List<byte> offsets = new List<byte>();
                                List<byte> values = new List<byte>();
                                offsets.Clear();
                                values.Clear();

                                for (int i = 0; i < num; i++)
                                {
                                    offsets.Add(payload[i * 2]);
                                    values.Add(payload[(i * 2) + 1]);
                                }

                                descriptionToInsert = "F7 RAM TABLE VALUE | OFFSET: " + Util.ByteToHexStringSimple(offsets.ToArray());
                                valueToInsert = Util.ByteToHexStringSimple(values.ToArray());
                            }
                            else
                            {
                                descriptionToInsert = "F7 RAM TABLE SELECTED";
                                valueToInsert = string.Empty;
                            }
                            unitToInsert = string.Empty;
                            break;
                        case 0xF8: // high-speed mode RAM table
                            if (payload.Length > 1)
                            {
                                ushort num = (ushort)(payload.Length / 2);
                                List<byte> offsets = new List<byte>();
                                List<byte> values = new List<byte>();
                                offsets.Clear();
                                values.Clear();

                                for (int i = 0; i < num; i++)
                                {
                                    offsets.Add(payload[i * 2]);
                                    values.Add(payload[(i * 2) + 1]);
                                }

                                descriptionToInsert = "F8 RAM TABLE VALUE | OFFSET: " + Util.ByteToHexStringSimple(offsets.ToArray());
                                valueToInsert = Util.ByteToHexStringSimple(values.ToArray());
                            }
                            else
                            {
                                descriptionToInsert = "F8 RAM TABLE SELECTED";
                                valueToInsert = string.Empty;
                            }
                            unitToInsert = string.Empty;
                            break;
                        case 0xF9: // high-speed mode RAM table
                            if (payload.Length > 1)
                            {
                                ushort num = (ushort)(payload.Length / 2);
                                List<byte> offsets = new List<byte>();
                                List<byte> values = new List<byte>();
                                offsets.Clear();
                                values.Clear();

                                for (int i = 0; i < num; i++)
                                {
                                    offsets.Add(payload[i * 2]);
                                    values.Add(payload[(i * 2) + 1]);
                                }

                                descriptionToInsert = "F9 RAM TABLE VALUE | OFFSET: " + Util.ByteToHexStringSimple(offsets.ToArray());
                                valueToInsert = Util.ByteToHexStringSimple(values.ToArray());
                            }
                            else
                            {
                                descriptionToInsert = "F9 RAM TABLE SELECTED";
                                valueToInsert = string.Empty;
                            }
                            unitToInsert = string.Empty;
                            break;
                        case 0xFA: // high-speed mode RAM table
                            if (payload.Length > 1)
                            {
                                ushort num = (ushort)(payload.Length / 2);
                                List<byte> offsets = new List<byte>();
                                List<byte> values = new List<byte>();
                                offsets.Clear();
                                values.Clear();

                                for (int i = 0; i < num; i++)
                                {
                                    offsets.Add(payload[i * 2]);
                                    values.Add(payload[(i * 2) + 1]);
                                }

                                descriptionToInsert = "FA RAM TABLE VALUE | OFFSET: " + Util.ByteToHexStringSimple(offsets.ToArray());
                                valueToInsert = Util.ByteToHexStringSimple(values.ToArray());
                            }
                            else
                            {
                                descriptionToInsert = "FA RAM TABLE SELECTED";
                                valueToInsert = string.Empty;
                            }
                            unitToInsert = string.Empty;
                            break;
                        case 0xFB: // high-speed mode RAM table
                            if (payload.Length > 1)
                            {
                                ushort num = (ushort)(payload.Length / 2);
                                List<byte> offsets = new List<byte>();
                                List<byte> values = new List<byte>();
                                offsets.Clear();
                                values.Clear();

                                for (int i = 0; i < num; i++)
                                {
                                    offsets.Add(payload[i * 2]);
                                    values.Add(payload[(i * 2) + 1]);
                                }

                                descriptionToInsert = "FB RAM TABLE VALUE | OFFSET: " + Util.ByteToHexStringSimple(offsets.ToArray());
                                valueToInsert = Util.ByteToHexStringSimple(values.ToArray());
                            }
                            else
                            {
                                descriptionToInsert = "FB RAM TABLE SELECTED";
                                valueToInsert = string.Empty;
                            }
                            unitToInsert = string.Empty;
                            break;
                        case 0xFC: // high-speed mode RAM table
                            if (payload.Length > 1)
                            {
                                ushort num = (ushort)(payload.Length / 2);
                                List<byte> offsets = new List<byte>();
                                List<byte> values = new List<byte>();
                                offsets.Clear();
                                values.Clear();

                                for (int i = 0; i < num; i++)
                                {
                                    offsets.Add(payload[i * 2]);
                                    values.Add(payload[(i * 2) + 1]);
                                }

                                descriptionToInsert = "FC RAM TABLE VALUE | OFFSET: " + Util.ByteToHexStringSimple(offsets.ToArray());
                                valueToInsert = Util.ByteToHexStringSimple(values.ToArray());
                            }
                            else
                            {
                                descriptionToInsert = "FC RAM TABLE SELECTED";
                                valueToInsert = string.Empty;
                            }
                            unitToInsert = string.Empty;
                            break;
                        case 0xFD: // high-speed mode RAM table
                            if (payload.Length > 1)
                            {
                                ushort num = (ushort)(payload.Length / 2);
                                List<byte> offsets = new List<byte>();
                                List<byte> values = new List<byte>();
                                offsets.Clear();
                                values.Clear();

                                for (int i = 0; i < num; i++)
                                {
                                    offsets.Add(payload[i * 2]);
                                    values.Add(payload[(i * 2) + 1]);
                                }

                                descriptionToInsert = "FD RAM TABLE VALUE | OFFSET: " + Util.ByteToHexStringSimple(offsets.ToArray());
                                valueToInsert = Util.ByteToHexStringSimple(values.ToArray());
                            }
                            else
                            {
                                descriptionToInsert = "FD RAM TABLE SELECTED";
                                valueToInsert = string.Empty;
                            }
                            unitToInsert = string.Empty;
                            break;
                        case 0xFE: // select low-speed mode
                            if (message.Length >= minLength)
                            {
                                valueToInsert = string.Empty;
                            }
                            else // error
                            {
                                valueToInsert = "ERROR";
                            }
                            unitToInsert = string.Empty;
                            break;
                        case 0xFF: // PCM wake up
                            descriptionToInsert = "PCM WAKE UP";
                            valueToInsert = string.Empty;
                            unitToInsert = string.Empty;
                            break;
                        default:
                            descriptionToInsert = string.Empty;
                            valueToInsert = string.Empty;
                            unitToInsert = string.Empty;
                            break;
                    }
                }

                MessageDatabase.Rows[rowIndex]["message"] = hexBytesToInsert; // edit data table
                MessageDatabase.Rows[rowIndex]["description"] = descriptionToInsert; // edit data table
                MessageDatabase.Rows[rowIndex]["value"] = valueToInsert; // edit data table
                MessageDatabase.Rows[rowIndex]["unit"] = unitToInsert; // edit data table
            }

            if (message.Length < 9) // max 8 message byte fits the message column
            {
                hexBytesToInsert = Util.ByteToHexString(message, 0, message.Length) + " ";
            }
            else // trim message (display 7 bytes only) and insert two dots at the end indicating there's more to it
            {
                hexBytesToInsert = Util.ByteToHexString(message, 0, 7) + " .. ";
            }

            if (descriptionToInsert.Length > 51) descriptionToInsert = Util.TruncateString(descriptionToInsert, 48) + "...";
            if (valueToInsert.Length > 23) valueToInsert = Util.TruncateString(valueToInsert, 20) + "...";
            if (unitToInsert.Length > 11) unitToInsert = Util.TruncateString(unitToInsert, 8) + "...";

            rowToAdd.Remove(hexBytesColumnStart, hexBytesToInsert.Length);
            rowToAdd.Insert(hexBytesColumnStart, hexBytesToInsert);

            rowToAdd.Remove(descriptionColumnStart, descriptionToInsert.Length);
            rowToAdd.Insert(descriptionColumnStart, descriptionToInsert);

            rowToAdd.Remove(valueColumnStart, valueToInsert.Length);
            rowToAdd.Insert(valueColumnStart, valueToInsert);

            rowToAdd.Remove(unitColumnStart, unitToInsert.Length);
            rowToAdd.Insert(unitColumnStart, unitToInsert);

            Diagnostics.AddRow(modifiedID, rowToAdd.ToString());

            if (speed == "62500 baud") Diagnostics.AddRAMTableDump(data);

            UpdateHeader();

            if (Properties.Settings.Default.Timestamp == true)
            {
                TimeSpan ElapsedTime = TimeSpan.FromMilliseconds(timestamp[0] << 24 | timestamp[1] << 16 | timestamp[2] << 8 | timestamp[3]);
                DateTime Timestamp = DateTime.Today.Add(ElapsedTime);
                string TimestampString = Timestamp.ToString("HH:mm:ss.fff") + " ";
                File.AppendAllText(MainForm.PCMLogFilename, TimestampString); // no newline is appended!
            }

            File.AppendAllText(MainForm.PCMLogFilename, Util.ByteToHexStringSimple(message) + Environment.NewLine);

            if (!engineFaultCodesSaved)
            {
                StringBuilder sb = new StringBuilder();

                if (engineFaultCodeList.Count > 0)
                {
                    sb.Append("FAULT CODE LIST:" + Environment.NewLine);

                    foreach (byte code in engineFaultCodeList)
                    {
                        int index = EngineDTC.Rows.IndexOf(EngineDTC.Rows.Find(code));
                        byte[] temp = new byte[1] { code };

                        if (index > -1) // DTC description found
                        {
                            sb.Append(Util.ByteToHexStringSimple(temp) + ": " + EngineDTC.Rows[index]["description"] + Environment.NewLine);
                        }
                        else // no DTC description found
                        {
                            sb.Append(Util.ByteToHexStringSimple(temp) + ": -" + Environment.NewLine);
                        }
                    }

                    sb.Remove(sb.Length - 1, 1); // remove last newline character

                    File.AppendAllText(MainForm.PCMLogFilename, Environment.NewLine + sb.ToString() + Environment.NewLine);
                }
                else
                {
                    sb.Append("NO FAULT CODES");
                    File.AppendAllText(MainForm.PCMLogFilename, Environment.NewLine + sb.ToString() + Environment.NewLine + Environment.NewLine);
                }

                engineFaultCodesSaved = true;
            }
        }
    }
}
