  try
            {
                // Open the JSON file using an OpenFileDialog
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
                if (openFileDialog.ShowDialog() == true)
                {
                    string jsonData = File.ReadAllText(openFileDialog.FileName);
                    List<jsData> objects = JsonConvert.DeserializeObject<List<jsData>>(jsonData);

                    MySqlConnection conn = new MySqlConnection(connectionState.ConnectionString);
                    conn.OpenAsync();
                    int countp = 0;
                    int countEXE = 3;
                    progressBar.Minimum = 0;
                    progressBar.Maximum = objects.Count;
                    progressBar.Visibility = Visibility.Visible;
                    foreach (jsData obj in objects)
                    {
                        if (obj.sn != "First")
                        {
                            {
                                MySqlCommand cmd2 = new MySqlCommand("SELECT COUNT(*) FROM screens WHERE sn = @field1 ", conn);

                                MySqlCommand cmd = new MySqlCommand("SELECT COUNT(*) FROM stable WHERE sn = @field1 AND sid = @field2", conn);
                                cmd.Parameters.AddWithValue("@field1", obj.sn);
                                cmd2.Parameters.AddWithValue("@field1", obj.sn);
                                cmd.Parameters.AddWithValue("@field2", obj.sid);
                                int count2 = int.Parse(cmd2.ExecuteScalar().ToString());
                                if (count2 != 0)
                                {
                                    int count = int.Parse(cmd.ExecuteScalar().ToString());

                                    if (count == 0)
                                    {
                                        MySqlCommand insertCmd = new MySqlCommand("INSERT INTO stable (sn, sid,m1,m2,m3,m4,m5,m6,T1,T2,T3,T4,T5,T6,A,B,F,flowSec,L,br1,br2,speed,time,isSend,TimeRecived,seconds) VALUES (@sn, @sid,@m1,@m2,@m3,@m4,@m5,@m6,@T1,@T2,@T3,@T4,@T5,@T6,@A,@B,@F,@flowSec,@L,@br1,@br2,@speed,@time,@isSend,'" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "',@seconds)", conn);
                                        insertCmd.Parameters.AddWithValue("@sn", obj.sn);
                                        insertCmd.Parameters.AddWithValue("@sid", obj.sid);
                                        insertCmd.Parameters.AddWithValue("@m1", obj.m1);
                                        insertCmd.Parameters.AddWithValue("@m2", obj.m2);
                                        insertCmd.Parameters.AddWithValue("@m3", obj.m3);
                                        insertCmd.Parameters.AddWithValue("@m4", obj.m4);
                                        insertCmd.Parameters.AddWithValue("@m5", obj.m5);
                                        insertCmd.Parameters.AddWithValue("@m6", obj.m6);
                                        insertCmd.Parameters.AddWithValue("@T1", obj.T1);
                                        insertCmd.Parameters.AddWithValue("@T2", obj.T2);
                                        insertCmd.Parameters.AddWithValue("@T3", obj.T3);
                                        insertCmd.Parameters.AddWithValue("@T4", obj.T4);
                                        insertCmd.Parameters.AddWithValue("@T5", obj.T5);
                                        insertCmd.Parameters.AddWithValue("@T6", obj.T6);
                                        insertCmd.Parameters.AddWithValue("@A", obj.A);
                                        insertCmd.Parameters.AddWithValue("@B", obj.B);
                                        insertCmd.Parameters.AddWithValue("@F", obj.F);
                                        insertCmd.Parameters.AddWithValue("@flowSec", obj.flowSec);
                                        insertCmd.Parameters.AddWithValue("@L", obj.L);
                                        insertCmd.Parameters.AddWithValue("@br1", obj.br1);
                                        insertCmd.Parameters.AddWithValue("@br2", obj.br2);
                                        insertCmd.Parameters.AddWithValue("@speed", obj.speed);
                                        insertCmd.Parameters.AddWithValue("@time", obj.time);
                                        insertCmd.Parameters.AddWithValue("@isSend", obj.isSend);
                                        insertCmd.Parameters.AddWithValue("@seconds", obj.Seconds);
                                        insertCmd.ExecuteNonQueryAsync();
                                        countEXE = count;

                                    }
                                    else if (count == 1)
                                    {
                                        MySqlCommand insertCmd = new MySqlCommand("UPDATE `stable` SET `sn`=@sn,`sid`=@sid,`m1`=@m1,`m2`=@m2,`m3`=@m3,`m4`=@m4,`m5`=@m5,`m6`=@m6,`T1`=@T1,`T2`=@T2,`T3`=@T3,`T4`=@T4,`T5`=@T5,`T6`=@T6,`A`=@A,`B`=@B',`F`=@F,`flowSec`=@flowSec,`L`=@L,`br1`=@br1,`br2`=@br2,`speed`=@speed,`time`=@time,`isSend`=@isSend,`TimeRecived`='" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "',`seconds`=@seconds WHERE sn=@sn", conn);
                                        insertCmd.Parameters.AddWithValue("@sn", obj.sn);
                                        insertCmd.Parameters.AddWithValue("@sid", obj.sid);
                                        insertCmd.Parameters.AddWithValue("@m1", obj.m1);
                                        insertCmd.Parameters.AddWithValue("@m2", obj.m2);
                                        insertCmd.Parameters.AddWithValue("@m3", obj.m3);
                                        insertCmd.Parameters.AddWithValue("@m4", obj.m4);
                                        insertCmd.Parameters.AddWithValue("@m5", obj.m5);
                                        insertCmd.Parameters.AddWithValue("@m6", obj.m6);
                                        insertCmd.Parameters.AddWithValue("@T1", obj.T1);
                                        insertCmd.Parameters.AddWithValue("@T2", obj.T2);
                                        insertCmd.Parameters.AddWithValue("@T3", obj.T3);
                                        insertCmd.Parameters.AddWithValue("@T4", obj.T4);
                                        insertCmd.Parameters.AddWithValue("@T5", obj.T5);
                                        insertCmd.Parameters.AddWithValue("@T6", obj.T6);
                                        insertCmd.Parameters.AddWithValue("@A", obj.A);
                                        insertCmd.Parameters.AddWithValue("@B", obj.B);
                                        insertCmd.Parameters.AddWithValue("@F", obj.F);
                                        insertCmd.Parameters.AddWithValue("@flowSec", obj.flowSec);
                                        insertCmd.Parameters.AddWithValue("@L", obj.L);
                                        insertCmd.Parameters.AddWithValue("@br1", obj.br1);
                                        insertCmd.Parameters.AddWithValue("@br2", obj.br2);
                                        insertCmd.Parameters.AddWithValue("@speed", obj.speed);
                                        insertCmd.Parameters.AddWithValue("@time", obj.time);
                                        insertCmd.Parameters.AddWithValue("@isSend", obj.isSend);
                                        insertCmd.Parameters.AddWithValue("@seconds", obj.Seconds);
                                        insertCmd.ExecuteNonQueryAsync();
                                        countEXE = count;


                                    }
                                    else
                                    {
                                        countEXE = count;

                                    }
                                }
                                else { countEXE = 5; }
                            }
                        }
                            countp++;
                            progressBar.Value = countp;
                        }

                        conn.CloseAsync();
                    if (countEXE == 0)
                    {
                        MessageBox.Show("تم إضافة البيانات بنجاح", "تقرير", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (countEXE == 1)
                    {
                        MessageBox.Show("تم تحديث البيانات بنجاح", "تقرير", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (countEXE == 5)
                    {
                        MessageBox.Show("خطأ لم يتم العثور على القاطرة", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                        {

                            MessageBox.Show("خطأ لم تتم إضاقة البيانات", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);


                        }
                        progressBar.Visibility = Visibility.Collapsed;

                    
                }
            }
            catch (Exception exe) { MessageBox.Show(exe.Message); }