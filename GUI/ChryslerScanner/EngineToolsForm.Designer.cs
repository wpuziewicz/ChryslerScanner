﻿namespace ChryslerScanner
{
    partial class EngineToolsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.FaultCodeGroupBox = new System.Windows.Forms.GroupBox();
            this.EraseFaultCodesButton = new System.Windows.Forms.Button();
            this.ReadFaultCodesButton = new System.Windows.Forms.Button();
            this.BaudrateGroupBox = new System.Windows.Forms.GroupBox();
            this.Baud976Button = new System.Windows.Forms.Button();
            this.Baud125000Button = new System.Windows.Forms.Button();
            this.Baud62500Button = new System.Windows.Forms.Button();
            this.Baud7812Button = new System.Windows.Forms.Button();
            this.ActuatorTestGroupBox = new System.Windows.Forms.GroupBox();
            this.ActuatorTestComboBox = new System.Windows.Forms.ComboBox();
            this.ActuatorTestStopButton = new System.Windows.Forms.Button();
            this.ActuatorTestStartButton = new System.Windows.Forms.Button();
            this.DiagnosticDataGroupBox = new System.Windows.Forms.GroupBox();
            this.DiagnosticDataCSVCheckBox = new System.Windows.Forms.CheckBox();
            this.MillisecondsLabel01 = new System.Windows.Forms.Label();
            this.DiagnosticDataRepeatIntervalTextBox = new System.Windows.Forms.TextBox();
            this.DiagnosticDataRepeatIntervalCheckBox = new System.Windows.Forms.CheckBox();
            this.DiagnosticDataClearButton = new System.Windows.Forms.Button();
            this.DiagnosticDataStopButton = new System.Windows.Forms.Button();
            this.DiagnosticDataListBox = new System.Windows.Forms.ListBox();
            this.DiagnosticDataReadButton = new System.Windows.Forms.Button();
            this.SetIdleSpeedGroupBox = new System.Windows.Forms.GroupBox();
            this.IdleSpeedNoteLabel = new System.Windows.Forms.Label();
            this.RPMLabel = new System.Windows.Forms.Label();
            this.SetIdleSpeedTextBox = new System.Windows.Forms.TextBox();
            this.SetIdleSpeedTrackBar = new System.Windows.Forms.TrackBar();
            this.SetIdleSpeedStopButton = new System.Windows.Forms.Button();
            this.SetIdleSpeedSetButton = new System.Windows.Forms.Button();
            this.ResetMemoryGroupBox = new System.Windows.Forms.GroupBox();
            this.ResetMemoryComboBox = new System.Windows.Forms.ComboBox();
            this.ResetMemoryOKButton = new System.Windows.Forms.Button();
            this.SecurityGroupBox = new System.Windows.Forms.GroupBox();
            this.LegacySecurityCheckBox = new System.Windows.Forms.CheckBox();
            this.SecurityLevelComboBox = new System.Windows.Forms.ComboBox();
            this.SecurityUnlockButton = new System.Windows.Forms.Button();
            this.FaultCodeGroupBox.SuspendLayout();
            this.BaudrateGroupBox.SuspendLayout();
            this.ActuatorTestGroupBox.SuspendLayout();
            this.DiagnosticDataGroupBox.SuspendLayout();
            this.SetIdleSpeedGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SetIdleSpeedTrackBar)).BeginInit();
            this.ResetMemoryGroupBox.SuspendLayout();
            this.SecurityGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // FaultCodeGroupBox
            // 
            this.FaultCodeGroupBox.Controls.Add(this.EraseFaultCodesButton);
            this.FaultCodeGroupBox.Controls.Add(this.ReadFaultCodesButton);
            this.FaultCodeGroupBox.Location = new System.Drawing.Point(9, 7);
            this.FaultCodeGroupBox.Name = "FaultCodeGroupBox";
            this.FaultCodeGroupBox.Size = new System.Drawing.Size(126, 46);
            this.FaultCodeGroupBox.TabIndex = 0;
            this.FaultCodeGroupBox.TabStop = false;
            this.FaultCodeGroupBox.Text = "Fault code";
            // 
            // EraseFaultCodesButton
            // 
            this.EraseFaultCodesButton.Location = new System.Drawing.Point(65, 16);
            this.EraseFaultCodesButton.Name = "EraseFaultCodesButton";
            this.EraseFaultCodesButton.Size = new System.Drawing.Size(55, 23);
            this.EraseFaultCodesButton.TabIndex = 16;
            this.EraseFaultCodesButton.Text = "Erase";
            this.EraseFaultCodesButton.UseVisualStyleBackColor = true;
            this.EraseFaultCodesButton.Click += new System.EventHandler(this.EraseFaultCodesButton_Click);
            // 
            // ReadFaultCodesButton
            // 
            this.ReadFaultCodesButton.Location = new System.Drawing.Point(6, 16);
            this.ReadFaultCodesButton.Name = "ReadFaultCodesButton";
            this.ReadFaultCodesButton.Size = new System.Drawing.Size(55, 23);
            this.ReadFaultCodesButton.TabIndex = 15;
            this.ReadFaultCodesButton.Text = "Read";
            this.ReadFaultCodesButton.UseVisualStyleBackColor = true;
            this.ReadFaultCodesButton.Click += new System.EventHandler(this.ReadFaultCodesButton_Click);
            // 
            // BaudrateGroupBox
            // 
            this.BaudrateGroupBox.Controls.Add(this.Baud976Button);
            this.BaudrateGroupBox.Controls.Add(this.Baud125000Button);
            this.BaudrateGroupBox.Controls.Add(this.Baud62500Button);
            this.BaudrateGroupBox.Controls.Add(this.Baud7812Button);
            this.BaudrateGroupBox.Location = new System.Drawing.Point(142, 7);
            this.BaudrateGroupBox.Name = "BaudrateGroupBox";
            this.BaudrateGroupBox.Size = new System.Drawing.Size(246, 46);
            this.BaudrateGroupBox.TabIndex = 1;
            this.BaudrateGroupBox.TabStop = false;
            this.BaudrateGroupBox.Text = "Baudrate";
            // 
            // Baud976Button
            // 
            this.Baud976Button.Enabled = false;
            this.Baud976Button.Location = new System.Drawing.Point(7, 16);
            this.Baud976Button.Name = "Baud976Button";
            this.Baud976Button.Size = new System.Drawing.Size(55, 23);
            this.Baud976Button.TabIndex = 20;
            this.Baud976Button.Text = "976.5";
            this.Baud976Button.UseVisualStyleBackColor = true;
            this.Baud976Button.Click += new System.EventHandler(this.Baud976Button_Click);
            // 
            // Baud125000Button
            // 
            this.Baud125000Button.Enabled = false;
            this.Baud125000Button.Location = new System.Drawing.Point(184, 16);
            this.Baud125000Button.Name = "Baud125000Button";
            this.Baud125000Button.Size = new System.Drawing.Size(55, 23);
            this.Baud125000Button.TabIndex = 19;
            this.Baud125000Button.Text = "125000";
            this.Baud125000Button.UseVisualStyleBackColor = true;
            this.Baud125000Button.Click += new System.EventHandler(this.Baud125000Button_Click);
            // 
            // Baud62500Button
            // 
            this.Baud62500Button.Location = new System.Drawing.Point(125, 16);
            this.Baud62500Button.Name = "Baud62500Button";
            this.Baud62500Button.Size = new System.Drawing.Size(55, 23);
            this.Baud62500Button.TabIndex = 18;
            this.Baud62500Button.Text = "62500";
            this.Baud62500Button.UseVisualStyleBackColor = true;
            this.Baud62500Button.Click += new System.EventHandler(this.Baud62500Button_Click);
            // 
            // Baud7812Button
            // 
            this.Baud7812Button.Location = new System.Drawing.Point(66, 16);
            this.Baud7812Button.Name = "Baud7812Button";
            this.Baud7812Button.Size = new System.Drawing.Size(55, 23);
            this.Baud7812Button.TabIndex = 17;
            this.Baud7812Button.Text = "7812.5";
            this.Baud7812Button.UseVisualStyleBackColor = true;
            this.Baud7812Button.Click += new System.EventHandler(this.Baud7812Button_Click);
            // 
            // ActuatorTestGroupBox
            // 
            this.ActuatorTestGroupBox.Controls.Add(this.ActuatorTestComboBox);
            this.ActuatorTestGroupBox.Controls.Add(this.ActuatorTestStopButton);
            this.ActuatorTestGroupBox.Controls.Add(this.ActuatorTestStartButton);
            this.ActuatorTestGroupBox.Location = new System.Drawing.Point(9, 55);
            this.ActuatorTestGroupBox.Name = "ActuatorTestGroupBox";
            this.ActuatorTestGroupBox.Size = new System.Drawing.Size(379, 73);
            this.ActuatorTestGroupBox.TabIndex = 1;
            this.ActuatorTestGroupBox.TabStop = false;
            this.ActuatorTestGroupBox.Text = "Actuator test";
            // 
            // ActuatorTestComboBox
            // 
            this.ActuatorTestComboBox.DropDownHeight = 226;
            this.ActuatorTestComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ActuatorTestComboBox.Font = new System.Drawing.Font("Courier New", 8.25F);
            this.ActuatorTestComboBox.FormattingEnabled = true;
            this.ActuatorTestComboBox.IntegralHeight = false;
            this.ActuatorTestComboBox.Items.AddRange(new object[] {
            "1300 |",
            "1301 | Ignition coil bank #1",
            "1302 | Ignition coil bank #2",
            "1303 | Ignition coil bank #3",
            "1304 | Fuel injector bank #1",
            "1305 | Fuel injector bank #2",
            "1306 | Fuel injector bank #3",
            "1307 | Idle air control (IAC) stepper motor",
            "1308 | Radiator fan relay",
            "1309 | A/C clutch relay",
            "130A | Automatic shutdown (ASD) relay",
            "130B | Evap purge solenoid",
            "130C | Speed control solenoids (vacuum/vent)",
            "130D | Alternator field",
            "130E | Tachometer output",
            "130F | Torque converter clutch",
            "1310 | EGR solenoid",
            "1311 | Wastegate solenoid",
            "1312 | Barometer solenoid",
            "1313 |",
            "1314 | All solenoids / relays",
            "1315 |",
            "1316 | Transmission O/D solenoid",
            "1317 | Shift indicator lamp",
            "1318 |",
            "1319 | Surge valve solenoid",
            "131A | Speed control vent solenoid",
            "131B | Speed control vacuum solenoid",
            "131C | ASD fuel system",
            "131D | Fuel injector bank #4",
            "131E | Fuel injector bank #5",
            "131F | Fuel injector bank #6",
            "1320 |",
            "1321 |",
            "1322 |",
            "1323 | Ignition coil bank #4",
            "1324 | Ignition coil bank #5",
            "1325 | Fuel injector bank #7",
            "1326 | Fuel injector bank #8",
            "1327 |",
            "1328 | Intake heater bank #1",
            "1329 | Intake heater bank #2",
            "132A |",
            "132B |",
            "132C | Speed control 12 volt feed",
            "132D | Intake manifold tune valve",
            "132E | Low speed radiator fan relay",
            "132F | High speed radiator fan relay",
            "1330 | Fuel injector bank #9",
            "1331 | Fuel injector bank #10",
            "1332 | 2-3 lockout solenoid",
            "1333 | Fuel pump relay",
            "1334 |",
            "1335 |",
            "1336 |",
            "1337 |",
            "1338 |",
            "1339 |",
            "133A |",
            "133B | IAC stepper motor step up",
            "133C | IAC stepper motor step down",
            "133D | Leak detection pump solenoid",
            "133E |",
            "133F |",
            "1340 | O2 sensor heater relay",
            "1341 | Overdrive lamp",
            "1342 |",
            "1343 | Transmission 12 volt relay",
            "1344 | Reverse lockout solenoid",
            "1345 |",
            "1346 | Short runner valve",
            "1347 |",
            "1348 |",
            "1349 | Wait to start lamp",
            "134A |",
            "134B |",
            "134C |",
            "134D |",
            "134E |",
            "134F |",
            "1350 |",
            "1351 |",
            "1352 | 1/1 2/1 O2 sensor heater relay",
            "1353 | 1/2 2/2 O2 sensor heater relay",
            "1354 |",
            "1355 |",
            "1356 | 1/1 O2 sensor heater relay",
            "1357 | O2 sensor heater relay",
            "1358 |",
            "1359 |",
            "135A | Radiator fan solenoid",
            "135B | 1/2 O2 sensor heater relay",
            "135C |",
            "135D | Exhaust brake",
            "135E | Fuel control",
            "135F | PWM radiator fan",
            "1360 |",
            "1361 |",
            "1362 |",
            "1363 |",
            "1364 |",
            "1365 |",
            "1366 |",
            "1367 |",
            "1368 |",
            "1369 |",
            "136A |",
            "136B |",
            "136C |",
            "136D |",
            "136E |",
            "136F |",
            "1370 |",
            "1371 |",
            "1372 |",
            "1373 |",
            "1374 |",
            "1375 |",
            "1376 |",
            "1377 |",
            "1378 |",
            "1379 |",
            "137A |",
            "137B |",
            "137C |",
            "137D |",
            "137E |",
            "137F |"});
            this.ActuatorTestComboBox.Location = new System.Drawing.Point(7, 16);
            this.ActuatorTestComboBox.Name = "ActuatorTestComboBox";
            this.ActuatorTestComboBox.Size = new System.Drawing.Size(365, 22);
            this.ActuatorTestComboBox.TabIndex = 29;
            // 
            // ActuatorTestStopButton
            // 
            this.ActuatorTestStopButton.Location = new System.Drawing.Point(65, 43);
            this.ActuatorTestStopButton.Name = "ActuatorTestStopButton";
            this.ActuatorTestStopButton.Size = new System.Drawing.Size(55, 23);
            this.ActuatorTestStopButton.TabIndex = 20;
            this.ActuatorTestStopButton.Text = "Stop";
            this.ActuatorTestStopButton.UseVisualStyleBackColor = true;
            this.ActuatorTestStopButton.Click += new System.EventHandler(this.ActuatorTestStopButton_Click);
            // 
            // ActuatorTestStartButton
            // 
            this.ActuatorTestStartButton.Location = new System.Drawing.Point(6, 43);
            this.ActuatorTestStartButton.Name = "ActuatorTestStartButton";
            this.ActuatorTestStartButton.Size = new System.Drawing.Size(55, 23);
            this.ActuatorTestStartButton.TabIndex = 19;
            this.ActuatorTestStartButton.Text = "Start";
            this.ActuatorTestStartButton.UseVisualStyleBackColor = true;
            this.ActuatorTestStartButton.Click += new System.EventHandler(this.ActuatorTestStartButton_Click);
            // 
            // DiagnosticDataGroupBox
            // 
            this.DiagnosticDataGroupBox.Controls.Add(this.DiagnosticDataCSVCheckBox);
            this.DiagnosticDataGroupBox.Controls.Add(this.MillisecondsLabel01);
            this.DiagnosticDataGroupBox.Controls.Add(this.DiagnosticDataRepeatIntervalTextBox);
            this.DiagnosticDataGroupBox.Controls.Add(this.DiagnosticDataRepeatIntervalCheckBox);
            this.DiagnosticDataGroupBox.Controls.Add(this.DiagnosticDataClearButton);
            this.DiagnosticDataGroupBox.Controls.Add(this.DiagnosticDataStopButton);
            this.DiagnosticDataGroupBox.Controls.Add(this.DiagnosticDataListBox);
            this.DiagnosticDataGroupBox.Controls.Add(this.DiagnosticDataReadButton);
            this.DiagnosticDataGroupBox.Location = new System.Drawing.Point(9, 130);
            this.DiagnosticDataGroupBox.Name = "DiagnosticDataGroupBox";
            this.DiagnosticDataGroupBox.Size = new System.Drawing.Size(379, 168);
            this.DiagnosticDataGroupBox.TabIndex = 2;
            this.DiagnosticDataGroupBox.TabStop = false;
            this.DiagnosticDataGroupBox.Text = "Diagnostic data";
            // 
            // DiagnosticDataCSVCheckBox
            // 
            this.DiagnosticDataCSVCheckBox.AutoSize = true;
            this.DiagnosticDataCSVCheckBox.Checked = true;
            this.DiagnosticDataCSVCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DiagnosticDataCSVCheckBox.Location = new System.Drawing.Point(325, 142);
            this.DiagnosticDataCSVCheckBox.Name = "DiagnosticDataCSVCheckBox";
            this.DiagnosticDataCSVCheckBox.Size = new System.Drawing.Size(47, 17);
            this.DiagnosticDataCSVCheckBox.TabIndex = 27;
            this.DiagnosticDataCSVCheckBox.Text = "CSV";
            this.DiagnosticDataCSVCheckBox.UseVisualStyleBackColor = true;
            // 
            // MillisecondsLabel01
            // 
            this.MillisecondsLabel01.AutoSize = true;
            this.MillisecondsLabel01.Location = new System.Drawing.Point(281, 143);
            this.MillisecondsLabel01.Name = "MillisecondsLabel01";
            this.MillisecondsLabel01.Size = new System.Drawing.Size(20, 13);
            this.MillisecondsLabel01.TabIndex = 24;
            this.MillisecondsLabel01.Text = "ms";
            // 
            // DiagnosticDataRepeatIntervalTextBox
            // 
            this.DiagnosticDataRepeatIntervalTextBox.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.DiagnosticDataRepeatIntervalTextBox.Location = new System.Drawing.Point(246, 139);
            this.DiagnosticDataRepeatIntervalTextBox.Name = "DiagnosticDataRepeatIntervalTextBox";
            this.DiagnosticDataRepeatIntervalTextBox.Size = new System.Drawing.Size(34, 21);
            this.DiagnosticDataRepeatIntervalTextBox.TabIndex = 26;
            this.DiagnosticDataRepeatIntervalTextBox.Text = "50";
            // 
            // DiagnosticDataRepeatIntervalCheckBox
            // 
            this.DiagnosticDataRepeatIntervalCheckBox.AutoSize = true;
            this.DiagnosticDataRepeatIntervalCheckBox.Checked = true;
            this.DiagnosticDataRepeatIntervalCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DiagnosticDataRepeatIntervalCheckBox.Location = new System.Drawing.Point(184, 142);
            this.DiagnosticDataRepeatIntervalCheckBox.Name = "DiagnosticDataRepeatIntervalCheckBox";
            this.DiagnosticDataRepeatIntervalCheckBox.Size = new System.Drawing.Size(64, 17);
            this.DiagnosticDataRepeatIntervalCheckBox.TabIndex = 25;
            this.DiagnosticDataRepeatIntervalCheckBox.Text = "Repeat:";
            this.DiagnosticDataRepeatIntervalCheckBox.UseVisualStyleBackColor = true;
            this.DiagnosticDataRepeatIntervalCheckBox.CheckedChanged += new System.EventHandler(this.DiagnosticDataRepeatIntervalCheckBox_CheckedChanged);
            // 
            // DiagnosticDataClearButton
            // 
            this.DiagnosticDataClearButton.Location = new System.Drawing.Point(124, 138);
            this.DiagnosticDataClearButton.Name = "DiagnosticDataClearButton";
            this.DiagnosticDataClearButton.Size = new System.Drawing.Size(55, 23);
            this.DiagnosticDataClearButton.TabIndex = 23;
            this.DiagnosticDataClearButton.Text = "Clear";
            this.DiagnosticDataClearButton.UseVisualStyleBackColor = true;
            this.DiagnosticDataClearButton.Click += new System.EventHandler(this.DiagnosticDataClearButton_Click);
            // 
            // DiagnosticDataStopButton
            // 
            this.DiagnosticDataStopButton.Location = new System.Drawing.Point(65, 138);
            this.DiagnosticDataStopButton.Name = "DiagnosticDataStopButton";
            this.DiagnosticDataStopButton.Size = new System.Drawing.Size(55, 23);
            this.DiagnosticDataStopButton.TabIndex = 22;
            this.DiagnosticDataStopButton.Text = "Stop";
            this.DiagnosticDataStopButton.UseVisualStyleBackColor = true;
            this.DiagnosticDataStopButton.Click += new System.EventHandler(this.DiagnosticDataStopButton_Click);
            // 
            // DiagnosticDataListBox
            // 
            this.DiagnosticDataListBox.Font = new System.Drawing.Font("Courier New", 8.25F);
            this.DiagnosticDataListBox.FormattingEnabled = true;
            this.DiagnosticDataListBox.ItemHeight = 14;
            this.DiagnosticDataListBox.Items.AddRange(new object[] {
            "1400 |",
            "1401 | Ambient air temperature sensor voltage",
            "1402 | Upstream O2 sensor voltage",
            "1403 |",
            "1404 |",
            "1405 | Engine coolant temperature",
            "1406 | Engine coolant temperature sensor voltage",
            "1407 | Throttle position sensor voltage",
            "1408 | Minimum throttle position sensor voltage",
            "1409 | Knock sensor voltage",
            "140A | Battery voltage",
            "140B | Intake manifold absolute pressure",
            "140C | Target IAC stepper motor position",
            "140D |",
            "140E | Long term fuel trim bank 1",
            "140F | Barometric pressure",
            "1410 | Minimum airflow",
            "1411 | Engine speed",
            "1412 | CAM/CRANK sync sense",
            "1413 | Key-on cycles error 1",
            "1414 |",
            "1415 | Spark advance",
            "1416 | Cylinder 1 retard",
            "1417 | Cylinder 2 retard",
            "1418 | Cylinder 3 retard",
            "1419 | Cylinder 4 retard",
            "141A | Target boost",
            "141B | Intake air temperature",
            "141C | Intake air temperature sensor voltage",
            "141D | Cruise target speed",
            "141E | Key-on cycles error 2",
            "141F | Key-on cycles error 3",
            "1420 | Cruise status",
            "1421 |",
            "1422 |",
            "1423 |",
            "1424 | Target battery charging voltage",
            "1425 | Over 5 psi boost timer",
            "1426 | Wastegate duty cycle",
            "1427 | Theft alarm status",
            "1428 | Wastegate duty cycle",
            "1429 | Read fuel setting",
            "142A | Read set sync",
            "142B |",
            "142C |",
            "142D |",
            "142E |",
            "142F | Upstream O2 sensor voltage",
            "1430 |",
            "1431 | Long term fuel trim bank 2",
            "1432 | A/C high side pressure sensor voltage",
            "1433 | A/C high side pressure",
            "1434 |",
            "1435 |",
            "1436 |",
            "1437 |",
            "1438 |",
            "1439 |",
            "143A |",
            "143B | Fuel system status",
            "143C |",
            "143D |",
            "143E | Read fuel factor LH",
            "143F | Downstream O2 sensor voltage",
            "1440 | Intake MAP sensor voltage",
            "1441 | Vehicle speed",
            "1442 | Upstream O2 level",
            "1443 |",
            "1444 |",
            "1445 |",
            "1446 | Throttle position sensor",
            "1447 |",
            "1448 | Downstream O2 level",
            "1449 |",
            "144A |",
            "144B |",
            "144C |",
            "144D |",
            "144E | Fuel level sensor voltage",
            "144F | Fuel level",
            "1450 |",
            "1451 |",
            "1452 |",
            "1453 |",
            "1454 |",
            "1455 |",
            "1456 |",
            "1457 | Fuel system status 2",
            "1458 | Cruise status 1",
            "1459 | Cruise status 2",
            "145A | Output shaft speed",
            "145B | Governor pressure duty cycle",
            "145C | Calculated engine load",
            "145D |",
            "145E |",
            "145F |",
            "1460 |",
            "1461 |",
            "1462 |",
            "1463 |",
            "1464 |",
            "1465 |",
            "1466 |",
            "1467 |",
            "1468 |",
            "1469 |",
            "146A |",
            "146B |",
            "146C |",
            "146D | T-case switch voltage",
            "146E |",
            "146F |",
            "1470 |",
            "1471 |",
            "1472 |",
            "1473 |",
            "1474 |",
            "1475 |",
            "1476 |",
            "1477 |",
            "1478 |",
            "1479 |",
            "147A | FCA current",
            "147B |",
            "147C | Oil temperature sensor voltage",
            "147D | Oil temperature",
            "147E |",
            "147F |"});
            this.DiagnosticDataListBox.Location = new System.Drawing.Point(7, 17);
            this.DiagnosticDataListBox.Name = "DiagnosticDataListBox";
            this.DiagnosticDataListBox.ScrollAlwaysVisible = true;
            this.DiagnosticDataListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.DiagnosticDataListBox.Size = new System.Drawing.Size(365, 116);
            this.DiagnosticDataListBox.TabIndex = 4;
            // 
            // DiagnosticDataReadButton
            // 
            this.DiagnosticDataReadButton.Location = new System.Drawing.Point(6, 138);
            this.DiagnosticDataReadButton.Name = "DiagnosticDataReadButton";
            this.DiagnosticDataReadButton.Size = new System.Drawing.Size(55, 23);
            this.DiagnosticDataReadButton.TabIndex = 21;
            this.DiagnosticDataReadButton.Text = "Read";
            this.DiagnosticDataReadButton.UseVisualStyleBackColor = true;
            this.DiagnosticDataReadButton.Click += new System.EventHandler(this.DiagnosticDataReadButton_Click);
            // 
            // SetIdleSpeedGroupBox
            // 
            this.SetIdleSpeedGroupBox.Controls.Add(this.IdleSpeedNoteLabel);
            this.SetIdleSpeedGroupBox.Controls.Add(this.RPMLabel);
            this.SetIdleSpeedGroupBox.Controls.Add(this.SetIdleSpeedTextBox);
            this.SetIdleSpeedGroupBox.Controls.Add(this.SetIdleSpeedTrackBar);
            this.SetIdleSpeedGroupBox.Controls.Add(this.SetIdleSpeedStopButton);
            this.SetIdleSpeedGroupBox.Controls.Add(this.SetIdleSpeedSetButton);
            this.SetIdleSpeedGroupBox.Location = new System.Drawing.Point(9, 300);
            this.SetIdleSpeedGroupBox.Name = "SetIdleSpeedGroupBox";
            this.SetIdleSpeedGroupBox.Size = new System.Drawing.Size(379, 95);
            this.SetIdleSpeedGroupBox.TabIndex = 28;
            this.SetIdleSpeedGroupBox.TabStop = false;
            this.SetIdleSpeedGroupBox.Text = "Set idle speed";
            // 
            // IdleSpeedNoteLabel
            // 
            this.IdleSpeedNoteLabel.AutoSize = true;
            this.IdleSpeedNoteLabel.Location = new System.Drawing.Point(121, 70);
            this.IdleSpeedNoteLabel.Name = "IdleSpeedNoteLabel";
            this.IdleSpeedNoteLabel.Size = new System.Drawing.Size(193, 13);
            this.IdleSpeedNoteLabel.TabIndex = 27;
            this.IdleSpeedNoteLabel.Text = "Default idle speed is restored after stop.";
            // 
            // RPMLabel
            // 
            this.RPMLabel.AutoSize = true;
            this.RPMLabel.Location = new System.Drawing.Point(9, 49);
            this.RPMLabel.Name = "RPMLabel";
            this.RPMLabel.Size = new System.Drawing.Size(31, 13);
            this.RPMLabel.TabIndex = 27;
            this.RPMLabel.Text = "RPM";
            // 
            // SetIdleSpeedTextBox
            // 
            this.SetIdleSpeedTextBox.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.SetIdleSpeedTextBox.Location = new System.Drawing.Point(7, 24);
            this.SetIdleSpeedTextBox.Name = "SetIdleSpeedTextBox";
            this.SetIdleSpeedTextBox.Size = new System.Drawing.Size(34, 21);
            this.SetIdleSpeedTextBox.TabIndex = 27;
            this.SetIdleSpeedTextBox.Text = "1500";
            this.SetIdleSpeedTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SetIdleSpeedTextBox_KeyPress);
            // 
            // SetIdleSpeedTrackBar
            // 
            this.SetIdleSpeedTrackBar.LargeChange = 100;
            this.SetIdleSpeedTrackBar.Location = new System.Drawing.Point(39, 14);
            this.SetIdleSpeedTrackBar.Maximum = 2000;
            this.SetIdleSpeedTrackBar.Name = "SetIdleSpeedTrackBar";
            this.SetIdleSpeedTrackBar.Size = new System.Drawing.Size(337, 45);
            this.SetIdleSpeedTrackBar.SmallChange = 50;
            this.SetIdleSpeedTrackBar.TabIndex = 21;
            this.SetIdleSpeedTrackBar.TickFrequency = 50;
            this.SetIdleSpeedTrackBar.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.SetIdleSpeedTrackBar.Value = 1500;
            this.SetIdleSpeedTrackBar.Scroll += new System.EventHandler(this.SetIdleSpeedTrackBar_Scroll);
            // 
            // SetIdleSpeedStopButton
            // 
            this.SetIdleSpeedStopButton.Location = new System.Drawing.Point(65, 65);
            this.SetIdleSpeedStopButton.Name = "SetIdleSpeedStopButton";
            this.SetIdleSpeedStopButton.Size = new System.Drawing.Size(55, 23);
            this.SetIdleSpeedStopButton.TabIndex = 20;
            this.SetIdleSpeedStopButton.Text = "Stop";
            this.SetIdleSpeedStopButton.UseVisualStyleBackColor = true;
            this.SetIdleSpeedStopButton.Click += new System.EventHandler(this.SetIdleSpeedStopButton_Click);
            // 
            // SetIdleSpeedSetButton
            // 
            this.SetIdleSpeedSetButton.Location = new System.Drawing.Point(6, 65);
            this.SetIdleSpeedSetButton.Name = "SetIdleSpeedSetButton";
            this.SetIdleSpeedSetButton.Size = new System.Drawing.Size(55, 23);
            this.SetIdleSpeedSetButton.TabIndex = 19;
            this.SetIdleSpeedSetButton.Text = "Set";
            this.SetIdleSpeedSetButton.UseVisualStyleBackColor = true;
            this.SetIdleSpeedSetButton.Click += new System.EventHandler(this.SetIdleSpeedSetButton_Click);
            // 
            // ResetMemoryGroupBox
            // 
            this.ResetMemoryGroupBox.Controls.Add(this.ResetMemoryComboBox);
            this.ResetMemoryGroupBox.Controls.Add(this.ResetMemoryOKButton);
            this.ResetMemoryGroupBox.Location = new System.Drawing.Point(9, 397);
            this.ResetMemoryGroupBox.Name = "ResetMemoryGroupBox";
            this.ResetMemoryGroupBox.Size = new System.Drawing.Size(379, 73);
            this.ResetMemoryGroupBox.TabIndex = 30;
            this.ResetMemoryGroupBox.TabStop = false;
            this.ResetMemoryGroupBox.Text = "Reset memory";
            // 
            // ResetMemoryComboBox
            // 
            this.ResetMemoryComboBox.DropDownHeight = 226;
            this.ResetMemoryComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ResetMemoryComboBox.Font = new System.Drawing.Font("Courier New", 8.25F);
            this.ResetMemoryComboBox.FormattingEnabled = true;
            this.ResetMemoryComboBox.IntegralHeight = false;
            this.ResetMemoryComboBox.Items.AddRange(new object[] {
            "2300 |",
            "2301 | Engine fault codes",
            "2302 | Adaptive fuel factor",
            "2303 | IAC counter",
            "2304 | Minimum TPS volts",
            "2305 | Flex fuel percent",
            "2306 | Cam/Crank sync",
            "2307 | Fuel shutoff",
            "2308 | Runtime at stall",
            "2309 | Door lock enable",
            "230A | Door lock disable",
            "230B | Cam/Crank timing reference",
            "230C | A/C fault enable",
            "230D | A/C fault disable",
            "230E | S/C fault enable",
            "230F | S/C fault disable",
            "2310 | PS fault enable",
            "2311 | PS fault disable",
            "2312 | EEPROM / Adaptive numerator",
            "2313 | SKIM F4",
            "2314 | Duty cycle monitor",
            "2315 | Trip/idle/cruise/injector/O/D off/water in fuel",
            "2316 |",
            "2317 |",
            "2318 |",
            "2319 |",
            "231A |",
            "231B |",
            "231C |",
            "231D |",
            "231E |",
            "231F |",
            "2320 | TPS adaptive for ETC",
            "2321 | Minimum TPS",
            "2322 | Learned knock correction",
            "2323 | Learned misfire correction",
            "2324 | Idle adaptation",
            "2325 |",
            "2326 |",
            "2327 |"});
            this.ResetMemoryComboBox.Location = new System.Drawing.Point(7, 16);
            this.ResetMemoryComboBox.Name = "ResetMemoryComboBox";
            this.ResetMemoryComboBox.Size = new System.Drawing.Size(365, 22);
            this.ResetMemoryComboBox.TabIndex = 29;
            // 
            // ResetMemoryOKButton
            // 
            this.ResetMemoryOKButton.Location = new System.Drawing.Point(6, 43);
            this.ResetMemoryOKButton.Name = "ResetMemoryOKButton";
            this.ResetMemoryOKButton.Size = new System.Drawing.Size(55, 23);
            this.ResetMemoryOKButton.TabIndex = 19;
            this.ResetMemoryOKButton.Text = "OK";
            this.ResetMemoryOKButton.UseVisualStyleBackColor = true;
            this.ResetMemoryOKButton.Click += new System.EventHandler(this.ResetMemoryOKButton_Click);
            // 
            // SecurityGroupBox
            // 
            this.SecurityGroupBox.Controls.Add(this.LegacySecurityCheckBox);
            this.SecurityGroupBox.Controls.Add(this.SecurityLevelComboBox);
            this.SecurityGroupBox.Controls.Add(this.SecurityUnlockButton);
            this.SecurityGroupBox.Location = new System.Drawing.Point(9, 472);
            this.SecurityGroupBox.Name = "SecurityGroupBox";
            this.SecurityGroupBox.Size = new System.Drawing.Size(129, 73);
            this.SecurityGroupBox.TabIndex = 31;
            this.SecurityGroupBox.TabStop = false;
            this.SecurityGroupBox.Text = "Security";
            // 
            // LegacySecurityCheckBox
            // 
            this.LegacySecurityCheckBox.AutoSize = true;
            this.LegacySecurityCheckBox.Checked = true;
            this.LegacySecurityCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.LegacySecurityCheckBox.Location = new System.Drawing.Point(66, 47);
            this.LegacySecurityCheckBox.Name = "LegacySecurityCheckBox";
            this.LegacySecurityCheckBox.Size = new System.Drawing.Size(61, 17);
            this.LegacySecurityCheckBox.TabIndex = 27;
            this.LegacySecurityCheckBox.Text = "Legacy";
            this.LegacySecurityCheckBox.UseVisualStyleBackColor = true;
            // 
            // SecurityLevelComboBox
            // 
            this.SecurityLevelComboBox.DropDownHeight = 226;
            this.SecurityLevelComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SecurityLevelComboBox.Font = new System.Drawing.Font("Courier New", 8.25F);
            this.SecurityLevelComboBox.FormattingEnabled = true;
            this.SecurityLevelComboBox.IntegralHeight = false;
            this.SecurityLevelComboBox.Items.AddRange(new object[] {
            "Level 1",
            "Level 2"});
            this.SecurityLevelComboBox.Location = new System.Drawing.Point(7, 16);
            this.SecurityLevelComboBox.Name = "SecurityLevelComboBox";
            this.SecurityLevelComboBox.Size = new System.Drawing.Size(115, 22);
            this.SecurityLevelComboBox.TabIndex = 29;
            this.SecurityLevelComboBox.SelectedIndexChanged += new System.EventHandler(this.SecurityLevelComboBox_SelectedIndexChanged);
            // 
            // SecurityUnlockButton
            // 
            this.SecurityUnlockButton.Location = new System.Drawing.Point(6, 43);
            this.SecurityUnlockButton.Name = "SecurityUnlockButton";
            this.SecurityUnlockButton.Size = new System.Drawing.Size(55, 23);
            this.SecurityUnlockButton.TabIndex = 19;
            this.SecurityUnlockButton.Text = "Unlock";
            this.SecurityUnlockButton.UseVisualStyleBackColor = true;
            this.SecurityUnlockButton.Click += new System.EventHandler(this.SecurityUnlockButton_Click);
            // 
            // EngineToolsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(397, 553);
            this.Controls.Add(this.SecurityGroupBox);
            this.Controls.Add(this.ResetMemoryGroupBox);
            this.Controls.Add(this.SetIdleSpeedGroupBox);
            this.Controls.Add(this.DiagnosticDataGroupBox);
            this.Controls.Add(this.ActuatorTestGroupBox);
            this.Controls.Add(this.BaudrateGroupBox);
            this.Controls.Add(this.FaultCodeGroupBox);
            this.Name = "EngineToolsForm";
            this.Text = "Engine tools";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EngineToolsForm_FormClosing);
            this.FaultCodeGroupBox.ResumeLayout(false);
            this.BaudrateGroupBox.ResumeLayout(false);
            this.ActuatorTestGroupBox.ResumeLayout(false);
            this.DiagnosticDataGroupBox.ResumeLayout(false);
            this.DiagnosticDataGroupBox.PerformLayout();
            this.SetIdleSpeedGroupBox.ResumeLayout(false);
            this.SetIdleSpeedGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SetIdleSpeedTrackBar)).EndInit();
            this.ResetMemoryGroupBox.ResumeLayout(false);
            this.SecurityGroupBox.ResumeLayout(false);
            this.SecurityGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox FaultCodeGroupBox;
        private System.Windows.Forms.GroupBox BaudrateGroupBox;
        private System.Windows.Forms.GroupBox ActuatorTestGroupBox;
        private System.Windows.Forms.GroupBox DiagnosticDataGroupBox;
        private System.Windows.Forms.Button EraseFaultCodesButton;
        private System.Windows.Forms.Button ReadFaultCodesButton;
        private System.Windows.Forms.Button Baud62500Button;
        private System.Windows.Forms.Button Baud7812Button;
        private System.Windows.Forms.ListBox DiagnosticDataListBox;
        private System.Windows.Forms.Button ActuatorTestStopButton;
        private System.Windows.Forms.Button ActuatorTestStartButton;
        private System.Windows.Forms.Button Baud125000Button;
        private System.Windows.Forms.Button DiagnosticDataStopButton;
        private System.Windows.Forms.Button DiagnosticDataReadButton;
        private System.Windows.Forms.Button Baud976Button;
        private System.Windows.Forms.Button DiagnosticDataClearButton;
        private System.Windows.Forms.Label MillisecondsLabel01;
        private System.Windows.Forms.TextBox DiagnosticDataRepeatIntervalTextBox;
        private System.Windows.Forms.CheckBox DiagnosticDataRepeatIntervalCheckBox;
        private System.Windows.Forms.GroupBox SetIdleSpeedGroupBox;
        private System.Windows.Forms.TextBox SetIdleSpeedTextBox;
        private System.Windows.Forms.TrackBar SetIdleSpeedTrackBar;
        private System.Windows.Forms.Button SetIdleSpeedStopButton;
        private System.Windows.Forms.Button SetIdleSpeedSetButton;
        private System.Windows.Forms.Label RPMLabel;
        private System.Windows.Forms.Label IdleSpeedNoteLabel;
        private System.Windows.Forms.ComboBox ActuatorTestComboBox;
        private System.Windows.Forms.GroupBox ResetMemoryGroupBox;
        private System.Windows.Forms.ComboBox ResetMemoryComboBox;
        private System.Windows.Forms.Button ResetMemoryOKButton;
        private System.Windows.Forms.GroupBox SecurityGroupBox;
        private System.Windows.Forms.ComboBox SecurityLevelComboBox;
        private System.Windows.Forms.Button SecurityUnlockButton;
        private System.Windows.Forms.CheckBox LegacySecurityCheckBox;
        private System.Windows.Forms.CheckBox DiagnosticDataCSVCheckBox;
    }
}