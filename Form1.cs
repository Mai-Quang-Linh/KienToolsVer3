using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Threading;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using ImageMagick;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;


//google-id      685952129405-ri090cqoo6vmo00dn40sg853obpj53hb.apps.googleusercontent.com
//google-secret  6NPOFhRH60qJrucDeEFjssmx
namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private ListViewColumnSorter lvwColumnSorter;
        private string dir;
        private List<bool> checkedprev=new List<bool>();
        private HttpClient client = new HttpClient();
        private readonly string RefreshToken = "1650bb637d20407a287d28ad3d0b5bae934cbacd";
        private readonly string ClientSecret = "584e535a992d8ad8724fd9310fbe761dfc709b02";
        private readonly string ClientID = "a38c9c31d407563";
        private readonly string Rapid_API_key = "75b58d4919msh52bcb5a0139bbdbp11f670jsn459876a2e373";
        private readonly string AWSkey = "992812574221";
        private string accessToken;
        private int workerThreads;
        private int portThreads;
        private int maxThreads;
        private Task initClient;
        private bool client_ready = false;
        private bool checkall;
        private int maxFileLength = 258;
        private int maxNameLength = 200;
        private Bitmap bg = new Bitmap("sample.bmp");
        private Bitmap bg2 = new Bitmap("sample2.bmp");
        private Dictionary<int,Object> TaskMap = new Dictionary<int, Object>();
        private IList<IList<Object>> RenameList1;
        private List<List<string>> RenameList= new List<List<string>>();

        long check=0;
        public Form1()
        {
            initRenameList();
            log.initLog(null);
            InitializeComponent();
            lvwColumnSorter = new ListViewColumnSorter();
            this.listView1.ListViewItemSorter = lvwColumnSorter;
            //innit imgur
            initClient = getAccessToken();
        }
        private void initRenameList()
        {
            string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
            string ApplicationName = "Google Sheets API .NET Quickstart";
            UserCredential credential;

            using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
                {
                    // The file token.json stores the user's access and refresh tokens, and is created
                    // automatically when the authorization flow completes for the first time.
                    string credPath = "token.json";
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        Scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(credPath, true)).Result;
                    Console.WriteLine("Credential file saved to: " + credPath);
                }

                // Create Google Sheets API service.
                var service = new SheetsService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });

                // Define request parameters.
                String spreadsheetId = "15mJ42P9TpQ4qEnwgIaXd4eFyorI4f7b0ommMMhqfJ5U";
                String range = "A:B";
            SpreadsheetsResource.ValuesResource.GetRequest request =
                service.Spreadsheets.Values.Get(spreadsheetId, range);
            ValueRange response = request.Execute();
            RenameList1 = response.Values;
            for (int i = 0; i < RenameList1.Count; i++) {
                List<string> newitem = new List<string>();
                newitem.Add((string)RenameList1[i][0]);
                if (RenameList1[i].Count == 1){
                    newitem.Add("");
                }
                else
                {
                    newitem.Add((string)RenameList1[i][1]);
                }
                RenameList.Add(newitem);
            }
        }
        private void initlog()
        {
            List<string> newlog = new List<string>();
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                newlog.Add(String.Copy(listView1.Items[i].SubItems[2].Text));
            }
            log.initLog(newlog);
        }
        private string rename(string name,string from,string to)
        {
            for (int i = 0; i <= name.Length - from.Length; i++)
            {
                if (String.Compare(name.Substring(i, from.Length).ToLower(), from) == 0)
                {
                    name = name.Substring(0, i) + to +name.Substring(i+from.Length);
                }
            }
            return name;
        }
        private async Task getAccessToken()
        {
            ThreadPool.GetMaxThreads(out workerThreads,out portThreads);
            if (workerThreads > portThreads)
                maxThreads = portThreads;
            else
                maxThreads = workerThreads;
            maxThreads = maxThreads / 4;
            client.Timeout = Timeout.InfiniteTimeSpan;
            /* imgur API
            var keys = new Dictionary<string, string>
                {
                    { "refresh_token", RefreshToken},
                    { "client_id", ClientID },
                    { "client_secret", ClientSecret },
                    { "grant_type","refresh_token"}
                };
            var keysURL = new FormUrlEncodedContent(keys);
            HttpResponseMessage response = await client.PostAsync("https://api.imgur.com/oauth2/token", keysURL);
            string responseString = await response.Content.ReadAsStringAsync();
            accessToken = responseString.Substring(17, 40);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            client.DefaultRequestHeaders.Add("x-rapidapi-key",Rapid_API_key);*/
        }
        private void updateLog()
        {
            List<string> newlog = new List<string>();
            for (int i=0;i< listView1.Items.Count; i++)
            {
                newlog.Add(String.Copy(listView1.Items[i].SubItems[2].Text));
            }
            log.update(newlog);
        }
        private void disableButton()
        {
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            button7.Enabled = false;
            button8.Enabled = false;
            button9.Enabled = false;
            button10.Enabled = false;
            button11.Enabled = false;
            button12.Enabled = false;
            button13.Enabled = false;
            button14.Enabled = false;
            button15.Enabled = false;
            checkBox1.Enabled = false;
            listView1.Enabled = false;
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            textBox6.Enabled = false;
        }
        private void enableButton()
        {
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;
            button6.Enabled = true;
            button7.Enabled = true;
            button8.Enabled = true;
            button9.Enabled = true;
            button10.Enabled = true;
            button11.Enabled = true;
            button12.Enabled = true;
            button13.Enabled = true;
            button14.Enabled = true;
            button15.Enabled = true;
            checkBox1.Enabled = true;
            listView1.Enabled = true;
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            textBox3.Enabled = true;
            textBox4.Enabled = true;
            textBox5.Enabled = true;
            textBox6.Enabled = true;
        }
        private static Image ToImg( MagickImage imageMagick)
        {
            MemoryStream memStream = new MemoryStream();
            imageMagick.Write(memStream);
            memStream.Position = 0;

            /* Do not dispose the memStream, the bitmap owns it. */
            Image Img = Image.FromStream(memStream);
            return Img;
        }
        private string getFileExtention(string file)
        {
            string res;
            int l = file.Length;
            int i;
            for (i = l - 1; i >= 0; i--)
                if (file[i] == '.') break;
            if (i == -1) return null;
            res = String.Copy(file.Substring(i, l - i));
            return res;
        }
        private string uppercasing(String name)
        {
            string res = String.Copy(name);
            if (Char.IsLower(res[0]) == true)
                res=Char.ToUpper(res[0])+res.Substring(1);
            for (int i = 1; i < name.Length; i++)
                if(res[i-1]==' '&& Char.IsLower(res[i]))
                {
                    res = res.Substring(0, i) + Char.ToUpper(res[i]) + res.Substring(i + 1);
                }
            return res;
        }
        private string getFileName(string file)
        {
            string res;
            int l = file.Length;
            int i,j;
            for (i = l - 1; i >= 0; i--)
                if (file[i] == '.') break;
            if (i == -1) return null;
            for (j = i - 1; j >= 0; j--)
                if (file[j] == '\\') break;
            res = String.Copy(file.Substring(j+1, i - j-1));
            return res;
        }
        private bool isImg(string file)
        {
            string ext = getFileExtention(file);
            if (String.Compare(ext,".jpg")==0||
                String.Compare(ext, ".jpeg") == 0 ||
                String.Compare(ext, ".jpe") == 0 ||
                String.Compare(ext, ".jif") == 0 ||
                String.Compare(ext, ".jfif") == 0 ||
                String.Compare(ext, ".jfi") == 0 ||
                String.Compare(ext, ".png") == 0 ||
                String.Compare(ext, ".gif") == 0 ||
                String.Compare(ext, ".webp") == 0 ||
                String.Compare(ext, ".tiff") == 0 ||
                String.Compare(ext, ".tif") == 0 ||
                String.Compare(ext, ".psd") == 0 ||
                String.Compare(ext, ".raw") == 0 ||
                String.Compare(ext, ".arw") == 0 ||
                String.Compare(ext, ".cr2") == 0 ||
                String.Compare(ext, ".nrw") == 0 ||
                String.Compare(ext, ".k25") == 0 ||
                String.Compare(ext, ".bmp") == 0 ||
                String.Compare(ext, ".dip") == 0 ||
                String.Compare(ext, ".heif") == 0 ||
                String.Compare(ext, ".heic") == 0 ||
                String.Compare(ext, ".ind") == 0 ||
                String.Compare(ext, ".indd") == 0 ||
                String.Compare(ext, ".indt") == 0 ||
                String.Compare(ext, ".jp2") == 0 ||
                String.Compare(ext, ".jpx") == 0 ||
                String.Compare(ext, ".jpm") == 0 ||
                String.Compare(ext, ".mj2") == 0 ||
                String.Compare(ext, ".j2k") == 0 ||
                String.Compare(ext, ".webp") == 0)
                return true;
            return false;
        }
        private void updateImgList(bool newdir)
        {
            if (Directory.Exists(textBox1.Text))
            {
                string []files = Directory.GetFiles(textBox1.Text);
                dir = String.Copy(textBox1.Text);
                int itemnum = 0;
                List<ListViewItem> item = new List<ListViewItem>();
                List<string> imgfiles = new List<string>();
                if (newdir)
                {
                    checkedprev.Clear();
                }
                else
                {
                    for(int i = 0; i < listView1.Items.Count; i++)
                    {
                        checkedprev[Int32.Parse(listView1.Items[i].SubItems[3].Text)] = listView1.Items[i].Checked;
                    }
                }
                listView1.Items.Clear();
                foreach (string file in files)
                    if (isImg(file))
                    {
                        imgfiles.Add(file);
                        item.Add(new ListViewItem(getFileName(file)));
                        item[itemnum].SubItems.Add(getFileExtention(file));
                        item[itemnum].SubItems.Add(file);
                        item[itemnum].SubItems.Add(itemnum.ToString());
                        if (newdir)
                        {
                            item[itemnum].Checked = true;
                            checkedprev.Add(true);
                        }
                        else
                        {
                            item[itemnum].Checked = checkedprev[itemnum];
                        }
                        itemnum++;
                    }
                listView1.Items.AddRange(item.ToArray<ListViewItem>());
                files = imgfiles.ToArray<string>();
                checkBox1.Checked = true;
            }
            else
            {
                MessageBox.Show("Làm gì có folder này, kiểm tra lại đê!!", "Directory not found!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                foreach (ListViewItem item in listView1.Items)
                    item.Checked = true;
            }
            else
            {
                foreach (ListViewItem item in listView1.Items)
                    item.Checked = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            disableButton();
            updateImgList(true);
            initlog();
            enableButton();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            disableButton();
            DialogResult result = folderBrowserDialog1.ShowDialog();

            if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog1.SelectedPath))
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
            }
            updateImgList(true);
            initlog();
            enableButton();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count == 0)
            {
                MessageBox.Show("Làm gì có file nào mà đổi tên...", "Empty folder!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            disableButton();

            if (MessageBox.Show("Bạn có chắc không...!? (ಠ_ಠ)", "Are you sure!?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                enableButton();
                return;
            }
            string inputText = textBox2.Text;
            foreach (ListViewItem item in listView1.CheckedItems)
            {
                string file = item.SubItems[2].Text;
                if (isImg(file))
                {
                    string newname = inputText + getFileName(file);
                    if (String.Compare(newname, getFileName(file)) != 0)
                    {
                        int originalL = newname.Length;
                        int errTime = 0;
                        File.Move(file, dir + "\\" + getFileName(file) + ".tmp");
                        while (File.Exists(dir + "\\" + newname + getFileExtention(file)))
                        {
                            errTime++;
                            newname = newname.Substring(0, originalL);
                            newname = String.Concat(newname, " No.", errTime.ToString());
                        }
                        if (dir.Length+newname.Length + getFileExtention(file).Length > maxFileLength || newname.Length > maxNameLength)
                        {
                            Form2 renameForm = new Form2(newname, getFileExtention(file),dir);
                            renameForm.ShowDialog();
                            newname = renameForm.getRes();
                        }
                        item.SubItems[0].Text = newname;
                        newname = String.Concat(dir, "\\", newname, getFileExtention(file));
                        item.SubItems[2].Text = newname;
                        File.Move(dir + "\\" + getFileName(file) + ".tmp", newname);
                    }
                }
            }
            updateLog();
            enableButton();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count == 0)
            {
                MessageBox.Show("Làm gì có file nào mà đổi tên...", "Empty folder!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            disableButton();

            if (MessageBox.Show("Bạn có chắc không...!? (ಠ_ಠ)", "Are you sure!?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                enableButton();
                return;
            }
            string inputText = textBox2.Text;
            foreach (ListViewItem item in listView1.CheckedItems)
            {
                string file = item.SubItems[2].Text;
                if (isImg(file))
                {
                    string newname = getFileName(file) + inputText;
                    if (String.Compare(newname, getFileName(file)) != 0)
                    {
                        int originalL = newname.Length;
                        int errTime = 0;
                        File.Move(file, dir + "\\" + getFileName(file) + ".tmp");
                        while (File.Exists(dir + "\\" + newname + getFileExtention(file)))
                        {
                            errTime++;
                            newname = newname.Substring(0, originalL);
                            newname = String.Concat(newname, " No.", errTime.ToString());
                        }
                        if (dir.Length+newname.Length + getFileExtention(file).Length > maxFileLength || newname.Length > maxNameLength)
                        {
                            Form2 renameForm = new Form2(newname, getFileExtention(file),dir);
                            renameForm.ShowDialog();
                            newname = renameForm.getRes();
                        }
                        item.SubItems[0].Text = newname;
                        newname = String.Concat(dir, "\\", newname, getFileExtention(file));
                        item.SubItems[2].Text = newname;
                        File.Move(dir + "\\" + getFileName(file) + ".tmp", newname);
                    }
                }
            }
            updateLog();
            enableButton();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count == 0)
            {
                MessageBox.Show("Làm gì có file nào mà đổi tên...", "Empty folder!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string inputText1 = textBox3.Text;
            string inputText2 = textBox4.Text;
            if (String.Compare(inputText1, "") == 0)
                return;
            disableButton();

            if (MessageBox.Show("Bạn có chắc không...!? (ಠ_ಠ)", "Are you sure!?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                enableButton();
                return;
            }
            foreach (ListViewItem item in listView1.CheckedItems)
            {
                string file = item.SubItems[2].Text;
                if (isImg(file))
                {
                    string newname = getFileName(file).Replace(inputText1, inputText2);
                    if (String.Compare(newname, getFileName(file)) != 0)
                    {
                        int originalL = newname.Length;
                        int errTime = 0;
                        File.Move(file, dir + "\\" + getFileName(file) + ".tmp");
                        while (File.Exists(dir + "\\" + newname + getFileExtention(file)))
                        {
                            errTime++;
                            newname = newname.Substring(0, originalL);
                            newname = String.Concat(newname, " No.", errTime.ToString());
                        }
                        if (dir.Length+newname.Length + getFileExtention(file).Length > maxFileLength || newname.Length > maxNameLength)
                        {
                            Form2 renameForm = new Form2(newname, getFileExtention(file),dir);
                            renameForm.ShowDialog();
                            newname = renameForm.getRes();
                        }
                        item.SubItems[0].Text = newname;
                        newname = String.Concat(dir, "\\", newname, getFileExtention(file));
                        item.SubItems[2].Text = newname;
                        File.Move(dir + "\\" + getFileName(file) + ".tmp", newname);
                    }
                }
            }
            updateLog();
            enableButton();
        }
        private void button6_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count == 0)
            {
                MessageBox.Show("Làm gì có file nào mà đổi tên...", "Empty folder!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            disableButton();

            if (MessageBox.Show("Bạn có chắc không...!? (ಠ_ಠ)", "Are you sure!?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                enableButton();
                return;
            }
            foreach (ListViewItem item in listView1.CheckedItems)
            {
                string file = item.SubItems[2].Text;
                if (isImg(file))
                {
                    string newname = uppercasing(getFileName(file));
                    if (String.Compare(newname, getFileName(file)) != 0)
                    {
                        int originalL = newname.Length;
                        int errTime = 0;
                        File.Move(file, dir + "\\" + getFileName(file) + ".tmp");
                        while (File.Exists(dir + "\\" + newname + getFileExtention(file)))
                        {
                            errTime++;
                            newname = newname.Substring(0, originalL);
                            newname = String.Concat(newname, " No.", errTime.ToString());
                        }
                        if (dir.Length + newname.Length + getFileExtention(file).Length > maxFileLength || newname.Length > maxNameLength)
                        {
                            Form2 renameForm = new Form2(newname, getFileExtention(file), dir);
                            renameForm.ShowDialog();
                            newname = renameForm.getRes();
                        }
                        item.SubItems[0].Text = newname;
                        newname = String.Concat(dir, "\\", newname, getFileExtention(file));
                        item.SubItems[2].Text = newname;
                        File.Move(dir + "\\" + getFileName(file) + ".tmp", newname);
                    }
                }
            }
            updateLog();
            enableButton();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count == 0)
            {
                MessageBox.Show("Làm gì có file nào mà đổi tên...", "Empty folder!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            disableButton();

            if (MessageBox.Show("Bạn có chắc không...!? (ಠ_ಠ)", "Are you sure!?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                enableButton();
                return;
            }
            foreach (ListViewItem item in listView1.CheckedItems)
            {
                string file = item.SubItems[2].Text;
                if (isImg(file))
                {
                    string newname = getFileName(file).ToLower();
                    if (String.Compare(newname, getFileName(file)) != 0)
                    {
                        int originalL = newname.Length;
                        int errTime = 0;
                        File.Move(file, dir + "\\" + getFileName(file) + ".tmp");
                        while (File.Exists(dir + "\\" + newname + getFileExtention(file)))
                        {
                            errTime++;
                            newname = newname.Substring(0, originalL);
                            newname = String.Concat(newname, " No.", errTime.ToString());
                        }
                        if (dir.Length + newname.Length + getFileExtention(file).Length > maxFileLength || newname.Length > maxNameLength)
                        {
                            Form2 renameForm = new Form2(newname, getFileExtention(file), dir);
                            renameForm.ShowDialog();
                            newname = renameForm.getRes();
                        }
                        item.SubItems[0].Text = newname;
                        newname = String.Concat(dir, "\\", newname, getFileExtention(file));
                        item.SubItems[2].Text = newname;
                        File.Move(dir + "\\" + getFileName(file) + ".tmp", newname);
                    }
                }
            }
            updateLog();
            enableButton();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count == 0)
            {
                MessageBox.Show("Làm gì có file nào mà đổi tên...", "Empty folder!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            disableButton();

            if (MessageBox.Show("Bạn có chắc không...!? (ಠ_ಠ)", "Are you sure!?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                enableButton();
                return;
            }
            foreach (ListViewItem item in listView1.CheckedItems)
            {
                string file = item.SubItems[2].Text;
                if (isImg(file))
                {
                    string newname = getFileName(file).ToUpper();
                    if (String.Compare(newname, getFileName(file)) != 0)
                    {
                        int originalL = newname.Length;
                        int errTime = 0;
                        File.Move(file, dir + "\\" + getFileName(file) + ".tmp");
                        while (File.Exists(dir + "\\" + newname + getFileExtention(file)))
                        {
                            errTime++;
                            newname = newname.Substring(0, originalL);
                            newname = String.Concat(newname, " No.", errTime.ToString());
                        }
                        if (dir.Length+newname.Length + getFileExtention(file).Length > maxFileLength || newname.Length > maxNameLength)
                        {
                            Form2 renameForm = new Form2(newname, getFileExtention(file),dir);
                            renameForm.ShowDialog();
                            newname = renameForm.getRes();
                        }
                        item.SubItems[0].Text = newname;
                        newname = String.Concat(dir, "\\", newname, getFileExtention(file));
                        item.SubItems[2].Text = newname;
                        File.Move(dir + "\\" + getFileName(file) + ".tmp", newname);
                    }
                }
            }
            updateLog();
            enableButton();
        }

        
        private async void button9_Click(object sender, EventArgs e)
        {
            if (String.Compare(textBox5.Text,"")==0)
            {
                MessageBox.Show("Chưa ghi tên file lưu kết quả upload kìa.", "Please enter scv file name!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (listView1.Items.Count == 0)
            {
                MessageBox.Show("Làm gì có file nào mà upload...", "Empty folder!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            disableButton();

            if (MessageBox.Show("Bạn có chắc không...!? (ಠ_ಠ)", "Are you sure!?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                enableButton();
                return;
            }
            label7.Text = "Uploading...";
            progressBar1.Value = 0;
            progressBar1.Visible = true;
            //wait imgur
            await initClient;
            List<string> scvList = new List<string>();
            scvList.Add("Image,Link");
            int progress = 0;
            List<Task<HttpResponseMessage>> PostList = new List<Task<HttpResponseMessage>>();
            foreach (ListViewItem item in listView1.CheckedItems)
            {
                string Imgfile = item.SubItems[2].Text;
                string Imgext = item.SubItems[1].Text;
                if (isImg(Imgfile))
                {
                    string base64String;
                    check = GC.GetTotalMemory(false);

                    using (MagickImage Mimage = new MagickImage(Imgfile))
                    {
                        Mimage.BackgroundColor = MagickColors.White;
                        Mimage.Alpha(AlphaOption.Remove);
                        MagickImage Mimage2 = new MagickImage(Mimage);
                        Mimage2.Format = MagickFormat.Jpeg;
                        base64String = Mimage2.ToBase64();
                    }
                    /*{
                        using (MemoryStream m = new MemoryStream())
                        {
                            image.Save(m, image.RawFormat);
                            byte[] imageBytes = m.ToArray();

                            // Convert byte[] to Base64 String
                            base64String = Convert.ToBase64String(imageBytes);
                        }
                    }*/
                    //ImgurAPI
                    /*
                    var options = new
                    {
                        image = base64String,
                        type = "base64",
                        name = getFileName(Imgfile) + Imgext,
                        title = getFileName(Imgfile)
                    };

                    // Serialize our concrete class into a JSON String
                    var stringPayload = JsonConvert.SerializeObject(options);
                    var formURL = new StringContent(stringPayload, Encoding.UTF8, "application/json");
                    PostList.Add(client.PostAsync("https://imgur-apiv3.p.rapidapi.com/3/image", formURL));
                    TaskMap.Add(PostList[PostList.Count - 1].Id, options);
                    while (PostList.Count == maxThreads || GC.GetTotalMemory(true) > 1000000000 )
                    {
                        Task<HttpResponseMessage> finishedTask = await Task.WhenAny<HttpResponseMessage>(PostList);
                        int TaskID = finishedTask.Id;
                        HttpResponseMessage response = await finishedTask;
                        string resultString = await response.Content.ReadAsStringAsync();
                        if (resultString.IndexOf("\"error\":\"File is over the size limit\",") > -1)
                        {
                            MessageBox.Show("file " + TaskMap[TaskID] + " to quá up ko nổi.", "File is over size limit");
                            progress++;
                        }
                        else if (resultString.IndexOf("API is unreachable, please contact the API provider") > -1)
                        {
                            var Parm = TaskMap[TaskID];
                            var stringPayload2 = JsonConvert.SerializeObject(Parm);
                            var formURL2 = new StringContent(stringPayload2, Encoding.UTF8, "application/json");
                            PostList.Add(client.PostAsync("https://imgur-apiv3.p.rapidapi.com/3/image", formURL2));
                            TaskMap.Add(PostList[PostList.Count - 1].Id, Parm);
                        }
                        else
                        {
                            string file = resultString.Substring(resultString.IndexOf(",\"title\":\"") + 10);
                            string URL = file.Substring(file.IndexOf(",\"link\":\"") + 9);
                            URL = URL.Substring(0, URL.IndexOf("\""));
                            URL = URL.Replace("\\/", "/");
                            file = file.Substring(0, file.IndexOf("\""));
                            file = System.Text.RegularExpressions.Regex.Unescape(file);
                            file = file.Replace("\\u2018", "\'").Replace("\\u2019", "\'");
                            scvList.Add("\"" + file + "\"" + "," + URL);
                            progress++;

                        }
                        progressBar1.Value = (progress * 100) / listView1.CheckedItems.Count;
                        PostList.Remove(finishedTask);
                        TaskMap.Remove(TaskID);
                    }
                }

            }
            while (PostList.Count > 0)
            {
                Task<HttpResponseMessage> finishedTask = await Task.WhenAny<HttpResponseMessage>(PostList);
                int TaskID = finishedTask.Id;
                HttpResponseMessage response = await finishedTask;
                string resultString = await response.Content.ReadAsStringAsync();
                if (resultString.IndexOf("\"error\":\"File is over the size limit\",") > -1)
                {
                    MessageBox.Show("file " + TaskMap[TaskID] + " to quá up ko nổi.", "File is over size limit");
                    progress++;
                }
                else if (resultString.IndexOf("API is unreachable, please contact the API provider") > -1)
                {
                    var Parm = TaskMap[TaskID];
                    var stringPayload2 = JsonConvert.SerializeObject(Parm);
                    var formURL2 = new StringContent(stringPayload2, Encoding.UTF8, "application/json");
                    PostList.Add(client.PostAsync("https://imgur-apiv3.p.rapidapi.com/3/image", formURL2));
                    TaskMap.Add(PostList[PostList.Count - 1].Id, Parm);
                }
                else
                {
                    string file = resultString.Substring(resultString.IndexOf(",\"title\":\"") + 10);
                    string URL = file.Substring(file.IndexOf(",\"link\":\"") + 9);
                    URL = URL.Substring(0, URL.IndexOf("\""));
                    URL = URL.Replace("\\/", "/");
                    file = file.Substring(0, file.IndexOf("\""));
                    file = System.Text.RegularExpressions.Regex.Unescape(file);
                    file = file.Replace("\\u2018", "\'").Replace("\\u2019", "\'");
                    scvList.Add("\"" + file + "\"" + "," + URL);
                    progress++;
                    
                }
                progressBar1.Value = (progress * 100) / listView1.CheckedItems.Count;
                PostList.Remove(finishedTask);
                TaskMap.Remove(TaskID);
             */
                    //AWS
                    var options = new
                    {
                        userid = AWSkey,
                        image = base64String,
                        mime = "image/jpeg",
                        title = getFileName(Imgfile),
                    };

                    // Serialize our concrete class into a JSON String
                    var stringPayload = JsonConvert.SerializeObject(options);
                    var formURL = new StringContent(stringPayload, Encoding.UTF8, "application/json");
                    PostList.Add(client.PostAsync("https://p0jg61c410.execute-api.us-east-1.amazonaws.com/dev/img-upload", formURL));
                    TaskMap.Add(PostList[PostList.Count - 1].Id, options);
                    while (PostList.Count == maxThreads || GC.GetTotalMemory(true) > 1000000000)
                    {
                        Task<HttpResponseMessage> finishedTask = await Task.WhenAny<HttpResponseMessage>(PostList);
                        int TaskID = finishedTask.Id;
                        HttpResponseMessage response = await finishedTask;
                        string resultString = await response.Content.ReadAsStringAsync();
                        if (resultString.IndexOf("server error") > -1)
                        {
                            var Parm = TaskMap[TaskID];
                            var stringPayload2 = JsonConvert.SerializeObject(Parm);
                            var formURL2 = new StringContent(stringPayload2, Encoding.UTF8, "application/json");
                            PostList.Add(client.PostAsync("https://p0jg61c410.execute-api.us-east-1.amazonaws.com/dev/img-upload", formURL2));
                            TaskMap.Add(PostList[PostList.Count - 1].Id, Parm);
                        }
                        else
                        {
                            string file = resultString.Substring(resultString.IndexOf("{\"title\":\"") + 10);
                            string URL = file.Substring(file.IndexOf(",\"link\":\"") + 9);
                            URL = URL.Substring(0, URL.IndexOf("\""));
                            URL = URL.Replace("\\/", "/");
                            file = file.Substring(0, file.IndexOf("\""));
                            file = System.Text.RegularExpressions.Regex.Unescape(file);
                            file = file.Replace("\\u2018", "\'").Replace("\\u2019", "\'");
                            scvList.Add("\"" + file + "\"" + "," + URL);
                            progress++;

                        }
                        progressBar1.Value = (progress * 100) / listView1.CheckedItems.Count;
                        PostList.Remove(finishedTask);
                        TaskMap.Remove(TaskID);
                    }
                }
            }
            while (PostList.Count > 0)
            {
                Task<HttpResponseMessage> finishedTask = await Task.WhenAny<HttpResponseMessage>(PostList);
                int TaskID = finishedTask.Id;
                HttpResponseMessage response = await finishedTask;
                string resultString = await response.Content.ReadAsStringAsync();
                if (resultString.IndexOf("server error") > -1)
                {
                    var Parm = TaskMap[TaskID];
                    var stringPayload2 = JsonConvert.SerializeObject(Parm);
                    var formURL2 = new StringContent(stringPayload2, Encoding.UTF8, "application/json");
                    PostList.Add(client.PostAsync("https://p0jg61c410.execute-api.us-east-1.amazonaws.com/dev/img-upload", formURL2));
                    TaskMap.Add(PostList[PostList.Count - 1].Id, Parm);
                }
                else
                {
                    string file = resultString.Substring(resultString.IndexOf("{\"title\":\"") + 10);
                    string URL = file.Substring(file.IndexOf(",\"link\":\"") + 9);
                    URL = URL.Substring(0, URL.IndexOf("\""));
                    URL = URL.Replace("\\/", "/");
                    file = file.Substring(0, file.IndexOf("\""));
                    file = System.Text.RegularExpressions.Regex.Unescape(file);
                    file = file.Replace("\\u2018", "\'").Replace("\\u2019", "\'");
                    scvList.Add("\"" + file + "\"" + "," + URL);
                    progress++;
                    
                }
                progressBar1.Value = (progress * 100) / listView1.CheckedItems.Count;
                PostList.Remove(finishedTask);
                TaskMap.Remove(TaskID);
            }
            System.IO.File.WriteAllLines(dir + "\\" + textBox5.Text + ".csv", scvList, Encoding.UTF8);
            progressBar1.Visible = false;
            label7.Text = "Finished!!";

            enableButton();
        }
        
        private void button10_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count == 0)
            {
                MessageBox.Show("Làm gì có file nào chỉnh ảnh...", "Empty folder!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            disableButton();

            if (MessageBox.Show("Bạn có chắc không...!? (ಠ_ಠ)", "Are you sure!?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                enableButton();
                return;
            }
            int progress = 0;
            label7.Text = "Changing Img...";
            progressBar1.Value = 0;
            progressBar1.Visible = true;
            foreach (ListViewItem item in listView1.CheckedItems)
            {
                Thread.Sleep(100);
                string Imgfile = item.SubItems[2].Text;
                string Imgext = item.SubItems[1].Text;
                if (isImg(Imgfile))
                {
                    Bitmap bitmap;
                    ImageFormat format;
                    float Hres, Vres;
                    using (MagickImage Mimage = new MagickImage(Imgfile))
                    {
                        Mimage.BackgroundColor = MagickColors.White;
                        Mimage.Alpha(AlphaOption.Remove);
                        MagickImage Mimage2 = new MagickImage(Mimage);
                        Mimage2.Format = MagickFormat.Jpeg;
                        using (Image image = ToImg(Mimage2))
                        {
                            bitmap = new Bitmap(image);
                            format = image.RawFormat;
                            Hres = image.HorizontalResolution;
                            Vres = image.VerticalResolution;
                        }

                    }
                    /*for (int i = 0; i < bitmap.Width; i++)
                        for (int j = 0; j < bitmap.Height; j++)
                        {
                            if (bitmap.GetPixel(i, j).R + bitmap.GetPixel(i, j).B + bitmap.GetPixel(i, j).G <300)
                            {
                                bitmap.SetPixel(i, j, Color.Red);
                            }
                        }
                    bitmap.Save("sample1.bmp", ImageFormat.Bmp);
                    break;*/
                    if (bitmap.Width == bg2.Width && bitmap.Height == bg2.Height)
                    {
                        for (int i = 0; i < bitmap.Width; i++)
                            for (int j = 0; j < bitmap.Height; j++)
                            {
                                if (bg2.GetPixel(i, j).ToArgb() == System.Drawing.Color.White.ToArgb())
                                    bitmap.SetPixel(i, j, System.Drawing.Color.White);
                            }
                    }
                    if (bitmap.Width == bg.Width && bitmap.Height == bg.Height)
                    {
                        for (int i = 0; i < bitmap.Width; i++)
                            for (int j = 0; j < bitmap.Height; j++)
                            {
                                if (bg.GetPixel(i, j).R < 00)
                                    bitmap.SetPixel(i, j, System.Drawing.Color.White);
                                else
                                {
                                    var color = bitmap.GetPixel(i, j);
                                    int blur = 255 - (bg.GetPixel(i, j).G + bg.GetPixel(i, j).B) / 2;
                                    int red = ((255 - color.R) * blur) / 255;
                                    int green = ((255 - color.G) * blur) / 255;
                                    int blue = ((255 - color.B) * blur) / 255;
                                    red = 255 - red;
                                    green = 255 - green;
                                    blue = 255 - blue;
                                    bitmap.SetPixel(i, j, System.Drawing.Color.FromArgb(red, green, blue));

                                }
                            }
                    }
                    if (String.Compare(Imgext, ".jpg") != 0)
                    {
                        int errTime = 0;
                        string newname = getFileName(Imgfile);
                        int originalL = newname.Length;
                        while (File.Exists(dir + "\\" + newname + ".jpg"))
                        {
                            errTime++;
                            newname = newname.Substring(0, originalL);
                            newname = String.Concat(newname, " No.", errTime.ToString());
                        }
                        if (dir.Length + newname.Length + Imgext.Length > maxFileLength || newname.Length > maxNameLength)
                        {
                            Form2 renameForm = new Form2(newname, Imgext, dir);
                            renameForm.ShowDialog();
                            newname = renameForm.getRes();
                        }
                        item.SubItems[0].Text = newname;
                        bitmap.Save(dir + "\\" + newname + ".jpg", format);
                        File.Delete(Imgfile);
                        item.SubItems[1].Text = ".jpg";
                        item.SubItems[2].Text = dir + "\\" + newname + ".jpg";
                    }
                    else
                    {
                        bitmap.Save(dir + "\\" + getFileName(Imgfile) + Imgext, format);
                    }
                }
                progress++;
                progressBar1.Value = (progress * 100) / listView1.CheckedItems.Count;
            }
            progressBar1.Visible = false;
            label7.Text = "Finished!!";

           
            enableButton();
        }
        private void button11_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count == 0)
            {
                MessageBox.Show("Làm gì có file nào chỉnh ảnh...", "Empty folder!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            disableButton();

            if (MessageBox.Show("Bạn có chắc không...!? (ಠ_ಠ)", "Are you sure!?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                enableButton();
                return;
            }
            label7.Text = "Changing Img...";
            int progress = 0;
            progressBar1.Value = 0;
            progressBar1.Visible = true;
            foreach (ListViewItem item in listView1.CheckedItems)
            {
                string Imgfile = item.SubItems[2].Text;
                string Imgext = item.SubItems[1].Text;

                if (isImg(Imgfile))
                {
                    Bitmap bg = new Bitmap("staple.bmp");
                    Bitmap bitmap;
                    ImageFormat format;
                    float Hres, Vres;
                    using (MagickImage Mimage = new MagickImage(Imgfile))
                    {
                        Mimage.BackgroundColor = MagickColors.White;
                        Mimage.Alpha(AlphaOption.Remove);
                        MagickImage Mimage2 = new MagickImage(Mimage);
                        Mimage2.Format = MagickFormat.Jpeg;
                        using (Image image = ToImg(Mimage2))
                        {
                            bitmap = new Bitmap(image);
                            format = image.RawFormat;
                            Hres = image.HorizontalResolution;
                            Vres = image.VerticalResolution;
                        }

                    }
                    Bitmap destImage;
                    if (Math.Max(bitmap.Width, bitmap.Height) < 1000)
                    {
                        int width, height;
                        if (bitmap.Width > bitmap.Height)
                        {
                            height = (int)Math.Ceiling((bitmap.Height * 1000) / (double)bitmap.Width);
                            width = 1000;
                        }
                        else
                        {
                            width = (int)Math.Ceiling((bitmap.Width * 1000) / (double)bitmap.Height);
                            height = 1000;
                        }
                        var destRect = new Rectangle(0, 0, width, height);
                        destImage = new Bitmap(width, height);
                        destImage.SetResolution(Hres, Vres);

                        using (var graphics = Graphics.FromImage(destImage))
                        {
                            graphics.CompositingMode = CompositingMode.SourceCopy;
                            graphics.CompositingQuality = CompositingQuality.HighQuality;
                            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            graphics.SmoothingMode = SmoothingMode.HighQuality;
                            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                            using (var wrapMode = new ImageAttributes())
                            {
                                wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                                graphics.DrawImage(bitmap, destRect, 0, 0, bitmap.Width, bitmap.Height, GraphicsUnit.Pixel, wrapMode);
                            }
                        }
                    }
                    else
                    {
                        destImage = bitmap;
                    }
                    if (String.Compare(Imgext, ".jpg") != 0)
                    {
                        int errTime = 0;
                        string newname = getFileName(Imgfile);
                        int originalL = newname.Length;
                        while (File.Exists(dir + "\\" + newname + ".jpg"))
                        {
                            errTime++;
                            newname = newname.Substring(0, originalL);
                            newname = String.Concat(newname, " No.", errTime.ToString());
                        }
                        if (dir.Length + newname.Length + Imgext.Length > maxFileLength || newname.Length > maxNameLength)
                        {
                            Form2 renameForm = new Form2(newname, Imgext, dir);
                            renameForm.ShowDialog();
                            newname = renameForm.getRes();
                        }
                        item.SubItems[0].Text = newname;
                        destImage.Save(dir + "\\" + newname + ".jpg", format);
                        File.Delete(Imgfile);
                        item.SubItems[1].Text = ".jpg";
                        item.SubItems[2].Text = dir + "\\" + newname + ".jpg";
                    }
                    else
                    {
                        destImage.Save(dir + "\\" + getFileName(Imgfile) + Imgext, format);
                    }
                }
                progress++;
                progressBar1.Value = (progress * 100) / listView1.CheckedItems.Count;

            }
            progressBar1.Visible = false;
            label7.Text = "Finished!!";

            enableButton();
        }
        private void button12_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count == 0)
            {
                MessageBox.Show("Làm gì có file nào mà đổi tên...", "Empty folder!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            disableButton();

            if (MessageBox.Show("Bạn có chắc không...!? (ಠ_ಠ)", "Are you sure!?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                enableButton();
                return;
            }
            initRenameList();
            string inputText = textBox2.Text;
            foreach (ListViewItem item in listView1.CheckedItems)
            {
                string file = item.SubItems[2].Text;
                if (isImg(file))
                {
                    string newname = getFileName(file);
                    newname = newname + " ";
                    for (int i = 0; i < RenameList.Count; i++)
                    {
                        newname = rename(newname, (string)RenameList[i][0], (string)RenameList[i][1]);
                    }
                    newname = newname.Substring(0, newname.Length - 1);
                    if (String.Compare(newname, getFileName(file)) != 0)
                    {
                        int originalL = newname.Length;
                        int errTime = 0;
                        File.Move(file, dir + "\\" + getFileName(file) + ".tmp");
                        while (File.Exists(dir + "\\" + newname + getFileExtention(file)))
                        {
                            errTime++;
                            newname = newname.Substring(0, originalL);
                            newname = String.Concat(newname, " No.", errTime.ToString());
                        }
                        if (dir.Length + newname.Length + getFileExtention(file).Length > maxFileLength || newname.Length > maxNameLength)
                        {
                            Form2 renameForm = new Form2(newname, getFileExtention(file), dir);
                            renameForm.ShowDialog();
                            newname = renameForm.getRes();
                        }
                        item.SubItems[0].Text = newname;
                        newname = String.Concat(dir, "\\", newname, getFileExtention(file));
                        item.SubItems[2].Text = newname;
                        File.Move(dir + "\\" + getFileName(file) + ".tmp", newname);
                    }
                }
            }
            updateLog();
            enableButton();
        }
        private void button13_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count == 0)
            {
                MessageBox.Show("Làm gì có file nào mà đổi tên...", "Empty folder!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int charCount;
            if (!int.TryParse(textBox6.Text,out charCount))
            {
                MessageBox.Show("Điền số vào nhá", "Inputed data is not a number!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            disableButton();
            if (MessageBox.Show("Bạn có chắc không...!? (ಠ_ಠ)", "Are you sure!?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                enableButton();
                return;
            }
            foreach (ListViewItem item in listView1.CheckedItems)
            {
                string file = item.SubItems[2].Text;
                if (isImg(file))
                {
                    string newname = getFileName(file).Substring(0,getFileName(file).Length-charCount);
                    if (String.Compare(newname, getFileName(file)) != 0)
                    {
                        int originalL = newname.Length;
                        int errTime = 0;
                        File.Move(file, dir + "\\" + getFileName(file) + ".tmp");
                        while (File.Exists(dir + "\\" + newname + getFileExtention(file)))
                        {
                            errTime++;
                            newname = newname.Substring(0, originalL);
                            newname = String.Concat(newname, " No.", errTime.ToString());
                        }
                        if (dir.Length + newname.Length + getFileExtention(file).Length > maxFileLength || newname.Length > maxNameLength)
                        {
                            Form2 renameForm = new Form2(newname, getFileExtention(file), dir);
                            renameForm.ShowDialog();
                            newname = renameForm.getRes();
                        }
                        item.SubItems[0].Text = newname;
                        newname = String.Concat(dir, "\\", newname, getFileExtention(file));
                        item.SubItems[2].Text = newname;
                        File.Move(dir + "\\" + getFileName(file) + ".tmp", newname);
                    }
                }
            }
            updateLog();
            enableButton();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count == 0)
            {
                MessageBox.Show("Làm gì có file nào mà đổi tên...", "Empty folder!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int charCount;
            if (!int.TryParse(textBox6.Text, out charCount))
            {
                MessageBox.Show("Điền số vào nhá", "Inputed data is not a number!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            disableButton();
            if (MessageBox.Show("Bạn có chắc không...!? (ಠ_ಠ)", "Are you sure!?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                enableButton();
                return;
            }
            foreach (ListViewItem item in listView1.CheckedItems)
            {
                string file = item.SubItems[2].Text;
                if (isImg(file))
                {
                    string newname = getFileName(file).Substring(charCount);
                    if (String.Compare(newname, getFileName(file)) != 0)
                    {
                        int originalL = newname.Length;
                        int errTime = 0;
                        File.Move(file, dir + "\\" + getFileName(file) + ".tmp");
                        while (File.Exists(dir + "\\" + newname + getFileExtention(file)))
                        {
                            errTime++;
                            newname = newname.Substring(0, originalL);
                            newname = String.Concat(newname, " No.", errTime.ToString());
                        }
                        if (dir.Length + newname.Length + getFileExtention(file).Length > maxFileLength || newname.Length > maxNameLength)
                        {
                            Form2 renameForm = new Form2(newname, getFileExtention(file), dir);
                            renameForm.ShowDialog();
                            newname = renameForm.getRes();
                        }
                        item.SubItems[0].Text = newname;
                        newname = String.Concat(dir, "\\", newname, getFileExtention(file));
                        item.SubItems[2].Text = newname;
                        File.Move(dir + "\\" + getFileName(file) + ".tmp", newname);
                    }
                }
            }
            updateLog();
            enableButton();
        }
        private void button15_Click(object sender, EventArgs e)
        {
            List<string> prev = log.undo();
            if (prev == null)
            {
                MessageBox.Show("Hết lùi nổi rồi!", "Unable to undo!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            disableButton();

            if (MessageBox.Show("Bạn có chắc không...!? (ಠ_ಠ)", "Are you sure!?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                enableButton();
                return;
            }

            for (int i = listView1.Items.Count - 1; i >= 0; i--)
            {
                string file = listView1.Items[i].SubItems[2].Text;
                File.Move(file, prev[i]);
            }
            updateImgList(true);
            enableButton();
        }
        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (lvwColumnSorter.Order == SortOrder.Ascending)
                {
                    lvwColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    lvwColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            this.listView1.Sort();
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            checkBox1.Checked = !checkBox1.Checked;
        }
        private int ColorDis(System.Drawing.Color a, System.Drawing.Color b)
        {
            return ((a.R-b.R)* (a.R - b.R) + (a.B - b.B)*(a.B - b.B) + (a.G - b.G)*(a.G - b.G) );
        }
    }
}
