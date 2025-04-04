using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SampleWindowsFormsApp
{
    public partial class Form1: Form
    {
        public Form1()
        {

            InitializeComponent();

            SetupDataGridView();

            // 初期表示時に1行追加
            dataGridView1.Rows.Add();

            // DataGridViewにフォーカスを設定
            dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells["商品コード"];
            dataGridView1.BeginEdit(true);

            // フォームのサイズを自動調整
            this.Load += (s, e) =>
            {
                int width = dataGridView1.PreferredSize.Width;
                int height = dataGridView1.PreferredSize.Height;
                this.ClientSize = new System.Drawing.Size(width, height);
            };
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var savedLocation = Properties.Settings.Default.FormLocation;

            if (savedLocation != Point.Empty && IsLocationVisible(savedLocation))
            {
                this.Location = savedLocation;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.FormLocation = this.Location;
            Properties.Settings.Default.Save();
        }

        private bool IsLocationVisible(Point location)
        {
            foreach (var screen in Screen.AllScreens)
            {
                if (screen.WorkingArea.Contains(location))
                    return true;
            }
            return false;
        }

        // DataGridView初期設定
        private void SetupDataGridView()
        {
            dataGridView1.ColumnCount = 6;
            dataGridView1.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;


            dataGridView1.Columns[0].Name = "商品コード";
            dataGridView1.Columns[1].Name = "入数";
            dataGridView1.Columns[2].Name = "入数単位";
            dataGridView1.Columns[3].Name = "売上数";
            dataGridView1.Columns[4].Name = "単位";
            dataGridView1.Columns[5].Name = "総数量";

            // すべての列を編集可能に設定
            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                col.ReadOnly = false;
            }

            // 入数単位用のComboBox列を設定
            var unitColumn = new DataGridViewComboBoxColumn();
            unitColumn.Name = "入数単位";
            unitColumn.Items.AddRange("個", "箱", "セット");
            dataGridView1.Columns.RemoveAt(2);
            dataGridView1.Columns.Insert(2, unitColumn);

            // 削除ボタンを追加
            //var deleteButton = new DataGridViewButtonColumn();
            //deleteButton.Name = "削除";
            //deleteButton.Text = "削除";
            //deleteButton.UseColumnTextForButtonValue = true;
            //dataGridView1.Columns.Add(deleteButton);

            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AllowUserToDeleteRows = false;
        }

        // 行追加ボタン
        private void buttonAddRow_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Add();
        }

        // 送信ボタン
        private void buttonSubmit_Click(object sender, EventArgs e)
        {
            MessageBox.Show("データが送信されました。", "送信完了");
        }

        // DataGridViewでボタンがクリックされた時のイベント（行の削除処理）
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "削除")
            {
                if (e.RowIndex >= 0)
                {
                    dataGridView1.Rows.RemoveAt(e.RowIndex);
                }
            }
        }

        // 総数量の自動計算
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 &&
                (dataGridView1.Columns[e.ColumnIndex].Name == "入数" ||
                 dataGridView1.Columns[e.ColumnIndex].Name == "売上数"))
            {
                var row = dataGridView1.Rows[e.RowIndex];

                if (int.TryParse(Convert.ToString(row.Cells["入数"].Value), out int packSize) &&
                    int.TryParse(Convert.ToString(row.Cells["売上数"].Value), out int saleQty))
                {
                    row.Cells["総数量"].Value = packSize * saleQty;
                }
                else
                {
                    row.Cells["総数量"].Value = null;
                }
            }
        }

        // DataGridViewのセル入力後に即時変更を反映させるための処理
        private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridView1.IsCurrentCellDirty)
                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }


        // フォームの表示時に実行されるイベントハンドラ
        private void Form1_Shown(object sender, EventArgs e)
        {
            // フォームが完全表示されたタイミングでサイズ調整
            this.Size = new System.Drawing.Size(750, 700);

            // ポップアップ表示
            MessageBox.Show(
                "日頃よりご利用いただきありがとうございます。システムの更なる向上を目指し、 以下の日程でバージョンアップに伴うシステムメンテナンスを実施いたします。\r\n\r\n" +
                "メンテナンス日時: 2025年6月1日（日） 午前0:00 〜 午前6:00\r\n\r\n" +
                "この間、システムをご利用いただけませんのでご注意ください。お客様にはご迷惑をお掛けしますが、 ご理解とご協力を賜りますようお願い申し上げます。\r\n\r\n",
                "バージョンアップのお知らせ",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();  // アプリケーションを終了する
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
