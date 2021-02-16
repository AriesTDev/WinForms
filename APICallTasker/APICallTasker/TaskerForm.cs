using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace APICallTasker
{
    public partial class TaskerForm : Form
    {
        //TODO: set baseUri in Config
        private static string baseUri = "https://ebcx7wvx2d.execute-api.us-east-1.amazonaws.com";
        private static HttpClient client = new HttpClient();

        public TaskerForm()
        {
            InitializeComponent();

            client.BaseAddress = new Uri(baseUri);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

        }

        private void TaskerForm_Load(object sender, EventArgs e)
        {
            if (!bgWorker.IsBusy)
                bgWorker.RunWorkerAsync(3000);
        }

        private void BgWorker_DoWorkAsync(object sender, DoWorkEventArgs e)
        {
            var result = GetTask(txtVIN.Text);
            e.Result = (string)result;
        }

        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // First, handle the case where an exception was thrown.
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
            else if (e.Cancelled)
            {
                // CancelAsync was called.
                //DO NOTHING
            }
            else
            {
                // Finally, handle the case where the operation
                // succeeded.
                if(e.Result != null)
                {
                    dgList.Rows.Add(e.Result as string);
                    dgList.Refresh();
                    dgList.PerformLayout();

                    bgWorker.RunWorkerAsync(3000);
                }
            }
        }

        #region Methods
        private string GetTask(string vin)
        {
            var result = client.GetStringAsync($"/default/mhits/tasks?vin={vin}").GetAwaiter().GetResult();
            return result.ToString();
        }
        #endregion

    }
}
