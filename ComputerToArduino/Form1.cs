using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;

namespace ComputerToArduino
{
    public partial class Form1 : Form

    {
        bool isConnected = false;
        String[] ports;
        SerialPort port;
        
        int[] playerScore = new int[4] { 25000,25000,25000,25000};
        //リーチ棒の数
        int reachCount = 0;
        //供託棒の数
        int depositCount = 0;

        //現在の局数
        int round = 1;
        int han = 0;
        int hu = 0;

        int[] scores = new int[2];

        int mode = 4;

        //麻雀の点数表、まずは子のツモの点数表、childScore[翻数][符数]から求められるように
        int[,] childChildTsumoScore = new int[18, 11]{
            {0, 0, 300, 400, 400, 500, 600, 700, 800, 800, 900},
            {400, 0, 500, 700, 800, 1000, 1200, 1300, 1500, 1600, 1800},
            {700, 800, 1000, 1300, 1600, 2000, 2000, 2000, 2000, 20002, 2000},
            {1300, 1600, 2000, 2000, 2000, 2000, 2000, 2000, 2000, 2000, 2000},
            {2000,2000,2000,2000,2000,2000,2000,2000,2000,2000,2000},
            {3000,3000,3000,3000,3000,3000,3000,3000,3000,3000,3000},
            {3000,3000,3000,3000,3000,3000,3000,3000,3000,3000,3000},
            {4000,4000,4000,4000,4000,4000,4000,4000,4000,4000,4000},
            {4000,4000,4000,4000,4000,4000,4000,4000,4000,4000,4000},
            {4000,4000,4000,4000,4000,4000,4000,4000,4000,4000,4000},
            {6000,6000,6000,6000,6000,6000,6000,6000,6000,6000,6000},
            {6000,6000,6000,6000,6000,6000,6000,6000,6000,6000,6000},
            {8000,8000,8000,8000,8000,8000,8000,8000,8000,8000,8000},
            {16000,16000,16000,16000,16000,16000,16000,16000,16000,16000,16000},
            {32000,32000,32000,32000,32000,32000,32000,32000,32000,32000,32000},
            {48000,48000,48000,48000,48000,48000,48000,48000,48000,48000,48000},
            {64000,64000,64000,64000,64000,64000,64000,64000,64000,64000,64000},
            {80000,80000,80000,80000,80000,80000,80000,80000,80000,80000,80000}
            };

        int[,] childParentTsumoScore = new int[18, 11]{
            {0, 0, 500, 700, 800, 1000, 1200, 1300, 1500, 1600, 1800},
            {700, 0, 1000, 1300, 1600, 2000, 2300, 2600, 2900, 3200, 3600},
            {1300, 1600, 2000, 2600, 3200, 3900, 4000, 4000, 4000, 4000, 4000},
            {2600, 3200, 3900, 4000, 4000, 4000, 4000, 4000, 4000, 4000, 4000},
            {4000,4000,4000,4000,4000,4000,4000,4000,4000,4000,4000},
            {6000,6000,6000,6000,6000,6000,6000,6000,6000,6000,6000},
            {6000,6000,6000,6000,6000,6000,6000,6000,6000,6000,6000},
            {8000,8000,8000,8000,8000,8000,8000,8000,8000,8000,8000},
            {8000,8000,8000,8000,8000,8000,8000,8000,8000,8000,8000},
            {8000,8000,8000,8000,8000,8000,8000,8000,8000,8000,8000},
            {12000,12000,12000,12000,12000,12000,12000,12000,12000,12000,12000},
            {12000,12000,12000,12000,12000,12000,12000,12000,12000,12000,12000},
            {16000,16000,16000,16000,16000,16000,16000,16000,16000,16000,16000},
            {32000,32000,32000,32000,32000,32000,32000,32000,32000,32000,32000},
            {48000,48000,48000,48000,48000,48000,48000,48000,48000,48000,48000},
            {60000,60000,60000,60000,60000,60000,60000,60000,60000,60000,60000},
            {80000,80000,80000,80000,80000,80000,80000,80000,80000,80000,80000},
            {96000,96000,96000,96000,96000,96000,96000,96000,96000,96000,96000}
            };

        int[,] childRonScore = new int[18, 11]{
            {0, 0, 1000, 1300, 1600, 2000, 2300, 2600, 2900, 3200, 3600},
            {0, 1600, 2000, 2600, 3200, 3900, 4500, 5200, 5800, 6400, 7100},
            {0, 3200, 3900,5200, 6400, 7700, 8000, 8000, 8000, 8000, 8000},
            {0, 6400, 7700,8000,8000,8000,8000,8000,8000,8000,8000},
            {8000,8000,8000,8000,8000,8000,8000,8000,8000,8000,8000},
            {12000,12000,12000,12000,12000,12000,12000,12000,12000,12000,12000},
            {12000,12000,12000,12000,12000,12000,12000,12000,12000,12000,12000},
            {16000,16000,16000,16000,16000,16000,16000,16000,16000,16000,16000},
            {16000,16000,16000,16000,16000,16000,16000,16000,16000,16000,16000},
            {16000,16000,16000,16000,16000,16000,16000,16000,16000,16000,16000},
            {24000,24000,24000,24000,24000,24000,24000,24000,24000,24000,24000},
            {24000,24000,24000,24000,24000,24000,24000,24000,24000,24000,24000},
            {32000,32000,32000,32000,32000,32000,32000,32000,32000,32000,32000},
            {64000,64000,64000,64000,64000,64000,64000,64000,64000,64000,64000},
            {96000,96000,96000,96000,96000,96000,96000,96000,96000,96000,96000},
            {120000,120000,120000,120000,120000,120000,120000,120000,120000,120000,120000},
            {160000,160000,160000,160000,160000,160000,160000,160000,160000,160000,160000},
            {192000,192000,192000,192000,192000,192000,192000,192000,192000,192000,192000}
            };

        //親のツモの点数表
        int[,] parentTsumoScore = new int[18, 11]{
            {0, 0, 500, 700, 800, 1000, 1200, 1300, 1500, 1600, 1800},
            {700, 0, 1000, 1300, 1600, 2000, 2300, 2600, 2900, 3200, 3600},
            {1300, 1600, 2000, 2600, 3200, 3900, 4000, 4000, 4000, 4000, 4000},
            {2600, 3200, 3900, 4000, 4000, 4000, 4000, 4000, 4000, 4000, 4000},
            {4000,4000,4000,4000,4000,4000,4000,4000,4000,4000,4000},
            {6000,6000,6000,6000,6000,6000,6000,6000,6000,6000,6000},
            {6000,6000,6000,6000,6000,6000,6000,6000,6000,6000,6000},
            {8000,8000,8000,8000,8000,8000,8000,8000,8000,8000,8000},
            {8000,8000,8000,8000,8000,8000,8000,8000,8000,8000,8000},
            {8000,8000,8000,8000,8000,8000,8000,8000,8000,8000,8000},
            {12000,12000,12000,12000,12000,12000,12000,12000,12000,12000,12000},
            {12000,12000,12000,12000,12000,12000,12000,12000,12000,12000,12000},
            {16000,16000,16000,16000,16000,16000,16000,16000,16000,16000,16000},
            {32000,32000,32000,32000,32000,32000,32000,32000,32000,32000,32000},
            {48000,48000,48000,48000,48000,48000,48000,48000,48000,48000,48000},
            {64000,64000,64000,64000,64000,64000,64000,64000,64000,64000,64000},
            {80000,80000,80000,80000,80000,80000,80000,80000,80000,80000,80000},
            {96000,96000,96000,96000,96000,96000,96000,96000,96000,96000,96000}
            };

        //親のロンの点数表
        int[,] parentRonScore = new int[18, 11]{
            {0, 0, 1500, 2000, 2400, 2900, 3400, 3900, 4400, 4800, 5300},
            {0, 2400, 2900, 3900, 4800, 5800, 6800, 7700, 8700, 9600, 10600},
            {0, 4800, 5800, 7700, 9600, 11600, 12000, 12000, 12000, 12000, 12000},
            {0, 9600, 11600, 12000, 12000, 12000, 12000, 12000, 12000, 12000, 12000},
            {12000,12000,12000,12000,12000,12000,12000,12000,12000,12000,12000},
            {18000,18000,18000,18000,18000,18000,18000,18000,18000,18000,18000},
            {18000,18000,18000,18000,18000,18000,18000,18000,18000,18000,18000},
            {24000,24000,24000,24000,24000,24000,24000,24000,24000,24000,24000},
            {24000,24000,24000,24000,24000,24000,24000,24000,24000,24000,24000},
            {24000,24000,24000,24000,24000,24000,24000,24000,24000,24000,24000},
            {32000,32000,32000,32000,32000,32000,32000,32000,32000,32000,32000},
            {32000,32000,32000,32000,32000,32000,32000,32000,32000,32000,32000},
            {48000,48000,48000,48000,48000,48000,48000,48000,48000,48000,48000},
            {96000,96000,96000,96000,96000,96000,96000,96000,96000,96000,96000},
            {144000,144000,144000,144000,144000,144000,144000,144000,144000,144000,144000},
            {192000,192000,192000,192000,192000,192000,192000,192000,192000,192000,192000},
            {240000,240000,240000,240000,240000,240000,240000,240000,240000,240000,240000},
            {288000,288000,288000,288000,288000,288000,288000,288000,288000,288000,288000}
            };

        string[] playerNames = new string[4] {"A","B","C","D"};

        public Form1()
        {
            InitializeComponent();
            disableControls();
            getAvailableComPorts();

            foreach (string port in ports)
            {
                comboBox1.Items.Add(port);
                Console.WriteLine(port);
                if (ports[0] != null)
                {
                    comboBox1.SelectedItem = ports[0];
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!isConnected)
            {
                connectToArduino();
            } else
            {
                disconnectFromArduino();
            }
        }

        void getAvailableComPorts()
        {
            ports = SerialPort.GetPortNames();
        }

        private void connectToArduino()
        {
            isConnected = true;
            string selectedPort = comboBox1.GetItemText(comboBox1.SelectedItem);
            port = new SerialPort(selectedPort, 9600, Parity.None, 8, StopBits.One);
            port.Open();
            port.Write("#STAR\n");
            button1.Text = "Disconnect";
            enableControls();
        }

        

        

        private void disconnectFromArduino()
        {
            isConnected = false;
            port.Write("#STOP\n");
            port.Close();
            button1.Text = "Connect";
            disableControls();
            resetDefaults();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (isConnected)
            {
                port.Write("#TEXT" + textBox1.Text + "#\n");
            }
        }

        private void enableControls()
        {
            
            button2.Enabled = true;
            textBox1.Enabled = true;
            
            groupBox3.Enabled = true;

        }

        private void disableControls()
        {

            button2.Enabled = false;
            textBox1.Enabled = false;

            groupBox3.Enabled = false;
        }

        private void resetDefaults()
        {

            textBox1.Text = "";
            
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            groupBoxPlayer1.Text = textBoxPlayer1.Text;
            groupBoxPlayer2.Text = textBoxPlayer2.Text;
            groupBoxPlayer3.Text = textBoxPlayer3.Text;
            groupBoxPlayer4.Text = textBoxPlayer4.Text;
            checkBoxPlayer1.Text = textBoxPlayer1.Text;
            checkBoxPlayer2.Text = textBoxPlayer2.Text;
            checkBoxPlayer3.Text = textBoxPlayer3.Text;
            checkBoxPlayer4.Text = textBoxPlayer4.Text;
            checkBox2Player1.Text = textBoxPlayer1.Text;
            checkBox2Player2.Text = textBoxPlayer2.Text;
            checkBox2Player3.Text = textBoxPlayer3.Text;
            checkBox2Player4.Text = textBoxPlayer4.Text;
            playerNames[0] = textBoxPlayer1.Text;
            playerNames[1] = textBoxPlayer2.Text;
            playerNames[2] = textBoxPlayer3.Text;
            playerNames[3] = textBoxPlayer4.Text;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if(comboBoxMode.Text == "三人麻雀")
            {
                mode = 3;
                for (int i = 0; i < 3; i++)
                {
                    playerScore[i] = 35000;
                }
                playerScore[3] = 0;
            }
            else
            {
                mode = 4;
                for (int i = 0; i < 4; i++)
                {
                    playerScore[i] = 25000;
                }
            }

            UpdateScores();

        }

        private void button7_Click(object sender, EventArgs e)
        {
            //ツモの場合
            if (radioButton1.Checked)
            {
                if (checkBoxPlayer1.Checked)
                {
                    //親番
                    if (round % mode == 1)
                    {
                        playerScore[0] += scores[0]*(mode-1) + reachCount*1000;
                        playerScore[1] -= scores[0];
                        playerScore[2] -= scores[0];
                        playerScore[3] -= scores[0];
                    }
                    else
                    {
                        playerScore[0] += scores[0] + scores[1]*(mode-2) + reachCount*1000;
                        if(round % mode == 2)
                        {
                            playerScore[1] -= scores[0];
                            playerScore[2] -= scores[1];
                            playerScore[3] -= scores[1];
                        }
                        else if(round % mode == 3)
                        {
                            playerScore[1] -= scores[1];
                            playerScore[2] -= scores[0];
                            playerScore[3] -= scores[1];
                        }
                        else
                        {
                            playerScore[1] -= scores[1];
                            playerScore[2] -= scores[1];
                            playerScore[3] -= scores[0];
                        }
                    }
                }else if (checkBoxPlayer2.Checked)
                {
                    //親番
                    if (round % mode == 2)
                    {
                        playerScore[1] += scores[0]*(mode-1) + reachCount*1000;
                        playerScore[0] -= scores[0];
                        playerScore[2] -= scores[0];
                        playerScore[3] -= scores[0];
                    }
                    else
                    {
                        playerScore[1] += scores[0] + scores[1]*(mode-2) + reachCount*1000;
                        if(round % mode == 3)
                        {
                            playerScore[0] -= scores[1];
                            playerScore[2] -= scores[0];
                            playerScore[3] -= scores[1];
                        }
                        else if(round % mode == 0)
                        {
                            playerScore[0] -= scores[1];
                            playerScore[2] -= scores[1];
                            playerScore[3] -= scores[0];
                        }
                        else
                        {
                            playerScore[0] -= scores[0];
                            playerScore[2] -= scores[1];
                            playerScore[3] -= scores[1];
                        }
                    }
                }else if (checkBoxPlayer3.Checked){
                    //親番
                    if (round % mode == 3)
                    {
                        playerScore[2] += scores[0]*(mode-1) + reachCount*1000;
                        playerScore[0] -= scores[0];
                        playerScore[1] -= scores[0];
                        playerScore[3] -= scores[0];
                    }
                    else
                    {
                        playerScore[2] += scores[0] + scores[1]*(mode-2) + reachCount*1000;
                        if(round % mode == 0)
                        {
                            playerScore[0] -= scores[1];
                            playerScore[1] -= scores[1];
                            playerScore[3] -= scores[0];
                        }
                        else if(round % mode == 1)
                        {
                            playerScore[0] -= scores[0];
                            playerScore[1] -= scores[1];
                            playerScore[3] -= scores[1];
                        }
                        else
                        {
                            playerScore[0] -= scores[1];
                            playerScore[1] -= scores[0];
                            playerScore[3] -= scores[1];
                        }
                    }
                }else if (checkBoxPlayer4.Checked){
                    //親番
                    if (round % mode == 0)
                    {
                        playerScore[3] += scores[0]*(mode-1) + reachCount*1000;
                        playerScore[0] -= scores[0];
                        playerScore[1] -= scores[0];
                        playerScore[2] -= scores[0];
                    }
                    else
                    {
                        playerScore[3] += scores[0] + scores[1]*(mode-2) + reachCount*1000;
                        if(round % mode == 1)
                        {
                            playerScore[0] -= scores[0];
                            playerScore[1] -= scores[1];
                            playerScore[2] -= scores[1];
                        }
                        else if(round % mode == 2)
                        {
                            playerScore[0] -= scores[1];
                            playerScore[1] -= scores[0];
                            playerScore[2] -= scores[1];
                        }
                        else
                        {
                            playerScore[0] -= scores[1];
                            playerScore[1] -= scores[1];
                            playerScore[2] -= scores[0];
                        }
                    }
                }
                reachCount = 0;
            }

            //ロンの場合
            if(radioButton2.Checked){
                if(checkBoxPlayer1.Checked)
                {
                    playerScore[0] += scores[0] + reachCount*1000;
                }else if(checkBoxPlayer2.Checked)
                {
                    playerScore[1] += scores[0] + reachCount*1000;
                }else if(checkBoxPlayer3.Checked){
                    playerScore[2] += scores[0] + reachCount*1000;
                }else if(checkBoxPlayer4.Checked){
                    playerScore[3] += scores[0] + reachCount*1000;
                }

                if(checkBox2Player1.Checked){
                    playerScore[0] -= scores[0];
                }else if(checkBox2Player2.Checked){
                    playerScore[1] -= scores[0];
                }else if(checkBox2Player3.Checked){
                    playerScore[2] -= scores[0];
                }else if(checkBox2Player4.Checked){
                    playerScore[3] -= scores[0];
                }
                reachCount = 0;
            }

            //流局の場合
            if(radioButton3.Checked){
                if(checkBoxPlayer1.Checked) playerScore[0] += scores[0];
                else playerScore[0] -= scores[1];
                if(checkBoxPlayer2.Checked) playerScore[1] += scores[0];
                else playerScore[1] -= scores[1];
                if(checkBoxPlayer3.Checked) playerScore[2] += scores[0];
                else playerScore[2] -= scores[1];
                if(checkBoxPlayer4.Checked) playerScore[3] += scores[0];
                else playerScore[3] -= scores[1];

            }

            //ログの追加
            string agariLog = "";
            if(radioButton1.Checked) agariLog = "ツモ";
            if(radioButton2.Checked) agariLog = "ロン";
            if(radioButton3.Checked) agariLog = "流局";

            string winnerLog = "";
            string loserLog = "";
            string scoreLog = ScoreLabel.Text;

            if(radioButton1.Checked)
            {
                if(checkBoxPlayer1.Checked) winnerLog = playerNames[0];
                if(checkBoxPlayer2.Checked) winnerLog = playerNames[1];
                if(checkBoxPlayer3.Checked) winnerLog = playerNames[2];
                if(checkBoxPlayer4.Checked) winnerLog = playerNames[3];
            }else if(radioButton2.Checked)
            {
                if(checkBoxPlayer1.Checked) winnerLog = playerNames[0];
                if(checkBoxPlayer2.Checked) winnerLog = playerNames[1];
                if(checkBoxPlayer3.Checked) winnerLog = playerNames[2];
                if(checkBoxPlayer4.Checked) winnerLog = playerNames[3];
                
                if(checkBox2Player1.Checked) loserLog = playerNames[0];
                if(checkBox2Player2.Checked) loserLog = playerNames[1];
                if(checkBox2Player3.Checked) loserLog = playerNames[2];
                if(checkBox2Player4.Checked) loserLog = playerNames[3];
            }else if(radioButton3.Checked){
                //勝った人はwinnerLogに連ねる AとBが勝ったならA,Bとなる
                if(checkBoxPlayer1.Checked) winnerLog += playerNames[0] + ",";
                if(checkBoxPlayer2.Checked) winnerLog += playerNames[1] + ",";
                if(checkBoxPlayer3.Checked) winnerLog += playerNames[2] + ",";
                if(checkBoxPlayer4.Checked) winnerLog += playerNames[3] + ",";
            }

            //ログの追加
            string log = "";
            if(radioButton1.Checked) log = roundLabel.Text + " " + depositCount.ToString() + "本場 " + agariLog + " " + winnerLog + " " + scoreLog;
            if(radioButton2.Checked) log = roundLabel.Text + " " + depositCount.ToString() + "本場 " + agariLog + " " + winnerLog + "が" + loserLog + "から" + scoreLog;
            if(radioButton3.Checked) log = roundLabel.Text + " " + depositCount.ToString() + "本場 " + agariLog + " " + winnerLog + " テンパイ";
            logBox.AppendText(log+"\r\n");

            ResetScoreCalculation();
            UpdateScores();
        }

        public void ResetScoreCalculation()
        {
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
            radioButton4.Checked = false;
            checkBoxPlayer1.Checked = false;
            checkBoxPlayer2.Checked = false;
            checkBoxPlayer3.Checked = false;
            checkBoxPlayer4.Checked = false;
            checkBox2Player1.Checked = false;
            checkBox2Player2.Checked = false;
            checkBox2Player3.Checked = false;
            checkBox2Player4.Checked = false;
            comboBox2.Text = "";
            comboBox3.Text = "";
            ScoreLabel.Text = "0";
            hu = 0;
            han = 0;
            scores[0] = 0;
            scores[1] = 0;
        }

        public void UpdateScores(){
            scorePlayer1.Text = playerScore[0].ToString();
            scorePlayer2.Text = playerScore[1].ToString();
            scorePlayer3.Text = playerScore[2].ToString();
            if (mode == 3) playerScore[3] = 0;
            scorePlayer4.Text = playerScore[3].ToString();
            
            reachCountLabel.Text = "×" + reachCount.ToString();
            depositCountLabel.Text = "×" + depositCount.ToString();
            //roundが1-4なら東〇局、5-8なら南〇局、9-12なら西〇局、13-16なら北〇局
            if(round <= mode)
            {
                roundLabel.Text = "東" + round.ToString() + "局";
            }
            else if(round <= mode*2)
            {
                roundLabel.Text = "南" + (round - mode).ToString() + "局";
            }
            else if(round <= mode*3)
            {
                roundLabel.Text = "西" + (round - mode*2).ToString() + "局";
            }
            else
            {
                roundLabel.Text = "北" + (round - mode*3).ToString() + "局";
            }
        }

        private void ReachPlayer1_Click(object sender, EventArgs e)
        {
            playerScore[0] -= 1000;
            reachCount++;
            UpdateScores();
            ReachPlayer1.Enabled = false;
        }

        private void ReachPlayer2_Click(object sender, EventArgs e)
        {
            playerScore[1] -= 1000;
            reachCount++;
            UpdateScores();
            ReachPlayer2.Enabled = false;
        }

        private void ReachPlayer3_Click(object sender, EventArgs e)
        {
            playerScore[2] -= 1000;
            reachCount++;
            UpdateScores();
            ReachPlayer3.Enabled = false;
        }

        private void ReachPlayer4_Click(object sender, EventArgs e)
        {
            playerScore[3] -= 1000;
            reachCount++;
            UpdateScores();
            ReachPlayer4.Enabled = false;
        }

        private void changeButton_Click(object sender, EventArgs e)
        {
            round++;
            depositCount = 0;
            UpdateScores();
            ReachPlayer1.Enabled = true;
            ReachPlayer2.Enabled = true;
            ReachPlayer3.Enabled = true;
            ReachPlayer4.Enabled = true;
        }

        private void continueButton_Click(object sender, EventArgs e)
        {
            depositCount++;
            UpdateScores();
            ReachPlayer1.Enabled = true;
            ReachPlayer2.Enabled = true;
            ReachPlayer3.Enabled = true;
            ReachPlayer4.Enabled = true;
        }

        public void ScoreCalculation()
        {
            
            //ツモの場合
            if (radioButton1.Checked)
            {
                if (checkBoxPlayer1.Checked)
                {
                    //親番
                    if (round % mode == 1)
                    {
                        scores[0] = parentTsumoScore[han, hu] + depositCount * 100;
                        ScoreLabel.Text = scores[0].ToString() + "オール";
                    }
                    else
                    {
                        scores[0] = childParentTsumoScore[han, hu] + depositCount * 100;
                        scores[1] = childChildTsumoScore[han, hu] + depositCount * 100;
                        ScoreLabel.Text = scores[1].ToString() + " / " + scores[0].ToString();
                    }
                }else if (checkBoxPlayer2.Checked)
                {
                    //親番
                    if (round % mode == 2)
                    {
                        scores[0] = parentTsumoScore[han, hu] + depositCount * 100;
                        ScoreLabel.Text = scores[0].ToString() + "オール";
                    }
                    else
                    {
                        scores[0] = childParentTsumoScore[han, hu] + depositCount * 100;
                        scores[1] = childChildTsumoScore[han, hu] + depositCount * 100;
                        ScoreLabel.Text = scores[1].ToString() + " / " + scores[0].ToString();
                    }
                }else if (checkBoxPlayer3.Checked){
                    //親番
                    if (round % mode == 3)
                    {
                        scores[0] = parentTsumoScore[han, hu] + depositCount * 100;
                        ScoreLabel.Text = scores[0].ToString() + "オール";
                    }
                    else
                    {
                        scores[0] = childParentTsumoScore[han, hu] + depositCount * 100;
                        scores[1] = childChildTsumoScore[han, hu] + depositCount * 100;
                        ScoreLabel.Text = scores[1].ToString() + " / " + scores[0].ToString();
                    }
                }else if (checkBoxPlayer4.Checked)
                {
                    //親番
                    if (round % mode == 0)
                    {
                        scores[0] = parentTsumoScore[han, hu] + depositCount * 100;
                        ScoreLabel.Text = scores[0].ToString() + "オール";
                    }
                    else
                    {
                        scores[0] = childParentTsumoScore[han, hu] + depositCount * 100;
                        scores[1] = childChildTsumoScore[han, hu] + depositCount * 100;
                        ScoreLabel.Text = scores[1].ToString() + " / " + scores[0].ToString();
                    }
                }
            }

            //ロンの場合
            if (radioButton2.Checked)
            {
                if (checkBoxPlayer1.Checked)
                {
                    //親番
                    if (round % mode == 1)
                    {
                        scores[0] = parentRonScore[han, hu] + depositCount * 300;
                        ScoreLabel.Text = scores[0].ToString();
                    }else{
                        scores[0] = childRonScore[han, hu] + depositCount * 300;
                        ScoreLabel.Text = scores[0].ToString();
                    }
                }else if (checkBoxPlayer2.Checked)
                {
                    //親番
                    if (round % mode == 2)
                    {
                        scores[0] = parentRonScore[han, hu] + depositCount * 300;
                        ScoreLabel.Text = scores[0].ToString();
                    }else{
                        scores[0] = childRonScore[han, hu] + depositCount * 300;
                        ScoreLabel.Text = scores[0].ToString();
                    }
                }else if (checkBoxPlayer3.Checked)
                {
                    //親番
                    if (round % mode == 3)
                    {
                        scores[0] = parentRonScore[han, hu] + depositCount * 300;
                        ScoreLabel.Text = scores[0].ToString();
                    }else{
                        scores[0] = childRonScore[han, hu] + depositCount * 300;
                        ScoreLabel.Text = scores[0].ToString();
                    }
                }else if (checkBoxPlayer4.Checked){
                    //親番
                    if (round % mode == 0)
                    {
                        scores[0] = parentRonScore[han, hu] + depositCount * 300;
                        ScoreLabel.Text = scores[0].ToString();
                    }else{
                        scores[0] = childRonScore[han, hu] + depositCount * 300;
                        ScoreLabel.Text = scores[0].ToString();
                    }
                }
            }

            //流局の場合
            if (radioButton3.Checked)
            {
                //テンパイした人数
                int tenpaiCount = 0;
                if (checkBoxPlayer1.Checked) tenpaiCount++;
                if (checkBoxPlayer2.Checked) tenpaiCount++;
                if (checkBoxPlayer3.Checked) tenpaiCount++;
                if (checkBoxPlayer4.Checked) tenpaiCount++;

                if(tenpaiCount != 0) scores[0] = (mode-1) *1000 / tenpaiCount;
                scores[1] = (mode-1) *1000 / (mode-tenpaiCount);
                ScoreLabel.Text = scores[0].ToString();
            }
        }

        private void hanChanged(object sender, EventArgs e)
        {
            han = comboBox2.SelectedIndex;
            ScoreCalculation();
        }

        private void huChanged(object sender, EventArgs e)
        {
            hu = comboBox3.SelectedIndex;
            ScoreCalculation();
        }
    }
}
